using Assets.Code.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Assets.Code.Components
{
    public class Combat : MonoBehaviour
    {
        [Header("TYPE")]
        [SerializeField]
        private CombatType _type;

        [Space]
        [Header("Configurations")]
        [SerializeField]
        private float _maxHP;
        [Space]
        [SerializeField]
        private float _baseDamage;
        [SerializeField]
        private float _atkSpeed;

        [Header("UI")]
        [SerializeField]
        private TMP_Text _labelDamage;
        [SerializeField]
        private Animator _animatorDamage;
        [SerializeField]
        private UILifeBar _lifeBar;
        [Space]
        [SerializeField]
        private Animator _combatAnimator;
        [SerializeField]
        private float _waitBeforeAtk;

        [Header("AUDIO")]
        [SerializeField]
        private AudioSource _audioSourceAtack;
        [SerializeField]
        private List<AudioClip> _audioAtackClips;



        public bool IsAlive { get; private set; }

        private float _currentHP;
        private float _currentDamage;
        private Combat _target;
        private int _hashAnimatorAtk = Animator.StringToHash("Atk");
        private int _hashAnimatorDie = Animator.StringToHash("Die");

        private int _hashAnimatorDamage = Animator.StringToHash("ShowDmg");

        public void ReceiveDamage(float damage)
        {
            if (!this.IsAlive) return;
            ShowDamageOnUi(damage);

            _currentHP -= damage;
            _lifeBar.RemoveLife(_currentHP / _maxHP);

            if (_currentHP <= 0)
            {
                this.IsAlive = false;

                if (_combatAnimator != null)
                    _combatAnimator.SetTrigger(_hashAnimatorDie);

                switch (_type)
                {
                    case CombatType.HERO:
                        //Hero died
                        EventSingleton.Events.OnLose.Invoke();
                        break;
                    case CombatType.ENEMY:
                        // Enemy died
                        EventSingleton.Events.OnWin.Invoke();
                        break;
                    default:
                        break;
                }
            }
        }

        public void UpdateBaseDamage(float percentage)
        {
            percentage += 1;
            _currentDamage = _baseDamage * percentage;
        }

        private void ShowDamageOnUi(float damage)
        {

            _labelDamage.SetText(damage.ToString("0.00"));
            _animatorDamage.ResetTrigger(_hashAnimatorDamage);
            _animatorDamage.SetTrigger(_hashAnimatorDamage);
        }

        private void Awake()
        {
            IsAlive = true;
            _currentHP = _maxHP;
            _currentDamage = _baseDamage;
        }


        private void Start()
        {
            EventSingleton.Events.OnStartGame.AddListener(SearchForTarget);
            EventSingleton.Events.OnLose.AddListener(OnLose);
            EventSingleton.Events.OnWin.AddListener(OnWin);
        }

        private void OnWin() => StopAllCoroutines();

        private void OnLose()
        {
            StopAllCoroutines();
        }


        IEnumerator Fight()
        {
            while (_target != null && _target.IsAlive)
            {
                yield return new WaitForSeconds(_atkSpeed);
                if (_combatAnimator != null)
                    _combatAnimator.SetTrigger(_hashAnimatorAtk);
                StartCoroutine(WaitSomeTimeThenAtk());
            }
        }

        private void MakeSomeNoise()
        {
            _audioSourceAtack.clip = _audioAtackClips[Random.Range(0, _audioAtackClips.Count)];
            _audioSourceAtack.Play();
        }

        IEnumerator WaitSomeTimeThenAtk()
        {
            yield return new WaitForSeconds(_waitBeforeAtk);
      
            MakeSomeNoise();
            _target.ReceiveDamage(_currentDamage);
        }

        IEnumerator LookingForTarget()
        {
            while (_target == null)
            {
                yield return new WaitForSeconds(1);
                switch (_type)
                {
                    case CombatType.HERO:
                        _target = GameObject.FindGameObjectWithTag(CombatType.ENEMY.ToString()).GetComponent<Combat>();
                        break;
                    case CombatType.ENEMY:
                        _target = GameObject.FindGameObjectWithTag(CombatType.HERO.ToString()).GetComponent<Combat>();
                        break;
                    default:
                        break;
                }
            }

            StartCoroutine(Fight());
        }

        private void SearchForTarget()
        {
            StartCoroutine(LookingForTarget());
        }
    }

    public enum CombatType
    {
        HERO,
        ENEMY
    }
}
