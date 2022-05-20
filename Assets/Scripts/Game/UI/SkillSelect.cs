using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillSelect : MonoBehaviour
{
    [SerializeField] List<GameObject> _selectList;

    List<SkillSelectTable> _selectTable = new List<SkillSelectTable>();
    List<UnityEngine.UI.Text> _selectText = new List<UnityEngine.UI.Text>();
    CanvasGroup _canvas;

    bool _startEvent = false;

    private void Awake()
    {
        _canvas = GetComponent<CanvasGroup>();
        //_canvas.alpha = 0;
    }

    void Start()
    {
        for (int i = 0; i < _selectList.Count; ++i)
        {
            _selectTable.Add(null);
            _selectText.Add(_selectList[i].GetComponentInChildren<UnityEngine.UI.Text>());
            {
                var index = i;
                var btn = _selectList[i].GetComponentInChildren<UnityEngine.UI.Button>();
                btn.onClick.AddListener(()=>
                {
                    if (_canvas.alpha == 0) return;
                    OnClick(index);
                });
            }
        }
    }

    private void Update()
    {
        if(_startEvent)
        {
            SelectStart();
            _startEvent = false;
        }
    }

    public void SelectStartDelay()
    {
        _startEvent = true;
    }

    public void SelectStart()
    {
        _canvas.alpha = 1;

        List<SkillSelectTable> table = new List<SkillSelectTable>();
        var list = GameData.SkillSelectTable.Where(s => GameManager.Level >= s.Level);

        int totalProb = list.Sum(s => s.Probability);
        int rand = Random.Range(0, totalProb);

        for (int i = 0; i < _selectList.Count; ++i)
        {
            _selectTable[i] = null;
            _selectText[i].text = "";
        }

        for (int i = 0; i < _selectList.Count; ++i)
        {
            foreach (var s in list)
            {
                if(rand < s.Probability)
                {
                    _selectTable[i] = s;
                    _selectText[i].text = s.Name;
                    list = list.Where(ls => !(ls.Type == s.Type && ls.TargetId == s.TargetId));
                    break;
                }
                rand -= s.Probability;
            }
        }
    }

    public void OnClick(int index)
    {
        GameManager.Instance.LevelUpSelect(_selectTable[index]);

        _canvas.alpha = 0;
    }
}
