using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public List<GameObject> monsterList = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log (other.transform.name);

		if (other.transform.CompareTag("Fire"))
		{
			GameObject.Find("Central").SendMessage("GoInside", monsterList);
			Debug.Log ("Hit");
			//Application.LoadLevel("test1");
		}
	}
}
