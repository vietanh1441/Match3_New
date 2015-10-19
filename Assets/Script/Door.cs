using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Vector3 target;
	public GameObject player;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Fire");
	}

	void OnMouseDown()
	{
		Debug.Log ("hit");
		player.SendMessage("SetTarget", target);

	}
}
