using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Order  {
    /// <summary>
    /// When created, the owner will pass 5 numbers instead.
    /// When GetOrder() is called, it will return a converted tag version
    /// 
    /// </summary>

    public string[] tag = new string[4];
    public int[] ingredient = new int[4];

    public Order(string[] t) {
        for(int i = 0; i<4; i++)
        {
            tag[i] = String.Copy(t[i]);
        }
    }

    public Order(int[] j)
    {
        for (int i = 0; i < 4; i++)
        {
            ingredient[i] = j[i];
            if(j[i] == 0)
            {
                tag[i] = "ItemA";
            }
            if (j[i] == 1)
            {
                tag[i] = "ItemB";
            }
            if (j[i] == 2)
            {
                tag[i] = "ItemC";
            }
            if (j[i] == 3)
            {
                tag[i] = "ItemD";
            }
            if (j[i] == 4)
            {
                tag[i] = "ItemE";
            }
        }
    }

    public string[] GetOrder()
    {
        return tag;
    }
        

}
