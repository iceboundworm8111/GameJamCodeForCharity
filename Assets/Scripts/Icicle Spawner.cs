using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IcicleSpawner : MonoBehaviour
{
    public float timeBetweenSpawn = 5f;
    public float timer = 0;

    public bool activated = false;

    public GameObject iclicle;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            timer += Time.deltaTime;

            if (timer > timeBetweenSpawn)
            {
                timer = 0;
                Instantiate(iclicle, spawnPoint);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activated = true;
        }
    }

    // Only deactivate when the player leaves
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activated = false;
        }
    }
}
