using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameState currentState = GameState.Startup;

    private BoardManager BoardScript;

    void Awake()
    {
        DontDestroyOnLoad(this);
        this.BoardScript = this.GetComponent<BoardManager>();
    }

	// Use this for initialization
	void Start () {
        this.BoardScript.GenerateFloor();
    }
    
    // Update is called once per frame
    void Update () {

        
		
	}

    public static void ChangeState(GameState newState)
    {
        
        if (currentState == newState)
        {
            return;
        }
        currentState = newState;
        switch (newState)
        {
            case GameState.Startup:
                SceneManager.LoadScene("Startup");
                break;
            case GameState.MainMenu:
                SceneManager.LoadScene("MainMenu");
                break;
            case GameState.StartGame:
                SceneManager.LoadScene("Level1");
                break;
            case GameState.DiedAlcohol:
                SceneManager.LoadScene("DeadDrunk");
                break;
            case GameState.DiedBullet:
                SceneManager.LoadScene("DeadBullet");
                break;
            case GameState.DiedZombie:
                SceneManager.LoadScene("DeadZombie");
                break;
            case GameState.GotHighscore:
                SceneManager.LoadScene("HighScore");
                break;
            default:
                break;
        }
        

    }

    public void NewGame()
    {
        ChangeState(GameState.StartGame);
    }

    public void About()
    {
        // show the about Panel
        GameObject.Find("MainPanel").SetActive(false);
        GameObject.Find("AboutPanel").SetActive(true);

    }

    public void BackButton()
    {
        // if on highscore load main menu
        if (currentState == GameState.ShowHighscores)
        {
            SceneManager.LoadScene("Startup");
        }
        // otherwise show main panel
        GameObject.Find("MainPanel").SetActive(true);
        GameObject.Find("AboutPanel").SetActive(false);
    }

    public void HighScores()
    {
        ChangeState(GameState.ShowHighscores);
    }

    public void QuitGame()
    {
        // Scream
        // Belch
        // Show blood
        Application.Quit();
    }
    
}
