using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private BoardManager boardManager;

    private PlayerController player;

    public UIUpdateMainGame UI;
    private int currentLevel = 1;
    

    void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        GetPlayer();
        currentLevel = 1;
    }

	void Start ()
    {
        GenerateLevel(1);
        
    }

    void GetPlayer()
    {
        player = FindObjectOfType<PlayerController>();
    }
	// Update is called once per frame
	void FixedUpdate ()
	{
	    if (player == null)
	    {
	        GetPlayer();

	    }
	    UI.UpdateHealthMeter(player.GetHealthPercent());
	}

    public void GenerateLevel(int level)
    {
        this.boardManager?.GenerateFloor();
        SpawnPlayer();
    }

    public void IncreaseLevel()
    {
        HidePlayer();
        currentLevel++;
        GenerateLevel(currentLevel);


    }

    void HidePlayer()
    {
        player.gameObject.SetActive(false);
    }

    void SpawnPlayer()
    {

        player.transform.position = boardManager.GetStart();
        player.gameObject.SetActive(true);
    }

}
