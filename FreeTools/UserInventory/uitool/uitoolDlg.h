// uitoolDlg.h : header file
//

#pragma once
#include "afxcmn.h"
#include "afxwin.h"
#include "hyperlink.h"
#include "afxext.h"
#include "afxbutton.h"
// CuitoolDlg dialog
class CuitoolDlg : public CDialog
{
// Construction
public:
	CuitoolDlg(CWnd* pParent = NULL);	// standard constructor
	CBitmapButton btnHome;
// Dialog Data
	enum { IDD = IDD_UITOOL_DIALOG };

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
	CListCtrl list2;
	afx_msg void OnBnClickedButton1();
	CString filepath;	
	CComboBox cbox;	
	afx_msg void OnBnClickedButton2();
	afx_msg void OnBnClickedExport();
	afx_msg void OnBnClickedButton3();	
	afx_msg void OnBnClickedAdvance();	
	CStatic m_nm;
	CBitmap home_bitmap;
	CHyperLink c_link1;
	afx_msg void backClicked();
	CMFCButton CSVDeButton;
	CMFCButton CSVButton;
	CMFCButton Generatebutton;
	CMFCButton Exportbutton;
	CMFCButton Advancedbutton;
	CMFCButton Filebutton;
	CButton home_button;
	afx_msg void OnBnClickedButton4();
//	afx_msg void OnBnClickedButton2();
	
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
};
