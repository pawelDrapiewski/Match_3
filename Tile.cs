using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public int updateCounter = 0;

	public int Xpos;
	public int Ypos;

	[SerializeField]
	private TestGemSphere actualGem;

	public TestGemSphere ActualGem
	{
		get 
		{
			return actualGem;
		}

		set
		{
			actualGem = value;
		}
	}
	

	// Use this for initialization
	void Awake ()
	{
		//print ( "Tile AWAKE.");
	}

	void OnEnable()
	{
		//print ( "Tile ONENABLE.");
	}

	void Start () {
		//print ( "Tile START.");
	}
	
	// Update is called once per frame
	void Update () {

		if (updateCounter < 4)
		{
			//print (updateCounter.ToString() + "update counter GEM.");
			updateCounter++;
		}
	}

	void OnMouseDown()
	{
		print ("clicked tile");
	}
}
