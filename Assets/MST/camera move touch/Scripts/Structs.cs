using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MinMax
{
    public MinMax(int min,int max)
    {
        Min = min;
        Max = max;
    }

    public int Min;
    public int Max;

    public bool Between(int num)
    {
        return (num >= Min && num <= Max);
    }
}