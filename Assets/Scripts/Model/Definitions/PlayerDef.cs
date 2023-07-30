using UnityEngine;

namespace Scripts.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _invenotySize;
        [SerializeField] private int _maxHealth;
        public int InventorySize => _invenotySize;
        public int MaxHealth => _maxHealth;
    }
}