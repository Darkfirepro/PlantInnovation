using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class GenerateAllImageTarget : MonoBehaviour
{

    public GameObject imageTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        StateManager sm = TrackerManager.Instance.GetStateManager();
        IEnumerable<TrackableBehaviour> tbs = sm.GetActiveTrackableBehaviours();
        foreach (TrackableBehaviour tb in tbs)
        {
            string tarName = tb.TrackableName;
            if (tarName != "CSIRO_Lab")
            {
                GameObject newTarget = Instantiate(imageTarget, gameObject.transform, false);
                //newTarget.GetComponent<ImageTargetBehaviour>() = tb.Trackable;
            }
            

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
