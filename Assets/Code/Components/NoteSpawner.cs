using Assets.Code.Models;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Components
{
    public class NoteSpawner : MonoBehaviour
    {
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


        private Color _firstColor;
        private Color _secondColor;
        private Color _thirdColor;
        private Color _fourthColor;

        DataConfig _dataConfig;

        public void SetSetupConfig(DataConfig dataConfig)
        {
            _dataConfig = dataConfig;
        }

        public void StartRiff()
        {
            if (_riffAudio.isPlaying) return;

            //_dataConfig.notes = _dataConfig.notes.OrderBy(e => e.time).ToList();
            StartCoroutine(ReleaseTheNotes());
            StartCoroutine(ReleaseTheSong());
        }

        private void Awake()
        {
            _firstColor = _firt.gameObject.GetComponent<SpriteRenderer>().color;
            _secondColor = _second.gameObject.GetComponent<SpriteRenderer>().color;
            _thirdColor = _third.gameObject.GetComponent<SpriteRenderer>().color;
            _fourthColor = _fourth.gameObject.GetComponent<SpriteRenderer>().color;
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
