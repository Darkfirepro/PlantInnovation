using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSendServer : MonoBehaviour
{
    private GameObject anchorSync;

    public void Start()
    {
        anchorSync = GameObject.Find("SyncAnchor");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "character")
        {
            anchorSync.GetComponent<AnchorAttachName>().anchorObject = transform.parent.GetChild(2).gameObject;
        }
    }
}
