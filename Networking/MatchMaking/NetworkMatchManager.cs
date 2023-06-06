using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Mirror;
using MatchMaking;

[RequireComponent(typeof(MatchInterestManager), typeof(MatchManager))]
public partial class NetworkMatchManager : NetworkManager
{
    [Scene]
    [SerializeField]
    string matchScene;
    Scene loadedMatchScene;

    [SerializeField]
    MatchInterestManager matchInterestManager;

    MatchManager matches;

    bool hostOnlyMatch = false;

    Transform exits;

    Dictionary<NetworkConnection, InitPlayerMessage> playerMessages = new Dictionary<NetworkConnection, InitPlayerMessage>();

    public override void OnValidate()
    {
        base.OnValidate();
    }

    public override void Awake()
    {
        base.Awake();
        InitRandom();
    }

    void InitRandom()
    {
        Random.InitState(System.Environment.TickCount);
    }

    public override void Start()
    {
        base.Start();
        Init();
    }

    void Init()
    {
#if (UNITY_SERVER)
        InitServer();
#else
        InitClient();
#endif
    }

    void InitServer()
    {
        StartHost();
    }

    void InitHostMatch()
    {
        hostOnlyMatch = true;
        StartHost();
    }

    public override void OnStartServer()
    {
        matches = GetComponent<MatchManager>();
        matches.Init(OnMatchStateChanged);
        NetworkServer.RegisterHandler<InitPlayerMessage>(OnCreatePlayer);

#if (UNITY_SERVER)
        matches.CreateBackgroundMatch();
#endif
    }

    public override void OnStartHost()
    {

    }

    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerChangeScene(string newSceneName)
    {

    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName == onlineScene)
        {
            LoadMatchScene();
        }
    }

    void LoadMatchScene()
    {
        SceneManager.LoadSceneAsync(matchScene, LoadSceneMode.Additive).completed += op =>
        {
            loadedMatchScene = SceneManager.GetSceneByPath(matchScene);
            matches.MatchSceneLoaded();
        };
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (hostOnlyMatch)
        {
            matches.AddPlayerToHostOnlyMatch(conn);
        }
        else
        {
            matches.AddPlayerToOpenMatch(conn);
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        RemovePlayerFromMatch(conn);
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {

    }

    void OnMatchStateChanged(Match match)
    {
        if (match.state == MatchState.InProgress)
        {
            StartMatch(match);
        }
        else if (match.state == MatchState.Closed)
        {
            CloseMatch(match);
        }
    }

    async void StartMatch(Match match)
    {
        while (!loadedMatchScene.isLoaded || !match.IsReadyToStart())
        {
            await Task.Yield();
        }

        if (mode == NetworkManagerMode.Host)
        {
            ClientMoveToScene(loadedMatchScene);
        }

        MovePlayersToMatch(match);
        matches.MatchReadyForSpawning(match);
        matchInterestManager.UpdateMatch(match);
    }

    void MovePlayersToMatch(Match match)
    {
        foreach (NetworkConnection playerConn in match.players)
        {
            playerConn.Send(new SceneMessage { sceneName = matchScene, sceneOperation = SceneOperation.LoadAdditive, customHandling = true });
            SceneManager.MoveGameObjectToScene(playerConn.identity.gameObject, loadedMatchScene);
            matches.HandlePlayerMessageForMatch(playerConn, playerMessages[playerConn], match);
            playerMessages.Remove(playerConn);
        }
    }

    public void AddItemToMatch(NetworkIdentity inMatch, NetworkIdentity toAdd)
    {
        if (!matchInterestManager.identityToMatch.ContainsKey(inMatch))
        {
            Debug.LogError("AddItemToMatch: Doesn't exist in match");
            return;
        }

        Match match = GetMatchForNetId(inMatch);
        match.AddItem(toAdd);
        matchInterestManager.UpdateMatch(match);
    }

    public void RemoveItemFromMatch(NetworkIdentity inMatch)
    {
        if (!matchInterestManager.identityToMatch.ContainsKey(inMatch))
        {
            Debug.LogError("RemoveItemFromMatch: Doesn't exist in match");
            return;
        }

        Match match = GetMatchForNetId(inMatch);
        match.RemoveItem(inMatch);
        matchInterestManager.UpdateMatch(match);
    }

    Match GetMatchForNetId(NetworkIdentity id)
    {
        System.Guid matchId = matchInterestManager.GetMatchIDForNetId(id);
        return matches.GetMatch(matchId);
    }

    void RemovePlayerFromMatch(NetworkConnection conn)
    {
        if (!hostOnlyMatch)
        {
            matches.RemovePlayerFromMatch(conn);
            matchInterestManager.RemoveFromMatch(conn.identity);
        }
    }

    public void OnAIHumanDeath(NetworkIdentity aiHumanId)
    {
        matches.RemoveAIHumanFromMatch(aiHumanId);
        matchInterestManager.RemoveFromMatch(aiHumanId);
    }

    void CloseMatch(Match match)
    {
        matchInterestManager.RemoveMatch(match);
    }

    public void Disconnect()
    {
        switch (mode)
        {
            case NetworkManagerMode.ServerOnly:
                StopServer();
                break;
            case NetworkManagerMode.Host:
                offlineScene = null;
                StopHost();
                break;
            case NetworkManagerMode.ClientOnly:
                offlineScene = null;
                StopClient();
                break;
            default:
                Debug.LogError("Not a valid network mode");
                break;
        }

        Destroy(gameObject);
    }

    public override void OnDestroy()
    {
        if (mode == NetworkManagerMode.ServerOnly || mode == NetworkManagerMode.Host)
        {
            Shutdown();
            transport.Shutdown();
        }
        base.OnDestroy();
    }

    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}
