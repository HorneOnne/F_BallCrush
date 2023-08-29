using System.Collections;
using UnityEngine;


namespace BallCrush
{
    public class BaseBlock : MonoBehaviour
    {
        protected Rigidbody2D rb;
        [SerializeField] protected LayerMask ballLayer;
        private Vector2 targetPosition;
        private float moveSpeed = 5.0f; // Adjust the speed as needed

        private const float _gameoverY = 4.0f;
        #region Properties
        public int Health { get; protected set; }

        #endregion

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            GameplayManager.OnStartNextRound += MoveUp;
        }

        public virtual void SetHealth(int health)
        {
            this.Health = health;
        }


        public virtual void TakeDamage(int damage)
        {

        }

        public void MoveUp()
        {
            StartCoroutine(PerformMoveUp());
        }

        private IEnumerator PerformMoveUp()
        {
            float distanceThreshold = 0.001f;
            targetPosition = transform.position + new Vector3(0f, 1.6f, 0f);

            while (Vector3.Distance(transform.position, targetPosition) > distanceThreshold)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPosition;

            if(transform.position.y > _gameoverY)
            {           
                if(GameplayManager.Instance.CurrentState != GameplayManager.GameState.GAMEOVER)
                {
                    GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.GAMEOVER);
                }
            }    
        }

        protected virtual void OnDestroy()
        {
            GameplayManager.OnStartNextRound -= MoveUp;
        }
    }
}

