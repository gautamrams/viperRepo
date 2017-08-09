#pragma once
#include "afxwin.h"
#include "afxbutton.h"


// dialog_pwdpolicy dialog

class dialog_pwdpolicy : public CDialog
{
	DECLARE_DYNAMIC(dialog_pwdpolicy)

public:
	dialog_pwdpolicy(CWnd* pParent = NULL);   // standard constructor
	virtual ~dialog_pwdpolicy();

// Dialog Data
	enum { IDD = IDD_DIALOG_PWDPOLICY };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	LPCWSTR user_name_ppm, password_ppm, dom_name_ppm;
public:
	CString hist;
public:
	CString max;
public:
	CString min;
public:
	CString len;
public:
	afx_msg void OnEnChangeHist();
public:
	afx_msg void OnEnChangeMax();
public:
	afx_msg void OnEnChangeMin();
public:
	afx_msg void OnEnChangeLen();
public:
	CButton rad1;
public:
	CButton rad2;
public:
	CButton rad3;
public:
	CButton rad4;
	CMFCButton m_generate;
	CMFCButton m_save;
	CMFCButton m_default;
	CMFCButton m_logout;

public:
	afx_msg void OnBnClickedGenerate();
public:
	afx_msg void OnBnClickedSave();
public:
	afx_msg void OnBnClickedDefault();
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
};
