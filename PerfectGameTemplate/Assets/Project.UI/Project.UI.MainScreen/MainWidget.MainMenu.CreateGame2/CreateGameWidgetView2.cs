﻿#nullable enable
namespace Project.UI.MainScreen {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Framework.UI;
    using UnityEngine.Scripting;
    using UnityEngine.UIElements;

    public partial class CreateGameWidgetView2 {
        [Preserve]
        public new class UxmlFactory : UxmlFactory<CreateGameWidgetView2, UxmlTraits> { }
        public record OkeyCommand() : UICommand<CreateGameWidgetView2>;
        public record BackCommand() : UICommand<CreateGameWidgetView2>;
    }
    public partial class CreateGameWidgetView2 : UIWidgetViewBase {

        // Content
        private Label title = default!;
        private GameView game = default!;
        private PlayerView player = default!;
        private PlayersView players = default!;
        private Button okey = default!;
        private Button back = default!;
        // Props
        public TextElementWrapper Title => title.Wrap();
        public GameView Game => game;
        public PlayerView Player => player;
        public PlayersView Players => players;
        public TextElementWrapper Okey => okey.Wrap();
        public TextElementWrapper Back => back.Wrap();

        // Constructor
        public CreateGameWidgetView2() {
        }
        public override void Initialize() {
            base.Initialize();
            // Content
            title = this.RequireElement<Label>( "title" );
            game = this.RequireElement<GameView>( null );
            player = this.RequireElement<PlayerView>( null );
            okey = this.RequireElement<Button>( "okey" );
            back = this.RequireElement<Button>( "back" );
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
        public override void Dispose() {
            base.Dispose();
        }

    }
    public partial class CreateGameWidgetView2 : UIWidgetViewBase {
        public class GameView : UISubViewBase {
            [Preserve]
            public new class UxmlFactory : UxmlFactory<GameView, UxmlTraits> { }
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
            }
            public override void Initialize() {
                base.Initialize();
                // Content
                title = this.RequireElement<Label>( null );
                gameName = this.RequireElement<TextField>( "game-name" );
                gameMode = this.RequireElement<DropdownField2>( "game-mode" );
                gameWorld = this.RequireElement<DropdownField2>( "game-world" );
                isGamePrivate = this.RequireElement<Toggle>( "is-game-private" );
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
            public override void Dispose() {
                base.Dispose();
            }

        }
        public class PlayerView : UISubViewBase {
            [Preserve]
            public new class UxmlFactory : UxmlFactory<PlayerView, UxmlTraits> { }
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
            }
            public override void Initialize() {
                base.Initialize();
                // Content
                title = this.RequireElement<Label>( null );
                playerName = this.RequireElement<TextField>( "player-name" );
                playerRole = this.RequireElement<DropdownField2>( "player-role" );
                // OnEvent
                playerName.OnChange( evt => {
                    new PlayerNameEvent( evt.newValue ).Raise( this );
                } );
                playerRole.OnChange( evt => {
                    new PlayerRoleEvent( evt.newValue ).Raise( this );
                } );
            }
            public override void Dispose() {
                base.Dispose();
            }

        }
        public class PlayersView : UISubViewBase {
            [Preserve]
            public new class UxmlFactory : UxmlFactory<PlayersView, UxmlTraits> { }
        }
    }
}
