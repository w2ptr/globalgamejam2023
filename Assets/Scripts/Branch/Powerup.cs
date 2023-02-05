using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalGameJam2023
{
    public class Powerup : MonoBehaviour
    {
        [System.Serializable]
        public enum PowerupType
        {
            SplitBranch,
            AddWater,
            DestroyBranch,
            KillOtherBranch,
        }

        public PowerupType Type;

        private const float speed = 0.03f;

        void Start()
        {
            var collider = GetComponent<Collider2D>();
            collider.enabled = false;
        }

        void Update()
        {
            var collider = GetComponent<Collider2D>();
            if (!collider.enabled)
            {
                if (transform.position.z >= 0)
                {
                    collider.enabled = true;
                    transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                }
                else
                {
                    transform.Translate(0, 0, speed);
                }
            }
        }
    }
}
