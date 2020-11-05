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
    public GameObject userTips;
    public TextMeshPro trackingTips;
    public GameObject trackingAsset;
    //public GameObject finalLocationCanvas;
    public GameObject trackingIndicator;
    //public TextMeshPro queryTips;
    public GameObject queryIndicator;
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
        queryModeIndicator();
    }

    public void ClosePantMap()
    {
        modeText.text = writeIn;
        gameObject.SetActive(true);
        plantMap.SetActive(false);
    }

    void Start()
    {
        trackingAsset.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (anchorParent.transform.childCount == 0)
        {
            string tips = "AR IPS V1.0" + "\nPlease author you space in Authroing mode!";
            userTips.GetComponent<TextMeshPro>().text = tips;
        }
        else
        {
            string tips = "AR IPS V1.0" + "\nSpace has been authored!";
            userTips.GetComponent<TextMeshPro>().text = tips;
        }
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
            trackingIndicator.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            trackingTips.text = "Please firstly author your space with spatial anchors!";
            trackingIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    public void queryModeIndicator()
    {
        if (anchorParent.transform.childCount > 0)
        {
            queryIndicator.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            queryIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
            ClosePantMap();
            ShowMainTool();
        }
    }


}
