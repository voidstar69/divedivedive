using UnityEngine;
using System.Collections;

public class CollectibleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D other) {
		// destroy myself, as the player has collected me
		//audio.Play();
		//Destroy(gameObject);
	}
}