using UnityEngine;

namespace RPSLS.Framework.Services
{
    public class HighscoreService: IHighscore
    {
        public void Start()
        {
            Debug.Log("Highscore Service Started");
        }

        public void OnDestroy()
        {
            Debug.Log("Highscore Service Destroyed");
        }
    }
}