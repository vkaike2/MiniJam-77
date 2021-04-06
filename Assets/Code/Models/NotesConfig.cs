using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Models
{
    [Serializable]
    public class DataConfig
    {
        public float cooldownToStart;
        public List<NotesConfig> notes;
    }

    [Serializable]
    public class NotesConfig
    {
        public List<Belt> belts;
        public float time;
    }

    [Serializable]
    public enum Belt
    {
        first ,
        second,
        down,
        fourth,
        wait
    }
}
