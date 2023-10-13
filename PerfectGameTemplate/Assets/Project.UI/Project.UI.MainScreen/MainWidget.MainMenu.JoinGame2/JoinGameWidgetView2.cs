﻿#nullable enable
namespace Project.UI.MainScreen {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Framework.UI;
    using UnityEngine.UIElements;

    public partial class JoinGameWidgetView2 {
        public record OkeyCommand() : UICommand<JoinGameWidgetView2>;
        public record BackCommand() : UICommand<JoinGameWidgetView2>;
    }
    public partial class JoinGameWidgetView2 : UIWidgetViewBase {

        // Content
        private Label title = default!;
        private GameView game = default!;
        private PlayerView player = default!;
        private Button okey = default!;
        private Button back = default!;
        // Props
        public TextElementWrapper Title => title.Wrap();
        public GameView Game => game;
        public PlayerView Player => player;
        public TextElementWrapper Okey => okey.Wrap();
        public TextElementWrapper Back => back.Wrap();

        // Constructor
        public JoinGameWidgetView2() {
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
        private static Card GetContent(out Label title, out GameView game, out PlayerView player, out Button okey, out Button back) {
            return UIFactory.Card(
                UIFactory.Header(
                    title = UIFactory.Label( "Join Game", "title" )
                ),
                UIFactory.Content(
                    UIFactory.RowScope(
                        i => i.SetUp( null, "grow-0", "basis-40" ),
                        game = new GameView().SetUp( null, "grow-1", "basis-0" ),
                        player = new PlayerView().SetUp( null, "grow-1", "basis-0" )
                    )
                ),
                UIFactory.Footer(
                    okey = UIFactory.Button( "Ok", "okey" ),
                    back = UIFactory.Button( "Back", "back" )
                )
            );
        }

    }
    public partial class JoinGameWidgetView2 : UIWidgetViewBase {
        public class GameView : UISubViewBase {
            public record GameNameEvent(string GameName) : UIEvent<GameView>;
            public record GameModeEvent(object? GameMode) : UIEvent<GameView>;
            public record GameWorldEvent(object? GameWorld) : UIEvent<GameView>;
            public record IsGamePrivateEvent(bool IsGamePrivate) : UIEvent<GameView>;

            // Content
            private Label title = default!;
            private TextField gameName = default!;
            private DropdownField2 gameMode = default!;
            private DropdownField2 gameWorld = default!;
            private Toggle isGamePrivate = default!;
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
                    title = UIFactory.Label( "Game", "title", "title" ),
                    UIFactory.RowScope(
                        gameName = UIFactory.TextFieldReadOnly( "Name", 100, false, "game-name", "label-width-150px", "grow-1" )
                    ),
                    UIFactory.RowScope(
                        gameMode = UIFactory.DropdownField( "Mode", "game-mode", ".label-width-150px", "grow-1" ),
                        gameWorld = UIFactory.DropdownField( "World", "game-world", ".label-width-150px", "grow-1" ),
                        isGamePrivate = UIFactory.Toggle( "Private", "is-game-private", ".label-width-150px", "grow-0" )
                    )
                );
            }

        }
        public class PlayerView : UISubViewBase {
            public record PlayerNameEvent(string PlayerName) : UIEvent<PlayerView>;
            public record PlayerRoleEvent(object? PlayerRole) : UIEvent<PlayerView>;

            // Content
            private Label title = default!;
            private TextField playerName = default!;
            private DropdownField2 playerRole = default!;
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
                    title = UIFactory.Label( "Player", "title", "title" ),
                    UIFactory.RowScope(
                        playerName = UIFactory.TextFieldReadOnly( "Name", 100, false, "player-name", "label-width-150px", "grow-1" )
                    ),
                    UIFactory.RowScope(
                        playerRole = UIFactory.DropdownField( "Role", "player-role", "label-width-150px", "grow-1" )
                    )
                );
            }

        }
    }
}
