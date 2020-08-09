using System;

public delegate void Callback();
public delegate void Callback<T>(T arg);
public delegate void Callback<T1, T2>(T1 arg1, T2 arg2);
public delegate void Callback<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
