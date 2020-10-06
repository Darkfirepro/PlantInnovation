using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vuforia;

public class ChangeButtonName : MonoBehaviour
{
    [SerializeField] private GameObject imageTargetObject;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshPro>().text = imageTargetObject.GetComponent<ImageTargetBehaviour>().TrackableName;
    }
}
