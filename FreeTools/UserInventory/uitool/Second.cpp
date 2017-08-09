// Second.cpp : implementation file
//

#include "stdafx.h"
#include "uitool.h"
#include "Second.h"
#include ".\second.h"


// CSecond dialog

IMPLEMENT_DYNAMIC(CSecond, CDialog)
CSecond::CSecond(CWnd* pParent /*=NULL*/)
	: CDialog(CSecond::IDD, pParent)
{
}

CSecond::~CSecond()
{
}

void CSecond::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST1, lbox1);
	DDX_Control(pDX, IDC_LIST2, lbox2);
}


BEGIN_MESSAGE_MAP(CSecond, CDialog)
	ON_LBN_SELCHANGE(IDC_LIST1, OnLbnSelchangeList1)
	ON_BN_CLICKED(IDC_BUTTON1, OnBnClickedButton1)
END_MESSAGE_MAP()


// CSecond message handlers

void CSecond::OnLbnSelchangeList1()
{
	// TODO: Add your control notification handler code here
}

void CSecond::OnBnClickedButton1()
{
	// TODO: Add your control notification handler code here
}
