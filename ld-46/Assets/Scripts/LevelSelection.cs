using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public GameObject leadButtons;
    public GameObject loadingPanel;
    public GameObject selectPanel;
    
    public List<LeaderButton> btnList;
    public LeaderButton personalBest;

    public string selectedLevel = string.Empty;
    public GameObject playButton;

    public void LoadLeaderboard(string levelName)
    {
        selectPanel.SetActive(false);
        loadingPanel.SetActive(true);
        leadButtons.SetActive(false);

        StartCoroutine(Work());

        IEnumerator Work()
        {
            yield return new WaitForSeconds(0.2f);
            LeaderboardController.Instance.FetchLeaderboard(levelName).Then(response =>
            {
                var entries = JsonConvert.DeserializeObject<LeaderboardEntry[]>(response.Text);
                var index = 0;
                var perso = FindPersonalBest(entries);

                foreach (var btn in btnList)
                {
                    if (index >= entries.Length)
                    {
                        var meta = new LeaderboardMeta()
                            {Alias = "-", FanUsed = -1, Score = -1, Time = "-", TotalFan = -1, Tries = -1};
                        btn.SetText(meta);
                    }
                    else
                    {
                        var meta = JsonConvert.DeserializeObject<LeaderboardMeta>(entries[index++].Metadata);
                        btn.SetText(meta);
                    }
                }

                if (perso != null)
                {
                    personalBest.SetText(perso, true);
                }
                else
                {
                    var meta = new LeaderboardMeta()
                        {Alias = "-", FanUsed = -1, Score = -1, Time = "-", TotalFan = -1, Tries = -1};
                    personalBest.SetText(meta, true);
                }

                leadButtons.SetActive(true);
                loadingPanel.SetActive(false);
            });
        }
    }

    public LeaderboardMeta FindPersonalBest(LeaderboardEntry[] entries)
    {
        var alias = PlayerPrefs.GetString("player_alias");
        var index = 1;
        foreach (var entry in entries)
        {
            if (entry.Key.Equals(alias))
            {
                Debug.Log("Personal Best : " + entry);
                var meta = JsonConvert.DeserializeObject<LeaderboardMeta>(entry.Metadata);
                meta.Index = index;
                Debug.Log(meta);
                return meta;
            }

            index++;
        }

        return null;
    }

    public void PlaySelectedLevel()
    {
        GameManager.Instance.LoadLevel(selectedLevel);
    }
}