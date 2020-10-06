using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRefObjToCertain : MonoBehaviour
{

    public int totalSyncNum = 0;
    public GameObject cheese;
    bool trigger;
    // Start is called before the first frame update
    void Start()
    {
        trigger = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveRefObj()
    {
        if (trigger == false)
        {
            gameObject.transform.position = cheese.transform.position;
            trigger = true;
        }
        
    }

}
