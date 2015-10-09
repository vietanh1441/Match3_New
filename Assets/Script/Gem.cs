using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gem : MonoBehaviour {
	//Set up for changing sprite color
	public Sprite[] color_sprite = new Sprite[5];
	private SpriteRenderer spriteRenderer;
	public int color;
	bool release = true;
	bool ready = false;
	bool detonate = false;

	//if gems is a character
	public bool isChar;




	//Set up for click and drag
	private Vector3 offset;
	private Vector3 screenPoint;

	//The original coordinate
	public int curX;
	public int curY;

	//mark whether it's up for destroy
	private bool marked = false;				
	//value when matched
	public int value;

	//Gem Holder cache
	private GameObject gemHolder_obj;
	private GemHolder gemHolder_scr;

	//Currently Coordinate, get by its local position
	public int XCoord
	{
		get
		{
			return Mathf.RoundToInt(transform.localPosition.x);
		}
	}
	public int YCoord
	{
		get
		{
			return Mathf.RoundToInt(transform.localPosition.y);
		}
	}

	/*public void NewTurn()
	{

		//Get the gem ready if it's not, change its tag to its appropriate color
		if (!ready && !isChar) {
			ready = true;
			if (color == 0)
			{
				transform.tag = "Fire";

			}
			if (color == 1)
			{
				transform.tag = "Tree";
				//spriteRenderer.color = Color.blue;
			}
			if (color == 2)
			{
				transform.tag = "Water";
			//	spriteRenderer.color = Color.red;
			}
			if (color == 3)
			{
				transform.tag = "Heart";
				//spriteRenderer.color = Color.green;
			}
			if(color== 4)
			{
				transform.tag = "Dark";
				//spriteRenderer.color = Color.gray;
			}
			spriteRenderer.sprite = color_sprite[color];
			transform.localScale = new Vector3(0.9f,0.9f,0.9f);
			//Debug.Log ("Ready");
		}


	}*/

	void Start () 
	{
	
			transform.localScale = new Vector3(0.9f,0.9f,0.9f);

		gemHolder_obj = GameObject.Find ("GemHolder");
		gemHolder_scr = gemHolder_obj.GetComponent<GemHolder> ();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		CreateGem();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateGem()
	{
		int color = Random.Range (0, 5);
		if (isChar) {
			transform.tag = "Char";
		} else {
			Init_color ();
		}
		transform.parent = GameObject.Find ("GemHolder").transform;
		curX = XCoord;
		curY = YCoord;
	}

	/// <summary>
	/// Initialize color
	/// </summary>
	void Init_color()
	{
		//transform.tag = "Unready";
		int color_num = 5;
		color = Random.Range(0, color_num);
		if (color == 0)
		{
			transform.tag = "Fire";
			//spriteRenderer.sprite  = color_sprite[color];
		}
		if (color == 1)
		{
			transform.tag = "Tree";
			//spriteRenderer.sprite  = color_sprite[color];
		}
		if (color == 2)
		{
			transform.tag = "Water";
			//spriteRenderer.sprite  = color_sprite[color];
		}
		if (color == 3)
		{
			transform.tag = "Heart";
			//spriteRenderer.sprite  = color_sprite[color];
		}
		if(color== 4)
		{
			transform.tag = "Dark";
			//spriteRenderer.sprite  = color_sprite[color];
		}
		spriteRenderer.sprite = color_sprite[color];
		//transform.tag = "Unready";
	}

	void OnMouseDown()
	{
		if (isChar)
			return;
		Debug.Log ("CheckStatus");
		Debug.Log ((int)gemHolder_scr.status);
		if ((int)gemHolder_scr.status == 0) {
			Debug.Log ("Called Mouse Down");
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
			Invoke ("ForceMouseUp", 5);
			release = false;
		}
	}
	
	void OnMouseDrag()
	{
		//Debug.Log ((int)gemHolder_scr.status);
		if (isChar)
			return;
		if (release == false &&(int)gemHolder_scr.status == 0 ) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			if (curPosition.x >= gemHolder_scr.GridWidth+5 - 0.5f || curPosition.x <= 4.5f) {
				curPosition.x = curX;
			}

			if (curPosition.y >= gemHolder_scr.GridHeight+5 - 0.5f || curPosition.y <= 4.5f) {
				curPosition.y = curY;
			}


//			Debug.Log (XCoord);
			transform.position = curPosition;
			//Debug.Log (XCoord);
			//Debug.Log (curX);
			if ((XCoord != curX || YCoord != curY) && gemHolder_scr.gems [XCoord, YCoord] != null) {

				gemHolder_scr.SwapGem (gameObject, gemHolder_scr.gems [XCoord, YCoord]);
			}
		}
	}



	//Register the current coordinate
	public void Register()
	{
		curX = XCoord;
		curY = YCoord;
	}

	void ForceMouseUp()
	{
	
		if (release == false) {

			Release ();
		}
	}

	void OnMouseUp()
	{
		if (isChar)
			return;
		if (release == false &&(int)gemHolder_scr.status == 0 ) {
			Release ();
		}
	}

	public void Release()
	{
		if ((int)gemHolder_scr.status == 0)
		{
			//Debug.Log ("RELEASE");
		release = true;
		transform.localPosition = new Vector2 (curX, curY);
		gemHolder_scr.NextAction ();
	}

	}

	/// <summary>
	/// Mark the specified value.
	/// </summary>
	/// <param name="value">the length of the match, e.g. match 3 or 4</param>
	void Mark(int val)
	{
		marked = true;
		value = val;
	}

	public bool isMarked()
	{
		return marked;
	}

	public void DestroyMarked()
	{
		//Error checking make sure that the character does not get killed in matching
		if (isChar)
			return;

		if (marked) {


			//Destroy(gameObject);
			StartCoroutine("MatchAnimation");
		}
	}

	IEnumerator MatchAnimation()
	{
		detonate = true;
		if(transform.CompareTag("Dark"))
		{
			DealDamage(XCoord,YCoord+1);
			DealDamage(XCoord,YCoord-1);
			DealDamage(XCoord+1,YCoord);
			DealDamage(XCoord-1,YCoord);

		}
		transform.localScale = new Vector3(0.5f,0.5f,0.5f);
		yield return new WaitForSeconds (1f);
		//gemHolder_scr.CreateNewGem(XCoord, YCoord);
		Destroy (gameObject);
	}

	void DealDamage(int x,int y)
	{
		if(gemHolder_scr.gems[x,y] != null)
		{
			gemHolder_scr.gems[x,y].SendMessage("ApplyDamage",1);
		}
	}

	public void ApplyDamage(int damage)
	{

		//Debug.Log ("ApplyDamage");
		if (isChar) {
			gameObject.GetComponent<Character> ().ApplyDamage (damage);
		} else {
			if (transform.tag == "Heart") {
				gemHolder_scr.hp--;
				//Lose level hp
				StartCoroutine("MatchAnimation");
			}
			else
			{
				if(detonate == true)
				{

				}
				else
				{
				StartCoroutine("MatchAnimation");
				}
			}
		}
	}
}
