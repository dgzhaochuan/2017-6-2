using System.Collections.Generic;


[System.Serializable]
public class BuffModel : BaseModel
{
    /// <summary>
    /// 效果属性
    /// </summary>
    public List<AdditionalModel> AdditionalAttribute=new List<AdditionalModel>();
    /// <summary>
    /// 持续回合数
    /// </summary>
    public int Duration;
    /// <summary>
    /// 作用范围
    /// </summary>
    public int targetRange;
    /// <summary>
    /// 触发 0 开始结束出发，1 每回合触发
    /// </summary>
    public int IsContinued;
    public buffEnum buffType;
    /// <summary>
    /// 
    /// </summary>如果是个伤害buff的话 0是最低伤害，1是最高，2是伤害类型（抗性类型）
    public List<IntArray> startValue = new List<IntArray>();

    /// <summary>
    /// 如果类型是1伤害类型，第一个是最小伤害，比如说没回合伤害10-15
    /// 如果 是2就只有一个长度数组，1是是否百分百，比如说受到伤害减少50%
    /// </summary>
    //public int[][] Damage;
    /// <summary>
    /// 消耗类型
    /// 1 回合
    /// 2 物理攻击
    /// 3 魔法攻击
    /// 4 被击
    /// </summary>
    public int consume;

    public void SetStartValue(List<int[]> data)
    {
        startValue = new List<global::IntArray>();
        for (int i = 0; i < data.Count; i++)
        {
            startValue.Add(new IntArray(data[i]));
        }
    }


    public string GetEffect(BaseAttribute model)
    {
        int[][] Damage = GetDamage(model);
        string[] _describe = new string[Damage.Length];
        for (int i = 0; i < Damage.Length; i++)
        {
            _describe[i] = Damage[i][1].ToString();
            if (Damage[i][0] == 1)
            {
                _describe[i] += "%";
            }
        }
        return string.Format(GetDescribe, _describe);
    }

    public int[][] GetDamage(BaseAttribute attribute)
    {
        int[][] Damage = new int[this.startValue.Count][];
        for (int i = 0; i < Damage.Length; i++)
        {
            Damage[i] = new int[2];
            Damage[i][0] = startValue[i].data[0];
            Damage[i][1] += (startValue[i].data[1]);
            if (i < AdditionalAttribute.Count)
            {
                Damage[i][1] += AdditionalAttribute[i].GetValue(attribute);
            }
        }
        /*
        switch (valueType)
        {
            case 1:
                for (int i = 0; i < Damage.Length; i ++)
                {
                    Damage[i] = new int[2];
                    Damage[i][0] = startValue[i].data[0];
                    Damage[i][1] += (startValue[i].data[1]);
                    if (i < AdditionalAttribute.Count)
                    {
                        Damage[i][1] += AdditionalAttribute[i].GetValue(attribute);
                    }
                }
                break;
            case 2:
                for (int i = 0; i < Damage.Length; i += 2)
                {
                    Damage[i] = new int[2];
                    Damage[i][0] = startValue[i].data[0];
                    Damage[i][1] += (int)(this.startValue[i].data[1]);
                    if (i < AdditionalAttribute.Count)
                    {
                        Damage[i][1] += AdditionalAttribute[i].GetValue(attribute);
                    }
                }
                break;
        }
        */
        return Damage;
    }

    void SetAdditionalAttribute(List<float[]> data)
    {
        AdditionalAttribute = new List<global::AdditionalModel>();
        for (int i = 0; i < data.Count; i++)
        {
            var additiona = new AdditionalModel(data[i]);
            AdditionalAttribute.Add(additiona);
        }
    }
}
