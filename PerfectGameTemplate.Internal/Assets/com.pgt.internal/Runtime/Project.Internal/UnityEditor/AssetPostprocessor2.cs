#if UNITY_EDITOR
#nullable enable
namespace UnityEditor {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    public class AssetPostprocessor2 : AssetPostprocessor {

        // OnGeneratedSlnSolution
        public static string OnGeneratedSlnSolution(string path, string content) {
            return content;
        }

        // OnGeneratedCSProject
        public static string OnGeneratedCSProject(string path, string content) {
            return content;
        }

        // OnPostprocessAllAssets
        public static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFrom) {
            foreach (var imported_ in imported) {
                OnAssetImported( imported_ );
            }
            foreach (var deleted_ in deleted) {
                OnAssetDeleted( deleted_ );
            }
            foreach (var (moved_, movedFrom_) in moved.Zip( movedFrom, (a, b) => (a, b) )) {
                OnAssetMoved( moved_, movedFrom_ );
            }
        }

        // OnAsset
        private static void OnAssetImported(string path) {
            if (Path.GetExtension( path ) == ".pug") {
                CompilePug( path, Path.ChangeExtension( path, ".uxml" ) );
                AssetDatabase.ImportAsset( Path.ChangeExtension( path, ".uxml" ) );
            }
            if (Path.GetExtension( path ) == ".css") {
                CompilePostCss( path, Path.ChangeExtension( path, ".uss" ) );
                AssetDatabase.ImportAsset( Path.ChangeExtension( path, ".uss" ) );
            }
            if (Path.GetExtension( path ) == ".styl") {
                CompileStylus( path, Path.ChangeExtension( path, ".uss" ) );
                AssetDatabase.ImportAsset( Path.ChangeExtension( path, ".uss" ) );
            }
        }
        private static void OnAssetDeleted(string path) {
            if (Path.GetExtension( path ) == ".pug") {
                AssetDatabase.DeleteAsset( Path.ChangeExtension( path, ".uxml" ) );
            }
            if (Path.GetExtension( path ) == ".css") {
                AssetDatabase.DeleteAsset( Path.ChangeExtension( path, ".uss" ) );
            }
            if (Path.GetExtension( path ) == ".styl") {
                AssetDatabase.DeleteAsset( Path.ChangeExtension( path, ".uss" ) );
            }
        }
        private static void OnAssetMoved(string path, string oldPath) {
            if (Path.GetExtension( oldPath ) == ".pug") {
                AssetDatabase.MoveAsset( Path.ChangeExtension( oldPath, ".uxml" ), Path.ChangeExtension( path, ".uxml" ) );
            }
            if (Path.GetExtension( oldPath ) == ".css") {
                AssetDatabase.MoveAsset( Path.ChangeExtension( oldPath, ".uss" ), Path.ChangeExtension( path, ".uss" ) );
            }
            if (Path.GetExtension( oldPath ) == ".styl") {
                AssetDatabase.MoveAsset( Path.ChangeExtension( oldPath, ".uss" ), Path.ChangeExtension( path, ".uss" ) );
            }
        }

