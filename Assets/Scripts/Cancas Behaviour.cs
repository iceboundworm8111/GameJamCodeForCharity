using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build;
using UnityEngine;

public class CancasBehaviour : MonoBehaviour
{
    public CameraCenterOnPlayer cameraScript;
    public PlayerMovement playerScript;

    public GameObject platform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPressed()
    {
        gameObject.SetActive(false);
        cameraScript.StartPressed();
        playerScript.StartPressed();
        platform.SetActive(false);
    }
}
