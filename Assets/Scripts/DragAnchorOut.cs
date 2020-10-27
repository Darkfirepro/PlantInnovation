﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAnchorOut : MonoBehaviour
{
    public GameObject anchorSet;
    public GameObject anchorControl;
    public GameObject point;

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
        string Id = Guid.NewGuid().ToString();
        GameObject anchor = Instantiate(anchorSet, anchorControl.transform);
        anchor.transform.position = point.transform.position;
        anchor.name = Id;
}
}
