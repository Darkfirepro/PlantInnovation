using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeasureSize : MonoBehaviour
{

    public GameObject cheese1;
    public GameObject cheese2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float length = Vector3.Distance(cheese1.transform.localPosition, cheese2.transform.localPosition);
        string txt = "The length is: " + length.ToString() + " cm";
        GetComponent<TextMeshPro>().text = txt;
    }
}
