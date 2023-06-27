using System.Collections;
using Scripts.ColliderBased;
using Scripts.Components;
using Scripts.Components.Health;
using Scripts.Model;
using Scripts.Model.Data;
using Scripts.Utils;
using UnityEditor.Animations;
using UnityEngine;

namespace Scripts.Creatures.Hero
{
    public class Hero : Creature, ICanAddInventory
    {
        [Header("Additional params")]
        [SerializeField] private float _slamDownVelocity;

        [SerializeField] private ProbabilityDropComponent _hitDrop;
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disArmed;
        [SerializeField] private ColliderCheck _wallCheck;
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private Cooldown _throwCooldown;

        [Header("Super Throw")] 
        [SerializeField] private int _superThrowParticles;
        [SerializeField] private float _superThrowDelay;
        [SerializeField] private Cooldown _superThrowCooldown;

        private bool _superThrow;
        private bool _allowDoubleJump;
        private bool _isOnWall;
        private float _defaultGravityScale;
        
        private GameSession _session;
        private HealthComponent _health;

        private static readonly int IsThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");
        
        private int CoinsCount => _session.Data.Inventory.Count("Coin"); 
        private int SwordCount => _session.Data.Inventory.Count("Sword");

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _health = GetComponent<HealthComponent>();
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            
            _health.SetHealth(_session.Data.Health);
            UpdateHeroWeapon();
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();

            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            if (_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
            Animator.SetBool(IsOnWall, _isOnWall);
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (CoinsCount > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(CoinsCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);

            _hitDrop.SetCount(numCoinsToDispose);
            _hitDrop.CalculateDrop();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        protected override float CalculateYVelocity()
        {
            //var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded || _isOnWall)
            {
                _allowDoubleJump = true;
            }

            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity )
        {
            if (!IsGrounded && _allowDoubleJump && !_isOnWall)
            {
                _allowDoubleJump = false;
                DoJumpVfx();
                return _jumpForce;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _particles.Spawn("SlamDown");
                }
            }
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id,value);
        }
        
        public override void Attack()
        {
            if (SwordCount <= 0) return;
            
            base.Attack();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disArmed;
        }

        public void OnDoThrow()
        {
            if (_superThrow)
            {
                var numThrows = Mathf.Min(_superThrowParticles, SwordCount - 1);

                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
                ThrowAndRemoveFromInventory();

            _superThrow = false;
        }

        private IEnumerator DoSuperThrow(int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory();
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }

        private void ThrowAndRemoveFromInventory()
        {
            Sounds.Play("Range");
            _particles.Spawn("Throw");
            _session.Data.Inventory.Remove("Sword", 1);
        }
        
        public void StartThrowing()
        {
            _superThrowCooldown.Reset();
        }

        public void PerformThrowing()
        {
            if (!_throwCooldown.IsReady || SwordCount <= 1) return;

            if (_superThrowCooldown.IsReady) _superThrow = true;
            
            Animator.SetTrigger(IsThrowKey);
            _throwCooldown.Reset();
        }

        public void UsePotion()
        {
            var potionCount = _session.Data.Inventory.Count("HealthPotion");

            if (potionCount > 0)
            {
                _health.ModifyHealth(5);
                _session.Data.Inventory.Remove("HealthPotion",1);
            }
        }
    }
}