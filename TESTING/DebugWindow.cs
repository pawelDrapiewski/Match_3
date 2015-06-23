using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugWindow : MonoBehaviour {


	public Text[] canvasTexts;
	public Text canvasTextRight;
	public Text canvasTextLeft;
	public string debugText;
	public LevelStarter levelStarter;
	public GameData gameData;

	void Awake ()
	{
		levelStarter = GetComponent<LevelStarter>();
		gameData = GetComponent<GameData>();
		canvasTexts = new Text[2];
		canvasTexts = GameObject.FindGameObjectWithTag("UI").GetComponentsInChildren<Text>();
		canvasTextLeft = canvasTexts[1];
		canvasTextRight = canvasTexts[0];

		debugText = canvasTextRight.text;
	}

	// Use this for initialization
	void Start () {
		canvasTextLeft.text = "Destro:" + "\n";
		canvasTextRight.text = "Above:" + "\n";
	}
	
	// Update is called once per frame
	public void UpdateText () {

		for (int i =0; i < GameData.gridWidth; i++)
		{
			foreach (TestGemSphere gem in gameData.AboveInColumn[i])
			{
				if (null == gem)
				{
					//dont remember what is it for :/
					canvasTextRight.text += "Empty at: " + i.ToString() + "\n";
				}
				else if (null != gem)
				{
					canvasTextRight.text += "<color=blue>Column: " + i.ToString() + "</color>" + "\n";
					canvasTextRight.text += "<color=magneta>Gem: " + gem.name.ToString() + "</color>" + "\n";
				}
			}
		}

//		for (int i =0; i < GameData.gridWidth; i++)
//		{
//			foreach (TestGemSphere gem in gameData.DestroInColumn[i])
//			{
//				if (null == gem)
//				{
//					//dont remember what is it for :/
//					canvasTextLeft.text += "Empty at: " + i.ToString() + "\n";
//				}
//				else if (null != gem)
//				{
//					canvasTextLeft.text += "<color=blue>Column: " + i.ToString() + "</color>" + "\n";
//					canvasTextLeft.text += "<color=magneta>Gem: " + gem.name.ToString() + "</color>" + "\n";
//				}
//			}
//		}

		for (int i = 0; i < GameData.gridWidth; i++)
		{
			for (int z = 0; z < gameData.DestroInColumn[i].Count; z++)
			{
				if (null == gameData.DestroInColumn[i][z])
				{
					//dont remember what is it for :/
					canvasTextLeft.text += "Empty at: " + i.ToString() + "\n";
				}
				else if (null != gameData.DestroInColumn[i][z])
				{
					canvasTextLeft.text += i.ToString() + z.ToString() + "\n";
					canvasTextLeft.text += "<color=blue>Column: " + i.ToString() + "</color>" + "\n";
					canvasTextLeft.text += "<color=magneta>Gem: " + gameData.DestroInColumn[i][z].ToString()
						+ "</color>" + "\n";

				}
			}
		}


	}
}
