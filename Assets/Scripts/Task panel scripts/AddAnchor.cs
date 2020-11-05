using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Sharing;

public class AddAnchor : MonoBehaviour
{
    AnchorAdd an;
    public TextMeshPro anchorNum;

    public void Start()
    {
        anchorNum = GameObject.FindGameObjectWithTag("AnchorNumber").GetComponent<TextMeshPro>();
        PutAnchorOnObject();
    }

    public void PutAnchorOnObject()
    {
        if (transform.parent.gameObject.GetComponent<WorldAnchor>() == null)
        {
            WorldAnchor wa = transform.parent.gameObject.AddComponent<WorldAnchor>();
        }
    }

    public void RemoveAnchorObject()
    {
        if (transform.parent.gameObject.GetComponent<WorldAnchor>() != null)
        {
            Destroy(transform.parent.gameObject.GetComponent<WorldAnchor>());
        }
    }

    public void DeleteAnchorObject()
    {
        anchorNum.text = (int.Parse(anchorNum.text) - 1).ToString();
        Destroy(transform.parent.gameObject);
    }
}
