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
using TMPro;

public class WorldAnchorOperation : MonoBehaviour
{

    public enum Selection { CreateNew, SycnDirectly};
    public Selection Choice;
    [HideInInspector]
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
    public TMP_Dropdown dropDownMenu;
    public string dropValue;

    byte[] anchorDataRecv;

    //public List<AnchorAdd> anchorList = new List<AnchorAdd>();


    // Start is called before the first frame update
    void Start()
    {
        tCP = GameObject.Find("NetworkTransfer").GetComponent<TCPClientReceive>();
        spaceId = GameObject.FindGameObjectWithTag("SpaceNameObject").GetComponent<ImageTargetBehaviour>().TrackableName;
        dropValue = dropDownMenu.options[dropDownMenu.value].text;
        bytePath = Path.Combine(Application.persistentDataPath, dropValue);
        indicator.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    // Update is called once per frame
    public void DropDownValueChange()
    {
        dropValue = dropDownMenu.options[dropDownMenu.value].text;
        bytePath = Path.Combine(Application.persistentDataPath, dropValue);
    }

    public void PressAndGenerate()
    {
        if ((anchorSetControl.transform.childCount > 0) & (indicator.GetComponent<MeshRenderer>().material.color != Color.yellow))
        {
            WorldAnchorTransferBatch watb = new WorldAnchorTransferBatch();

            foreach (Transform a in anchorSetControl.transform)
            {
                watb.AddWorldAnchor(a.name, a.GetComponent<WorldAnchor>());
            }
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

    public void ImportAnchorWithReadingBytes()
    {
        if (File.Exists(bytePath) & indicator.GetComponent<MeshRenderer>().material.color != Color.yellow)
        {
            tCP.SocketSendByte(new SendMsg("start to read world anchor from local anchor data"));
            anchorDataRecv = File.ReadAllBytes(bytePath);
            tCP.SocketSendByte(new SendMsg("start to import world anchor from local anchor data"));
            indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
            WorldAnchorTransferBatch.ImportAsync(anchorDataRecv, OnImportComplete);
        }
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
                    if (anchorObj.GetComponent<WorldAnchor>() != null) Destroy(anchorObj.GetComponent<WorldAnchor>());
                    deserializedTransferBatch.LockObject(anid, anchorObj);
                }
                else
                {
                    GameObject ins = Instantiate(anchorObjPrefab, anchorSetControl.transform);
                    ins.name = anid;
                    if (ins.GetComponent<WorldAnchor>() != null) Destroy(ins.GetComponent<WorldAnchor>());
                    deserializedTransferBatch.LockObject(anid, ins);
                }
            }
            indicator.GetComponent<MeshRenderer>().material.color = Color.green;
            GameObject.FindGameObjectWithTag("AnchorNumber").GetComponent<TextMeshPro>().text = anchorSetControl.transform.childCount.ToString();
        }
        else
        {
            Debug.Log("Failed to find object for anchor id: " + spaceId);
            indicator.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
