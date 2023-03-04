using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDie;

        public void ModifyHealth(int healthDelta)
        {
            var currentHealth = _health;
            _health += healthDelta;
            if (currentHealth > _health)
            {
                _onDamage?.Invoke();
            }
            else
            {
                _onHeal?.Invoke();
            }
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
            Debug.Log($"Current health {_health}");
        }
    }   
}