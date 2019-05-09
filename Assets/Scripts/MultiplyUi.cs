using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA;

public class MultiplyUi : MonoBehaviour
{
    public GameObject taskPanel;
    GameObject referenceScene;
    public GameObject imgSet;
    public GameObject planUiAnchor;
    public List<string> col;
    public List<int> row;

    public void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        //imgSet.SetActive(false);
        taskPanel.SetActive(false);
        col = new List<string> { "A", "B", "C", "D" };
        row = new List<int> { 1, 2, 3, 4, 5 };
    }

    public void AddWorldAnchor()
    {
        referenceScene = GameObject.Find("RefernceSceneBuilder");
        referenceScene.AddComponent<WorldAnchor>();
    }

    public void ShutDownGame()
    {
        Application.Quit();
    }

    public void GeneratePlantAnchor(string pname, Vector3 pos, Vector3 rotate, bool localOrNot)
    {
        if (GameObject.Find(pname) != null)
        {
            GameObject existPlantUI = GameObject.Find(pname);
            //delete world anchor:
            //Destroy(existPlantUI.GetComponent<WorldAnchor>());
            if (localOrNot == false)
            {
                existPlantUI.transform.position = pos;
                existPlantUI.transform.eulerAngles = rotate;
            }
            else
            {
                existPlantUI.transform.localPosition = pos;
                existPlantUI.transform.localEulerAngles = rotate;
            }
            //add world anchor:
            //existPlantUI.AddComponent<WorldAnchor>();
        }
        else
        {
            GameObject newPlantUI = Instantiate(planUiAnchor, GameObject.Find("RefernceSceneBuilder").transform, false);
            //delet world anchor:
            //Destroy(newPlantUI.GetComponent<WorldAnchor>());
            newPlantUI.name = pname;
            if (localOrNot == false)
            {
                newPlantUI.transform.position = pos;
                newPlantUI.transform.eulerAngles = rotate;
            }
            else
            {
                newPlantUI.transform.localPosition = pos;
                newPlantUI.transform.localEulerAngles = rotate;
            }
            newPlantUI.transform.localPosition = pos;
            newPlantUI.transform.localEulerAngles = rotate;
            //add world anchor:
            //newPlantUI.AddComponent<WorldAnchor>();
            //put each name of each plant:
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
                string trayNumS = pname.Split('_')[0];
                int trayNum = int.Parse(trayNumS);
                int potNum = (trayNum * col.Count * row.Count) - (col.Count * row.Count) + p_number;
                uiWhole.name = pname.ToString() + "|" + p_number.ToString();
                uiWhole.GetChild(1).GetChild(1).GetComponent<InputField>().text = pname.ToString() + "|" + p_number.ToString();
                uiWhole.GetChild(1).GetChild(2).GetComponent<InputField>().text = trayNumS + location[p_number - 1];
                uiWhole.GetChild(1).GetChild(3).GetComponent<InputField>().text = potNum.ToString();
                p_number++;
            }
        }

    }


    // Update is called once per frame
    void Update()
    {

    }

}
