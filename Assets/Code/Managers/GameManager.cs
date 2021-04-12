using Assets.Code.Components;
using Assets.Code.Singletons;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Code.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Music Score")]
        [SerializeField]
        private float _maxScore;
        [SerializeField]
        private float _scorePerNote;

        [Header("Audio Sources")]
        [SerializeField]
        private AudioSource _riffAudio;
        [SerializeField]
        private AudioSource _backgroundAudio;
        [SerializeField]
        private AudioSource _loseAudio;
        [SerializeField]
        private AudioSource _winAudio;

        [Header("UI")]
        [SerializeField]
        private GameObject _canvas;

        [Header("Configuration")]
        [SerializeField]
        private float _winCdw;
        [SerializeField]
        private float _loseCdw;
        [SerializeField]
        private string _nextSceneName;

        [Space]
        [Header("Events")]
        [SerializeField]
        private UnityEvent _startGame;
        [SerializeField]
        private UnityEvent _winGame;

        public float MaxScore => _maxScore;
        public float ScorePerNote => _scorePerNote;
        public float CurrentScore { get; set; }
        public GameStage GameStage => _gameStage;

        private Combat _heroCombat;
        private GameStage _gameStage;

        public void InvokeUptadeScore(bool update)
        {
            if (update)
            {
                CurrentScore += _scorePerNote;
                if (CurrentScore > _maxScore) CurrentScore = _maxScore;

            }
            else
            {
                CurrentScore -= _scorePerNote;
                if (CurrentScore < 0) CurrentScore = 0;
            }

            _heroCombat.UpdateBaseDamage(CurrentScore / _maxScore);
            EventSingleton.Events.OnUpdateScore.Invoke(CurrentScore / _maxScore);
        }
        private AudioSource[] _allAudioSources;

        public void StartGame()
        {
            EventSingleton.Events.OnStartGame.Invoke();
        }

        private void Awake()
        {
            _gameStage = GameStage.NOT_STARTED;
            _canvas.SetActive(true);
        }

        private void Start()
        {
            _allAudioSources = GameObject.FindObjectsOfType<AudioSource>();
            _heroCombat = GameObject.FindGameObjectWithTag("HERO").GetComponent<Combat>();
            EventSingleton.Events.OnLose.AddListener(OnLose);
            EventSingleton.Events.OnWin.AddListener(OnWin);
        }

        private void Update()
        {
            StartCooldownToStartGame();
        }

        private void OnWin()
        {
            StopMusic();
            _winAudio.Play();

            StartCoroutine(WinOnCdw());
        }

        private void OnLose()
        {
            StopMusic();
            _loseAudio.Play();
            StartCoroutine(RestartOnCdw());
        }

        private void StopMusic()
        {
            _riffAudio.Stop();
            _backgroundAudio.Stop();
        }

        private void StartCooldownToStartGame()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (_gameStage)
                {
                    case GameStage.NOT_STARTED:
                        _startGame.Invoke();
                        _gameStage = GameStage.COMBAT;
                        break;
                    case GameStage.COMBAT:
                        break;
                    case GameStage.WIN:
                        SceneManager.LoadScene(_nextSceneName);
                        break;
                    default:
                        break;
                }
            }
        }


        IEnumerator WinOnCdw()
        {
            yield return new WaitForSecondsRealtime(_winCdw);
            _gameStage = GameStage.WIN;
            _winGame.Invoke();
        }

        IEnumerator RestartOnCdw()
        {
            yield return new WaitForSecondsRealtime(_loseCdw);
            //SceneManager.LoadScene("OtherSceneName", LoadSceneMode.Additive);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public enum GameStage
    {
        NOT_STARTED,
        COMBAT,
        WIN
    }
}
