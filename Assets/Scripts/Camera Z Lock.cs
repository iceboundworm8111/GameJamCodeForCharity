using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZLock : MonoBehaviour
{
    public float zPos = -10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
    }
}
