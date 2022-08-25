using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeystrokeListener : MonoBehaviour
{
    public event Action<KeyCode> OnKeystrokeDetected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        foreach(KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode))){
            if(Input.GetKey(keyCode)) {
                OnKeystrokeDetected?.Invoke(keyCode);
            }
        }
    }
}
