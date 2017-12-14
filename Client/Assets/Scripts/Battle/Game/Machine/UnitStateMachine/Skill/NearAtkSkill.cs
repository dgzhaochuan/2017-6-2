

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NearAtkSkill : BaseSkillObject
{
    protected override void Start_Skill()
    {
        base.Start_Skill();
        OnAtk();
    }
    protected override void End_Move()
    {
        base.End_Move();
        Exit();
    }
}