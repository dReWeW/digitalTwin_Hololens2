using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedUI : MonoBehaviour
{
    //    private MeshRenderer meshRenderer;

    // Use this for initialization
    // void Start()
    // {
    //     // Grab the mesh renderer that's on the same object as this script.
    //     meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
    // }

    // Update is called once per frame
    void Update()
    {
        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var rotateDirection=Camera.main.transform.rotation;


            // If the raycast hit a hologram...
            // Display the cursor mesh.
            // meshRenderer.enabled = true;

            // Move thecursor to the point where the raycast hit.
            this.transform.position = headPosition;

            // Rotate the cursor to hug the surface of the hologram.
            this.transform.rotation = rotateDirection;
    }
}
