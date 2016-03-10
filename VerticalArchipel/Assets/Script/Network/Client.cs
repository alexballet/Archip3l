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

    private int sendingPort = 1523;
    private int listeningPort = 5054;
    private string serverIP = "192.168.1.91";

    //All events raised
    private delegate void DelegateEvent(object send, EventArgs e);
    private event EventHandler<MessageEventArgs> MessageEvent;

    public event EventHandler<MessageEventArgs> MessageBuildingConstructionEvent;
    public event EventHandler<MessageEventArgs> MessageBuildingUpgradeEvent;
    public event EventHandler<MessageEventArgs> MessageBuildingDestructionEvent;
    public event EventHandler<MessageEventArgs> MessageTrophyWonEvent;
    public event EventHandler<MessageEventArgs> MessageResourceProductionUpdateEvent;
    public event EventHandler<MessageEventArgs> MessageResourceStockUpdateEvent;
    public event EventHandler<MessageEventArgs> MessageResourceTransfertEvent;
    public event EventHandler<MessageEventArgs> MessageChallengeCompleteEvent;
    public event EventHandler<MessageEventArgs> MessageScoreUpdateEvent;
    public event EventHandler<MessageEventArgs> MessageSystemStartOfGame;
    public event EventHandler<MessageEventArgs> MessageSystemEndOfGameEvent;


    void Awake()
    {
        _client = new UdpClient();
        //_client.Connect("172.18.136.49", 1523);
        _client.Connect(this.serverIP, this.sendingPort);
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
            listener = new UdpClient(this.listeningPort);
        }
        catch
        {
            Debug.Log("Unable to establish connect to UDP " + this.listeningPort + " port. Verify your network configuration.");
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
        //Go to see the excell to get message format

        string[] split = message.Split('@');
        this.MessageEvent = null;

        int code = Int32.Parse(split[1]);

        //Raise event
        switch (code)
        {
            case 11221:
            case 12221:
            case 12321:
            case 12421:
                MessageEvent += MessageTrophyWonEvent;
                break;
            case 21111:
            case 22111:
            case 23111:
            case 24111:
                MessageEvent += MessageBuildingConstructionEvent;
                break;
            case 21121:
            case 22121:
            case 23121:
            case 24121:
                MessageEvent += MessageBuildingUpgradeEvent;
                break;
            case 21161:
            case 22161:
            case 23161:
            case 24161:
                MessageEvent += MessageBuildingDestructionEvent;
                break;
            case 21331:
            case 22331:
            case 23331:
            case 24331:
                MessageEvent += MessageResourceTransfertEvent;
                break;
            case 21355:
            case 22355:
            case 23355:
            case 24355:
                MessageEvent += MessageResourceStockUpdateEvent;
                break;
            case 30001:
                MessageEvent += MessageSystemStartOfGame;
                break;
            case 30002:
                MessageEvent += MessageSystemEndOfGameEvent;
                break;
        }

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