using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {




	void ChangeColor(Color c)
	{
		gameObject.GetComponent<SpriteRenderer>().color = c;
	}
}
