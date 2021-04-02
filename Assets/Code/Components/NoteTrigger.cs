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
        private ArrowNotes _arrowInput;
        [SerializeField]
        private Color _color;

        [Header("Configuraton")]
        [SerializeField]
        private float _activatonTime;
        [SerializeField]
        private float _cdwToDestroyNote;
        [SerializeField]
        private float _muteCdw;

        [Header("Event")]
        [SerializeField]
        private UnityEvent _invokeOnError;

        [Header("Audio Source")]
        [SerializeField]
        private AudioSource _riffAudioSource;


        private const float INITIAL_ALPHA_COLOR = 0.5f;
        private const float ACTIVATION_ALPHA_COLOR = 1f;

        private Note _currentNote;
        private SpriteRenderer _spriteRenderer;
        private float _initialVolume;

        private void Awake()
        {
            _initialVolume = _riffAudioSource.volume;
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
            _color.a = INITIAL_ALPHA_COLOR;
            _spriteRenderer.color = _color;
        }

        private void Update()
        {
            if (Input.GetKeyDown((KeyCode)_arrowInput))
            {
                StartCoroutine(ActivateNote());

                if (_currentNote is null)
                {
                    _invokeOnError.Invoke();
                    return;
                }

                // ponto
                _currentNote.correct = true;
                _riffAudioSource.volume = _initialVolume;
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
                    _riffAudioSource.volume = 0;
                note.Miss();
                //StartCoroutine(MuteNote());
                StartCoroutine(DestroyNoteAfterMis(_currentNote));

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

        IEnumerator DestroyNoteAfterMis(Note note)
        {
            yield return new WaitForSecondsRealtime(_cdwToDestroyNote);
            if (note != null)
                Destroy(note.gameObject);
        }
    }

    public enum ArrowNotes
    {
        UpArrow = 273,
        //
        // Resumo:
        //     Down arrow key.
        DownArrow = 274,
        //
        // Resumo:
        //     Right arrow key.
        RightArrow = 275,
        //
        // Resumo:
        //     Left arrow key.
        LeftArrow = 276,
        //
        // Resumo:
        //     'a' key.
        A = 97,
        //
        // Resumo:
        //     'b' key.
        B = 98,
        //
        // Resumo:
        //     'c' key.
        C = 99,
        //
        // Resumo:
        //     'd' key.
        D = 100,
        //
        // Resumo:
        //     'e' key.
        E = 101,
        //
        // Resumo:
        //     'f' key.
        F = 102,
        //
        // Resumo:
        //     'g' key.
        G = 103,
        //
        // Resumo:
        //     'h' key.
        H = 104,
        //
        // Resumo:
        //     'i' key.
        I = 105,
        //
        // Resumo:
        //     'j' key.
        J = 106,
        //
        // Resumo:
        //     'k' key.
        K = 107,
        //
        // Resumo:
        //     'l' key.
        L = 108,
        //
        // Resumo:
        //     'm' key.
        M = 109,
        //
        // Resumo:
        //     'n' key.
        N = 110,
        //
        // Resumo:
        //     'o' key.
        O = 111,
        //
        // Resumo:
        //     'p' key.
        P = 112,
        //
        // Resumo:
        //     'q' key.
        Q = 113,
        //
        // Resumo:
        //     'r' key.
        R = 114,
        //
        // Resumo:
        //     's' key.
        S = 115,
        //
        // Resumo:
        //     't' key.
        T = 116,
        //
        // Resumo:
        //     'u' key.
        U = 117,
        //
        // Resumo:
        //     'v' key.
        V = 118,
        //
        // Resumo:
        //     'w' key.
        W = 119,
        //
        // Resumo:
        //     'x' key.
        X = 120,
        //
        // Resumo:
        //     'y' key.
        Y = 121,
        //
        // Resumo:
        //     'z' key.
        Z = 122,
    }
}
