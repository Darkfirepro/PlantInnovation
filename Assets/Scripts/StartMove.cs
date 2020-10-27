using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMove : MonoBehaviour
{
    public GameObject plantTray;
    public TCPClientReceive tcp;

    public void MovingPlantTray()
    {
        GameObject anchorNeed = GameObject.FindGameObjectWithTag("SyncAnchor");
        GameObject anchorObejct = anchorNeed.GetComponent<AnchorAttachName>().anchorObject;
        if (anchorObejct != null)
        {
            string anchorName = anchorObejct.transform.parent.name;

            plantTray.transform.position = transform.position;
            plantTray.transform.rotation = transform.rotation;
            plantTray.transform.parent = anchorObejct.transform;


            NavigationData dataSend = new NavigationData("QRcode", plantTray.transform.localPosition, plantTray.transform.position, plantTray.transform.localRotation,
                anchorName, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "Start Moving");
            tcp.SocketSendByte(dataSend);
            Debug.Log("send navi data to server");
            anchorNeed.GetComponent<SendCamPos>().trigger = true;
        }


    }

    public void PlacingPlantTray()
    {
        GameObject anchorNeed = GameObject.FindGameObjectWithTag("SyncAnchor");
        GameObject anchorObejct = anchorNeed.GetComponent<AnchorAttachName>().anchorObject;
        if (anchorObejct != null)
        {
            string anchorName = anchorObejct.transform.parent.name;

            plantTray.transform.position = transform.position;
            plantTray.transform.rotation = transform.rotation;
            plantTray.transform.parent = anchorObejct.transform;


            NavigationData dataSend = new NavigationData("QRcode", plantTray.transform.localPosition, plantTray.transform.position, plantTray.transform.localRotation,
                anchorName, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "Complete Moving");
            tcp.SocketSendByte(dataSend);
            Debug.Log("send navi data to server");
            anchorNeed.GetComponent<SendCamPos>().trigger = false;
        }

    }
}
