using System;
using Mirror;

namespace Units
{
    public class Unit : NetworkBehaviour
    {
        public event Action OnSelected = delegate { };
        public event Action OnDeselected = delegate { };

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
