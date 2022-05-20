using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface ISkill
{
    SkillDef SkillId { get; }
    void Setup();
    void Update();
    void Levelup();
}

public enum SkillDef
{
    Invalid = 0,
    ShotBullet = 1,
    AreaAttack = 2,
}