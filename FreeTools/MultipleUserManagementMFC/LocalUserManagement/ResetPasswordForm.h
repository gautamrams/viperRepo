#pragma once
#include "afxwin.h"
#include "IADs.h"
#include "ADshlp.h"



// ResetPasswordForm dialog

class ResetPasswordForm : public CDialog
{
	DECLARE_DYNAMIC(ResetPasswordForm)

public:
	CString userName;
	CString machineName;
	CString user;
	CString pass;
	bool defaultuser;
	CString retUserName;
	CString retPassword;
	bool canProceed;
	CString newPassword;
	CString confirmPassword;

	ResetPasswordForm(CWnd* pParent = NULL);   // standard constructor
	ResetPasswordForm(CString str1,CString str2,CString user,CString pass,bool defaultuser);
	ResetPasswordForm(int i);
	virtual ~ResetPasswordForm();

// Dialog Data
	enum { IDD = IDD_RESETPASSWORD_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedClosebutton();
	afx_msg void OnBnClickedOkbutton();
	void OnOK();
	CEdit c_NewPassword;
	CEdit c_ConfirmPassword;
};
