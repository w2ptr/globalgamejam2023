using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalGameJam2023
{
    public class Tree : MonoBehaviour
    {
        public float WaterScaleIncrease = 0.1f;

        public void AddWater()
        {
            transform.localScale = transform.localScale + Vector3.one * WaterScaleIncrease;
        }
    }
}
