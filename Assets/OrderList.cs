using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderList : MonoBehaviour {

    /// <summary>
    /// This list will be empty at the start. When player unlock new skill, this will be added
    /// During CheckChar turn, the player will check with the list in this orderlist 
    /// </summary>
    private List<Order> orders = new List<Order>();


	// Use this for initialization
	void Start () {
        //Mock data
        int[] i = new int[4] { 0, 0, 0, 0 };
        Order o = new Order(i);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
