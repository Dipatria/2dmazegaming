using UnityEngine;
using System.Collections;


public class Timer : MonoBehaviour {
	public bool ready = false;
	private bool finished = false;
	public TextMesh counterText;
	private float second;
	private float minute;
	private float timePlay;
	private float startTime;
	// Use this for initialization
	void Start () {
		counterText = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (finished)
			return;
		
		if (ready) {
			if (startTime == 0f)
				startTime = Time.time;
			timePlay = Time.time - startTime;
			minute = (int)(timePlay / 60f);
			second = timePlay % 60f;
			counterText.text = minute.ToString("00") + ":" + second.ToString("00.000");
		}
	}

	public void Finish() {
		finished = true;
		counterText.color = Color.red;
	}
}
