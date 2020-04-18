using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OverScope_kek.Level
{
    public class Level : SerializedMonoBehaviour
    {
        [JsonProperty("levelName")]
        public string levelName;
        
        [JsonProperty("grid")]
        public Grid grid;

        public void Init(Level level)
        {
            levelName = level.levelName;
            grid = level.grid;
            InitGrid();
        }

        private void InitGrid()
        {
            for (int x = 0; x < grid.cells.Length; x++)
            {
                for (int y = 0; y < grid.cells[x].Length; y++)
                {
                    var cell = grid.cells[x][y];
                    cell.x = x;
                    cell.y = y;
                    InitCell(cell);
                }
            }
        }

        private void InitCell(Cell cell)
        {
            var spacing = LevelLoader.Instance.cellSpacing;
            cell.cellObject = new GameObject($"{cell.x}_{cell.y}");
            cell.cellObject.transform.SetParent(transform);
            cell.cellObject.transform.localPosition = new Vector3(cell.x + (cell.x * spacing), cell.y + (cell.y * spacing));

            var scriptable = LevelLoader.Instance.GetCellScriptableOfType(cell.type);
            var renderer = cell.cellObject.AddComponent<SpriteRenderer>();
            renderer.sprite = scriptable.sprite;
        }
    }
}