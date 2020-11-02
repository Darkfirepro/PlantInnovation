using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnchorPlaceIndicator : MonoBehaviour
{
    public Image indicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject _camera = GetComponent<SendCamPos>().cam;
        GameObject _anchor = GetComponent<AnchorAttachName>().anchorObject;
        if (_anchor == null)
        {
            indicator.color = Color.white;
            return;
        }
        if (Vector3.Distance(_camera.transform.position, _anchor.transform.position) < 3.0f)
        {
            indicator.color = Color.red;
        }
        else
        {
            indicator.color = Color.green;
        }
    }
}
