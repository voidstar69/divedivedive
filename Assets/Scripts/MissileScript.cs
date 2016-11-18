using UnityEngine;
using System.Collections;

public class MissileScript : MonoBehaviour {

	public float moveForce;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// move missile back and forth along a horizontal path
		int dir = (transform.position.x > 0 ? -1 : +1);
		rigidbody2D.AddForce(new Vector2(moveForce * dir, 0));

		// ensure missile faces correct direction
		if(rigidbody2D.velocity.x > 0)
			transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
		else
			transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
	}
}
