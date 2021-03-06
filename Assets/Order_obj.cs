﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order_obj : MonoBehaviour {
    /// <summary>
    /// No functionality, this is purely for display order
    /// Display order: up down right left
    /// </summary>
    public Sprite[] s = new Sprite[5];
    private GameObject hud;
    private HUDText hud_text;

    void Start()
    {
        hud = GameObject.FindGameObjectWithTag("HUD");
        //Debug.Log(hud);
        hud_text = hud.GetComponent<HUDText>();
    }

    public void SetOrder(Order order)
    {
        for(int i =0; i < 4; i++)
        {
            
                transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = s[order.ingredient[i]];
            
        }
    }

    public void SetScore(int score)
    {
        hud.SendMessage("FollowTarget", transform);
        hud_text.Add(score, Color.yellow, 5);
    }
}
