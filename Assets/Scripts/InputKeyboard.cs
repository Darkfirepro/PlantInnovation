using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputKeyboard : MonoBehaviour
{
    public TouchScreenKeyboard keyboard;
    private string keyboardText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (keyboard != null)
        {
            keyboardText = keyboard.text;
            GetComponent<InputField>().text = keyboardText;
            // Do stuff with keyboardText
        }
    }

    public void OpenSystemKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
    }
}
