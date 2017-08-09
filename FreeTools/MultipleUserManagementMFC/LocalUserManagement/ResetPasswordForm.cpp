// ResetPasswordForm.cpp : implementation file
//

#include "stdafx.h"
#include "LocalUserManagement.h"
#include "ResetPasswordForm.h"
#include "Adserr.h"





// ResetPasswordForm dialog

IMPLEMENT_DYNAMIC(ResetPasswordForm, CDialog)

ResetPasswordForm::ResetPasswordForm(CWnd* pParent /*=NULL*/)
	: CDialog(ResetPasswordForm::IDD, pParent)
{

}

ResetPasswordForm::ResetPasswordForm(CString str1,CString str2,CString user,CString pass,bool defaultuser)
	: CDialog(ResetPasswordForm::IDD, NULL)
{
	this->userName=str1;
	this->machineName=str2;
	this->defaultuser = defaultuser;
	this->user=user;
	this->pass=pass;
}
ResetPasswordForm::ResetPasswordForm(int i)
	: CDialog(ResetPasswordForm::IDD, NULL)
{
	this->canProceed=false;
}
/*
ResetPasswordForm::ResetPasswordForm()
	: CDialog(ResetPasswordForm::IDD, NULL)
{
	MessageBox(L"called");
}
*/
ResetPasswordForm::~ResetPasswordForm()
{
}

void ResetPasswordForm::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_NewPasswordtextBox, c_NewPassword);
	DDX_Control(pDX, IDC_ConfirmPasswordtextBox, c_ConfirmPassword);
}


BEGIN_MESSAGE_MAP(ResetPasswordForm, CDialog)
	ON_BN_CLICKED(IDC_CloseButton, &ResetPasswordForm::OnBnClickedClosebutton)
	ON_BN_CLICKED(IDC_OKButton, &ResetPasswordForm::OnBnClickedOkbutton)
END_MESSAGE_MAP()

BOOL ResetPasswordForm::OnInitDialog()
{
	CDialog::OnInitDialog();
	//SetWindowPos(NULL,0,0,300,300,SWP_NOZORDER | SWP_NOMOVE );
	SetWindowPos(NULL,0,0,300,240,SWP_NOZORDER | SWP_NOMOVE );
//	LPOLESTR name = new OLECHAR[MAX_PATH];
//	wcscpy(name,L"Reset Password for \'");
//	wcscat(name,this->userName);
//	wcscat(name,L"\'");
	SetWindowText(L"Reset Password");

	CStatic* cs=(CStatic*)GetDlgItem(IDC_NewPasswordLabel);
	cs->ShowWindow(SW_SHOW);
//	cs->MoveWindow(49,93,98,15,1);
	cs->MoveWindow(49,43,98,15,1);

	cs=(CStatic*)GetDlgItem(IDC_ConfirmPasswordLabel);
	cs->ShowWindow(SW_SHOW);
//	cs->MoveWindow(36,137,116,15,1);
	cs->MoveWindow(36,87,116,15,1);

//	cs=(CStatic*)GetDlgItem(IDC_USERNAMELABEL);
//	cs->ShowWindow(SW_SHOW);
//	cs->MoveWindow(125,40,200,16,1);
//	cs->SetWindowText(this->userName);

	CEdit* ce=(CEdit*)GetDlgItem(IDC_NewPasswordtextBox);
	ce->ShowWindow(SW_SHOW);
//	ce->MoveWindow(156,93,100,20,1);
	ce->MoveWindow(156,43,100,20,1);

	ce=(CEdit*)GetDlgItem(IDC_ConfirmPasswordtextBox);
	ce->ShowWindow(SW_SHOW);
	//ce->MoveWindow(156,134,100,20,1);
	ce->MoveWindow(156,84,100,20,1);

	CButton* cb=(CButton*)GetDlgItem(IDC_OKButton);
	cb->ShowWindow(SW_SHOW);
//	cb->MoveWindow(52,209,75,23,1);
	cb->MoveWindow(52,149,75,23,1);

	cb=(CButton*)GetDlgItem(IDC_CloseButton);
	cb->ShowWindow(SW_SHOW);
//	cb->MoveWindow(156,209,75,23,1);
	cb->MoveWindow(156,149,75,23,1);

	return true;
}
// ResetPasswordForm message handlers

	// Close Button
void ResetPasswordForm::OnBnClickedClosebutton()
{
	this->EndDialog(IDOK);
}	// Close Button

void ResetPasswordForm::OnOK()
{
	OnBnClickedOkbutton();
}

	// OK Button
void ResetPasswordForm::OnBnClickedOkbutton()
{
	//CString newPassword;
	//CString confirmPassword;
	c_NewPassword.GetWindowText(this->newPassword);
	c_ConfirmPassword.GetWindowText(this->confirmPassword);

	if( newPassword.IsEmpty() || confirmPassword.IsEmpty() )
	{
		AfxMessageBox(L"Please Enter details correctly",MB_ICONINFORMATION,MB_OK);
		return;
	}
	
	if( newPassword.Compare(confirmPassword) )
	{
		AfxMessageBox(L" \"Confirm Password\" is not equal to \"New Password\"",MB_ICONINFORMATION,MB_OK);
		return;
	}

	this->canProceed=true;
	this->EndDialog(IDOK);
	/*
	try
	{
		LPOLESTR adsPath = new OLECHAR[MAX_PATH];
		wcscpy(adsPath,L"WinNT://");
		wcscat(adsPath,machineName);
		wcscat(adsPath,L"/");
		wcscat(adsPath,userName);
		wcscat(adsPath,L",User");
		CoInitialize(NULL);
		IADsUser *pADsUser=NULL;
		HRESULT hr;
		
		if( defaultuser )
		{
			hr = ADsGetObject( adsPath, IID_IADsUser, (void**) &pADsUser );
		}
		else
		{
			hr = ADsOpenObject(adsPath,user,pass,ADS_SECURE_AUTHENTICATION, IID_IADsUser, (void**) &pADsUser);
		}

		if( !SUCCEEDED(hr) )
		{
			AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);
			return;
		}

		BSTR bstrPassword=newPassword.AllocSysString();
			
		hr=pADsUser->SetPassword(bstrPassword);
			

		if( !SUCCEEDED(hr) )
		{
			if( hr == -2147022651 )
				AfxMessageBox(L"The password does not meet the password policy requirements",MB_ICONINFORMATION,MB_OK);
			else if( hr == -2147024891 )
				AfxMessageBox(L"You dont have permission to reset Password",MB_ICONINFORMATION,MB_OK);
			else
				AfxMessageBox(L"Unspecified error occurred while reset the password",MB_ICONINFORMATION,MB_OK);

			SysFreeString(bstrPassword);
			return;
		}
		
		AfxMessageBox(L"Password is changed successfully",MB_ICONINFORMATION,MB_OK);
		SysFreeString(bstrPassword);
		this->EndDialog(IDOK);			
	}
	catch(...)
	{
		AfxMessageBox(L"Unspecified error occurred ",MB_ICONINFORMATION,MB_OK);
		return;
	}
	*/
}	// OK Button
