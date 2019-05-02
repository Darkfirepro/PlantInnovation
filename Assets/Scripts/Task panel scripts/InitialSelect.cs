using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSelect : MonoBehaviour
{

    public enum Selection { Sync, MainTool, PlantView };
    public Selection Choice;
    public GameObject syncStart;
    // Start is called before the first frame update

    public void PressButton()
    {
        if (Choice == Selection.Sync)
        {
            syncStart.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
