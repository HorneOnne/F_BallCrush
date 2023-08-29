using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

namespace BallCrush
{
    public class Ball : MonoBehaviour
    {
        private Rigidbody2D _rb;

        [SerializeField] private LayerMask _blockLayer;
        [SerializeField] private LayerMask _specialBlockLayer;
        [SerializeField] private LayerMask _wallLayer;

        private float _gravityStrength = 19.81f;
        private float  _maxVelocity = 18.0f;
        private float _ballForce = 20.0f;
        private float _minY = -8.0f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }


        public void Shoot(Vector2 direction)
        {
            _rb.AddForce(direction * _ballForce, ForceMode2D.Impulse);
        }


        private void FixedUpdate()
        {
            if (GameplayManager.Instance.CurrentState == GameplayManager.GameState.GAMEOVER) return;
        

            ApplyCustomGravity(_gravityStrength);
            ClampVelocity(_maxVelocity);

            if(_rb.position.y < _minY)
            {
                BallSpawner.Instance.ReturnBall();
                Destroy(this.gameObject);
            }
        }

        private void ApplyCustomGravity(float gravityStrength)
        {
            Vector2 customGravityDirection = Vector2.down; // Adjust the gravity direction as needed
            Vector2 gravityForce = customGravityDirection * gravityStrength;

            _rb.AddForce(gravityForce, ForceMode2D.Force);
        }

        private void ClampVelocity(float maxVelocity)
        {
            if (_rb.velocity.magnitude > maxVelocity)
            {
                _rb.velocity = _rb.velocity.normalized * maxVelocity;
            }
        }


        public void MoveToTarget(Vector2 targetPosition, System.Action OnReachTarget)
        {
            StartCoroutine(PerformMoveUp(targetPosition, OnReachTarget));
        }
        private IEnumerator PerformMoveUp(Vector2 targetPosition, System.Action OnReachTarget)
        {
            float distanceThreshold = 0.001f;
            while (Vector3.Distance(transform.position, targetPosition) > distanceThreshold)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, 10f * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPosition;
            OnReachTarget?.Invoke();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((_blockLayer.value & (1 << collision.gameObject.layer)) != 0)
            {
                SoundManager.Instance.PlaySound(SoundType.HitBlock, false);
            }

            if ((_specialBlockLayer.value & (1 << collision.gameObject.layer)) != 0)
            {
                SoundManager.Instance.PlaySound(SoundType.HitKey, false);
            }

            if ((_wallLayer.value & (1 << collision.gameObject.layer)) != 0)
            {
                //SoundManager.Instance.PlaySound(SoundType.HitWall, false);
            }
        }
    }
}

