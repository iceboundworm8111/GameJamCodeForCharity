using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IcicleSpawner : MonoBehaviour
{
    public float timeBetweenSpawn = 5f;
    private float timer = 0;

    private bool activated = false;

    public GameObject iclicle;
    public Transform spawnPoint;

    public AudioSource soundSource;

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
                GameObject icicle = Instantiate(iclicle, spawnPoint);
                icicle.GetComponent<Icicle>().GiveScript(this);
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

    public void IcicleDestroy()
    {
        soundSource.Play();
    }
}
