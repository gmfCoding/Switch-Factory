using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProvider<T>
{
    public T Get();
}
