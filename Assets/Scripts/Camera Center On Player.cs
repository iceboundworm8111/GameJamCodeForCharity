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
    public float lerpAmount = 0.5f;

    public bool normal = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPressed)
            return;
        
        if (normal)
        {
            // Normal
            transform.position = new Vector3(player.position.x, player.position.y, zPos);
        }
        else
        {
            // Zooming in
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y, zPos), lerpAmount);

            if (Vector3.Distance(transform.position, new Vector3(player.position.x, player.position.y, zPos)) < 1)
            {
                normal = true;
            }
        }
        
    }

    public void StartPressed()
    {
        startPressed = true;
    }
}
