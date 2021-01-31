
using System.Threading;
using UnityEngine;
using UnityEditor;
using System.Net.Sockets;
using System;

public class SimulatorHandler : MonoBehaviour 
{
    public static SimulatorHandler main;
    public static SocketCommunicator sc;
    bool connected = false;

    void Start()
    {
        main = this;
        Debug.Break();
        EditorApplication.pauseStateChanged += OnPause;
    }
    void FixedUpdate()
    {
        ApplyCommands();
    }
    private void OnPause(PauseState pause)
    {
        StartSimulation();
        EditorApplication.pauseStateChanged -= OnPause;
    }
    void StartSimulation()
    {
        connected = true;
            try
            {
                sc = new SocketCommunicator(29071);
                sc.thread = new Thread(new ThreadStart(StartClient));
                sc.thread.Start();
            }
            catch (SocketException e)
            {
                Debug.LogError("There was an error connecting to the controller.");
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
    }
    void StartClient()
    {
        string msg; 
        msg=sc.GetMessage();
        if (msg == "RESET")
        {
            while (true)
            {
                msg = sc.GetMessage();
                if (!connected)
                {
                    sc.SendMessage("RESET");
                    CommandHandler.ClearCommands();
                    break;
                }
                if (msg == "RESET")
                {
                    break; 
                }
                CommandHandler.QueueCommand(msg, sc);
            }
        }
        else
        {
            sc.SendMessage("RESET");
        }
        connected = false;
        sc.CloseClient();
        sc = null;
    }
    
    void ApplyCommands()
    {
        if (connected)
        {
            CommandHandler.ApplyCommands();
        }
    }
    
}

