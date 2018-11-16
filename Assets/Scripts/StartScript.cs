using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartScript : MonoBehaviour {

	public bool start = false;
	GameObject ball;
	GameObject timer;
	private Text counterText;
	private float count = 4F;
	// Use this for initialization
	void Start () {
		timer = GameObject.FindGameObjectWithTag("Timer");
		counterText = transform.FindChild("Countdown").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (start && count > 0F) {
			count -=  Time.deltaTime;
			counterText.text = ((int)count).ToString ("0");
			if (count <= 1F) {
				counterText.text = ("GO");
			}
		}
			
		ball = GameObject.FindGameObjectWithTag ("Player");
		if (ball && count <= 0F && GameObject.Find("Countdown")) {
			Destroy (transform.FindChild ("Countdown").gameObject);
			ball.GetComponent<MoveBall> ().ready = start;
			timer.GetComponent<Timer> ().ready = start;
		}
	}
}
