using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA;

public class MultiplyUi : MonoBehaviour
{
    public GameObject taskPanel;
    GameObject referenceScene;
    public GameObject planUiAnchor;
    public List<string> col;
    public List<int> row;

    public void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
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

    public void GeneratePlantAnchor(string pname, Vector3 pos, Vector3 rotate)
    {
        GameObject newPlantUI = Instantiate(planUiAnchor, GameObject.Find("RefernceSceneBuilder").transform, false);
        newPlantUI.name = pname;
        newPlantUI.transform.localPosition = pos;
        newPlantUI.transform.localEulerAngles = rotate;
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
                print(col[n] + row[i].ToString());
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


    // Update is called once per frame
    void Update()
    {

    }

}
