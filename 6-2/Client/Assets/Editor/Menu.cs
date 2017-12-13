using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;


namespace editor
{

    public class Menu
    {
        [MenuItem("Tools/SaveFile/OpenSaveFile")]
        static public void OpenSaveFile()
        {
            System.Diagnostics.Process.Start(Application.persistentDataPath);
        }
        [MenuItem("Tools/OpenExcelFile")]
        static public void OpenExcelFile()
        {
            System.Diagnostics.Process.Start(Application.dataPath.Replace("/Client/Assets", "") + "/Excel");
        }
        [MenuItem("Tools/SaveFile/DestorySaveFile")]
        static public void DestorySaveFile()
        {
            string path = Application.persistentDataPath + "/Json";
            if (Directory.Exists(path) == false) return;
            DirectoryInfo di = new DirectoryInfo(path);
            di.Delete(true);
        }

        [MenuItem("Tools/ExcelToJson")]
        static public void ExcelToJson()
        {

            Write<EquipmentAttribute>(DataManager.EquipmentFileName);
            Write<PropAttribute>(DataManager.PropFileName);
            Write<CharacterAttribute>(DataManager.CharacterFileName);
            Write<SkillAttribute>(DataManager.SkillFileName);
            Write<BuffModel>(DataManager.BuffFileName);
            Write<SceneModel>(DataManager.SceneFileName);
            Debug.Log("ExcelToJson End");
        }
        static void Write<T>(string FileName) where T : BaseModel
        {
            string ExcelPath = Application.dataPath.Replace("/Client/Assets", "") + "/Excel/";
            string JsonPath = Application.streamingAssetsPath + "/Json/";
            if (Directory.Exists(JsonPath) == false)
            {
                Directory.CreateDirectory(JsonPath);
            }
            SaveObject<T> model = new SaveObject<T>();
            model.ObjAry = ReadData.ReadExcel<T>(ExcelPath + FileName + DataManager.ExcelFileType,FileName);
            string data = JsonUtility.ToJson(model);
            File.WriteAllText(JsonPath + FileName+DataManager.JsonType, data, System.Text.Encoding.Default);
        }
    }
}
