using System;
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
        inputField = gameObject.GetComponent<InputField>();
        bondingBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<InputField>().text == "1")
        {
            bondingBox.SetActive(true);
        }
        else if (gameObject.GetComponent<InputField>().text == "0" || gameObject.GetComponent<InputField>().text == "")
        {
            bondingBox.SetActive(false);
        }
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

    //public void ValueChangeCheck()
    //{
    //    try
    //    {
    //        if (gameObject.GetComponent<InputField>().text == "1")
    //        {
    //            bondingBox.SetActive(true);
    //        }
    //        else if (gameObject.GetComponent<InputField>().text == "0" || gameObject.GetComponent<InputField>().text == "")
    //        {
    //            bondingBox.SetActive(false);
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        print(e);
    //    }

    //}

}
