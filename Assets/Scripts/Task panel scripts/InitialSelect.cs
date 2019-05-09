using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class InitialSelect : MonoBehaviour
{
    public GameObject syncStart;
    public GameObject mainToolStart;
    // Start is called before the first frame update

    public void ShowSync()
    {
        syncStart.SetActive(true);
        gameObject.SetActive(false);
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
    }

    public void CloseMainTool()
    {
        gameObject.SetActive(true);
        mainToolStart.SetActive(false);
    }

    void Start()
    {
        GameObject userTips = GameObject.FindGameObjectWithTag("AssistantTips");
        string spaceName = GameObject.FindGameObjectWithTag("SpaceNameObject").GetComponent<ImageTargetBehaviour>().TrackableName;
        string tips = "Welcome to: " + spaceName + "\nPlease synchronizing you system first";
        userTips.GetComponent<TMPro.TextMeshPro>().text = tips;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
