using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateUI : MonoBehaviour
{
    
    public  GameObject UIContainerAnchor;
    private GameObject newPlantUI;
    public List<string> col;
    public List<int> row;

    public void Start()
    {
        col = new List<string> { "A", "B", "C", "D" };
        row = new List<int> { 1, 2, 3, 4, 5 };
    }

    public void StartLocatingPant(string plantName)
    {
        if (GameObject.Find(plantName) == null)
        {
            newPlantUI = Instantiate(UIContainerAnchor, GameObject.Find("RefernceSceneBuilder").transform, false);
            newPlantUI.name = plantName;
            newPlantUI.transform.position = transform.position;
            newPlantUI.transform.eulerAngles = transform.eulerAngles;
            PutName(plantName);
        }
        else
        {
            GameObject.Find(plantName).transform.position = transform.position;
            GameObject.Find(plantName).transform.eulerAngles = transform.eulerAngles;
        }
    }

    public void PutName(string plantName)
    {
        Transform UIContainer = newPlantUI.transform.GetChild(0);
        int p_number = 1;
        //create items of location:
        List<string> location = new List<string>();
        for (int i = 0; i < row.Count; i++)
        {
            for (int n = 0; n < col.Count; n++)
            {
                location.Add(col[n] + row[i].ToString());
            }
        }
        foreach (Transform uiWhole in UIContainer)
        {
            string trayNumS = plantName.Split('_')[0];
            int trayNum = int.Parse(trayNumS);
            int potNum = (trayNum * col.Count * row.Count) - (col.Count * row.Count) + p_number;
            uiWhole.name = plantName.ToString() + "|" + p_number.ToString();
            uiWhole.GetChild(1).GetChild(1).GetComponent<InputField>().text = plantName.ToString() + "|" + p_number.ToString();
            uiWhole.GetChild(1).GetChild(2).GetComponent<InputField>().text = trayNumS + location[p_number-1];
            uiWhole.GetChild(1).GetChild(3).GetComponent<InputField>().text = potNum.ToString();
            p_number++;
        }
    }
}
