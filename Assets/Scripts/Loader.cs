using UnityEngine;

namespace Assets.Scripts
{
    public class Loader : MonoBehaviour
    {
        public GameObject GameManager;          //GameManager prefab to instantiate.

        void Awake()
        {
            //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
            if (Scripts.GameManager.Instance == null)
            {
                //Instantiate gameManager prefab
                Instantiate(GameManager);
            }
        }
    }
}
