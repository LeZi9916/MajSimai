﻿#if NET7_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MajSimai.Unmanaged;
[StructLayout(LayoutKind.Sequential)]
internal unsafe struct UnmanagedSimaiChart
{
    public int levelLen;
    public int designerLen;
    public int fumenLen;
    public int noteTimingsLen;
    public int commaTimingsLen;
    public bool isEmpty;

    public char* level;
    public char* designer;
    public char* fumen;
    public UnmanagedSimaiTimingPoint* noteTimings;
    public UnmanagedSimaiTimingPoint* commaTimings;
    

    public readonly static UnmanagedSimaiChart Empty = new()
    {
        level = null,
        levelLen = 0,
        designer = null,
        designerLen = 0,
        fumen = null,
        fumenLen = 0,
        isEmpty = true,
        noteTimings = null,
        noteTimingsLen = 0,
        commaTimings = null,
        commaTimingsLen = 0,
    };
}
#endif