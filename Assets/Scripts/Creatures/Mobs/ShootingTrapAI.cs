using System;
using Scripts.ColliderBased;
using Scripts.Components.GoBased;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        
        [Header("Melee")]
        [SerializeField] private Cooldown _meleeCoolDown;
        [SerializeField] private CheckCircleOverlap _meleeAttackCheck;
        [SerializeField] private LayerCheck _meleeCanAttack;

        [Header("Range")]
        [SerializeField] private Cooldown _rangeCoolDown;
        [SerializeField] private SpawnComponent _rangeAttack;
        
        private static readonly int MeleeAttackKey = Animator.StringToHash("meleeAttack");
        private static readonly int RangeAttackKey = Animator.StringToHash("rangeAttack");


        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_meleeCanAttack.IsTouchingLayer)
                {
                    if (_meleeCoolDown.IsReady)
                        MeleeAttack();
                    return;
                }

                if (_rangeCoolDown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        public void OnMeleeAttack()
        {
            _meleeAttackCheck.Check();
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
        
        private void MeleeAttack()
        {
            _meleeCoolDown.Reset();
            _animator.SetTrigger(MeleeAttackKey);
        }
        
        private void RangeAttack()
        {
            _rangeCoolDown.Reset();
            _animator.SetTrigger(RangeAttackKey);
        }
    }
}