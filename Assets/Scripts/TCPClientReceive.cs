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
    byte[] recvData = new byte[8192];
    byte[] sendData = new byte[8192];
    int recvLen;
    Thread connectThread;
    //public object objClient;
    private object oldObject;
    private bool recOrNot = false;
    public MultiplyUi mpUI;
    public bool transSwitch = false;
    public int plantSetNum;
    public byte[] anchorData = null;

    public PathHistoryVisual phv;


    public void InitSocket()
    {
        ip = IPAddress.Parse("207.148.86.204");
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

        //recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
        //print(recvStr);

        OperatingRecStr(recvData, recvLen);
    }

    public void SocketSendByte(object obj)
    {
        sendData = new byte[8192];
        string senDataJson = JsonUtility.ToJson(obj) + "<EOF>";
        sendData = Encoding.ASCII.GetBytes(senDataJson);
        serverSocket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
    }

    public void SendWorlAnchor(byte[] w)
    {
        byte[] sendData = new byte[8192];
        sendData = Combine(w, Encoding.ASCII.GetBytes("<EOF>"));
        int totalByteToSend = sendData.Length;
        int byteSend = 0;
        Debug.Log("the total byte is:" + sendData.Length);
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
            recvData = new byte[8192];
            recvLen = serverSocket.Receive(recvData);
            //print(recvLen);
            if (recvLen == 0)
            {
                SocketConnet();
                continue;
            }
            //recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
            //print(recvStr);
            OperatingRecStr(recvData, recvLen);

        }
    }

    void OperatingRecStr(byte[] recTemp, int len)
    {
        recvStr = Encoding.ASCII.GetString(recTemp, 0, len);
        tempString += recvStr;
        try
        {
            if (recvStr.Substring(recvStr.Length - 5, 5) == "<EOF>")
            {
                listRecvStr = new List<string>(tempString.Split(new string[] { "<EOF>" }, StringSplitOptions.RemoveEmptyEntries));
                foreach (string i in listRecvStr)
                {
                    print(i);
                }

                tempString = "";

            }
        }
        catch (Exception e)
        {
            print(e);
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

                    else if (header == "NaviData")
                    {
                        NavigationData nd = JsonUtility.FromJson<NavigationData>(recvFinal);
                        phv.SpawnObj(nd);
                    }
                }
                listRecvStr = new List<string>();
            }

        }
        catch (Exception e)
        {
            anchorData = Encoding.ASCII.GetBytes(listRecvStr[0]);
            Debug.Log("download anchor with tcp successfully");
            Debug.Log("the byte size is: " + listRecvStr[0].Length);
            listRecvStr = new List<string>();
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

    public static byte[] Combine(byte[] first, byte[] second)
    {
        byte[] ret = new byte[first.Length + second.Length];
        Buffer.BlockCopy(first, 0, ret, 0, first.Length);
        Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
        return ret;
    }

}