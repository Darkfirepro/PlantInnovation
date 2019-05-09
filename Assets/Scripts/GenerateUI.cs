using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA;
using Vuforia;

public class GenerateUI : MonoBehaviour
{
    private string plantName;
    public MultiplyUi mpUI;

    public void Start()
    {
        plantName = transform.parent.GetComponent<ImageTargetBehaviour>().TrackableName;
        mpUI = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MultiplyUi>();
    }

    public void StartLocatingPant()
    {
        mpUI.GeneratePlantAnchor(plantName, transform.position, transform.eulerAngles, false);
        //mpUI.GeneratePlantAnchor(plantName, transform.position, transform.eulerAngles, false);

    }

}
