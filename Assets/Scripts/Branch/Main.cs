using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GlobalGameJam2023
{
    public class Main : MonoBehaviour, GGJ2023InputActions.IGeneralActions
    {
        private static Main _instance;
        public static Main Instance => _instance;

        private GGJ2023InputActions _ggj2023InputActions;

        private float _player1HorizontalInput = 0f;
        private float _player1VerticalInput = 0f;

        private float _player2HorizontalInput = 0f;
        private float _player2VerticalInput = 0f;

        //public GameObject BranchControllerPrefab;
        //public GameObject TargetTransformPrefab;

        [SerializeField] private Tree _player1Tree;
        [SerializeField] private Tree _player2Tree;

        public GameObject _player1BranchPrefab;
        public GameObject _player2BranchPrefab;

        public enum Player
        {
            Player1,
            Player2
        }

        public class PlayerData
        {
            public Tree Tree;
            public GameObject BranchPrefab;

            public List<Branch> Branches = new List<Branch>();

            public void AddBranch(Branch branch)
            {
                Branches.Add(branch);
            }

            public void InstantiateBranch()
            {
                GameObject newBranchControllerGameObject = Instantiate(BranchPrefab, null, false);
                newBranchControllerGameObject.transform.position = Tree.transform.position;
                Branch branch = newBranchControllerGameObject.GetComponent<Branch>();
                AddBranch(branch);
                Debug.Log(Branches.Count);
            }

            public void RemoveBranch(Branch branch)
            {
                Debug.Log(branch);
                Debug.Log(Branches.Count);
                Branches.Remove(branch);
                Debug.Log(Branches.Count);

                if (Branches.Count == 0)
                {
                    InstantiateBranch();
                }

                Destroy(branch._branchRenderer.gameObject);
                Destroy(branch.gameObject);
            }
        }

        public Dictionary<Player, PlayerData> PlayersData;

        [SerializeField] private string Player1LayerMask;
        [SerializeField] private string Player2LayerMask;

        public int GetOtherPlayerLayerMask(Player player)
        {
            string layerMaskName = "";
            switch (player)
            {
                case Player.Player1:
                    layerMaskName = Player2LayerMask;
                    break;
                case Player.Player2:
                    layerMaskName = Player1LayerMask;
                    break;
            }
            return LayerMask.NameToLayer(layerMaskName);
        }

        public float GetHorizontal(Player player)
        {
            switch (player)
            {
                case Player.Player1:
                    return _player1HorizontalInput;
                case Player.Player2:
                    return _player2HorizontalInput;
                default:
                    return 0.0f;
            }
        }

        public float GetVertical(Player player)
        {
            switch (player)
            {
                case Player.Player1:
                    return _player1VerticalInput;
                case Player.Player2:
                    return _player2VerticalInput;
                default:
                    return 0.0f;
            }
        }

        private void Awake()
        {
            // Singleton implemention
            if (_instance != null && _instance != this) { Destroy(this); } else { _instance = this; }
        }

        private void Start()
        {
            PlayersData = new Dictionary<Player, PlayerData>()
            {
                {
                    Player.Player1,
                    new PlayerData()
                    {
                        Tree = _player1Tree,
                        BranchPrefab = _player1BranchPrefab
                    }
                },
                {
                    Player.Player2,
                    new PlayerData()
                    {
                        Tree = _player2Tree,
                        BranchPrefab = _player2BranchPrefab
                    }
                }
            };

            PlayersData[Player.Player1].InstantiateBranch();
            PlayersData[Player.Player2].InstantiateBranch();

            if (_ggj2023InputActions == null)
            {
                _ggj2023InputActions = new GGJ2023InputActions();
            }

            _ggj2023InputActions.General.SetCallbacks(this);
            _ggj2023InputActions.General.Enable();
        }

        void GGJ2023InputActions.IGeneralActions.OnPlayer1_HorizontalAxis(InputAction.CallbackContext context)
        {
            _player1HorizontalInput = context.ReadValue<float>();
        }

        void GGJ2023InputActions.IGeneralActions.OnPlayer1_VerticalAxis(InputAction.CallbackContext context)
        {
            _player1VerticalInput = context.ReadValue<float>();
        }

        void GGJ2023InputActions.IGeneralActions.OnPlayer2_HorizontalAxis(InputAction.CallbackContext context)
        {
            _player2HorizontalInput = context.ReadValue<float>();
        }

        void GGJ2023InputActions.IGeneralActions.OnPlayer2_VerticalAxis(InputAction.CallbackContext context)
        {
            _player2VerticalInput = context.ReadValue<float>();
        }
    }
}
