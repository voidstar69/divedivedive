using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public MonoBehaviour player;

	private const float moveSpeed = 1.0f; // units per second

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		Vector3 dir = (player.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
		dir.z = 0;
		transform.position += dir;

		//print("Foo");
		//Debug.Log("bar");
	}
}
