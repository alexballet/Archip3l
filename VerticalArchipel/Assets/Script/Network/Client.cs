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
    public event EventHandler<MessageConstructionBuildingEventArgs> MessageConstructionBuilding;

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
    /// <summary>
    /// To BE REWORKED
    /// </summary>
    /// <param name="message"></param>
    private void ProcessMessage(string message)
    {
        //The message must start with @ because ip sender automaticaly added
        //Message Format : @SUPPORT@ISLANDNUMBER@TYPE@ACTION@STATUS
        //Use NULL if no need
        string[] split = message.Split('@');

        //Message code utilisation
        /*  10000 board
            20000 table

            1000 island1
            2000 island2
            3000 island3
            4000 island4

            100 building
            200 trophy
            300 challenge
            400 resource
            
            10 construction
            20 upgrade
            30 transfert

            1 success
            2 failure
            3 data
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

        //Raise event
        switch(code)
        {

        }
    }
}

public class MessageConstructionBuildingEventArgs: EventArgs
{
    string island;
    string buildingType;
}
