using System;
using System.Collections.Generic;
using Core.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Visualisation
{
    public class Border : MonoBehaviour, IStateAffectible
    {
        private Dictionary<GameLoopState, Color> _colors;

        [SerializeField] private RawImage _image;
        [SerializeField] private ColorStateBunch[] _states;

        public GameLoopState CurrentState { get; private set; }

        private void Awake()
        {
            OnValidate();
            FillDictionaryFromInspector();
        }

        private void FillDictionaryFromInspector()
        {
            _colors = new Dictionary<GameLoopState, Color>();
            for (int i = 0; i < _states.Length; i++)
            {
                ColorStateBunch currentState = _states[i];
                _colors.Add(currentState.State, currentState.Color);
            }
        }

        public void SetState(GameLoopState state)
        {
            CurrentState = state;
            _image.color = _colors[state];
        }

        private void OnValidate()
        {
            if (_states == null || _states.Length == 0)
            {
                throw new NotImplementedException(nameof(Border) + "::" + nameof(_states));
            }

            if (_image == null)
            {
                _image = GetComponent<RawImage>();
            }
        }
    }

    [Serializable]
    public class ColorStateBunch
    {
        public GameLoopState State;
        public Color Color;
    }
}
