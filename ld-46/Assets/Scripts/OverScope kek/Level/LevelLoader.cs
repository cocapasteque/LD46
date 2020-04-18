using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace OverScope_kek.Level
{
    public class LevelLoader : Singleton<LevelLoader>
    {
        public CellScriptable[] Cells;
        public float cellSpacing = 1;
        public Level LoadLevel(string filePath)
        {
            var asset = Resources.Load<TextAsset>(filePath);
            var level = JsonConvert.DeserializeObject<Level>(asset.text);
            
            var obj = new GameObject("Level", typeof(Level));
            obj.GetComponent<Level>().Init(level);
            obj.transform.localRotation = Quaternion.Euler(0, 0, -90);
            return obj.GetComponent<Level>();
        }

        public CellScriptable GetCellScriptableOfType(CellType type)
        {
            return Cells.First(x => x.type == type);
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                LoadLevel("level");
            }
        }
    }
}