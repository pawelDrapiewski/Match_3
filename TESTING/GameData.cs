using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour {

	public static GameObject[,] Tiles;
	public static GameObject[,] GemsTemp;
	public List<GameObject> Gems;
	public static int gridWidth = 6;
	public static int gridHeight = 6;
	public GameObject tilePrefab;
	public GameObject gemPrefab;
	public float spacing = 0.2f;
	public float speed = 10f;
	public TestGemSphere firstSelected;
	public TestGemSphere secondSelected;
	public List<TestGemSphere> horizontalMatchedList;
	public List<TestGemSphere> verticalMatchedList;
	public List<TestGemSphere> gemsAbove;
	public List<TestGemSphere> gemsDestroyed;
	public List<TestGemSphere> movedGems;

	public List<TestGemSphere>[] DestroInColumn;
	public List<TestGemSphere>[] AboveInColumn;



	// Use this for initialization
	void Awake () {
		Tiles = new GameObject[gridWidth, gridHeight];
		GemsTemp = new GameObject[gridWidth, gridHeight];
		DestroInColumn = new List<TestGemSphere>[gridWidth];
		for (int i = 0; i < gridWidth; i++)
		{
			DestroInColumn[i] = new List<TestGemSphere>();
		}
		AboveInColumn = new List<TestGemSphere>[gridWidth];
		for (int i = 0; i < gridWidth; i++)
		{
			AboveInColumn[i] = new List<TestGemSphere>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
