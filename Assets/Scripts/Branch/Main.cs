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

        public float HorizontalInput = 0f;
        public float VerticalInput = 0f;

        public GameObject BranchControllerPrefab;
        public GameObject TargetTransformPrefab;

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

        void GGJ2023InputActions.IGeneralActions.OnHorizontalAxis(InputAction.CallbackContext context)
        {
            HorizontalInput = context.ReadValue<float>();
        }

        void GGJ2023InputActions.IGeneralActions.OnVerticalAxis(InputAction.CallbackContext context)
        {
            VerticalInput = context.ReadValue<float>();
        }
    }
}
