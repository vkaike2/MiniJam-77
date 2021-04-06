using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Configurations")]
        [SerializeField]
        private string _nextSceneName;

        private bool _canStart;

        public void CanStart()
        {
            _canStart = true;
        }

        private void Awake()
        {
            _canStart = false;
        }


        private void Update()
        {
            if (!_canStart) return;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(_nextSceneName);
            }
        }
    }
}
