using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class TaskManager
{
    static private TaskManager _instance = new TaskManager();
    static public TaskManager Instance => _instance;
    private TaskManager() { }

    public delegate int UpdateCall();

    /// <summary>
    /// 優先度付きキュー
    /// 
    /// NOTE: 優先度で並び替えが行われる
    /// </summary>
    class PriorityQueue
    {
        enum UpdateType
        {
            Invalid,
            Task,
            Interface,
            Delegate,
        }
        public int Priority { get; private set; }   //優先度
        TaskBase _task;                      //
        ITaskBase _inf;                      //
        UpdateCall _call;                    //
        UpdateType _type = UpdateType.Task;

        /// <summary>
        /// 初期化
        /// </summary>
        public PriorityQueue(int p, UpdateCall call)
        {
            Priority = p;
            _call = call;
            _type = UpdateType.Delegate;
            if (_call == null)
            {
                _type = UpdateType.Invalid;
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public PriorityQueue(int p, ITaskBase inf)
        {
            Priority = p;
            _inf = inf;
            _type = UpdateType.Interface;
            if (_inf == null)
            {
                _type = UpdateType.Invalid;
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public PriorityQueue(int p, TaskBase task)
        {
            Priority = p;
            _task = task;
            _type = UpdateType.Task;
            if (_inf == null)
            {
                _type = UpdateType.Invalid;
            }
        }

        public int Update()
        {
            switch(_type)
            {
                case UpdateType.Invalid:
                    return 1;
                case UpdateType.Task:
                    return _task.Update();
                case UpdateType.Interface:
                    return _inf.Update();
                case UpdateType.Delegate:
                    return _call();
                default:
                    return 1;
            }
        }
    }

    TaskManagerAttachment _attach;
    LinkedList<PriorityQueue> _tasks = new LinkedList<PriorityQueue>();
    List<PriorityQueue> _addQueue = new List<PriorityQueue>();             // 更新処理の追加用キュー
    List<PriorityQueue> _delQueue = new List<PriorityQueue>();             // 更新処理の削除用キュー

    public void SetAttachment(TaskManagerAttachment attach)
    {
        _attach = attach;
        _attach.SetUpdateCallback(Update);
    }

    public void Register(UpdateCall call, int priority = 0)
    {
        _addQueue.Add(new PriorityQueue(priority, call));
    }

    public void Register(TaskBase task, int priority = 0)
    {
        _addQueue.Add(new PriorityQueue(priority, task));
    }

    public void Register(ITaskBase inf, int priority = 0)
    {
        _addQueue.Add(new PriorityQueue(priority, inf));
    }

    void Update()
    {
        foreach (var t in _tasks)
        {
            int ret = t.Update();
            if(ret == 1)
            {
                _delQueue.Add(t);
            }
        }

        //追加する更新処理があれば追加する
        if (_delQueue.Count > 0)
        {
            foreach (var q in _delQueue)
            {
                _tasks.Remove(q);
            }
            _delQueue.Clear();
        }
        if (_addQueue.Count > 0)
        {
            //優先度に応じて追加
            foreach (var q in _addQueue)
            {
                var node = _tasks.LastOrDefault(u => u.Priority <= q.Priority);
                if (node == null)
                {
                    _tasks.AddFirst(q);
                }
                else
                {
                    _tasks.AddAfter(_tasks.Find(node), q);
                }
            }
            _addQueue.Clear();
        }
    }
}
