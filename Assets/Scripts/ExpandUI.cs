using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandUI : MonoBehaviour {

    public void ShowUiInfor()
    {
        if (gameObject.activeSelf == true)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        
    }

}
