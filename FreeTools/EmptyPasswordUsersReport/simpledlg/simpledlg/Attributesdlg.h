#pragma once
#include "afxcmn.h"
#include "afxwin.h"
#include "afxbutton.h"

// CAttributesdlg dialog

class CAttributesdlg : public CDialog
{
	DECLARE_DYNAMIC(CAttributesdlg)

public:
	CAttributesdlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CAttributesdlg();

// Dialog Data
	enum { IDD = IDD_DIALOG3 };

protected:
	virtual BOOL OnInitDialog();
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButton1();
	CListCtrl c_AttributeList;
	CComboBox c_Attributetype;	
	afx_msg void OnBnClickedOk();	
	CButton c_ConfirmChkBox;
	CMFCButton m_button1;
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
};
