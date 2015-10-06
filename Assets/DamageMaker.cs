﻿using UnityEngine;
using System.Collections;

public class DamageMaker : MonoBehaviour {
	public int damage;
	public bool enemy_only;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(enemy_only )
		{
			if( other.transform.CompareTag("Monster"))
			other.gameObject.SendMessage ("ApplyDamage", damage);
		}
		else
		{
			other.gameObject.SendMessage ("ApplyDamage", damage);
		}

	}

	void SelfDestruct()
	{
		Destroy(gameObject);
	}

	void SetDamage(int dam)
	{
		damage = dam;
		gameObject.GetComponent<BoxCollider2D>().enabled = true;
		Invoke("SelfDestruct", 3f);
	}
}
