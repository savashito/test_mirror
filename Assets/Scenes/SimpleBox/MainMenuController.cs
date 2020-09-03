using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public NetworkManager netManager;
    void Start()
    {
        
    }
    public void StartServer()
    {
        netManager.StartHost();
        this.gameObject.SetActive(false);

    }
    public void ConectToServer()
    {
        // netManager.networkAddress;
        // netManager.net
        
        netManager.StartClient();
        this.gameObject.SetActive(false);
        // netManager.cone()
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
