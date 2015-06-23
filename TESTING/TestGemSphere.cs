using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TestGemSphere : MonoBehaviour, IComparable
{
	
	// VARIABLES
	public Transform startingPos;
	private Material color;
	public Material[] gemMats;
	public Tile actualTile;
	public int Xpos;
	public int Ypos;
	public List<TestGemSphere> matchedGemsAround;

	public List<TestGemSphere> horizontalMatchedList;
	public List<TestGemSphere> verticalMatchedList;
	
	public enum State
	{
		selected,
		idle
	}


	//get set for colors
	public enum ColorType
	{
		Blue,
		Green,
		Orange,
		Purple,
		Red,
		White,
		Yellow
	}
	
	public ColorType colorType;
	public State state;

	[Range(0.0f,1.0f)]
	public float
		selectedSize = 0.2f;
	// END of variables
	
	[Range(0.1f,5.0f)]
	public float
		animationSpeed = 1f;
	
	//TESTING VARIABLES
	public Vector3 moveToPosition;
	public int updateCounter = 0;
	
	
	// Use this for initialization
	void Awake ()
	{
		//print ( "Gem AWAKE.");
		startingPos = gameObject.transform;
		CreateGem ();
	}

	void Start ()
	{
		//print ( "Gem START.");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (state == State.selected) {
			
			//ResetPosition();
			SizeAnimation ();
//			StartCoroutine("MoveForward");
			
		}
		
		if (state == State.idle) {
			ResetScale ();
			//StopCoroutine("MoveForward");
			//ResetPosition();
		}
	}
	
	// Randomize colorType
	public void CreateGem ()
	{
		colorType = (ColorType)UnityEngine.Random.Range (0, 6);
		color = gemMats [(int)colorType];
		//print(colorType.ToString());
		
		gameObject.GetComponent<Renderer> ().material = color;
		
		state = State.idle;
		
		//print (color.ToString());
	}
	
	// Change of state to selected when clicked
	public void ToggleStateOnClick ()
	{
		if (state == State.idle) {
			state = State.selected;
			//print (state.ToString());
		} else if (state == State.selected) {
			state = State.idle;
			//print (state.ToString());
			//ResetScale();
			//ResetPosition();
		}
	}
	
	
	// Animation called when selected
	public void SizeAnimation ()
	{
		this.transform.localScale = new Vector3 (
			Mathf.PingPong (Time.time / (selectedSize * 10), selectedSize) + 0.8f,
			Mathf.PingPong (Time.time / (selectedSize * 10), selectedSize) + 0.8f,
			Mathf.PingPong (Time.time / (selectedSize * 10), selectedSize) + 0.8f);
	}
	
	// Animation called when selected - second type
	public void MoveAnimation ()
	{
		this.transform.position = new Vector3 (transform.position.x,
		                                      transform.position.y, 
		                                      Mathf.PingPong (Time.time, -0.5f) + 0.5f);
	}
	
	IEnumerator MoveForward (TestGemSphere gem)
	{ 
		//print ("coroutine started");
		while (Vector3.Distance(gem.transform.position, gem.moveToPosition) > 0.05f) {
			//print (gem.transform.position.ToString() + " " + gem.moveToPosition.ToString());
			gem.transform.position = Vector3.MoveTowards (gem.transform.position,
			                                  gem.moveToPosition,
			                                  1f * Time.deltaTime);
			yield return null;
		}
		if (Vector3.Distance (gem.transform.position, gem.moveToPosition) < 0.05f) {
			gem.transform.position = gem.moveToPosition;
		}
		//print ("DONE MoveForward");

		TestGameBoard board = transform.parent.GetComponent<TestGameBoard> ();
		board.gameState = TestGameBoard.GameState.stopAnimation;

	}
	
	public void ResetScale ()
	{
		transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
		//transform.rotation = new Quaternion(45f, 45f, 0f, 0f);
	}
	
	public void ResetPosition ()
	{
		transform.position = ActualTilePosition();
	}
	
	void OnMouseDown ()
	{
		TestGameBoard board = transform.parent.GetComponent<TestGameBoard> ();
		//board.StopCoroutine("MoveToBoard");
		//StartCoroutine(MoveForward(this));
		if (board.gameState != TestGameBoard.GameState.swapingGemsAnimation) {
			ToggleStateOnClick ();
			//print ("clicked");

			//board.gameState = TestGameBoard.GameState.testState;
			board.GemSelected (this);
		}
	}

	/// <summary>
	/// Gets the actual tile vector position.
	/// </summary>
	/// <returns>The actual tile vector position.</returns>
	public Vector3 ActualTilePosition()
	{
		return actualTile.transform.position;
	}


	//Default sorting is by Ypos
	int IComparable.CompareTo(object obj)
	{
		TestGemSphere c=(TestGemSphere)obj;
		return String.Compare(this.Ypos.ToString(),c.Ypos.ToString());
	}
}