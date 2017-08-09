#pragma once
#include "afxcmn.h"
#include "comdef.h"
#include "afxwin.h"
#include "lastlogonreport.h"
#include "label.h"
#include "hyperlink.h"

// DomainControllerLists dialog

class DomainControllerLists : public CDialog
{
	DECLARE_DYNAMIC(DomainControllerLists)

public:
	DomainControllerLists(CWnd* pParent = NULL);   // standard constructor
	virtual ~DomainControllerLists();
	void InsertDCColumns(int index);
	enum { IDD = IDD_DIALOG1 };

protected:
	CBitmapButton button1;
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();
	void ListDomainControllers();
	DECLARE_MESSAGE_MAP()
public:
	CListCtrl v_dclist;
	afx_msg void OnBnClickedRadio1();
	void setDomainName(CString);
	void CloseDialog();
	int r_lastlogon1;
	int v_lastlogonTimestamp;
	afx_msg void OnBnClickedRadio2();
	CButton r1_timestamp;
	CButton r_new;
	int r_new2;
	int r_radiogroup;
	afx_msg void OnBnClickedRadio9();
	afx_msg void OnBnClickedRadio8();
	afx_msg void OnBnClickedGeneratereport();
	CLabel c_p2Label1;
	CLabel c_p2Label2;
	afx_msg void OnBnClickedButton1();
	afx_msg void OnBnClickedButton2();
	afx_msg void OnLvnItemchangedList1(NMHDR *pNMHDR, LRESULT *pResult);
	CHyperLink c_p2Link2;
};
