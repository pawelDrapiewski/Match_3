using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBoard : MonoBehaviour {

	public GameObject[,] Tiles;
	public GameObject[,] GemsTemp;
	public List<GameObject> Gems;
	public int gridWidth = 6;
	public int gridHeight = 6;
	public GameObject tilePrefab;
	public GameObject gemPrefab;
	public float spacing = 0.2f;
	public float speed = 5f;
	public GemSphere firstSelected;
	public GemSphere secondSelected;


	//TEST



	// Use this for initialization
	void Start () {

		// init arrays
		Tiles = new GameObject[gridWidth,gridHeight];
		GemsTemp = new GameObject[gridWidth,gridHeight];
		
		// TILES
		CreateTiles();

		// SPHERES
		CreateSpheres();

		// TEST
		while (CheckStartingBoard())
		{
			DestroyBoard();
			CreateSpheres();
			print ("TRUE");
		}

	}
	
	// Update is called once per frame
	void Update () {


		// MOVE SPHERES make this coroutine later????
		for (int y = 0; y < gridHeight; y++)
		{
			speed = 10f;

			for (int x = 0; x < gridWidth; x++)
			{
				if (GemsTemp[x,y] != null)
				{
					GemSphere gemObject = GemsTemp[x,y].GetComponent<GemSphere>();

					speed -= 2 * Time.deltaTime;
					if (speed < 5) speed = 5;


					GemsTemp[x,y].transform.position = Vector3.MoveTowards(gemObject.startingPos.position,
				                                                Tiles[x,y].transform.position,
				                                                speed * Time.deltaTime);
										
					//gemObject.positionOnBoard = Tiles[x,y].transform;				
				}
			}
		}


	}

	public void ResetBoard()
	{
		DestroyBoard();
		CreateSpheres();
		while (CheckStartingBoard())
		{
			DestroyBoard();
			CreateSpheres();
			print ("TRUE");
		}
	}

	public void DestroyBoard()
	{
		for (int y = 0; y < gridHeight; y++)
		{
			
			for (int x = 0; x < gridWidth; x++)
			{
				Destroy(GemsTemp[x,y]);
			}
		}

	}

	public void CreateSpheres()
	{
		for (int y = 0; y < gridHeight; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{
				GameObject g = Instantiate (gemPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
				
				g.transform.parent = gameObject.transform;
				
				// W przyszlosci wyliczyc dokladna pozycje miedzy dwoma filarami
				// centrum = ilosc kostek w rzedzie / 2 - pol kostki
				// umiescic kostke odpowiednio wzgledem centrum
				g.transform.localPosition = new Vector3(x, y + 15 + (y * 0.3f), 0);
				
				g.name = y.ToString() + x.ToString();
				
				// uncomment to show number 
				//g.GetComponentInChildren<TextMesh>().text = g.name;

				GemsTemp[x,y] = g;

				GemSphere gemObject = GemsTemp[x,y].GetComponent<GemSphere>();

				gemObject.positionOnBoard = Tiles[x,y].transform;
				gemObject.moveToPosition = gemObject.positionOnBoard.position;
				gemObject.moveToPosition = new Vector3(gemObject.positionOnBoard.position.x,
				                             gemObject.positionOnBoard.position.y,
				                             gemObject.positionOnBoard.position.z - 1);
			}
		}
	}

	public bool CheckStartingBoard()
	{
		// CHECK FOR HORIZONTAL MATCHES
		for (int y = 0; y < gridHeight; y++)
		{
			for (int x = 0; x < gridWidth - 2; x++)
			{
				GemSphere currentGem = GemsTemp[x,y].GetComponent<GemSphere>();
				GemSphere gem1 = GemsTemp[x+1,y].GetComponent<GemSphere>();
				GemSphere gem2 = GemsTemp[x+2,y].GetComponent<GemSphere>();
				
				if (currentGem.colorType == gem1.colorType && currentGem.colorType == gem2.colorType)
				{
					print (currentGem.name.ToString() + " Ma w prawo 2 takie same.");
					print (currentGem.colorType);

					return true;
				}
			}
		}

		// CHECK FOR VERTICAL MATCHES
		for (int y = 0; y < gridHeight - 2; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{
				GemSphere currentGem = GemsTemp[x,y].GetComponent<GemSphere>();
				GemSphere gem1 = GemsTemp[x,y+1].GetComponent<GemSphere>();
				GemSphere gem2 = GemsTemp[x,y+2].GetComponent<GemSphere>();
				
				if (currentGem.colorType == gem1.colorType && currentGem.colorType == gem2.colorType)
				{
					print (currentGem.name.ToString() + " Ma w gore 2 takie same.");
					print (currentGem.colorType);

					return true;
				}
			}
		}

		return false;
	}

	public void CreateTiles()
	{
		for (int y = 0; y < gridHeight; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{
				GameObject g = Instantiate (tilePrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
				g.transform.parent = gameObject.transform;
				
				// W przyszlosci wyliczyc dokladna pozycje miedzy dwoma filarami
				// centrum = ilosc kostek w rzedzie / 2 - pol kostki
				// umiescic kostke odpowiednio wzgledem centrum
				g.transform.localPosition = new Vector3(x, y, 0);
				
				g.name = "Tile: " + y.ToString() + x.ToString();
				
				// uncomment to show number
				//g.GetComponentInChildren<TextMesh>().text = g.name;
				
				Tiles[x,y] = g;
			}
		}
	}

	public void GemSelected(GemSphere gemSelected)
	{
		if (firstSelected == null)
		{
			firstSelected = gemSelected;
		}
		else if (secondSelected == null)
		{
			secondSelected = gemSelected;
		}
		else if (secondSelected != null)
		{
			//firstSelected.ChangeStateOnClick();
			secondSelected = firstSelected;
			firstSelected = gemSelected;
			//secondSelected.ChangeStateOnClick();
		}
	}

	void OnMouseDown()
	{
		print ("clicked");
	}
}
