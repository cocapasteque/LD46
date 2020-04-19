using TMPro;
using UnityEngine;

public class LeaderButton : MonoBehaviour
{
    public TMP_Text alias;
    public TMP_Text meta;

    public void SetText(LeaderboardMeta entry)
    {
        alias.text = entry.Alias;
        meta.text =
            $"Tries: {(entry.Tries == -1 ? "-" : entry.Tries.ToString())} - Time: {entry.Time} " +
            $"- Fans: {(entry.FanUsed == -1 ? "-" : $"{entry.FanUsed}/{entry.TotalFan}")}";
    }
}