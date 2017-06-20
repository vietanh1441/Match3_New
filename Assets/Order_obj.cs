using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order_obj : MonoBehaviour {
    /// <summary>
    /// No functionality, this is purely for display order
    /// Display order: up down right left
    /// </summary>
    public Sprite[] s = new Sprite[5];
    public void SetOrder(Order order)
    {
        for(int i =0; i < 4; i++)
        {
            
                transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = s[order.ingredient[i]];
            
        }
    }
}
