using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


interface ITaskBase
{
    int Update();
}

class TaskBase : ITaskBase
{
    public int Update()
    {
        return 0;
    }
}