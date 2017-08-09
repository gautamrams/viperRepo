// ListDlg.cpp : implementation file
//

#include "stdafx.h"
#include "NewText.h"
#include "ListDlg.h"


// CListDlg dialog

IMPLEMENT_DYNAMIC(CListDlg, CDialog)
CListDlg::CListDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CListDlg::IDD, pParent)
{
}

CListDlg::~CListDlg()
{
}

void CListDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CListDlg, CDialog)
END_MESSAGE_MAP()


// CListDlg message handlers
