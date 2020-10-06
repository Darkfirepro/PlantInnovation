using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.WSA.Sharing;

public class OldSystemSync : MonoBehaviour
{
    //time here:
    public TextMeshProUGUI startWaTime;
    public TextMeshProUGUI CompleteWaTime;

    private TCPClientReceive tCP;
    public bool trigger = false;
    private int retryCount = 10;
    public TMP_InputField IF;
    private int plantNum;
    [HideInInspector] public MultiplyUi mpUI;
    [HideInInspector] public int countPlant = 0;
    public SetRefObjToCertain srotc;
    string anchorId;
    public GameObject indicator;
    private byte[] anchorData;
    private List<string> PlantNameList = new List<string> { "1_GC35L", "3_GC35L", "4_GC35L", "5_GC35L", "6_GC35R", "7_GC35L", "8_GC35L", "14_GC03R" };
    // Start is called before the first frame update
    void Start()
    {
        mpUI = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MultiplyUi>();
        tCP = GameObject.Find("NetworkTransfer").GetComponent<TCPClientReceive>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            if (srotc.totalSyncNum == int.Parse(IF.text))
            {
                trigger = false;
                CompleteWaTime.text = Time.time.ToString();
                indicator.GetComponent<MeshRenderer>().material.color = Color.black;
                LatencyTest latencyTest = new LatencyTest(IF.text, startWaTime.text, CompleteWaTime.text, "NA", "NA", "Old Method");
                tCP.SocketSendByte(latencyTest);
            }
        }
    }

    public void SyncWorldAnchor()
    {
        trigger = true;
        indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
        startWaTime.text = Time.time.ToString();
        plantNum = int.Parse(IF.text);
        for (int i = 0; i < plantNum; i++)
        {
            TextAsset asset = Resources.Load("AnchorData/" + PlantNameList[i]) as TextAsset;
            anchorData = asset.bytes;
            anchorId = PlantNameList[i];
            GameObject plantUI = mpUI.GeneratePlantAnchor(anchorId, Vector3.zero, Vector3.zero, false);
            plantUI.GetComponent<SyncSinglePlant>().StartSyncSinglePlant(anchorData, indicator);
        }
    }
}
