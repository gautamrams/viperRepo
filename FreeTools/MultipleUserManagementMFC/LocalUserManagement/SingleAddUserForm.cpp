// SingleAddUserForm.cpp : implementation file
//

#include "stdafx.h"
#include "LocalUserManagement.h"
#include "SingleAddUserForm.h"


// SingleAddUserForm dialog

IMPLEMENT_DYNAMIC(SingleAddUserForm, CDialog)

SingleAddUserForm::SingleAddUserForm(CWnd* pParent /*=NULL*/)
	: CDialog(SingleAddUserForm::IDD, pParent)
{

}

SingleAddUserForm::~SingleAddUserForm()
{
}

void SingleAddUserForm::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TitleLabel, c_TitleLabel);
}


BEGIN_MESSAGE_MAP(SingleAddUserForm, CDialog)
	ON_BN_CLICKED(IDC_AddButton, &SingleAddUserForm::OnBnClickedAddbutton)
	ON_BN_CLICKED(IDC_ClearButton, &SingleAddUserForm::OnBnClickedClearbutton)
END_MESSAGE_MAP()

BOOL SingleAddUserForm::OnInitDialog()
{
	CDialog::OnInitDialog();

	this->canAdd=false;

	FormLoad();
	return true;
}

void SingleAddUserForm::FormLoad()
{
	SetWindowPos(NULL,0,0,293,291,SWP_NOZORDER | SWP_NOMOVE );

	CStatic* cs=(CStatic*)GetDlgItem(IDC_TitleLabel);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(83,28,115,16,1);
	c_TitleLabel.SetFontName(L"Microsoft Sans Serif");
	c_TitleLabel.SetFontSize(10);
	c_TitleLabel.SetFontBold(1);
	c_TitleLabel.SetFontUnderline(1);

	cs=(CStatic*)GetDlgItem(IDC_USERNAMELABEL);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(43,62,82,15,1);

	cs=(CStatic*)GetDlgItem(IDC_FullNameLabel);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(48,96,73,15,1);

	cs=(CStatic*)GetDlgItem(IDC_DescriptionLabel);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(43,133,78,15,1);

	cs=(CStatic*)GetDlgItem(IDC_PASSWORDLABEL);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(51,171,70,15,1);

	CEdit* ce=(CEdit*)GetDlgItem(IDC_UserNameTextBox);
	ce->ShowWindow(SW_SHOW);
	ce->MoveWindow(142,61,100,20,1);

	ce=(CEdit*)GetDlgItem(IDC_FullNameTextBox);
	ce->ShowWindow(SW_SHOW);
	ce->MoveWindow(142,96,100,20,1);
	
	ce=(CEdit*)GetDlgItem(IDC_DescriptionTextBox);
	ce->ShowWindow(SW_SHOW);
	ce->MoveWindow(142,133,100,20,1);

	ce=(CEdit*)GetDlgItem(IDC_PasswordTextBox);
	ce->ShowWindow(SW_SHOW);
	ce->MoveWindow(142,170,100,20,1);

	CButton* cb=(CButton*)GetDlgItem(IDC_AddButton);
	cb->ShowWindow(SW_SHOW);
	cb->MoveWindow(54,214,75,23,1);

	cb=(CButton*)GetDlgItem(IDC_ClearButton);
	cb->ShowWindow(SW_SHOW);
	cb->MoveWindow(158,214,75,23,1);

}

void SingleAddUserForm::OnOK()
{
	OnBnClickedAddbutton();
}

	// Add Button
void SingleAddUserForm::OnBnClickedAddbutton()
{
	CString us;
	CString full;
	CString desc;
	CString pass;
	
	CEdit* ce=(CEdit*)GetDlgItem(IDC_UserNameTextBox);
	ce->GetWindowText(us);
	this->username=us;

	ce=(CEdit*)GetDlgItem(IDC_PasswordTextBox);
	ce->GetWindowText(pass);
	this->password=pass;

	ce=(CEdit*)GetDlgItem(IDC_FullNameTextBox);
	ce->GetWindowText(full);
	this->fullname=full;

	ce=(CEdit*)GetDlgItem(IDC_DescriptionTextBox);
	ce->GetWindowText(desc);
	this->desc=desc;

	if( this->username.IsEmpty() )
	{
		AfxMessageBox(L"Please Enter User Name",MB_ICONINFORMATION,MB_OK);
		return;
	}
	if( this->password.IsEmpty() )
	{
		AfxMessageBox(L"Please Enter Password",MB_ICONINFORMATION,MB_OK);
		return;
	}
	
	this->canAdd=true;
	this->EndDialog(1);
}

	// Clear Button
void SingleAddUserForm::OnBnClickedClearbutton()
{
	CEdit* ce=(CEdit*)GetDlgItem(IDC_UserNameTextBox);
	ce->SetWindowText(L"");

	ce=(CEdit*)GetDlgItem(IDC_PasswordTextBox);
	ce->SetWindowText(L"");

	ce=(CEdit*)GetDlgItem(IDC_FullNameTextBox);
	ce->SetWindowText(L"");

	ce=(CEdit*)GetDlgItem(IDC_DescriptionTextBox);
	ce->SetWindowText(L"");
} // Clear Button
