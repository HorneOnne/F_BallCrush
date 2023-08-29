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

        [SerializeField] private GameObject _ballGhost;
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

            _ballGhost.transform.position = _shootPoint.position;
        }


        private void OnEnable()
        {
            InputHanlder.OnShoot += ShootBall;
        }

        private void OnDisable()
        {
            InputHanlder.OnShoot -= ShootBall;
        }

        private void Start()
        {
            CurrentBallCount = 3;
            _waitForSeconds = new WaitForSeconds(0.1f);
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
                Debug.Log("AA");
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.ROUNDFINISHED);
            }
        }
    }
}

