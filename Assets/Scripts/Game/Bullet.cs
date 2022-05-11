using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour, IObjectPool
{
    [SerializeField] float _speed = 255;
    RectTransform _rectTransform;
    UnityEngine.UI.Image _image;
    Enemy _target;
    Vector3 _shootVec;
    public RectTransform RectTransform => _rectTransform;

    float _timer = 0.0f;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<UnityEngine.UI.Image>();
    }

    public void Shoot()
    {
        var list = GameManager.EnemyList;
        _target = null;
        float len = -1;
        Vector3 vec;
        foreach(var e in list)
        {
            if (!e.IsActive) continue;
            vec = e.RectTransform.position - GameManager.Player.RectTransform.position;
            if(len == -1 || vec.magnitude < len)
            {
                _target = e;
                len = vec.magnitude;
            }
        }

        _shootVec = _target.RectTransform.position - GameManager.Player.RectTransform.position;
        _shootVec.Normalize();
    }

    void Update()
    {
        _rectTransform.position += _shootVec * _speed * Time.deltaTime;

        var list = GameManager.EnemyList;
        _target = null;
        Vector3 vec;
        foreach (var e in list)
        {
            if (!e.IsActive) continue;

            vec = e.RectTransform.position - this.RectTransform.position;
            if (vec.magnitude < 25.0f)
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
