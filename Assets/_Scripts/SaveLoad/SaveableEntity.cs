﻿using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.SaveLoad
{
    public class SaveableEntity : MonoBehaviour
    {
        public string ID { get; private set; }

        private void Awake()
        {
            ID = transform.position.sqrMagnitude + "-" + name + "-" + transform.GetSiblingIndex();
        }

        public object CaptureState()
        {
            var state = new Dictionary<string, object>();

            foreach (var saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            var stateDictionary = (Dictionary<string, object>) state;

            foreach (var saveable in GetComponents<ISaveable>())
            {
                string typeName = saveable.GetType().ToString();

                if (stateDictionary.TryGetValue(typeName, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }
    }
}
