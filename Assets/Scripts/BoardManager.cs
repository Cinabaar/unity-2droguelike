using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class BoardManager : MonoBehaviour
    {
        [Serializable]
        public class Count
        {
            public int Minimum;
            public int Maximum;

            public Count(int minimum, int maximum)
            {
                Minimum = minimum;
                Maximum = maximum;
            }
        }

        public int Rows = 8;
        public int Columns = 8;
        public Count WallCount = new Count(5, 9);
        public Count FoodCount = new Count(1, 5);
        public GameObject Exit;
        public GameObject[] FloorTiles;
        public GameObject[] WallTiles;
        public GameObject[] FoodTiles;
        public GameObject[] EnemyTiles;
        public GameObject[] OuterWallTiles;

        private Transform _boardHolder;
        private List<Vector3> _gridPositions = new List<Vector3>();

        void InitializeList()
        {
            _gridPositions.Clear();
            for (var x = 1; x < Columns - 1; x++)
            {
                for (var y = 1; y < Rows - 1; y++)
                {
                    _gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }

        void BoardSetup()
        {
            _boardHolder = new GameObject("Board").transform;

            for (var x = -1; x < Columns + 1; x++)
            {
                for (var y = -1; y < Rows + 1; y++)
                {
                    var toInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];
                    if (x == -1 || x == Columns || y == -1 || y == Rows)
                    {
                        toInstantiate = OuterWallTiles[Random.Range(0, OuterWallTiles.Length)];
                    }
                    var instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                    instance.transform.SetParent(_boardHolder);
                }
            }
        }

        Vector3 RandomPosition()
        {
            var randomIndex = Random.Range(0, _gridPositions.Count);
            var randomPosition = _gridPositions[randomIndex];
            _gridPositions.RemoveAt(randomIndex);
            return randomPosition;
        }

        void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
        {
            //Choose a random number of objects to instantiate within the minimum and maximum limits
            var objectCount = Random.Range(minimum, maximum + 1);

            //Instantiate objects until the randomly chosen limit objectCount is reached
            for (var i = 0; i < objectCount; i++)
            {
                //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
                var randomPosition = RandomPosition();

                //Choose a random tile from tileArray and assign it to tileChoice
                var tileChoice = tileArray[Random.Range(0, tileArray.Length)];

                //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
                Instantiate(tileChoice, randomPosition, Quaternion.identity);
            }
        }
        public void SetupScene (int level)
        {
            //Creates the outer walls and floor.
            BoardSetup ();
            //Reset our list of gridpositions.
            InitializeList ();
            //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (WallTiles, WallCount.Minimum, WallCount.Maximum);
            //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (FoodTiles, FoodCount.Minimum, FoodCount.Maximum);
            //Determine number of enemies based on current level number, based on a logarithmic progression
            var enemyCount = (int)Mathf.Log(level, 2f);
            //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (EnemyTiles, enemyCount, enemyCount);
            //Instantiate the exit tile in the upper right hand corner of our game board
            Instantiate (Exit, new Vector3 (Columns - 1, Rows - 1, 0f), Quaternion.identity);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
