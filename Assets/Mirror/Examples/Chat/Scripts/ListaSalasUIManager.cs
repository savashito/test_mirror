using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace Mirror.Examples.Chat
{
    public class ListaSalasUIManager : MonoBehaviour
    {
        public Text listaSalasText;
       
        public void Awake()
        {
            Player.OnPlayerJoinGame += OnPlayerJoinGame;
            Player.OnPlayerExitGame += OnPlayerExitGame;
            Player.OnCreateSala += OnCreateSala;
        }
        public void CreateSala()
        {
            Player player = NetworkClient.connection.identity.GetComponent<Player>();
            player.CmdCreateSala(player.playerName);
        }
        public void OnCreateSala(Player player, string message)
        {
            Debug.Log("Se cre la sala " + message);
            UpdateTextSalas();
        }
        void UpdateTextSalas()
        {
            GameObject[] salasA;
            if (listaSalasText != null)
            {
                listaSalasText.text = "";
                salasA = GameObject.FindGameObjectsWithTag("Sala");
                foreach (GameObject entry in salasA)
                {
                    Sala player = entry.GetComponent<Sala>();
                    listaSalasText.text += $"<color=green> {player.salaName} </color> \n";
                }
            }
        }
        void OnPlayerJoinGame(Player player)
        {
            UpdateTextSalas();
        }
        private void OnPlayerExitGame(Player player)
        {
            UpdateTextSalas();
        }
    }
}
