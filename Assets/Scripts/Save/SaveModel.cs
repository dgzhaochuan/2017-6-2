
using System.Collections.Generic;


[System.Serializable]
public class SaveObject<T>where T:BaseModel
{
    public List<T> ObjAry = new List<T>();
}

