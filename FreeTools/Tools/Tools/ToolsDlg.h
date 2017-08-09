// ToolsDlg.h : header file
//

#pragma once
#include "HyperLink.h"
#include "label.h"

// CToolsDlg dialog
class CToolsDlg : public CDialog
{
// Construction
public:
	CToolsDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_TOOLS_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	virtual LPSTR CToolsDlg::getDir();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CHyperLink c_link1;
	CHyperLink c_link2;	
	CHyperLink c_link3;
	CHyperLink c_icon1;
	CHyperLink c_icon2;	
	CHyperLink c_icon3;		
	CHyperLink c_icon4;	
	CHyperLink c_Icon5;	
	afx_msg void OnStnClickedLink5();
	afx_msg void OnStnClickedSam();
	CLabel c_Label1;
public:
	CLabel c_label2;
public:
	afx_msg void OnStnClickedLabel2();
};
