using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    [SerializeField] private UnityEvent _onPressedEvent;
    private bool _bCanBeenPressed = true;
    
    public void PressSwitch()
    {
        if (_bCanBeenPressed)
        {
            _onPressedEvent.Invoke();
            _bCanBeenPressed = false;
            _onPressedEvent.RemoveAllListeners();
        }
        
        
    }
}