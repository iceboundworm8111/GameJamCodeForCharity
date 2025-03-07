using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    public float launchForce;

    public Animator spawnAnimator;

    public bool resetVelocity = false;

    public IcicleSpawner spawner;

    // Start is called before the first frame update
    void Awake()
    {
        spawnAnimator.Play("IcicleSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (!(spawnAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1))
        {
            spawnAnimator.enabled = false;

            if (!resetVelocity)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                resetVelocity = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (spawnAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) 
            return;

        if (collision.collider.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().LaunchPlayer(transform, launchForce);
        }

        spawner.IcicleDestroy();

        Destroy(gameObject);
    }

    public void GiveScript(IcicleSpawner _spawner)
    {
        spawner = _spawner;
    }
}
