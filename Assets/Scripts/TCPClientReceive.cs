using UnityEngine;
using System.Collections;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using SimpleJSON;
using UnityEngine.UI;

public class TCPClientReceive : MonoBehaviour
{
    Socket serverSocket; 
    IPAddress ip; 
    IPEndPoint ipEnd;
    string recvStr;
    string sendStr;
    byte[] recvData = new byte[1024];
    byte[] sendData = new byte[1024];
    int recvLen; 
    Thread connectThread;
    public object objClient;
    private object oldObject;
    private bool recOrNot = false;
    public MultiplyUi mpUI;

    void InitSocket()
    {
        ip = IPAddress.Parse("45.32.190.167");
        ipEnd = new IPEndPoint(ip, 6666);

        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    void SocketConnet()
    {
        if (serverSocket != null)
            serverSocket.Close();
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        print("ready to connect");
        serverSocket.Connect(ipEnd);
        recvLen = serverSocket.Receive(recvData);
        print(recvLen);
        recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
        recOrNot = true;
    }

    void SocketSend(string sendStr)
    {

        sendData = new byte[1024];

        sendData = Encoding.UTF8.GetBytes(sendStr);

        serverSocket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
    }

    void SocketSendByte(object obj)
    {
        sendData = new byte[1024];
        string senDataJson = JsonUtility.ToJson(obj);

        sendData = Encoding.UTF8.GetBytes(senDataJson);
        serverSocket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
    }

    void SocketReceive()
    {
        SocketConnet();

        while (true)
        {
            recvData = new byte[1024];
            recvLen = serverSocket.Receive(recvData);
            if (recvLen == 0)
            {
                SocketConnet();
                continue;
            }
            recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
            recOrNot = true;
            print(recvStr);
        }
    }

    void SocketQuit()
    {
        //SocketSend("ClientShutDown");
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }

        if (serverSocket != null)
            serverSocket.Close();
        recvStr = "";
        print("disconnect");
    }


    void Start()
    {
        oldObject = objClient;
        mpUI = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MultiplyUi>();
        InitSocket();
    }

    void Update()
    {
        try
        {
            if (oldObject != objClient)
            {
                SocketSendByte(objClient);
                oldObject = objClient;
            }
            if (recOrNot)
            {
                
                recOrNot = false;
                JSONNode jData = JSON.Parse(recvStr);
                string header = jData["header"];
                if (header == "ps")
                {
                    PlantSet ps = JsonUtility.FromJson<PlantSet>(recvStr);
                    GameObject plantSetWant = GameObject.Find(ps.Name);
                    if (plantSetWant != null)
                    {
                        plantSetWant.transform.localPosition = ps.pos;
                        plantSetWant.transform.localEulerAngles = ps.rotate;
                    }
                    else if (plantSetWant == null)
                    {
                        mpUI.GeneratePlantAnchor(ps.Name, ps.pos, ps.rotate);
                    }
                }

                else if (header == "pds")
                {
                    SingPlant spR = JsonUtility.FromJson<SingPlant>(recvStr);
                    print(spR.singName + "|" + spR.singId.ToString());
                    GameObject singlePlant = GameObject.Find(spR.singName + "|" + spR.singId.ToString());
                    singlePlant.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<InputField>().text = spR.param1;
                    singlePlant.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<InputField>().text = spR.param2;
                    singlePlant.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<InputField>().text = spR.param3;
                }
            }
        }
        catch (Exception e)
        {
            print("Error happens here:" + e);
        }
    }

    void OnApplicationQuit()
    {
        SocketQuit();
    }
}