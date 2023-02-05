using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GlobalGameJam2023
{
    /// <summary>
    /// The branch controller is responsible for controlling a branch
    /// Is controlled by the player
    /// </summary>
    public class BranchController : MonoBehaviour
    {
        [SerializeField] private GameObject _branchRendererPrefab;

        public float TurnSpeedDegreesPerSecond = 45f;

        /// <summary>
        /// The transform that is used as the branch grow direction.
        /// </summary>
        public Transform TargetTransform;

        public float GrowthMetersPerSecond = 0.1f;

        private float _branchLength = 0f;
        private float _distanceFromPreviousPoint = 0f;

        private int _pointsCountFromLastBranchOff;

        public int BranchOffCount;

        /// <summary>
        /// Place a point every x meters in length
        /// </summary>
        public float PointPlacementInterval = 1f;

        private BranchRenderer _branchRenderer;

        private void Start()
        {
            InstantiateBranchRenderer();
        }

        private void InstantiateBranchRenderer()
        {
            GameObject instantiatedBranchRendererGameObject = Instantiate(_branchRendererPrefab, null, false);
            _branchRenderer = instantiatedBranchRendererGameObject.GetComponent<BranchRenderer>();

            _branchRenderer.Points.Clear();
            BranchRenderer.Point emptyPoint = new BranchRenderer.Point()
            {
                Position = TargetTransform.position,
                Width = 1.0f
            };
            _branchRenderer.Points = new List<BranchRenderer.Point>()
            {
                emptyPoint,
                emptyPoint,
                emptyPoint
            };
        }

        private void Update()
        {
            float deltaTimeSeconds = Time.deltaTime;

            // Update rotation of the target transform
            float turnSpeed = TurnSpeedDegreesPerSecond * deltaTimeSeconds;
            float angle = Main.Instance.HorizontalInput * turnSpeed;
            TargetTransform.Rotate(Vector3.up, angle); // in degrees;

            // Update position of the target transform
            float speed = GrowthMetersPerSecond * deltaTimeSeconds;
            
            float deltaLength = Main.Instance.VerticalInput * speed; 

            _branchLength += deltaLength;
            _distanceFromPreviousPoint += deltaLength;

            Vector3 direction = TargetTransform.forward;
            Vector3 deltaPosition = direction * deltaLength;
            TargetTransform.position = TargetTransform.position + deltaPosition;

            // set the last point's position
            int lastIndex = _branchRenderer.Points.Count -1;
            _branchRenderer.Points[lastIndex] = new BranchRenderer.Point()
            {
                Position = TargetTransform.position + TargetTransform.forward,
                Width = 0.5f
            };
            _branchRenderer.Points[lastIndex-1] = new BranchRenderer.Point()
            {
                Position = TargetTransform.position,
                Width = 0.7f
            };

            //Debug.Log(_distanceFromPreviousPoint);

            if (_distanceFromPreviousPoint > PointPlacementInterval)
            {
                //Debug.Log("Over threshold");
                _distanceFromPreviousPoint = 0.0f;
                // Place a point
                _branchRenderer.Points.Insert(lastIndex, new BranchRenderer.Point()
                {
                    Position = TargetTransform.position,
                    Width = 1.0f
                });

                _pointsCountFromLastBranchOff += 1;

                if (_pointsCountFromLastBranchOff >= BranchOffCount)
                {
                    void BranchOff()
                    {
                        // Branch off
                        GameObject newTargetTransformGameObject = Instantiate(Main.Instance.TargetTransformPrefab, null, false);
                        Transform newTargetTransform = newTargetTransformGameObject.transform;
                        newTargetTransform.position = TargetTransform.position;
                        newTargetTransform.rotation = TargetTransform.rotation;//Quaternion.Euler(0, TargetTransform.rotation.y + Random.Range(20, 20), 0);
                        GameObject newBranchControllerGameObject = Instantiate(Main.Instance.BranchControllerPrefab, null, false);
                        BranchController newBranchController = newBranchControllerGameObject.GetComponent<BranchController>();
                        newBranchController.TargetTransform = newTargetTransform.transform;
                        newBranchController.GrowthMetersPerSecond = GrowthMetersPerSecond * 1.5f;// Random.Range(1f, 10f);
                        newBranchController.TurnSpeedDegreesPerSecond = TurnSpeedDegreesPerSecond * 1.2f;
                        newBranchController.BranchOffCount = BranchOffCount * 2;//Random.Range(5, 20);
                    }

                    BranchOff();

                    _pointsCountFromLastBranchOff = 0;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(TargetTransform.position, 0.1f);
        }

    }
}

