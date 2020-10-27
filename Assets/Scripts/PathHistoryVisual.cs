using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathHistoryVisual : MonoBehaviour
{
    TCPClientReceive tCP;
    public GameObject spawnIns;
    Vector3 posOrigin = new Vector3(0, 0, 0);
    public GameObject linePath;
    List<Vector3> posList = new List<Vector3>();


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

        if (posOrigin == new Vector3(0, 0, 0))
        {
            posOrigin = nd.posWorld;
        }
        if ((Vector3.Distance(nd.posWorld, posOrigin) > 0.5f) || (nd.devType == "QRcode"))
        {
            tCP.SocketSendByte(new SendMsg("start to spawn object location"));
            GameObject qrCode = Instantiate(spawnIns, parentAnchor.transform.GetChild(2));
            qrCode.transform.localPosition = nd.pos;
            qrCode.transform.localRotation = nd.rot;
            qrCode.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Time: " + nd.timeAction + "\nAction: " + nd.actionType;
            if (nd.devType == "QRcode")
            {
                qrCode.GetComponent<MeshRenderer>().material.color = Color.green;
            }
            else if (nd.devType == "Camera")
            {
                qrCode.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
            
            //set line render:
            if (posList.Count > 0)
            {
                GameObject lineObject = Instantiate(linePath, parentAnchor.transform.GetChild(2));
                LineRenderer _lineRender = lineObject.GetComponent<LineRenderer>();
                _lineRender.SetPosition(0, posList[posList.Count - 1]);
                _lineRender.SetPosition(1, qrCode.transform.position);
            }
            posList.Add(qrCode.transform.position);
            posOrigin = nd.posWorld;
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
