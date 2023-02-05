using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalGameJam2023
{
    public class MapUIElement : MonoBehaviour
    {
        private SettingsMenu _settingsMenu;

        [SerializeField] private GameObject _overlay;

        [System.Serializable]
        public class MapUIElementData
        {
            public bool IsCool;
            public float HowCool;
            public bool Yes;
        }

        public MapUIElementData Data;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                _overlay.SetActive(_isSelected);
            }
            //{
            //    _isSelected = value;
            //}
        }

        private void OnHoverEnter()
        {
            _settingsMenu.Select(this);
        }

        private void OnHoverExit()
        {
            _settingsMenu.TryDeselect(this);
        }
    }

}