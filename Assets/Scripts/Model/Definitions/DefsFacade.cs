using UnityEngine;

namespace Scripts.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;

        public InventoryItemsDef Items => _items;

        private static DefsFacade _instaance;
        public static DefsFacade I => _instaance == null ? LoadDefs() : _instaance;

        private static DefsFacade LoadDefs()
        {
            return _instaance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}