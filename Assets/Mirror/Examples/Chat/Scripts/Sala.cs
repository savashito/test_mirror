using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Examples.Chat
{
    //[System.Serializable]
    //public class SyncListItem : SyncList<Player> { }
    public class Sala : NetworkBehaviour
    {
        [SyncVar]
        public string salaName;
        //[SyncVar]
        //public SyncListItem players;

    }

}