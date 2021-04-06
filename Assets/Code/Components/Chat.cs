using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Code.Components
{
    public class Chat : MonoBehaviour
    {
        [Header("Configurations")]
        [SerializeField]
        private float _cdwBetweenMsg;
        [SerializeField]
        private float _cdwBetweenCharacters;
        [TextArea]
        [SerializeField]
        private List<string> _messages;

        [Header("Events")]
        [SerializeField]
        private UnityEvent _onEndChat;

        
        private TMP_Text _text;

        private void Awake()
        {
            _text = this.GetComponent<TMP_Text>();
        }

        private void Start()
        {
            StartCoroutine(StartChat());
        }

        IEnumerator StartChat()
        {
            string currentMessage = "";

            for (int i = 1; i <= _messages.Count; i++)
            {
                foreach (char character in _messages[i-1])
                {
                    currentMessage += character;
                    _text.SetText(currentMessage);
                    yield return new WaitForSecondsRealtime(_cdwBetweenCharacters);
                }

                yield return new WaitForSecondsRealtime(_cdwBetweenMsg);
                currentMessage = "";
                _text.SetText(currentMessage);

                if (i == _messages.Count)
                {
                    _onEndChat.Invoke();
                }
            }
        } 

    }
}
