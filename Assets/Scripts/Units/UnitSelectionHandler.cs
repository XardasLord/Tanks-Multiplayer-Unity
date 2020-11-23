using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Units
{
    public class UnitSelectionHandler : MonoBehaviour
    {
        [SerializeField] private RectTransform unitSelectionArea;
        [SerializeField] private LayerMask layerMask;

        public List<Unit> SelectedUnits { get; } = new List<Unit>();

        private Vector2 _startSelectionPosition;
        private RTSPlayer _player;
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            // TODO: After lobby system we can move this caching in the Start method
            if (_player is null)
                _player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartSelectionArea();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ClearSelectionArea();
            }
            else if (Mouse.current.leftButton.isPressed)
            {
                UpdateSelectionArea();
            }
        }

        private void StartSelectionArea()
        {
            if (!Keyboard.current.leftShiftKey.isPressed)
                DeselectAllUnits();

            unitSelectionArea.gameObject.SetActive(true);

            _startSelectionPosition = Mouse.current.position.ReadValue();

            UpdateSelectionArea();
        }

        private void UpdateSelectionArea()
        {
            var mousePosition = Mouse.current.position.ReadValue();

            var areaWidth = mousePosition.x - _startSelectionPosition.x;
            var areaHeight = mousePosition.y - _startSelectionPosition.y;

            unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
            unitSelectionArea.anchoredPosition = _startSelectionPosition + new Vector2(areaWidth / 2, areaHeight / 2); // We want the anchore to be in the middle
        }

        private void ClearSelectionArea()
        {
            unitSelectionArea.gameObject.SetActive(false);

            if (unitSelectionArea.sizeDelta.magnitude == 0)
            {
                SelectSingleUnit();
            }
            else
            {
                SelectSelectedUnits();
            }
        }

        private void DeselectAllUnits()
        {
            foreach (var selectedUnit in SelectedUnits)
            {
                selectedUnit.Deselect();
            }

            SelectedUnits.Clear();
        }

        private void SelectSingleUnit()
        {
            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
                return;

            if (!hit.collider.TryGetComponent<Unit>(out var unit))
                return;

            if (!unit.hasAuthority)
                return;

            SelectedUnits.Add(unit);

            foreach (var selectedUnit in SelectedUnits)
            {
                selectedUnit.Select();
            }
        }

        private void SelectSelectedUnits()
        {
            var min = unitSelectionArea.anchoredPosition - unitSelectionArea.sizeDelta / 2;
            var max = unitSelectionArea.anchoredPosition + unitSelectionArea.sizeDelta / 2;

            foreach (var unit in _player.GetMyUnits())
            {
                if (SelectedUnits.Contains(unit))
                    continue;
                
                var screenPosition = _mainCamera.WorldToScreenPoint(unit.transform.position);

                if (screenPosition.x > min.x &&
                    screenPosition.x < max.x &&
                    screenPosition.y > min.y &&
                    screenPosition.y < max.y)
                {
                    SelectedUnits.Add(unit);
                    unit.Select();
                }
            }
        }
    }
}
