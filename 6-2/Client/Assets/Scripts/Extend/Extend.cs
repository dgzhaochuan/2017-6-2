using System;
using UnityEngine;
using System.Collections.Generic;
using UI;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

static public class Extend
{
    static public T NewPanel<T>(this Transform transform, GameObject prefab) where T : Panel
    {
        GameObject game = UnityEngine.Object.Instantiate<GameObject>(prefab);
        T panel = game.AddComponent<T>();
        game.transform.SetParent(transform, false);
        RectTransform rect = game.GetComponent<RectTransform>();
        if (rect.anchorMin == Vector2.zero && rect.anchorMax == Vector2.one)
        {
            rect.sizeDelta = Vector2.zero;
        }
        rect.anchoredPosition = Vector2.zero;
        game.transform.rotation = Quaternion.Euler(Vector3.zero);
        game.transform.localScale = Vector3.one;
        panel.mAwake();
        panel.gameObject.SetActive(false);
        return panel;
    }

    static public int ToInt(this float value)
    {
        return (int)value;
    }

    static public T Copy<T>(this System.Object _obj) 
    {
        BinaryFormatter BF2 = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream())
        {
            BF2.Serialize(stream, _obj);
            stream.Position = 0;
            var obj= BF2.Deserialize(stream);
            return (T)obj;
        }
    }

    static public void UpdateAdditional(this BaseAttribute data, BaseAttribute _base, AdditionalAttribute Additional)
    {
        for (int i = 0; i < Additional.data.Count; i++)
        {
            data[Additional.data[i].Type] += (int)Additional.data[i].GetValue(_base);
        }
        for (int i = 0; i < Additional.attribute.Count; i++)
        {
            data[(AttributeEnum)Additional.attribute[i].Type] += (int)Additional.attribute[i].GetValue(_base);
        }
        for (int i = 0; i < Additional.resistance.Count; i++)
        {
            data[(ResistanceEnum)Additional.resistance[i].Type] += (int)Additional.resistance[i].GetValue(_base);
        }
        for (int i = 0; i < Additional.skill.Count; i++)
        {
            data[(skillEnum)Additional.resistance[i].Type] += (int)Additional.skill[i].GetValue(_base);
        }
    }

    static public void AddListModel(this BaseAttribute data, BaseAttribute _base, List<AdditionalModel> item)
    {
        for (int i = 0; i < item.Count; i++)
        {
            switch (item[i].Type)
            {
                case 1:
                    data[item[i].Type] += item[i].GetValue(_base);
                    break;
                case 2:
                    data[(AttributeEnum)item[i].Type] += item[i].GetValue(_base);
                    break;
                case 3:
                    data[(ResistanceEnum)item[i].Type] += item[i].GetValue(_base);
                    break;
                case 4:
                    data[(skillEnum)item[i].Type] += item[i].GetValue(_base);
                    break;
            }
        }
    }

    static public GameObject UIInstantiate(this GameObject prefab, Transform parent, string name = "")
    {
        if (name == "") name = prefab.name;
        GameObject game = UnityEngine.Object.Instantiate<GameObject>(prefab);
        game.transform.SetParent(parent, false);
        game.transform.localScale = Vector3.one;
        game.name = name;
        return game;
    }

    static public bool IsNull(this string str)
    {
        return string.IsNullOrEmpty(str);
    }
    
    static public void SetParent(this GameObject obj, Transform parent)
    {
        obj.transform.SetParent(parent, false);
        obj.transform.localScale = Vector3.one;
        obj.transform.rotation = Quaternion.Euler(Vector3.zero);
        obj.transform.localPosition = Vector3.zero;
    }

    static public int Clamp(this int value, int min, int max)
    {
        return Mathf.Clamp(value, min, max);
    }
    static public float Clamp(this float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }
    static public int RoundToInt(this float value)
    {
        return Mathf.RoundToInt(value);
    }

    static public int GetCurrentValue(int min, int max, float currentValue, float maxValue)
    {
        float value = 0;
        float ap = 0;
        value = currentValue / maxValue;
        value = value.Clamp(0, 1);
        ap = min + (1 - value) * max;
        return ap.Clamp(min, max).RoundToInt();
    }

    static public int GetDataEnumType(this BaseAttribute attribute, int type, int EnumType)
    {
        switch (type)
        {
            case 1:
                return attribute[(DataEnum)EnumType];
            case 2:
                return attribute[(AttributeEnum)EnumType];
            case 3:
                return attribute[(ResistanceEnum)EnumType];
            case 4:
                return attribute[(skillEnum)EnumType];
            case 5:
                return attribute[EnumType];
            default:
                return 0;
        }
    }
    //返回列表最后一个
    static public T Last<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }
    //移除列表最后一个
    static public void ReLast<T>(this List<T> list)
    {
        list.RemoveAt(list.Count - 1);
    }

    static public List<AdditionalModel> GetAdditionalModel(this List<float[]> data)
    {
        List<AdditionalModel> array = new List<AdditionalModel>();
        for (int i = 0; i < data.Count; i++)
        {
            array.Add(new AdditionalModel(data[i]));
        }
        return array;
    }



    //battle
    static public Color GetCellColor(this HexCell cell)
    {
        switch (cell.State)
        {
            case HexCellState.Null:
                return Config.Nullcolor;
            case HexCellState.Move:
                return Config.Opencolor;
            case HexCellState.MovePath:
                return Config.MovePathcolor;
            case HexCellState.Obstacle:
                return Config.Obstaclecolor;
            case HexCellState.Attack:
                return Config.Attackcolor;
            default:
                return Color.white;
        }
    }
    static public int GetAtkValue(this UnitModel model)
    {
        int value = (int)(model[DataEnum.matk] * Config.AtkOff);
        return UnityEngine.Random.Range(model[DataEnum.Atk]-value, model[DataEnum.Atk]+value);
    }
    static public void UIReset(this Transform transform, Transform parent)
    {
        transform.SetParent(parent, false);
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
    }

    static public int GetValue(this BaseAdditionalAttribute attribute,DataEnum type)
    {
        return attribute.baseAttribute.data.Find(a => a.Type == (int)type).GetValue();
    }

    static public void   UpdatePrompt(string msg)
    {
        PanelManager.Instantiate.GetPanel<PromptPanel>().Open(msg);
    }
    static public void ClosePrompt( )
    {
        PanelManager.Instantiate.GetPanel<PromptPanel>().Close();
    }

    static public Vector2 MouseUI_Point(Canvas canvas)
    {
        Vector2 pos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.GetComponent<Camera>(), out pos);

        return pos;
    }

    

}

