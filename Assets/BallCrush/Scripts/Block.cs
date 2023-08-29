using System.Collections;
using TMPro;
using UnityEngine;


namespace BallCrush
{
    public class NumberBlock : MonoBehaviour
    {

    }

    public class Block : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _healthText;
        [SerializeField] private LayerMask _ballLayer;

        private Rigidbody2D _rb;
        private float moveSpeed = 5.0f; // Adjust the speed as needed
        private Vector2 targetPosition;
        private bool isMoving = false;

        #region Properties
        public int Health { get; private set; }

        #endregion
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
           
        }

        private void Start()
        {
            GameplayManager.OnStartNextRound += MoveUp;

            Health = 3;
            UpdateHealthText();
        }



        public void MoveUp()
        {
            StartCoroutine(PerformMoveUp());
        }

        private IEnumerator PerformMoveUp()
        {
            float distanceThreshold = 0.001f;
            targetPosition = transform.position + new Vector3(0f, 1.5f, 0f);

            while (Vector3.Distance(transform.position, targetPosition) > distanceThreshold)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPosition;
        }

        public void SetHealth(int health)
        {
            this.Health = health;
        }

        private void TakeDamage(int damage = 1)
        {
            Health -= damage;
            if(Health < 1)
            {
                Destroy(this.gameObject);
            }
            UpdateHealthText();
        }

        private void UpdateHealthText()
        {
            _healthText.text = $"{Health}";
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((_ballLayer.value & (1 << collision.gameObject.layer)) != 0)
            {
                TakeDamage();
            }
        }

        private void OnDestroy()
        {
            GameplayManager.OnStartNextRound -= MoveUp;
        }
    }
}

