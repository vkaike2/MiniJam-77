using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Code.Components
{
    public class UICooldown : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField]
        private int _cdwToStart;

        [Header("UI")]
        [SerializeField]
        private TMP_Text _labelCdw;
        [SerializeField]
        private AudioSource _adioSourceStart;
        [SerializeField]
        private AudioSource _adioSourceCdw;

        [Space]
        [SerializeField]
        private UnityEvent _onFinishCdw;

        private int _currentCdw;
        public void SetNextNumber()
        {
            if (_currentCdw == 0)
            {
                _onFinishCdw.Invoke();
                return;
           
            }
            _currentCdw--;

            if (_currentCdw == 0)
            {
                _adioSourceStart.Play();
                _labelCdw.text = "Go!";
                return;
            }
            _adioSourceCdw.Play();
            AddNumber(_currentCdw);
        }

        private void Awake()
        {
            _currentCdw = _cdwToStart;
            AddNumber(_currentCdw);
            this.gameObject.SetActive(false);
        }


        private void AddNumber(int number)
        {
            string numberString = number.ToString();
           
            _labelCdw.text = numberString.PadLeft(2, '0');
        }
    }
}
