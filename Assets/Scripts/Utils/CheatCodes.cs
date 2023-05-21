using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Scripts
{
    public class CheatCodes : MonoBehaviour
    {
        [SerializeField] private float _inputTimeToLive;
        [SerializeField] private CheatItem[] _cheats;
        
        private string _currentInput;
        private float _inputLifeTime;
        private void Awake()
        {
            Keyboard.current.onTextInput += OnTextInput;
        }

        private void OnDestroy()
        {
            Keyboard.current.onTextInput -= OnTextInput;
        }

        private void Update()
        {
            if (_inputLifeTime > 0)
            {
                _currentInput = string.Empty;
            }
            else
            {
                _inputLifeTime -= Time.deltaTime;
            }
        }

        private void OnTextInput(char inputChar)
        {
            _currentInput += inputChar;
            _inputLifeTime = _inputTimeToLive;

            FindAnyCheats();
        }

        private void FindAnyCheats()
        {
            foreach (var cheatItem in _cheats)
            {
                if (_currentInput.Contains(cheatItem.Name))
                {
                    cheatItem.Action?.Invoke();
                    _currentInput = string.Empty;
                }
            }
        }
        [Serializable]
        public class CheatItem
        {
            public string Name;
            public UnityEvent Action;
        }
    }
}