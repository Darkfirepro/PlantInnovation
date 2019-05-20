using UnityEngine;
using UnityEngine.UI;

public class TCPClient : MonoBehaviour
{
    public enum Client { PlantSet, SinglePlantDetails };
    public Client clientChoice;

    private Vector3 oldPos = Vector3.zero;
    private object pObject;
    public TCPClientReceive tCP;
    private bool UIexist = false;


    // Use this for initialization
    void Start()
    {
        //InitSocket();
        tCP = GameObject.Find("NetworkTransfer").GetComponent<TCPClientReceive>();
    }


    // Update is called once per frame
    void Update()
    {
        if (clientChoice == Client.PlantSet && oldPos != transform.localPosition && UIexist == true)
        {
            //GeneratePlansetSend();
            oldPos = transform.localPosition;
        }
    }

    public void GeneratePlansetSend()
    {
        if (clientChoice == Client.PlantSet && name != "UIContainerAnchor")
        {
            pObject = new PlantSet
            {
                Name = name,
                rotate = transform.localEulerAngles,
                pos = transform.localPosition,
                header = "ps"
            };
            //tCP.objClient = pObject;

            tCP.SocketSendByte(pObject);

            //tCP.SendMessage(JsonUtility.ToJson(pObject));
        }
        else if (clientChoice == Client.SinglePlantDetails)
        {
            SendSingleDetails(1);
        }

    }

    public void SendSingleDetails(int i)
    {
        pObject = new SingPlant
        {
            singName = transform.parent.parent.name.Split('|')[0],
            singId = int.Parse(transform.parent.parent.name.Split('|')[1]),
            param1 = transform.parent.GetChild(1).GetChild(0).GetComponent<InputField>().text,
            param2 = transform.parent.GetChild(2).GetChild(0).GetComponent<InputField>().text,
            param3 = transform.parent.GetChild(3).GetChild(0).GetComponent<InputField>().text,
            showPlant = i,
            header = "pds"
        };
        //tCP.objClient = pObject;

        tCP.SocketSendByte(pObject);

        //tCP.SendMessage(JsonUtility.ToJson(pObject));

        transform.parent.gameObject.SetActive(false);
    }

}