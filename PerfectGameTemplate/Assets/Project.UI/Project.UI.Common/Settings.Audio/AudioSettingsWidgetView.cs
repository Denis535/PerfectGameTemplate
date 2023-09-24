﻿#nullable enable
namespace Project.UI.Common {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Framework.UI;
    using UnityEngine.Scripting;
    using UnityEngine.UIElements;

    public partial class AudioSettingsWidgetView {
        [Preserve]
        public new class UxmlFactory : UxmlFactory<AudioSettingsWidgetView, UxmlTraits> { }
        public record MasterVolumeEvent(float MasterVolume) : UIEvent<AudioSettingsWidgetView>;
        public record MusicVolumeEvent(float MusicVolume) : UIEvent<AudioSettingsWidgetView>;
        public record SfxVolumeEvent(float SfxVolume) : UIEvent<AudioSettingsWidgetView>;
        public record GameVolumeEvent(float GameVolume) : UIEvent<AudioSettingsWidgetView>;
        public record OkeyCommand() : UICommand<AudioSettingsWidgetView>;
        public record BackCommand() : UICommand<AudioSettingsWidgetView>;
    }
    public partial class AudioSettingsWidgetView : UIWidgetViewBase {

        // Content
        private Label title = default!;
        private Slider masterVolume = default!;
        private Slider musicVolume = default!;
        private Slider sfxVolume = default!;
        private Slider gameVolume = default!;
        private Button okey = default!;
        private Button back = default!;
        // Props
        public TextElementWrapper Title => title.Wrap();
        public SliderFieldWrapper<float> MasterVolume => masterVolume.Wrap();
        public SliderFieldWrapper<float> MusicVolume => musicVolume.Wrap();
        public SliderFieldWrapper<float> SfxVolume => sfxVolume.Wrap();
        public SliderFieldWrapper<float> GameVolume => gameVolume.Wrap();
        public TextElementWrapper Okey => okey.Wrap();
        public TextElementWrapper Back => back.Wrap();

        // Constructor
        public AudioSettingsWidgetView() {
        }
        public override void Initialize() {
            base.Initialize();
            // Content
            title = this.RequireElement<Label>( "title" );
            masterVolume = this.RequireElement<Slider>( "master-volume" );
            musicVolume = this.RequireElement<Slider>( "music-volume" );
            sfxVolume = this.RequireElement<Slider>( "sfx-volume" );
            gameVolume = this.RequireElement<Slider>( "game-volume" );
            okey = this.RequireElement<Button>( "okey" );
            back = this.RequireElement<Button>( "back" );
            // OnEvent
            this.OnAttachToPanel( evt => {
            } );
            masterVolume.OnChange( evt => {
                new MasterVolumeEvent( evt.newValue ).Raise( this );
            } );
            musicVolume.OnChange( evt => {
                new MusicVolumeEvent( evt.newValue ).Raise( this );
            } );
            sfxVolume.OnChange( evt => {
                new SfxVolumeEvent( evt.newValue ).Raise( this );
            } );
            gameVolume.OnChange( evt => {
                new GameVolumeEvent( evt.newValue ).Raise( this );
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
}