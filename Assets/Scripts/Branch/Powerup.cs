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
            DestroyBranch
        }

        public PowerupType Type;
    }
}
