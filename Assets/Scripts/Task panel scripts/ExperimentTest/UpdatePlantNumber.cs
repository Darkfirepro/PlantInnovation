using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdatePlantNumber : MonoBehaviour
{

    public TMP_InputField IF;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseNumber()
    {
        if (int.Parse(IF.text) < 8)
        {
            int nextNum = int.Parse(IF.text) + 1;
            IF.text = nextNum.ToString();
        }
    }

    public void DecreaseNumber()
    {
        if (int.Parse(IF.text) > 1)
        {
            int nextNum = int.Parse(IF.text) - 1;
            IF.text = nextNum.ToString();
        }
    }
}
