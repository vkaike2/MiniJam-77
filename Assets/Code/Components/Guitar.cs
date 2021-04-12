using Assets.Code.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Components
{
    public class Guitar : MonoBehaviour
    {
        [Header("Scriptable Object")]
        [SerializeField]
        private Music _music;


        public Music Music => _music;

    }
}
