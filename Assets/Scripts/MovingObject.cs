using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class MovingObject : MonoBehaviour
    {
        public float MoveTime = 0.1f;
        public LayerMask BlockingLayer;

        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody;
        private float _inverseMoveTime;


        // Use this for initialization
        protected virtual void Start ()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _inverseMoveTime = 1.0f/MoveTime;
        }

        protected IEnumerator SmoothMovement(Vector3 end)
        {
            var sqRemainingDistance = (transform.position - end).sqrMagnitude;
            while (sqRemainingDistance > float.Epsilon)
            {
                var newPosition = Vector3.MoveTowards(_rigidbody.position, end, _inverseMoveTime*Time.deltaTime);
                _rigidbody.MovePosition(newPosition);
                sqRemainingDistance = (transform.position - end).sqrMagnitude;
                yield return null;
            }
        }
        protected abstract void OnCantMove<T>(T component)
            where T : Component;

        protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
        {
            var start = (Vector2)transform.position;
            var end = start + new Vector2(xDir, yDir);

            _boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, BlockingLayer);
            _boxCollider.enabled = true;
            if (hit.transform == null)
            {
                StartCoroutine(SmoothMovement(end));
                return true;
            }
            return false;
        }

        protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
        {
            RaycastHit2D hit;
            var canMove = Move(xDir, yDir, out hit);

            if (canMove)
            {
                return;
            }

            var hitComponent = hit.transform.GetComponent<T>();
            if (hitComponent != null)
            {
                OnCantMove(hitComponent);
            }
        }

        // Update is called once per frame
        void Update () {
	
        }
    }
}
