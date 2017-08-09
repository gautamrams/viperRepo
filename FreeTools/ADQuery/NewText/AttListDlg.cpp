// AttListDlg.cpp : implementation file

#include "stdafx.h"
#include "NewText.h"
#include "AttListDlg.h"

//#include "NewTextDlg.h"

int cnt;
// CAttListDlg dialog

BOOL CAttListDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	m_listleft.AddString("Name");
	m_listleft.AddString("First Name");
	m_listleft.AddString("Initial");
	m_listleft.AddString("Last Name");
	m_listleft.AddString("Display Name");
	m_listleft.AddString("Description");
	m_listleft.AddString("Office");
	m_listleft.AddString("Telephone Number");
	m_listleft.AddString("E-Mail ID");
	m_listleft.AddString("Website Address");
	m_listleft.AddString("Address");
	m_listleft.AddString("Post Box Number");
	m_listleft.AddString("City");
	m_listleft.AddString("State/Province");
	m_listleft.AddString("Zip Code");
	m_listleft.AddString("Country");
	m_listleft.AddString("Logan Name");
//	DWORD dwCount=sizeof(rgpwszAttribute)/sizeof(LPWSTR);
/*	for(int m=0;m<dwCount;m++)
	{
		this->m_listleft.AddString(att[m]);
	}
*/
	m_listright.AddString("Department");
	m_listright.AddString("Middle Name");
	m_listright.AddString("Sam Account Name");
	m_listright.AddString("Job Title");
	m_listright.AddString("Company");
	m_listright.AddString("Division");
	
	return TRUE;
}

IMPLEMENT_DYNAMIC(CAttListDlg, CDialog)
CAttListDlg::CAttListDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CAttListDlg::IDD, pParent)
	
	, ceditsam2(_T(""))
{
}

CAttListDlg::~CAttListDlg()
{
}

void CAttListDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_LEFT, m_listleft);
	DDX_Control(pDX, IDC_LIST_RIGHT, m_listright);

	DDX_Text(pDX, IDC_EDIT1, ceditsam2);
}


BEGIN_MESSAGE_MAP(CAttListDlg, CDialog)
	
	
	ON_BN_CLICKED(IDD_BTN_LA, OnBnClickedBtnLa)
	ON_BN_CLICKED(IDD_BTN_OK, OnBnClickedBtnOk)
	ON_LBN_SELCHANGE(IDC_LIST_RIGHT, OnLbnSelchangeListRight)
	ON_BN_CLICKED(IDD_BTN_RO, OnBnClickedBtnRo)
	ON_LBN_SELCHANGE(IDC_LIST_LEFT, OnLbnSelchangeListLeft)
	ON_BN_CLICKED(IDCANCEL, OnBnClickedCancel)
	ON_BN_CLICKED(IDD_BTN_LO, OnBnClickedBtnLo)
	ON_BN_CLICKED(IDD_BTN_RA, OnBnClickedBtnRa)
	//ON_EN_CHANGE(IDC_EDIT1, OnEnChangeEdit1)
END_MESSAGE_MAP()


// CAttListDlg message handlers





void CAttListDlg::OnBnClickedBtnLa()
{
	// TODO: Add your control notification handler code here
	CString s,a;
	int l,i;
	l=m_listright.GetCount();
	s.Format(_T("%d"),l);
	

	//MessageBox(s);
	for(i=l-1;i>=0;i--)
	{
		//m_listright.SetCurSel(i);
		m_listright.GetText(i,a);
		
		UpdateData();
		m_listleft.AddString(a);
		UpdateData(FALSE);
		
	}
	m_listright.ResetContent();

	UpdateData(FALSE);
}

void CAttListDlg::OnBnClickedBtnOk()
{
	// TODO: Add your control notification handler code here
	OnOK();
}

void CAttListDlg::OnLbnSelchangeListRight()
{
	// TODO: Add your control notification handler code here
}

void CAttListDlg::OnBnClickedBtnRo()
{
	// TODO: Add your control notification handler code here
	CString aaa;
	m_listleft.GetText(m_listleft.GetCurSel(),aaa);
	m_listleft.DeleteString(m_listleft.GetCurSel());
    //MessageBox(aaa);
	UpdateData(FALSE);
	UpdateData();
	m_listright.AddString(aaa);
	UpdateData(FALSE);
}

void CAttListDlg::OnLbnSelchangeListLeft()
{
	// TODO: Add your control notification handler code here
	
}

void CAttListDlg::OnBnClickedCancel()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}

void CAttListDlg::OnBnClickedBtnLo()
{
	// TODO: Add your control notification handler code here
	CString aa;
	m_listright.GetText(m_listright.GetCurSel(),aa);
	m_listright.DeleteString(m_listright.GetCurSel());
    //MessageBox(aa);
	UpdateData(FALSE);
	UpdateData();
	m_listleft.AddString(aa);
	UpdateData(FALSE);
}

void CAttListDlg::OnBnClickedBtnRa()
{
	// TODO: Add your control notification handler code here
	CString s,a;
	int l,i;
	l=m_listleft.GetCount();
	s.Format(_T("%d"),l);
	

	//MessageBox(s);
	for(i=l-1;i>=0;i--)
	{
		//m_listright.SetCurSel(i);
		m_listleft.GetText(i,a);
		
		UpdateData();
		m_listright.AddString(a);
		UpdateData(FALSE);
		
	}
	m_listleft.ResetContent();

	UpdateData(FALSE);
}