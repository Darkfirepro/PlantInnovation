using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class InitialSelect : MonoBehaviour
{
    public GameObject syncStart;
    public GameObject mainToolStart;
    public GameObject plantMap;
    public GameObject anchorParent;
    // Start is called before the first frame update

    public void ShowSync()
    {
        syncStart.SetActive(true);
        gameObject.SetActive(false);
        ShowHideAnchor(false);
    }

    public void CloseSyncStart()
    {
        gameObject.SetActive(true);
        syncStart.SetActive(false);
    }

    public void ShowMainTool()
    {
        mainToolStart.SetActive(true);
        gameObject.SetActive(false);
        ShowHideAnchor(true);
    }

    public void CloseMainTool()
    {
        gameObject.SetActive(true);
        mainToolStart.SetActive(false);
    }

    public void ShowPlantMap()
    {
        plantMap.SetActive(true);
        gameObject.SetActive(false);
        ShowHideAnchor(false);
    }

    public void ClosePantMap()
    {
        gameObject.SetActive(true);
        plantMap.SetActive(false);
    }

    void Start()
    {
        GameObject userTips = GameObject.FindGameObjectWithTag("AssistantTips");
        string spaceName = "Hancock Library";
        string tips = "Welcome to: " + spaceName + "\nPlease synchronizing you system first";
        userTips.GetComponent<TMPro.TextMeshPro>().text = tips;
    }

    public void ShowHideAnchor(bool trigger)
    {
        if (anchorParent.transform.childCount > 0)
        {
            foreach (Transform t in anchorParent.transform)
            {
                t.GetComponent<MeshRenderer>().enabled = trigger;
                t.GetComponent<ManipulationHandler>().enabled = trigger;
                t.GetComponent<BoundingBox>().enabled = trigger;
                t.GetChild(0).gameObject.SetActive(trigger);
                t.GetChild(1).GetComponent<MeshRenderer>().enabled = trigger;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
