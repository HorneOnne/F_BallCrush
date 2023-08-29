using System.Collections;
using UnityEditor.Build.Content;
using UnityEngine;


namespace BallCrush
{
    public class BallSpawner : MonoBehaviour
    {
        public static BallSpawner Instance { get; private set; }
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private Transform _shootPoint;


        private WaitForSeconds _waitForSeconds;


        #region Properties
        public Transform ShootPoint { get => _shootPoint; }
        public int CurrentBallCount { get; private set; }
        public int BallShootedCount { get; private set; }
        public int BallReturnedCount { get; private set; }
        #endregion
        private void Awake()
        {
            Instance = this;
        }


        private void OnEnable()
        {
            InputHanlder.OnShoot += ShootBall;
            GameplayManager.OnStartNextRound += SpawnBallGhost;
        }

        private void OnDisable()
        {
            InputHanlder.OnShoot -= ShootBall;

            GameplayManager.OnStartNextRound -= SpawnBallGhost;
        }

        private void Start()
        {
            CurrentBallCount = 1;
            _waitForSeconds = new WaitForSeconds(0.1f);
        }


 
        public void AddBall()
        {
            CurrentBallCount++;
        }


        private void ShootBall(Vector2 mousePosition)
        {      
            Vector2 direction = (mousePosition - (Vector2)_shootPoint.position).normalized;
            StartCoroutine(PerformShootPoint(direction));

            BallShootedCount = CurrentBallCount;
            BallReturnedCount = 0;
        } 

        private IEnumerator PerformShootPoint(Vector2 direction, System.Action OnFinish = null)
        {
            for (int i = 0; i < CurrentBallCount; i++)
            {
                Ball ball = Instantiate(_ballPrefab, _shootPoint.position, Quaternion.identity);
                ball.Shoot(direction);
                yield return _waitForSeconds;
            }

            OnFinish?.Invoke();
        }


        public void ReturnBall()
        {
            BallReturnedCount++;

            if(BallReturnedCount == BallShootedCount)
            {
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.ROUNDFINISHED);
            }
        }

        private void SpawnBallGhost()
        {
            StartCoroutine(PerformSpawnBallGhost(ShootPoint.position, InputHanlder.Instance.ShowGhost, () =>
            {                
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.PLAYING);
            }));
        }

        public IEnumerator PerformSpawnBallGhost(Vector2 targetPosition, System.Action OnFirst, System.Action OnFinished)
        {
            for (int i = 0; i < CurrentBallCount; i++)
            {
                Vector2 position = (Vector2)ShootPoint.position + new Vector2(Random.Range(-2f, 2f), 2f);
                Ball ball = Instantiate(_ballPrefab, position, Quaternion.identity);
                ball.GetComponent<Rigidbody2D>().isKinematic = true;
                ball.MoveToTarget(targetPosition, () =>
                {
                    Destroy(ball.gameObject);
                });

                if(i == 0)
                {
                    OnFirst?.Invoke();
                }

                yield return _waitForSeconds;
            }

            OnFinished?.Invoke();
        }
    }
}

