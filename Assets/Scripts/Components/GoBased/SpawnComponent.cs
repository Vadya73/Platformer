﻿using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace Scripts.Components.GoBased
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _invertXScale;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var instance = SpawnUtils.Spawn(_prefab, _target.position);
            
            var scale = _target.lossyScale;
            scale.x *= _invertXScale ? -1 : 1;
            instance.transform.localScale = scale;
            
            instance.SetActive(true);
        }
    }
}