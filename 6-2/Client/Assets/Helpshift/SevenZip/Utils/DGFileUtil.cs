using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DGFileUtil
{

	public static List<string> GetAllFiles(DirectoryInfo dir, string except=null)
	{
		List<string> fileList = new List<string>();
		if(!Directory.Exists(dir.FullName) && !File.Exists(dir.FullName))
		{
			Debug.LogError("Directory not exists : "+dir.FullName);
			return fileList;
		}
		
		FileInfo[] allFile = dir.GetFiles();
		for(int i=0; i<allFile.Length; i++)
		{
			FileInfo fi = allFile[i];
			if(except != null && fi.Name.IndexOf(except) != -1)
			{
				continue;
			}
			fileList.Add(fi.FullName);
		}
		
		DirectoryInfo[] allDir= dir.GetDirectories();
		foreach (DirectoryInfo d in allDir)
		{
			if(d.FullName == dir.FullName)
			{
				continue;
			}
			fileList.AddRange(GetAllFiles(d, except));
		}
		return fileList;
	}

    public static void DeleteAllDirectory(string path)
    {
        DirectoryInfo info = new DirectoryInfo(path);
        List<string> tempLists = GetChildrenDirectory(info);
        for(int i=0; i < tempLists.Count; i++)
        {
            if(File.Exists(tempLists[i] + ".meta"))
            {
                File.Delete(tempLists[i] + ".meta");
            }
            Directory.Delete(tempLists[i]);
        }
    }

	public static List<string> GetChildrenDirectory(DirectoryInfo dir)
	{
		List<string> fileList = new List<string>();
		if(!Directory.Exists(dir.FullName) && !File.Exists(dir.FullName))
		{
			Debug.LogError("Directory not exists : "+dir.FullName);
			return fileList;
		}
		
		DirectoryInfo[] allDir= dir.GetDirectories();
		foreach (DirectoryInfo d in allDir)
		{
			if(d.FullName == dir.FullName)
			{
				continue;
			}
			fileList.Add(d.FullName);
		}
		return fileList;
	}

	public static void CreateDirectoryWhenNotExists(string destination)
	{
		destination = destination.Replace("\\", "/");// ‘/’替换‘\\’
        //截取“/”之前的字符串
		string dir = destination.Substring(0, destination.LastIndexOf("/"));//LastIndexOf的作用是对字符串进行从后往前的检索，找到第一个匹配的位置
        if (!Directory.Exists(dir))//判断指定路径文件是否存在
		{
            if(File.Exists(dir)) File.Delete(dir);
			Directory.CreateDirectory(dir);//在指定路径中创建所有目录和子目录,返回此目录的对象
        }
	}

	public static void CopyFile(string source, string destination)
	{
		CreateDirectoryWhenNotExists(destination);
		File.Copy(source, destination);
	}

	public static void CopyFileOrDirectory(string source, string destination, string except=null)
	{
		List<string> list = DGFileUtil.GetAllFiles(new DirectoryInfo(source), except);
		for(int i=0; i<list.Count; i++)
		{
			string path = list[i];
			string destPath = destination + path.Substring(source.Length);
			DGFileUtil.CopyFile(path, destPath);
		}
	}


	public static string GetFileNameByPath(string path)
	{
		path = path.Replace("\\", "/");
		return path.Substring(path.LastIndexOf("/")+1);
	}
}

