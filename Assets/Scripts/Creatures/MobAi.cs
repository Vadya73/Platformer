using System;
using System.Collections;
using Scripts.Components;
using UnityEngine;

namespace Scripts.Creatures
{
    public class MobAi : MonoBehaviour
    {
        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _missHeroCooldown = 0.5f;
        
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        private bool _isDead;
        
        private Coroutine _current;
        private GameObject _target;
        private SpawnListComponent _particles;
        private Creature _creature;
        private Animator _animator;

        private Patrol _patrol;
        
        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");


        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;
            
            _target = go;
            StartState(AgroToHero());
        }

        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);
            
            if (_current != null)
                StopCoroutine(_current);
        }
        
        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);
            if (_current != null)
                StopCoroutine(_current);
            _current = StartCoroutine(coroutine);
        }
        
        private IEnumerator AgroToHero()
        {
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            
            StartState(GoToHero());
        }
        
        private IEnumerator GoToHero()
        {
            while (_vision.isTouchingLayer)
            {
                if (_canAttack.isTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();

                }
                yield return null;
            }
            
            _particles.Spawn("MissHero");
            yield return new WaitForSeconds(_missHeroCooldown);
        }

        private IEnumerator Attack()
        {
            while (_canAttack.isTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            
            StartState(GoToHero());
        }

        private void SetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            
            _creature.SetDirection(direction.normalized);
            

        }
    }
}