using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager
{
    static private GameManager _instance = new GameManager();
    static public GameManager Instance => _instance;
    private GameManager() { }

    int _exp = 0;
    int _level = 1;
    int _stackLevelup = 0;
    Player _player = null;
    public void SetPlayer(Player p) { _player = p; }
    List<Enemy> _enemies = new List<Enemy>();
    List<int> _passive = new List<int>();
    SkillSelect _sklSelect = null;

    public void Setup()
    {
        //ObjectPoolに依存している
        _enemies = GameObject.FindObjectsOfType<Enemy>(true).ToList();

        _sklSelect = GameObject.FindObjectOfType<SkillSelect>();
    }

    public void GetExperience(int exp)
    {
        _exp += exp;

        //level up
        if (GameData.LevelTable.Count > _level && _exp > GameData.LevelTable[_level])
        {
            _level++;

            if (Time.timeScale > 0.99f)
            {
                _sklSelect.SelectStart();
                Time.timeScale = 0;
            }
            else
            {
                _stackLevelup++;
            }
        }
    }

    public void LevelUpSelect(SkillSelectTable table)
    {
        switch(table.Type)
        {
            case SelectType.Skill:
                _player.AddSkill(table.TargetId);
                break;

            case SelectType.Passive:
                _passive.Add(table.TargetId);
                break;

            case SelectType.Execute:
                //TODO:
                break;
        }

        if (_stackLevelup > 0)
        {
            _sklSelect.SelectStartDelay();
            _stackLevelup--;
        }
        else
        {
            Time.timeScale = 1;
        }
    }


    static public Player Player => _instance._player;
    static public List<Enemy> EnemyList => _instance._enemies;
    static public int Level => _instance._level;
}
