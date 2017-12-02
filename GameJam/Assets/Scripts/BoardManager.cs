using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{    
    public class BoardManager : MonoBehaviour
    {
        public Building TrumpTower { get; set; }
        public GameObject[] FloorTiles;
        public BuildingFloor CurrentFloor;

        public void Awake()
        {
            this.InitBuilding();
        }

        public void GenerateFloor()
        {
            this.CurrentFloor = this.GenerateFloor(this.TrumpTower.CurrentFloor);
        }

        public BuildingFloor GenerateFloor(int floorNumber)
        {
            var result = new BuildingFloor(floorNumber, this.TrumpTower.GetFloorSize());
            for (int x = 0; x < result.Size.x; x++)
            {
                for (int y = 0; y < result.Size.y; y++)
                {
                    result.gridPositions.Add(new Vector2(x, y), new Vector2(x, y));
                    GameObject toInstantiate = this.FloorTiles[UnityEngine.Random.Range(0, this.FloorTiles.Length)];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(result.FloorHolder);
                }
            }

            return result;
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
        public Dictionary<Vector2, Vector2> gridPositions = new Dictionary<Vector2, Vector2>();
        public int FloorNumber { get; set; }
        public BuildingFloor(int floorNumber, Vector2 size)
        {
            this.Size = size;
            this.FloorNumber = floorNumber;
            this.FloorHolder = new GameObject($"BuildingFloor-{floorNumber}").transform;
        }
    }
}