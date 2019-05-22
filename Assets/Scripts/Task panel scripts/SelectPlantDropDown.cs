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
    private string oldPlantNmae;
    private string plantString;
    private int value;

    // Start is called before the first frame update
    void Start()
    {
        oldPlantSetList = plantSetList;
        value = gameObject.GetComponent<TMP_Dropdown>().value;
        plantString = gameObject.GetComponent<TMP_Dropdown>().options[value].text;
        oldPlantNmae = plantString;
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


        value = gameObject.GetComponent<TMP_Dropdown>().value;
        plantString = gameObject.GetComponent<TMP_Dropdown>().options[value].text;
        if (plantString != oldPlantNmae)
        {
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
                    cubeTrans.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }

    public void UpdateSelectArrowLeft()
    {
        ChangeValue(0);
    }

    public void UpdateSelectArrowRight()
    {
        ChangeValue(1);
    }

    private void ChangeValue(int i)
    {
        int currentVal = gameObject.GetComponent<TMP_Dropdown>().value;

        if (i == 0)
        {
            if (currentVal - 1 >= 0)
            {
                gameObject.GetComponent<TMP_Dropdown>().value--;
            }
            else
            {
                gameObject.GetComponent<TMP_Dropdown>().value = gameObject.GetComponent<TMP_Dropdown>().options.Count - 1;
            }
        }
        else if (i == 1)
        {
            if (currentVal + 1 < gameObject.GetComponent<TMP_Dropdown>().options.Count)
            {
                gameObject.GetComponent<TMP_Dropdown>().value++;
            }
            else
            {
                gameObject.GetComponent<TMP_Dropdown>().value = 0;
            }
        }

    }

}
