using System;
using System.Collections;
using System.Collections.Generic;
using RPSLS.Framework;
using RPSLS.Framework.Services;
using UnityEngine;

public class BootstrapDummy : MonoBehaviour
{
    private void Awake()
    {
        if (FindAnyObjectByType<Bootstrap>())
            return;

        InitializeAllServices(() => StartCoroutine(OnInitialized()));
    }

    private void OnDestroy()
    {
        ServiceLocator.UnregisterAllServices();
    }
    
    private static void InitializeAllServices(Action onComplete = null)
    {
        Dictionary<Type, IService> map = new Dictionary<Type, IService>()
        {
            { typeof(IGameService), new GameManager() },
            { typeof(IHighscore), new HighscoreService() },
            { typeof(ISceneService), new SceneService() },
            { typeof(IRoundService), new RoundManager() },
        };

        foreach (KeyValuePair<Type, IService> item in map)
        {
            ServiceLocator.RegisterService(item.Key, item.Value);
        }
            
        onComplete?.Invoke();
    }
    
    private IEnumerator OnInitialized()
    {
        yield return new WaitForEndOfFrame();
        ServiceLocator.GetGameManager().SwitchState(GameState.MainMenu);
    }
}
