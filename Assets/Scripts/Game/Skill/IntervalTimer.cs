using UnityEngine;

public class IntervalTimer
{
    float _interval = 1;
    float _timer = 0;

    public void Setup(float inv)
    {
        _interval = inv;
    }

    public bool RunTimer()
    {
        _timer += Time.deltaTime;
        if(_timer >= _interval)
        {
            _timer -= _interval;
            return true;
        }
        return false;
    }
}
