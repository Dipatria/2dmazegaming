using UnityEngine;
using System.Collections;

public class TrackObject : MonoBehaviour {
	//public Transform obj;
	GameObject obj;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 trackPosition = new Vector3 (obj.transform.position.x, obj.transform.position.y + 30.0f, obj.transform.position.z);
		//Vector3 trackPosition = obj.transform.position;
		obj = GameObject.FindGameObjectWithTag ("Player");
		if (obj != null)
			transform.position = new Vector3 (obj.transform.position.x, 24, obj.transform.position.z) + offset;
		transform.rotation = Quaternion.Euler (new Vector3 (90f, 0, 0));
	}
}
