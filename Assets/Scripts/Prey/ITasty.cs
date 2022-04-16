using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ITasty
{
    /// <summary>
    /// Whether the ITasty is currently tasty
    /// </summary>
    /// <returns></returns>
    bool IsTasty();

    Vector3 GetPosition();
}

