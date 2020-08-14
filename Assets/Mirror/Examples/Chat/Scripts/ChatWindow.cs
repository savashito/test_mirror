using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.Chat
{
    public class ChatWindow : MonoBehaviour
    {
        static readonly ILogger logger = LogFactory.GetLogger(typeof(ChatWindow));

        public InputField chatMessage;
        public Text chatHistory;
        public Text textUsers;
        public Scrollbar scrollbar;
        Dictionary<string, Player> players = new Dictionary<string, Player>();

        public void Awake()
        {
            Player.OnMessage += OnPlayerMessage;
            Player.OnPlayerJoinLobby += OnPlayerJoinLobby;
            logger.Log("Added callbacks to Chat Window");

        }

        void OnPlayerJoinLobby(Player player)
        {
            /*
            string prettyMessage = player.isLocalPlayer ?
                $"<color=red>{player.playerName}: </color> {message}" :
                $"<color=blue>{player.playerName}: </color> {message}";
            AppendMessage(prettyMessage);
            */
            players[player.playerName] = player;
            /*
            Player p =  ;
            if (p == null)
            {

            }*/
            // UnityEngine.Debug.Log("Noew player koko " + player.playerName);
            textUsers.text += player.playerName + "\n";
            // logger.Log(player.playerName);
        }
        void OnPlayerMessage(Player player, string message)
        {
            string prettyMessage = player.isLocalPlayer ?
                $"<color=red>{player.playerName}: </color> {message}" :
                $"<color=blue>{player.playerName}: </color> {message}";
            AppendMessage(prettyMessage);

            logger.Log(message);
        }

        public void OnSend()
        {
            if (chatMessage.text.Trim() == "")
                return;

            // get our player
            Player player = NetworkClient.connection.identity.GetComponent<Player>();

            // send a message
            player.CmdSend(chatMessage.text.Trim());

            chatMessage.text = "";
        }

        internal void AppendMessage(string message)
        {
            StartCoroutine(AppendAndScroll(message));
        }

        IEnumerator AppendAndScroll(string message)
        {
            chatHistory.text += message + "\n";

            // it takes 2 frames for the UI to update ?!?!
            yield return null;
            yield return null;

            // slam the scrollbar down
            scrollbar.value = 0;
        }
    }
}
