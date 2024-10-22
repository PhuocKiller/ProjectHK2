using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
   NetworkRunner runner;
    [SerializeField]
    GameObject character;
    GameNetWorkCallBack gameNetWorkCallBack;

    private void Awake()
    {
        runner = GetComponent<NetworkRunner>();
        gameNetWorkCallBack = GetComponent<GameNetWorkCallBack>();

    }
    void SpawnPlayer(NetworkRunner m_runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {
            runner.Spawn(character, inputAuthority: player);
        }
            
    }
    /*public async void OnClickBtn(Button btn)
    {
        if (roomName.Length > 0 && runner != null)
        {
            btn.interactable = false;
            Singleton<Loading>.Instance.ShowLoading();
            gameNetWorkCallBack ??= GetComponent<GameNetworkCallBack>();
            gameNetWorkCallBack.OnPlayerJoinRegister(SpawnPlayer);
            await runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Shared,
                SessionName = "",
                CustomLobbyName = "VN",
                SceneManager = GetComponent<LoadSceneManager>()
            });
            onConnected?.Invoke();
            Singleton<Loading>.Instance.HideLoading();
        }
    }*/
}
