using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelWin : MonoBehaviour
    {

        private BoardManager boardManager;
        private GameObject player;
        private PlayerController playerController;

        private bool isOpen = false;
        // Use this for initialization
        void Start()
        {
            var board = GameObject.Find("Board");
            boardManager = board.GetComponent<BoardManager>();
            player = GameObject.Find("Player");
            playerController = player.GetComponent<PlayerController>();
        }

        public void SetExitOpen(bool open)
        {
            isOpen = open;
        }
        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (isOpen && col.collider.gameObject.CompareTag("Player"))
            {
                this.player.SetActive(false);
                this.playerController.LevelWin();
                Thread.Sleep(1000);
                // TODO: Fade to black 
                this.boardManager.GenerateFloor();
                this.player.transform.position = boardManager.CurrentFloor.StartPosition;
                this.player.SetActive(true);
            }
        }
    } 
}
