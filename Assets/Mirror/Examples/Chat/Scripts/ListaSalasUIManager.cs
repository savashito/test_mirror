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
        public GameObject buttonUnirteSala;
        private GameObject[] buttonsSalas;
        public GameObject spawnButtons;
        public void Start()
        {
            Player.OnPlayerJoinGame += OnPlayerJoinGame;
            Player.OnPlayerExitGame += OnPlayerExitGame;
            Player.OnCreateSala += OnCreateSala;
            buttonsSalas = new GameObject[10];
            
            for (int i = 0; i < buttonsSalas.Length; i++)
            {
                GameObject buttonUnirte = Instantiate(buttonUnirteSala, spawnButtons.transform);
                // buttonUnirte
                buttonUnirte.SetActive(false);
                buttonUnirte.transform.position = new Vector3(590f,650f - i*35f,0f);
                buttonsSalas[i] = buttonUnirte;
            }
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
        public void UneteASala(Text text)
        {
            Player player = NetworkClient.connection.identity.GetComponent<Player>();
            player.CmdUneteSala(text.text);
        }
        void UpdateTextSalas()
        {   
            GameObject[] salasA;
            int i;
            if (listaSalasText != null)
            {
                for (i = 0; i < buttonsSalas.Length; i++)
                {
                    buttonsSalas[i].SetActive(false);
                }
                listaSalasText.text = "";
                salasA = GameObject.FindGameObjectsWithTag("Sala");
                i = 0;
                foreach (GameObject entry in salasA)
                {
                    Sala sala = entry.GetComponent<Sala>();
                    listaSalasText.text += $"<color=green> {sala.salaName} </color> \n";
                    Text t = buttonsSalas[i].GetComponentInChildren<Text>();
                    t.text = sala.salaName;
                    buttonsSalas[i].SetActive(true);
                    ++i;
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
