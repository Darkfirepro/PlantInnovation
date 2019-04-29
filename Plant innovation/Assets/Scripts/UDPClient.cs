using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.UI;
//using Object = System.Object;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class UDPClient : MonoBehaviour
{
    public enum Client{PlantSet, SinglePlantDetails};
    public Client clientChoice;

    private object pObject;
    private string UDPClientIP;
    Socket socket;
    EndPoint serverEnd;
    IPEndPoint ipEnd;
    byte[] recvData = new byte[1024];
    byte[] sendData = new byte[1024];
    int recvLen = 0;


    void Start()
    {
        //our ip address of server:
        UDPClientIP = "45.32.190.167";
        //UDPClientIP = "10.1.1.123";
        UDPClientIP = UDPClientIP.Trim();
        InitSocket();
        
    }

    void InitSocket()
    {
        ipEnd = new IPEndPoint(IPAddress.Parse(UDPClientIP), 6666);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        serverEnd = (EndPoint)sender;
        //start a new thread   
        //connectThread = new Thread(new ThreadStart(SocketReceive));
        //connectThread.Start();
    }

    void SocketSend(string sendStr)
    {
        //clean up      
        sendData = new byte[1024];
        //data convert    
        sendData = Encoding.UTF8.GetBytes(sendStr);
        //send message to server 
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);

    }

    void SocketSendByte(object obj)
    {
        sendData = new byte[1024];
        string senDataJson = JsonUtility.ToJson(obj);
        
        sendData = Encoding.UTF8.GetBytes(senDataJson);
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
    }


    void SocketQuit()
    {
        SocketSend("QUIT now");

        //socket close finally   
        if (socket != null)
            socket.Close();
    }

    void OnApplicationQuit()
    {        
        SocketQuit();
    }

    void Update()
    {
        if (clientChoice == Client.PlantSet && name != "UIContainerAnchor")
        {
            GeneratePlansetSend();
        }
         
    }

    public void GeneratePlansetSend()
    {
        if (clientChoice == Client.PlantSet && name != "UIContainerAnchor")
        {
            pObject = new PlantSet
            {
                Name = name,
                rotate = transform.localRotation,
                pos = transform.localPosition,
                header = "ps"
            };
            SocketSendByte(pObject);
        }
        else if (clientChoice == Client.SinglePlantDetails)
        {
            pObject = new SingPlant
            {
                singName = transform.parent.parent.name.Split('|')[0],
                singId = int.Parse(transform.parent.parent.name.Split('|')[1]),
                param1 = transform.parent.GetChild(1).GetChild(0).GetComponent<InputField>().text,
                param2 = transform.parent.GetChild(2).GetChild(0).GetComponent<InputField>().text,
                param3 = transform.parent.GetChild(3).GetChild(0).GetComponent<InputField>().text,
                header = "pds"
            };
            SocketSendByte(pObject);
        }

    }

    private byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);
        return ms.ToArray();
    }

    private object ByteArrayToObject(byte[] arrBytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        object obj = (object)binForm.Deserialize(memStream);
        return obj;
    }

            //    for (int i = 0; i<UIsingle.childCount; i++)
            //{

            //    singPl.param1 = UIsingle.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<InputField>().text;
            //    singPl.param2 = UIsingle.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<InputField>().text;
            //    singPl.param3 = UIsingle.GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetComponent<InputField>().text;
            //    //singPlantDetail.Add(singPl);

            //    //Debug.Log("see the plant.singDct" + wholePlantDict.singDict);
            //}
}