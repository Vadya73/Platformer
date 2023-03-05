using Scripts.Components;
using Scripts.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Scripts
{
    public class Hero : MonoBehaviour
    {
        [Header("Character stats")]
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _damageJumpForce;
        [SerializeField] private float _slamDownVelocity;

        [Space] [Header("Ground Check")]
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;
        [SerializeField] private LayerMask _groundLayer;
        // [SerializeField] private LayerCheck _groundCheck; // GroundCheck with collider
        
        [Space] [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private SpawnComponent _footParticles;
        [SerializeField] private SpawnComponent _jumpParticles;
        [SerializeField] private SpawnComponent _slamDownParticles;

        [Space] [Header("Others")]
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private Vector3 _InteractionPositionDelta;
        
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disArmed;

        [SerializeField] private CheckCircleOverlap _attackRange;
        
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private bool _isArmed;

        private int _coins;
        
        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Collider2D[] _interactionResult = new Collider2D[1];
        



        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int IsHitKey = Animator.StringToHash("hit");
        private static readonly int IsAttackKey = Animator.StringToHash("attack");

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _isGrounded = IsGrounded();
        }

        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);


            _animator.SetBool(IsGroundKey, _isGrounded);
            _animator.SetFloat(VerticalVelocityKey, _rigidbody.velocity.y);
            _animator.SetBool(IsRunningKey, _direction.x != 0);
            
            UpdateSpriteDirection();
        }

        public void TakeDamage()
        {
            _animator.SetTrigger(IsHitKey);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpForce);
            if (_coins > 0)
            {
                SpawnParticleCoins();
            }
        }

        private void SpawnParticleCoins()
        {
            var numCoinsToDispose = Mathf.Min(_coins, 5);
            _coins -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();   
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position + _InteractionPositionDelta, 
                    _interactionRadius,
                    _interactionResult, 
                _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded) _allowDoubleJump = true;

            if (isJumpPressing)
            {
                yVelocity = CalculateJumpVelocity(yVelocity);
            } 
            else if (_rigidbody.velocity.y > 0 )
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity )
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpForce;
                _jumpParticles.Spawn();
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpForce;
                _allowDoubleJump = false;
                _jumpParticles.Spawn();
            }

            return yVelocity;
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }
        
        public void SaySomething()
        {
            Debug.Log("Say!");
        }

        private bool IsGrounded()
        {
            // return _groundCheck.isTouchingLayer; // GroundCheck with circle collider

             var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, 
                 _groundCheckRadius,Vector2.down,0,  _groundLayer);
             
             return hit.collider != null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = IsGrounded() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position + _groundCheckPositionDelta,Vector3.forward, _groundCheckRadius);
        }
#endif

        public void SpawnFootDust()
        {
            _footParticles.Spawn();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _slamDownParticles.Spawn();
                }
            }
        }

        public void AddCoin()
        {
            _coins++;
            Debug.Log(_coins);
        }

        public void Attack()
        {
            if (!_isArmed) return;
            
            _animator.SetTrigger(IsAttackKey);
        }

        public void OnAttack()
        {
            var gos = _attackRange.GetObjectsInRange();
            foreach (var go in gos)
            {
                var hp = go.GetComponent<HealthComponent>();
                if (hp != null && go.CompareTag("Enemy"))
                {
                    hp.ModifyHealth(_damage);
                }
            }
        }

        public void ArmHero()
        {
            _isArmed = true;
            _animator.runtimeAnimatorController = _armed;
        }
    }

}