using Assets.Code.Components;
using Assets.Code.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace Assets.Code.Managers
{
    public class DebugManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField]
        private AudioSource _riffAudio;
        [SerializeField]
        private AudioSource _backgroundAudio;

        [Header("Canvas")]
        [SerializeField]
        private GameObject _debugOptions;

        [Header("Inputs")]
        [SerializeField]
        private TMP_InputField _inputBackgroundMusic;
        [SerializeField]
        private TMP_InputField _inputRiffMusic;
        [SerializeField]
        private TMP_InputField _inputJson;

        private NoteSpawner _noteSpawner;

        public void ApplyDebugSetup()
        {
            string jsonString = File.ReadAllText(_inputJson.text);
            DataConfig dataConfig = JsonUtility.FromJson<DataConfig>(jsonString);

            StartCoroutine(LoadAudio(_riffAudio, _inputRiffMusic.text));
            StartCoroutine(LoadAudio(_backgroundAudio, _inputBackgroundMusic.text));

            _noteSpawner.SetSetupConfig(dataConfig);

            _debugOptions.SetActive(false);
        }

        private void Awake()
        {
            _noteSpawner = GameObject.FindObjectOfType<NoteSpawner>();
            _debugOptions.SetActive(false);
        }

        private void Update()
        {
            ToggleDebugOptions();

            PlayMusic();
        }

        private void PlayMusic()
        {
            if (Input.GetKeyDown(UnityEngine.KeyCode.Return))
            {
                _noteSpawner.StartRiff();
            }
        }

        private void ToggleDebugOptions()
        {
            if (Input.GetKeyDown(UnityEngine.KeyCode.Escape))
            {
                _riffAudio.Stop();
                _backgroundAudio.Stop();
                _debugOptions.SetActive(!_debugOptions.activeSelf);
            }
        }


        IEnumerator LoadAudio(AudioSource audioSource, string path)
        {
            using (var www = new WWW(path))
            {
                yield return www;
                audioSource.clip = www.GetAudioClip();
            }
        }
    }

    [Serializable]
    public class JsonTest
    {
        public string teste;
    }
}