        // Compile
        private static void CompilePug(string src, string dist) {
            EvaluateJavaScript( $@"
            const FS = require('fs');
            const Path = require('path');
            const Pug = require( require.resolve('pug', {{ paths: [ Path.join(process.env.APPDATA, '/npm/node_modules') ] }} ) );

            const src = '{src}';
            const dist = '{dist}';
            const source = FS.readFileSync(src, 'utf8');
            const options = {{
                doctype: 'xml',
                pretty: true
            }};

            Pug.render(source, options, onComplete);

            // onCallback
            function onComplete(error, result) {{
                if (error) {{
                    console.error(error);
                    FS.writeFile(dist, '', onError);
                }} else {{
                    FS.writeFile(dist, result.replaceAll('::', '.'), onError);
                }}
            }}
            function onError(error) {{
                if (error) {{
                    console.error(error);
                }}
            }}
            " );
        }
        private static void CompilePostCss(string src, string dist) {
            EvaluateJavaScript( $@"
            const FS = require('fs');
            const Path = require('path');
            const PostCss = require( require.resolve('postcss', {{ paths: [ Path.join(process.env.APPDATA, '/npm/node_modules') ] }} ) );
            const PostCssUse = require( require.resolve('postcss-use', {{ paths: [ Path.join(process.env.APPDATA, '/npm/node_modules') ] }} ) );

            const src = '{src}';
            const dist = '{dist}';
            const source = FS.readFileSync(src, 'utf8');
            const plugins = [new PostCssUse({{ modules: '*' }})];
            const options = {{
                from: src,
                to: dist
            }};

            PostCss(plugins)
                .process(source, options)
                .then(onComplete);

            // onCallback
            function onComplete(result) {{
                for (const warning of result.warnings()) {{
                    console.log(warning.toString());
                }}
                FS.writeFile(dist, result.css, onError);
            }}
            function onError(error) {{
                if (error) {{
                    console.error(error);
                }}
            }}
            " );
        }
        private static void CompileStylus(string src, string dist) {
            EvaluateJavaScript( $@"
            const FS = require('fs');
            const Path = require('path');
            const Stylus = require( require.resolve('stylus', {{ paths: [ Path.join(process.env.APPDATA, '/npm/node_modules') ] }} ) );

            const src = '{src}';
            const dist = '{dist}';
            const source = FS.readFileSync(src, 'utf8');

            Stylus(source)
                .set('filename', Path.basename(src))
                .set('paths', [Path.dirname(src)])
                .define('eval', evalEx)
                .define('raw-eval', rawEvalEx, raw = true)
                .define('get-string', getStringEx, raw = true)
                .render(onComplete);

            // onCallback
            function onComplete(error, result) {{
                if (error) {{
                    console.error(error);
                    FS.writeFile(dist, '', onError);
                }} else {{
                    FS.writeFile(dist, result, onError);
                }}
            }}
            function onError(error) {{
                if (error) {{
                    console.error(error);
                }}
            }}
            
            // extensions
            function evalEx(script, arg, arg2, arg3, arg4, arg5, arg6) {{
                return eval(script.val);
            }}
            function rawEvalEx(script, arg, arg2, arg3, arg4, arg5, arg6) {{
                return eval(script.nodes[0].val);
            }}
            function getStringEx(obj) {{
                if (obj.constructor.name == 'Expression') {{
                    const result = new Stylus.nodes.Expression(obj.isList);
                    for (const node of obj.nodes.map(getStringEx)) {{
                        result.push(node);
                    }}
                    return result;
                }}
                if (obj.constructor.name == 'RGBA') {{
                    return new Stylus.nodes.Literal(obj.name);
                }}
                if (obj.constructor.name == 'Function') {{
                    return new Stylus.nodes.Literal(obj.name);
                }}
                return new Stylus.nodes.Literal(obj.string ?? obj.toString());
            }}
            " );
        }

        // Helpers
        private static void EvaluateJavaScript(string script) {
            script = script.Replace( @"""", @"\""" );
            using var process = System.Diagnostics.Process.Start( new System.Diagnostics.ProcessStartInfo() {
                FileName = "node",
                Arguments = $@"--eval ""{script}""",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            } );
            {
                var outputTask = process.StandardOutput.ReadToEndAsync();
                var errorTask = process.StandardError.ReadToEndAsync();

                if (!string.IsNullOrEmpty( outputTask.Result )) Debug.Log( outputTask.Result ); // todo: it can freeze (when buffer is overloaded) due to sync nature
                if (!string.IsNullOrEmpty( errorTask.Result )) Debug.LogError( errorTask.Result );

                process.StandardInput.Close();
                process.StandardOutput.Close();
                process.StandardError.Close();
            }
            process.WaitForExit( 10_000 );
        }

    }
}
#endif
