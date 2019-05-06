using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputKeyboard : MonoBehaviour
{
    public TouchScreenKeyboard keyboard;
    string inputWant;
    //public TextMesh debugMessage;

    // Start is called before the first frame update
    void Start()
    {

    }

#if UNITY_WSA
    // Update is called once per frame
    private void Update()
    {
        if (keyboard != null)
        {
            GetComponent<InputField>().text = inputWant + keyboard.text;
            // Do stuff with keyboardText
            if (TouchScreenKeyboard.visible)
            {
                //debugMessage.text = "typing... " + keyboard.text;
            }
            else
            {
                //debugMessage.text = "typed " + keyboard.text;
                keyboard = null;
            }
        }
    }
#endif

    public void OpenSystemKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
        inputWant = GetComponent<InputField>().text;
    }
}
