using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraCenterOnPlayer : MonoBehaviour
{
    public Transform player;

    public float zPos = -10f;

    private bool startPressed = false;

    public float zoomInTimer = 5;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPressed)
            return;
        
        if (timer > zoomInTimer)
        {
            // Normal
            transform.position = new Vector3(player.position.x, player.position.y, zPos);
        }
        else
        {
            // Zooming in

        }
        
    }

    public void StartPressed()
    {
        startPressed = true;
    }
}
