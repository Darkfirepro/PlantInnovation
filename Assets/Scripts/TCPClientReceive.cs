using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using SimpleJSON;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.XR.WSA;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class TCPClientReceive : MonoBehaviour
{
    Socket serverSocket;
    IPAddress ip;
    IPEndPoint ipEnd;
    string recvStr;
    string sendStr;
    public List<string> listRecvStr;
    private List<string> oldListRecStr;
    private string tempString = "";
    byte[] recvData = new byte[1024];
    byte[] sendData = new byte[1024];
    int recvLen;
    Thread connectThread;
    //public object objClient;
    private object oldObject;
    private bool recOrNot = false;
    public MultiplyUi mpUI;
    public bool transSwitch = false;
    public int plantSetNum;

    public void InitSocket()
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
        //print("ready to connect");
        serverSocket.Connect(ipEnd);

        recvLen = serverSocket.Receive(recvData);
        //print(recvLen);
        recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
        //print(recvStr);

        OperatingRecStr(recvStr);
    }

    public void SocketSendByte(object obj)
    {
        sendData = new byte[1024];
        string senDataJson = JsonUtility.ToJson(obj) + "<EOF>";
        sendData = Encoding.UTF8.GetBytes(senDataJson);
        serverSocket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
    }

    public void SendWorlAnchor(WorldAnchorTrans w)
    {
        byte[] sendData = new byte[1024];
        //string senDataJson = JsonUtility.ToJson(w) + "<EOF>";
        string senDataJson = JsonConvert.SerializeObject(w) + "<EOF>";
        //sendData = ObjectToByteArray(w);
        sendData = Encoding.UTF8.GetBytes(senDataJson);

        int totalByteToSend = sendData.Length;
        int byteSend = 0;
        print("the total byte is:" + sendData.Length);
        while (byteSend < totalByteToSend)
        {
            byteSend += serverSocket.Send(sendData, byteSend, totalByteToSend - byteSend, SocketFlags.None);
        }

        //serverSocket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
    }

    void SocketReceive()
    {
        SocketConnet();

        while (true)
        {
            recvData = new byte[1024];
            recvLen = serverSocket.Receive(recvData);
            //print(recvLen);
            if (recvLen == 0)
            {
                SocketConnet();
                continue;
            }
            recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
            //print(recvStr);
            OperatingRecStr(recvStr);
          
        }
    }

    void OperatingRecStr(string recTemp)
    {
        tempString += recTemp;
        try
        {
            if (recTemp.Substring(recTemp.Length - 5, 5) == "<EOF>")
            {
                listRecvStr = new List<string>(tempString.Split(new string[] { "<EOF>" }, StringSplitOptions.RemoveEmptyEntries));
                foreach (string i in listRecvStr)
                {
                    print(i);
                }
                
                tempString = "";
                
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            print("Make sure your server send correct objects!");
        }

    }

    void SocketQuit()
    {
        recvStr = "";
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }

        if (serverSocket != null)
            serverSocket.Close();
        print("disconnect");
    }

    void Start()
    {
        //oldObject = objClient;
        
        mpUI = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<MultiplyUi>();
        InitSocket();
    }

    void Update()
    {
        try
        {
            if (listRecvStr.Count != 0)
            {
                //oldListRecStr = listRecvStr;
                //recOrNot = false;
                foreach (string recvFinal in listRecvStr)
                {
                    //print(recvFinal + count_plantSet.ToString());
                    JSONNode jData = JSON.Parse(recvFinal);
                    //print(jData);
                    string header = jData["header"];
                    if (header == "ps")
                    {
                        PlantSet ps = JsonUtility.FromJson<PlantSet>(recvFinal);
                        //PlantSet ps = JsonConvert.DeserializeObject<PlantSet>(recvFinal);
                        GameObject plantSetWant = GameObject.Find(ps.Name);
                        mpUI.GeneratePlantAnchor(ps.Name, ps.pos, ps.rotate, true);
                    }

                    else if (header == "pds")
                    {
                        GameObject singlePlant = FindSinglePlant(jData["singName"], jData["singId"]);
                        UpdateSingPlant(singlePlant, jData, true);
                    }

                    else if (header == "pds_sync")
                    {
                        GameObject singlePlant = FindSinglePlant(jData["singName"], jData["singId"]);
                        UpdateSingPlant(singlePlant, jData, false);
                    }

                    else if (header == "PN")
                    {
                        plantSetNum = int.Parse(jData["PlantNumber"]);
                    }
                }
                listRecvStr = new List<string>();
            }

        }
        catch (Exception e)
        {
            print("Error happens here:" + e);
        }
    }

    public GameObject FindSinglePlant(string plantName, int pNum)
    {
        GameObject uiWhole = GameObject.Find(plantName).transform.GetChild(0).GetChild(pNum - 1).gameObject;
        return uiWhole;
    }

    public void UpdateSingPlant(GameObject singlePlant, JSONNode jData, bool showOrNot)
    {
        //print(jData.ToString());
        singlePlant.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<InputField>().text = jData["param1"];
        singlePlant.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<InputField>().text = jData["param2"];
        singlePlant.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<InputField>().text = jData["param3"];
        if (int.Parse(jData["showPlant"]) == 1)
        {
            singlePlant.SetActive(true);
            singlePlant.transform.GetChild(0).gameObject.SetActive(showOrNot);
            if (singlePlant.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<InputField>().text == "1")
            {
                singlePlant.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        else if (int.Parse(jData["showPlant"]) == 0)
        {
            print("the showplant or not is 0 here");
            singlePlant.SetActive(false);
        }
    }

    void OnApplicationQuit()
    {
        SocketQuit();
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

}