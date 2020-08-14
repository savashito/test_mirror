using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest2 : NetworkBehaviour
{
    [SerializeField] private Vector3 movement = new Vector3();
    [SerializeField] private string nombre = "jojo";


    [Client]
    void Update()
    {
        if (!hasAuthority) { return; }

        if (!Input.GetKeyDown(KeyCode.Space)) { return; }
        // 
        TellServerMove();
        // transform.Translate(movement);
    }
    [Command]
    private void TellServerMove()
    {
        // Aqui en el server se hace la validacion
        // de que si el cliente tiene permiso de realizar la accion.

        TellClientMove();
    }
    [ClientRpc]
    private void TellClientMove() {
        nombre = "lechuga";
        transform.Translate(movement);
    }
}
