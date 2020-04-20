using TMPro;
using UnityEngine;

public class LeaderButton : MonoBehaviour
{
    public TMP_Text alias;
    public TMP_Text meta;
    public TMP_Text index;
    
    public void SetText(LeaderboardMeta entry, bool setIndex = false)
    {
        alias.text = entry.Alias;
        meta.text =
            $"Tries: {(entry.Tries == -1 ? "-" : entry.Tries.ToString())} - Time: {entry.Time} " +
            $"- Fans: {(entry.FanUsed == -1 ? "-" : $"{entry.FanUsed}/{entry.TotalFan}")}";

        if (setIndex)
        {
            if (entry.Index == 0)
            {
                index.text = "-";
            }
            else
            {
                index.text = $"# {entry.Index}.";
            }
        }
    }
}