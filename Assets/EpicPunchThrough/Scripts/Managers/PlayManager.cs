using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManager
{
    #region Static

    private static PlayManager instance;
    public static PlayManager Instance
    {
        get {
            if( instance == null ) {
                instance = new PlayManager();
                instance.Initialize(ScriptableObject.CreateInstance<PlaySettings>());
            }
            return instance;
        }
    }

    #endregion

    public PlaySettings settings;

    [SerializeField] private string gameSceneName = "Game";

    private bool isInitialized = false;
    public bool Initialize(PlaySettings settings)
    {
        this.settings = settings;

        GameManager.Instance.stateChanged -= GameStateChanged;
        GameManager.Instance.stateChanged += GameStateChanged;
        GameManager.Instance.fixedUpdated -= DoUpdate;
        GameManager.Instance.fixedUpdated += DoUpdate;

        isInitialized = true;
        return isInitialized;
    }

    public Scene GetGameScene()
    {
        return SceneManager.GetSceneByName(gameSceneName);
    }

    bool Pause(float value)
    {
        if( value > 0 ) {
            GameManager.Instance.SetState(GameManager.GameState.pause);

            return true;
        }

        return false;
    }

    void GameStateChanged(GameManager.GameState previousState, GameManager.GameState newState)
    {
        switch( newState ) {
            case GameManager.GameState.play:
                if( previousState == GameManager.GameState.play ) { return; }

                InputManager.Instance.PauseInput += Pause;

                if( !SceneManager.GetSceneByName(gameSceneName).isLoaded ) {
                    SceneManager.LoadScene(gameSceneName, LoadSceneMode.Additive);

                    SceneManager.sceneLoaded += SetupPlay;
                } else {
                    SetupPlay(GetGameScene(), LoadSceneMode.Additive);
                }

                break;
            case GameManager.GameState.pause:
                // coexist with pause menu
                // pause all actions
                InputManager.Instance.PauseInput -= Pause;

                break;
            default:
                InputManager.Instance.PauseInput -= Pause;

                if( ( previousState == GameManager.GameState.play
                    || previousState == GameManager.GameState.pause )
                    && SceneManager.GetSceneByName(gameSceneName).isLoaded ) 
                {
                    GameManager.Instance.activeCamera.Fade(1, 0);
                    SceneManager.UnloadSceneAsync(gameSceneName);
                    EnvironmentManager.Instance.UnloadEnvironment();
                }
                break;
        }
    }
    void SetupPlay(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= SetupPlay;
        EnvironmentManager.Instance.LoadEnvironment(scene, "Arena");
        SceneManager.sceneLoaded += SpawnPlayer;

        GameManager.Instance.activeCamera.Fade(0, GameManager.Instance.settings.sceneTransitionFadeDuration, true);
    }

    void SpawnPlayer(Scene scene, LoadSceneMode mode)
    {
        if( AgentManager.Instance.playerAgent != null ) {
            GameObject.Destroy(AgentManager.Instance.playerAgent.gameObject);
        }

        GameObject playerSpawn = GameObject.Find("SceneEntrance");
        if( playerSpawn == null ) {
            playerSpawn = GameObject.Find("PlayerSpawn");
        }
        AgentManager.AgentSpawnData spawnData = new AgentManager.AgentSpawnData(new Vector3(0, 5, -10), 
                                                        false, "Player", 
                                                        AgentManager.AgentType.player, 
                                                        0, "Vat Grown", 
                                                        (ulong)AgentManager.Instance.settings.baseVitalForce, 
                                                        (ulong)AgentManager.Instance.settings.defaultActiveVFFactor);
        if( playerSpawn != null ) {
            AgentSpawn playerSpawnScript = playerSpawn.GetComponent<AgentSpawn>();
            if( playerSpawnScript != null ) {
                spawnData = new AgentManager.AgentSpawnData(playerSpawnScript.transform.position, 
                    playerSpawnScript.isFacingRight, 
                    playerSpawnScript.agentName, 
                    AgentManager.AgentType.player, 
                    0, "Vat Grown", 
                    (ulong)AgentManager.Instance.settings.baseVitalForce,
                    (ulong)AgentManager.Instance.settings.defaultActiveVFFactor);
            }
        }
        AgentManager.Instance.SpawnAgent(spawnData);

        FollowCamera followCamera = GameObject.Find("SceneEntrance").GetComponent<FollowCamera>();
        if( followCamera != null ) { followCamera.target = AgentManager.Instance.playerAgent.transform; }

        SceneManager.sceneLoaded -= SpawnPlayer;
    }

    void DoUpdate(GameManager.UpdateData data)
    {
    }
}
