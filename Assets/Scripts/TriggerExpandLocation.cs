using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExpandLocation : MonoBehaviour
{

    public void ShowLocInfo()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideLocInfo()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
