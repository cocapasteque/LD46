using Newtonsoft.Json;
using Sirenix.OdinInspector;

namespace OverScope_kek.Level
{
    public class Grid : SerializedMonoBehaviour
    {
        [JsonProperty("cells")]
        public Cell[][] cells;
    }
}