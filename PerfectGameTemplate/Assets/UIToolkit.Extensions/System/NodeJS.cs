#if UNITY_EDITOR
#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    internal static class NodeJS {

        public static void EvaluateJavaScript(string script) {
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
