#ifndef CreatorSource_H_INCLUDED
#define CreatorSource_H_INCLUDED
#include<stdio.h>
#include<iostream>
#include<conio.h>
#include<string>
#include<WinSock2.h>
#include<Windows.h>
#include<winhttp.h>
#include <tchar.h>
#include<time.h>
#include<sstream>
#include<fstream>

#pragma comment(lib,"winhttp.lib")
#pragma comment(lib, "Ws2_32.lib")
int getInfo();
void  writeToFile(int num);
int  getFileDate();
int  startApp();
void  addDb();
void  findId();
void  updateDb();
#endif