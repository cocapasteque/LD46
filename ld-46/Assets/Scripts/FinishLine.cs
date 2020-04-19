using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public string unlockingLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Unlocking level
            var levelsString = PlayerPrefs.GetString("unlocked_levels'", "['Tutorial','Level 1']");
            var array = JsonConvert.DeserializeObject<string[]>(levelsString);
            var list = array.ToList();
            list.Add(unlockingLevel);
            levelsString = JsonConvert.SerializeObject(list.ToArray());
            Debug.Log(levelsString);
            PlayerPrefs.SetString("unlocked_levels", levelsString);
            GameManager.Instance.ReachedFinishLine();
        }
    }
}