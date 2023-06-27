using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Widgets
{
    public class CustomButton : Button
    {
        [SerializeField] private GameObject _normalState;
        [SerializeField] private GameObject _pressedState;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            
            _normalState.SetActive(state != SelectionState.Pressed);
            _pressedState.SetActive(state == SelectionState.Pressed);
        }
    }
}