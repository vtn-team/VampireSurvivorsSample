using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float _time = 0.05f;
    [SerializeField] Enemy _prefab = null;
    [SerializeField] Transform _root = null;

    float _timer = 0.0f;
    float _cRad = 0.0f;
    Vector3 _popPos = new Vector3(0, 0, 0);

    ObjectPool<Enemy> _enemyPool = new ObjectPool<Enemy>();

    private void Start()
    {
        _enemyPool.SetBaseObj(_prefab, _root);
        _enemyPool.SetCapacity(1000);
        GameManager.Instance.SetList();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _time)
        {
            Spawn();
            _timer -= _time;
        }
    }

    void Spawn()
    {
        var script = _enemyPool.Instantiate();
        /*
        var go = GameObject.Instantiate(_prefab);
        var script = go.GetComponent<Enemy>();
        */
        _popPos.x = GameManager.Player.RectTransform.position.x + 1080 * Mathf.Cos(_cRad);
        _popPos.y = GameManager.Player.RectTransform.position.y + 1080 * Mathf.Sin(_cRad);
        script.RectTransform.position = _popPos;
        _cRad += 0.1f;
    }
}
