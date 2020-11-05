using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DragAnchorOut : MonoBehaviour
{
    public GameObject anchorSet;
    public GameObject anchorControl;
    public GameObject point;
    public TextMeshPro anchorNum;
    public GameObject placeIndicator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DragAnchor()
    {
        if (placeIndicator.GetComponent<MeshRenderer>().material.color == Color.green)
        {
            anchorNum.text = (int.Parse(anchorNum.text) + 1).ToString();
            string Id = Guid.NewGuid().ToString();
            GameObject anchor = Instantiate(anchorSet, anchorControl.transform);
            anchor.transform.position = point.transform.position;
            anchor.name = Id;
            GameObject.FindGameObjectWithTag("SyncAnchor").GetComponent<AnchorAttachName>().anchorObject = anchor;
        }
    }
}
