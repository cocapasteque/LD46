using TMPro;
using UnityEngine;

public class LeaderButton : MonoBehaviour
{
    public TMP_Text alias;
    public TMP_Text meta;
    public TMP_Text index;
    
    public void SetText(LeaderboardMeta entry, bool setIndex = false)
    {
        if (entry.Score == -1)
        {
            alias.text = "-";
            meta.text = "-";
        }
        else
        {
            alias.text = $"{entry.Alias} - {entry.Score:N0}";
            meta.text =
                $"Time: {entry.Time} - Tries: {entry.Tries}";
        }
        
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