using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts
{
    [RequireComponent(typeof(Hero))]
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        private HeroInputActions _inputActions;

        private void Awake()
        {
            if (_hero == null) {_hero = GetComponent<Hero>();}

            _inputActions = new HeroInputActions();
            _inputActions.Hero.HorizontalMovement.performed += OnHorizontalMovement;
            _inputActions.Hero.HorizontalMovement.canceled += OnHorizontalMovement;
        
            _inputActions.Hero.SaySomething.performed += OnSaySomething;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        public void OnHorizontalMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<float>();
            _hero.SetDirection(direction);
        }

        public void OnVerticalMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<float>();
            _hero.SetDirection(direction);
        }

        public void OnSaySomething(InputAction.CallbackContext context)
        {
            _hero.SaySomething();
        }
    }
}