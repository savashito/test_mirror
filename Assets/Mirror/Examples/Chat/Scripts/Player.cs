using System;
using System.Collections.Generic;
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
        
        public static event Action<Player, string> OnPlayerJoinSala;
        public static event Action<Player> OnPlayerExitSala;
        public static event Action<Player> OnPlayerJoinGame;
        public static event Action<Player> OnPlayerExitGame;

        private Dictionary<string, GameObject> salas = new Dictionary<string, GameObject>();

        public void Start()
        {
            UnityEngine.Debug.Log("Noew player " + playerName);

            OnPlayerJoinGame?.Invoke(this);
        }

        private void OnDestroy()
        {
            OnPlayerExitGame?.Invoke(this);
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
            // ownSala.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
            // NetworkServer.Spawn(ownSala);
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
        [Command]
        public void CmdUneteSala(string name)
        {
            RpcUneteSala(name);
        }

        [ClientRpc]
        public void RpcUneteSala(string salaName)
        {
            UnityEngine.Debug.Log("RpcUneteSala");
            Debug.Log("" + isLocalPlayer);

            if (isLocalPlayer)
                salas[salaName].transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
            OnPlayerJoinSala?.Invoke(this, salaName);
        }

        [ClientRpc]
        public void RcpCreateSala(string salaName)
        {
            UnityEngine.Debug.Log("RcpCreateSala");
            GameObject sala = Instantiate(salaPrefab);
            sala.GetComponent<Sala>().salaName = salaName;
            salas.Add(salaName, sala);
            Debug.Log("" + isLocalPlayer);
            if (isLocalPlayer)
                CmdUneteSala(salaName);
            // sala.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
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
