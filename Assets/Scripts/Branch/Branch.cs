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
    public class Branch : MonoBehaviour
    {
        public Main.Player Player;

        [SerializeField] private GameObject _branchRendererPrefab;

        public float TurnSpeedDegreesPerSecond = 45f;

        public float GrowthMetersPerSecond = 0.1f;

        private float _branchLength = 0f;
        private float _distanceFromPreviousPoint = 0f;

        /// <summary>
        /// Place a point every x meters in length
        /// </summary>
        public float PointPlacementInterval = 1f;

        public float RandomRotationDegreesOnBranchOff = 30f;

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
                Position = transform.position,
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
            float angle = Main.Instance.GetHorizontal(Player) * turnSpeed;
            transform.Rotate(Vector3.back, angle); // in degrees;

            // Update position of the target transform
            float speed = GrowthMetersPerSecond * deltaTimeSeconds;
            
            float deltaLength = Main.Instance.GetVertical(Player) * speed;

            _branchLength += deltaLength;
            _distanceFromPreviousPoint += deltaLength;

            Vector3 direction = transform.up;
            Vector3 deltaPosition = direction * deltaLength;
            transform.position = transform.position + deltaPosition;

            // set the last point's position
            int lastIndex = _branchRenderer.Points.Count -1;
            _branchRenderer.Points[lastIndex] = new BranchRenderer.Point()
            {
                Position = transform.position + transform.up * 0.1f,
                Width = 0.5f
            };
            _branchRenderer.Points[lastIndex-1] = new BranchRenderer.Point()
            {
                Position = transform.position,
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
                    Position = transform.position,
                    Width = 1.0f
                });

                //SphereCollider newSphereCollider = gameObject.AddComponent<SphereCollider>();
                //newSphereCollider.center = transform.position;
                //newSphereCollider.radius = 0.5f;
            }
        }

        private void BranchOff()
        {
            GameObject newBranchControllerGameObject = Instantiate(gameObject, null, false);
            newBranchControllerGameObject.transform.position = transform.position;
            newBranchControllerGameObject.transform.rotation = Quaternion.Euler(0, 0, newBranchControllerGameObject.transform.rotation.z + Random.Range(-RandomRotationDegreesOnBranchOff, RandomRotationDegreesOnBranchOff));

            Branch newBranchController = newBranchControllerGameObject.GetComponent<Branch>();
            newBranchController.Player = Player;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent<Powerup>(out Powerup powerup))
            {
                switch (powerup.Type)
                {
                    case Powerup.PowerupType.SplitBranch:
                        BranchOff();
                        break;
                    case Powerup.PowerupType.AddWater:
                        Tree tree = Main.Instance.GetTree(Player);
                        tree.AddWater();
                        break;
                    case Powerup.PowerupType.DestroyBranch:
                        Destroy(gameObject);
                        break;
                }

                Destroy(powerup.gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }

    }
}

