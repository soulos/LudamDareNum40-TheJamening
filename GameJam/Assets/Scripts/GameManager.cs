using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameState currentState = GameState.Startup;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
	// Use this for initialization
	void Start () {

		
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
    
}
