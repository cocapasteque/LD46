using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public GameObject leadButtons;
    public List<LeaderButton> btnList;
    public string selectedLevel = string.Empty;
    public GameObject playButton;
    
    public void LoadLeaderboard(string levelName)
    {
        LeaderboardController.Instance.FetchLeaderboard(levelName).Then(response =>
        {
            var entries = JsonConvert.DeserializeObject<LeaderboardEntry[]>(response.Text);
            var index = 0;
            foreach (var btn in btnList)
            {
                if (index >= entries.Length)
                {
                    Debug.Log("No leaderboard entry yet.");
                    var meta = new LeaderboardMeta(){Alias = "-", FanUsed = -1, Score = -1, Time = "-", TotalFan = -1, Tries = -1};
                    btn.SetText(meta);
                }
                else
                {
                    var meta = JsonConvert.DeserializeObject<LeaderboardMeta>(entries[index++].Metadata);
                    btn.SetText(meta);
                }
            }

            leadButtons.SetActive(true);
        });
    }

    public void PlaySelectedLevel()
    {
        GameManager.Instance.LoadLevel(selectedLevel);
    }
}