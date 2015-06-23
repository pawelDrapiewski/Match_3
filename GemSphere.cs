using UnityEngine;
using System.Collections;

public class GemSphere : MonoBehaviour {

	// VARIABLES
	public Transform startingPos;
	public Transform positionOnBoard;
	private Material color;
	public Material[] gemMats;

	public enum State
	{
		selected,
		idle
	}

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
	public float selectedSize = 0.2f;
	// END of variables

	[Range(0.1f,5.0f)]
	public float animationSpeed = 0.1f;

	//TESTING VARIABLES
	public Vector3 moveToPosition; 

	public enum AnimationType
	{
		Size,
		Move
	}

	public AnimationType animationType = AnimationType.Move;


	// Use this for initialization
	void Awake () 
	{
		startingPos = gameObject.transform;
		CreateGem();
	}

	void Start ()
	{
//		moveToPosition = positionOnBoard.position;
//		moveToPosition = new Vector3(positionOnBoard.position.x,
//		                                      positionOnBoard.position.y,
//		                                      positionOnBoard.position.z - 1);
//		print (moveToPosition.ToString());

	}
	
	// Update is called once per frame
	void Update () 
	{

		if (state == State.selected)
		{
			if (animationType == AnimationType.Move)
			{
				//ResetScale();
				//MoveAnimation();
			}

			if (animationType == AnimationType.Size)
			{
				ResetPosition();
				//SizeAnimation();
				MoveForward();
			}
		}

		if (state == State.idle)
		{

		}
	}

	// Randomize colorType
	public void CreateGem()
	{
		colorType = (ColorType)Random.Range(0, 6);
		color = gemMats[(int)colorType];
		//print(colorType.ToString());

		gameObject.GetComponent<Renderer>().material = color;

		state = State.idle;

		//print (color.ToString());
	}

	// Change of state to selected when clicked
	public void ChangeStateOnClick()
	{
		if (state == State.idle)
		{
			state = State.selected;
			//print (state.ToString());
		} 
		else if (state == State.selected)
		{
			state = State.idle;
			//print (state.ToString());
			ResetScale();
			ResetPosition();
		}
	}


	// Animation called when selected
	public void SizeAnimation()
	{
		this.transform.localScale = new Vector3(
			Mathf.PingPong(Time.time /(selectedSize * 10), selectedSize) + 0.8f ,
			Mathf.PingPong(Time.time /(selectedSize * 10), selectedSize) + 0.8f ,
			Mathf.PingPong(Time.time /(selectedSize * 10), selectedSize) + 0.8f );
	}

	// Animation called when selected - second type
	public void MoveAnimation()
	{
		this.transform.position = new Vector3(transform.position.x,
		                                     transform.position.y, 
		                                    Mathf.PingPong(Time.time , -0.5f) + 0.5f);
	}

	public void MoveForward()
	{

		transform.position = Vector3.MoveTowards(startingPos.position,
		                                         moveToPosition,
		                                         animationSpeed * Time.deltaTime);
	}

	public void ResetScale()
	{
		transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
	}

	public void ResetPosition()
	{
		transform.position = positionOnBoard.position;
	}

	void OnMouseDown()
	{
		ChangeStateOnClick();
		//print ("clicked");
		GameBoard board = transform.parent.GetComponent<GameBoard>();
		board.GemSelected(this);
	}
}
