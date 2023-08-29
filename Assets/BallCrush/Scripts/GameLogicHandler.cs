using UnityEngine;


namespace BallCrush
{
    public class GameLogicHandler : MonoBehaviour
    {
        public static event System.Action OnFinishRound;
        public static event System.Action OnStartNextRound;
    }
}

