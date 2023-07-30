using System;
using Scripts.Model;
using Scripts.Model.Definitions;
using Scripts.UI.Widgets;
using UnityEngine;

namespace Scripts.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healtBar;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Health.OnChanged += OnHealthChanged;
            
            OnHealthChanged(_session.Data.Health.Value, 0);
        }

        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = DefsFacade.I.Player.MaxHealth;
            var value = (float) newValue / maxHealth;
            
            _healtBar.SetProgress(value);
        }

        private void OnDestroy()
        {
            _session.Data.Health.OnChanged -= OnHealthChanged;
        }
    }
}