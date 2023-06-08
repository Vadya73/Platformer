using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Creatures.Hero1
{
    [RequireComponent(typeof(Hero.Hero))]
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero.Hero _hero;
        
        private void Awake()
        {
            if (_hero == null) {_hero = GetComponent<Hero.Hero>();}
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }
        
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Interact();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Attack();
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _hero.StartThrowing();
            }

            if (context.canceled)
            {
                _hero.PerformThrowing();
            }
        }
    }
}