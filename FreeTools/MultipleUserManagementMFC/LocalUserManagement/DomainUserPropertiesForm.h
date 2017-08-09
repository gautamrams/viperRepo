#pragma once
#include "afxwin.h"

#include<list>
using namespace std;

// DomainUserPropertiesForm dialog

class DomainUserPropertiesForm : public CDialog
{
	DECLARE_DYNAMIC(DomainUserPropertiesForm)

public:
	CString userName;
	CString machineName;
	bool defaultuser;
	CString user;
	CString pass;
	bool canUpdate;
	list<CString> GroupList;

	bool RemoveMembership();
	void AddMembership(CString groupName);
	void GroupMembershipLoad();
	void LoadGroups();
	void FindMembership();
	DomainUserPropertiesForm(CWnd* pParent = NULL);   // standard constructor
	DomainUserPropertiesForm(CString str1,CString str2,CString user,CString pass,bool defaultuser);
	void FormLoad();
	
	virtual ~DomainUserPropertiesForm();

// Dialog Data
	enum { IDD = IDD_DOMAINUSERPROPERTIES_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
public:
	void OnOK();
	afx_msg void OnBnClickedApply2button();
	afx_msg void OnBnClickedClose2button();
	CComboBox c_GroupCombo;
	afx_msg void OnBnClickedRadio1();
	afx_msg void OnBnClickedRadio2();
	afx_msg void OnBnClickedRadio3();
	afx_msg void OnCbnSelchangeCombo1();
};
