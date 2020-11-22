﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Units
{
    public class UnitSelectionHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        public List<Unit> SelectedUnits { get; } = new List<Unit>();

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                DeselectAllUnits();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                SelectAllUnits();
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

        private void SelectAllUnits()
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
    }
}
