using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class gamepadScript : MonoBehaviour {

	public EventSystem eventSystem;
	public GameObject obj;
	private bool gamepad;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxisRaw("Vertical")!= 0 && !gamepad){
			eventSystem.SetSelectedGameObject (obj);
			gamepad = true;
		}
	}
	void OnDisable(){
		gamepad = false;
	}
}
