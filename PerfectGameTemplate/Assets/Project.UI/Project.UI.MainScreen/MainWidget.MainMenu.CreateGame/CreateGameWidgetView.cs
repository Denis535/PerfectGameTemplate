﻿#nullable enable
namespace Project.UI.MainScreen {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Framework.UI;
    using UnityEngine.UIElements;

    public partial class CreateGameWidgetView {
        public record OkeyCommand() : UICommand<CreateGameWidgetView>;
        public record BackCommand() : UICommand<CreateGameWidgetView>;
    }
    public partial class CreateGameWidgetView : UIWidgetViewBase {

        // Content
        private readonly Label title;
        private readonly GameView game;
        private readonly PlayerView player;
        private readonly Button okey;
        private readonly Button back;
        // Props
        public TextElementWrapper Title => title.Wrap();
        public GameView Game => game;
        public PlayerView Player => player;
        public TextElementWrapper Okey => okey.Wrap();
        public TextElementWrapper Back => back.Wrap();

        // Constructor
        public CreateGameWidgetView() {
            AddToClassList( "large-widget-view" );
            // Content
            Add( GetContent( out title, out game, out player, out okey, out back ) );
            // OnEvent
            this.OnAttachToPanel( evt => {
            } );
            okey.OnClick( evt => {
                new OkeyCommand().Execute( this );
            } );
            back.OnClick( evt => {
                new BackCommand().Execute( this );
            } );
        }
        public override void Initialize() {
            base.Initialize();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers
        private static Card GetContent(out Label title, out GameView gameView, out PlayerView playerView, out Button okey, out Button back) {
            return UIFactory.Card(
                UIFactory.Header(
                    title = UIFactory.Label( "Create Game" ).SetUp( "title" )
                ),
                UIFactory.Content(
                    UIFactory.RowScope(
                        i => i.SetUp( null, "grow-0", "basis-40" ),
                        gameView = new GameView().SetUp( null, "grow-1", "basis-0" ),
                        playerView = new PlayerView().SetUp( null, "grow-1", "basis-0" )
                    ),
                    UIFactory.ColumnGroup(
                        i => i.SetUp( null, "dark5", "medium", "grow-1" ),
                        UIFactory.Label( "Lobby" ).SetUp( "title", "title" )
                    )
                ),
                UIFactory.Footer(
                    okey = UIFactory.Button( "Ok" ).SetUp( "okey" ),
                    back = UIFactory.Button( "Back" ).SetUp( "back" )
                )
            );
        }

    }
    public partial class CreateGameWidgetView : UIWidgetViewBase {
        public class GameView : UISubViewBase {
            public record GameNameEvent(string GameName) : UIEvent<GameView>;
            public record GameModeEvent(object? GameMode) : UIEvent<GameView>;
            public record GameWorldEvent(object? GameWorld) : UIEvent<GameView>;
            public record IsGamePrivateEvent(bool IsGamePrivate) : UIEvent<GameView>;

            // Content
            private readonly Label title;
            private readonly TextField gameName;
            private readonly DropdownField2 gameMode;
            private readonly DropdownField2 gameWorld;
            private readonly Toggle isGamePrivate;
            // Props
            public TextElementWrapper Title => title.Wrap();
            public FieldWrapper<string> GameName => gameName.Wrap();
            public PopupFieldWrapper<object> GameMode => gameMode.Wrap();
            public PopupFieldWrapper<object> GameWorld => gameWorld.Wrap();
            public FieldWrapper<bool> IsGamePrivate => isGamePrivate.Wrap();

            // Constructor
            public GameView() {
                // Content
                Add( GetContent( out title, out gameName, out gameMode, out gameWorld, out isGamePrivate ) );
                // OnEvent
                gameName.OnChange( evt => {
                    new GameNameEvent( evt.newValue ).Raise( this );
                } );
                gameMode.OnChange( evt => {
                    new GameModeEvent( evt.newValue ).Raise( this );
                } );
                gameWorld.OnChange( evt => {
                    new GameWorldEvent( evt.newValue ).Raise( this );
                } );
                isGamePrivate.OnChange( evt => {
                    new IsGamePrivateEvent( evt.newValue ).Raise( this );
                } );
            }
            public override void Initialize() {
                base.Initialize();
            }
            public override void Dispose() {
                base.Dispose();
            }

            // Helpers
            private static ColumnGroup GetContent(out Label title, out TextField gameName, out DropdownField2 gameMode, out DropdownField2 gameWorld, out Toggle isGamePrivate) {
                return UIFactory.ColumnGroup(
                    i => i.SetUp( null, "light5", "medium", "grow-1" ),
                    title = UIFactory.Label( "Game" ).SetUp( "title", "title" ),
                    UIFactory.RowScope(
                        gameName = UIFactory.TextField( "Name", 100, false ).SetUp( "game-name", "label-width-150px", "grow-1" )
                    ),
                    UIFactory.RowScope(
                        gameMode = UIFactory.DropdownField( "Mode" ).SetUp( "game-mode", "label-width-150px", "grow-1" ),
                        gameWorld = UIFactory.DropdownField( "World" ).SetUp( "game-world", "label-width-150px", "grow-1" ),
                        isGamePrivate = UIFactory.Toggle( "Private" ).SetUp( "is-game-private", "label-width-150px", "grow-0" )
                    )
                );
            }

        }
        public class PlayerView : UISubViewBase {
            public record PlayerNameEvent(string PlayerName) : UIEvent<PlayerView>;
            public record PlayerRoleEvent(object? PlayerRole) : UIEvent<PlayerView>;

            // Content
            private readonly Label title;
            private readonly TextField playerName;
            private readonly DropdownField2 playerRole;
            // Props
            public TextElementWrapper Title => title.Wrap();
            public FieldWrapper<string> PlayerName => playerName.Wrap();
            public PopupFieldWrapper<object> PlayerRole => playerRole.Wrap();

            // Constructor
            public PlayerView() {
                // Content
                Add( GetContent( out title, out playerName, out playerRole ) );
                // OnEvent
                playerName.OnChange( evt => {
                    new PlayerNameEvent( evt.newValue ).Raise( this );
                } );
                playerRole.OnChange( evt => {
                    new PlayerRoleEvent( evt.newValue ).Raise( this );
                } );
            }
            public override void Initialize() {
                base.Initialize();
            }
            public override void Dispose() {
                base.Dispose();
            }

            // Helpers
            private static ColumnGroup GetContent(out Label title, out TextField playerName, out DropdownField2 playerRole) {
                return UIFactory.ColumnGroup(
                    i => i.SetUp( null, "light5", "medium", "grow-1" ),
                    title = UIFactory.Label( "Player" ).SetUp( "title", "title" ),
                    UIFactory.RowScope(
                        playerName = UIFactory.TextField( "Name", 100, false ).SetUp( "player-name", "label-width-150px", "grow-1" )
                    ),
                    UIFactory.RowScope(
                        playerRole = UIFactory.DropdownField( "Role" ).SetUp( "player-role", "label-width-150px", "grow-1" )
                    )
                );
            }

        }
    }
}
