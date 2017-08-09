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
END_MESSAGE_MAP()


// CNewReadMe message handlers
