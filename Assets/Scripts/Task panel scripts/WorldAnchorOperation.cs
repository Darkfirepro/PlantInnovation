using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Sharing;
using Vuforia;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Azure.SpatialAnchors;
using System.Collections;
using System.IO;

public class WorldAnchorOperation : MonoBehaviour
{

    public enum Selection { CreateNew, SycnDirectly};
    public Selection Choice;
    [HideInInspector]
    public WorldAnchorTransferBatch watb;
    public GameObject anchorObjPrefab;
    public GameObject anchorSetControl;
    private byte[] exportedData;
    private List<byte> e;
    private string spaceId;
    public TCPClientReceive tCP;
    public GameObject indicator;
    private int retryCount = 15;
    private bool syncOrNot = false;

    string bytePath;

    byte[] anchorDataRecv;

    //world anchor unity web request:
    public string anchorStoreHost = "https://ie.csiro.au/services/dan-test-server/v1/api/spaces/";
    private string spaceIdWeb = "Dan";
    const string ANCHOR_STORE = "anchors/";
    const string ANCHOR_DATA = "data/";


    // Start is called before the first frame update
    void Start()
    {
        watb = new WorldAnchorTransferBatch();
        tCP = GameObject.Find("NetworkTransfer").GetComponent<TCPClientReceive>();
        spaceId = GameObject.FindGameObjectWithTag("SpaceNameObject").GetComponent<ImageTargetBehaviour>().TrackableName;
        bytePath = Path.Combine(Application.persistentDataPath, "anchorData");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PressAndGenerate()
    {
        if (Choice == Selection.CreateNew)
        {
            exportedData = new byte[0];
            e = new List<byte>();
            indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
            syncOrNot = true;
            WorldAnchorTransferBatch.ExportAsync(watb,
            (data) =>
            {
                e.AddRange(data);
                exportedData = data;
            },
                (reason) =>
                {

                    if (reason == SerializationCompletionReason.Succeeded)
                    {
                        //tCP.SendWorlAnchor(e.ToArray());
                        //CreateNewAnchorInManager();
                        if (File.Exists(bytePath))
                        {
                            File.Delete(bytePath);
                        }
                        using (FileStream fs = File.Create(bytePath))
                        {
                            fs.Write(e.ToArray(), 0, e.ToArray().Length);
                        }
                        tCP.SocketSendByte(new SendMsg("success, anchor data size:" + e.ToArray().Length.ToString()));
                        indicator.GetComponent<MeshRenderer>().material.color = Color.green;

                        syncOrNot = true;
                    }
                    else
                    {
                        Debug.Log("failed to upload world anchor, please try agagin");
                        indicator.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                });
        }
    }

    public void SyncAnchor()
    {
        indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
        AnchorRequire ar = new AnchorRequire(spaceId);
        tCP.SocketSendByte(ar);
    }

    public void ImportAnchor()
    {
        if (indicator.GetComponent<MeshRenderer>().material.color != Color.green)
        {
            indicator.GetComponent<MeshRenderer>().material.color = Color.blue;
            tCP.SocketSendByte(new SendMsg("start to import world anchor"));
            anchorDataRecv = tCP.anchorData;
            WorldAnchorTransferBatch.ImportAsync(anchorDataRecv, OnImportComplete);
        }
    }

    public void ImportAnchorWithReadingBytes()
    {
        //TextAsset asset = Resources.Load("wa_data") as TextAsset;
        //tCP.SocketSendByte(new SendMsg("start to import world anchor"));
        //anchorDataRecv = asset.bytes;
        //WorldAnchorTransferBatch.ImportAsync(anchorDataRecv, OnImportComplete);
        tCP.SocketSendByte(new SendMsg("start to read world anchor from local anchor data"));
        anchorDataRecv = File.ReadAllBytes(bytePath);
        tCP.SocketSendByte(new SendMsg("start to import world anchor from local anchor data"));
        WorldAnchorTransferBatch.ImportAsync(anchorDataRecv, OnImportComplete);
    }


    public void OnImportComplete(SerializationCompletionReason completionReason, WorldAnchorTransferBatch deserializedTransferBatch)
    {
        tCP.SocketSendByte(new SendMsg("start to check on import complete"));
        if (completionReason != SerializationCompletionReason.Succeeded)
        {
            tCP.SocketSendByte(new SendMsg("fail to import and try again"));
            if (retryCount > 0)
            {
                retryCount--;
                indicator.GetComponent<MeshRenderer>().material.color = Color.cyan;
                WorldAnchorTransferBatch.ImportAsync(anchorDataRecv, OnImportComplete);
            }
            return;
        }
        else if (completionReason == SerializationCompletionReason.Succeeded)
        {
            if (deserializedTransferBatch.anchorCount == 0)
            {
                tCP.SocketSendByte(new SendMsg("sucess, but anchor count is 0"));
                if (retryCount > 0)
                {
                    retryCount--;
                    indicator.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    WorldAnchorTransferBatch.ImportAsync(anchorDataRecv, OnImportComplete);
                }
                return;
            }
            
            tCP.SocketSendByte(new SendMsg("import successfully, trying to check anchor id"));
            foreach (string anid in deserializedTransferBatch.GetAllIds())
            {
                tCP.SocketSendByte(new SendMsg("the anchor id contained is: " + anid));
                GameObject anchorObj = GameObject.Find(anid);
                if (anchorObj != null)
                {
                    deserializedTransferBatch.LockObject(anid, anchorObj);
                }
                else
                {
                    GameObject ins = Instantiate(anchorObjPrefab, anchorSetControl.transform);
                    ins.name = anid;
                    deserializedTransferBatch.LockObject(anid, ins);
                }
            }
            GameObject syncAnchor = GameObject.Find("SyncAnchor");
            syncAnchor.GetComponent<WorldAnchorOperation>().watb = deserializedTransferBatch;
            indicator.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            Debug.Log("Failed to find object for anchor id: " + spaceId);
            indicator.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    public void DownloadAnchor(string id)
    {
        indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
        AnchorRequire ar = new AnchorRequire(id);
        tCP.SocketSendByte(ar);
        //yield return new WaitUntil(() => tCP.anchorData.Length != 0);
        //anchorData = tCP.anchorData;
        //tCP.anchorData = null;
        //if (indicator.GetComponent<MeshRenderer>().material.color != Color.green)
        //{
        //    indicator.GetComponent<MeshRenderer>().material.color = Color.blue;
        //    Debug.Log("start to import the world anchor");
        //    ImportWorldAnchor(anchorData);
        //}
        //else
        //{
        //    syncOrNot = true;
        //    //syncPlantInfor = true;
        //}
    }






}
