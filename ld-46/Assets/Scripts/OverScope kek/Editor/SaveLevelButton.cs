using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace OverScope_kek.Editor
{
    public class SaveLevelButton
    {
        [MenuItem("Level/Save")]
        public static void SaveLevel()
        {
            Debug.Log("Saving level");
            var selected = Selection.gameObjects[0].GetComponent<Level.Level>();
            if (selected == null)
            {
                selected = Object.FindObjectOfType<Level.Level>();
                Debug.Log(selected);
                if (selected == null)
                {
                    Debug.LogWarning("No level found, cannot save");
                    return;
                }
            }

            var serialized = JsonConvert.SerializeObject(selected);
            Debug.Log(serialized);
        }
    }
}