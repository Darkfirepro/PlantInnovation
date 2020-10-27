using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFollowMew : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnFollowMe()
    {
        gameObject.GetComponent<Orbital>().enabled = !GetComponent<Orbital>().isActiveAndEnabled;
    }
}
