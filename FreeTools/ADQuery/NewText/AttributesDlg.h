#pragma once
#include "afxwin.h"


// CAttributesDlg dialog

class CAttributesDlg : public CDialog
{
	DECLARE_DYNAMIC(CAttributesDlg)

public:
	CAttributesDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CAttributesDlg();

// Dialog Data
	enum { IDD = IDD_DIALOG1 };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();
	DECLARE_MESSAGE_MAP()
public:
	CListBox c_leftAttList;
	CListBox c_rightAttList;
	afx_msg void OnBnClickedButton1();
	afx_msg void OnBnClickedButton2();
	afx_msg void OnBnClickedButton3();
	afx_msg void OnBnClickedButton4();
	afx_msg void OnBnClickedOk();
	CButton c_ConfirmChkBox;
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
};
