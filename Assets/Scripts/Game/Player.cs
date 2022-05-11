﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 0.1f;
    [SerializeField] float _shootTime = 0.3f;
    [SerializeField] Bullet _prefab = null;
    [SerializeField] Transform _root = null;
    RectTransform _rectTransform;
    public RectTransform RectTransform => _rectTransform;

    float _timer = 0.0f;

    ObjectPool<Bullet> _bulletPool = new ObjectPool<Bullet>();

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        GameManager.Instance.SetPlayer(this);
    }

    private void Start()
    {
        _bulletPool.SetBaseObj(_prefab, _root);
        _bulletPool.SetCapacity(100);
    }

    private void Update()
    {
        float w = Input.GetAxis("Horizontal");
        float h = Input.GetAxis("Vertical");

        _rectTransform.position += new Vector3(w * _speed * Time.deltaTime, h * _speed * Time.deltaTime, 0);

        _timer += Time.deltaTime;
        if (_timer > _shootTime)
        {
            var script = _bulletPool.Instantiate();
            script.RectTransform.position = this.RectTransform.position;
            script.Shoot();
            _timer -= _shootTime;
        }
    }
}