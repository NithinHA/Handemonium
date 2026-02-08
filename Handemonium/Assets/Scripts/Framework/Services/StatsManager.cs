using System;
using UnityEngine;
using RPSLS.Game;

namespace RPSLS.Framework.Services
{
    [Serializable]
    public class PlayerStats
    {
        public int Wins;
        public int Losses;
        public int GamesPlayed;
        public int BestScore; // Highest surviv streak

        public static PlayerStats FromJson(string json)
        {
            return JsonUtility.FromJson<PlayerStats>(json);
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }

    public class StatsManager : IService
    {
        private const string STATS_KEY = "RPSLS_PlayerStats";
        private PlayerStats _stats;

        public PlayerStats GetStats()
        {
            if (_stats == null)
            {
                LoadStats();
            }
            return _stats;
        }

        public void AddWin()
        {
            _stats.Wins++;
            _stats.GamesPlayed++;
            SaveStats();
        }

        public void AddLoss()
        {
            _stats.Losses++;
            _stats.GamesPlayed++;
            SaveStats();
        }

        public void RecordGameScore(int score)
        {
            if (score > _stats.BestScore)
            {
                _stats.BestScore = score;
                SaveStats();
            }
        }

        private void LoadStats()
        {
            string json = PlayerPrefs.GetString(STATS_KEY, "");
            if (string.IsNullOrEmpty(json))
            {
                _stats = new PlayerStats();
            }
            else
            {
                _stats = PlayerStats.FromJson(json);
            }
        }

        private void SaveStats()
        {
            if (_stats != null)
            {
                PlayerPrefs.SetString(STATS_KEY, _stats.ToJson());
                PlayerPrefs.Save();
            }
        }

        public void Start()
        {
            LoadStats();
        }

        public void OnDestroy()
        {
        }
    }
}
