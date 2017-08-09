// NewTextDlg.h : header file
//

#pragma once
#include "afxwin.h"
#include "afxcmn.h"
#include "hyperlink.h"
#include "afxext.h"
#include "afxbutton.h"
// CNewTextDlg dialog
class CNewTextDlg : public CDialog
{
// Construction
public:
	CNewTextDlg(CWnd* pParent = NULL);	// standard constructor
	CBitmapButton btnHome;
// Dialog Data
	enum { IDD = IDD_NEWTEXT_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	BOOL CNewTextDlg::IsEmpty(CString ctrl);
	void CNewTextDlg::InsertColumn(unsigned int index);
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CEdit C_DomainName;
	CComboBox l_domainNames;
	CEdit C_Query;
	CString V_Query;
	CListCtrl C_ListAttributes;
	CHyperLink c_link;
	afx_msg void OnBnClickedBtnconfigure();
	afx_msg void OnBnClickedBtngenerate();
	CStatic C_Count;
	CMFCButton l_generate;
	CMFCButton l_advance;
	CButton l_button1;
	CBitmap home_bitmap;
public:
	afx_msg void backClicked();
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
};
