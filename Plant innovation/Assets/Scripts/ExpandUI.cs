using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandUI : MonoBehaviour {

    public GameObject ToggleSet;

    public void ShowUiInfor()
    {
        if (ToggleSet.GetComponent<Toggle>().isOn == false)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        
    }

}
