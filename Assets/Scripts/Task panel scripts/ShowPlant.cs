using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveOrNot()
    {
        string[] singlePlantNameList = name.Split('|');
        string singleName = singlePlantNameList[0];
        int pNum = int.Parse(singlePlantNameList[1]);
        Transform singlePlant = GameObject.Find(singleName).transform.GetChild(0).GetChild(pNum - 1);
        if (singlePlant.gameObject.activeSelf)
        {
            singlePlant.GetChild(0).GetChild(4).GetComponent<TCPClient>().SendSingleDetails(0);
            singlePlant.gameObject.SetActive(false);
            GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        else
        {
            singlePlant.gameObject.SetActive(true);
            singlePlant.GetChild(0).GetChild(4).GetComponent<TCPClient>().SendSingleDetails(1);
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
}
