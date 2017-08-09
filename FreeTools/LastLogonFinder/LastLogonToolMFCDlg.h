// LastLogonToolMFCDlg.h : header file
//

#pragma once
#include "afxcmn.h"
#include "comdef.h"
#include "afxwin.h"
//#include "label.h"
#include "Activeds.h"
#include <Iads.h>
#include "DomainControllerLists.h"
#include "lastlogonreport.h"
#include "atlbase.h"
#include "Lm.h"
#include "Dsgetdc.h"
#include <wchar.h>
#include "string.h"
#include <Adshlp.h>// CLastLogonToolMFCDlg dialog
#include "label.h"
#include "hyperlink.h"
class CLastLogonToolMFCDlg : public CDialog
{
// Construction
public:
	CLastLogonToolMFCDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_LASTLOGONTOOLMFC_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	HICON m_hIcon;
	CBitmapButton button1,button2,button3,button4,button5;
	void InsertColumns(int index);
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	void SetCount();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()

public:
	CListCtrl v_SearchUser;
	CEdit l_userName;
	CComboBox l_domainNames;
	afx_msg void OnBnClickedGetlastlogondetails();
	CLabel c_P1Label1;
		CLabel c_P1Label2;
	CLabel c_P3Label3;
	afx_msg void OnBnClickedSearch();
	CLabel c_P1Label6;
	CLabel c_P1Label5;
	CLabel c_P1Label4;
	CLabel c_P1DomainName;
	CLabel c_P1UserName;
	afx_msg void OnBnClickedSelectall();
	afx_msg void OnBnClickedDeselectall();
	CHyperLink c_link;
	CLabel c_P1LabelLink;
	afx_msg void OnBnClickedHome();
	CStatic c_searchResult;
};
