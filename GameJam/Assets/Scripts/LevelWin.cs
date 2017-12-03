using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelWin : MonoBehaviour
    {

        private BoardManager boardManager;
        private GameObject player;
        // Use this for initialization
        void Start()
        {
            var board = GameObject.Find("Board");
            boardManager = board.GetComponent<BoardManager>();
            player = GameObject.Find("Player");
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter2D(Collision2D col)
        {
            this.player.SetActive(false);
            // TODO: Fade to black 
            this.boardManager.GenerateFloor();
            this.player.transform.position = this.boardManager.GetOffsetToPositionWall(boardManager.CurrentFloor.StartPosition, false);
            this.player.SetActive(true);
        }
    } 
}
