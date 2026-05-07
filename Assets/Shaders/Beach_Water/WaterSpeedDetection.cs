using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterSpeedDetection : MonoBehaviour
{
    public GameObject Water;
    public Material mat;
    public float speed = 0.0f;
    public Vector3 MovingDir;
    public Vector3 lastFramePos = Vector3.zero;
    public float testspeed = 0.0f;


    private void DetectSpeed()
    {
        speed = Vector3.Distance(Water.transform.position, lastFramePos) / Time.deltaTime;
        MovingDir = (Water.transform.position - lastFramePos).normalized;
        lastFramePos = Water.transform.position;
    }


    private void Start()
    {
        if(Water == null)
            Water = this.gameObject;
        if (Water != null)
            mat = Water.GetComponent<Renderer>().material;
        lastFramePos = Water.transform.position;

    }

    private void Update()
    {
        this.transform.position += new Vector3(0.01f * testspeed, 0, 0);
        if (Water != null && mat != null)
        {
            DetectSpeed();
            mat.SetFloat("_Speed", speed);
            mat.SetVector("_MovingDir", MovingDir);
        }
        
    }
}
