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
        private class Constants
        {
            public static int MinNumInteractablesPerLevel { get; private set; } = 10;
            public static float PercentInteractableCoverage { get; private set; } = .04f;
            public static int NumberOfFloorsInTrumpTower { get; private set; } = 58;
        }

        public Building TrumpTower { get; set; }
        public GameObject[] FloorTiles;
        public GameObject[] WallTiles;
        public GameObject[] InteractableTiles;
        public GameObject ExitTile;
        public BuildingFloor CurrentFloor;
        public float tileScale = 2f;
        public void Awake()
        {
            this.InitBuilding();
        }

        public class LevelBuildingState
        {
            public Dictionary<Vector2, GameObject> QueuedTiles { get; set; } = new Dictionary<Vector2, GameObject>();
        }
        
        public void GenerateFloor()
        {
            this.CurrentFloor = new BuildingFloor(this.TrumpTower.CurrentFloor, this.TrumpTower.GetFloorSize(), this.tileScale);
            var state = new LevelBuildingState();
            this.GenerateRandomState(state, this.CurrentFloor.Size);
            for (int x = 0; x < this.CurrentFloor.Size.x; x++)
            {
                for (int y = 0; y < this.CurrentFloor.Size.y; y++)
                {
                    var tile = this.SelectTileForPosition(new Vector2(x* tileScale, y * tileScale), state);
                    tile.transform.SetParent(this.CurrentFloor.FloorHolder);
                }
            }
        }

        public void GenerateRandomState(LevelBuildingState state, Vector2 size)
        {
            // populate some configurable sparsity of patterns of tile placement, like cubicle groups
            // never place groups of objects along the walls in such a way that they block the exit or interfere with the starting position

            // first  pass, let's just create a random number of water coolers and position them about
            var numToQueue = (int)Urandom.Range(Constants.MinNumInteractablesPerLevel, size.x * size.y * Constants.PercentInteractableCoverage);
            for (int i = 0; i < numToQueue; i++)
            {
                var tile = this.InteractableTiles[Urandom.Range(0, InteractableTiles.Length)];
                Vector2? pos = null;
                do
                {
                    pos = new Vector2((int)Urandom.Range(1, size.x - 2) * tileScale, (int)Urandom.Range(1, size.y - 2) * tileScale); 
                    if (state.QueuedTiles.ContainsKey(pos.Value))
                    {
                        pos = null;
                    }
                    else if (pos == this.CurrentFloor.ExitPosition || pos == this.CurrentFloor.StartPosition)
                    {
                        pos = null;
                    }
                } while (pos == null);

                state.QueuedTiles.Add(pos.Value, tile);
            }
        }

        private GameObject SelectTileForPosition(Vector2 pos, LevelBuildingState state)
        {
            GameObject tile = null;
            var isExit = false;
            if (IsWallPosition((int)pos.x, (int)pos.y))
            {
                if (pos.x == this.CurrentFloor.ExitPosition.x && pos.y == this.CurrentFloor.ExitPosition.y)
                {
                    tile = ExitTile;
                    isExit = true;
                }
                else
                {
                    tile = this.WallTiles[Urandom.Range(0, this.WallTiles.Length)];
                }
            }
            else if (state.QueuedTiles.ContainsKey(pos))
            {
                // Constructing some sort of group of related tiles
                tile = state.QueuedTiles[pos];
            }
            else
            {
                // Floor tiles
                tile = this.FloorTiles[Urandom.Range(0, this.FloorTiles.Length)];
            }

            GameObject result = Instantiate(tile, new Vector3(pos.x, pos.y, 0f), Quaternion.identity) as GameObject;

            if (isExit)
            {
                RotateExitToFaceInward(pos, result);
            }

            return result;
        }

        private void RotateExitToFaceInward(Vector2 pos, GameObject result)
        {
            var angle = 0; // top wall
            if (pos.y == 0)
            {
                angle = 180; // bottom wall
            }
            else if (pos.x == 0)
            {
                angle = 90; // left wall
            }
            else if (pos.x == (this.CurrentFloor.Size.x - 1) * tileScale)
            {
                angle = 270; // right wall
            }

            result.transform.RotateAround(
                result.GetComponent<Renderer>().bounds.center,
                Vector3.forward,
                angle
            );
        }

        private bool IsWallPosition(int x, int y)
        {
            var lastMostX =  this.CurrentFloor.Size.x - 1;
            var lastMostY = this.CurrentFloor.Size.y - 1;
            return (y == 0 || x == 0 || y == lastMostY * tileScale || x == lastMostX * tileScale);
        }

        private void InitBuilding()
        {
            this.TrumpTower = new Building()
            {
                NumberOfFloors = Constants.NumberOfFloorsInTrumpTower,
                CurrentFloor = Constants.NumberOfFloorsInTrumpTower
            };
        }
        public Vector3 GetStart()
        {
            return CurrentFloor.GetRandomPositionInsideWalls();
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
        public float TileScale { get; }
        public Transform FloorHolder;
        public int FloorNumber { get; set; }
        public Vector2 ExitPosition { get; set; }
        public Vector2 StartPosition { get; set; }
        public BuildingFloor(int floorNumber, Vector2 size, float tileScale, Vector2? prevExitPosition = null)
        {
            this.Size = size;
            this.TileScale = tileScale;
            this.FloorNumber = floorNumber;
            this.ExitPosition = this.GetRandomPositionAlongWall();
            this.StartPosition = prevExitPosition ?? GetRandomPositionInsideWalls();
            this.FloorHolder = new GameObject($"BuildingFloor-{floorNumber}").transform;
        }

        public Vector2 GetRandomPositionInsideWalls()
        {
            return new Vector2(Urandom.Range(1, this.Size.x), Urandom.Range(1, this.Size.y));
        }

        private Vector2 GetRandomPositionAlongWall()
        {
            int x;
            var xType = Urandom.Range(0, 3);
            if (xType == 0)
            {
                x = 0; // left wall
            }
            else if (xType == 1)
            {
                x = (int)this.Size.x - 1; // right wall
            }
            else
            {
                x = (int)Urandom.Range(1, this.Size.x - 2); // top or bottom walls from 0 -> x avoiding corners
            }

            int y; 
            if (x == 0 || x == this.Size.x - 1)
            {
                // If 'x' is constrained to the left or right walls then y is free to spread along its range
                y = (int)Urandom.Range(1, this.Size.y - 2); // top or bottom walls from 0 -> y avoiding corners
            }
            else
            {
                // otherwise, 'x' is free, so y is constrained
                y = Urandom.Range(0, 2) > 0 ? (int)this.Size.y - 1 : 0;
            }
            var result = new Vector2(x * TileScale, y * TileScale);
            return result;
        }

       
    }
}