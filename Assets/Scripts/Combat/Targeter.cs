using Mirror;
using UnityEngine;

namespace Combat
{
    public class Targeter : NetworkBehaviour
    {
        // TODO: [SerializeField] attribute only for testing
        [SerializeField] private Targetable _target;

        #region Server

        [Command]
        public void CmdSetTarget(GameObject targetGameObject)
        {
            if (!targetGameObject.TryGetComponent<Targetable>(out var target))
                return;

            _target = target;
        }

        [Server]
        public void ClearTarget() => _target = null;

        #endregion

        #region Client



        #endregion
    }
}
