using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private BoardManager boardManager;

    private PlayerController player;

    private int currentLevel = 1;

    void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        player = FindObjectOfType<PlayerController>();
        currentLevel = 1;
    }

	void Start ()
    {
        GenerateLevel(1);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void GenerateLevel(int level)
    {
        this.boardManager?.GenerateFloor();
    }

    public void IncreaseLevel()
    {
        currentLevel++;
        GenerateLevel(currentLevel);

    }

}
