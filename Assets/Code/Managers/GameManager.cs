using Assets.Code.Components;
using Assets.Code.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Score")]
        [SerializeField]
        private float _maxScore;
        [SerializeField]
        private float _scorePerNote;


        public float MaxScore => _maxScore;
        public float ScorePerNote => _scorePerNote;
        public float CurrentScore { get; set; }

        private Combat _heroCombat;

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


        private void Start()
        {
            _heroCombat = GameObject.FindGameObjectWithTag("HERO").GetComponent<Combat>();
        }
    }
}
