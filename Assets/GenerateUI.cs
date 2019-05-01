using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateUI : MonoBehaviour
{
    public GameObject UIContainerAnchor;
    private GameObject newPlantUI;

    public void StartLocatingPant(string plantName)
    {
        if (GameObject.Find(plantName) == null)
        {
            newPlantUI = Instantiate(UIContainerAnchor, GameObject.Find("RefernceSceneBuilder").transform, false);
            newPlantUI.name = plantName;
            newPlantUI.transform.position = transform.position;
            newPlantUI.transform.rotation = transform.rotation;
            PutName(plantName);
        }
        else
        {
            GameObject.Find(plantName).transform.position = transform.position;
            GameObject.Find(plantName).transform.rotation = transform.rotation;
        }
    }

    public void PutName(string plantName)
    {
        Transform UIContainer = newPlantUI.transform.GetChild(0);
        int p_number = 1;
        foreach (Transform uiWhole in UIContainer)
        {
            uiWhole.name = plantName.ToString() + "|" + p_number.ToString();
            uiWhole.GetChild(1).GetChild(3).GetComponent<InputField>().text = plantName.ToString() + "|" + p_number.ToString();
            p_number++;
        }
    }
}
