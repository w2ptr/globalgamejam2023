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

        public GameObject BranchControllerPrefab;
        public GameObject TargetTransformPrefab;

        [SerializeField] private Tree _player1Tree;
        [SerializeField] private Tree _player2Tree;

        public enum Player
        {
            Player1,
            Player2
        }

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

        public Tree GetTree(Player player)
        {
            switch (player)
            {
                case Player.Player1:
                    return _player1Tree;
                case Player.Player2:
                    return _player2Tree;
                default:
                    return null;
            }
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
