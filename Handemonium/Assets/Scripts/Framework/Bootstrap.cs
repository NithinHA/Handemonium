using System;
using System.Collections.Generic;
using RPSLS.Framework.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPSLS.Framework
{
    public class Bootstrap : MonoBehaviour
    {
        private void Awake()
        {
            InitializeAllServices(OnInitialized);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnregisterAllServices();
        }

        /// <summary>
        /// Initialize all the persistent core game services here-
        /// </summary>
        private static void InitializeAllServices(Action onComplete = null)
        {
            Dictionary<Type, IService> map = new Dictionary<Type, IService>()
            {
                { typeof(IGameService), new GameManager() },
                { typeof(IHighscore), new HighscoreService() }
            };

            foreach (KeyValuePair<Type, IService> item in map)
            {
                ServiceLocator.RegisterService(item.Key, item.Value);
            }
            
            onComplete?.Invoke();
        }

        private void OnInitialized()
        {
            SceneManager.LoadSceneAsync(Constants.SceneNames.GAME);
        }

    }
}