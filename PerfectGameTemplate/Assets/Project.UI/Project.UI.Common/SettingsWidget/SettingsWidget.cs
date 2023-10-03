﻿#nullable enable
namespace Project.UI.Common {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Framework.UI;

    public class SettingsWidget : UIWidgetBase<SettingsWidgetView> {

        // Constructor
        public SettingsWidget() {
            View = CreateView();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnAttach
        public override void OnBeforeAttach() {
        }
        public override void OnAttach() {
        }
        public override void OnDetach() {
        }
        public override void OnAfterDetach() {
        }

        // Helpers
        private SettingsWidgetView CreateView() {
            var view = UIViewFactory.SettingsWidget();
            view.OnCommand( (SettingsWidgetView.PlayerProfileCommand cmd) => {
                this.AttachChild( UIWidgetFactory.PlayerProfileWidget() );
            } );
            view.OnCommand( (SettingsWidgetView.VideoSettingsCommand cmd) => {
                this.AttachChild( UIWidgetFactory.VideoSettingsWidget() );
            } );
            view.OnCommand( (SettingsWidgetView.AudioSettingsCommand cmd) => {
                this.AttachChild( UIWidgetFactory.AudioSettingsWidget() );
            } );
            view.OnCommand( (SettingsWidgetView.BackCommand cmd) => {
                this.DetachSelf();
            } );
            return view;
        }

    }
}
