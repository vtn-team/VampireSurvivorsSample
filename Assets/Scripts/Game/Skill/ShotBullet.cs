using System;
using UnityEngine;

public class ShotBullet : ISkill
{
    public SkillDef SkillId => SkillDef.ShotBullet;
    IntervalTimer _timer = new IntervalTimer();

    ObjectPool<Bullet> _bulletPool = new ObjectPool<Bullet>();

    float _interval = 0.5f;
    int _shotCount = 1;

    public void Setup()
    {
        _timer.Setup(0.5f);

        Bullet prefab = Resources.Load<Bullet>("Bullet");
        Transform root = GameObject.Find("/BulletRoot").transform;

        _bulletPool.SetBaseObj(prefab, root);
        _bulletPool.SetCapacity(100);
    }

    public void Update()
    {
        if(_timer.RunTimer())
        {
            var list = GameManager.EnemyList;
            Enemy[] targets = new Enemy[_shotCount];

            for (int i = 0; i < targets.Length; ++i)
            {
                targets[i] = null;
            }
            float len = -1;
            Vector3 vec;
            foreach (var e in list)
            {
                if (!e.IsActive) continue;
                vec = e.transform.position - GameManager.Player.transform.position;
                if (len == -1 || vec.magnitude < len)
                {
                    for (int i = 1; i < targets.Length; ++i)
                    {
                        targets[i] = targets[i-1];
                    }
                    targets[0] = e;
                    len = vec.magnitude;
                }
            }

            if (targets[0] == null) return;


            for (int i = 0; i < targets.Length; ++i)
            {
                var script = _bulletPool.Instantiate();
                script.transform.position = GameManager.Player.transform.position;
                script.Shoot(targets[i]);
            }
        }
    }

    public void Levelup()
    {
        _shotCount+=2;
    }
}
