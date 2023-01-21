using System;
using UnityEngine;

namespace Scripts
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositiondelta;
        // [SerializeField] private LayerCheck _groundCheck; //GroundCheck with circle collider
        
        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector3(_direction.x * _speed, _rigidbody.velocity.y);

            var isJumping = _direction.y > 0;
            if (isJumping)
            {
                if (IsGrounded())
                {
                    _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

                }
            } 
            else if (_rigidbody.velocity.y > 0 )
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
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
            // return _groundCheck.isTouchingLayer; //GroundCheck with circle collider

             var hit = Physics2D.CircleCast(transform.position + _groundCheckPositiondelta, 
                 _groundCheckRadius,Vector2.down,0,  _groundLayer);

             return hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + _groundCheckPositiondelta, _groundCheckRadius);
        }
    }

}