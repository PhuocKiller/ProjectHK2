using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
   NetworkRunner runner;
    [SerializeField]
    GameObject character, gameManagerObj, playerManagerObj;
    GameNetworkCallBack gameNetworkCallBack;
    [SerializeField]
    UnityEvent onConnected;
    [SerializeField]
    Transform spawnPoint;

    private void Awake()
    {
        runner = GetComponent<NetworkRunner>();
        gameNetworkCallBack = GetComponent<GameNetworkCallBack>();

    }
   
    private void SpawnPlayer(NetworkRunner m_runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer && runner.IsSharedModeMasterClient)
        {
            runner.Spawn(gameManagerObj, inputAuthority: player);
            runner.Spawn(playerManagerObj, inputAuthority: player);
        }

        if (player == runner.LocalPlayer)
        {
            NetworkObject robo = runner.Spawn(character, spawnPoint.position, Quaternion.identity, inputAuthority: player);
                /*, onBeforeSpawned: OnBeforeSpawned);

            void OnBeforeSpawned(NetworkRunner runner, NetworkObject roboObject)
            {
                
            }*/
        }
    }
    public async void OnClickBtn(Button btn)
    {
        if (runner != null)
        {
            btn.interactable = false;
            Singleton<Loading>.Instance.ShowLoading();
            gameNetworkCallBack ??= GetComponent<GameNetworkCallBack>();
            gameNetworkCallBack.OnPlayerJoinRegister(SpawnPlayer);
            await runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Shared,
                SessionName = "Begin",
                CustomLobbyName = "VN",
                SceneManager = GetComponent<LoadSceneManager>()
            });
            onConnected?.Invoke();
            Singleton<Loading>.Instance.HideLoading();
        }
    }
}
