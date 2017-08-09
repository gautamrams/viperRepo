#pragma once
#include "afxcmn.h"

#include "list"
#include "label.h"
using namespace std;

// AddForm dialog

class AddForm : public CDialog
{
	DECLARE_DYNAMIC(AddForm)

public:
	CString user;
	CString pass;
	bool defaultuser;
	bool canUpdate;
	list<CString> computerList;

	AddForm(CWnd* pParent = NULL);   // standard constructor
	AddForm(CString user,CString pass,bool defaultuser,list<CString> compList);
	void FormLoad();
	bool isValidFile(CString fileName);
	void AddUsers();
	virtual ~AddForm();

// Dialog Data
	enum { IDD = IDD_ADD_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
public:
	CListCtrl c_UserListView;
	CListCtrl c_ComputerListView;
	CListCtrl c_GroupListView;
	afx_msg void OnBnClickedAdduserbutton();
	afx_msg void OnBnClickedCsvbutton();
	afx_msg void OnBnClickedClosebutton();
	afx_msg void OnBnClickedCreatebutton();
	void OnOK();
public:
	CLabel c_TitleLabel;
};
