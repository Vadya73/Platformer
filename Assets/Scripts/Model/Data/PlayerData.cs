﻿using System;
using UnityEngine;

namespace Scripts.Model.Data
{
    [Serializable]
    public class PlayerData
    {
        public InventoryData Inventory => _inventory;
        [SerializeField] private InventoryData _inventory;
        
        public int Health;
        
        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}