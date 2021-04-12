using Assets.Code.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Singletons
{
    public class OptionSingleton : MonoBehaviour
    {
        public static float? volume;

        public static KeyCode? first_input;
        public static KeyCode? second_input;
        public static KeyCode? third_input;
        public static KeyCode? fourth_input;

        public static KeyCode GetNoteInput(Position position)
        {
            switch (position)
            {
                case Position.First:
                    return first_input.GetValueOrDefault();
                case Position.Second:
                    return second_input.GetValueOrDefault();
                case Position.Third:
                    return third_input.GetValueOrDefault();
                case Position.Fourth:
                    return fourth_input.GetValueOrDefault();
            }
            throw new Exception("You did a mistake my boy");
        }

        public static void SetNoteInput(Position position, KeyCode keyCode)
        {
            switch (position)
            {
                case Position.First:
                    first_input = keyCode;
                    break;
                case Position.Second:
                    second_input = keyCode;
                    break;
                case Position.Third:
                    third_input = keyCode;
                    break;
                case Position.Fourth:
                    fourth_input = keyCode;
                    break;
            }
        }

    }
}
