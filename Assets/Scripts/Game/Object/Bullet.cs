using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour, IObjectPool
{
    [SerializeField] float _speed = 255;
    SpriteRenderer _image;
    Enemy _target;
    Vector3 _shootVec;

    float _timer = 0.0f;

    void Awake()
    {
        _image = GetComponent<SpriteRenderer>();
    }

    public void Shoot(Enemy target)
    {
        _target = target;
        if (_target == null) return;

        _shootVec = _target.transform.position - GameManager.Player.transform.position;
        _shootVec.Normalize();
    }

    void Update()
    {
        if (!_isActrive) return;

        transform.position += _shootVec * _speed * Time.deltaTime;

        var list = GameManager.EnemyList;
        _target = null;
        Vector3 vec;
        foreach (var e in list)
        {
            if (!e.IsActive) continue;

            vec = e.transform.position - this.transform.position;
            if (vec.magnitude < 1.5f)
            {
                e.Damage();
                Destroy();
                break;
            }
        }

        _timer += Time.deltaTime;
        if(_timer > 3.0f)
        {
            Destroy();
        }
    }

    //ObjectPool
    bool _isActrive = false;
    public bool IsActive => _isActrive;
    public void DisactiveForInstantiate()
    {
        _image.enabled = false;
        _isActrive = false;
    }
    public void Create()
    {
        _timer = 0.0f;
        _image.enabled = true;
        _isActrive = true;
    }
    public void Destroy()
    {
        _image.enabled = false;
        _isActrive = false;
    }
}
