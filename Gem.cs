using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gem : MonoBehaviour {

	public GameObject board;
	public GameObject sphere;
	public GameObject selector;
	private Material color;
	public Material[] gemMats;
	public List<Gem> Neighbors = new List<Gem>();
	public bool isSelected = false;

	//private string name;

	public bool isMatched = false;

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


	// Use this for initialization
	void Start () 
	{
		CreateGem();
		//board = this.transform.parent.gameObject;
		board = GameObject.Find("Board");
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ToggleSelector()
	{
		isSelected = !isSelected;

		selector.SetActive(isSelected);
	}

	void OnMouseDown()
	{
		if(!board.GetComponent<Board>().isSwapping)
		{
			ToggleSelector();
			board.GetComponent<Board>().SwapGems(this);
		}
	}

	public void AddNeighbor(Gem g)
	{
		if (!Neighbors.Contains(g))
		{
			Neighbors.Add(g);
		}
	}

	public void RemoveNeighbor(Gem g)
	{
		Neighbors.Remove(g);
	}

	public void CreateGem()
	{
		color = gemMats[Random.Range(0,gemMats.Length)];
		sphere.GetComponent<Renderer>().material = color;
		sphere.transform.localPosition = Vector3.zero;
		isMatched = false;
		//print (color.ToString());
	}

	public bool IsNeighborWith(Gem gem)
	{
		if (Neighbors.Contains(gem))
		{
			return true;
		} 
		else
		{
			return false;
		}
	}

}
