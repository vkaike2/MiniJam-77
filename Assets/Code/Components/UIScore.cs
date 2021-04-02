using Assets.Code.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Code.Components
{
    public class UIScore : MonoBehaviour
    {
        [Header("Configuation")]
        [SerializeField]
        private Color _maxColor;
        [SerializeField]
        private Color _goodColor;
        [SerializeField]
        private Color _mediumColor;
        [SerializeField]
        private Color _lowColor;


        [Space]
        [Header("Components")]
        [SerializeField]
        private Image _fillImage;


        private void Awake()
        {
            _fillImage.fillAmount = 0;
        }

        private void Start()
        {

            EventSingleton.Events.OnUpdateScore.AddListener(OnUpdateScore);
        }

        private void OnUpdateScore(float fill)
        {
            if (fill < 0.3)
            {
                _fillImage.color = _lowColor;
            }
            else if (fill < 0.6)
            {
                _fillImage.color = _mediumColor;
            }
            else if (fill < 0.99)
            {
                _fillImage.color = _goodColor;
            }
            else
            {
                _fillImage.color = _maxColor;
            }

            _fillImage.fillAmount = fill;
        }
    }

}
