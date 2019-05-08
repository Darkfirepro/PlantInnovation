using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBoxInUI : MonoBehaviour
{
    InputField inputField;
    public GameObject bondingBox;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<InputField>();
        bondingBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PressNo()
    {
        inputField.text = "";
        inputField.text = "0";
    }

    public void PressYes()
    {
        inputField.text = "";
        inputField.text = "1";
    }

    public void ValueChangeCheck()
    {
        if (inputField.text == "1")
        {
            bondingBox.SetActive(true);
        }
        else if (inputField.text == "0" || inputField.text == "")
        {
            bondingBox.SetActive(false);
        }
    }

}
