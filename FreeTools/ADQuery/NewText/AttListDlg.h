#pragma once
#include "afxwin.h"
#include "stdafx.h"


// CAttListDlg dialog

class CAttListDlg : public CDialog
{
	DECLARE_DYNAMIC(CAttListDlg)

public:
	CAttListDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CAttListDlg();

// Dialog Data
	enum { IDD = IDD_DIALOG1 };

protected:
	virtual BOOL OnInitDialog();
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedBtnCancel();
	
	CListBox m_listleft;
	CListBox m_listright;
	afx_msg void OnBnClickedBtnLa();
	afx_msg void OnBnClickedBtnOk();
	afx_msg void OnLbnSelchangeListRight();
	afx_msg void OnBnClickedBtnRo();
	afx_msg void OnLbnSelchangeListLeft();
	afx_msg void OnBnClickedCancel();
	afx_msg void OnBnClickedBtnLo();
	afx_msg void OnBnClickedBtnRa();
	CString ceditsam2;
	//afx_msg void OnEnChangeEdit1();
};
