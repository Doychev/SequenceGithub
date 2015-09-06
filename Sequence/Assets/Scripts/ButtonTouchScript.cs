using UnityEngine;
using System.Collections;

public class ButtonTouchScript : MonoBehaviour {

	public GameObject buttonsHolder;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseDown() {
		bool playing = buttonsHolder.GetComponent<ButtonsScript> ().getPlaying ();
		if (playing) {
			string last = this.name.Substring(this.name.Length - 1);
			int number = int.Parse (last);
			StartCoroutine(buttonsHolder.GetComponent<ButtonsScript> ().checkInput(number));
		}
	}
}