using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Sharing;
using Vuforia;
using Newtonsoft.Json;
using System.Text;

public class WorldAnchorOperation : MonoBehaviour
{

    public enum Selection { CreateNew, SycnDirectly };
    public Selection Choice;
    private GameObject ParentAnchor;
    private byte[] exportedData;
    private List<byte> e;
    private string spaceId;
    public TCPClientReceive tCP;
    public GameObject indicator;
    public GameObject imgSet;
    private int retryCount = 10;
    private byte[] anchorData;
    private bool syncOrNot = false;
    private bool syncPlantInfor = false;

    //world anchor unity web request:
    public string anchorStoreHost = "https://ie.csiro.au/services/dan-test-server/v1/api/spaces/";
    private string spaceIdWeb = "Dan";
    const string ANCHOR_STORE = "anchors/";
    const string ANCHOR_DATA = "data/";


    // Start is called before the first frame update
    void Start()
    {
        tCP = GameObject.Find("NetworkTransfer").GetComponent<TCPClientReceive>();
        spaceId = GameObject.FindGameObjectWithTag("SpaceNameObject").GetComponent<ImageTargetBehaviour>().TrackableName;
    }

    // Update is called once per frame
    void Update()
    {
        if (syncOrNot == true)
        {
            syncOrNot = false;

            tCP.SocketSend("NeedToSyncPlantSet");
        }
        if (GameObject.FindGameObjectsWithTag("PlantSet").Length == tCP.plantSetNum && syncPlantInfor)
        {
            syncPlantInfor = false;
            tCP.SocketSend("NeedToSyncPlantInfor");
        }
    }

    public void PressAndGenerate()
    {
        if (Choice == Selection.CreateNew)
        {
            ParentAnchor = GameObject.Find("RefernceSceneBuilder");
            WorldAnchor wa = ParentAnchor.AddComponent<WorldAnchor>();
            WorldAnchorTransferBatch watb = new WorldAnchorTransferBatch();
            watb.AddWorldAnchor(spaceId, wa);
            imgSet.SetActive(true);
            //tCP.InitSocket();
            
            exportedData = new byte[0];
            e = new List<byte>();
            indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
            syncOrNot = true;
            //WorldAnchorTransferBatch.ExportAsync(watb,
            //(data) =>
            //{
            //    e.AddRange(data);
            //    exportedData = data;
            //},
            //    (reason) =>
            //    {

            //        if (reason == SerializationCompletionReason.Succeeded)
            //        {
            //            //WorldAnchorTrans wat = new WorldAnchorTrans
            //            //{
            //            //    header = "wa",
            //            //    spaceName = spaceId,
            //            //    data = e.ToArray()
            //            //};
            //            //tCP.SendWorlAnchor(wat);
            //            CreateNewAnchorInManager();
            //            indicator.GetComponent<MeshRenderer>().material.color = Color.green;
            //            syncOrNot = true;
            //        }
            //        else
            //        {
            //            print("failed to upload world anchor, please try agagin");
            //            indicator.GetComponent<MeshRenderer>().material.color = Color.red;
            //        }
            //    });
        }
        else if (Choice == Selection.SycnDirectly)
        {
            //tCP.InitSocket();

            indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;

            //anchorData = DownloadAnchorData(spaceId);
            if (indicator.GetComponent<MeshRenderer>().material.color != Color.green)
            {
                TextAsset asset = Resources.Load("data_anchor") as TextAsset;
                anchorData = asset.bytes;
                ImportWorldAnchor(anchorData);
            }
            else
            {
                syncOrNot = true;
                syncPlantInfor = true;
            }
            //unit test:
            //syncOrNot = true;
            //syncPlantInfor = true;
        }
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

        GameObject gameObject = GameObject.Find("RefernceSceneBuilder");
        if (gameObject != null)
        {
            indicator.GetComponent<MeshRenderer>().material.color = Color.green;
            deserializedTransferBatch.LockObject(spaceId, gameObject);
            
            syncOrNot = true;
            syncPlantInfor = true;
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
        AnchorInfo anchorNew = CreateAnchor(anchorInfo1);
        anchorNew = UpdateAnchorData(anchorInfo1.id, e.ToArray());
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






}
