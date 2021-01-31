using System;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
public class SocketCommunicator
{
    string cache = "";
    Socket sender;
    public Thread thread;
    public SocketCommunicator(int port)
    {
            IPAddress ipAddress = new IPAddress(new byte[4] { 127, 0, 0, 1 });
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
            sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(remoteEP);

            Debug.Log("Socket connected to " + sender.RemoteEndPoint.ToString());

    }
    public void SendMessage(string message)
    {
        try
        { 
            byte[] msg = Encoding.ASCII.GetBytes(message+Environment.NewLine);    
            int bytesSent = sender.Send(msg); 
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    public string GetMessage() 
    {
        try
        {
            byte[] bytes = new byte[1024];
            while (true)
            {
                int bytesRec = sender.Receive(bytes);
                if (bytesRec != 0)
                {
                    string lastCache = cache;
                    string message = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    int index = message.LastIndexOf(Environment.NewLine);
                    if (index != -1)
                    {
                        cache = message.Substring(index).Trim();
                        message = message.Remove(index);
                    }
                    return (lastCache + message);            
                }
                Thread.Sleep(0);
            }
        }
        catch (SocketException e)
        {
            if (e.ErrorCode == 10054)
            {
                return "RESET";
            }
            else
            {
                Debug.Log(e.ToString());
            }
        }
        return ""; //Error message
    }
    public void CloseClient()
    {
        try { 
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
