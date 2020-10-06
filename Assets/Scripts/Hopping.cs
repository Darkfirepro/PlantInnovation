using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hopping : MonoBehaviour
{
    public GameObject mainObj;
    public List<Transform> anchorList;
    public TextMeshProUGUI textTip;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform anchor in anchorList)
        {
            if (anchor.gameObject.activeSelf == true)
            {
                float dist = Vector3.Distance(mainObj.transform.position, anchor.position);
                if (dist < 3.0f)
                {
                    transform.parent = anchor;
                    textTip.text = "This tray is currently set to Anchor: \n" + anchor.name + "\nThe distance is: \n" + dist.ToString();
                    break;
                }
                else
                {
                    textTip.text = "This plant is out of the range: \n" + dist.ToString() + "\nCan't find any reasonable anchor";
                }
            }

        }
    }
}
