
using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;


public static class ReadData
{
    public static SaveObject<T> ReadJson<T>(string FileName) where T : BaseModel
    {
        string path = DataManager.GetJsonPath(FileName);
        string data = File.ReadAllText(path, System.Text.Encoding.Default);
        SaveObject<T> model = JsonUtility.FromJson<SaveObject<T>>(data);
        return model;
    }
    public static List<T> ReadExcel<T>(string path, string filename) where T : BaseModel
    {
        List<T> models = new List<T>();
        //string[] str = OpenFile(path).Split(new string[] { "\r\n" }, StringSplitOptions.None);
        string[] str = Excel(path);
        string[] keys = str[3].Split(',');
        string[] datas = null;
        string[] dataType = str[2].Split(',');
        for (int i = 5; i < str.Length; i++)
        {
            if (string.IsNullOrEmpty(str[i])) continue;
            datas = str[i].Split(',');
            if (datas[0].IsNull()) continue;
            Type t = typeof(T);
            object obj = GetObject<T>(t);
            for (int j = 0; j < datas.Length; j++)
            {
                MethodInfo mi = t.GetMethod(keys[j], BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                try
                {
                    if (mi != null)
                    {
                        object objdata = GetFieldInfo(dataType[j], datas[j]);
                        mi.Invoke(obj, new object[] { objdata });
                    }
                    else
                    {
                        FieldInfo fi = t.GetField(keys[j], BindingFlags.Public | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            object objdata = GetFieldInfo(dataType[j], datas[j]);
                            fi.SetValue(obj, objdata);
                        }
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError("FileName ->" + filename
                        + "\n dataType ->" + dataType[j] + "\n Key ->" + keys[j] + "\n data ->" + datas[j]);
                }
            }
            models.Add(obj as T);
        }
        return models;
    }
    static object GetFieldInfo(string type, string data)
    {
        switch (int.Parse(type))
        {
            case 1:
                return data;
            case 2:
                if (string.IsNullOrEmpty(data)) return 0;
                return int.Parse(data);
            case 3:
                if (string.IsNullOrEmpty(data)) return 0.0f;
                return float.Parse(data);
            case 4:
                if (string.IsNullOrEmpty(data)) return new int[0];
                return GetIntAry(data);
            case 5:
                if (string.IsNullOrEmpty(data)) return new float[0];
                return GetFloatAry(data);
            case 6:
                if (string.IsNullOrEmpty(data)) return new List<int[]>();
                {
                    data = Replace(data);
                    string[] _data = data.Split('|');
                    List<int[]> value = new List<int[]>();
                    for (int i = 0; i < _data.Length; i++)
                    {
                        if (string.IsNullOrEmpty(_data[i])) continue;
                        value.Add(GetIntAry(_data[i]));
                    }
                    return value;
                }
            case 7:
                if (string.IsNullOrEmpty(data)) return new List<float[]>();
                {
                    data = Replace(data);
                    string[] _data = data.Split('|');
                    List<float[]> value = new List<float[]>();
                    for (int i = 0; i < _data.Length; i++)
                    {
                        if (string.IsNullOrEmpty(_data[i])) continue;
                        value.Add(GetFloatAry(_data[i]));
                    }
                    return value;
                }
        }
        return null;
    }
    static int[] GetIntAry(string data)
    {
        string[] _data = data.Split(':');
        int[] value = new int[_data.Length];
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = int.Parse(_data[i]);
        }
        return value;
    }
    static float[] GetFloatAry(string data)
    {
        string[] _data = data.Split(':');
        float[] value = new float[_data.Length];
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = float.Parse(_data[i]);
        }
        return value;
    }
    static string Replace(string value)
    {
        value = value.Replace("\r", "");
        value = value.Replace("\n", "");
        value = value.Replace("\"", "");
        //data = data.Replace("\\", "");
        return value;
    }
    static object GetObject<T>(Type t)
    {
        Type[] types = new Type[0];//为构造函数准备参数类型  
        ConstructorInfo ci = t.GetConstructor(types); //获得构造函数  
        object[] objs = new object[0];//为构造函数准备参数值  
        object obj = ci.Invoke(objs);//调用构造函数创建对象  
        return obj;
    }
    static public string OpenFile(string path)
    {
        string value = File.ReadAllText(path, Encoding.Default);
        value = value.Replace(" ", "");
        return value;
    }


    static string[] Excel(string fileNamet)
    {
        string xmlName = Path.GetFileNameWithoutExtension(fileNamet);
        DataSet _Excel = ReadExcel(fileNamet);
        //取得表格中的数据
        //取得table中所有的行数据
        DataRowCollection rowCollection = _Excel.Tables[0].Rows;
        //取得table中的列数据
        DataColumnCollection columnCollection = _Excel.Tables[0].Columns;


        //取得行中所有个数
        int rowCount = rowCollection.Count;
        //取得列中所有个数
        int columnCount = columnCollection.Count;

        string[] data = new string[rowCount];
        //遍历行数
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                string value = rowCollection[i][j].ToString() + ",";
                value = value.Replace("：", ":");
                data[i] += value;
            }
        }
        return data;
    }
    static DataSet ReadExcel(string path)
    {
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        return excelReader.AsDataSet();
    }



    #region  反射
    /*
    class Program
    {
        static void Main(string[] args)
        {
            Type t = typeof(ChangeValue);
            Type[] types = new Type[0];//为构造函数准备参数类型  
            ConstructorInfo ci = t.GetConstructor(types); //获得构造函数  
            object[] objs = new object[0];//为构造函数准备参数值  
            object obj = ci.Invoke(objs);//调用构造函数创建对象  
            MethodInfo mi = t.GetMethod("WriteLine");//获得公有的writeline方法  
            //mi.Invoke(obj, null);
            FieldInfo fi = t.GetField("firstValue", BindingFlags.Public | BindingFlags.Instance);//获得私有字段  
            Console.WriteLine(fi.GetValue(obj) + "    aaa");

            fi.SetValue(obj, "bbbbbbbbbb");
            Console.WriteLine(fi.GetValue(obj) + "    aaa");

            FieldInfo[] fis = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            Console.WriteLine(fis.Length);
            foreach (var fieldInfo in fis)
            {
                Console.WriteLine(fieldInfo.GetValue(obj));
            }
            //fi.SetValue(obj, "new Value"); //改写私有字段  
            //mi.Invoke(obj, null);
            //MethodInfo mi2 = t.GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance);//获得私有的Write方法  
            //mi2.Invoke(obj, null);
            Console.ReadLine();
        }
    }
    public class ChangeValue
    {
        private string myValue = "old Value";
        public string firstValue = "values1";
        public void WriteLine()
        {
            Console.WriteLine("MyValue is: " + myValue);
        }
        private void Write()
        {
            Console.WriteLine("MyValue is: " + myValue);
        }
    }
    */
    #endregion
}


