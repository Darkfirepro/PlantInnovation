using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.WSA;

public class ResetPlant : MonoBehaviour
{

    public OldSystemSync oss;
    public NewSystemSync nss;
    public SetRefObjToCertain sfotc;
    public TextMeshProUGUI startWaTime;
    public TextMeshProUGUI CompleteWaTime;
    public TextMeshProUGUI startSocketTime;
    public TextMeshProUGUI CompleteSocketTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPlantScene()
    {
        GameObject[] plantList = GameObject.FindGameObjectsWithTag("PlantSet");
        foreach (GameObject plant in plantList)
        {
            Destroy(plant);
        }

        nss.countPlant = 0;
        oss.countPlant = 0;
        sfotc.totalSyncNum = 0;

        startWaTime.text = "waiting for";
        CompleteWaTime.text = "waiting for";
        startSocketTime.text = "waiting for";
        CompleteSocketTime.text = "waiting for";

        Destroy(GameObject.Find("RefernceSceneBuilder").GetComponent<WorldAnchor>());
    }

}
