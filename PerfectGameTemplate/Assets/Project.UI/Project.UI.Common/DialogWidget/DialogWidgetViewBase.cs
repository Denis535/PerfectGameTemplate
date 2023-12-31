#nullable enable
namespace Project.UI.Common {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Framework.UI;
    using UnityEngine.UIElements;
    using UnityEngine.UIElements.Experimental;

    public abstract class DialogWidgetViewBase : UIWidgetViewBase, IUIModalWidgetView {

        // View
        public override VisualElement VisualElement { get; }
        public ElementWrapper View { get; }
        public ElementWrapper Card { get; }
        public ElementWrapper Header { get; }
        public ElementWrapper Content { get; }
        public ElementWrapper Footer { get; }
        public LabelWrapper Title { get; }
        public LabelWrapper Message { get; }

        // Constructor
        public DialogWidgetViewBase(UIWidgetBase widget) : base( widget ) {
            VisualElement = CreateVisualElement( out var view, out var card, out var header, out var content, out var footer, out var title, out var message );
            VisualElement.OnAttachToPanel( evt => PlayAppearanceAnimation( VisualElement ) );
            View = view.Wrap();
            Card = card.Wrap();
            Header = header.Wrap().Pipe( i => i.IsDisplayed = false );
            Content = content.Wrap().Pipe( i => i.IsDisplayed = false );
            Footer = footer.Wrap().Pipe( i => i.IsDisplayed = false );
            Title = title.Wrap();
            Message = message.Wrap();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnSubmit
        public void OnSubmit(string text, Action? callback) {
            var button = UIFactory.Button( text ).Name( "submit" );
            button.OnClick( evt => {
                if (button.IsValid()) {
                    callback?.Invoke();
                }
            } );
            Footer.VisualElement.Add( button );
        }
        public void OnCancel(string text, Action? callback) {
            var button = UIFactory.Button( text ).Name( "cancel" );
            button.OnClick( evt => {
                if (button.IsValid()) {
                    callback?.Invoke();
                }
            } );
            Footer.VisualElement.Add( button );
        }

        // Heleprs
        protected abstract View CreateVisualElement(out View view, out Card card, out Header header, out Content content, out Footer footer, out Label title, out Label message);
        private static void PlayAppearanceAnimation(VisualElement view) {
            var animation = ValueAnimation<float>.Create( view, Mathf.LerpUnclamped );
            animation.valueUpdated = (view, t) => {
                var tx = Easing.OutBack( Easing.InPower( t, 2 ), 4 );
                var ty = Easing.OutBack( Easing.OutPower( t, 2 ), 4 );
                var x = Mathf.LerpUnclamped( 0.8f, 1f, tx );
                var y = Mathf.LerpUnclamped( 0.8f, 1f, ty );
                view.transform.scale = new Vector3( x, y, 1 );
            };
            animation.from = 0;
            animation.to = 1;
            animation.durationMs = 500;
            animation.Start();
        }

    }
    // Dialog
    public class DialogWidgetView : DialogWidgetViewBase {

        // Constructor
        public DialogWidgetView(DialogWidget widget) : base( widget ) {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers
        protected override View CreateVisualElement(out View view, out Card card, out Header header, out Content content, out Footer footer, out Label title, out Label message) {
            using (UIFactory.DialogWidget().AsScope( out view )) {
                using (UIFactory.DialogCard().AsScope( out card )) {
                    using (UIFactory.Header().AsScope( out header )) {
                        title = UIFactory.Label( null ).Name( "title" );
                    }
                    using (UIFactory.Content().AsScope( out content )) {
                        using (UIFactory.ColumnGroup().Classes( "gray", "medium", "grow-1", "justify-content-center", "align-items-center" ).AsScope()) {
                            message = UIFactory.Label( null ).Name( "message" );
                        }
                    }
                    using (UIFactory.Footer().AsScope( out footer )) {
                    }
                }
            }
            return view;
        }

    }
    // InfoDialog
    public class InfoDialogWidgetView : DialogWidgetViewBase {

        // Constructor
        public InfoDialogWidgetView(InfoDialogWidget widget) : base( widget ) {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers
        protected override View CreateVisualElement(out View view, out Card card, out Header header, out Content content, out Footer footer, out Label title, out Label message) {
            using (UIFactory.InfoDialogWidget().AsScope( out view )) {
                using (UIFactory.InfoDialogCard().AsScope( out card )) {
                    using (UIFactory.Header().AsScope( out header )) {
                        title = UIFactory.Label( null ).Name( "title" );
                    }
                    using (UIFactory.Content().AsScope( out content )) {
                        using (UIFactory.ColumnGroup().Classes( "gray", "medium", "grow-1", "justify-content-center", "align-items-center" ).AsScope()) {
                            message = UIFactory.Label( null ).Name( "message" );
                        }
                    }
                    using (UIFactory.Footer().AsScope( out footer )) {
                    }
                }
            }
            return view;
        }

    }
    // WarningDialog
    public class WarningDialogWidgetView : DialogWidgetViewBase {

        // Constructor
        public WarningDialogWidgetView(WarningDialogWidget widget) : base( widget ) {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers
        protected override View CreateVisualElement(out View view, out Card card, out Header header, out Content content, out Footer footer, out Label title, out Label message) {
            using (UIFactory.WarningDialogWidget().AsScope( out view )) {
                using (UIFactory.WarningDialogCard().AsScope( out card )) {
                    using (UIFactory.Header().AsScope( out header )) {
                        title = UIFactory.Label( null ).Name( "title" );
                    }
                    using (UIFactory.Content().AsScope( out content )) {
                        using (UIFactory.ColumnGroup().Classes( "gray", "medium", "grow-1", "justify-content-center", "align-items-center" ).AsScope()) {
                            message = UIFactory.Label( null ).Name( "message" );
                        }
                    }
                    using (UIFactory.Footer().AsScope( out footer )) {
                    }
                }
            }
            return view;
        }

    }
    // ErrorDialog
    public class ErrorDialogWidgetView : DialogWidgetViewBase {

        // Constructor
        public ErrorDialogWidgetView(ErrorDialogWidget widget) : base( widget ) {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers
        protected override View CreateVisualElement(out View view, out Card card, out Header header, out Content content, out Footer footer, out Label title, out Label message) {
            using (UIFactory.ErrorDialogWidget().AsScope( out view )) {
                using (UIFactory.ErrorDialogCard().AsScope( out card )) {
                    using (UIFactory.Header().AsScope( out header )) {
                        title = UIFactory.Label( null ).Name( "title" );
                    }
                    using (UIFactory.Content().AsScope( out content )) {
                        using (UIFactory.ColumnGroup().Classes( "gray", "medium", "grow-1", "justify-content-center", "align-items-center" ).AsScope()) {
                            message = UIFactory.Label( null ).Name( "message" );
                        }
                    }
                    using (UIFactory.Footer().AsScope( out footer )) {
                    }
                }
            }
            return view;
        }

    }
}
