using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenMultiBlock : MonoBehaviour
{
    public GameObject _plantBlock;
    public int row = 4;
    public int height = 5;
    public float hGap = 0.05f, vGap = 0.03f;
    private Vector3 newVec;

    // Start is called before the first frame update
    void Start()
    {
        for (int h = 0; h < height; h++)
        {
            for (int r = 0; r < row; r++)
            {
                GameObject _cube = Instantiate(_plantBlock, transform, false);
                newVec.x = _plantBlock.transform.localPosition.x + r * hGap;
                newVec.y = _plantBlock.transform.localPosition.y + h * vGap;
                newVec.z = _plantBlock.transform.localPosition.z;
                print("x is :" + newVec.x.ToString());
                print("y is :" + newVec.y.ToString());
                _cube.transform.localPosition = newVec;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
