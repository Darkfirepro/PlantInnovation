using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Sharing;

public class AddAnchor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PutAnchorOnObject()
    {
        WorldAnchor wa = transform.parent.gameObject.AddComponent<WorldAnchor>();
        GameObject syncAnchor = GameObject.Find("SyncAnchor");
        syncAnchor.GetComponent<WorldAnchorOperation>().watb.AddWorldAnchor(transform.parent.name, wa);
    }
}
