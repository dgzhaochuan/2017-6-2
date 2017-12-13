using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class TexturePacker : Editor
{
    #region TexturePacker  打包后源图片格式会被改变成Cursor
    //间隔
    static int padding=1;
    //true的时候解包图片会按照小图片原始大小
    //否则就固定大小（地图）
    static bool atlasTrimming=false;
    static string mapPath { get { return Application.dataPath + "/BundleResources/Texture/Map"; } }


    [MenuItem("Tools/SpritesPacker/SetAtas/按图片尺寸打包")]
    static public void AtlasTrimming()
    {
        atlasTrimming = false;
    }
    [MenuItem("Tools/SpritesPacker/SetAtas/紧凑打包")]
    static public void NotAtlasTrimming()
    {
        atlasTrimming = true;
    }


    //打包选中图片
    [MenuItem("Tools/SpritesPacker/GroupSelectedSprite")]
    static public void GroupSelectedSprite()
    {
        string path = EditorUtility.SaveFilePanelInProject("选择图集保存路径",
                      "New Atlas.png", "png", "Save atlas as...", mapPath);
        CommandBuild(GetSelectedTextures(), path);
        Debug.Log("打包完毕");
    }
    
    


    //打包文件夹下所有图片
    [MenuItem("Tools/SpritesPacker/GroupFolderSprite")]
    static public void GroupFolderSprite()
    {
        string inputPath = EditorUtility.OpenFolderPanel("图片目录", mapPath, "");
        if (string.IsNullOrEmpty(inputPath)) return;
        string path = EditorUtility.OpenFolderPanel("保存目录", mapPath, "");
        _GroupFolderSprite(inputPath,path);
        Debug.Log("打包完毕");
    }
    //打包文件夹下所有图片
    static public void _GroupFolderSprite(string inputPath,string path)
    {        
        string[] imagePath = Directory.GetFiles(inputPath);
        List<Texture2D> textures = new List<Texture2D>();
        for (int i = 0; i < imagePath.Length; i++)
        {
            if (Path.GetExtension(imagePath[i]) == ".png" || Path.GetExtension(imagePath[i]) == ".PNG")
            {
                string sheetPath = GetAssetPath(imagePath[i]);
                Object o = AssetDatabase.LoadAssetAtPath<Object>(sheetPath);
                Texture2D texture = o as Texture2D;
                if (texture != null)
                {
                    MakeReadable(texture);
                    textures.Add(texture);
                }
            }
        }

        string[] Directories = Directory.GetDirectories(inputPath);
        if (Directories.Length > 0)
        {
            for (int i = 0; i < Directories.Length; i++)
            {
                _GroupFolderSprite(Directories[i],path);
            }
        }
        inputPath=inputPath.Replace("\\", "/");
        string[] _s = inputPath.Split('/');
        path = path + "/" + _s[_s.Length-1] + ".png";
        CommandBuild(textures.ToArray(), path);
    }

    //打包图片
    static public void CommandBuild(Texture2D[] _tex, string path)
    {
        if (_tex.Length == 0) return;
        Texture2D tex = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
        Texture2D[] t= CreateSprites(_tex);
        Rect[] _re = tex.PackTextures(t, padding, 1024);
        if (string.IsNullOrEmpty(path)) return;
      
        Rect[] rects = GetRect(_re, tex, t);
        string atlasName = atlasName = System.IO.Path.GetFileName(path); 
        XDocument xml = GetXml(t, rects, tex, atlasName);
        string Extension = System.IO.Path.GetExtension(path);
        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);
        string xmlPath = path.Replace(".png", ".xml");
        xml.Save(xmlPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
    }
    
    static public Texture2D[] CreateSprites( Texture2D[] textures)
    {
        Texture2D[] t = new Texture2D[textures.Length];
        textures.CopyTo(t,0);

        for (int i = 0; i < textures.Length; i++)
        {
            Texture2D tex = t[i];
            Texture2D oldTex = ImportTexture(tex, true, false, true);
            if (oldTex == null) oldTex = tex;
            if (oldTex == null) continue;


            // If we want to trim transparent pixels, there is more work to be done
            Color32[] pixels = oldTex.GetPixels32();

            int xmin = oldTex.width;
            int xmax = 0;
            int ymin = oldTex.height;
            int ymax = 0;
            int oldWidth = oldTex.width;
            int oldHeight = oldTex.height;

            // Find solid pixels
            if (atlasTrimming)
            {
                for (int y = 0, yw = oldHeight; y < yw; ++y)
                {
                    for (int x = 0, xw = oldWidth; x < xw; ++x)
                    {
                        Color32 c = pixels[y * xw + x];

                        if (c.a != 0)
                        {
                            if (y < ymin) ymin = y;
                            if (y > ymax) ymax = y;
                            if (x < xmin) xmin = x;
                            if (x > xmax) xmax = x;
                        }
                    }
                }
            }
            else
            {
                xmin = 0;
                xmax = oldWidth - 1;
                ymin = 0;
                ymax = oldHeight - 1;
            }

            int newWidth = (xmax - xmin) + 1;
            int newHeight = (ymax - ymin) + 1;

            if (newWidth > 0 && newHeight > 0)
            {
                // If the dimensions match, then nothing was actually trimmed
                if ((newWidth != oldWidth || newHeight != oldHeight))
                {
                    // Copy the non-trimmed texture data into a temporary buffer
                    Color32[] newPixels = new Color32[newWidth * newHeight];

                    for (int y = 0; y < newHeight; ++y)
                    {
                        for (int x = 0; x < newWidth; ++x)
                        {
                            int newIndex = y * newWidth + x;
                            int oldIndex = (ymin + y) * oldWidth + (xmin + x);
                            //if (NGUISettings.atlasPMA) 
                            //newPixels[newIndex] = NGUITools.ApplyPMA(pixels[oldIndex]);
                            //else
                            newPixels[newIndex] = pixels[oldIndex];
                        }
                    }
                    
                    t[i] = new Texture2D(newWidth, newHeight);
                    t[i].name = tex.name;
                    t[i].SetPixels32(newPixels);
                    t[i].Apply();                    
                }
            }
        }
        return t;
    }

    static public Texture2D ImportTexture(Texture tex, bool forInput, bool force, bool alphaTransparency)
    {
        if (tex != null)
        {
            string path = AssetDatabase.GetAssetPath(tex.GetInstanceID());
            Texture2D newTex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            return newTex;
        }
        return null;
    }
    

    static XDocument GetXml(Texture2D[] textures, Rect[] rects, Texture AtlasTexture, string Atlas)
    {
        //实例化XDocument对象  
        XDocument xdoc = new XDocument();
        //创建根节点  
        XElement root = new XElement("imagePath", Atlas);
        xdoc.Add(root);
        root.SetAttributeValue("width", AtlasTexture.width);
        root.SetAttributeValue("height", AtlasTexture.height);
        for (int i = 0; i < textures.Length; i++)
        {
            Rect rect = rects[i];
            XElement cls = new XElement("SubTexture");
            cls.SetAttributeValue("name", textures[i].name);
            cls.SetAttributeValue("x", rect.x);
            cls.SetAttributeValue("y", rect.y);
            cls.SetAttributeValue("width", rect.width);
            cls.SetAttributeValue("height", rect.height);
            root.Add(cls);
        }
        return xdoc;
    }
    static Texture2D[] GetSelectedTextures()
    {
        //SetReadable();
        List<Texture2D> textures = new List<Texture2D>();

        if (Selection.objects != null && Selection.objects.Length > 0)
        {
            Object[] objects = EditorUtility.CollectDependencies(Selection.objects);

            foreach (Object o in objects)
            {
                Texture2D tex = o as Texture2D;
                MakeReadable(tex);
                if (tex == null) continue;
                textures.Add(tex);
            }
        }
        return textures.ToArray();
    }
    static public string MakeReadable(Texture2D tex, bool readable = true)
    {
        string path = AssetDatabase.GetAssetPath(tex);
        if (!string.IsNullOrEmpty(path))
        {
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            textureImporter.isReadable = readable;
            textureImporter.textureType = TextureImporterType.Cursor;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
        return path;
    }
    static public Rect[] GetRect(Rect[] rects, Texture tex, Texture2D[] textures)
    {
        if (rects.Length == 0) return new Rect[0];
        Rect[] _rects = new Rect[rects.Length];
        for (int i = 0; i < _rects.Length; i++)
        {
            _rects[i] = ConvertToPixels(rects[i], tex.width, tex.height, true);
        }
        return _rects;
    }
    static public Rect ConvertToPixels(Rect rect, int width, int height, bool round)
    {
        Rect final = rect;

        if (round)
        {
            final.xMin = Mathf.RoundToInt(rect.xMin * width);
            final.xMax = Mathf.RoundToInt(rect.xMax * width);
            final.yMin = Mathf.RoundToInt((1f - rect.yMax) * height);
            final.yMax = Mathf.RoundToInt((1f - rect.yMin) * height);
        }
        else
        {
            final.xMin = rect.xMin * width;
            final.xMax = rect.xMax * width;
            final.yMin = (1f - rect.yMax) * height;
            final.yMax = (1f - rect.yMin) * height;
        }
        return final;
    }

    #endregion

    #region  TexturePacker

    [MenuItem("Tools/SpritesPacker/TexturePacker")]
    public static void BuildTexturePacker()
    {
        string inputPath = EditorUtility.OpenFolderPanel("Atlas", mapPath, "");
        _BuildTexturePacker(inputPath);
        AssetDatabase.Refresh();
    }

    static void _BuildTexturePacker(string inputPath)
    {
        string[] imagePath = Directory.GetFiles(inputPath);
        foreach (string path in imagePath)
        {
            if (Path.GetExtension(path) == ".png" || Path.GetExtension(path) == ".PNG")
            {
                string sheetPath = GetAssetPath(path);
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(sheetPath);
                Debug.Log(texture.name);
                string rootPath = path.Replace(Path.GetFileName(path), "").Replace("\\", "/");
                string pngPath = rootPath + texture.name + ".png";
                string xmlPath = rootPath + texture.name + ".xml";
                if (File.Exists(xmlPath) == false)
                {
                    Debug.LogError(texture.name + "xml不存在");
                    continue;
                }
                TextureImporter asetImp = null;
                Dictionary<string, Vector4> tIpterMap = new Dictionary<string, Vector4>();
                if (File.Exists(pngPath))
                {
                    Debug.Log("exite: " + sheetPath);
                    asetImp = GetTextureIpter(sheetPath);
                    SaveBoreder(tIpterMap, asetImp);
                    //File.Delete(pngPath);
                }
                //File.Copy(inputPath + "/" + texture.name + ".png", pngPath);
                AssetDatabase.Refresh();
                FileStream fs = new FileStream(xmlPath, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string jText = sr.ReadToEnd();
                fs.Close();
                sr.Close();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(jText);
                XmlNodeList elemList = xml.GetElementsByTagName("SubTexture");
                WriteMeta(elemList, texture.name, tIpterMap, sheetPath);
                File.Delete(xmlPath);
            }
        }

        string[] Directories = Directory.GetDirectories(inputPath);
        if (Directories.Length > 0)
        {
            for (int i = 0; i < Directories.Length; i++)
            {
                _BuildTexturePacker(Directories[i]);
            }
        }
    }

    //如果这张图集已经拉好了9宫格，需要先保存起来
    static void SaveBoreder(Dictionary<string, Vector4> tIpterMap, TextureImporter tIpter)
    {
        for (int i = 0, size = tIpter.spritesheet.Length; i < size; i++)
        {
            tIpterMap.Add(tIpter.spritesheet[i].name, tIpter.spritesheet[i].border);
        }
    }

    static TextureImporter GetTextureIpter(Texture2D texture)
    {
        TextureImporter textureIpter = null;
        string impPath = AssetDatabase.GetAssetPath(texture);
        textureIpter = TextureImporter.GetAtPath(impPath) as TextureImporter;
        return textureIpter;
    }

    static TextureImporter GetTextureIpter(string path)
    {
        TextureImporter textureIpter = null;
        Texture2D textureOrg = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        string impPath = AssetDatabase.GetAssetPath(textureOrg);
        textureIpter = TextureImporter.GetAtPath(impPath) as TextureImporter;
        return textureIpter;
    }
    //写信息到SpritesSheet里
    static void WriteMeta(XmlNodeList elemList, string sheetName, Dictionary<string, Vector4> borders,string path)
    {
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        string impPath = AssetDatabase.GetAssetPath(texture);
        TextureImporter asetImp = TextureImporter.GetAtPath(impPath) as TextureImporter;
        SpriteMetaData[] metaData = new SpriteMetaData[elemList.Count];
        for (int i = 0, size = elemList.Count; i < size; i++)
        {
            XmlElement node = (XmlElement)elemList.Item(i);
            Rect rect = new Rect();
            rect.x = int.Parse(node.GetAttribute("x"));
            rect.y = texture.height - int.Parse(node.GetAttribute("y")) - int.Parse(node.GetAttribute("height"));
            //rect.y = int.Parse(node.GetAttribute("y")) ;
            rect.width = int.Parse(node.GetAttribute("width"));
            rect.height = int.Parse(node.GetAttribute("height"));
            metaData[i].rect = rect;
            metaData[i].pivot = new Vector2(0.5f, 0.5f);
            metaData[i].name = node.GetAttribute("name");
            if (borders.ContainsKey(metaData[i].name))
            {
                metaData[i].border = borders[metaData[i].name];
            }
        }
        asetImp.spritesheet = metaData;
        asetImp.textureType = TextureImporterType.Sprite;
        asetImp.spriteImportMode = SpriteImportMode.Multiple;
        asetImp.mipmapEnabled = false;
        asetImp.SaveAndReimport();
    }

    static string GetAssetPath(string path)
    {
        string[] seperator = { "Assets" };
        string p = "Assets" + path.Split(seperator, System.StringSplitOptions.RemoveEmptyEntries)[1];
        return p;
    }

    #endregion
}
