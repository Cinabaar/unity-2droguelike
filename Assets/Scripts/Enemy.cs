using System.Security.Cryptography;
using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MovingObject
    {

        public int PlayerDamage;

        private Animator _animator;
        private Transform _target;
        private bool _skipMove;
        public AudioClip EnemyAttack1;
        public AudioClip EnemyAttack2;

        protected override void Start ()
        {
            GameManager.Instance.AddEnemyToList(this);
            _animator = GetComponent<Animator>();
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            base.Start();
        }

        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            if (_skipMove)
            {
                _skipMove = false;
                return;
            }
            base.AttemptMove<T>(xDir, yDir);
            _skipMove = true;
        }

        public void MoveEnemy()
        {
            var xDir = 0;
            var yDir = 0;
            if (Mathf.Abs(_target.position.x - transform.position.x) < Mathf.Epsilon)
            {
                yDir = _target.position.x > transform.position.x ? 1 : -1;
            }
            else
            {
                xDir = _target.position.x > transform.position.x ? 1 : -1;
            }
            AttemptMove<Player>(xDir, yDir);
        }

        protected override void OnCantMove<T>(T component)
        {
            var hitPlayer = component as Player;
            _animator.SetTrigger("EnemyAttack");
            hitPlayer.LoseFood(PlayerDamage);
            SoundManager.Instance.RandomizeSfx(EnemyAttack1, EnemyAttack2);
        }
    }
}
