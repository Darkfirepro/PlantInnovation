﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.WSA;

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
    public int showPlant;
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
public class AnchorRequire
{
    public string header;
    public string id;
    public AnchorRequire(string ids)
    {
        header = "AnchorRequire";
        id = ids;
    }
}

[Serializable]
public class NavigationData
{
    public string header;
    public string devType;
    public Vector3 pos;
    public Quaternion rot;
    public string anchorName;
    public string timeAction;
    public string actionType;

    public NavigationData(string devT, Vector3 pos1, Quaternion rot1, string anchorName1, string timeAction1, string actionType1)
    {
        header = "NaviData";
        devType = devT;
        pos = pos1;
        rot = rot1;
        anchorName = anchorName1;
        timeAction = timeAction1;
        actionType = actionType1;
    }
}

[Serializable]
public class AnchorInfo
{
    public string header;
    public string anchorName;
    public Vector3 anchorPos;

    public AnchorInfo(string _name, Vector3 _pos)
    {
        header = "AnchorInfo";
        anchorName = _name;
        anchorPos = _pos;
    }
}

[Serializable]
public class AnchorAdd
{
    public string anchorName;
    public WorldAnchor anchor;

    public AnchorAdd(string _name, WorldAnchor _anchor)
    {
        anchorName = _name;
        anchor = _anchor;
    }
}

[Serializable]
public class TimeCost
{
    public string header;
    public string type;
    public string content;
    public TimeCost(string anchorType, string info)
    {
        header = "TimeCost";
        type = anchorType;
        content = info;
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
public class LatencyTest
{
    public string header;
    public string waStart;
    public string waComplete;
    public string socketStart;
    public string socketComplete;
    public string anchorNumber;
    public string latencyType;
    public LatencyTest(string num, string startWa, string completeWa, string startSocket, string completeSocket, string _type)
    {
        header = "LatencyTest";
        waStart = startWa;
        waComplete = completeWa;
        socketStart = startSocket;
        socketComplete = completeSocket;
        anchorNumber = num;
        latencyType = _type;
    }
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
class AnchorListResponse
{
    public AnchorInfo[] data;
}


