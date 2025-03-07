using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float windForce = 5;
    public Vector3 windDirection = new Vector3(0, 0, 0);

    private bool affectingPlayer = false;

    private PlayerMovement playersMovement;

    public ParticleSystem windParticles;

    // Start is called before the first frame update
    void Start()
    {
        var velocityModule = windParticles.velocityOverLifetime;
        velocityModule.enabled = true;

        // Apply wind direction
        velocityModule.x = new ParticleSystem.MinMaxCurve(windDirection.x * windForce);
        velocityModule.y = new ParticleSystem.MinMaxCurve(windDirection.y * windForce);
        velocityModule.z = new ParticleSystem.MinMaxCurve(windDirection.z * windForce);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + windDirection.normalized);

        if (affectingPlayer)
        {
            playersMovement.WindAffectingPlayer(windDirection, windForce * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            affectingPlayer = true;
            playersMovement = other.gameObject.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            affectingPlayer = false;
        }
    }
}
