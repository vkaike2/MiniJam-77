using Assets.Code.Enums;
using Assets.Code.Singletons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Managers
{
    public class OptionManager : MonoBehaviour
    {

        [Header("Default Inputs")]
        [SerializeField] private KeyCode _firstInput;
        [SerializeField] private KeyCode _secondInput;
        [SerializeField] private KeyCode _thirdInput;
        [SerializeField] private KeyCode _fourthInput;

        private AudioSource[] _allAudioSources;

        private GameManager _gameManager;

        private void Awake()
        {
            if (OptionSingleton.first_input == null) OptionSingleton.first_input = _firstInput;
            if (OptionSingleton.second_input == null) OptionSingleton.second_input = _secondInput;
            if (OptionSingleton.third_input == null) OptionSingleton.third_input = _thirdInput;
            if (OptionSingleton.fourth_input == null) OptionSingleton.fourth_input = _fourthInput;
        }

        private void Start()
        {
            _gameManager = GameObject.FindObjectOfType<GameManager>();
            _allAudioSources = GameObject.FindObjectsOfType<AudioSource>();

            EventSingleton.Events.OnCloseOption.AddListener(OnCloseOptions);
            EventSingleton.Events.OnChangeVolume.AddListener(OnChangeVolume);

            if (OptionSingleton.volume.HasValue) OnChangeVolume(OptionSingleton.volume.GetValueOrDefault());
            else OnChangeVolume(1);
        }

        private void OnCloseOptions()
        {

            PauseGame(false);
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleOptions();
            }
        }

        private void ToggleOptions()
        {
            if (SceneManager.GetSceneByName(Scenes.Options.ToString()).isLoaded == false)
            {
                SceneManager.LoadScene(Scenes.Options.ToString(), LoadSceneMode.Additive);
                PauseGame(true);
            }
            else
            {
                SceneManager.UnloadSceneAsync(Scenes.Options.ToString());
                PauseGame(false);
            }
        }

        public void OnChangeVolume(float volume)
        {
            foreach (var audioSource in _allAudioSources)
            {
                audioSource.volume = volume;
            }
        }

        private void PauseGame(bool value)
        {
            if (value)
            {
                Time.timeScale = 0;

                if (_gameManager?.GameStage != GameStage.NOT_STARTED)
                {
                    foreach (AudioSource audio in _allAudioSources)
                    {
                        audio.Pause();
                    }
                }
            }
            else
            {
                Time.timeScale = 1;

                if (_gameManager?.GameStage != GameStage.NOT_STARTED)
                {
                    foreach (AudioSource audio in _allAudioSources)
                    {
                        audio.UnPause(); 
                    }
                }

                EventSingleton.Events.OnUpdateInput?.Invoke();
            }
        }
    }
}
