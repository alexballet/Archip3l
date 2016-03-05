using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Client : MonoBehaviour
{
    private UdpClient _client;
    private bool _continue;
    private Thread _thListener;

    //All events raised
    private delegate void DelegateEvent(object send, EventArgs e);
    private event EventHandler<MessageEventArgs> MessageEvent;

    public event EventHandler<MessageEventArgs> MessageBuildingConstructionEvent;
    public event EventHandler<MessageEventArgs> MessageBuildingUpgradeEvent;

    void Awake()
    {
        _client = new UdpClient();
        //_client.Connect("172.18.136.49", 1523);
        _client.Connect("192.168.1.91", 1523);
        Debug.Log("Starting client...");

        _continue = true;
        _thListener = new Thread(new ThreadStart(ThreadListener));
        _thListener.Start();
    }

    public void sendData(string dataToSend)
    {
        //Debug.Log("Sending : " + dataToSend);
        byte[] data = Encoding.Default.GetBytes(dataToSend);
        _client.Send(data, data.Length);
    }

    private void StopClient()
    {
        _continue = false;
        _client.Close();
        _thListener.Join();
    }

    private void ThreadListener()
    {
        UdpClient listener = null;

        //Secure creation of the socket
        try
        {
            listener = new UdpClient(5053);
        }
        catch
        {
            Debug.Log("Unable to establish connect to UDP 5053 port. Verify your network configuration.");
            return;
        }

        listener.Client.ReceiveTimeout = 1000;

        //Listening loop
        while (_continue)
        {
            try
            {
                IPEndPoint ip = null;
                byte[] data = listener.Receive(ref ip);

                ProcessMessage(Encoding.Default.GetString(data));
            }
            catch
            {
            }
        }

        listener.Close();
    }

    private void ProcessMessage(string message)
    {
        Debug.Log("Client processing : " + message);
        //The message must start with @ because ip sender automaticaly added
        //Message Format : @SUPPORT@ISLANDNUMBER@TYPE@ACTION@STATUS
        //Use NULL if no need
        string[] split = message.Split('@');

        this.MessageEvent = null;

        //Message code utilisation
        /*  Support
            10000 BOARD
            20000 TABLE 

            Island
            1000 1
            2000 2
            3000 3
            4000 4

            Type
            100 BUILDING
            200 TROPHY
            300 CHALLENGE
            400 RESSOURCE
            
            Action
            10 CONSTRUCTION
            20 UPGRADE
            30 TRANSFERT

            Status
            1 SUCCESS
            2 FAILURE
            3 DATA

            Add data fields
       */
        int code = 0;
        if(split[1]=="BOARD")
        {
            code += 10000;
        }
        else
        {
            code += 20000;
        }
        code += Int32.Parse(split[2]) * 1000;
        switch (split[3])
        {
            case "BUILDING":
                code += 100;
                break;
            case "TROPHY":
                code += 200;
                break;
            case "CHALLENGE":
                code += 300;
                break;
            case "RESOURCE":
                code += 400;
                break;
        }
        switch (split[4])
        {
            case "CONSTRUCTION":
                code += 10;
                break;
            case "UPGRADE":
                code += 20;
                break;
            case "TRANSFERT":
                code += 30;
                break;
        }
        switch(split[5])
        {
            case "SUCCESS":
                code += 1;
                break;
            case "FAILURE":
                code += 2;
                break;
            case "DATA":
                code += 3;
                break;
        }
        //Debug.Log(code);


        //Raise event
        switch (code)
        {
            case 21111:
                MessageEvent += MessageBuildingConstructionEvent;
                break;
            case 21121:
                MessageEvent += MessageBuildingUpgradeEvent;
                break;
        }

        //Debug.Log(MessageEvent.ToString());
        if (this.MessageEvent != null)
        {
            //Debug.Log("Client processing : raising event");
            this.MessageEvent(this, new MessageEventArgs { message = message });
        }
    }
}
public class MessageEventArgs : EventArgs
{
    public string message { get; set; }
}