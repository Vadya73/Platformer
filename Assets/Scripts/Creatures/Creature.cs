using Scripts.Components;
using UnityEngine;

namespace Scripts.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] protected float _speed;
        [SerializeField] protected float _jumpForce;
        [SerializeField] protected float _damageVelocity;
        [SerializeField] protected int _damage;
        [SerializeField] private bool _invertScale;
        
        [Header("Checkers")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] protected CheckCircleOverlap _attackRange;
        [SerializeField] private LayerCheck _groundCheck ;
        [SerializeField] protected SpawnListComponent _particles;
        
        protected Vector2 Direction;
        protected Rigidbody2D Rigidbody;
        protected Animator Animator;
        protected bool IsGrounded;
        private bool _isJumpig;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int IsHitKey = Animator.StringToHash("hit");
        private static readonly int IsAttackKey = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }
        
        private void FixedUpdate()
        {
            var xVelocity = Direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);


            Animator.SetBool(IsGroundKey, IsGrounded);
            Animator.SetFloat(VerticalVelocityKey, Rigidbody.velocity.y);
            Animator.SetBool(IsRunningKey, Direction.x != 0);
            
            UpdateSpriteDirection(Direction);
        }
        
        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (isJumpPressing)
            {
                _isJumpig = true;
                
                var isFalling = Rigidbody.velocity.y <= 0.001f;
                if (!isFalling) return yVelocity;
                
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (Rigidbody.velocity.y > 0 )
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity )
        {
            if (IsGrounded)
            {
                yVelocity += _jumpForce;
                _particles.Spawn("Jump");
            }

            return yVelocity;
        }
        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }
        
        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }
        
        public virtual void TakeDamage()
        {
            _isJumpig = false;
            Animator.SetTrigger(IsHitKey);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity);

        }
        
        public virtual void Attack()
        {
            Animator.SetTrigger(IsAttackKey);
        }
        
        public void OnDoAttack()
        {
            _attackRange.Check();
        }
    }
}