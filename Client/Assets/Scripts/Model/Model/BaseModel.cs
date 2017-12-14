
[System.Serializable]
public class BaseModel
{
    public int id;
    public string name;
    public string icon;

    public string describe;
    public string effect;
    /// <summary>
    /// 效果
    /// </summary>
    public string GetDescribe { get { return describe; } }
    
    public BaseModel Copy()
    {
        BaseModel model = new BaseModel();
        model.id = id;
        model.name = name;
        model.icon = icon;
        model.describe = describe;
        model.effect = effect;
        return model;
    }
}
