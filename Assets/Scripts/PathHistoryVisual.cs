using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathHistoryVisual : MonoBehaviour
{
    TCPClientReceive tCP;
    public GameObject spawnIns;

    // Start is called before the first frame update
    void Start()
    {
        tCP = GameObject.Find("NetworkTransfer").GetComponent<TCPClientReceive>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SyncPathLocation()
    {
        tCP.SocketSendByte(new SendMsg("SyncLocation"));
    }

    public void SpawnObj(NavigationData nd)
    {
        GameObject parentAnchor = GameObject.Find(nd.anchorName);
        if (parentAnchor == null)
        {
            tCP.SocketSendByte(new SendMsg("can't find correpsonding anchor object"));
            return;
        }
        tCP.SocketSendByte(new SendMsg("start to spawn object location"));
        GameObject qrCode = Instantiate(spawnIns, parentAnchor.transform.GetChild(2));
        qrCode.transform.localPosition = nd.pos;
        qrCode.transform.localEulerAngles = nd.pos;
        qrCode.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Time: " + nd.timeAction + "\nAction: " + nd.actionType;
        if (nd.devType == "QRcode")
        {
            qrCode.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else if (nd.devType == "Camera")
        {
            qrCode.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }

    }

    public void DeletePathObject()
    {
        GameObject[] path_list = GameObject.FindGameObjectsWithTag("path_obj");
        if (path_list.Length != 0)
        {
            foreach (GameObject go in path_list)
            {
                Destroy(go);
            }
        }

    }

}
