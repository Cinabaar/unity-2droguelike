using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public float LevelStartDelay = 2f;
        public float TurnDelay = .1f;
        public static GameManager Instance;              //Static instance of GameManager which allows it to be accessed by any other script.
        private BoardManager _boardScript;                       //Store a reference to our BoardManager which will set up the level.
        private int Level = 1; //Current level number, expressed in game as "Day 1".
        public int PlayerFoodPoints = 20;
        [HideInInspector] public bool PlayersTurn = true;

        private Text _levelText;
        private GameObject _levelImage;
        private List<Enemy> _enemies;
        private bool _enemiesMoving;
        private bool _doingSetup;


        private void OnLevelWasLoaded()
        {
            Level++;
            InitGame();
        }

        //Awake is always called before any Start functions
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            _enemies = new List<Enemy>();
            _boardScript = GetComponent<BoardManager>();
            InitGame();
        }

        //Initializes the game for each level.
        void InitGame()
        {
            //Call the SetupScene function of the BoardManager script, pass it current level number.
            _doingSetup = true;

            _levelImage = GameObject.Find("LevelImage");
            _levelText = GameObject.Find("LevelText").GetComponent<Text>();
            _levelText.text = "Day " + Level;
            _levelImage.SetActive(true);
            Invoke("HideLevelImage", LevelStartDelay);

            _enemies.Clear();
            _boardScript.SetupScene(Level);
        }

        private void HideLevelImage()
        {
            _levelImage.SetActive(false);
            _doingSetup = false;
        }

        public void GameOver()
        {
            _levelText.text = "After " + Level + " days you died.";
            _levelImage.SetActive(true);
            enabled = false;
        }

        IEnumerator MoveEnemies()
        {
            _enemiesMoving = true;
            yield return new WaitForSeconds(TurnDelay);
            if (_enemies.Count == 0)
            {
                yield return new WaitForSeconds(TurnDelay);
            }
            foreach(var enemy in _enemies)
            {
                enemy.MoveEnemy();
                yield return new WaitForSeconds(enemy.MoveTime);
            }
            PlayersTurn = true;
            _enemiesMoving = false;
        }

        void Update()
        {
            if (PlayersTurn || _enemiesMoving || _doingSetup)
                return;
            StartCoroutine(MoveEnemies());
        } 

        public void AddEnemyToList(Enemy script)
        {
            _enemies.Add(script);
        }

    }
}
