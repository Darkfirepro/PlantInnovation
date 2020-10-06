using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{

    public bool isToggle = false;
    // Start is called before the first frame update

    public void changeValueToggle()
    {
        if (isToggle) isToggle = false;
        else isToggle = true;
    }
}
