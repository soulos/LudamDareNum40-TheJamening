using Assets.Scripts.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Unrandom = UnityEngine.Random;

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
        public GameObject[] EnemySpawnTiles;
        public GameObject ExitTile;
        public GameObject WinConditionTile;
        public Dictionary<Vector2, GameObject> TilesByPosition { get; set; } = new Dictionary<Vector2, GameObject>();

        internal void SomethingDied(Transform thing)
        {
            if (thing.tag == "EnemySpawner")
            {
                SpawnerDestroyed();
            }
        }

        private void SpawnerDestroyed()
        {
            this.CurrentFloor.SpawnersDestroyed++;
            if (this.CurrentFloor.SpawnersDestroyed == this.CurrentFloor.NumSpawners)
            {
                // TODO: Some kind of state machine would be nice here
                this.OpenExit();
            }
        }

        private void OpenExit()
        {
            GameObject exit;
            if (this.TilesByPosition.TryGetValue(this.CurrentFloor.ExitPosition, out exit))
            {
                exit.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        public LevelState CurrentFloor;
        public float tileScale = 2f;

        public void Awake()
        {
            this.InitBuilding();            
        }

        public void GenerateFloor()
        {
            this.CurrentFloor = this.GenerateRandomState(this.TrumpTower.GetFloorSize(), this.CurrentFloor);
            for (int x = 0; x < this.CurrentFloor.BoardSize.x; x++)
            {
                for (int y = 0; y < this.CurrentFloor.BoardSize.y; y++)
                {
                    var tile = this.SelectTileForPosition(new Vector2(x* tileScale, y * tileScale));
                    tile.transform.SetParent(this.CurrentFloor.FloorHolder);
                }
            }
        }

        public LevelState GenerateRandomState(Vector2 size, LevelState previousState = null)
        {
            // populate some configurable sparsity of patterns of tile placement, like cubicle groups
            // never place groups of objects along the walls in such a way that they block the exit or interfere with the starting position
            var state = new LevelState(this.CurrentFloor?.FloorNumber - 1 ?? this.TrumpTower.NumberOfFloors, tileScale, size);
            state.ExitPosition = this.GetRandomPositionAlongWall(state);
            state.StartPosition = previousState?.ExitPosition ?? GetRandomPositionInsideWalls(state);
            state.NumSpawners = this.TrumpTower.NumberOfFloors - this.TrumpTower.CurrentFloorNumber + 1;
            for (int i = 0; i < state.NumSpawners; i++)
            {
                var spawner = this.EnemySpawnTiles.RandomElement();
                Vector2? pos = GetAvailablePos(state, size);
                state.QueuedTiles.Add(pos.Value, spawner);
            }

            // first  pass, let's just create a random number of water coolers and position them about
            var numOfInteractables = (int)Unrandom.Range(Constants.MinNumInteractablesPerLevel, size.x * size.y * Constants.PercentInteractableCoverage);
            for (int i = 0; i < numOfInteractables; i++)
            {
                var tile = this.InteractableTiles.RandomElement();
                Vector2? pos = GetAvailablePos(state, size);
                state.QueuedTiles.Add(pos.Value, tile);
            }

            return state;
        }

        private static Vector2? GetAvailablePos(LevelState state, Vector2 size)
        {
            Vector2? pos;
            do
            {
                // Avoids walls outright by the 1 and (-2)
                pos = new Vector2((int)Unrandom.Range(1, size.x - 2) * state.TileScale, (int)Unrandom.Range(1, size.y - 2) * state.TileScale);
                if (state.QueuedTiles.ContainsKey(pos.Value))
                {
                    pos = null;
                }
                else if (pos == state.ExitPosition || pos == state.StartPosition)
                {
                    pos = null;
                }
            } while (pos == null);
            return pos;
        }

        private GameObject SelectTileForPosition(Vector2 pos)
        {
            GameObject tile = null;
            var isExit = false;
            //var msg = "";
            if (IsWallPosition((int)pos.x, (int)pos.y))
            {
                if (pos.x == this.CurrentFloor.ExitPosition.x && pos.y == this.CurrentFloor.ExitPosition.y)
                {
                    //msg = "exit";
                    tile = ExitTile;
                    isExit = true;
                }
                else
                {
                    //msg = "rnd wal";
                    tile = this.WallTiles.RandomElement();
                }
            }
            else if (this.CurrentFloor.QueuedTiles.ContainsKey(pos))
            {
                //msg = "queued";
                // Constructing some sort of group of related tiles
                tile = this.CurrentFloor.QueuedTiles[pos];
            }
            else
            {
                //msg = "floor";
                // Floor tiles
                tile = this.FloorTiles.RandomElement();
            }

            //Debug.LogError(msg);
            GameObject result = Instantiate(tile, new Vector3(pos.x, pos.y, 0f), Quaternion.identity) as GameObject;
            this.TilesByPosition[pos] = result;

            var enemyDie = result.GetComponent<EnemyDie>();
            if (enemyDie != null)
            {
                enemyDie.boardManager = this;
            }

            if (isExit)
            {
                RotateExitToFaceInward(pos, result);
                // Create the win box behind the exit
                Vector2 toUse = GetOffsetToPositionOutsideWall(pos);
                Instantiate(this.WinConditionTile, new Vector3(pos.x + toUse.x, pos.y + toUse.y, 0f), Quaternion.identity);
            }

            return result;
        }

        private Vector2 GetOffsetToPositionOutsideWall(Vector2 pos)
        {
            Vector2 result;
            var roomside = GetRoomSide(pos);
            if (roomside == RoomSide.Top)
            {
                result = new Vector2(0, tileScale);
            }
            else if (roomside == RoomSide.Bottom)
            {
                result = new Vector2(0, -tileScale);
            }
            else if (roomside == RoomSide.Left)
            {
                result = new Vector2(-tileScale, 0);
            }
            else
            {
                result = new Vector2(tileScale, 0);
            }
            return result;
        }

        public RoomSide GetRoomSide(Vector2 pos)
        {
            var result = RoomSide.None;
            if (pos.y == 0)
            {
                result = RoomSide.Bottom;
            }
            else if (pos.x == 0)
            {
                result = RoomSide.Left;
            }
            else if (pos.x == (this.CurrentFloor.BoardSize.x - 1) * tileScale)
            {
                result = RoomSide.Right;
            }
            else if (pos.y == (this.CurrentFloor.BoardSize.y - 1) * tileScale)
            {
                result = RoomSide.Top;
            }
            return result;
        }

        private void RotateExitToFaceInward(Vector2 pos, GameObject result)
        {
            var roomside = GetRoomSide(pos);
            var angle = 0; // top wall
            if (roomside == RoomSide.Bottom)
            {
                angle = 180;
            }
            else if (roomside == RoomSide.Left)
            {
                angle = 90;
            }
            else if (roomside == RoomSide.Right)
            {
                angle = 270;
            }

            result.transform.RotateAround(
                result.GetComponent<Renderer>().bounds.center,
                Vector3.forward,
                angle
            );
        }

        private bool IsWallPosition(int x, int y)
        {
            var lastMostX =  this.CurrentFloor.BoardSize.x - 1;
            var lastMostY = this.CurrentFloor.BoardSize.y - 1;
            return (y == 0 || x == 0 || y == lastMostY * tileScale || x == lastMostX * tileScale);
        }

        public Vector2 GetRandomPositionInsideWalls(LevelState state = null)
        {
            if (this.CurrentFloor == null && state == null)
            {
                throw new Exception("Can't get a random position inside the walls until the board is generated or you pass in state.");
            }

            // If I weren't a bastard I wouldn't do this this way io
            return new Vector2(
                Unrandom.Range(1 * (state ?? this.CurrentFloor).TileScale, (state ?? this.CurrentFloor).BoardSize.x * (state ?? this.CurrentFloor).TileScale), 
                Unrandom.Range(1 * (state ?? this.CurrentFloor).TileScale, (state ?? this.CurrentFloor).BoardSize.y * (state ?? this.CurrentFloor).TileScale)
            );
        }

        private Vector2 GetRandomPositionAlongWall(LevelState state = null)
        {
            if (this.CurrentFloor == null && state == null)
            {
                throw new Exception("Can't get a random position until the board is generated or you pass in state.");
            }

            int x;
            var xType = Unrandom.Range(0, 3);
            if (xType == 0)
            {
                x = 0; // left wall
            }
            else if (xType == 1)
            {
                x = (int)(state ?? this.CurrentFloor).BoardSize.x - 1; // right wall
            }
            else
            {
                x = (int)Unrandom.Range(1, (state ?? this.CurrentFloor).BoardSize.x - 2); // top or bottom walls from 0 -> x avoiding corners
            }

            int y;
            if (x == 0 || x == (state ?? this.CurrentFloor).BoardSize.x - 1)
            {
                // If 'x' is constrained to the left or right walls then y is free to spread along its range
                y = (int)Unrandom.Range(1, (state ?? this.CurrentFloor).BoardSize.y - 2); // top or bottom walls from 0 -> y avoiding corners
            }
            else
            {
                // otherwise, 'x' is free, so y is constrained
                y = Unrandom.Range(0, 2) > 0 ? (int)(state ?? this.CurrentFloor).BoardSize.y - 1 : 0;
            }
            var result = new Vector2(x * (state ?? this.CurrentFloor).TileScale, y * (state ?? this.CurrentFloor).TileScale);
            return result;
        }

        private void InitBuilding()
        {            
            this.TrumpTower = new Building()
            {
                NumberOfFloors = Constants.NumberOfFloorsInTrumpTower,
                CurrentFloorNumber = Constants.NumberOfFloorsInTrumpTower
            };
        }

        public Vector3 GetStart()
        {
            return GetRandomPositionInsideWalls();
        }
    }

    public class LevelState
    {        
        public LevelState(int floorNumber, float tileScale, Vector2 size)
        {
            this.BoardSize = size;
            this.TileScale = tileScale;
            this.FloorNumber = floorNumber;
            this.FloorHolder = new GameObject($"BuildingFloor-{this.FloorNumber}").transform;
        }       
        public Dictionary<Vector2, GameObject> QueuedTiles { get; set; } = new Dictionary<Vector2, GameObject>();
        public Vector2 ExitPosition { get; set; }
        public Vector2 StartPosition { get; set; }
        public int NumSpawners { get; internal set; }
        public int SpawnersDestroyed { get; internal set; }
        public int FloorNumber { get; set; }
        public float TileScale { get; set; }
        public Vector2 BoardSize { get; set; }
        public Transform FloorHolder;
    }

    public class Building
    {
        public int NumberOfFloors { get; set; }
        public int CurrentFloorNumber { get; set; }        

        public Vector2 GetFloorSize()
        {
            // basically, we want the floors to get larger as the player descends
            // but maybe something cooler could take place
            var level = this.NumberOfFloors - this.CurrentFloorNumber;
            return new Vector2(100 - this.CurrentFloorNumber + 10 * level, 100 - this.CurrentFloorNumber + 10 * level);
        }

        public void PlayerDescended()
        {
            this.CurrentFloorNumber--;
        }
    }
}