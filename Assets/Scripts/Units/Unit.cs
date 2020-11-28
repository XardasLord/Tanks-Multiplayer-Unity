using System;
using Combat;
using Mirror;
using UnityEngine;

namespace Units
{
    public class Unit : NetworkBehaviour
    {
        [SerializeField] private UnitMovement unitMovement;
        [SerializeField] private Targeter targeter;

        public event Action AuthorityOnSelected = delegate { };
        public event Action AuthorityOnDeselected = delegate { };
        public static event Action<Unit> AuthorityOnUnitSpawned = delegate { };
        public static event Action<Unit> AuthorityOnUnitDespawned = delegate { };

        public static event Action<Unit> ServerOnUnitSpawned = delegate { };
        public static event Action<Unit> ServerOnUnitDespawned = delegate { };

        public UnitMovement UnitMovement => unitMovement;
        public Targeter Targeter => targeter;

        #region Server

        public override void OnStartServer()
        {
            ServerOnUnitSpawned(this);
        }

        public override void OnStopServer()
        {
            ServerOnUnitDespawned(this);
        }

        #endregion

        #region Client

        public override void OnStartClient()
        {
            if (!isClientOnly || !hasAuthority)
                return;

            AuthorityOnUnitSpawned(this);
        }

        public override void OnStopClient()
        {
            if (!isClientOnly || !hasAuthority)
                return;

            AuthorityOnUnitDespawned(this);
        }

        [Client]
        public void Select()
        {
            if (!hasAuthority)
                return;

            AuthorityOnSelected();
        }

        [Client]
        public void Deselect()
        {
            if (!hasAuthority)
                return;

            AuthorityOnDeselected();
        }

        #endregion
    }
}
