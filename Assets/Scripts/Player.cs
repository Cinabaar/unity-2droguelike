using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Player : MovingObject
    {
        public int WallDamage = 1;
        public int PointsPerFood = 10;
        public int PointsPerSoda = 20;
        public float RestartLevelDelay = 1f;
        public Text FoodText;
        public AudioClip MoveSound1;
        public AudioClip MoveSound2;
        public AudioClip EatSound1;
        public AudioClip EatSound2;
        public AudioClip DrinkSound1;
        public AudioClip DrinkSound2;
        public AudioClip GameOverSound;

        private Animator _animator;
        private int _food;

        // Use this for initialization
        protected override void Start ()
        {
            _animator = GetComponent<Animator>();
            _food = GameManager.Instance.PlayerFoodPoints;

            FoodText.text = "Food: " + _food;
            base.Start();
        }

        private void OnDisable()
        {
            GameManager.Instance.PlayerFoodPoints = _food;
        }

        private void CheckIfGameOver()
        {
            if (_food <= 0)
            {
                GameManager.Instance.GameOver();
                SoundManager.Instance.RandomizeSfx(GameOverSound);
                SoundManager.Instance.MusicSource.Stop();
            }
        }

        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            _food--;
            FoodText.text = "Food: " + _food;
            base.AttemptMove<T>(xDir, yDir);
            RaycastHit2D hit;
            if (Move(xDir, yDir, out hit))
            {
                SoundManager.Instance.RandomizeSfx(MoveSound1, MoveSound2);
            }
            GameManager.Instance.PlayersTurn = false;
        }

        protected override void OnCantMove<T>(T component)
        {
            var hitWall = component as Wall;
            hitWall.DamageWall(WallDamage);
            _animator.SetTrigger("PlayerChop");
        }

        private void Restart()
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        public void LoseFood(int loss)
        {
            _animator.SetTrigger("PlayerHit");
            _food -= loss;
            FoodText.text = "-" + loss + " Food: " + _food;
            CheckIfGameOver();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Exit")
            {
                Invoke("Restart", RestartLevelDelay);
                enabled = false;
            }
            else if (other.tag == "Food")
            {
                _food += PointsPerFood;
                other.gameObject.SetActive(false);
                FoodText.text = "+" + PointsPerFood + " Food: " + _food;
                SoundManager.Instance.RandomizeSfx(EatSound1, EatSound2);
            }
            else if (other.tag == "Soda")
            {
                _food += PointsPerSoda;
                other.gameObject.SetActive(false);
                FoodText.text = "+" + PointsPerSoda + " Food: " + _food;
                SoundManager.Instance.RandomizeSfx(DrinkSound1, DrinkSound2);
            }
        }

        // Update is called once per frame
        void Update () {
	        if(!GameManager.Instance.PlayersTurn) return;

            var horizontal = (int) Input.GetAxisRaw("Horizontal");
            var vertical = (int) Input.GetAxisRaw("Vertical");

            if (horizontal != 0)
                vertical = 0;

            if (horizontal != 0 || vertical != 0)
            {
                AttemptMove<Wall>(horizontal, vertical);
                CheckIfGameOver();
            }
        }
    }
}
