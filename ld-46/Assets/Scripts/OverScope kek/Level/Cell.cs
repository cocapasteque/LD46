using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OverScope_kek.Level
{
    public class Cell : SerializedMonoBehaviour
    {
        public int x;
        public int y;
        public GameObject cellObject;
        
        [JsonProperty("type")]
        public CellType type;
    }

    public enum CellType
    {
        None, Saw, Flamethrower
    }
}