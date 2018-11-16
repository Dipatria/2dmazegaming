using UnityEngine;
using System.Collections;

public class MoveWithArrow : MonoBehaviour {
	public bool ready = false;
	public float speed = 5.0f;
	public Vector3 sensor = new Vector3 (0.0f, -1.0f, 0.0f);
	// Use this for initialization
	private Rigidbody Ball;
	void Start () {
		Ball = transform.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		sensor.x = 0f;
		sensor.z = 0f;
		if (Input.GetKey (KeyCode.UpArrow)) {
			sensor.z = 1f; 
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			sensor.z = -1f; 
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			sensor.x = -1f; 
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			sensor.x = 1f; 
		}
			
		if (ready) {
			Physics.gravity = sensor * speed;
			if (Ball.velocity.magnitude > 10f) {
				Ball.velocity = Ball.velocity.normalized * 10f;
			}
		}
	}
}
