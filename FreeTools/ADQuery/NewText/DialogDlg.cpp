// DialogDlg.cpp : implementation file
//

#include "stdafx.h"
#include "NewText.h"
#include "DialogDlg.h"


// CDialogDlg dialog

IMPLEMENT_DYNAMIC(CDialogDlg, CDialog)
CDialogDlg::CDialogDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CDialogDlg::IDD, pParent)
{
}

CDialogDlg::~CDialogDlg()
{
}

void CDialogDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CDialogDlg, CDialog)
END_MESSAGE_MAP()


// CDialogDlg message handlers
