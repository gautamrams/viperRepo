// Sampledlg.cpp : implementation file
//

#include "stdafx.h"
#include "NewText.h"
#include "Sampledlg.h"
//#include ".\sampledlg.h"


// CSampledlg dialog

IMPLEMENT_DYNAMIC(CSampledlg, CDialog)
CSampledlg::CSampledlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSampledlg::IDD, pParent)
	, m_samedit3(_T(""))
{
}

CSampledlg::~CSampledlg()
{
}

void CSampledlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT1, m_samedit3);
}


BEGIN_MESSAGE_MAP(CSampledlg, CDialog)
	ON_BN_CLICKED(IDCANCEL, OnBnClickedCancel)
	ON_BN_CLICKED(IDOK, OnBnClickedOk)
END_MESSAGE_MAP()


// CSampledlg message handlers

void CSampledlg::OnBnClickedCancel()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}

void CSampledlg::OnBnClickedOk()
{
	// TODO: Add your control notification handler code here
	OnOK();
}
