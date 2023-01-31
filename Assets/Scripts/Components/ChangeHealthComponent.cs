using UnityEngine;

namespace Scripts.Components
{
    public class ChangeHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _valueToChangeHp;

        public void ApplyDamage (GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();

            if (healthComponent != null)
            {
                healthComponent.ApplyHealthChange(_valueToChangeHp);
            }
        }
    }
}