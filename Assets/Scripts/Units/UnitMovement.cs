using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class UnitMovement : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent agent;

        #region Server

        [Command]
        public void CmdMove(Vector3 destinationPosition)
        {
            if (!NavMesh.SamplePosition(destinationPosition, out var hit, 1f, NavMesh.AllAreas))
                return;

            agent.SetDestination(hit.position);
        }

        #endregion
    }
}