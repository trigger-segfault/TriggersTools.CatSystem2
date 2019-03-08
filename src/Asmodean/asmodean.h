#pragma once

#ifndef ASMODEAN_DLL_H
#define ASMODEAN_DLL_H

#define ASMODEAN_API extern "C" __declspec(dllexport)

typedef unsigned char        byte;
typedef   signed short      int16;
typedef unsigned short     uint16;
typedef   signed long       int32;
typedef unsigned long      uint32;
typedef   signed long long  int64;
typedef unsigned long long uint64;

#endif /* ASMODEAN_DLL */