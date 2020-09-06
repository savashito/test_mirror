using System;
using UnityEngine;

namespace Mirror.Examples.Chat
{
    public class Player : NetworkBehaviour
    {
        [SyncVar]
        public string playerName;
        [SyncVar]
        public bool listo;
        [SyncVar]
        public bool lider;

        public GameObject salaPrefab;
        public GameObject ownSala;
        public static event Action<Player, string> OnMessage;
        public static event Action<Player, bool> OnReady;
        // public static event Action<Player, bool> OnLider;
        public static event Action<Player, string> OnCreateSala;
        
        public static event Action<Player> OnPlayerJoinLobby;
        public static event Action<Player> OnPlayerExitLobby;

        public void Start()
        {
            UnityEngine.Debug.Log("Noew player " + playerName);

            OnPlayerJoinLobby?.Invoke(this);
        }

        private void OnDestroy()
        {
            OnPlayerExitLobby?.Invoke(this);
            NetworkServer.Destroy(ownSala);

            if (hasAuthority)
            {
                Debug.Log("Server OnDestroy");
            }
            else
            {
                Debug.Log("client OnDestroy");
            }
            

        }

        [Command]
        public void CmdCreateSala(string salaName)
        {
            Debug.Log("CmdCreateSala");
            ownSala = Instantiate(salaPrefab);
            ownSala.GetComponent<Sala>().salaName = salaName;
            NetworkServer.Spawn(ownSala);
            if (salaName.Trim() != "")
                RcpCreateSala(salaName.Trim());
        }
        [Command]
        public void CmdSend(string message)
        {
            if (message.Trim() != "")
                RpcReceive(message.Trim());
        }

        [Command]
        public void CmdReady(bool ready)
        {
            RcpReady(ready);
        }

        [ClientRpc]
        public void RcpCreateSala(string salaName)
        {
            UnityEngine.Debug.Log("RcpCreateSala");

            OnCreateSala?.Invoke(this, salaName);

        }
        [ClientRpc]
        public void RcpReady(bool ready)
        {
            OnReady?.Invoke(this, ready);

        }

        [ClientRpc]
        public void RpcReceive(string message)
        {
            OnMessage?.Invoke(this, message);
        }
    }
}
