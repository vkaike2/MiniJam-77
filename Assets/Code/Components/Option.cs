using Assets.Code.Enums;
using Assets.Code.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Code.Components
{
    public class Option : MonoBehaviour
    {


        [Header("Input Buttons")]
        [SerializeField]
        private CustomButton _firstInput;
        [SerializeField]
        private CustomButton _secondInput;
        [SerializeField]
        private CustomButton _thirdInput;
        [SerializeField]
        private CustomButton _fourthInput;

        [Header("Volume")]
        [SerializeField]
        private Slider _sliderVolume;

        public void OnClose()
        {
            EventSingleton.Events.OnCloseOption?.Invoke();
            SceneManager.UnloadSceneAsync(Scenes.Options.ToString());
        }

        public void OnVolumeChanged(float volume)
        {
            OptionSingleton.volume = volume;
            EventSingleton.Events.OnChangeVolume.Invoke(volume);
        }

        private void Awake()
        {
            _firstInput.GetComponentInChildren<TMP_Text>().SetText(OptionSingleton.first_input.GetValueOrDefault().ToString());
            _secondInput.GetComponentInChildren<TMP_Text>().SetText(OptionSingleton.second_input.GetValueOrDefault().ToString());
            _thirdInput.GetComponentInChildren<TMP_Text>().SetText(OptionSingleton.third_input.GetValueOrDefault().ToString());
            _fourthInput.GetComponentInChildren<TMP_Text>().SetText(OptionSingleton.fourth_input.GetValueOrDefault().ToString());

            if (OptionSingleton.volume.HasValue) _sliderVolume.value = OptionSingleton.volume.GetValueOrDefault();
            else _sliderVolume.value = 1;
        }

        private void Start()
        {
            _sliderVolume.onValueChanged.AddListener(OnVolumeChanged);
        }

        private void OnGUI()
        {
            OnChangeInput(_firstInput, Position.First);
            OnChangeInput(_secondInput, Position.Second);
            OnChangeInput(_thirdInput, Position.Third);
            OnChangeInput(_fourthInput, Position.Fourth);
        }

        private void OnChangeInput(CustomButton _input, Position position)
        {
            if (_input.IsSelected)
            {
                Event evt = Event.current;

                if (evt != null &&
                    evt.isKey &&
                    evt.keyCode != KeyCode.None &&
                    evt.keyCode != KeyCode.Escape)
                {
                    _input.GetComponentInChildren<TMP_Text>().SetText(evt.keyCode.ToString());
                    OptionSingleton.SetNoteInput(position, evt.keyCode);
                }
            }
        }
    }
}
