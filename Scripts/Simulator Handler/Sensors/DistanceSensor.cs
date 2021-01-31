using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSensor : MonoBehaviour
{
    public string Tag;
    void Start()
    {
        StartCoroutine(UpdateSensor());
    }

    IEnumerator UpdateSensor()
    {
        while (true)
        {
            float measuredDistance;
            Ray ray = new Ray(transform.position, transform.rotation * Vector3.right);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                measuredDistance = hit.distance;
            } else
            {
                measuredDistance = 300;
            }
            if (SimulatorHandler.sc != null)
            {
                SimulatorHandler.sc.SendMessage("S;" + ((int) SensorCommand.DistanceSensor) + ";" + Tag + ";" + measuredDistance + ";");
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
