﻿#nullable enable
namespace Project.UI.MainScreen {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Framework.UI;
    using UnityEngine.UIElements;

    public partial class MainMenuWidgetView {
        public record CreateGameCommand() : UICommand<MainMenuWidgetView>;
        public record JoinGameCommand() : UICommand<MainMenuWidgetView>;
        public record SettingsCommand() : UICommand<MainMenuWidgetView>;
        public record QuitCommand() : UICommand<MainMenuWidgetView>;
    }
    public partial class MainMenuWidgetView : UIWidgetViewBase {

        // View
        private readonly VisualElement visualElement;
        private readonly Label title;
        private readonly Button createGame;
        private readonly Button joinGame;
        private readonly Button settings;
        private readonly Button quit;
        // View
        public override VisualElement VisualElement => visualElement;
        public LabelWrapper Title => title.Wrap();
        public ButtonWrapper CreateGame => createGame.Wrap();
        public ButtonWrapper JoinGame => joinGame.Wrap();
        public ButtonWrapper Settings => settings.Wrap();
        public ButtonWrapper Quit => quit.Wrap();

        // Constructor
        public MainMenuWidgetView(MainMenuWidget widget) : base( widget ) {
            visualElement = CreateVisualElement( out title, out createGame, out joinGame, out settings, out quit );
            // View
            visualElement.OnAttachToPanel( evt => {
            } );
            createGame.OnClick( evt => {
                new CreateGameCommand().Execute( this );
            } );
            joinGame.OnClick( evt => {
                new JoinGameCommand().Execute( this );
            } );
            settings.OnClick( evt => {
                new SettingsCommand().Execute( this );
            } );
            quit.OnClick( evt => {
                new QuitCommand().Execute( this );
            } );
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers
        private static View CreateVisualElement(out Label title, out Button createGame, out Button joinGame, out Button settings, out Button quit) {
            return UIFactory.LeftWidget( "main-menu-widget-view" ).Children(
                UIFactory.Card().Children(
                    UIFactory.Header().Children(
                        title = UIFactory.Label( "Main Menu" ).Name( "main-menu" )
                    ),
                    UIFactory.Content().Children(
                        createGame = UIFactory.Button( "Create Game" ).Name( "create-game" ),
                        joinGame = UIFactory.Button( "Join Game" ).Name( "join-game" ),
                        settings = UIFactory.Button( "Settings" ).Name( "settings" ),
                        quit = UIFactory.Button( "Quit" ).Name( "quit" )
                    )
                )
            );
        }

    }
}
