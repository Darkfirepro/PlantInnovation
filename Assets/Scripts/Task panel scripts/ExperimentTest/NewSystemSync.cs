using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.WSA.Sharing;

public class NewSystemSync : MonoBehaviour
{

    //time here:
    public TextMeshProUGUI startWaTime;
    public TextMeshProUGUI CompleteWaTime;
    public TextMeshProUGUI startSocketTime;
    public TextMeshProUGUI CompleteSocketTime;

    private TCPClientReceive tCP;
    public bool trigger = false;
    private int retryCount = 10;
    public TMP_InputField IF;
    [HideInInspector] public MultiplyUi mpUI;
    [HideInInspector] public int countPlant = 0;
    public GameObject indicator;
    private byte[] anchorData;

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
            if (countPlant == int.Parse(IF.text))
            {
                trigger = false;
                CompleteSocketTime.text = Time.time.ToString();
                indicator.GetComponent<MeshRenderer>().material.color = Color.green;
                LatencyTest latencyTest = new LatencyTest(IF.text, startWaTime.text, CompleteWaTime.text, startSocketTime.text, CompleteSocketTime.text, "New Method");
                tCP.SocketSendByte(latencyTest);
            }
        }
    }

    public void SyncWorldAnchor()
    {
        trigger = true;
        indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
        startWaTime.text = Time.time.ToString();
        TextAsset asset = Resources.Load("AnchorData/" + "1_GC35L") as TextAsset;
        anchorData = asset.bytes;
        ImportWorldAnchor(anchorData);
    }

    private void ImportWorldAnchor(byte[] importedData)
    {
        WorldAnchorTransferBatch.ImportAsync(importedData, OnImportComplete);
    }

    private void OnImportComplete(SerializationCompletionReason completionReason, WorldAnchorTransferBatch deserializedTransferBatch)
    {
        if (completionReason != SerializationCompletionReason.Succeeded)
        {
            Debug.Log("Failed to import: " + completionReason.ToString());
            if (retryCount > 0)
            {
                retryCount--;
                indicator.GetComponent<MeshRenderer>().material.color = Color.cyan;
                WorldAnchorTransferBatch.ImportAsync(anchorData, OnImportComplete);
            }
            return;
        }

        GameObject sr = GameObject.Find("RefernceSceneBuilder");
        if (sr != null)
        {
            deserializedTransferBatch.LockObject("1_GC35L", sr);
            CompleteWaTime.text = Time.time.ToString();
            SendMsg sendstr = new SendMsg("SyncPlantNum:" + IF.text);
            startSocketTime.text = Time.time.ToString();
            tCP.SocketSendByte(sendstr);
            
        }
        else
        {
            Debug.Log("Failed to find object for anchor id: " + "1_GC35L");
            indicator.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
