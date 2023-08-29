using UnityEngine;


namespace BallCrush
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance { get; private set; }
        public static event System.Action OnStateChanged;
        public static event System.Action OnPlaying;
        public static event System.Action OnWin;
        public static event System.Action OnGameOver;
        public static event System.Action OnRoundFinished;
        public static event System.Action OnStartNextRound;

        public enum GameState
        {
            PLAYING,
            WAITING,
            STARTNEXTROUND,   
            ROUNDFINISHED,   
            WIN,
            GAMEOVER,
            PAUSE,
            EXIT,
        }


        [Header("Properties")]
        [SerializeField] private GameState _currentState;
       


        #region Properties
        public GameState CurrentState { get => _currentState; }
        #endregion


        #region Init & Events
        private void Awake()
        {
            Instance = this;
    
        }

        private void OnEnable()
        {
            OnStateChanged += SwitchState;
        }

        private void OnDisable()
        {
            OnStateChanged -= SwitchState;
        }

        private void Start()
        {
            ChangeGameState(GameState.PLAYING);
        }
        #endregion



        public void ChangeGameState(GameState state)
        {
            _currentState = state;
            OnStateChanged?.Invoke();
        }


        private void SwitchState()
        {
            switch (_currentState)
            {
                default: break;
                case GameState.PLAYING:

                    OnPlaying?.Invoke();
                    break;
                case GameState.WAITING:
                    

                    break;              
                case GameState.ROUNDFINISHED:

                    OnRoundFinished?.Invoke();
                    ChangeGameState(GameState.STARTNEXTROUND);
                    break;
                case GameState.STARTNEXTROUND:

                    OnStartNextRound?.Invoke();
                    ChangeGameState(GameState.PLAYING);
                    break;
                case GameState.WIN:


                    OnWin?.Invoke();
                    break;
                case GameState.GAMEOVER:
                

                    OnGameOver?.Invoke();
                    break;
                case GameState.PAUSE:
               
                    break;
                case GameState.EXIT:
         
                    break;
            }
        }
    }
}

