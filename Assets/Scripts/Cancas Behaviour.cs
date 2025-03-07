using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
//using UnityEditor.Build;
using UnityEngine;

public class CancasBehaviour : MonoBehaviour
{
    public CameraCenterOnPlayer cameraScript;
    public PlayerMovement playerScript;

    public GameObject platform;

    public GameObject StartPanel;
    //public GameObject GameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        //GameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerScript.isRope == true)
        //{
        //    GameOverPanel.SetActive(true);
        //}
    }

    public void PlayPressed()
    {
        StartPanel.SetActive(false);
        cameraScript.StartPressed();
        playerScript.StartPressed();
        platform.SetActive(false);
    }

    public void EndPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
