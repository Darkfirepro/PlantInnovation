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

public class WorldAnchorOperation : MonoBehaviour
{

    public enum Selection { CreateNew, SycnDirectly, LocationSync, CreateNew_multi, Version1, Version2 };
    public Selection Choice;
    private GameObject ParentAnchor;
    private byte[] exportedData;
    private List<byte> e;
    private string spaceId;
    public TCPClientReceive tCP;
    public GameObject indicator;
    public GameObject imgSet;
    private int retryCount = 3;
    private byte[] anchorData;
    private bool syncOrNot = false;

    //world anchor unity web request:
    public string anchorStoreHost = "https://ie.csiro.au/services/dan-test-server/v1/api/spaces/";
    private string spaceIdWeb = "Dan";
    const string ANCHOR_STORE = "anchors/";
    const string ANCHOR_DATA = "data/";

    public GameObject anchorObject1;
    public GameObject anchorObject2;
    public GameObject anchorObject3;
    public GameObject anchorObject4;
    public GameObject anchorObject5;
    public GameObject anchorObject2_1;
    public GameObject anchorObject3_1;


    // Start is called before the first frame update
    void Start()
    {
        tCP = GameObject.Find("NetworkTransfer").GetComponent<TCPClientReceive>();
        spaceId = GameObject.FindGameObjectWithTag("SpaceNameObject").GetComponent<ImageTargetBehaviour>().TrackableName;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PressAndGenerate()
    {
        if (Choice == Selection.CreateNew)
        {
            //TextAsset asset = Resources.Load("data_demonstration") as TextAsset;
            //anchorData = asset.bytes;
            //WorldAnchorTrans wat = new WorldAnchorTrans
            //{
            //    header = "wa",
            //    spaceName = spaceId,
            //    data = anchorData
            //};
            //tCP.SendWorlAnchor(wat);

            TimeCost tc4 = new TimeCost("OFALL", Time.time.ToString() + "start generating the anchor");
            tCP.SocketSendByte(tc4);
            //GameObject parentanchor = GameObject.Find("anchor1");
            WorldAnchor wa1 = anchorObject1.AddComponent<WorldAnchor>();
            WorldAnchorTransferBatch watb = new WorldAnchorTransferBatch();
            watb.AddWorldAnchor(anchorObject1.name, wa1);
            WorldAnchor wa2 = anchorObject2.AddComponent<WorldAnchor>();
            watb.AddWorldAnchor(anchorObject2.name, wa2);
            TimeCost tc5 = new TimeCost("OFALL", Time.time.ToString() + "Generating anchor done");
            tCP.SocketSendByte(tc5);
            imgSet.SetActive(true);
            //tCP.InitSocket();

            exportedData = new byte[0];
            e = new List<byte>();
            indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
            syncOrNot = true;
            TimeCost tc6 = new TimeCost("OFALL", Time.time.ToString() + "start serializing the anchor");
            tCP.SocketSendByte(tc6);
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
                        WorldAnchorTrans wat = new WorldAnchorTrans
                        {
                            header = "wa",
                            spaceName = spaceId,
                            data = e.ToArray()
                        };
                        TimeCost tc7 = new TimeCost("OFALL", Time.time.ToString() + "start sending anchor data");
                        tCP.SocketSendByte(tc7);
                        tCP.SendWorlAnchor(wat);
                        //CreateNewAnchorInManager();
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
        else if (Choice == Selection.SycnDirectly)
        {
            //tCP.InitSocket();
            StartCoroutine(DownloadAnchor(spaceId));

        }
        else if (Choice == Selection.LocationSync)
        {
            SendMsg msgNeed = new SendMsg("NeedToSyncPlantSet");
            tCP.SocketSendByte(msgNeed);
        }

        else if (Choice == Selection.CreateNew_multi)
        {
            WorldAnchor wa1 = anchorObject1.AddComponent<WorldAnchor>();
            WorldAnchorTransferBatch watb = new WorldAnchorTransferBatch();
            watb.AddWorldAnchor(anchorObject1.name, wa1);
            WorldAnchor wa2 = anchorObject2.AddComponent<WorldAnchor>();
            watb.AddWorldAnchor(anchorObject2.name, wa2);
            WorldAnchor wa3 = anchorObject3.AddComponent<WorldAnchor>();
            WorldAnchor wa4 = anchorObject4.AddComponent<WorldAnchor>();
            WorldAnchor wa5 = anchorObject5.AddComponent<WorldAnchor>();
            WorldAnchor wa2_1 = anchorObject2_1.AddComponent<WorldAnchor>();
            WorldAnchor wa3_1 = anchorObject3_1.AddComponent<WorldAnchor>();
            imgSet.SetActive(true);
        }
        else if (Choice == Selection.Version1)
        {
            anchorObject1.SetActive(true);
            anchorObject2_1.SetActive(true);
            anchorObject3_1.SetActive(true);
        }
        else if (Choice == Selection.Version2)
        {
            anchorObject1.SetActive(true);
            anchorObject2.SetActive(true);
            anchorObject3.SetActive(true);
            anchorObject4.SetActive(true);
            anchorObject5.SetActive(true);
        }
    }

    IEnumerator SendAnchorTime(TimeCost tc)
    {
        tCP.SocketSendByte(tc);
        yield return new WaitForSeconds(0.1f);
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

        GameObject rb = GameObject.Find("anchor1");
        if (rb != null)
        {

            foreach (string anid in deserializedTransferBatch.GetAllIds())
            {
                Debug.Log("the anchor id contained is: " + anid);
                if (anid == "anchor1") deserializedTransferBatch.LockObject(anid, anchorObject1);
                else if (anid == "anchor2") deserializedTransferBatch.LockObject(anid, anchorObject2);
            }
            indicator.GetComponent<MeshRenderer>().material.color = Color.green;
            TimeCost tc3 = new TimeCost("OFALL", Time.time.ToString() + "successfully locate the anchor");
            tCP.SocketSendByte(tc3);
            syncOrNot = true;
            //syncPlantInfor = true;
        }
        else
        {
            Debug.Log("Failed to find object for anchor id: " + spaceId);
            indicator.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    public void CreateNewAnchorInManager()
    {
        List<Plant> plant = new List<Plant>();
        PlantInfo plantInfo1 = new PlantInfo(plant);
        AnchorInfo anchorInfo1 = new AnchorInfo(plantInfo1);
        anchorInfo1.id = spaceId;
        //AnchorInfo anchorNew = CreateAnchor(anchorInfo1);
        UpdateAnchorData(anchorInfo1.id, e.ToArray());
        //anchorInfoManager.anchorInfoList.Add(anchorNew);
    }

    public AnchorInfo CreateAnchor(AnchorInfo anchorInfo)
    {
        UploadHandlerRaw u = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(anchorInfo)));

        DownloadHandlerBuffer d = new DownloadHandlerBuffer();

        UnityWebRequest r = new UnityWebRequest(anchorStoreHost + GetSpaceId() + "/" + ANCHOR_STORE + "/" + anchorInfo.id, "PUT", d, u);
        r.SetRequestHeader("Content-Type", "application/json");
        r.SendWebRequest();
        return ReadJSONResponse<AnchorInfo>(r);
    }


    public T ReadJSONResponse<T>(UnityWebRequest r)
    {
        if (CheckResponse(r))
        {
            return JsonUtility.FromJson<T>(r.downloadHandler.text);

        }
        else
        {
            return default(T);
        }
    }

    public string GetSpaceId()
    {
        return spaceIdWeb;
    }

    public AnchorInfo UpdateAnchorData(string id, byte[] data)
    {
        UnityWebRequest r = UnityWebRequest.Put(anchorStoreHost + GetSpaceId() + "/" + ANCHOR_STORE + id + "/" + ANCHOR_DATA, data);

        r.SendWebRequest();

        return ReadJSONResponse<AnchorInfo>(r);
    }

    public bool CheckResponse(UnityWebRequest r)
    {
        if (r.responseCode / 100 != 2)
        {
            //if (HoloControllerManager.Instance != null)
            //    HoloControllerManager.Instance.ShowLabel("WEB ERROR: " + r.responseCode + " - " + r.error + "\n" + r.downloadHandler.text);
            //Debug.LogError(r.responseCode + " - " + r.error + "\n" + r.downloadHandler.text + "\n \n");
            return false;
        }
        else
        {
            return true;
        }
    }

    public byte[] DownloadAnchorData(string id)
    {
        UnityWebRequest r = UnityWebRequest.Get(anchorStoreHost + GetSpaceId() + "/" + ANCHOR_STORE + id + "/" + ANCHOR_DATA);
        r.SendWebRequest();
        if (CheckResponse(r))
        {
            return r.downloadHandler.data;
        }
        else
        {
            indicator.GetComponent<MeshRenderer>().material.color = Color.blue;
            return new byte[0];
            //return r.downloadHandler.data;
        }
    }

    IEnumerator DownloadAnchor(string id)
    {
        imgSet.SetActive(true);
        indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
        AnchorRequire ar = new AnchorRequire(id);
        TimeCost tc = new TimeCost("OFALL", Time.time.ToString() + "send request to server to download anchor");
        tCP.SocketSendByte(tc);
        tCP.SocketSendByte(ar);
        yield return new WaitUntil(() => tCP.anchorData.Length != 0);
        TimeCost tc1 = new TimeCost("OFALL", Time.time.ToString() + "get the anchor done");
        tCP.SocketSendByte(tc1);
        //Debug.Log("the size of anchor: " + tCP.anchorData.Length.ToString());
        anchorData = tCP.anchorData;
        tCP.anchorData = null;
        //Debug.Log("start to change colour of cube");
        if (indicator.GetComponent<MeshRenderer>().material.color != Color.green)
        {

            //TextAsset asset = Resources.Load("AnchorData/CSIRO_Lab") as TextAsset;
            //anchorData = asset.bytes;
            indicator.GetComponent<MeshRenderer>().material.color = Color.blue;
            TimeCost tc2 = new TimeCost("OFALL", Time.time.ToString() + "start locating the anchor");
            tCP.SocketSendByte(tc2);
            ImportWorldAnchor(anchorData);
        }
        else
        {
            syncOrNot = true;
            //syncPlantInfor = true;
        }
        //unit test:
        //syncOrNot = true;
        //syncPlantInfor = true;
    }






}
