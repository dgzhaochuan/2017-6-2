using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using SevenZip;
using System.Text.RegularExpressions;

public class ConfigeWindows : EditorWindow
{
    const string _build = @"D:\ZhaoChuan\CaptureGhosts\CaptureGhosts2017\Build\";
    string[] _buildFile = new string[] {
        @"D:\ZhaoChuan\CaptureGhosts\CaptureGhosts2017\Assets\Editor\ReadData.cs",
        @"D:\ZhaoChuan\CaptureGhosts\CaptureGhosts2017\Assets\Plugins\Excel"
    };


    //ZIP
    List<string> zip_Path = new List<string>();
    int zip_index = -1;
    bool waitZip = false;
    Dictionary<string, string> xml_Path = new Dictionary<string,string>();


    [MenuItem("Tools/ConfigeWindow")]
    static void AddWindow()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 500, 500);
        ConfigeWindows window = (ConfigeWindows)EditorWindow.GetWindow(typeof(ConfigeWindows));
        window.Show();
    }
    bool GetBtn(string s)
    {
        return GUILayout.Button(s, GUILayout.Height(30));
        //return GUILayout.Button(s, GUILayout.Height(30), GUILayout.Width(200));
    }
    private void OnGUI()
    {

        if (GetBtn("测试场景"))
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene
                ("Assets/Scenes/Test.unity");
        }
        if (GetBtn("UI场景"))
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene
                ("Assets/Scenes/UITest.unity");
        }
        if (GetBtn("战斗场景"))
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene
                ("Assets/Scenes/BattleTest.unity");
        }
        GUILayout.Space(10);
        
        //if (GetBtn("打包AB")) { Builder.BuildAssetResource(); }
        //if (GetBtn("删除AB")) { Builder.DestoryAssetBundles(); }
        //if (GetBtn("清除所有AB标签")) { Builder.ClearAssetBundlesName(); }
        //GUILayout.Space(10);

        //if (GetBtn("打包移除Excel")) { StartBuild(); }
        //if (GetBtn("转换数据添加Excel")) { EndBuild(); }
        //if (GetBtn("删除保存数据")) { DeleteLocal(); }
        //if (GetBtn("打开保存数据文件夹")) { OpenLocalFinder(); }
        //if (GetBtn("创建P1鬼预制")) { GhostPrefab(); }
        //if (GetBtn("创建P2鬼Spine预制")) { GhostSpinePrefab(); }


        //if (GetBtn("ZIP压缩")) { Compress(); }
        //if (GetBtn("停止压缩"))
        //{
        //    waitZip = false;
        //    zip_index = zip_Path.Count;
        //    xml_Path.Clear();
        //}

    }


    //void Update()
    //{
    //    if (waitZip)
    //    {
    //        if (zip_index<zip_Path.Count)
    //        {
    //            Debug.Log(string.Format("ZIP：{0}/{1}",zip_index,zip_Path.Count));
    //            string path = Regex.Split(zip_Path[zip_index], "StreamingAssets")[1];
    //            string zip_save_path= Application.streamingAssetsPath + "\\AB_ZIP_Server_Test\\" + path;
    //            xml_Path.Add(path, zip_Path[zip_index]);
    //            SevenZipManager.CompressFileLZMA(zip_Path[zip_index], zip_save_path, delegate() { zip_index++;waitZip = true; });
    //        }
    //        else
    //        {
    //            EstablishMD5();
    //            Debug.Log("打包完毕");
    //            waitZip = false;
    //        }
    //    }
    //}


    //void StartBuild()
    //{
    //    File.Copy(_buildFile[0], _build + "ReadData.cs", true);
    //    if (Directory.Exists(_build + "Excel"))
    //    {
    //        Directory.Delete(_build + "Excel", true);
    //    }
    //    Directory.Move(_buildFile[1], _build + "Excel");
    //    File.Delete(_buildFile[0]);
    //    //Directory.Delete(_buildFile[1], true);
    //}
    //void EndBuild()
    //{
    //    if (File.Exists(_build + "ReadData.cs"))
    //        File.Copy(_build + "ReadData.cs", _buildFile[0], true);
    //    if (Directory.Exists(_build + "Excel"))
    //        Directory.Move(_build + "Excel", _buildFile[1]);
    //}
    //void DeleteLocal()
    //{
    //    string path = GameModelManager.savePaeh;
    //    if (File.Exists(path))
    //    {
    //        File.Delete(path);
    //        Debug.Log("delete[" + path + "] successed!");
    //    }
    //}
    //void OpenLocalFinder()
    //{
    //    System.Diagnostics.Process.Start(Application.persistentDataPath);
    //}
    //void GhostPrefab()
    //{
    //    string path = Application.dataPath + @"\Resources\AssetBundle\Textures\GhostSprite";
    //    string save_path = Application.dataPath + @"\Resources\AssetBundle\Textures\GhostSprite";

    //    Pack(path,save_path,1);
    //    return;


    //    DirectoryInfo Folder = new DirectoryInfo(path);
    //    foreach (var NextFile in Folder.GetFiles())
    //    {
    //        if (NextFile.FullName.Contains(".meta")) continue;
    //        if (NextFile.Name.Contains(".png") == false) continue;
    //        string _path = NextFile.FullName.Remove(0, NextFile.FullName.IndexOf("Assets")).Replace("\\", "/");
    //        string spritePath = _path;
    //        _path = _path.Replace(".png", ".prefab");
    //        if (File.Exists(_path)) { File.Delete(_path); }
    //        Object tempPrefab = PrefabUtility.CreateEmptyPrefab(_path);
    //        GameObject game = new GameObject(NextFile.Name);
    //        SpriteRenderer renderer = game.AddComponent<SpriteRenderer>();
    //        renderer.sortingOrder = 10;
    //        renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    //        renderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
    //        game.AddComponent<PolygonCollider2D>();
    //        PrefabUtility.ReplacePrefab(game, tempPrefab);
    //        UnityEngine.Object.DestroyImmediate(game);
    //    }
    //}
    //void GhostSpinePrefab()
    //{
    //    string path = Application.dataPath + @"/Resources/AssetBundle/Spine";
    //    string save_path = Application.dataPath + @"/Resources/AssetBundle/Spine";
    //    Pack(path, save_path, 2);
    //    return;
    //}
    //void EstablishMD5()
    //{
    //    string data = "";
    //    foreach (var item in xml_Path)
    //    {
    //        data += item.Key + ":" + MD5Util.GetFileHash(item.Value) + "|";
    //    }
    //    SaveXml(data);
    //}
    //void Compress()
    //{
    //    //    SevenZipManager.CompressFileLZMA(@"D:\ZhaoChuan\CaptureGhosts\CaptureGhosts2017\Assets\StreamingAssets\AB_ZIP_Server_Test\Windows\assetbundle\data\config\aboutmodel.unity3d",
    //    //        @"D:\ZhaoChuan\CaptureGhosts\CaptureGhosts2017\Assets\StreamingAssets\AB_ZIP_Server_Test\Windows\assetbundle\data\config\aboutmodel.unity3d.zip", 
    //    //        null);
    //    //    return;
    //    string plat = Builder.GetPlat(EditorUserBuildSettings.activeBuildTarget);
    //    string path = Path.Combine(Application.streamingAssetsPath , plat);
    //    //string save_path = Application.persistentDataPath + "\\AssetBundle\\";
    //    string save_path = Application.streamingAssetsPath + "\\AssetBundle\\";
    //    if (Directory.Exists(save_path) == false) Directory.CreateDirectory(save_path);
    //    zip_Path = new List<string>();
    //    Pack(path, save_path, 3);
    //    zip_index = 0;
    //    waitZip = true;
    //}
  
    //void UpCompress()
    //{
    //    SevenZipManager.CompressFileLZMA(@"D:\ZhaoChuan\CaptureGhosts\CaptureGhosts2017\Assets\StreamingAssets\AB_ZIP_Server_Test\Windows\assetbundle\data\config\aboutmodel.unity3d.zip",
    //     @"D:\ZhaoChuan\CaptureGhosts\CaptureGhosts2017\Assets\StreamingAssets\AB_ZIP_Server_Test\Windows\assetbundle\data\config\aboutmodel_1.unity3d",
    //     null);

    //    return;

    //    return;
    //    string path1 = @"D:\ZhaoChuan\CaptureGhosts\CaptureGhosts2017\Assets\StreamingAssets\AB_ZIP_Client_Test\Windows\assetbundle\data\config\aboutmodel.unity3d.zip";
    //    string path2 = @"D:\ZhaoChuan\CaptureGhosts\CaptureGhosts2017\Assets\StreamingAssets\AB_ZIP_Client_Test\Windows\assetbundle\data\config\aboutmodel.unity3d";
    //    SevenZipManager.DecompressFileLZMA(path1, path2);

    //}
    //void SaveXml(string files)
    //{
    //    File.WriteAllText(Application.streamingAssetsPath+ @"\Server_Test_AssetBundlesFile.txt", files);
    //}



    ///// <summary>
    ///// type 1 sprite  
    /////         2 spine
    ///// </summary>
    ///// <param name="source"></param>
    ///// <param name="type"></param>
    //void Pack(string source,string target, int type)
    //{
    //    DirectoryInfo folder = new DirectoryInfo(source);
    //    if (type == 0) return;
    //    FileSystemInfo[] files = folder.GetFileSystemInfos();
    //    int length = files.Length;
    //    for (int i = 0; i < length; i++)
    //    {
    //        if (files[i] is DirectoryInfo)
    //        {
    //            Pack(files[i].FullName, target, type);
    //        }
    //        else
    //        {
    //            if (files[i].Name.Contains(".meta")) continue;
    //            switch (type)
    //            {
    //                case 1:
    //                    if (files[i].Name.EndsWith(".png"))
    //                    {
    //                        EstablishSpritePrefab(files[i],  target);
    //                    }
    //                    break;
    //                case 2:
    //                    if (files[i].Name.Contains("_SkeletonData"))
    //                    {
    //                        EstablishSpinePrefab(files[i],  target);
    //                    }
    //                    break;
    //                case 3:
    //                    if (!files[i].Name.EndsWith(".unity3d")) continue;
    //                    zip_Path.Add(files[i].FullName);
    //                    break;
    //            }
    //        }
    //    }
    //}
    //void EstablishSpritePrefab(FileSystemInfo file,string target)
    //{
    //    string _path = file.FullName.Remove(0, file.FullName.IndexOf("Assets")).Replace("\\", "/");
    //    string spritePath = _path;
    //    _path = _path.Replace(".png", ".prefab");
    //    if (File.Exists(_path)) { File.Delete(_path); }
    //    Object tempPrefab = PrefabUtility.CreateEmptyPrefab(_path);
    //    GameObject game = new GameObject(file.Name);
    //    SpriteRenderer renderer = game.AddComponent<SpriteRenderer>();
    //    renderer.sortingOrder = 10;
    //    renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    //    renderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
    //    game.AddComponent<PolygonCollider2D>();
    //    PrefabUtility.ReplacePrefab(game, tempPrefab);
    //    UnityEngine.Object.DestroyImmediate(game);
    //}
    //void EstablishSpinePrefab(FileSystemInfo file, string target)
    //{
    //    string spineAssetPath = file.FullName.Remove(0, file.FullName.IndexOf("Assets")).Replace("\\", "/");
    //    string _path = spineAssetPath.Replace("_SkeletonData.asset", ".prefab");
    //    Object tempPrefab = PrefabUtility.CreateEmptyPrefab(_path);
    //    SkeletonDataAsset spineAsset = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(spineAssetPath);
    //    GameObject game = new GameObject(file.Name);
    //    game.AddComponent<SkeletonAnimation>().skeletonDataAsset = spineAsset;
    //    PrefabUtility.ReplacePrefab(game, tempPrefab);
    //    UnityEngine.Object.DestroyImmediate(game);
    //}
    
}
