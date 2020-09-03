using System;

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

        }

        [Command]
        public void CmdCreateSala(string message)
        {
            if (message.Trim() != "")
                RpcReceive(message.Trim());
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
