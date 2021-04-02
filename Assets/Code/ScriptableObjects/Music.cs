using Assets.Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Music", menuName = "SCRIPTABLE/MUSIC", order = 1)]
    public class Music : ScriptableObject
    {
        [Header("AUDIO")]
        [SerializeField]
        private AudioClip _backgroundClip;
        [SerializeField]
        private AudioClip _riffClip;
        [Space]
        [Header("JSON")]
        [TextArea(5, 100)]
        [SerializeField]
        private string _jsonString;

        [Header("CONFIGURATION")]
        [SerializeField]
        private DataConfig _dataConfig;


        public AudioClip BackgroundClip => _backgroundClip;
        public AudioClip RiffClip => _riffClip;
        public DataConfig DataConfig => _dataConfig;


        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(_jsonString))
            {
                _dataConfig = JsonUtility.FromJson<DataConfig>(_jsonString);
            }
        }

    }
}
