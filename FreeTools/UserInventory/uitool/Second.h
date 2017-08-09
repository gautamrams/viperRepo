#pragma once
#include "afxwin.h"


// CSecond dialog
//const CString useratt[]={"givenName","sn","DisplayName","Description"};

class CSecond : public CDialog
{
	DECLARE_DYNAMIC(CSecond)
	//unsigned int sizeuseratt;

public:
	CSecond(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSecond();

// Dialog Data
	enum { IDD = IDD_DIALOG1 };

protected:
	
	virtual void DoDataExchange(CDataExchange* pDX);  // DDX/DDV support
	

	DECLARE_MESSAGE_MAP()
public:
	CListBox lbox1;
	CListBox lbox2;
	afx_msg void OnLbnSelchangeList1();
	afx_msg void OnBnClickedButton1();
};
