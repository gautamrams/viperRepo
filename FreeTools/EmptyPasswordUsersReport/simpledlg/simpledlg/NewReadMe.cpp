// NewReadMe.cpp : implementation file
//

#include "stdafx.h"
#include "simpledlg.h"
#include "NewReadMe.h"


// CNewReadMe dialog

IMPLEMENT_DYNAMIC(CNewReadMe, CDialog)
CNewReadMe::CNewReadMe(CWnd* pParent /*=NULL*/)
	: CDialog(CNewReadMe::IDD, pParent)
{
}

CNewReadMe::~CNewReadMe()
{
}

void CNewReadMe::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);		
}


BEGIN_MESSAGE_MAP(CNewReadMe, CDialog)
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()


// CNewReadMe message handlers


BOOL CNewReadMe::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

         CRect r;
         pDC->GetClipBox(&r);
		 pDC->FillSolidRect(r, RGB(244,244,244)); 
         return TRUE;
}
