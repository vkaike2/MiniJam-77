using Assets.Code.Models;
using Assets.Code.ScriptableObjects;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Code.Components
{
    public class NoteSpawner : MonoBehaviour
    {
        [Header("Scriptable Object")]
        [SerializeField]
        private Music _music;

        [Header("Configuration")]
        [SerializeField]
        private float _velocity;
       

        [Header("Prefab")]
        [SerializeField]
        private GameObject _notePrefab;

        [Header("Spawner Locations")]
        [SerializeField]
        private Transform _firt;
        [SerializeField]
        private Transform _second;
        [SerializeField]
        private Transform _third;
        [SerializeField]
        private Transform _fourth;

        [Header("Audio Sources")]
        [SerializeField]
        private AudioSource _riffAudio;
        [SerializeField]
        private AudioSource _backgroundAudio;

       

        DataConfig _dataConfig;

        private Color _firstColor;
        private Color _secondColor;
        private Color _thirdColor;
        private Color _fourthColor;

        
        public void SetSetupConfig(DataConfig dataConfig)
        {
            _dataConfig = dataConfig;
        }

        public void StartRiff()
        {
            if (_riffAudio.isPlaying) return;

            StartCoroutine(ReleaseTheNotes());
            StartCoroutine(ReleaseTheSong());
        }

        private void Awake()
        {
            _firstColor = _firt.gameObject.GetComponent<SpriteRenderer>().color;
            _secondColor = _second.gameObject.GetComponent<SpriteRenderer>().color;
            _thirdColor = _third.gameObject.GetComponent<SpriteRenderer>().color;
            _fourthColor = _fourth.gameObject.GetComponent<SpriteRenderer>().color;

            if(_music != null)
            {
                _dataConfig = _music.DataConfig;
                _riffAudio.clip = _music.RiffClip;
                _backgroundAudio.clip = _music.BackgroundClip;
            }

        }


        private void Start()
        {
            StartRiff();
        }

        IEnumerator ReleaseTheNotes()
        {
            GameObject currentNoteGameObject = null;
            Note currentNote = null;
            foreach (var note in _dataConfig.notes)
            {
                yield return new WaitForSecondsRealtime(note.time);

                foreach (var belt in note.belts)
                {

                    switch (belt)
                    {
                        case Belt.first:
                            InstantiateNote(currentNoteGameObject,
                                            currentNote,
                                            _firt,
                                            _firstColor);
                            break;
                        case Belt.second:
                            InstantiateNote(currentNoteGameObject,
                                             currentNote,
                                             _second,
                                             _secondColor);
                            break;
                        case Belt.down:
                            InstantiateNote(currentNoteGameObject,
                                            currentNote,
                                            _third,
                                            _thirdColor);
                            break;
                        case Belt.fourth:
                            InstantiateNote(currentNoteGameObject,
                                           currentNote,
                                           _fourth,
                                           _fourthColor);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        IEnumerator ReleaseTheSong()
        {
            yield return new WaitForSecondsRealtime(_dataConfig.cooldownToStart);
            _riffAudio.Play();
            _backgroundAudio.Play();
        }

        public void InstantiateNote(GameObject gameObject, Note note, Transform spawnLocation, Color noteColor)
        {
            gameObject = Instantiate(_notePrefab, spawnLocation);
            note = gameObject.GetComponent<Note>();
            note.velocity = _velocity;
            note.SetColor(noteColor);
        }

    }


}
