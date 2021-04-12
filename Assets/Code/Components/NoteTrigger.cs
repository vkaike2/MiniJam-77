using Assets.Code.Enums;
using Assets.Code.Managers;
using Assets.Code.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Code.Components
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class NoteTrigger : MonoBehaviour
    {
        [SerializeField]
        private Position _inputPosition;
        [SerializeField]
        private Color _color;

        [Header("Configuraton")]
        [SerializeField]
        private float _activatonTime;
        [SerializeField]
        private float _cdwToDestroyNote;

        [Header("Event")]
        [SerializeField]
        private UnityEvent _invokeOnError;

        [Header("Audio Source")]
        [SerializeField]
        private AudioSource _riffAudioSource;

        [Header("Canvas")]
        [SerializeField]
        private TMP_Text _label_Input;


        private const float INITIAL_ALPHA_COLOR = 0.5f;
        private const float ACTIVATION_ALPHA_COLOR = 1f;

        private Note _currentNote;
        private SpriteRenderer _spriteRenderer;
        private float _initialVolume;
        private GameManager _gameManager;
        private KeyCode _currentInput;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _initialVolume = _riffAudioSource.volume;
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
            _color.a = INITIAL_ALPHA_COLOR;
            _spriteRenderer.color = _color;
        }

        private void Start()
        {
            SetInputValues();

            EventSingleton.Events.OnUpdateInput.AddListener(SetInputValues);
        }

        private void SetInputValues()
        {
            _label_Input.SetText(OptionSingleton.GetNoteInput(_inputPosition).ToString());
            _currentInput = OptionSingleton.GetNoteInput(_inputPosition);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_currentInput))
            {
                StartCoroutine(ActivateNote());

                if (_currentNote is null)
                {
                    _invokeOnError.Invoke();
                    _gameManager?.InvokeUptadeScore(false);
                    return;
                }


                _currentNote.correct = true;
                _riffAudioSource.volume = _initialVolume;
                _gameManager?.InvokeUptadeScore(true);
                _currentNote.Kill();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Note note = collision.gameObject.GetComponent<Note>();
            if (note != null)
            {
                _currentNote = note;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Note note = collision.gameObject.GetComponent<Note>();
            if (note != null)
            {
                if (_currentNote != null && !_currentNote.correct)
                {
                    _riffAudioSource.volume = 0;
                    _gameManager.InvokeUptadeScore(false);
                }
                note.Kill();

                _currentNote = null;
            }
        }


        IEnumerator ActivateNote()
        {
            Color color = _spriteRenderer.color;
            color.a = ACTIVATION_ALPHA_COLOR;
            _spriteRenderer.color = color;
            yield return new WaitForSecondsRealtime(_activatonTime);
            color.a = INITIAL_ALPHA_COLOR;
            _spriteRenderer.color = color;

        }
    }

}
