using UnityEngine;
using System.Collections;

public class SkillSet : MonoBehaviour {

	public GameObject mine;

	public Vector2[] LoopUpVector(int skill, int type)
	{
		Vector2[] set = new Vector2[10];
		//Normal Mage
		if(type == 1)
		{
			if(skill == 0)
			{

				set[0] = new Vector2(1,1);
				set[1] = new Vector2(-1,1);
				return set;
			}
			if(skill == 2)
			{
				set[0] = new Vector2(0,1);
			}
		}
		return set;
	}

	public string[] LookUpElement(int skill, int type)
	{
		string[] s = new string[10];

		if(type == 1)
		{
			if(skill == 0)
			{
				s[0] = "Fire";
				s[1] = "Tree";
			}
			if(skill == 2)
			{
				s[0] = "Fire";
			}
			
		}
		return s;
	}

	public int LookUpDamage(int skill, int type, int level, int dmg)
	{
		int damage = 0;
		if(type == 1)
		{
			if(skill == 0)
			{
				damage = dmg + 2 * level;
			}
			if(skill == 2)
			{
				damage = 5;
			}
		}

		return damage;
	}

	public Vector2[] LookUpDamageTile(int skill, int type)
	{
		Vector2[] set = new Vector2[20];
		if(type == 1)
		{
			if(skill == 0)
			{
				set[0]  = new Vector2(0,2);
			}
			if(skill == 2)
			{
				set[0] = new Vector2(0,1);
			}
		}
		return set;
	}

	public GameObject LookUpSummon(int skill, int type)
	{
		GameObject item;
		item = null;
		if(type == 1)
		{
			if(skill == 2)
			{
				item = mine;
			}

		}
		return item;
	}
}
