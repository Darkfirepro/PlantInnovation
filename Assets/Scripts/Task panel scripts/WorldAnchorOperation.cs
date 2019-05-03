using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Sharing;
using Vuforia;

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


    public void PressAndGenerate()
    {
        if (Choice == Selection.CreateNew)
        {
            ParentAnchor = GameObject.Find("RefernceSceneBuilder");
            WorldAnchor wa = ParentAnchor.AddComponent<WorldAnchor>();
            WorldAnchorTransferBatch watb = new WorldAnchorTransferBatch();
            spaceId = GameObject.FindGameObjectWithTag("SpaceNameObject").GetComponent<ImageTargetBehaviour>().TrackableName;
            watb.AddWorldAnchor(spaceId, wa);
            tCP.InitSocket();
            imgSet.SetActive(true);
            exportedData = new byte[0];
            e = new List<byte>();
            indicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
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
                            header = "World Anchor",
                            spaceName = spaceId,
                            data = e.ToArray()
                        };
                        tCP.SendWorlAnchor(wat);
                        indicator.GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    else
                    {
                        print("failed to upload world anchor, please try agagin");
                        indicator.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                });
        }
        else if (Choice == Selection.SycnDirectly)
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tCP = GameObject.Find("NetworkTransfer").GetComponent<TCPClientReceive>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
