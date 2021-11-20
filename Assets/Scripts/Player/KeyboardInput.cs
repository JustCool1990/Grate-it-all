using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardInput : MonoBehaviour
{
    public event UnityAction PressedJumpButton;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PressedJumpButton?.Invoke();
        }
    }
}
