using HoloToolkit.Unity;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowPlant : MonoBehaviour
{
    [HideInInspector]public GameObject PlantMapSet;
    public GameObject directionIndicator;
    public bool navigate = false;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            PlantMapSet = gameObject.transform.parent.GetChild(2).gameObject;
        }
        catch (UnityException)
        {
            print("pass here");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActOrNavi()
    {
        ShowPlant parentValue = transform.parent.parent.GetComponent<ShowPlant>();
        if (parentValue.navigate)
        {
            FindLocation();
        }
        else
        {
            ActiveOrNot();
        }
    }

    public void ActiveOrNot()
    {
        try
        {
            Transform singlePlant = GetSingPlant(name);
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
        ShowPlant parentValue = transform.parent.GetComponent<ShowPlant>();
        if (!parentValue.navigate)
        {
            foreach (Transform childCube in PlantMapSet.transform)
            {
                Transform singlePlant = GetSingPlant(childCube.name);
                if (singlePlant.gameObject.activeSelf)
                {
                    singlePlant.GetChild(0).GetChild(4).GetComponent<TCPClient>().SendSingleDetails(0);
                    singlePlant.gameObject.SetActive(false);
                    GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }

    public void FindLocation()
    {
        string direcName = name + "|Navi";
        Transform singlePlant = GetSingPlant(name);
        GameObject referenceAnchor = GameObject.Find("RefernceSceneBuilder");
        GameObject directionalIns = GameObject.Find(direcName);
        if (directionalIns != null)
        {
            return;
        }
        directionalIns = Instantiate(directionIndicator, referenceAnchor.transform, false);
        directionalIns.name = direcName;
        singlePlant.gameObject.SetActive(true);
        directionalIns.transform.position = singlePlant.position;
        directionalIns.transform.rotation = singlePlant.rotation;
        string indexLoc = transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text;
        directionalIns.GetComponent<DirectionIndicator>().coneTextContent = indexLoc;
        directionalIns.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = name.Split('|')[0] + "\n" + indexLoc;
        GetComponent<MeshRenderer>().material.color = Color.cyan;   
    }


    private Transform GetSingPlant(string _name)
    {
        string[] singlePlantNameList = _name.Split('|');
        string singleName = singlePlantNameList[0];
        int pNum = int.Parse(singlePlantNameList[1]);
        Transform singlePlant = GameObject.Find(singleName).transform.GetChild(0).GetChild(pNum - 1);
        return singlePlant;
    }

    public void DestroyDirectionPanel()
    {
        Destroy(gameObject);
    }

    public void StartNavigate()
    {
        ShowPlant parentValue = transform.parent.GetComponent<ShowPlant>();
        if (parentValue.navigate)
        {
            parentValue.navigate = false;
            transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = "Navigate?";
            gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        else
        {
            parentValue.navigate = true;
            transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = "In Navigate";
            gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
    }

}
