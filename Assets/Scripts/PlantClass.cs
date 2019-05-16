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
    public Vector3 rotate;
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

[Serializable]
public class SendMsg
{
    public string header;
    public string msg;
    public SendMsg(string content)
    {
        header = "msg";
        msg = content;
    }
}

[Serializable]
public class WorldAnchorTrans
{
    public string header;
    public string spaceName;
    public byte[] data;
}

[Serializable]
public class PlantInfo
{
    public List<Plant> plantList;
    public PlantInfo(List<Plant> infor)
    {
        this.plantList = infor;
    }
}

[Serializable]
public class Plant
{
    public string plantId = "";
    public string height = "";
    public string emergenceDate = "";
    public string leafNumber = "";

    public Plant()
    { }

    public Plant(string id, string h, string data, string lfno)
    {
        plantId = id;
        height = h;
        emergenceDate = data;
        leafNumber = lfno;
    }
}


[Serializable]
public class AnchorInfo
{
    public string id;
    public string hash;
    public int size;
    public PlantInfo info;
    public AnchorInfo(PlantInfo info)
    {
        this.info = info;
    }
}

[Serializable]
class AnchorListResponse
{
    public AnchorInfo[] data;
}


