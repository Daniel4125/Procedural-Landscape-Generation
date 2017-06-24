using UnityEngine;
using System.Collections;

public class FPSInput : MonoBehaviour {

	public float speed = 50f;
	public float gravity = -9.8f;

	private CharacterController charController;

	void Start() {
		charController = GetComponent<CharacterController>();
	}

	void Update() {
		float deltaX = Input.GetAxis("Horizontal") * speed;
		float deltaZ = Input.GetAxis("Vertical") * speed;

		Vector3 movement = new Vector3(deltaX, 0, deltaZ);
		movement = Vector3.ClampMagnitude(movement, speed); //Limit diagonal movement
		movement.y = gravity;

		movement *= Time.deltaTime; //deltaTime: movement speed same on all computers
		movement = transform.TransformDirection(movement); //Coordinate Transform movement vector from local to global coordinates
		charController.Move(movement);
 
	}


}
