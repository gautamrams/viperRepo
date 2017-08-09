// Password_Policy_ManagerDlg.h : header file
//

#pragma once
#include "afxwin.h"
#include "hyperlink.h"
#include "afxbutton.h"

// CPassword_Policy_ManagerDlg dialog
class CPassword_Policy_ManagerDlg : public CDialog
{
// Construction
public:
	CPassword_Policy_ManagerDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_PASSWORD_POLICY_MANAGER_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CBitmap bitmap_home;
public:
	CString uname;
public:
	CString pwd;
public:
	afx_msg void OnEnChangeUname();
public:
	afx_msg void OnEnChangePwd();
public:
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
public:
	afx_msg void OnBnClickedHome();
public:
	afx_msg void OnBnClickedLogin();
	LPCWSTR domm_new;
public:
	CButton button_home;
	CMFCButton m_button2;
	CMFCButton m_button1;
public:
	CHyperLink link;
public:
	CComboBox l_domainNames;
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
};
