using UnityEngine;

namespace Scripts.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;
        [SerializeField] private PlayerDef _player;

        public InventoryItemsDef Items => _items;
        public PlayerDef Player => _player;

        private static DefsFacade _instaance;
        public static DefsFacade I => _instaance == null ? LoadDefs() : _instaance;

        private static DefsFacade LoadDefs()
        {
            return _instaance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}