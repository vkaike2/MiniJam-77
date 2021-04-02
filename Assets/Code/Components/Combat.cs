using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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


        public bool IsAlive { get; private set; }

        private float _currentHP;
        private float _currentDamage;
        private Combat _target;
        private UILifeBar _lifeBar;

        private int _hashAnimatorDamage = Animator.StringToHash("ShowDmg");

        public void ReceiveDamage(float damage)
        {
            if (!this.IsAlive) return;

            _currentHP -= damage;
            ShowDamageOnUi(damage);
            _lifeBar.RemoveLife(_currentHP / _maxHP);

            if (_currentHP <= 0)
            {
                this.IsAlive = false;
                Debug.Log(_type.ToString() + " MORREU!");

                if (_type == CombatType.HERO)
                {
                    SceneManager.LoadScene(0);
                    //Application.LoadLevel(Application.loadedLevel);
                }
            }
        }

        public void UpdateBaseDamage(float percentage)
        {
            percentage += 1;
            _currentDamage = _baseDamage* percentage;
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
            SearchForTarget();

            _lifeBar = this.GetComponentInChildren<UILifeBar>();

        }


        private void Start()
        {
            StartCoroutine(Fight());
        }

        IEnumerator Fight()
        {
            while (_target != null && _target.IsAlive)
            {
                yield return new WaitForSecondsRealtime(_atkSpeed);
                _target.ReceiveDamage(_currentDamage);
            }
        }

        private void SearchForTarget()
        {
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
    }

    public enum CombatType
    {
        HERO,
        ENEMY
    }
}
