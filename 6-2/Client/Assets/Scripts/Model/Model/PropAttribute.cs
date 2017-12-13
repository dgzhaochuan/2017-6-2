using System;
using System.Collections.Generic;
using System.Text;


    /// <summary>
    /// 道具
    /// </summary>
    [System.Serializable]
    public class PropAttribute:BaseAdditionalAttribute
    {
        public propEnum type;

        /// <summary>
        /// 消耗
        /// </summary>
        public AdditionalModel[] consumeData = new AdditionalModel[0];
        /// <summary>
        /// 触发类型 1主动/被动
        /// </summary>
        public int skillType;
        /// <summary>
        /// 目标敌方/己方
        /// </summary>
        public targetTypeEnum targetType;
        /// <summary>
        /// 0无，选择地图格子就能释放
        /// 1敌人，选择敌人
        /// 2队友，选中队友
        /// </summary>
        public int releaseType;
        /// <summary>
        /// 目标数量 目标是己方全体或者是地方全体的时候忽略这个参数
        /// </summary>
        public int targetCount;

        public override AdditionalAttributeEnum PropType
        {
            get
            {
                return AdditionalAttributeEnum.prop;
            }
        }
        public override string getType
        {
            get
            {
                return type.ToString();
            }
        }

    }
