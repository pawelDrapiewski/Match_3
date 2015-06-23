using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestGameBoard : MonoBehaviour
{
	
	//public GameObject[,] Tiles;
	//public GameObject[,] GemsTemp;
	public List<GameObject> Gems;
	public int gridWidth = 6;
	public int gridHeight = 6;
	public GameObject tilePrefab;
	public GameObject gemPrefab;
	public float spacing = 0.2f;
	public float speed = 10f;
	public TestGemSphere firstSelected;
	public TestGemSphere secondSelected;
	//public List<TestGemSphere> horizontalMatchedList;
	//public List<TestGemSphere> verticalMatchedList;
	public List<TestGemSphere> gemsAbove;
	public List<TestGemSphere> gemsDestroyed;
	public List<TestGemSphere> movedGems;


	public LevelStarter levelStarter;
	public GameData gameData;
	public DebugWindow debugWindow;
	
	//TEST

	public int coroutineCounter = 0;
	
	public enum GameState
	{
		animation,
		play,
		pause,
		stopAnimation,
		swapingGems,
		checkForMatches,
		swapingGemsAnimation
	}

	public GameState gameState = GameState.animation;
	public static int counter = 0;


	// SwapNew
	public float timeTakenDuringLerp = 5f;
	private float timeStartedLerping;

	void Awake ()
	{
		levelStarter = GetComponent<LevelStarter>();
		gameData = GetComponent<GameData>();
		debugWindow = GetComponent<DebugWindow>();
		//print ("Board AWAKE.");
		//GameData.Tiles = new GameObject[gridWidth, gridHeight];
		//GameData.GemsTemp = new GameObject[gridWidth, gridHeight];
	}

	// Use this for initialization
	void Start ()
	{
		//print ("Board START.");
				
		// TILES
		levelStarter.CreateTiles ();
		
		// SPHERES
		levelStarter.CreateGems ();
		
		// TEST

		//CreateGem (5, 5);
		//CreateGem (2, 1);
		//CreateGem (0, 4);
		while (levelStarter.CheckStartingBoard()) {
			levelStarter.DestroyBoard ();
			levelStarter.CreateGems ();
			print ("TRUE");
		}

		MoveToBoardAll ();
	}

	void FixedUpdate ()
	{
	

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (gameState == GameState.animation) {

		}

		if (gameState == GameState.stopAnimation) {
			StopAllCoroutines ();
		}

		if (gameState == GameState.swapingGems) {
			//print ("game swaping");
			gameState = GameState.swapingGemsAnimation;
			StartCoroutine (SwapGemsNew ());
		}

		if (gameState == GameState.checkForMatches) {
			gameState = GameState.play;
			// Check for matches
			CheckForMatches ();

			//first down above gems
			ReplaceDestroyed ();

		}

	}

	public void CheckForMatches ()
	{
		int horizontalMatch = 0;
		int verticalMatch = 0;

		foreach (TestGemSphere movedGem in movedGems) {
			int x = movedGem.actualTile.Xpos;
			int y = movedGem.actualTile.Ypos;

			for (int i = x+1; i < gridWidth; i++) {
				if (CheckSingle (movedGem, i, y, ref horizontalMatch, movedGem.horizontalMatchedList)) {
					continue;
				} else
					break;
			}

			for (int i = x-1; i >= 0; i--) {
				if (CheckSingle (movedGem, i, y, ref horizontalMatch, movedGem.horizontalMatchedList)) {
					continue;
				} else
					break;
			}

			for (int i = y+1; i < gridHeight; i++) {
				if (CheckSingle (movedGem, x, i, ref verticalMatch, movedGem.verticalMatchedList)) {
					continue;
				} else
					break;
			}

			for (int i = y-1; i >= 0; i--) {
				if (CheckSingle (movedGem, x, i, ref verticalMatch, movedGem.verticalMatchedList)) {
					continue;
				} else
					break;
			}

//			Debug.Log ("<color=red>" + movedGem.horizontalMatchedList.Count.ToString ()
//				+ " Horizontal matched List count. For Gem: " + movedGem.name.ToString () + "</color>"); 
//			Debug.Log ("<color=red>" + movedGem.verticalMatchedList.Count.ToString ()
//				+ " Vertical matched List count. For Gem: " + movedGem.name.ToString () + "</color>");

			if (movedGem.verticalMatchedList.Count > 2) {
				foreach (TestGemSphere matchedGem in movedGem.verticalMatchedList) {
					gemsDestroyed.Add (matchedGem);
					int xcolumn = matchedGem.Xpos;
					gameData.DestroInColumn[xcolumn].Add(matchedGem);
					GameObject tempGem = this.transform.FindChild (matchedGem.name.ToString ()).gameObject;
//					Debug.Log ("Destroing: " + tempGem.name.ToString ());
					Destroy (tempGem);
				}
			}

			if (movedGem.horizontalMatchedList.Count > 2) {
				foreach (TestGemSphere matchedGem in movedGem.horizontalMatchedList) {
					gemsDestroyed.Add (matchedGem);
					int xcolumn = matchedGem.Xpos;

					if (!gameData.DestroInColumn[xcolumn].Contains(matchedGem))
					{
						gameData.DestroInColumn[xcolumn].Add(matchedGem);
					}

					GameObject tempGem = this.transform.FindChild (matchedGem.name.ToString ()).gameObject;
//					Debug.Log ("Destroing: " + tempGem.name.ToString ());
					Destroy (tempGem);
				}
			}

			for (int i =0; i < GameData.gridWidth; i++)
			{
				foreach (TestGemSphere gem in gameData.DestroInColumn[i])
				{
					if (null == gem)
					{
						Debug.Log ("Empty at: " + i.ToString());
					}
					else if (null != gem)
					{
						Debug.Log ("<color=blue>Column: " + i.ToString() + "</color>");
						Debug.Log ("<color=magneta>Gem: " + gem.name.ToString() + "</color>");
					}
				}
			}


			movedGem.verticalMatchedList.Clear ();
			movedGem.horizontalMatchedList.Clear ();

			horizontalMatch = 0;
			verticalMatch = 0;
		}

//		foreach (TestGemSphere matchedGem in movedGem.horizontalMatchedList) {
//			Debug.Log ("Horizontal matched gems: " + matchedGem.name.ToString ());
//		}

//		Debug.Log ("<color=blue>" + verticalMatch.ToString() + " vertical.</color>");
//		Debug.Log ("<color=blue>" + horizontalMatch.ToString() + " horizontal.</color>");





		//TEST function
		RearangeByYpos(gameData.DestroInColumn);



		GetGemsAbove (gemsDestroyed);

		ChangePositionAboveGems(gameData.AboveInColumn, gameData.DestroInColumn);

//		movedGem.verticalMatchedList.Clear ();
//		movedGem.horizontalMatchedList.Clear ();
		movedGems.Clear ();

	}

	public bool CheckSingle (TestGemSphere movedGem, int x, int y, 
	                        ref int matchCount, List<TestGemSphere> matchedList)
	{
		if (movedGem.colorType == GameData.Tiles [x, y].GetComponent<Tile> ().ActualGem.colorType) {
			matchedList.Add (GameData.Tiles [x, y].GetComponent<Tile> ().ActualGem);
			matchCount++;

//			Debug.Log ("<color=red>" + matchCount.ToString () + " match count</color>");
//			Debug.Log (GameData.Tiles [x, y].GetComponent<Tile> ().ActualGem.name.ToString () + " Added.");

			if (!matchedList.Contains (movedGem)) {
				matchCount++;
				matchedList.Add (movedGem);
			}

			return true;
		}
		return false;
	}

	public void GetGemsAbove (List<TestGemSphere> destroyedGemsList)
	{
		foreach (TestGemSphere destroyedGem in destroyedGemsList) {
			int x = destroyedGem.actualTile.Xpos;
			int y = destroyedGem.actualTile.Ypos;

//			Debug.Log (x.ToString () + " X");
//			Debug.Log (y.ToString () + " Y");

			for (int i = y; i < gridHeight; i++) {
				if (!gemsAbove.Contains (GameData.Tiles [x, i].GetComponent<Tile> ().ActualGem) &&
				    !destroyedGemsList.Contains (GameData.Tiles [x, i].GetComponent<Tile> ().ActualGem)) {

					Debug.Log ("<color=blue>" + GameData.Tiles [x, i].GetComponent<Tile> ().ActualGem.name.ToString ()
						+ " Added as Above.</color>");

					//gemsaboveincolumn ?
					gemsAbove.Add (GameData.Tiles [x, i].GetComponent<Tile> ().ActualGem);
					gameData.AboveInColumn[x].Add(GameData.Tiles [x, i].GetComponent<Tile> ().ActualGem);
				}
			}
		}

		// :D
		debugWindow.UpdateText();

	}

	public void RearangeByYpos(List<TestGemSphere>[] gemsList)
	{
		int [,] posY;

		//is X width and Y height - OK???
		posY = new int[gridWidth,gridHeight];

		for (int i =0; i < GameData.gridWidth; i++)
		{
			gemsList[i].Sort();
//			foreach (TestGemSphere gem in gemsList[i])
//			{
//				if (null == gem)
//				{
//					//dont remember what is it for :/
//					//canvasTextLeft.text += "Empty at: " + i.ToString() + "\n";
//				}
//				else if (null != gem)
//				{
//					posY[i,gem.Ypos] = gem.Ypos;
//
//				}
//			}
		}
	}

	public void ChangePositionAboveGems (List<TestGemSphere>[] gemsList, List<TestGemSphere>[] gemsPosList)
	{
		for (int x = 0; x < gemsList.Length; x++)
		{
//			gemsList[i].Sort();

			for (int y = 0; y < gemsList[x].Count; y++)
			{
				if (null == gemsList[x][y])
				{
					//dont remember what is it for :/
					//canvasTextLeft.text += "Empty at: " + i.ToString() + "\n";
				}
				else if (null != gemsList[x][y])
				{
					if (null != gemsPosList[x][y])
					{
						gemsList[x][y].transform.position = gemsPosList[y][y].actualTile.transform.position;
					}
					else
					{
						gemsList[x][y].transform.position = gemsList[x][y-1].actualTile.transform.position;
					}

				}
			}
		}
	}

	/// <summary>
	/// Replaces destroyed gems.
	/// </summary>
	public void ReplaceDestroyed ()
	{
		List<TestGemSphere> newGems;

		//musze miec dane z destro x oraz y ??
		foreach (TestGemSphere destro in gemsDestroyed)
		{
			int x = destro.Xpos;
			int y = destro.Ypos;
			levelStarter.CreateGem(x,y);
		}

		gemsDestroyed.Clear();

		//MoveToBoardAll ();

	}

	/// <summary>
	/// Swaps the gems (new).
	/// </summary>
	/// <returns>Swap gems new.</returns>
	IEnumerator SwapGemsNew ()
	{
		timeStartedLerping = Time.time;


		Vector3 firstGemPosStart = firstSelected.transform.position;
		Vector3 firstGemPosEnd = secondSelected.ActualTilePosition();
		Vector3 firstGemScaleStart = firstSelected.transform.localScale;

		Vector3 secondGemPosStart = secondSelected.transform.position;
		Vector3 secondGemPosEnd = firstSelected.ActualTilePosition();

		float precentageComplete = 0;
		float timeSinceStarted;



		while (precentageComplete < 1.0f) {
			timeSinceStarted = (Time.time - timeStartedLerping) * 3f;
			precentageComplete = timeSinceStarted / timeTakenDuringLerp;


			/*
			print (Time.time.ToString() + " time");
			print (timeStartedLerping.ToString() + " time started");
			print (timeSinceStarted.ToString() + " time since started");
			int precentageShow = (int)(precentageComplete * 100);
			print (precentageShow.ToString() + " precentage");
			*/

			firstSelected.transform.localScale = Vector3.Lerp (firstGemScaleStart, Vector3.zero, precentageComplete);
			secondSelected.transform.localScale = Vector3.Lerp (firstGemScaleStart, Vector3.zero, precentageComplete);

			yield return null;
		}


		firstSelected.transform.position = firstGemPosEnd;
		secondSelected.transform.position = secondGemPosEnd;

		timeStartedLerping = Time.time;
		precentageComplete = 0;

		while (precentageComplete < 1.0f) {
			timeSinceStarted = (Time.time - timeStartedLerping) * 3f;
			precentageComplete = timeSinceStarted / timeTakenDuringLerp;

			firstSelected.transform.localScale = Vector3.Lerp (Vector3.zero, firstGemScaleStart, precentageComplete);
			secondSelected.transform.localScale = Vector3.Lerp (Vector3.zero, firstGemScaleStart, precentageComplete);
			
			yield return null;
		}
		
		/*Tile tempTile = firstSelected.boardPositionTile;
		firstSelected.boardPositionTile = secondSelected.boardPositionTile;
		secondSelected.boardPositionTile = tempTile;
		
		firstSelected = null;
		secondSelected = null;*/


		//print ("Done swaping.");

		Tile tempTile = firstSelected.actualTile;
		firstSelected.actualTile = secondSelected.actualTile;
		secondSelected.actualTile = tempTile;

		int tempX = firstSelected.Xpos;
		int tempY = firstSelected.Ypos;
		GameData.GemsTemp[tempX,tempY] = secondSelected.gameObject;
		GameData.GemsTemp[secondSelected.Xpos, secondSelected.Ypos] = firstSelected.gameObject;
		firstSelected.Xpos = secondSelected.Xpos;
		firstSelected.Ypos = secondSelected.Ypos;
		secondSelected.Xpos = tempX;
		secondSelected.Ypos = tempY;

		firstSelected.state = TestGemSphere.State.idle;
		secondSelected.state = TestGemSphere.State.idle;

		firstSelected.actualTile.ActualGem = firstSelected;
		secondSelected.actualTile.ActualGem = secondSelected;

		movedGems.Add (firstSelected);
		movedGems.Add (secondSelected);

		firstSelected = null;
		secondSelected = null;

		gameState = GameState.checkForMatches;

		foreach (TestGemSphere movedGem in movedGems) {
			Debug.Log ("Moved gems: " + movedGem.name.ToString ());
		}
	}

	IEnumerator TestCoroutine ()
	{
		print ("TEST Coroutine");

		float startTime = Time.time;
		float timeElapsed = 0;

		Vector3 firstGemPosStart = firstSelected.transform.position;
		Vector3 mainUp = Vector3.zero;

		while (Vector3.Distance(firstSelected.transform.position, mainUp) > 0.05f) {
			timeElapsed += Time.deltaTime;
			firstSelected.transform.position = 
				Vector3.MoveTowards (firstGemPosStart, mainUp, 1f * timeElapsed);
			coroutineCounter += 1;
			print (firstSelected.transform.position.ToString ());
			print (mainUp.ToString ());
			print (firstGemPosStart.ToString ());
			print (coroutineCounter.ToString ());
			yield return new WaitForSeconds (0.5f);
		}
		print ("TEST Coroutine after");
	}

	public void MoveToBoardAll ()
	{
		for (int y = 0; y < gridHeight; y++) {
			speed = 10f;
			
			for (int x = 0; x < gridWidth; x++) {
				if (GameData.GemsTemp [x, y] != null) {
					speed -= 2f * Time.deltaTime;
					if (speed < 5f)
						speed = 5f;

					StartCoroutine (MoveToBoard (x, y, speed));
				}
			}
		}
	}
	
	IEnumerator MoveToBoard (int x, int y, float speed)
	{
		//print ("coroutine started");
		//yield return new WaitForSeconds (1f);
			
		while (Vector3.Distance(GameData.GemsTemp[x,y].transform.position, GameData.Tiles[x,y].transform.position) > 0.05f) {
		
			GameData.GemsTemp [x, y].transform.position = 
				Vector3.MoveTowards (GameData.GemsTemp [x, y].GetComponent<TestGemSphere> ().startingPos.position, 
				                     GameData.Tiles [x, y].transform.position,
		                                                       speed * Time.deltaTime);
			yield return null;
		}
	
		if (GameData.GemsTemp [x, y] != null) {
			if (Vector3.Distance (GameData.GemsTemp [x, y].transform.position, GameData.Tiles [x, y].transform.position) < 0.05f) {
				GameData.GemsTemp [x, y].transform.position = GameData.Tiles [x, y].transform.position;
				//print ("correct");
			}
		}

		//print ("DONE");
	}
	



	

	
	public void GemSelected (TestGemSphere gemSelected)
	{
		if (firstSelected == gemSelected) {
			firstSelected = null;
		} else if (firstSelected == null) {
			firstSelected = gemSelected;
		} else if (secondSelected == null && firstSelected != gemSelected) {
			secondSelected = gemSelected;
			gameState = GameState.swapingGems;
		} else if (secondSelected != null) {
			//firstSelected.ChangeStateOnClick();
			secondSelected = firstSelected;
			firstSelected = gemSelected;
			//secondSelected.ChangeStateOnClick();
		}
	}

	public void SwapGems ()
	{
		// zmienne pozycji gemow
		//TestGemSphere firstGem = firstSelected;
		//TestGemSphere secondGem = secondSelected;

		Vector3 firstGemPosStart = firstSelected.transform.position;
		Vector3 firstGemPosEnd = secondSelected.ActualTilePosition();

		Debug.Log (firstGemPosStart.ToString () + "1 start.");
		Debug.Log (firstGemPosEnd.ToString () + "1 end.");

		Vector3 secondGemPosStart = secondSelected.transform.position;
		Vector3 secondGemPosEnd = firstSelected.ActualTilePosition();

		Debug.Log (secondGemPosStart.ToString () + "2 start.");
		Debug.Log (secondGemPosEnd.ToString () + "2 end.");

		// punkt w polowie drogi 
		Vector3 halfWay = new Vector3 ((firstGemPosEnd.x + secondGemPosEnd.x) / 2f,
		                               (firstGemPosEnd.y + secondGemPosEnd.y) / 2f,
		                               (firstGemPosEnd.z + secondGemPosEnd.z) / 2f);

		Debug.DrawLine (firstGemPosEnd, halfWay, Color.red);
		Debug.DrawLine (halfWay, secondGemPosEnd, Color.blue);

		// pomocny wektor prostopadly
		Vector3 halfWayHelper = new Vector3 (halfWay.x, halfWay.y, halfWay.z - 1f);

		Debug.DrawLine (halfWay, halfWayHelper, Color.cyan);

		// wektory prostopadle
		Vector3 crossUp = Vector3.Cross (firstGemPosEnd, halfWayHelper).normalized;
		Vector3 crossDown = Vector3.Cross (secondGemPosEnd, halfWayHelper).normalized * -1;

		// miejsce w przestrzeni wzgledem wektorow
		Vector3 mainDown = (halfWay - crossDown);
		Vector3 mainUp = (halfWay - crossUp);

		Debug.DrawLine (halfWay, mainUp, Color.magenta);
		Debug.DrawLine (halfWay, mainDown, Color.magenta);

		/*firstSelected.transform.position = 
			Vector3.MoveTowards(firstGemPosStart, mainUp, 1f * Time.deltaTime);
		secondSelected.transform.position = 
			Vector3.MoveTowards(secondGemPosStart, mainDown, 1f * Time.deltaTime);*/
	
	
		
		/*if (Vector3.Distance(firstSelected.transform.position, firstGemPosEnd) > 0.1f)
		{
			print ("TRUE");
			firstSelected.transform.position = 
				Vector3.MoveTowards(firstGemPosStart, firstGemPosEnd, 1f * Time.deltaTime);
			secondSelected.transform.position = 
				Vector3.MoveTowards(secondGemPosStart, secondGemPosEnd, 1f * Time.deltaTime);
		}
		else 
		{
			print ("FALSE");
		}*/

		//gameState = GameState.play;

		if (gameState == GameState.swapingGems) {
			StartCoroutine (SwapingFirstStage (firstSelected, secondSelected, firstGemPosStart, 
			                                 firstGemPosEnd, secondGemPosStart, secondGemPosEnd,
			                                 mainUp, mainDown));
			//print ("swapping!!!!!!!!!!!!");
		}
		/*if (Vector3.Distance(firstSelected.transform.position, mainUp) > 0.1f ||
		    Vector3.Distance(secondSelected.transform.position, mainDown) > 0.1f)
		{
			firstSelected.transform.position = 
				Vector3.MoveTowards(firstGemPosStart, mainUp, 1f * Time.deltaTime);
			secondSelected.transform.position = 
				Vector3.MoveTowards(secondGemPosStart, mainDown, 1f * Time.deltaTime);

		}*/

		if (Vector3.Distance (firstSelected.transform.position, firstGemPosEnd) < 0.05f ||
			Vector3.Distance (secondSelected.transform.position, secondGemPosEnd) < 0.05f) {
			//print ("Done swaping.");
			firstSelected.transform.position = firstGemPosEnd;
			secondSelected.transform.position = secondGemPosEnd;

			Tile tempTile = firstSelected.actualTile;
			firstSelected.actualTile = secondSelected.actualTile;
			secondSelected.actualTile = tempTile;

			firstSelected = null;
			secondSelected = null;
			gameState = GameState.play;
		}
	
	}

	IEnumerator SwapingFirstStage (TestGemSphere firstSelectedGem, TestGemSphere secondSelectedGem, 
	                              Vector3 firstGemPosStart, Vector3 firstGemPosEnd, 
	                              Vector3 secondGemPosStart, Vector3 secondGemPosEnd,
	                              Vector3 mainUp, Vector3 mainDown)
	{
		float startTime = Time.time;
		float elapsedTime = 0;
		//gameState = GameState.play;

		//print (counter.ToString());
		//counter++;
		//print ("coroutine");
		//print (gameState.ToString());
		bool firstStage = true;

		if (gameState == GameState.swapingGems) {
			while (Vector3.Distance(firstSelected.transform.position, mainUp) > 0.1f ||
			       Vector3.Distance(secondSelected.transform.position, mainDown) > 0.1f) {
			//while (firstStage == true)
				//print ("in coroutine loop");

				Debug.Log (firstGemPosStart.ToString () + "1 start.");
				Debug.Log (firstGemPosEnd.ToString () + "1 end.");
				Debug.Log (firstSelected.transform.position.ToString () + "1 position.");


				//print (firstStage.ToString());
				elapsedTime += Time.deltaTime;
				firstSelectedGem.transform.position = 
					Vector3.MoveTowards (firstGemPosStart, mainUp, 1f * elapsedTime);
				secondSelectedGem.transform.position = 
					Vector3.MoveTowards (secondGemPosStart, mainDown, 1f * elapsedTime);

				yield return null;
			}

			firstStage = false;

			firstSelected.transform.position = mainUp;
			secondSelected.transform.position = mainDown;

			Debug.Log (firstGemPosStart.ToString () + "1 start.");
			Debug.Log (firstGemPosEnd.ToString () + "1 end.");
			Debug.Log (firstSelected.transform.position.ToString () + "1 position.");

			elapsedTime = 0;

			while (Vector3.Distance(firstSelected.transform.position, firstGemPosEnd) > 0.1f ||
			       Vector3.Distance(secondSelected.transform.position, secondGemPosEnd) > 0.1f) {
				//print ("in 2 coroutine loop");
				elapsedTime += Time.deltaTime;
				firstSelectedGem.transform.position = 
					Vector3.MoveTowards (mainUp, firstGemPosEnd, 1f * elapsedTime);
				secondSelectedGem.transform.position = 
					Vector3.MoveTowards (mainDown, secondGemPosEnd, 1f * elapsedTime);
				
				yield return null;
			}

			firstSelected.transform.position = firstGemPosEnd;
			secondSelected.transform.position = secondGemPosEnd;
			gameState = GameState.play;
		}

		gameState = GameState.play;
		//print ("before yield");
		yield return null;
		//print ("after yield");
	}
	
	void OnMouseDown ()
	{
	
		//print ("clicked");
	}
}