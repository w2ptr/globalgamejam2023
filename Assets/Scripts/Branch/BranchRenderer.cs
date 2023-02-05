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
        /// The scale of the transform
        /// </summary>
        public float Scale = 0.1f;

        /// <summary>
        /// World scale Y position
        /// </summary>
        public float Height = 0.5f;

        /// <summary>
        /// Constant for making sure the spline does not stop rendering
        /// (even if one of the points has a Height of 0.0f, the entire spline
        /// disappears)
        /// </summary>
        private const float k_MinimumSplineWidth = 0.01f;

        public float CurveScaleOffset = 1.0f;

        /// <summary>
        /// To make sure the InserPointAt doesn't fail (Point too close to neighbour error)
        /// </summary>
        private const float k_MinimumLocalPositionDistance = 0.05f;

        private const float k_GizmoSphereScale = 0.1f;

        [System.Serializable]
        public struct Point
        {
            public float Width;

            /// <summary>
            /// World space coordinates, perpendicular to object. 
            /// </summary>
            public Vector3 Position; 
        }

        public List<Point> Points;

        private void Awake()
        {
            if (_spriteShapeController == null)
            {
                _spriteShapeController = GetComponent<SpriteShapeController>();
            }
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            SetTransform();
            SetPoints(Points);
            SetCurve();
        }

        private void SetTransform()
        {
            transform.localPosition = new Vector3(0f, 0f, -Height);
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            transform.localScale = Vector3.one * Scale;
        }

        private void SetCurve()
        {
            Spline spline = _spriteShapeController.spline;

            int splinePointCount = spline.GetPointCount();
            for (int i = 0; i < splinePointCount; i++)
            {
                spline.SetTangentMode(i, ShapeTangentMode.Continuous);

                Vector3 point = spline.GetPosition(i);
                Vector3 previousPoint = i > 0 ? spline.GetPosition(i - 1) : point;
                Vector3 nextPoint = i < splinePointCount - 1 ? spline.GetPosition(i + 1) : point;

                float scale = CurveScaleOffset; /// Scale;
                float leftControlPointScale = scale * Vector3.Distance(point, previousPoint);
                float rightControlPointScale = scale * Vector3.Distance(point, nextPoint);

                SplineUtility.CalculateTangents(point, previousPoint, nextPoint, Vector3.up, leftControlPointScale, out Vector3 _, out Vector3 leftTangent);
                SplineUtility.CalculateTangents(point, previousPoint, nextPoint, Vector3.up, rightControlPointScale, out Vector3 rightTangent, out Vector3 _);

                _spriteShapeController.spline.SetLeftTangent(i, leftTangent);
                _spriteShapeController.spline.SetRightTangent(i, rightTangent);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            for (int i=0; i<Points.Count; i++)
            {
                Vector3 worldSpacePosition = Points[i].Position;
                Gizmos.DrawSphere(worldSpacePosition, k_GizmoSphereScale);
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
                //Debug.Log("Should add points");
                for (int i=splinePointCount; i < points.Count; i++)
                {
                    //Debug.Log($"Added point at {i}");
                    _spriteShapeController.spline.InsertPointAt(i, GetLocalPosition(i, points[i].Position));
                }
            }

            // Remove points
            if (points.Count < splinePointCount)
            {
                //Debug.Log("Should remove points");
                for (int i=splinePointCount-1; i > points.Count-1; i--)
                {
                    //Debug.Log($"Removed point at {i}");
                    _spriteShapeController.spline.RemovePointAt(i);
                }
            }

            // Set positions
            int count = points.Count;
            for (int i=0; i<count; i++)
            {
                _spriteShapeController.spline.SetPosition(i, GetLocalPosition(i, points[i].Position));
                float width = WidthOffset + points[i].Width;
                width = Mathf.Clamp(width, k_MinimumSplineWidth, Mathf.Infinity);
                _spriteShapeController.spline.SetHeight(i, width);
            }

            //Debug.Log(_spriteShapeController.spline.GetPointCount());
        }

        /// <summary>
        /// Cnverts the curve from world space to local space
        /// </summary>
        private Vector2 GetLocalPosition(int index, Vector3 worldSpacePosition)
        {
            Vector2 xy = new Vector2(worldSpacePosition.x, worldSpacePosition.y);
            Vector2 scaled = xy / Scale;
            Vector2 localPosition = scaled;

            //Debug.Log($"Getting local{index}");
            if (index > 0)
            {
                //Debug.Log("Should check");
                // Make sure the point gets offset if they come too close
                Vector3 previousLocalPointPosition = _spriteShapeController.spline.GetPosition(index-1);
                float distance = Vector3.Distance(localPosition, previousLocalPointPosition);
                if (distance < k_MinimumLocalPositionDistance)
                {
                    localPosition = new Vector2(localPosition.x, localPosition.y + k_MinimumLocalPositionDistance - distance);
                    //Debug.Log(localPosition);
                }
            }
            
            return localPosition;
        }
    }

}

