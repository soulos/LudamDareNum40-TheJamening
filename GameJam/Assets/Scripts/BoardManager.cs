using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Urandom = UnityEngine.Random;

namespace Assets.Scripts
{    
    public class BoardManager : MonoBehaviour
    {
        public Building TrumpTower { get; set; }
        public GameObject[] FloorTiles;
        public GameObject[] WallTiles;
        public GameObject[] InteractableTiles;
        public GameObject ExitTile;
        public BuildingFloor CurrentFloor;

        public void Awake()
        {
            this.InitBuilding();
        }

        public void GenerateFloor()
        {
            this.CurrentFloor = new BuildingFloor(this.TrumpTower.CurrentFloor, this.TrumpTower.GetFloorSize());            
            for (int x = 0; x < this.CurrentFloor.Size.x; x++)
            {
                for (int y = 0; y < this.CurrentFloor.Size.y; y++)
                {
                    var tile = this.SelectTileForPosition(x, y);
                    tile.transform.SetParent(this.CurrentFloor.FloorHolder);
                }
            }
        }

        private GameObject SelectTileForPosition(int x, int y)
        {
            GameObject tile = null;
            var isExit = false;
            if (IsWallPosition(x, y))
            {
                if (x == this.CurrentFloor.ExitPosition.x && y == this.CurrentFloor.ExitPosition.y)
                {
                    tile = ExitTile;
                    isExit = true;
                }
                else
                {
                    tile = this.WallTiles[Urandom.Range(0, this.WallTiles.Length)];
                }
            }
            else
            {
                // Floor tiles
                tile = this.FloorTiles[Urandom.Range(0, this.FloorTiles.Length)];
            }

            GameObject result = Instantiate(tile, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

            //if (isExit)
            //{
            //    result.transform.localRotation = Quaternion.Euler(0, -90, 0);
            //}

            return result;
        }

        private bool IsWallPosition(int x, int y)
        {
            var lastMostX = this.CurrentFloor.Size.x - 1;
            var lastMostY = this.CurrentFloor.Size.y - 1;
            return (y == 0 || x == 0 || y == lastMostY || x == lastMostX);
        }

        private void InitBuilding()
        {
            this.TrumpTower = new Building()
            {
                NumberOfFloors = 58,
                CurrentFloor = 58
            };
        }
    }

    public class Building
    {
        public int NumberOfFloors { get; set; }
        public int CurrentFloor { get; set; }        

        public Vector2 GetFloorSize()
        {
            // basically, we want the floors to get larger as the player descends
            // but maybe something cooler could take place
            var level = this.NumberOfFloors - this.CurrentFloor;
            return new Vector2(100 - this.CurrentFloor + 10 * level, 100 - this.CurrentFloor + 10 * level);
        }

        public void PlayerDescended()
        {
            this.CurrentFloor--;
        }
    }

    public class BuildingFloor
    {
        public Vector2 Size;
        public Transform FloorHolder;
        public int FloorNumber { get; set; }
        public Vector2 ExitPosition { get; set; }
        public BuildingFloor(int floorNumber, Vector2 size)
        {
            this.Size = size;
            this.FloorNumber = floorNumber;
            this.ExitPosition = this.GetRandomPositionAlongWall();
            this.FloorHolder = new GameObject($"BuildingFloor-{floorNumber}").transform;
        }

        private Vector2 GetRandomPositionAlongWall()
        {
            var result = new Vector2(Urandom.Range(0, 2) > 0 ? this.Size.x - 1 : 0, Urandom.Range(0, (int)this.Size.y - 1));
            return result;
        }
    }
}