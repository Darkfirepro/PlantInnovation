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

public class UDpDetectConn : MonoBehaviour
{
    private PlantSet ploc;
    public string recStr;
    private string UDPClientIP;
    string str = "hello, here is client";
    Socket socket;
    EndPoint serverEnd;
    IPEndPoint ipEnd;
    byte[] recvData = new byte[1024];
    byte[] sendData = new byte[1024];
    int recvLen = 0;
    Thread connectThread;



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
        Debug.Log("waiting for connection");
        SocketSend(str);
        //start a new thread   
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
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


    //recieve message from server:   
    void SocketReceive()
    {
        while (true)
        {

            recvData = new byte[1024];

            try
            {
                recvLen = socket.ReceiveFrom(recvData, ref serverEnd);
                Debug.Log("the length of this packet is:" + recvLen);
            }
            catch (Exception e)
            {
                Debug.Log("error happens: " + e);
            }

            if (recvLen < 25)
            {
                recStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
            }
            if (recvLen > 50)
            {
                string recStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
                ploc = JsonUtility.FromJson<PlantSet>(recStr);
            }
        }
    }


    //connect close 
    void SocketQuit()
    {
        //threading close 
        SocketSend("QUIT now");
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }

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

    }

}