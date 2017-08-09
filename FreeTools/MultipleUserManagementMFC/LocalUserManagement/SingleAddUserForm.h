#pragma once
#include "label.h"


// SingleAddUserForm dialog

class SingleAddUserForm : public CDialog
{
	DECLARE_DYNAMIC(SingleAddUserForm)

public:
	CString username;
	CString fullname;
	CString desc;
	CString password;
	bool canAdd;

	void FormLoad();
	SingleAddUserForm(CWnd* pParent = NULL);   // standard constructor
	virtual ~SingleAddUserForm();

// Dialog Data
	enum { IDD = IDD_SINGLEADDUSER_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedAddbutton();
	void OnOK();
public:
	afx_msg void OnBnClickedClearbutton();
public:
	CLabel c_TitleLabel;
};
