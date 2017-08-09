// JpegFrame.cpp : implementation file
//

#include "stdafx.h"
#include "JpegFrame.h"


// CJpegFrame

IMPLEMENT_DYNAMIC(CJpegFrame, CStatic)

CJpegFrame::CJpegFrame()
{
m_bTop = true;
}

CJpegFrame::~CJpegFrame()
{
}
BOOL CJpegFrame::Init(CString FileName)
{
	m_ImgDC = ::GetDC(m_hWnd);
	m_Jpeg.InitJpegHandler(m_hWnd,m_ImgDC);
	
	
	m_Jpeg.LoadImageFile(FileName);
	
	GetClientRect(&m_rt);
	//	m_Jpeg.Render(m_rt);
	//InvalidateRect(NULL);
	return TRUE;
}



BEGIN_MESSAGE_MAP(CJpegFrame, CStatic)
	ON_WM_PAINT()
END_MESSAGE_MAP()



// CJpegFrame message handlers



void CJpegFrame::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	GetClientRect(&m_rt);
	m_Jpeg.Render(m_rt,m_bTop);
}
