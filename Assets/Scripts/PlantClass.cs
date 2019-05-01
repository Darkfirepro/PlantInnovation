using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class PlantSet
{
    public string Name;
    public Quaternion rotate;
    public Vector3 pos;
    public string header;
}

[Serializable]
public class SingPlant
{
    public int singId;
    public string singName;
    public string param1;
    public string param2;
    public string param3;
    public string header;
}


