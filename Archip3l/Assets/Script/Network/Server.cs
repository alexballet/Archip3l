using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server : MonoBehaviour
{

    private class CommunicationData
    {
        public IPEndPoint Client { get; private set; }
        public byte[] Data { get; private set; }

        public CommunicationData(IPEndPoint client, byte[] data)
        {
            Client = client;
            Data = data;
        }
    }

    private UdpClient _broadcaster;
    private bool _continue = true;
    private Thread _thListener;

    private int connectionPort = 5054;
    private int listeningPort = 1523;

    void Awake()
    {
        _broadcaster = new UdpClient();
        _broadcaster.EnableBroadcast = true;
        _broadcaster.Connect(new IPEndPoint(IPAddress.Broadcast, this.connectionPort));

        this.StartServer();
    }

    private void StartServer()
    {
        Debug.Log("Starting server ...");

        _continue = true;
        _thListener = new Thread(new ThreadStart(ListenNetwork));
        _thListener.Start();
    }

    private void StopServer(bool attendre)
    {
        Debug.Log("Stoppping server ...");
        _continue = false;

        if (attendre && _thListener != null && _thListener.ThreadState == ThreadState.Running)
            _thListener.Join();
    }

    private void ListenNetwork()
    {
        UdpClient server = null;
        bool erreur = false;
        int attempts = 0;

        do
        {
            try
            {
                server = new UdpClient(this.listeningPort);
            }
            catch
            {
                erreur = true;
                attempts++;
                Thread.Sleep(400);
            }
        } while (erreur && attempts < 4);

        if (server == null)
        {
            Debug.Log("Verify the network configuration ...");
            this.StopServer(false);
            return;
        }

        server.Client.ReceiveTimeout = 1000;

        //Listening loop
        while (_continue)
        {
            try
            {
                IPEndPoint ip = null;
                byte[] data = server.Receive(ref ip);

                CommunicationData cd = new CommunicationData(ip, data);
                new Thread(new ParameterizedThreadStart(ProcessMessage)).Start(cd);
            }
            catch
            {
            }
        }
        server.Close();
    }

    private void ProcessMessage(object messageArgs)
    {
        try
        {
            CommunicationData data = messageArgs as CommunicationData;
            string message = string.Format("{0}:{1} > {2}", data.Client.Address.ToString(), data.Client.Port, Encoding.Default.GetString(data.Data));

            //Send over network
            byte[] donnees = Encoding.Default.GetBytes(message);
            _broadcaster.Send(donnees, donnees.Length);

            //Debug.Log("Server : broadcasting on network " + message);
        }
        catch { }
    }
}