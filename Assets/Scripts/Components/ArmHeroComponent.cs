using UnityEngine;

namespace Scripts.Components
{
    public class ArmHeroComponent : MonoBehaviour
    {
        public void Armhero(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null)
            {
                hero.ArmHero();
            }
        }
    }
}