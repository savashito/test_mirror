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
        /*[SyncVar(hook = nameof(UpdateParent))]
        public NetworkIdentity myParent = null;*/

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
        public static Dictionary<string, GameObject> salas = new Dictionary<string, GameObject>();

        public void Start()
        {
            UnityEngine.Debug.Log("Noew player " + playerName);
            //if(myParent != null)
              //  transform.parent = myParent.transform;
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
            salas.Add(salaName, ownSala);
           
            NetworkServer.Spawn(ownSala);
           // if (salaName.Trim() != "")
             //   RcpCreateSala(salaName.Trim(), ownSala);
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
        public void CmdUneteSala(string salaName)
        {
            GameObject sala = salas[salaName];
            //myParent = sala.GetComponent<NetworkIdentity>();
            //gameObject.transform.parent = myParent.transform;
            // RpcUneteSala(salaName);
        }
        [Command]
        public void CmdExitSala()
        {
            /*Debug.Log("Antes de null");
            myParent = null;
            transform.parent = null;
            Debug.Log("Despues de null");*/
        }
        void UpdateParent(NetworkIdentity old,NetworkIdentity nuevo)
        {
            
           /* Debug.Log("Se llamo elñ hook" + nuevo);
            if(nuevo != null)
            {
                Debug.Log("Nuevo no es null");

                transform.parent = nuevo.transform;
                Debug.Log("Se cambio el parent a " + nuevo);

                Sala sala = nuevo.GetComponent<Sala>();
                if (isLocalPlayer)
                    sala.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
                OnPlayerJoinSala?.Invoke(this, sala.salaName);
            }
            else
            {

                Debug.Log("Nuevo es null y old es" + old);
                if(old != null)
                {
                    old.transform.parent = null;
                    transform.parent = null;
                }
                
            }*/

        }
        [ClientRpc]
        public void RpcUneteSala(string salaName)
        {
            UnityEngine.Debug.Log("RpcUneteSala");
            Debug.Log(salaName + " " + isLocalPlayer);
            /*
            GameObject sala = salas[salaName];
            if (isLocalPlayer) 
                sala.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
                */
            // transform.parent = sala.transform;
            
        }

        [ClientRpc]
        public void RcpCreateSala(string salaName, GameObject sala)
        {
            UnityEngine.Debug.Log("RcpCreateSala " );
            // salas.Add(salaName, sala);
            Debug.Log(salaName + "Se agrego sala ");

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
