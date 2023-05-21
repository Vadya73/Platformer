using UnityEngine;
using UnityEngine.VFX;

namespace Scripts.Creatures.Weapons
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] private bool _invertX;

        protected int Direction;
        
        protected Rigidbody2D Rigidbody;
        
        protected virtual void Start()
        {
            var mod = _invertX ? -1 : 1;
            Direction = mod * transform.lossyScale.x > 0 ? 1 : -1;
            Rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}