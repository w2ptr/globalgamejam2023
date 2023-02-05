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

        public float MinRandomRotationDegreesOnBranchOff = 20f;
        public float MaxRandomRotationDegreesOnBranchOff = 50f;

        private Vector3 _lastPlacedPosition;
        private Quaternion _lastPlacedRotation;

        private BranchRenderer _branchRenderer;

        public float KillingTime;

        [SerializeField] private Material _killingMaterial;
        private Material _normalMaterial;

        private bool __killingMode = false;
        private bool _killingMode
        {
            get => __killingMode;
            set
            {
                __killingMode = value;
                GetComponent<Renderer>().material = __killingMode ? _killingMaterial : _normalMaterial;
            }
        }

        private void Start()
        {
            _normalMaterial = GetComponent<Renderer>().material;
            InstantiateBranchRenderer();
        }

        private void InstantiateBranchRenderer()
        {
            GameObject instantiatedBranchRendererGameObject = Instantiate(_branchRendererPrefab, null, false);
            _branchRenderer = instantiatedBranchRendererGameObject.GetComponent<BranchRenderer>();

            _branchRenderer.AssociatedBranch = this;
            _branchRenderer.Points.Clear();

            _lastPlacedPosition = transform.position;
            BranchRenderer.Point emptyPoint = new BranchRenderer.Point()
            {
                Position = _lastPlacedPosition,
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
            
            float deltaLength = Mathf.Clamp(Main.Instance.GetVertical(Player), 0f, 1f) * speed;

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
                _lastPlacedPosition = transform.position;
                _lastPlacedRotation = transform.rotation;
                // Place a point
                _branchRenderer.Points.Insert(lastIndex, new BranchRenderer.Point()
                {
                    Position = _lastPlacedPosition,
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
            newBranchControllerGameObject.transform.position = _lastPlacedPosition;
            newBranchControllerGameObject.transform.rotation = _lastPlacedRotation;
            newBranchControllerGameObject.transform.Rotate(transform.forward, (Random.Range(0, 1) == 1 ? -1 : 1) * Random.Range(MinRandomRotationDegreesOnBranchOff, MaxRandomRotationDegreesOnBranchOff));

            Main.Instance.PlayersData[Player].BranchCount++;

            Branch newBranchController = newBranchControllerGameObject.GetComponent<Branch>();
            newBranchController.Player = Player;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer == Main.Instance.GetOtherPlayerLayerMask(Player))
            {
                DestroyBranch();
                Debug.Log("Bumped into other player");
            }

            if (collision.collider.TryGetComponent<BranchRenderer>(out BranchRenderer branchRenderer))
            {
                if (_killingMode)
                {
                    // collided with
                    if (branchRenderer.AssociatedBranch != null)
                    {
                        branchRenderer.AssociatedBranch.DestroyBranch();
                        _killingMode = false;
                        StopAllCoroutines();
                    }
                }
            }

            if (collision.collider.TryGetComponent<Powerup>(out Powerup powerup))
            {
                switch (powerup.Type)
                {
                    case Powerup.PowerupType.SplitBranch:
                        BranchOff();
                        break;
                    case Powerup.PowerupType.AddWater:
                        Tree tree = Main.Instance.PlayersData[Player].Tree;
                        tree.AddWater();
                        break;
                    case Powerup.PowerupType.DestroyBranch:
                        DestroyBranch();
                        break;
                    case Powerup.PowerupType.KillOtherBranch:
                        _killingMode = true;
                        StartCoroutine(StopKilling());
                        break;
                }

                Destroy(powerup.gameObject);
            }
        }

        private IEnumerator StopKilling()
        {
            yield return new WaitForSeconds(KillingTime);
            _killingMode = false;
        }

        public void DestroyBranch()
        {
            Main.Instance.PlayersData[Player].BranchCount--;
            Main.Instance.PlayersData[Player].InstantiateIfNeeded();
            Destroy(_branchRenderer.gameObject);
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }

    }
}

