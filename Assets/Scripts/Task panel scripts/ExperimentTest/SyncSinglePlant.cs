using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Sharing;

public class SyncSinglePlant : MonoBehaviour
{

    private int retryCount = 20;
    private GameObject indicator;
    private byte[] anchorData;
    //[HideInInspector] public OldSystemSync oss;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //try
        //{
        //    oss = GameObject.FindGameObjectWithTag("OldSystemTest").GetComponent<OldSystemSync>();
        //}
        //catch (NullReferenceException)
        //{

        //}
    }

    public void StartSyncSinglePlant(byte[] data, GameObject indicator1)
    {
        
        anchorData = data;
        indicator = indicator1;
        indicator1.GetComponent<MeshRenderer>().material.color = Color.black;
        WorldAnchorTransferBatch.ImportAsync(data, OnImportComplete);
    }

    private void OnImportComplete(SerializationCompletionReason completionReason, WorldAnchorTransferBatch deserializedTransferBatch)
    {
        if (completionReason != SerializationCompletionReason.Succeeded)
        {
            Debug.Log("Failed to import: " + completionReason.ToString());
            if (retryCount > 0)
            {
                indicator.GetComponent<MeshRenderer>().material.color = Color.cyan;
                WorldAnchorTransferBatch.ImportAsync(anchorData, OnImportComplete);
            }
            return;
        }
        if (deserializedTransferBatch.GetAllIds().Length == 0)
        {
            if (retryCount > 0)
            {
                indicator.GetComponent<MeshRenderer>().material.color = Color.cyan;
                WorldAnchorTransferBatch.ImportAsync(anchorData, OnImportComplete);
            }
            return;
        }
        if (gameObject != null)
        {
            indicator.GetComponent<MeshRenderer>().material.color = Color.green;
            deserializedTransferBatch.LockObject(gameObject.name, gameObject);
            transform.parent.GetComponent<SetRefObjToCertain>().totalSyncNum++;
            gameObject.SetActive(true);
        }
        else
        {
            indicator.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
