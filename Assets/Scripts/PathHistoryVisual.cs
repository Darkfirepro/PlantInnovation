using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathHistoryVisual : MonoBehaviour
{
    TCPClientReceive tCP;
    public GameObject spawnIns;
    public GameObject arrowIns;
    Vector3 posOrigin = new Vector3(0, 0, 0);
    public GameObject linePath;
    List<GameObject> posList = new List<GameObject>();
    float originTime;
    List<GameObject> lineList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        tCP = GameObject.Find("NetworkTransfer").GetComponent<TCPClientReceive>();
        originTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - originTime > 3f)
        {
            originTime = Time.time;
            if (posList.Count > 1)
            {
                if (lineList.Count > 0)
                {
                    foreach (GameObject line in lineList)
                    {
                        Destroy(line);
                    }
                    lineList = new List<GameObject>();
                }
                for (int i = 0; i < posList.Count-1; i++)
                {
                    GameObject lineObject = Instantiate(linePath, posList[i].transform.parent);
                    LineRenderer _lineRender = lineObject.GetComponent<LineRenderer>();
                    _lineRender.SetPosition(0, posList[i].transform.position);
                    _lineRender.SetPosition(1, posList[i+1].transform.position);
                    lineList.Add(lineObject);
                }

            }
        }
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
            GameObject qrCode;
            if (nd.actionType == "Complete Moving") qrCode = Instantiate(arrowIns, parentAnchor.transform.GetChild(2));
            else qrCode = Instantiate(spawnIns, parentAnchor.transform.GetChild(2));
            qrCode.transform.localPosition = nd.pos;
            qrCode.transform.localRotation = nd.rot;
            List<string> listTime = new List<string>(nd.timeAction.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            qrCode.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = listTime[0] + "\n" + listTime[1];
            if (nd.devType == "QRcode")
            {
                qrCode.GetComponent<MeshRenderer>().material.color = Color.green;
                qrCode.transform.localScale = qrCode.transform.localScale * 1.3f;
            }
            else if (nd.devType == "Camera")
            {
                qrCode.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
            
            posList.Add(qrCode);
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
            lineList = new List<GameObject>();
            posList = new List<GameObject>();
        }

    }

}
