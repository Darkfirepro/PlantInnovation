using Microsoft.MixedReality.Toolkit.UI;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Sharing;
using Vuforia;

public class GenerateUI : MonoBehaviour
{
    private string plantName;
    [HideInInspector]public MultiplyUi mpUI;
    [HideInInspector]public TCPClientReceive tCP;

    //for testing the latency:
    private byte[] exportedData;
    private List<byte> e;
    public GameObject indicator;

    public string anchorStoreHost = "https://ie.csiro.au/services/dan-test-server/v1/api/spaces/";
    private string spaceIdWeb = "Dan";
    const string ANCHOR_STORE = "anchors/";
    const string ANCHOR_DATA = "data/";


    
    public void Start()
    {
        plantName = transform.parent.GetComponent<ImageTargetBehaviour>().TrackableName;
        mpUI = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MultiplyUi>();
        tCP = GameObject.Find("NetworkTransfer").GetComponent<TCPClientReceive>();
    }


    public void StartLocatingPant()
    {
        
        GameObject UIcontainer = mpUI.GeneratePlantAnchor(plantName, transform.position, transform.eulerAngles, false);
        //GameObject UIcontainer = GameObject.Find(plantName);
        PlantSet pObject = new PlantSet
        {
            Name = plantName,
            rotate = UIcontainer.transform.localEulerAngles,
            pos = UIcontainer.transform.localPosition,
            header = "ps"
        };
        //tCP.SocketSendByte(pObject);

        
        //foreach (Transform uiWhole in UIcontainer.transform.GetChild(0))
        //{
        //    GameObject button = uiWhole.GetChild(0).GetChild(4).gameObject;
        //    button.GetComponent<Interactable>().OnClick.Invoke();
        //}
    }

}
