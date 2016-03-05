﻿using UnityEngine;
using System.Collections;

public class Tuple<T1, T2>
{
    public T1 First { get; private set; }
    public T2 Second { get; private set; }
    internal Tuple(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}
public class Tuple<T1, T2, T3, T4>
{
    public T1 First { get; private set; }
    public T2 Second { get; private set; }
    public T3 Third { get; private set; }
    public T4 Fourth { get; private set; }
    internal Tuple(T1 first, T2 second, T3 third, T4 fourth)
    {
        First = first;
        Second = second;
        Third = third;
        Fourth = fourth;
    }
}

public static class Tuple
{
    public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
    {
        var tuple = new Tuple<T1, T2>(first, second);
        return tuple;
    }
    public static Tuple<T1, T2, T3, T4> New<T1, T2, T3, T4>(T1 first, T2 second, T3 third, T4 fourth)
    {
        var tuple = new Tuple<T1, T2, T3, T4>(first, second, third, fourth);
        return tuple;
    }
}