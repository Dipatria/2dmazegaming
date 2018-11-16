using UnityEngine;
using System.Collections;

public class RotateArrow : MonoBehaviour {
	GameObject finish;
	GameObject ball;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		finish = GameObject.FindGameObjectWithTag ("Finish");
		ball = GameObject.FindGameObjectWithTag ("Player");
		if (finish && ball) {
			Vector3 relative = ball.transform.position - finish.transform.position;
			float angle = Mathf.Atan2 (-relative.z, relative.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (new Vector3 (90,angle - 90f, 0));
		}
	}
}
