using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendCamPos : MonoBehaviour
{
    public GameObject cam;
    public bool trigger = false;
    private float timeCurrent;
    public TCPClientReceive tcp;

    // Start is called before the first frame update
    void Start()
    {
        timeCurrent = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger == true)
        {
            if (Time.time - timeCurrent > 0.33f)
            {
                GameObject anobi = GetComponent<AnchorAttachName>().anchorObject;
                Vector3 pos = anobi.transform.parent.InverseTransformPoint(cam.transform.position);
                Quaternion rot = new Quaternion(0, 0, 0, 0);
                string anchorName = anobi.transform.parent.name;

                NavigationData dataSend = new NavigationData("Camera", pos, rot, anchorName, 
                    System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "Moving to");
                tcp.SocketSendByte(dataSend);
            }
        }
    }
}
