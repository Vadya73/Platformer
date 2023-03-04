using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Scripts.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var instantiate =  Instantiate(_prefab, _target.position, quaternion.identity);
            instantiate.transform.localScale = _target.lossyScale;
        }
    }
}