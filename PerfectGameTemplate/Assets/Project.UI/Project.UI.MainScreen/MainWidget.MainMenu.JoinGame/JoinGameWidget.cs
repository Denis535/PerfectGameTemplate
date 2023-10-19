﻿#nullable enable
namespace Project.UI.MainScreen {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Project.App;
    using Project.Entities.GameScene;
    using UnityEngine;
    using UnityEngine.Framework;
    using UnityEngine.Framework.UI;

    public class JoinGameWidget : UIWidgetBase<JoinGameWidgetView> {

        // Globals
        private UIRouter Router { get; }
        private Application2 Application { get; }
        private Globals.PlayerProfile PlayerProfile { get; }
        //private ILobbyService LobbyService { get; }
        // View
        public override JoinGameWidgetView View { get; }
        public JoinGameWidgetView.GameView_ GameView { get; }
        public JoinGameWidgetView.PlayerView_ PlayerView { get; }

        // Constructor
        public JoinGameWidget() {
            Router = this.GetDependencyContainer().Resolve<UIRouter>( null );
            Application = this.GetDependencyContainer().Resolve<Application2>( null );
            PlayerProfile = this.GetDependencyContainer().Resolve<Globals.PlayerProfile>( null );
            //LobbyService = this.GetDependencyContainer().Resolve<ILobbyService>();
            View = CreateView( this );
            GameView = CreateGameView( this );
            PlayerView = CreatePlayerView( this );
            View.GameViewSlot.Add( GameView.VisualElement );
            View.PlayerViewSlot.Add( PlayerView.VisualElement );
        }
        public override void Dispose() {
            GameView.Dispose();
            PlayerView.Dispose();
            base.Dispose();
        }

        // OnAttach
        public override void OnBeforeAttach() {
            GameView.GameName.Value = "Anonymous";
            GameView.GameMode.ValueChoices = (GameMode._1x4, Enum2.GetValues<GameMode>().Cast<object?>().ToArray());
            GameView.GameWorld.ValueChoices = (GameWorld.TestWorld1, Enum2.GetValues<GameWorld>().Cast<object?>().ToArray());
            GameView.IsGamePrivate.Value = true;
            PlayerView.PlayerName.Value = PlayerProfile.PlayerName;
            PlayerView.PlayerRole.ValueChoices = (PlayerRole.Human, Enum2.GetValues<PlayerRole>().Cast<object?>().ToArray());
        }
        public override void OnAttach() {
        }
        public override void OnDetach() {
        }
        public override void OnAfterDetach() {
        }

        // OnDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant) {
            base.OnBeforeDescendantAttach( descendant );
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant) {
            base.OnAfterDescendantAttach( descendant );
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant) {
            base.OnBeforeDescendantDetach( descendant );
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant) {
            if (descendant is JoinGameWidget2 joinGameWidget) {
                GameView.GameName.Value = joinGameWidget.GameView.GameName.Value;
                GameView.GameMode.ValueChoices = joinGameWidget.GameView.GameMode.ValueChoices;
                GameView.GameWorld.ValueChoices = joinGameWidget.GameView.GameWorld.ValueChoices;
                GameView.IsGamePrivate.Value = joinGameWidget.GameView.IsGamePrivate.Value;
                PlayerView.PlayerName.Value = joinGameWidget.PlayerView.PlayerName.Value;
                PlayerView.PlayerRole.Value = joinGameWidget.PlayerView.PlayerRole.Value;
            }
            base.OnAfterDescendantDetach( descendant );
        }

        // Helpers
        private static JoinGameWidgetView CreateView(JoinGameWidget widget) {
            var view = UIViewFactory.JoinGameWidget( widget );
            view.Okey.OnClick( i => {
                widget.AttachChild( UIWidgetFactory.JoinGameWidget2() );
            } );
            view.Back.OnClick( i => {
                widget.DetachSelf();
            } );
            return view;
        }
        private static JoinGameWidgetView.GameView_ CreateGameView(JoinGameWidget widget) {
            var view = new JoinGameWidgetView.GameView_( widget );
            return view;
        }
        private static JoinGameWidgetView.PlayerView_ CreatePlayerView(JoinGameWidget widget) {
            var view = new JoinGameWidgetView.PlayerView_( widget );
            return view;
        }

    }
}
