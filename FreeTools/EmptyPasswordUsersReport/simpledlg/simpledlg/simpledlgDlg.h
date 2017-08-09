// simpledlgDlg.h : header file
//

#pragma once
#include "afxcmn.h"
#include "afxwin.h"
#include "hyperlink.h"
#include "label.h"
#include "afxext.h"
#include "afxbutton.h"
#include <Winuser.h>
// CsimpledlgDlg dialog
class CsimpledlgDlg : public CDialog
{
// Construction
public:
	CsimpledlgDlg(CWnd* pParent = NULL);	// standard constructor
	CBitmapButton btnHome;
// Dialog Data
	enum { IDD = IDD_SIMPLEDLG_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	BOOL IsEmpty(CString ctrl);
	BOOL IsValidData(LPWSTR DomainName,LPWSTR UserName, LPWSTR Password);
	void GetAllDatas();
	void InitializeAllDatas();
	void InsertColumns(int index);
	void SetCount();
	void SetItem(int i, BSTR str);
	void AddComboContenet();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CListCtrl v_UserAttributeList;
	afx_msg void OnBnClickedButton2();
	afx_msg void OnBnClickedButton1();	
	CString v_UserName;
	CString v_Domain;
	CString v_Password;
	CEdit c_Domain;
	CEdit c_UserName;
	CEdit c_Password;	
	CString v_Count;
	CStatic c_count;
	CMFCButton m_advanced;
	CMFCButton m_button2;
	CButton m_button3;
	CBitmap bitmap_home;
	afx_msg void OnBnClickedButton3();	
	CComboBox c_Continer;
	CComboBox c_Container;
	afx_msg void OnCbnDropdownCombo1();
	afx_msg void OnStnClickedBitmap();	
	afx_msg void OnBnClickedReadme();
	afx_msg void OnBnClickedsam();	
	afx_msg void OnStnClickedCsvexport();
	afx_msg void OnStnClickedReadmeimg();
	afx_msg void OnBnClickedAdvanced();		
	CHyperLink c_link;				
	afx_msg void OnCbnEditchangeCombo1();
public:
	CLabel c_Label1;
public:
	CLabel c_Label2;
public:
	CLabel c_Label3;
public:
	CLabel c_Label4;
public:
	afx_msg void backClicked();
	afx_msg void OnCbnSelchangeCombo1();
	afx_msg void OnLvnItemchangedList1(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnEnChangeEdit2();
	afx_msg void OnEnChangeEdit3();
};

