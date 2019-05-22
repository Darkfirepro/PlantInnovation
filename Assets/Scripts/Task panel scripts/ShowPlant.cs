using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlant : MonoBehaviour
{
    public GameObject PlantMapSet;

    // Start is called before the first frame update
    void Start()
    {
        PlantMapSet = gameObject.transform.parent.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveOrNot()
    {
        try
        {
            string[] singlePlantNameList = name.Split('|');
            string singleName = singlePlantNameList[0];
            int pNum = int.Parse(singlePlantNameList[1]);
            Transform singlePlant = GameObject.Find(singleName).transform.GetChild(0).GetChild(pNum - 1);
            if (singlePlant.gameObject.activeSelf)
            {
                singlePlant.GetChild(0).GetChild(4).GetComponent<TCPClient>().SendSingleDetails(0);
                singlePlant.gameObject.SetActive(false);
                GetComponent<MeshRenderer>().material.color = Color.red;
            }
            else
            {
                singlePlant.gameObject.SetActive(true);
                singlePlant.GetChild(0).GetChild(4).GetComponent<TCPClient>().SendSingleDetails(1);
                GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }
        catch (Exception e)
        {
            print("Error happens here:  " + e.ToString());
        }

    }


    public void ActOrDeactAll()
    {
        foreach (Transform childCube in PlantMapSet.transform)
        {
            childCube.GetComponent<ShowPlant>().ActiveOrNot();
        }
    }
}
