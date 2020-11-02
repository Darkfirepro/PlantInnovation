using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vuforia;

public class InitialSelect : MonoBehaviour
{
    public GameObject syncStart;
    public GameObject mainToolStart;
    public GameObject plantMap;
    public GameObject anchorParent;
    public TextMeshPro modeText;
    public string writeIn;
    public TextMeshPro trackingTips;
    public GameObject trackingAsset;
    // Start is called before the first frame update

    public void ShowSync()
    {
        modeText.text = "Tracking Mode";
        syncStart.SetActive(true);
        gameObject.SetActive(false);
        ShowHideAnchor(false);
        TrackingTipStart();
    }

    public void CloseSyncStart()
    {
        modeText.text = writeIn;
        gameObject.SetActive(true);
        syncStart.SetActive(false);
    }

    public void ShowMainTool()
    {
        modeText.text = "Authoring Mode";
        mainToolStart.SetActive(true);
        gameObject.SetActive(false);
        ShowHideAnchor(true);
    }

    public void CloseMainTool()
    {
        modeText.text = writeIn;
        gameObject.SetActive(true);
        mainToolStart.SetActive(false);
    }

    public void ShowPlantMap()
    {
        modeText.text = "Querying Mode";
        plantMap.SetActive(true);
        gameObject.SetActive(false);
        ShowHideAnchor(false);
    }

    public void ClosePantMap()
    {
        modeText.text = writeIn;
        gameObject.SetActive(true);
        plantMap.SetActive(false);
    }

    void Start()
    {
        GameObject userTips = GameObject.FindGameObjectWithTag("AssistantTips");
        string tips = "AR IPS V1.0" + "\nPlease synchronizing you system first";
        userTips.GetComponent<TextMeshPro>().text = tips;
        trackingAsset.SetActive(false);
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

    public void TrackingTipStart()
    {
        if (anchorParent.transform.childCount > 0)
        {
            trackingTips.text = "Space has been authored, please start tracking your item!";
            trackingAsset.SetActive(true);
        }
        else
        {
            trackingTips.text = "Please firstly author your space with spatial anchors!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
