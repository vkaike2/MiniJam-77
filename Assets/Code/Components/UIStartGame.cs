using UnityEngine;

namespace Assets.Code.Components
{
    public class UIStartGame : MonoBehaviour
    {


        [Header("CANVAS UI")]
        [SerializeField]
        private GameObject _backgroundGameObject;
        [SerializeField]
        private GameObject _lablGameObject;

        
        public void StartGame()
        {
            _lablGameObject.SetActive(false);
        }
    }
}
