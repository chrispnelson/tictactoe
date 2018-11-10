using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[System.Serializable]
public class Player {
	public Image panel;
	public Text text;
	public Button button;
}

[System.Serializable]
public class PlayerColor {
	public Color panelColor;
	public Color textColor;
}

public class GameController : MonoBehaviour {

	public Text[] ButtonList;
	public GameObject gameOverPanel;
	public Text gameOverText;
	public GameObject restartButton;
	
	public Player playerX;
	public Player playerO;
	public PlayerColor activePlayerColor;
	public PlayerColor inactivePlayerColor;

	private string playerSide;
	private int moveCount;

	private enum WINSTATE
	{
		X,
		O,
		D
	};
	
	void Awake ()
	{
		SetGameControllerReferenceOnButtons();
		gcInit();
	}
	
	void SetGameControllerReferenceOnButtons ()
	{
		for (int i = 0; i < ButtonList.Length; i++)
		{
			ButtonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
		}
	}

	void gcInit()
	{
		playerSide = "X";
		gameOverPanel.SetActive(false);
		restartButton.SetActive(false);
		gameOverText.text = playerSide + " Wins!";
		moveCount = 0;
		SetPlayerColors(playerX, playerO);
		SetBoardInteractable(true);
	}
	
	void SetPlayerButtons (bool toggle)
	{
		playerX.button.interactable = toggle;
		playerO.button.interactable = toggle;  
	}

	public void setStartingSide(string ws)
	{
		if (ws.ToUpper() == "X")
		{
			playerSide = "X";
			SetPlayerColors(playerX, playerO);
		}

		else if (ws.ToUpper() == "O")
		{
			playerSide = "O";
			SetPlayerColors(playerO, playerX);
		}

		else
		{
			playerSide = "X";
			SetPlayerColors(playerX, playerO);
		}
	}

	public string GetPlayerSide ()
	{
		return playerSide;
	}
	
	public void EndTurn ()
	{
		moveCount ++;
		
		/* ROWS */
		
		if (ButtonList[0].text == playerSide && 
		    ButtonList[1].text == playerSide && 
		    ButtonList[2].text == playerSide)
		{
			GameOver(getWSEnumFromString(playerSide));
		}
		else if (ButtonList[3].text == playerSide && 
		         ButtonList[4].text == playerSide &&
		         ButtonList[5].text == playerSide)
		{
			GameOver(getWSEnumFromString(playerSide));
		}
		else if (ButtonList[6].text == playerSide && 
		         ButtonList[7].text == playerSide && 
		         ButtonList[8].text == playerSide)
		{
			GameOver(getWSEnumFromString(playerSide));
		}
		
		/* COLUMNS */
		
		else if (ButtonList[0].text == playerSide && 
				 ButtonList[3].text == playerSide && 
				 ButtonList[6].text == playerSide)
		{
			GameOver(getWSEnumFromString(playerSide));
		}
		else if (ButtonList[1].text == playerSide && 
				 ButtonList[4].text == playerSide && 
				 ButtonList[7].text == playerSide)
		{
			GameOver(getWSEnumFromString(playerSide));
		}
		else if (ButtonList[2].text == playerSide && 
				 ButtonList[5].text == playerSide && 
				 ButtonList[8].text == playerSide)
		{
			GameOver(getWSEnumFromString(playerSide));
		}
		
		/* DIAGONALS */
		
		else if (ButtonList[0].text == playerSide && 
				 ButtonList[4].text == playerSide && 
				 ButtonList[8].text == playerSide)
		{
			GameOver(getWSEnumFromString(playerSide));
		}
		else if (ButtonList[2].text == playerSide && 
				 ButtonList[4].text == playerSide && 
				 ButtonList[6].text == playerSide)
		{
			GameOver(getWSEnumFromString(playerSide));
		}
		
		/* DRAW CONDITION MET */
		
		else if (moveCount >= 9)
		{
			GameOver(WINSTATE.D);
		}

		else
		{
			ChangeSides();
		}
	}
	
	public void RestartGame()
	{
		gcInit();
		SetPlayerButtons (true);
		SetPlayerColorsInactive();
	}

	void StartGame ()
	{
		SetBoardInteractable(true);
		SetPlayerButtons (false);
	}

	void SetBoardInteractable (bool toggle)
	{
		for (int i = 0; i < ButtonList.Length; i++)
		{
			ButtonList[i].GetComponentInParent<Button>().interactable = toggle;
			
			if (toggle)
			{
				ButtonList[i].text = "";
			}
		}
	}

	string getEnumString(WINSTATE ws)
	{
		string _ws_ = "";

		if (ws == WINSTATE.X)
		{
			_ws_ = "X";
		}
		if (ws == WINSTATE.O)
		{
			_ws_ = "O";
		}		
		if (ws == WINSTATE.D)
		{
			_ws_ = "D";
		}

		return _ws_;
	}

	WINSTATE getWSEnumFromString(string ws)
	{
		WINSTATE _ws_ = WINSTATE.D;

		if (ws.ToUpper() == "X")
		{
			_ws_ = WINSTATE.X;
		}
		if (ws.ToUpper() == "O")
		{
			_ws_ = WINSTATE.O;
		}
		if (ws.ToUpper() == "D")
		{
			_ws_ = WINSTATE.D;
		}

		return _ws_;
	}
	
	void SetPlayerColorsInactive ()
	{
		playerX.panel.color = inactivePlayerColor.panelColor;
		playerX.text.color = inactivePlayerColor.textColor;
		playerO.panel.color = inactivePlayerColor.panelColor;
		playerO.text.color = inactivePlayerColor.textColor;
	}
	
	void SetPlayerColors (Player newPlayer, Player oldPlayer)
	{
		newPlayer.panel.color = activePlayerColor.panelColor;
		newPlayer.text.color = activePlayerColor.textColor;
		oldPlayer.panel.color = inactivePlayerColor.panelColor;
		oldPlayer.text.color = inactivePlayerColor.textColor;
	}

	void ChangeSides ()
	{
		playerSide = (playerSide == "X") ? "O" : "X";

		if (getWSEnumFromString(playerSide) == WINSTATE.X)
		{
			SetPlayerColors(playerX, playerO);
		}
		if (getWSEnumFromString(playerSide) == WINSTATE.O)
		{
			SetPlayerColors(playerO, playerX);
		} 
	}
	
	void GameOver (WINSTATE ws)
	{
		restartButton.SetActive(true);
		
		SetBoardInteractable(false);

		if (ws == WINSTATE.X)
		{
			SetGameOverText("X Wins!");
		}
		else if (ws == WINSTATE.O)
		{
			SetGameOverText("O Wins!");
		}
		else if (ws == WINSTATE.D)
		{
			SetGameOverText("DRAW GAME!");
			SetPlayerColorsInactive();
		}
		else
		{
			Debug.LogError("WINSTATE unknown");
		}
	}
	
	void SetGameOverText(string value)
	{
		gameOverPanel.SetActive(true);
		gameOverText.text = value;
	}
}
