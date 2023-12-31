﻿#nullable enable
namespace Project.UI.Common {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Project.App;
    using UnityEngine;
    using UnityEngine.Framework;
    using UnityEngine.Framework.UI;

    public class AudioSettingsWidget : UIWidgetBase<AudioSettingsWidgetView> {

        // Globals
        private Globals.AudioSettings AudioSettings { get; }
        // View
        public override AudioSettingsWidgetView View { get; }

        // Constructor
        public AudioSettingsWidget() {
            AudioSettings = this.GetDependencyContainer().Resolve<Globals.AudioSettings>( null );
            View = CreateView( this, AudioSettings );
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnAttach
        public override void OnBeforeAttach() {
            View.MasterVolume.ValueMinMax = (AudioSettings.MasterVolume, 0, 1);
            View.MusicVolume.ValueMinMax = (AudioSettings.MusicVolume, 0, 1);
            View.SfxVolume.ValueMinMax = (AudioSettings.SfxVolume, 0, 1);
            View.GameVolume.ValueMinMax = (AudioSettings.GameVolume, 0, 1);
        }
        public override void OnAttach() {
        }
        public override void OnDetach() {
        }
        public override void OnAfterDetach() {
            AudioSettings.Load();
        }

        // Helpers
        private static AudioSettingsWidgetView CreateView(AudioSettingsWidget widget, Globals.AudioSettings audioSettings) {
            var view = new AudioSettingsWidgetView( widget );
            view.MasterVolume.OnChange( (i, masterVolume) => {
                audioSettings.MasterVolume = masterVolume;
            } );
            view.MusicVolume.OnChange( (i, musicVolume) => {
                audioSettings.MusicVolume = musicVolume;
            } );
            view.SfxVolume.OnChange( (i, sfxVolume) => {
                audioSettings.SfxVolume = sfxVolume;
            } );
            view.GameVolume.OnChange( (i, gameVolume) => {
                audioSettings.GameVolume = gameVolume;
            } );
            view.Okey.OnClick( i => {
                audioSettings.MasterVolume = view.MasterVolume.Value;
                audioSettings.MusicVolume = view.MusicVolume.Value;
                audioSettings.SfxVolume = view.SfxVolume.Value;
                audioSettings.GameVolume = view.GameVolume.Value;
                audioSettings.Save();
                widget.DetachSelf();
            } );
            view.Back.OnClick( i => {
                widget.DetachSelf();
            } );
            return view;
        }

    }
}
