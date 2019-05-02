using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleSet : MonoBehaviour
{
    public GameObject UIWhole;
    public int row = 5;
    public int height = 4;
    public float gap = 0.08f;
    public float floating;
    public string plantVar1 = "Height";
    public string plantVar2 = "Seeds number";
    public string plantVar3 = "Spike number";

    Vector3 newV;


    // Start is called before the first frame update
    void Awake()
    {
        //set defualt names of the plant parameters:
        UIWhole.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = plantVar1;
        UIWhole.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>().text = plantVar2;
        UIWhole.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>().text = plantVar3;
        int count = 1;
        for (int heightNum = 0; heightNum < height; heightNum++)
        {
            for (int num = 0; num < row; num++)
            {
                if (count < row * height)
                {
                    GameObject uiWholeD = Instantiate(UIWhole, transform, false);
                    newV.x = UIWhole.transform.localPosition.x + num * gap - gap * (row-1);
                    newV.y = UIWhole.transform.localPosition.y + 0.3f;
                    newV.z = UIWhole.transform.localPosition.z + heightNum * gap - gap * (height-1);
                    uiWholeD.transform.localPosition = newV;
                    uiWholeD.transform.GetChild(0).gameObject.SetActive(false);
                    count++;
                }
            }
        }
        UIWhole.transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
