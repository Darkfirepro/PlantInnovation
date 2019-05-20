using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;

public class SelectPlantDropDown : MonoBehaviour
{
    public List<string> plantSetListString;
    public GameObject[] plantSetList;
    private GameObject[] oldPlantSetList;
    public GameObject PlantMapSet;

    // Start is called before the first frame update
    void Start()
    {
        oldPlantSetList = GameObject.FindGameObjectsWithTag("PlantSet");
    }

    // Update is called once per frame
    void Update()
    {
        plantSetList = GameObject.FindGameObjectsWithTag("PlantSet");
        if (plantSetList.Length != oldPlantSetList.Length)
        {
            oldPlantSetList = plantSetList;
            plantSetListString = new List<string>();
            foreach (GameObject ps in plantSetList)
            {
                plantSetListString.Add(ps.name);      
            }
            gameObject.GetComponent<TMP_Dropdown>().ClearOptions();
            gameObject.GetComponent<TMP_Dropdown>().AddOptions(plantSetListString);
        }
    }

    public void UpdateCubeSet()
    {
        int value = gameObject.GetComponent<TMP_Dropdown>().value;
        string plantString = gameObject.GetComponent<TMP_Dropdown>().options[value].text;
        Transform plantUIContainer = GameObject.Find(plantString).transform.GetChild(0);
        int countChild = plantUIContainer.childCount;
        for (int count = 0; count < countChild; count++)
        {
            Transform cubeTrans = PlantMapSet.transform.GetChild(count);
            cubeTrans.name = plantUIContainer.GetChild(count).name + "|Map";
            string location = plantUIContainer.GetChild(count).GetChild(1).GetChild(2).GetComponent<InputField>().text;
            string potNum = plantUIContainer.GetChild(count).GetChild(1).GetChild(3).GetComponent<InputField>().text;
            cubeTrans.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = location + "\n" + potNum;
            if (plantUIContainer.GetChild(count).gameObject.activeSelf)
            {
                cubeTrans.GetComponent<MeshRenderer>().material.color = Color.green;
            }
            else
            {
                cubeTrans.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
        }
    }

}
