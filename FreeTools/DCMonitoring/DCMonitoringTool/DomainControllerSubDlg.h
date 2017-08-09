#pragma once
#include "afxcmn.h"
#include "afxbutton.h"
// CDomainControllerSubDlg dialog

class CDomainControllerSubDlg : public CDialog
{
	DECLARE_DYNAMIC(CDomainControllerSubDlg)

public:
	CDomainControllerSubDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CDomainControllerSubDlg();

// Dialog Data
	enum { IDD = IDD_DIALOG1 };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();	

	DECLARE_MESSAGE_MAP()
public:
	CListCtrl C_DomainControllerList;
	CMFCButton m_ok;
	afx_msg void OnBnClickedOk();
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
};
