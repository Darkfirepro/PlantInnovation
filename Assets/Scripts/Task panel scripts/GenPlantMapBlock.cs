using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenPlantMapBlock : MonoBehaviour
{
    public SelectPlantDropDown _dropDown;
    private GameObject[] plantList;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        plantList = _dropDown.plantSetList;
    }
}
