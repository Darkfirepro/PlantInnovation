using Microsoft.MixedReality.Toolkit.UI;
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
    public TCPClientReceive tCP;


    public void Start()
    {
        plantName = transform.parent.GetComponent<ImageTargetBehaviour>().TrackableName;
        mpUI = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MultiplyUi>();
        tCP = GameObject.Find("NetworkTransfer").GetComponent<TCPClientReceive>();
    }


    public void StartLocatingPant()
    {
        mpUI.GeneratePlantAnchor(plantName, transform.position, transform.eulerAngles, false);
        GameObject UIcontainer = GameObject.Find(plantName);
        PlantSet pObject = new PlantSet
        {
            Name = plantName,
            rotate = UIcontainer.transform.localEulerAngles,
            pos = UIcontainer.transform.localPosition,
            header = "ps"
        };
        tCP.SocketSendByte(pObject);

        //foreach (Transform uiWhole in UIcontainer.transform.GetChild(0))
        //{
        //    GameObject button = uiWhole.GetChild(0).GetChild(4).gameObject;
        //    button.GetComponent<Interactable>().OnClick.Invoke();
        //}
    }

}
