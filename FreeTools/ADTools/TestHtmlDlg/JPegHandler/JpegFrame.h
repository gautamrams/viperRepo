#pragma once
#include "JpegHandler.h"

// CJpegFrame

class CJpegFrame : public CStatic
{
	DECLARE_DYNAMIC(CJpegFrame)

public:
	CJpegFrame();
	virtual ~CJpegFrame();

	CJpegHadler			m_Jpeg;
	HDC					m_ImgDC ;
	HWND				m_TargethWnd;
	CRect				m_rt;
	bool				m_bTop;

	BOOL				Init(CString FileName);


protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnPaint();
};


