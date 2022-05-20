using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 0.1f;
    [SerializeField] float _shootTime = 0.3f;

    List<ISkill> _skill = new List<ISkill>();

    float _timer = 0.0f;

    void Awake()
    {
        GameManager.Instance.SetPlayer(this);

        //初期武器
        AddSkill(1);
    }

    private void Update()
    {
        float w = Input.GetAxis("Horizontal");
        float h = Input.GetAxis("Vertical");

        transform.position += new Vector3(w * _speed * Time.deltaTime, h * _speed * Time.deltaTime, 0);

        _skill.ForEach(s => s.Update());
    }

    public void AddSkill(int skillId)
    {
        var having = _skill.Where(s => s.SkillId == (SkillDef)skillId);
        if(having.Count() > 0)
        {
            having.Single().Levelup();
        }
        else
        {
            ISkill newSkill = null;
            switch((SkillDef)skillId)
            {
                case SkillDef.ShotBullet:
                    newSkill = new ShotBullet();
                    break;

                case SkillDef.AreaAttack:
                    newSkill = new AreaAttack();
                    break;
            }

            if (newSkill != null)
            {
                newSkill.Setup();
                _skill.Add(newSkill);
            }
        }
    }
}
