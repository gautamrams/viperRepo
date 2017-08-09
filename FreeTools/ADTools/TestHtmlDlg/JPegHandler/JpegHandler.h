#pragma once

#ifndef HIMETRIC_INCH   
	#define HIMETRIC_INCH   2540    // HIMETRIC units per inch
#endif

#include <atlbase.h>

// You may derive a class from CComModule and use it if you want
// to override something, but do not change the name of _Module

//extern CComModule _Module;

#include <atlcom.h>
#include <atlctl.h>


class CJpegHadler
{
public:
	CJpegHadler(void);
	virtual ~CJpegHadler(void);

	BOOL	InitJpegHandler(HWND TargetHandle,HDC hdc);

	BOOL	LoadImageFile(HWND HandleToTarget,CString FileName);
	BOOL	LoadImageFile(CString FileName);
	BOOL	LoadImageFile(void *Buffer,int BufferLen);

	BOOL	Render();
	BOOL	Render(CRect rt,bool bTop=true);

	BOOL	SetSize(int Width,int Height);
	BOOL	GetSize(int &Width,int &Height);

	BOOL	UnLoad();

	BOOL	IsLoaded();


private:

	HDC				m_dc;
	HWND			m_TargethWnd;
	long			m_Width,m_Height;
	CRect			m_rt;
	//CImage			m_Image;
	LPPICTURE		m_Picture;
	void			Free();
};
