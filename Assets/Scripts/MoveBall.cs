using UnityEngine;
using System.Collections;

public class MoveBall : MonoBehaviour {
	public bool ready = false;
	public float speed = 5.0f;
	public Vector3 sensor = new Vector3 (0.0f, -1.0f, 0.0f);
	// Use this for initialization
	private Rigidbody Ball;
	void Start () {
		Ball = transform.GetComponent<Rigidbody> ();
		Physics.gravity = sensor * speed;
	}

	
	// Update is called once per frame
	void Update () {
		if (ready) {
			#if UNITY_ANDROID
			sensor.x = Input.acceleration.x;
			sensor.z = Input.acceleration.y;
			#endif

			#if UNITY_TIZEN
			sensor.x = Input.acceleration.x;
			sensor.z = Input.acceleration.y;
			#endif
			
			#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
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
			#endif

			if (float.IsNaN (sensor.x) || float.IsNaN (sensor.z)) {
				return;
			}

			Physics.gravity = sensor * speed;
			if (Ball.velocity.magnitude > 10f) {
				Ball.velocity = Ball.velocity.normalized * 10f;
			}
		}
	}
}
