using System;
using UnityEngine;

namespace Scripts.Creatures.Weapons
{
    public class Projectile : BaseProjectile
    {
        private Rigidbody2D _rigidbody;
        
        protected override void Start()
        {
            base.Start();
            
            var force = new Vector2(Direction * _speed, 0);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}