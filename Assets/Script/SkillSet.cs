using UnityEngine;
using System.Collections;

public class SkillSet : MonoBehaviour {

	public GameObject mine;
	public GameObject totem;

	public Vector2[] LoopUpVector(int skill, int type)
	{
		Vector2[] set = new Vector2[10];
		//Normal Mage
		if(type == 1)
		{
			if(skill == 1)
			{

				set[0] = new Vector2(0,1);
				set[1] = new Vector2(0,2);
				return set;
			}
			if(skill == 2)
			{
				set[0] = new Vector2(-1,1);
				set[1] = new Vector2(1,1);
				set[2] = new Vector2(0,1);
			}
			if(skill == 3)
			{
				set[0] = new Vector2(-1,1);
				set[1] = new Vector2(1,1);
				set[2] = new Vector2(0,2);
			}
			if(skill == 4)
			{
				set[0] = new Vector2(0,1);
				set[1] = new Vector2(1,0);
				set[2] = new Vector2(0,-1);
				set[3] = new Vector2(-1,0);
			}
		}
		return set;
	}

	public string[] LookUpElement(int skill, int type)
	{
		string[] s = new string[10];

		if(type == 1)
		{
			if(skill == 1)
			{
				s[0] = "Fire";
				s[1] = "Fire";
			}
			if(skill == 2)
			{
				s[0] = "Fire";
				s[1] = "Fire";
				s[2] = "Dark";
			}
			if(skill == 3)
			{
				s[0] = "Tree";
				s[1] = "Tree";
				s[2] = "Fire";
			}
			if(skill == 4)
			{
				s[0] = "Fire";
				s[1] = "Fire";
				s[2] = "Fire";
				s[3] = "Fire";
			}
			
		}
		return s;
	}

	public int LookUpDamage(int skill, int type, int level, int dmg)
	{
		int damage = 0;
		if(type == 1)
		{
			if(skill == 1)
			{
				damage = dmg + 2 * level;
			}
			if(skill == 2)
			{
				damage = 5;
			}
			if(skill == 3)
			{
				damage = 2;
			}
			if(skill == 4)
			{
				damage = 900 + 3*level;
			}
		}

		return damage;
	}

	public Vector2[] LookUpDamageTile(int skill, int type)
	{
		Vector2[] set = new Vector2[20];
		if(type == 1)
		{
			if(skill == 1)
			{
				set[0]  = new Vector2(0,-1);
			}
			if(skill == 2)
			{
				set[0] = new Vector2(0,1);
			}
			if(skill == 3)
			{
				set[0] = new Vector2(0,2);
			}
			if(skill == 4)
			{
				set[0] = new Vector2(0,0);
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
			if(skill == 1)
			{
				item = null;
			}
			if(skill == 2)
			{
				item = mine;
			}
			if(skill == 3)
			{
				item = totem;
			}
			if(skill == 4)
			{
				item = null;
			}

		}
		return item;
	}
}
