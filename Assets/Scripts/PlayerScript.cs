using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {

	public GUIText text;
	public GameObject collectiblePrefab;
	public GameObject missilePrefab;
	public BoxCollider2D spawnBox;
	public Transform oxygenBar;
	public AudioClip itemPickupSound;
	public AudioClip refillAirSound;

	private const float moveSpeed = 3.0f; // units per second
	private const int oxygenCapacity = 5; // seconds
	private const int oxygenRefillSpeed = 2; // seconds of oxygen refilled every second!
	private float oxygenTimeLeft = oxygenCapacity; // seconds

	private const int maxItems = 5;
	private const int maxMissiles = 4;
	private int numItemsCollected;
	private int numItemsExist;
	private List<Object> items = new List<Object>();

	// Use this for initialization
	void Start () {
		ResetGame();
	}
	
	// Update is called once per frame
	void Update () {
//		float x = Input.GetAxis("Horizontal") * Time.deltaTime * 5;
//		float y = Input.GetAxis("Vertical") * Time.deltaTime * 5;
//		transform.position += new Vector3(x, y, 0);

		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 dir = mouseWorldPos - transform.position;
		dir = dir.normalized * Mathf.Min(dir.magnitude, moveSpeed * Time.deltaTime);
		transform.position += new Vector3(dir.x, dir.y, 0);

		// oxygen runs out over time
		oxygenTimeLeft -= Time.deltaTime;
		if(oxygenTimeLeft < 0)
		{
			// player dies
			ResetGame();
		}

		// show the amount of oxygen that the player has left
		oxygenBar.localScale = new Vector2(oxygenTimeLeft / oxygenCapacity * 55, oxygenBar.localScale.y);

		// debug text
		text.text = numItemsCollected + " collected. " + numItemsExist + " left. " + oxygenTimeLeft + "s oxygen left";
		//text.text = transform.position.ToString();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.CompareTag("Collectible"))
		{
			// destroy collectible, as the player has collected it
			Destroy(other.gameObject);
			numItemsExist--;
			numItemsCollected++;
			GetComponent<AudioSource>().PlayOneShot(itemPickupSound);
		}
		else if(other.CompareTag("Missile"))
		{
			// destroy missile and player
			Destroy(other.gameObject);
			ResetGame();
			GetComponent<AudioSource>().PlayOneShot(refillAirSound);
		}
		else if(other.CompareTag("Air"))
		{
			// create more collectibles
//			while(numItemsExist < maxItems)
//				SpawnCollectible();

//			audio.PlayOneShot(refillAirSound);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if(other.CompareTag("Air"))
		{
			// instantly refill player's oxygen tanks from the surface air
			oxygenTimeLeft = oxygenCapacity;
			// slowly refill player's oxygen tanks from the surface air
			//oxygenTimeLeft = Mathf.Min(oxygenCapacity, oxygenTimeLeft + oxygenRefillSpeed * Time.deltaTime);

			// do not allow the player to move above the water surface
			//var airBox = other as BoxCollider2D;
			//float surfaceY = airBox.center.y + airBox.size.y / 2.0f;
			var airTransform = other.transform;
			float surfaceY = airTransform.position.y - airTransform.localScale.y / 2.0f;
			transform.position = new Vector3(transform.position.x, Mathf.Min(transform.position.y, surfaceY), 0);
		}
	}

	private void SpawnCollectible()
	{
		Vector2 min = spawnBox.offset - spawnBox.size * 0.5f;
		Vector2 max = spawnBox.offset + spawnBox.size * 0.5f;
		var pos = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), 0);
		var item = Instantiate(collectiblePrefab, pos, Quaternion.identity);
		items.Add(item);
		numItemsExist++;
	}

	private void SpawnMissile()
	{
		Vector2 min = spawnBox.offset - spawnBox.size * 0.5f;
		Vector2 max = spawnBox.offset + spawnBox.size * 0.5f;
		var pos = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), 0);
		var item = Instantiate(missilePrefab, pos, Quaternion.identity);
		items.Add(item);
	}

	private void ResetGame()
	{
		oxygenTimeLeft = oxygenCapacity;

		// move player to near surface
		transform.position = new Vector2(spawnBox.offset.x, spawnBox.offset.y + spawnBox.size.y / 2);

		// destroy all collectibles and missiles
		foreach(var item in items)
			Destroy(item);
		items.Clear();
		numItemsExist = 0;
		numItemsCollected = 0;

		// create more collectibles
		for(int i = 0; i < maxItems; i++)
			SpawnCollectible();

		// create more missiles
		for(int i = 0; i < maxMissiles; i++)
			SpawnMissile();

		GetComponent<AudioSource>().PlayOneShot(itemPickupSound);
		GetComponent<AudioSource>().PlayOneShot(refillAirSound);
	}
}
