using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

using System.Linq;



[System.Serializable]
public struct GameNumber
{
    public int id;
    public int number;
    public AdditionalAttributeEnum PropType;
    public bool IsNull { get { return id == 0 || number == 0|| PropType==AdditionalAttributeEnum.none; } }
    public GameNumber(int id,int number, AdditionalAttributeEnum PropType)
    {
        this.id = id;
        this.number = number;
        this.PropType = PropType;
    }
    public GameNumber(BaseAdditionalAttribute attribute)
    {
        id = attribute.id;
        number = attribute.number;
        this.PropType = attribute.PropType;
    }
    public void SetNull()
    {
        id = 0;
        number = 0;
        PropType = AdditionalAttributeEnum.none;
    }
    public void SetData(GameNumber game)
    {
        this.id = game.id;
        this.number = game.number;
        this.PropType = game.PropType;
    }
    public bool Equals(BaseAdditionalAttribute game)
    {
        return game.id == this.id &&game.PropType == this.PropType;
    }

}

public static class SaveSprite
{


    #region SaveModel

    [System.Serializable]
    public class SaveModel
    {
        public int money = 100;
        public GameNumber[] Prop = new GameNumber[Config.BackPackCount];
        public List<CharacterAttribute> CharacterAry = new List<CharacterAttribute>();

        public SaveModel()
        {
            money = 100;
            Prop = new GameNumber[Config.BackPackCount];

            //TODO
            CharacterAry = new List<CharacterAttribute>();
            var unitAry = Manage.Instance.Data.GetDictionary<CharacterAttribute>().Values;
            foreach (CharacterAttribute item in unitAry)
            {
                var unit = item.Copy<CharacterAttribute>();
                unit.attributeCount = 6;
                CharacterAry.Add(unit);
            }
        }
    }

    #endregion




    const string SaveModelFileName = "SaveModel";
    static public SaveModel Model;
    static public Dictionary<int, CharacterAttribute> CharacterAry = new Dictionary<int, CharacterAttribute>();

    static public string Path(string fileName)
    {
        if(Directory.Exists(Application.persistentDataPath + "/Json")==false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Json");
        }
        return System.IO.Path.Combine(Application.persistentDataPath+"/Json/", fileName);
    }

    static SaveSprite()
    {
        Read();
    }
    static void Read()
    {
        string data = ReadFile(Path(SaveModelFileName));
        if (string.IsNullOrEmpty(data))
        {
            Model = new SaveModel();
            CharacterAry.Clear();
            for (int i = 0; i < Model.CharacterAry.Count; i++)
            {
                CharacterAry.Add(Model.CharacterAry[i].id, Model.CharacterAry[i]);
            }
            Write();
            Read();
        }
        else
        {
            Model = JsonUtility.FromJson<SaveModel>(data);
            CharacterAry.Clear();
            for (int i = 0; i < Model.CharacterAry.Count; i++)
            {
                CharacterAry.Add(Model.CharacterAry[i].id, Model.CharacterAry[i]);
            }
        }
    }
    static public void Write()
    {
        Model.CharacterAry = CharacterAry.Values.ToList();
        WriteFile(SaveModelFileName, JsonUtility.ToJson(Model));
    }

    static public int GetNullBackPackItem(int number = -1)
    {
        int count = 0;
        for (int i = 0; i < Model.Prop.Length; i++)
        {
            if (Model.Prop[i].IsNull)
            {
                count++;
                if (number != -1)
                {
                    if (count >= number) return count;
                }
            }
        }
        return count;
    }
    static public bool BackPackIsNull(int number)
    {
        int count = 0;
        for (int i = 0; i < Model.Prop.Length; i++)
        {
            if (Model.Prop[i].IsNull)
            {
                count++;
                if (count >= number)
                {
                    return true;
                }
            }
        }
        return false;
    }   
    static public bool AddProp(BaseAdditionalAttribute prop,int number)
    {
        //加入道具数量不能大于堆叠最大数量
        for (int i = 0; i < Model.Prop.Length; i++)
        {
            if (Model.Prop[i].IsNull)
            {
                GameNumber item = new GameNumber(prop);
                Model.Prop[i] = item;
                Write();
                return true;
            }
            if (Model.Prop[i].Equals(prop))
            {
                if (Model.Prop[i].number < prop.maxNumber)
                {
                    int count = Model.Prop[i].number + prop.number;
                    if (count > prop.maxNumber)
                    {
                        if (GetNullBackPackItem() < 1)
                        {
                            return false;
                        }
                        Model.Prop[i].number = prop.maxNumber;
                      return  AddProp(prop, count - prop.maxNumber);
                    }
                    else
                    {
                        Model.Prop[i].number = count;
                        Write();
                        return true;
                    }

                }
            }
        }
        return false;
    }
    static public bool RemoveProp(BaseAdditionalAttribute prop, int number)
    {
        for (int i = 0; i < Model.Prop.Length; i++)
        {
            if (Model.Prop[i].Equals(prop))
            {
                if (Model.Prop[i].number < number)
                {
                    if (RemoveProp(prop, number- Model.Prop[i].number))
                    {
                        Model.Prop[i].SetNull();
                        Write();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Model.Prop[i].number -= number;
                    if (Model.Prop[i].number <= 0)
                        Model.Prop[i].SetNull();
                    Write();
                    return true;
                }
            }
        }
        return false;
    }
    static public bool RemoveIndexProp(BaseAdditionalAttribute prop,int index, int number)
    {
        if (index == -1) return RemoveProp(prop,number);

        Model.Prop[index].number -= number;
        if (Model.Prop[index].number <= 0)
            Model.Prop[index].SetNull();
        Write();
        return true;
    }
    static public CharacterAttribute GetCharacterAttribute(int id)
    {
        return CharacterAry[id];
    }
    static public List<CharacterAttribute> GetCharacterAttributeAry()
    {
        return CharacterAry.Values.ToList();
    }
    static public void SetCharacterAttribute(CharacterAttribute model)
    {
        CharacterAry[model.id] = model;
        Write();
    }
    static public void AddMoney(float money)
    {
        Model.money += (int)money;
        Write();
    }
    static public string ReadFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            UnityEngine.Debug.LogError("文件不存在:" + filePath);
            return "";
        }
        return File.ReadAllText(filePath, System.Text.Encoding.Default);
    }
    static bool WriteFile(string fileName, string data)
    {
        string path = Path(fileName);
        try
        {
            File.WriteAllText(path, data, System.Text.Encoding.Default);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("写入文件" + path + "失败：" + e.Message);
            return false;
        }
    }
   public static bool WritePathFile(string path, string data)
    {
        try
        {
            File.WriteAllText(path, data, System.Text.Encoding.Default);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("写入文件" + path + "失败：" + e.Message);
            return false;
        }
    }
}