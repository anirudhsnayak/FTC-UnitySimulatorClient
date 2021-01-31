using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCMotor : MonoBehaviour
{
    const int TICKS_PER_ROT = 1120;
    const int REFRESH_RATE = 25;

    public new HingeJoint hingeJoint;
    public long ticks;
    public string Tag;

    float prevAngle;

    void Start()
    {
        hingeJoint.useMotor = true;
        CommandHandler.RegisterJoint(Tag, hingeJoint);
        StartCoroutine(UpdateSensor()); //Encoder
    }
    IEnumerator UpdateSensor()
    {
        while (true)
        {
            ticks += ticksSinceLastMeasurement();
            if (SimulatorHandler.sc != null)
            {
                SimulatorHandler.sc.SendMessage("S;" + ((int)SensorCommand.EncoderSensor) + ";" + Tag + ";" + ticks + ";");
            }
            yield return new WaitForSeconds(1 / REFRESH_RATE);
        }
    }
    int ticksSinceLastMeasurement() //reallly rough, need to get more accurate results
    {
        float dt = 1 / (float)REFRESH_RATE;
        int scaleTicks = (int)(hingeJoint.velocity * dt) * (TICKS_PER_ROT / 360);
        return scaleTicks;
    }
}
