using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.Chat
{
    //[System.Serializable]
    //public class SyncListItem : SyncList<Player> { }
    public class Sala : NetworkBehaviour
    {
        [SyncVar]
        public string salaName;

        public Text listaPlayersText;
        public void Start()
        {
            Player.OnPlayerJoinSala += OnJoinSala;
            Player.salas.Add(salaName, gameObject);

        }
        void OnJoinSala(Player player, string salaName)
        {
            UpdateTextUsersSalas();
        }
        void UpdateTextUsersSalas()
        {
            // GameObject[] players;
            int i;
            if (listaPlayersText != null)
            {
                listaPlayersText.text = "";
                Player [] players = gameObject.GetComponentsInChildren<Player>();
                i = 0;
                foreach (Player entry in players)
                {
                    // Player sala = entry.GetComponent<Player>();
                    listaPlayersText.text += $"<color=green> {entry.playerName} </color> \n";
                    ++i;
                }
            }
        }
    }
}