using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IObjectPool
{
    [SerializeField] float _speed = 10;
    RectTransform _rectTransform;
    UnityEngine.UI.Image _image;
    public RectTransform RectTransform => _rectTransform;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<UnityEngine.UI.Image>();
    }

    void Update()
    {
        if (!IsActive) return;

        Vector3 sub = GameManager.Player.RectTransform.position - RectTransform.position;
        sub.Normalize();

        _rectTransform.position += sub * _speed * Time.deltaTime;
    }

    public void Damage()
    {
        //TODO
        Destroy();
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
        _image.enabled = true;
        _isActrive = true;
    }
    public void Destroy()
    {
        _image.enabled = false;
        _isActrive = false;
    }
}
