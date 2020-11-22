using System;
using Mirror;
using UnityEngine;

namespace Units
{
    public class Unit : NetworkBehaviour
    {
        [SerializeField] private UnitMovement unitMovement;

        public event Action OnSelected = delegate { };
        public event Action OnDeselected = delegate { };

        public UnitMovement GetUnitMovement() => unitMovement;

        #region Client

        [Client]
        public void Select()
        {
            if (!hasAuthority)
                return;

            OnSelected();
        }

        [Client]
        public void Deselect()
        {
            if (!hasAuthority)
                return;

            OnDeselected();
        }

        #endregion
    }
}
