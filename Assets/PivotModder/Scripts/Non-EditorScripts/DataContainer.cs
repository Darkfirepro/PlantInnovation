#if UNITY_EDITOR


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PivotModder
{

    [System.Serializable]
    public class ObjectsHistory : SerializableDictionary<int, UtilityServices.UndoRedoOps> { }


    public class DataContainer : MonoBehaviour
    {

        public ObjectsHistory objectsHistory;

    }
}


#endif
