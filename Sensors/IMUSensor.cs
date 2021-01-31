using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMUSensor : MonoBehaviour
{
    public string Tag;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateSensor());
    }
    IEnumerator UpdateSensor()
    {
        while (true)
        {
            if (SimulatorHandler.sc != null)
            {
                SimulatorHandler.sc.SendMessage("S;" + ((int)SensorCommand.IMUSensor) + ";" + Tag + ";" + formatArgument(transform.position) + formatArgument(transform.rotation.eulerAngles));
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    string formatArgument(Vector3 vector3)
    {
        return vector3.x.ToString() + ";" + vector3.y.ToString() + ";" + vector3.z.ToString() + ";";
    }
}
