using UnityEngine;
using System.Collections;

public class LevelStarter : MonoBehaviour {

	public GameData gameData;

	// Use this for initialization
	void Awake () {
		gameData = GetComponent<GameData>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/// <summary>
	/// Creates spheres for all board.
	/// </summary>
	public void CreateGems ()
	{
		for (int y = 0; y < GameData.gridHeight; y++) {
			for (int x = 0; x < GameData.gridWidth; x++) {
				GameObject g = Instantiate (gameData.gemPrefab, new Vector3 (x, y, 0), Quaternion.identity) as GameObject;
				
				g.transform.parent = gameObject.transform;
				
				// W przyszlosci wyliczyc dokladna pozycje miedzy dwoma filarami
				// centrum = ilosc kostek w rzedzie / 2 - pol kostki
				// umiescic kostke odpowiednio wzgledem centrum
				g.transform.localPosition = new Vector3 (x, y + 15 + (y * 0.3f), 0);
				
				g.name = y.ToString () + x.ToString ();
				
				// uncomment to show number 
				g.GetComponentInChildren<TextMesh> ().text = g.name;
				
				GameData.GemsTemp [x, y] = g;
				
				TestGemSphere gemScript = g.GetComponent<TestGemSphere> ();

				gemScript.actualTile = GameData.Tiles [x, y].GetComponent<Tile> ();

				gemScript.moveToPosition = gemScript.ActualTilePosition();
				gemScript.moveToPosition = new Vector3 (gemScript.ActualTilePosition().x,
				                                        gemScript.ActualTilePosition().y,
				                                        gemScript.ActualTilePosition().z - 1);


				GameData.Tiles [x, y].GetComponent<Tile> ().ActualGem = gemScript;
				
				gemScript.Xpos = x;
				gemScript.Ypos = y;
			}
		}
	}

	/// <summary>
	/// Creates the gem.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public void CreateGem (int x, int y)
	{
		GameObject g = Instantiate (gameData.gemPrefab, new Vector3 (x, y, 0), Quaternion.identity) as GameObject;
		
		g.transform.parent = gameObject.transform;
		
		// W przyszlosci wyliczyc dokladna pozycje miedzy dwoma filarami
		// centrum = ilosc kostek w rzedzie / 2 - pol kostki
		// umiescic kostke odpowiednio wzgledem centrum
		g.transform.localPosition = new Vector3 (x, y + 15 + (y * 0.3f), 0);
		
		g.name = y.ToString () + x.ToString ();
		
		// uncomment to show number 
		g.GetComponentInChildren<TextMesh>().text = g.name;
		
		GameData.GemsTemp [x, y] = g;
		
		TestGemSphere gemScript = g.GetComponent<TestGemSphere> ();
		gemScript.actualTile = GameData.Tiles [x, y].GetComponent<Tile> ();

		gemScript.moveToPosition = gemScript.ActualTilePosition();
		gemScript.moveToPosition = new Vector3 (gemScript.ActualTilePosition().x,
		                                        gemScript.ActualTilePosition().y,
		                                        gemScript.ActualTilePosition().z - 1);
		

	}

	/// <summary>
	/// Creates the tiles.
	/// </summary>
	public void CreateTiles ()
	{
		for (int y = 0; y < GameData.gridHeight; y++) {
			for (int x = 0; x < GameData.gridWidth; x++) {
				GameObject g = Instantiate (gameData.tilePrefab, new Vector3 (x, y, 0), Quaternion.identity) as GameObject;

				g.transform.parent = this.transform;
				
				// W przyszlosci wyliczyc dokladna pozycje miedzy dwoma filarami
				// centrum = ilosc kostek w rzedzie / 2 - pol kostki
				// umiescic kostke odpowiednio wzgledem centrum
				g.transform.localPosition = new Vector3 (x, y, 0);
				
				g.name = "Tile: " + y.ToString () + x.ToString ();
				
				g.GetComponent<Tile> ().Xpos = x;
				g.GetComponent<Tile> ().Ypos = y;
				
				// uncomment to show number
				//g.GetComponentInChildren<TextMesh>().text = g.name;
				
				GameData.Tiles [x, y] = g;
			}
		}
	}

	/// <summary>
	/// Resets the board. Destroy and create again.
	/// </summary>
	public void ResetBoard ()
	{
		DestroyBoard ();
		CreateGems ();

		/*
		 while (CheckStartingBoard())
		{
			DestroyBoard();
			CreateGems();
			print ("TRUE");
		}
		*/
	}

	/// <summary>
	/// Destroies the board.
	/// </summary>
	public void DestroyBoard ()
	{
		for (int y = 0; y < GameData.gridHeight; y++) {
			
			for (int x = 0; x < GameData.gridWidth; x++) {
				if (GameData.GemsTemp [x, y] != null) {
					Destroy (GameData.GemsTemp [x, y]);
				}
			}
		}
		
	}
	
	
	/// <summary>
	/// Checks the starting board.
	/// </summary>
	/// <returns><c>true</c>, if starting board is with matching, <c>false</c> otherwise.</returns>
	public bool CheckStartingBoard ()
	{
		// CHECK FOR HORIZONTAL MATCHES
		for (int y = 0; y < GameData.gridHeight; y++) {
			for (int x = 0; x < GameData.gridWidth - 2; x++) {
				TestGemSphere currentGem = GameData.GemsTemp [x, y].GetComponent<TestGemSphere> ();
				TestGemSphere gem1 = GameData.GemsTemp [x + 1, y].GetComponent<TestGemSphere> ();
				TestGemSphere gem2 = GameData.GemsTemp [x + 2, y].GetComponent<TestGemSphere> ();
				
				if (currentGem.colorType == gem1.colorType && currentGem.colorType == gem2.colorType) {
					print (currentGem.name.ToString () + " Ma w prawo 2 takie same.");
					print (currentGem.colorType);
					
					return true;
				}
			}
		}
		
		// CHECK FOR VERTICAL MATCHES
		for (int y = 0; y < GameData.gridHeight - 2; y++) {
			for (int x = 0; x < GameData.gridWidth; x++) {
				TestGemSphere currentGem = GameData.GemsTemp [x, y].GetComponent<TestGemSphere> ();
				TestGemSphere gem1 = GameData.GemsTemp [x, y + 1].GetComponent<TestGemSphere> ();
				TestGemSphere gem2 = GameData.GemsTemp [x, y + 2].GetComponent<TestGemSphere> ();
				
				if (currentGem.colorType == gem1.colorType && currentGem.colorType == gem2.colorType) {
					print (currentGem.name.ToString () + " Ma w gore 2 takie same.");
					print (currentGem.colorType);
					
					return true;
				}
			}
		}
		
		return false;
	}
}
