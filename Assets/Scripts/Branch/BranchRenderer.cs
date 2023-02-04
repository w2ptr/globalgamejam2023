using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

namespace GlobalGameJam2023
{
    /// <summary>
    /// Responsible for rendering a branch, points in world space can be set
    /// 
    /// Automatically calculates the bezier curve for smooth branch rendering
    /// </summary>
    [RequireComponent(typeof(SpriteShapeController))]
    public class BranchRenderer : MonoBehaviour
    {
        [SerializeField] private SpriteShapeController _spriteShapeController;

        public float WidthOffset = 0.0f;

        /// <summary>
        /// Constant for making sure the spline does not stop rendering
        /// (even if one of the points has a Height of 0.0f, the entire spline
        /// disappears)
        /// </summary>
        private const float k_MinimumSplineWidth = 0.01f;

        [System.Serializable]
        public struct Point
        {
            public float Width;
            public Vector3 Position; // world space coordinates, perpendicular to object. 
        }

        [SerializeField] private List<Point> _points;

        private void Awake()
        {
            if (_spriteShapeController == null)
            {
                _spriteShapeController = GetComponent<SpriteShapeController>();
            }
        }

        private void Update()
        {
            SetPoints(_points);
            SetCurve();
        }

        private void SetCurve()
        {
            int splinePointCount = _spriteShapeController.spline.GetPointCount();
            for (int i = 0; i < splinePointCount; i++)
            {
                _spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            }
        }

        /// <summary>
        /// List of points in world space
        /// </summary>
        private void SetPoints(List<Point> points)
        {
            // update the amount of points of the spline
            int splinePointCount = _spriteShapeController.spline.GetPointCount();
            
            // Clear points
            if (points.Count == 0)
            {
                _spriteShapeController.spline.Clear();
            }

            // Add points
            if (points.Count > splinePointCount)
            {
                //_spriteShapeController.spline.InsertPointAt(splinePointCount, )
                for (int i=splinePointCount-1; i < points.Count-1; i++)
                {
                    Debug.Log($"Added point at {i}");
                    _spriteShapeController.spline.InsertPointAt(i, points[i].Position);
                }
            }

            // Remove points
            if (points.Count < splinePointCount)
            {
                for (int i=splinePointCount-1; i > points.Count-1; i--)
                {
                    Debug.Log($"Removed point at {i}");
                    _spriteShapeController.spline.RemovePointAt(i);
                }
            }

            // Set positions
            int count = points.Count;
            for (int i=0; i<count; i++)
            {
                _spriteShapeController.spline.SetPosition(i, points[i].Position);
                float width = WidthOffset + points[i].Width;
                width = Mathf.Clamp(width, k_MinimumSplineWidth, Mathf.Infinity);
                _spriteShapeController.spline.SetHeight(i, width);
            }

            Debug.Log(_spriteShapeController.spline.GetPointCount());
        }
    }

}

