
using UnityEngine;

public class Config
{

    public static Color white = Color.black;
    public static Color DownColor = Color.red;
    public static Color UpColor = Color.green;
    

    public const int StartPropCount = 4;
    public const int BackPackCount=20;
    public const float sell = .3f;
    //攻击力浮动
    public const float AtkOff = .1f;


    public const int EquipmentEnumLeng = 4;
    public const int MaxSkillCount = 11;




    private static int siblingIndex = 0;
    public static int SiblingIndex { get { siblingIndex++;return siblingIndex; } }


    #region prefabName
    /// <summary>
    /// 道具格子
    /// </summary>
    public const string PropStatsGame = "PropStatsGame";
    /// <summary>
    /// 角色界面
    /// </summary>
    public const string CharacterPanel = "CharacterPanel";
    /// <summary>
    /// 背包界面
    /// </summary>
    public const string BackPackPanel = "BackPackPanel";
    /// <summary>
    /// 错误提示界面
    /// </summary>
    public const string ErrorPanel = "ErrorPanel";
    /// <summary>
    /// 装备界面
    /// </summary>
    public const string PropStatsPanel = "PropStatsPanel";
    /// <summary>
    /// 选择角色界面
    /// </summary>
    public const string SelectedCharacterPanel = "SelectedCharacterPanel";
    /// <summary>
    /// 角色界面属性展示
    /// </summary>
    public const string AttributeStatsGame = "AttributeStatsGame";
    /// <summary>
    /// 装备界面属性展示
    /// </summary>
    public const string EquipemtAttributeGame = "EquipemtAttributeGame";
    /// <summary>
    /// 商店
    /// </summary>
    public const string ShopPanel = "ShopPanel";
    /// <summary>
    /// 数字输入
    /// </summary>
    public const string InputNumberPanel = "InputNumberPanel";
    /// <summary>
    /// 折叠
    /// </summary>
    public const string FoldTool = "FoldTool";
    /// <summary>
    /// 技能列表图标
    /// </summary>
    public const string SkillStatsGame = "SkillStatsGame";
    /// <summary>
    /// 技能折叠预制
    /// </summary>
    public const string FoldToolPrefab = "FoldToolPrefab";
    /// <summary>
    /// 技能详细属性
    /// </summary>
    public const string SkillStatsPanel = "SkillStatsPanel";
    /// <summary>
    /// 选择出站单位
    /// </summary>
    public const string SelectBattleUnitPanel = "SelectBattleUnitPanel";

    public const string LabelPrefab = "Label";

    #endregion






    //battle
    static public Color Nullcolor = Color.grey;
    static public Color Opencolor = Color.green;
    static public Color MovePathcolor = Color.yellow;
    static public Color Obstaclecolor = Color.yellow;
    static public Color Attackcolor = Color.red;
    public const float pathDepth = 2;

    public const int playerRanks = 1;
    public const int HexLayer = 8;
    public const int TestLayer = 31;
    public const int UnitLayer = 9;
    public const float walkSpeed = .2f;

    public const string Obstacle = "Obstacle";
    public const string Hex = "Hex";
    public const string HUD = "HUD";
    public const string WorldUI = "WorldUI";


    public const int ValueAp = 5;
    public const int ValueResistance = 80;
    public const int ValueDef = 30;
    public const int ValueMdef = 40;
    public static int[] ValueData = new int[] {-20,-1,50,0,50,-25, 30,60,-15,15,0,60,60,0,1,1,5,0,50 };
    public static int[] ValueAttribute = new int[] {5,5,5,5,5 };
    public static int[] ValueSkill = new int[] { 0,0,0,0,0,0,0};

}

