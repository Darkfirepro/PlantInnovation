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

    public void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        taskPanel.SetActive(false);
    }

    public void AddWorldAnchor()
    {
        referenceScene = GameObject.Find("RefernceSceneBuilder");
        referenceScene.AddComponent<WorldAnchor>();
    }

    public void GeneratePlantAnchor(string pname, Vector3 pos, Quaternion rotate)
    {
        GameObject newPlantUI = Instantiate(planUiAnchor, GameObject.Find("RefernceSceneBuilder").transform, false);
        newPlantUI.name = pname;
        newPlantUI.transform.localPosition = pos;
        newPlantUI.transform.localRotation = rotate;
        //put each name of each plant:
        Transform UIContainer = newPlantUI.transform.GetChild(0);
        int p_number = 1;
        foreach (Transform uiWhole in UIContainer)
        {
            uiWhole.name = pname + "|" + p_number.ToString();
            uiWhole.GetChild(1).GetChild(3).GetComponent<InputField>().text = pname + "|" + p_number.ToString();
            p_number++;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

}
