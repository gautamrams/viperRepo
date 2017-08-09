// DCMonitoringToolDlg.h : header file
//

#pragma once
#include "afxcmn.h"
#include "afxwin.h"
#include "hyperlink.h"
#include "afxbutton.h"

// CDCMonitoringToolDlg dialog
class CDCMonitoringToolDlg : public CDialog
{
// Construction
public:
	CDCMonitoringToolDlg(CWnd* pParent = NULL);	// standard constructor
	CBitmapButton btnHome;
// Dialog Data
	enum { IDD = IDD_DCMONITORINGTOOL_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	void GetAllDatas();	
	void Impersonate();
	BOOL IsValidDatas();
	BOOL CheckEmpty(CString Credentials);
	void InsertColumn(LPSTR ParameterName);
	void SetItem(LPSTR SetDCName,double ParameterValue);

	void GetCPUUsage(LPSTR SetDCName);
	void GetMemoryUtilization(LPSTR SetDCName);
	void GetDiskUtilization(LPSTR SetDCName);
	void GetPageReadsPerSec(LPSTR SetDCName);
	void GetPageWritesPerSec(LPSTR SetDCName);
	void GetHandleCountLSASS(LPSTR SetDCName);
	void GetHandleCountNTFRS(LPSTR SetDCName);
	void GetFileReadsPerSec(LPSTR SetDCName);
	void GetFileWritesPerSec(LPSTR SetDCName);
	void GetInterruptTime(LPSTR SetDCName);
	void GetPageFaultsPerSec(LPSTR SetDCName);
	void GetPagesInputPerSec(LPSTR SetDCName);
	void GetPagesOutputPerSec(LPSTR SetDCName);	

	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:				
	CTabCtrl C_ParameterTab;
	afx_msg void OnBnClickedButton1();
	CMFCButton C_SelectDCBtn;
	CButton C_Home;
	CString V_DomainNameEdit;	
	CEdit C_DomainNameEdit;
	CEdit C_UserNameEdit;
	CBitmap bitmap_home;
	CString V_UserNameEdit;
	CEdit C_PasswordEdit;
	CString V_PasswordEdit;	
	afx_msg void OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnTimer(UINT nIDEvent);		
	CListCtrl C_ParametersListCtrl;
	CHyperLink C_Link;
public:
	afx_msg void OnBnClickedButton2();
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
};

