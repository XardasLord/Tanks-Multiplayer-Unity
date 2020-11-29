using Mirror;
using UnityEngine;

namespace Combat
{
    public class Targeter : NetworkBehaviour
    {
        public Targetable Target { get; private set; }

        #region Server

        [Command]
        public void CmdSetTarget(GameObject targetGameObject)
        {
            if (!targetGameObject.TryGetComponent<Targetable>(out var target))
                return;

            Target = target;
        }

        [Server]
        public void ClearTarget() => Target = null;

        #endregion
    }
}
