﻿#nullable enable
namespace Project.UI.MainScreen {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Framework;
    using UnityEngine.Framework.UI;

    public class MainMenuWidget : UIWidgetBase<MainMenuWidgetView> {

        // Globals
        private UIRouter Router { get; }

        // Constructor
        public MainMenuWidget() {
            Router = this.GetDependencyContainer().Resolve<UIRouter>( null );
            View = CreateView();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnAttach
        public override void OnAttach() {
        }
        public override void OnShow() {
        }
        public override void OnHide() {
        }
        public override void OnDetach() {
        }

        // Helpers
        private MainMenuWidgetView CreateView() {
            var view = UIVisualFactory.MainMenuWidget();
            view.OnCommand( (MainMenuWidgetView.CreateGameCommand cmd) => {
                this.AttachChild( UILogicalFactory.CreateGameWidget() );
            } );
            view.OnCommand( (MainMenuWidgetView.JoinGameCommand cmd) => {
                this.AttachChild( UILogicalFactory.JoinGameWidget() );
            } );
            view.OnCommand( (MainMenuWidgetView.SettingsCommand cmd) => {
                this.AttachChild( UILogicalFactory.SettingsWidget() );
            } );
            view.OnCommand( (MainMenuWidgetView.QuitCommand cmd) => {
                var dialog = UILogicalFactory.DialogWidget( "Confirmation", "Are you sure?" ).OnSubmit( "Yes", () => Router.Quit() ).OnCancel( "No", null );
                this.AttachChild( dialog );
            } );
            return view;
        }

    }
}
