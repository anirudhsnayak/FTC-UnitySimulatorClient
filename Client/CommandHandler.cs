using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandHandler
{
    static bool updatedFromCache = true;
    static Dictionary<string, List<HingeJoint>> taggedJoints = new Dictionary<string, List<HingeJoint>>();
    static Dictionary<string, RobotCommand> currentRobotCommands = new Dictionary<string, RobotCommand>();
    static Dictionary<string, RobotCommand> cachedRobotCommands = new Dictionary<string, RobotCommand>();
    static List<RobotCommand> commandQueue = new List<RobotCommand>();
    public static void QueueCommand(string cmds, SocketCommunicator sc)
    {
        //Initialize RobotCommandList
        List<RobotCommand> robotCommandList = new List<RobotCommand>();
        string[] cmdList = cmds.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        foreach (string cmd in cmdList)
        {
            robotCommandList.Add(new RobotCommand(cmd));
        }
        //Use RobotCommandList to update current robot commands
        foreach (RobotCommand cmd in robotCommandList)
        {
            if (cmd.id == 1) //double checking is probably not necessary
            {
                foreach (RobotCommand _cmd in commandQueue)
                {
                    cachedRobotCommands[_cmd[0]] = _cmd;
                }
                updatedFromCache = false;
                currentRobotCommands = new Dictionary<string, RobotCommand>(cachedRobotCommands);
                updatedFromCache = true;
                commandQueue.Clear();
                return;
            }
            commandQueue.Add(cmd);
        }
    }
    public static void ClearCommands()
    {
        commandQueue.Clear();
    }
    public static void ApplyCommands()
    {
      if (updatedFromCache)
        {
            ApplyCommandsTable(currentRobotCommands);
        }
        else
        {
            ApplyCommandsTable(cachedRobotCommands);
        }
     }
    public static void RegisterJoint(string tag, HingeJoint hj)
    {
        if (!taggedJoints.ContainsKey(tag))
        {
            taggedJoints.Add(tag, new List<HingeJoint>());
        }
        taggedJoints[tag].Add(hj);
    }
    static void ApplyCommandsTable(Dictionary<string, RobotCommand> commandTable)
    {
        foreach (RobotCommand robotCommand in commandTable.Values.ToList())
        {
            switch (robotCommand.id)
            {
                case 0:
                    JointMotor motor = new JointMotor();
                    if (!taggedJoints.ContainsKey(robotCommand[0]))
                    {
                        break;
                    }
                    foreach (HingeJoint hj in taggedJoints[robotCommand[0]])
                    {
                        float arg;
                        if (float.TryParse(robotCommand[1], out arg))
                        {
                            motor.force = arg;
                        }
                        if (float.TryParse(robotCommand[2], out arg))
                        {
                            motor.targetVelocity = arg; //use tryparse
                        }
                        hj.motor = motor;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

public class RobotCommand
{
    public int id; //accessors?
    string[] args;
    public RobotCommand(string cmd)
    {
        List<string> argsList= new List<string>();
        int i = 0;
        int count = -1;
        int prevIndex = 0;
        foreach (char c in cmd)
        {
            if (c == ';')
            {
                count++;
                argsList.Add(cmd.Substring(prevIndex, 1+i-prevIndex).TrimEnd(';'));
                prevIndex = i+1;
            }
            i++;
        }
        id = (int) float.Parse(argsList[0]); //use tryparse
        argsList.RemoveAt(0);
        args = argsList.ToArray();
    }
    public string this[int index]{
        get { return args[index]; }
    }
} 

public enum SensorCommand
{
    DistanceSensor = 0,
    IMUSensor = 1,
    EncoderSensor = 2
}