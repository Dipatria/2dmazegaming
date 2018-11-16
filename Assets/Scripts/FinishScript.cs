using UnityEngine;
using System.Collections;

public class FinishScript : MonoBehaviour {

	bool finished = false;
	bool display = false;
	public Transform resetPrefab;
	public Transform menuPrefab;
	Transform resetBtn;
	Transform menuBtn;
	GameObject ball;

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			GameObject.FindWithTag ("Timer").SendMessage ("Finish");
			finished = true;
			displayBtn ();
		}

	}
	private void tooggleBtn(){
		

		if (display&& resetBtn.gameObject.activeSelf&& menuBtn.gameObject.activeSelf) {
			Destroy (GameObject.FindGameObjectWithTag ("MenuBtn"));
			Destroy (GameObject.FindGameObjectWithTag ("RestartBtn"));
			ball.GetComponent<MoveBall> ().ready = true;
		}
		if (!display) {
			displayBtn ();

		}
		display = !display;
	} 
	private void displayBtn(){

		ball.GetComponent<MoveBall> ().ready = false;

		resetBtn = (Transform)Instantiate(resetPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		resetBtn.SetParent(GameObject.Find("Canvas").transform);
		resetBtn.localPosition = new Vector3 (0, 85, 0);
		resetBtn.localRotation =  Quaternion.identity;
		resetBtn.localScale = new Vector3 (3, 3, 1);


		menuBtn = (Transform)Instantiate(menuPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		menuBtn.SetParent(GameObject.Find("Canvas").transform);
		menuBtn.localPosition = new Vector3 (0, -76, 0);
		menuBtn.localRotation =  Quaternion.identity;
		menuBtn.localScale = new Vector3 (3, 3, 1);

	}

	void Start(){
		ball = GameObject.FindGameObjectWithTag ("Player");
	}
			void Update(){
		if (Input.GetKey (KeyCode.Escape)&& !finished) {
					tooggleBtn ();
				}
	}
}
