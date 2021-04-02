using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Code.Singletons
{
    public class EventSingleton : MonoBehaviour
    {

        public static EventSingleton Events => _events;

        private static EventSingleton _events = null;
        public OnUpdateScore OnUpdateScore { get; set; }

        private void Awake()
        {
            if (_events is null)
                _events = this;

            OnUpdateScore = new OnUpdateScore();
        }
    }

    public class OnUpdateScore : UnityEvent<float> { }
}
