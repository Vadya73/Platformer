using System;
using Scripts.Model.Data;
using Scripts.Model.Data.Properties;
using UnityEngine;

namespace Scripts.Components.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSettingsComponent : MonoBehaviour
    {
        [SerializeField] private SoundSetting _mode;
        
        private AudioSource _source;
        private FloatPersistentProperty _model;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _model = FindProperty();
            _model.OnChanged += OnSoundSettingsChanged; 
            OnSoundSettingsChanged(_model.Value,_model.Value);
        }

        private void OnSoundSettingsChanged(float newvalue, float oldvalue)
        {
            _source.volume = newvalue;
        }

        private FloatPersistentProperty FindProperty()
        {
            switch (_mode)
            {
                case SoundSetting.Music:
                    return GameSettings.I.Music;
                case SoundSetting.Sfx:
                    return GameSettings.I.Sfx;
            }

            throw new ArgumentException("Undefined mode");
        }

        private void OnDestroy()
        {
            _model.OnChanged -= OnSoundSettingsChanged;
        }
    }
}