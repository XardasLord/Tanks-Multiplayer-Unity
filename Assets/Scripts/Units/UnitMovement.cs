using Combat;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class UnitMovement : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Targeter targeter;
        [SerializeField] private float chaseRange = 10f;

        #region Server

        [ServerCallback]
        private void Update()
        {
            var target = targeter.Target;

            if (target != null)
            {
                if (ShouldChaseTargeter(target))
                {
                    agent.SetDestination(target.transform.position);
                }
                else if (agent.hasPath)
                {
                    agent.ResetPath();
                }
                return;
            }

            if (!agent.hasPath)
                return;

            if (agent.remainingDistance > agent.stoppingDistance)
                return;

            agent.ResetPath();
        }

        private bool ShouldChaseTargeter(Targetable target)
        {
            // Square roots optimization, instead Vector3.Distance()
            return (target.transform.position - transform.position).sqrMagnitude > chaseRange * chaseRange;
        }

        [Command]
        public void CmdMove(Vector3 destinationPosition)
        {
            targeter.ClearTarget();

            if (!NavMesh.SamplePosition(destinationPosition, out var hit, 1f, NavMesh.AllAreas))
                return;

            agent.SetDestination(hit.position);
        }

        #endregion
    }
}