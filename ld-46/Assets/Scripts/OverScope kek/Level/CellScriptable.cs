using UnityEngine;

namespace OverScope_kek.Level
{
    [CreateAssetMenu(fileName = "Cell", menuName = "New Cell", order = 0)]
    public class CellScriptable : ScriptableObject
    {
        public string name;
        public Sprite sprite;
        public CellType type;
    }
}