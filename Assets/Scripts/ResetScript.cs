using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour {

	// Use this for initialization
	public void Reset () {
		SceneManager.LoadScene("maze");
	}

	public void GoMenu () {
		SceneManager.LoadScene("menu");
	}
}
