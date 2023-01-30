using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts
{
    [RequireComponent(typeof(Hero))]
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;
        
        private void Awake()
        {
            if (_hero == null) {_hero = GetComponent<Hero>();}
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }
        
        public void OnSaySomething(InputAction.CallbackContext context)
        {
            _hero.SaySomething();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Interact();
            }
        }
    }
}