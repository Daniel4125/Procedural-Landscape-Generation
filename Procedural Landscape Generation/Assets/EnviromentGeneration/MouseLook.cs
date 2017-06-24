using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

	//enum data structure 
	public enum RotationAxes {
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2
	}

	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityHor = 9.0f;
	public float sensitivityVert = 9.0f;
	public float minimumVert = -45f;
	public float maximumVert = 45f;

	private float rotationX = 0;

	void Update() {
		if (axes == RotationAxes.MouseX) {
			//horizontal rotation here
			//Rotate() is good for rotation without limit
			transform.Rotate (0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
		} else if (axes == RotationAxes.MouseY) {
			//verticle axes rotation here
			rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
			rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert); //Clamp verticle angle betweemn limits

			float rotationY = transform.localEulerAngles.y; //Keep same y angle (no rotation)

			transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
		} else {
			//Both horizontal and verticle rotation here
			rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
			rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert); 
			//Increment rotation angle by delta
			float delta = Input.GetAxis("Mouse X") * sensitivityHor;
			float rotationY = transform.localEulerAngles.y + delta;

			transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
		}
	}
}
