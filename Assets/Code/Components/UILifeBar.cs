using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Components
{
    public class UILifeBar: MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]
        private Image _fillImage;



        public void RemoveLife(float fill)
        {
            _fillImage.fillAmount = fill;
        }

    }
}
