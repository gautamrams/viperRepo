#pragma once
#include "afxcmn.h"

#include<list>
#include "afxwin.h"
using namespace std;

// PropertiesForm dialog

class PropertiesForm : public CDialog
{
	DECLARE_DYNAMIC(PropertiesForm)

public:

	CString userName;
	CString fullName;
	CString descritpion;
	CString machineName;
	CString user;
	CString pass;
	bool defaultuser;
	bool canUpdate;
	list<CString> GroupList;

	PropertiesForm(CWnd* pParent = NULL);   // standard constructor
	PropertiesForm(CString str1,CString str2,CString str3,CString str4,CString user,CString pass,bool defaultuser);
	virtual ~PropertiesForm();
	void FormLoad();
	void GroupMembershipLoad();
	void LoadGroups();
	void FindMembership();
	void TabOneShow();
	void TabOneHide();
	void TabTwoShow();
	void TabTwoHide();
	bool RemoveMembership();
	void AddMembership(CString groupName);

// Dialog Data
	enum { IDD = IDD_PROPERTIES_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();
	

	DECLARE_MESSAGE_MAP()
public:
	CTabCtrl c_TabControl;
	void OnOK();
	afx_msg void OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnEnChangeFullnametextbox();
	afx_msg void OnEnChangeDescriptiontextbox();
	afx_msg void OnCbnSelchangeCombo1();
	afx_msg void OnBnClickedRadio1();
	afx_msg void OnBnClickedRadio2();
	afx_msg void OnBnClickedRadio3();
	CComboBox c_GroupCombo;
	CButton c_Radio1;
	afx_msg void OnBnClickedClose2button();
	afx_msg void OnBnClickedClose1button();
	afx_msg void OnBnClickedApply1button();
	afx_msg void OnBnClickedApply2button();
	CEdit c_FullNameTextBox;
	CEdit c_DescriptionTextBox;
};
