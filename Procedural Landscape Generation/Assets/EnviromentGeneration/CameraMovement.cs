using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public Vector3 velocity;

    private Transform position;

    void Start()
    {
        position = GetComponent<Transform>();
        velocity = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            velocity.x = 1;
            position.position += velocity;
        }
        if (Input.GetKeyDown("s"))
        {
            velocity.x = -1;
            position.position += velocity;
        }
        if (Input.GetKeyDown("a"))
        {
            velocity.z = 1;
            position.position += velocity;
        }
        if (Input.GetKeyDown("d"))
        {
            velocity.z = -1;
            position.position += velocity;
        }
        
            
    }
	
}
