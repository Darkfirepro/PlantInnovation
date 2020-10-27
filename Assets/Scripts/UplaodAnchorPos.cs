using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UplaodAnchorPos : MonoBehaviour
{
    public TCPClientReceive tCP;

    // Start is called before the first frame update
    public void UploadPosServer()
    {
        GameObject anchorsParent = GameObject.FindGameObjectWithTag("AnchorParent");
        if (anchorsParent.transform.childCount > 0)
        {
            foreach (Transform child in anchorsParent.transform)
            {
                AnchorInfo ai = new AnchorInfo(child.name, child.localPosition);
                tCP.SocketSendByte(ai);
            }
        }
    }

}
