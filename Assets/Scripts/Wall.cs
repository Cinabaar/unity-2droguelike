using UnityEngine;

namespace Assets.Scripts
{
    public class Wall : MonoBehaviour
    {
        public Sprite DamageSprite;
        public int Hp = 4;
        public AudioClip ChopSound1;
        public AudioClip ChopSound2;


        private SpriteRenderer _spriteRenderer;
        // Use this for initialization
        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void DamageWall(int loss)
        {
            SoundManager.Instance.RandomizeSfx(ChopSound1, ChopSound2);
            _spriteRenderer.sprite = DamageSprite;
            Hp -= loss;
            if (Hp <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
