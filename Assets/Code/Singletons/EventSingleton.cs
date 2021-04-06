using UnityEngine;
using UnityEngine.Events;

namespace Assets.Code.Singletons
{
    public class EventSingleton : MonoBehaviour
    {

        public static EventSingleton Events => _events;

        private static EventSingleton _events = null;
        public OnUpdateScore OnUpdateScore { get; set; }
        public UnityEvent OnStartGame { get; set; }
        public UnityEvent OnLose { get; set; }
        public UnityEvent OnWin { get; set; }

        private void Awake()
        {
            if (_events is null)
                _events = this;

            OnUpdateScore = new OnUpdateScore();
            OnStartGame = new UnityEvent();
            OnLose = new UnityEvent();
            OnWin = new UnityEvent();
        }
    }

    public class OnUpdateScore : UnityEvent<float> { }
}
