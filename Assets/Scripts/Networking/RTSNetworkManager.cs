using Mirror;
using UnityEngine;

namespace Networking
{
    public class RTSNetworkManager : NetworkManager
    {
        [SerializeField] private GameObject unitSpawnerPrefab;

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);

            var addedPlayer = conn.identity.transform;

            var unitSpawnerInstance = Instantiate(
                unitSpawnerPrefab,
                addedPlayer.position,
                addedPlayer.rotation);

            NetworkServer.Spawn(unitSpawnerInstance, conn);
        }
    }
}
