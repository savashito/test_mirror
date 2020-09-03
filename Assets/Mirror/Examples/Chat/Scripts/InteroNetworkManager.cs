using UnityEngine;

namespace Mirror.Examples.Chat 
{
    [AddComponentMenu("")]
    public class InteroNetworkManager : NetworkManager
    {
        public string PlayerName { get; set; }
        public string[] usuariosConectados;
        public bool iniciasServer;
        public void SetHostname(string hostname)
        {
            networkAddress = hostname;
        }

        public ChatWindow chatWindow;
        public int coneccionId;
        private void Awake()
        {
            usuariosConectados = new string[10];
            if (iniciasServer)
            {
                base.StartServer();
                Debug.Log("Servidor Inicio");
            }
        }
        public class CreatePlayerMessage : MessageBase
        {
            public string name;
        }

        // esto corre en el server
        public override void OnStartServer()
        {
            base.OnStartServer();
            //Esto siempre esta escuchando
            NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
        }
        // corre en el cliente
        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            Debug.Log("Holi me uni a mi mismo");
            /*
             Debug.Log("Holi me uni a mi mismo");
             coneccionId = conn.connectionId;
             chatWindow.connectionId = conn.connectionId;
             // tell the server to create a player with this name
             */
            conn.Send(new CreatePlayerMessage { name = PlayerName });
        }
        // Esto corre en el server
        void OnCreatePlayer(NetworkConnection connection, CreatePlayerMessage createPlayerMessage)
        {
            // create a gameobject using the name supplied by client
            Debug.Log("Server.OnCreatePlayer ");
            GameObject playergo = Instantiate(playerPrefab);
            playergo.GetComponent<Player>().playerName = createPlayerMessage.name;
            /*
            if (connection.connectionId == 0)
            {
                playergo.GetComponent<Player>().lider = true;
            }

            Debug.Log("Conection id" + coneccionId);

            Debug.Log("Usuario " + createPlayerMessage.name);
            */
            // set it as the player
            NetworkServer.AddPlayerForConnection(connection, playergo);
            usuariosConectados[connection.connectionId - 1] = createPlayerMessage.name;

            // send message to all client
            //           NetworkServer.sen
            // chatWindow.gameObject.SetActive(true);
        }

        public override void OnServerDisconnect(NetworkConnection info)
        {
            Debug.Log("SOmeone disconnected");
            NetworkServer.DestroyPlayerForConnection(info);
            Debug.Log("" + info.connectionId);
        }
    }
}
