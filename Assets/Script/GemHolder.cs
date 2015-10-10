using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemHolder : MonoBehaviour {
	public GameObject[,] gems = new GameObject[15,15];
	public GameObject[,] floors = new GameObject[15,15];
	public int GridWidth = 5;
	public int GridHeight = 5;
	public GameObject gemPrefab;
	public GameObject[] charPrefab;
	public GameObject floorPrefab;
	private int turnCount=0;
	private int ult_turn, cap_turn;

	public bool ult_active = false;
	public bool cap_active = false;
	//public int hp = 10;
	public List<GameObject> darkMagicList = new List<GameObject>();
	//public GameObject[] monsterPrefab;
	//private bool[,] mark_to_create = new bool[10, 10];
	public List<GameObject> characterList = new List<GameObject>();
	public int charfinish = 0;

	public List<GameObject> monsterList = new List<GameObject>();
	public int monsterFinish = 0;

	public enum Status { Ready, CheckChar, CheckBoard, CheckMonster, NewTurn} ;
	public Status status;

	//general ultimate/capture/hp
	public int hp = 10, maxhp=10, ult = 0, maxUlt = 100, cap= 0, maxCap=100;
	public GameObject PowerBar, HpBar, HoTBar;
	UISlider PowerSlider, HpSlider, HoTSlider;
	UILabel PowerLabel, HpLabel, HoTLabel;

	//public int currentStatus;

	void Start () 
	{

		hp = maxhp;
		InitUI();
		status = Status.Ready;
		//currentStatus = (int)Status.NewTurn;

		for(int y=5;y<GridHeight+5;y++)
		{
			for(int x=5;x<GridWidth+5;x++)
			{
				CreateNewGem(x,y);
				CreateNewFloor(x,y);
			//	mark_to_create[x,y] = false;
//				gems.Add(g.GetComponent<Gem>());
			}
		}

		PutInCharacter ();
		Display();
	//	PutInMonster ();
	}

	void InitUI()
	{
		PowerSlider = PowerBar.GetComponent<UISlider>();
		HpSlider = HpBar.GetComponent<UISlider>();
		HoTSlider = HoTBar.GetComponent<UISlider>();
		PowerLabel = PowerBar.transform.FindChild("Label").gameObject.GetComponent<UILabel>();
		HpLabel = HpBar.transform.FindChild("Label").gameObject.GetComponent<UILabel>();
		HoTLabel = HoTBar.transform.FindChild("Label").gameObject.GetComponent<UILabel>();
		Display();
	}

	public void Display()
	{
		PowerSlider.value = (float)ult/maxUlt;
		HpSlider.value = (float)hp/maxhp;
		HoTSlider.value = (float)cap/maxCap;
		PowerLabel.text = ult + "/" + maxUlt;
		HpLabel.text = hp + "/" + maxhp;
		HoTLabel.text = cap + "/" + maxCap;
	}

	public void PutInMonster()
	{
		for (int i = 0; i < charPrefab.Length; i++) {
			int x = Random.Range (0,GridWidth);
			int y = Random.Range(0,GridHeight);
			
			Destroy (gems[x,y]);
			
			GameObject g = Instantiate (charPrefab[i], new Vector3 (x, y, 0), Quaternion.identity)as GameObject;
			g.transform.parent = gameObject.transform;
			gems [x, y] = g;
			characterList.Add (g);
		}
	}

	//Put a character into the board;
	public void PutInCharacter()
	{
		for (int i = 0; i < charPrefab.Length; i++) {
			int x = Random.Range (5,GridWidth+5);
			int y = Random.Range(5,GridHeight+5);

			Destroy (gems[x,y]);

			GameObject g = Instantiate (charPrefab[i], new Vector3 (x, y, 0), Quaternion.identity)as GameObject;
			g.transform.parent = gameObject.transform;
			gems [x, y] = g;
			characterList.Add (g);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Swaps the gem.
	/// </summary>
	/// <param name="original gems">Gem1.</param>
	/// <param name="gem use to swap">Gem2.</param>
	public void SwapGem(GameObject gem1, GameObject gem2)
	{
		int x, y;
		Gem gem2_scr = gem2.GetComponent<Gem> ();
		x = gem2_scr.XCoord;
		y = gem2_scr.YCoord;
		gems [gem2_scr.XCoord, gem2_scr.YCoord] = gem1;
		Gem gem1_scr = gem1.GetComponent<Gem> ();
		gems [gem1_scr.curX, gem1_scr.curY] = gem2;

		gem2.transform.localPosition = new Vector2 (gem1_scr.curX, gem1_scr.curY);
		gem1.transform.localPosition = new Vector2 (x, y);
		gem1_scr.Register ();
		gem2_scr.Register ();
	}

	//The game "clock", dictacte what to do next;
	public void NextAction()
	{

		if (status == Status.Ready) {
			status = Status.CheckChar;
			CheckChar ();
		}
		else if(status == Status.CheckChar) {
			CheckGameStatus();
			status = Status.CheckBoard;
			CheckBoard();

		}
		else if( status == Status.CheckBoard) {
			status = Status.CheckMonster;
			CheckMonster ();

		}
		else if( status == Status.CheckMonster) {
			CheckGameStatus();
			status = Status.NewTurn;
			NewTurn ();
		}
		else if( status == Status.NewTurn) {
			status = Status.Ready;
			//Ready ();
		}
	}

	void CheckGameStatus()
	{
		if(CheckWinning())
		{
			Winning();
		}
		if(CheckLosing())
		{
			Losing();
		}
	}

	void Winning()
	{
		Debug.Log ("Winning");
	}

	void Losing()
	{
		Debug.Log ("Losing");
	}

	bool CheckWinning()
	{
		if(monsterList.Count == 0)
			return true;
		return false;
	}

	bool CheckLosing()
	{
		if(characterList.Count == 0)
			return true;
		if(hp == 0)
			return true;
		return false;
	}

	//Check if any character is up for action
	public void CheckChar()
	{
		Debug.Log ("CheckChar");
		charfinish = 0;
		//Do Character stuff
		foreach(GameObject character in characterList)
		{
			character.SendMessage("DoAction");
		}


		//CheckBoard next
	//	Invoke ("NextAction", 1);

	}

	public void FinishChar()
	{
		charfinish++;
		if (charfinish == characterList.Count) {
			DestroyMarkedTile(false);
			Invoke ("NextAction" , 2);

		}
	}



	//Check for matches
	public void CheckBoard()
	{
		Debug.Log ("CheckBoard");
		//Check whole board for match, if it's indeed match, mark them
		for(int y=5;y<GridHeight+5;y++)
		{
			for(int x=5;x<GridWidth+5;x++)
			{
				Check(gems[x,y]);
			}
		}

		//Destroy all the mark and subtitutes with a new one.
		DestroyMarkedTile(true);



		Invoke ("NextAction", 2);
	}

	public void DestroyMarkedTile(bool t)
	{
		for(int y=5;y<GridHeight+5;y++)
		{
			for(int x=5;x<GridWidth+5;x++)
			{
				if(gems[x,y]!=null)
				gems[x,y].SendMessage("DestroyMarked",t);
				
			}
		}
	}

	public void ReadyForNewTurn()
	{
		for(int y=5;y<GridHeight+5;y++)
		{
			for(int x=5;x<GridWidth+5;x++)
			{
				if(gems[x,y] != null)
				gems[x,y].SendMessage("NewTurn");
				
			}
		}
	}

	/// <summary>
	/// Checks for any monster
	/// </summary>
	/// 
	public void CheckMonster()
	{

		for(int y=5;y<GridHeight+5;y++)
		{
			for(int x=5;x<GridWidth+5;x++)
			{ 
				if(floors[x,y] !=null)
				floors[x,y].SendMessage("ChangeColor", Color.white);
			}
		}

		Debug.Log ("CheckMonster");
		charfinish = 0;
		//Do Character stuff
		if(monsterList.Count == 0)
		{
			Invoke("NextAction", 1);
		}
		else
		{
			foreach(GameObject monster in monsterList)
			{
				monster.SendMessage("DoAction");
			}
		}
	//	Invoke ("NextAction", 1);
	}

	public void FinishMonster()
	{
		charfinish++;
		if (charfinish == characterList.Count) {
			Invoke ("NextAction" , 1);
		}
	}

	public void Replenished()
	{
		for(int y=5;y<GridHeight+5;y++)
		{
			for(int x=5;x<GridWidth+5;x++)
			{
				//gems[x,y].SendMessage("NewTurn");
				if(gems[x,y] == null)
				{
					CreateNewGem(x,y);
				}
			}
		}
	}

	/// <summary>
	/// Go to a new turn a do all things in new turn
	/// </summary>
	public void NewTurn()
	{


		Debug.Log ("NewTurn");
		if(cap_active)
		{
			if(turnCount != cap_turn)
			{
				cap_active = false;
				cap =0;
			}
		}
		if(ult_active)
		{
			if(turnCount != ult_turn)
			{
				ult = 0;
				ult_active = false;
			}
		}
		turnCount++;
		Display();
		//Ready all gems
		//ReadyForNewTurn();

		//Recreate all lost gems
		Replenished();

		

		//Create new gems

		//Allow moving again
		Invoke ("NextAction", 1);

	}

	/*public void Mark_Create(int x, int y)
	{
		bool[x,y] = true;
	}
*/
	public void CreateNewGem(int x, int y)
	{

			GameObject g = Instantiate (gemPrefab, new Vector3 (x, y, 0), Quaternion.identity)as GameObject;
			g.transform.parent = gameObject.transform;
			gems [x, y] = g;

	}

	public void CreateNewFloor(int x, int y)
	{
		
		GameObject g = Instantiate (floorPrefab, new Vector3 (x, y, 5), Quaternion.identity)as GameObject;
		g.transform.parent = gameObject.transform;
		floors [x, y] = g;
		
	}

	/// <summary>
	/// Check if there is any match for the param gem
	/// </summary>
	/// <param name="gem">Gem.</param>
	void Check(GameObject gem)
	{
		if(gem == null)
			return;
		//Check if this gem is already marked
		if (gem.GetComponent<Gem> ().isMarked ()) {
			return;
		}

		//If it's not ready then no need to check
		if(gem.transform.tag == "Unready")
		{
			return;
		}

		List<GameObject> list = new List<GameObject>();
		List<GameObject> tempList = new List<GameObject>();
		list.Add (gem);
		CheckVertical (gem,ref list, list);

		//Check each of the marked horizontally to find match
		if (list.Count > 0) {
			foreach (GameObject g in list) {
				CheckHorizontal (g, ref tempList, list);
			}
		}
		if (tempList.Count > 0) {
			foreach(GameObject g in tempList)
			{
				list.Add (g);
			}
			tempList.Clear ();
			foreach (GameObject g in list) {
				CheckVertical (g, ref tempList, list);
			}
			if (tempList.Count > 0) {
				foreach(GameObject g in tempList)
				{
					list.Add (g);
				}
				tempList.Clear ();
				foreach (GameObject g in list) {
					CheckHorizontal (g, ref tempList, list);
				}
				if (tempList.Count > 0) {
					foreach(GameObject g in tempList)
					{
						list.Add (g);
					}
				}
			}
		}



		if (list.Count > 2) {
			foreach ( GameObject g in list)
			{
				g.SendMessage("Mark", list.Count);
			}
		}


	}

	/// <summary>
	/// Checks the vertical.
	/// </summary>
	/// <param name="gem">Gem.</param>
	/// <param name="localList">Local list.</param>
	/// <param name="bigList">Big list.</param>
	void CheckVertical(GameObject gem , ref List<GameObject> localList, List<GameObject> bigList)
	{
		List<GameObject> list = new List<GameObject>();
		list.Add (gem);
		Gem gem_scr = gem.GetComponent<Gem> ();
		int x = gem_scr.XCoord;
		int y = gem_scr.YCoord;
		//get right side
		while (gems[x+1,y] != null) {
			if(gems[x+1,y].transform.CompareTag(gem.transform.tag))
			{
			list.Add (gems[x+1,y]);
			x++;
			}
			else
			{
				break;
				//goto Found;
			}
		}
		//get left side

		//if it's already in the list then there is no need to continue


		while (gems[x-1,y]!=null) {
			if(bigList.Contains(gems[x-1,y]))
			{
				break;
			}


			if(gems[x-1,y].transform.CompareTag(gem.transform.tag))
			{
				list.Add (gems[x-1,y]);
				x--;
			}
			else
			{
				break;
				//goto Found;
			}
		}
	
		if (list.Count > 2) {
			list.Remove (gem);
			foreach ( GameObject g in list)
			{
				localList.Add (g);
			}
		}
	}

	/// <summary>
	/// Checks the horizontal.
	/// </summary>
	/// <param name="gem">Gem.</param>
	/// <param name="localList">Local list.</param>
	/// <param name="bigList">Big list.</param>
	void CheckHorizontal(GameObject gem , ref List<GameObject> localList, List<GameObject> bigList)
	{
		List<GameObject> list = new List<GameObject>();
		list.Add (gem);
		Gem gem_scr = gem.GetComponent<Gem> ();
		int x = gem_scr.XCoord;
		int y = gem_scr.YCoord;
		//get up side
		while (gems[x,y+1] != null) {
			if(gems[x,y+1].transform.CompareTag(gem.transform.tag))
			{
				list.Add (gems[x,y+1]);
				y++;
			}
			else
			{
				break;
				//goto Found;
			}
		}
		//get down side
		while (gems[x,y-1]!=null) {
			if(bigList.Contains(gems[x,y-1]))
			{
				break;
			}
			if(gems[x,y-1].transform.CompareTag(gem.transform.tag))
			{
				list.Add (gems[x,y-1]);
				y--;
			}
			else
			{
				break;
				//goto Found;
			}
		}

		
		if (list.Count > 2) {
			list.Remove (gem);
			foreach ( GameObject g in list)
			{
				localList.Add (g);
			}
		}
	}


	public void ApplyDamage(int damage)
	{
		hp = hp-damage;
		Display();
	}

	public void AddHeart(int h)
	{
		cap = cap + h;

		if(cap_active == true)
		{
			cap = maxCap;
		}
		else if(cap >= maxCap && cap_active == false)
		{
			cap = maxCap;
			cap_turn = turnCount;
			cap_active = true;
			//Do Capture
		}
		Display();
	}

	public void AddCombo(int c)
	{
		ult = ult +c;
		Debug.Log (ult_active);
		if(ult_active == true)
		{
			ult = maxUlt;
		}
		else if(ult >= maxUlt && ult_active == false)
		{
			ult = maxUlt;
			ult_turn = turnCount;
			ult_active = true;
			Debug.Log ("Active ult");
			//Do Ult
		}
		Display();
	}
}
