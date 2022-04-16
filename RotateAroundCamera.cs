using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundCamera : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.FromToRotation(Vector3.forward, Camera.main.transform.forward);       
    }
}
