using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour {

	public int width=10, height=10;
	Text widthText, heightText;

	void Awake(){
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
//		Input.gyro.enabled = false;
	}

	// Use this for initialization
	void Start () {
		widthText = GameObject.FindWithTag ("Width").GetComponent<Text> ();
		heightText = GameObject.FindWithTag ("Height").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void WidthPlus(){
		if (width < 50) {
			width += 1;
			widthText.text = width.ToString ();
		}
	}

	public void WidthMinus(){
		if(width>10){
			width -= 1;
		widthText.text = width.ToString ();
		}
	}

	public void HeightPlus(){
		if(height<50){
			height += 1;
			heightText.text = height.ToString ();
		}
	}

	public void HeightMinus(){
		if(height>10){
			height -= 1;
			heightText.text = height.ToString ();
		}
	}

	public void StartMenu(){
		PlayerPrefs.SetInt ("Width", width);
		PlayerPrefs.SetInt ("Height", height);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("maze");
	}
}
