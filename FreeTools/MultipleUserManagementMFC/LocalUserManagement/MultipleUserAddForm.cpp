// MultipleUserAddForm.cpp : implementation file
//

#include "stdafx.h"
#include "LocalUserManagement.h"
#include "MultipleUserAddForm.h"


// MultipleUserAddForm dialog

IMPLEMENT_DYNAMIC(MultipleUserAddForm, CDialog)

MultipleUserAddForm::MultipleUserAddForm(CWnd* pParent /*=NULL*/)
	: CDialog(MultipleUserAddForm::IDD, pParent)
{
x=0;
}
MultipleUserAddForm::MultipleUserAddForm(int a)
	: CDialog(MultipleUserAddForm::IDD)
{
 x=a;
}
MultipleUserAddForm::~MultipleUserAddForm()
{
}

void MultipleUserAddForm::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_FileNameTextBox, c_FileNameTextBox);
	DDX_Control(pDX, IDC_TitleLabel, c_TitleLabel);
	DDX_Control(pDX, IDC_TitleLabel2, c_TitleLabel2);
	DDX_Control(pDX, IDC_Title3Label, c_Title3Label);
}


BEGIN_MESSAGE_MAP(MultipleUserAddForm, CDialog)
	ON_BN_CLICKED(IDC_OpenButton, &MultipleUserAddForm::OnBnClickedOpenbutton)
	ON_BN_CLICKED(IDC_CloseButton, &MultipleUserAddForm::OnBnClickedClosebutton)
	ON_BN_CLICKED(IDC_ImportButton, &MultipleUserAddForm::OnBnClickedImportbutton)
	
	
END_MESSAGE_MAP()

BOOL MultipleUserAddForm::OnInitDialog()
{
	CDialog::OnInitDialog();
	if(!x){
	this->canAdd=false;
	FormLoad();
	}
	else
	FormLoad2();
	return true;
}

void MultipleUserAddForm::FormLoad2()
{
	SetWindowPos(NULL,0,0,343,221,SWP_NOZORDER | SWP_NOMOVE );

	CStatic* cs=(CStatic*)GetDlgItem(IDC_TitleLabel);
	cs->ShowWindow(SW_SHOW);
	//cs->MoveWindow(91,30,136,16,1);
	//cs->MoveWindow(21,20,300,16,1);
	cs->MoveWindow(63,140,280,40,1);
	c_TitleLabel.SetWindowText(L"CSV file should contain one of the following headers name/cn/computername");
	c_TitleLabel.SetFontName(L"Verdana");
	c_TitleLabel.SetFontSize(8);
	//c_TitleLabel.SetFontBold(1);

	c_TitleLabel2.ShowWindow(SW_HIDE);

	c_Title3Label.ShowWindow(SW_SHOW);
	//c_TitleLabel2.MoveWindow(31,40,300,16,1);
	c_Title3Label.MoveWindow(21,140,40,16,1);
	c_Title3Label.SetFontName(L"Verdana");
	c_Title3Label.SetFontSize(8);
	c_Title3Label.SetFontBold(1);
	
	cs=(CStatic*)GetDlgItem(IDC_SelectedFileLabel);
	cs->ShowWindow(SW_SHOW);
	//cs->MoveWindow(31,86,87,15,1);
	cs->MoveWindow(31,44,87,15,1);

	CEdit* ce=(CEdit*)GetDlgItem(IDC_FileNameTextBox);
	ce->ShowWindow(SW_SHOW);
	//ce->MoveWindow(120,85,146,20,1);
	ce->MoveWindow(120,40,146,20,1);
	//ce->SetReadOnly(1);

	CButton* cb=(CButton*)GetDlgItem(IDC_OpenButton);
	cb->ShowWindow(SW_SHOW);
	//cb->MoveWindow(272,83,29,22,1);
	cb->MoveWindow(272,38,29,22,1);

	cb=(CButton*)GetDlgItem(IDC_ImportButton);
	cb->ShowWindow(SW_SHOW);
	//cb->MoveWindow(93,141,75,23,1);
	cb->MoveWindow(93,96,75,23,1);

	cb=(CButton*)GetDlgItem(IDC_CloseButton);
	cb->ShowWindow(SW_SHOW);
	//cb->MoveWindow(194,141,75,23,1);
	cb->MoveWindow(194,96,75,23,1);
}

void MultipleUserAddForm::FormLoad()
{
	SetWindowPos(NULL,0,0,343,221,SWP_NOZORDER | SWP_NOMOVE );

	CStatic* cs=(CStatic*)GetDlgItem(IDC_TitleLabel);
	cs->ShowWindow(SW_SHOW);
	//cs->MoveWindow(91,30,136,16,1);
	//cs->MoveWindow(21,20,300,16,1);
	cs->MoveWindow(61,140,300,16,1);
	c_TitleLabel.SetFontName(L"Verdana");
	c_TitleLabel.SetFontSize(8);
	//c_TitleLabel.SetFontBold(1);

	c_TitleLabel2.ShowWindow(SW_SHOW);
	//c_TitleLabel2.MoveWindow(31,40,300,16,1);
	c_TitleLabel2.MoveWindow(21,155,300,16,1);
	c_TitleLabel2.SetFontName(L"Verdana");
	c_TitleLabel2.SetFontSize(8);
	//c_TitleLabel2.SetFontBold(1);

	c_Title3Label.ShowWindow(SW_SHOW);
	//c_TitleLabel2.MoveWindow(31,40,300,16,1);
	c_Title3Label.MoveWindow(21,140,40,16,1);
	c_Title3Label.SetFontName(L"Verdana");
	c_Title3Label.SetFontSize(8);
	c_Title3Label.SetFontBold(1);


	cs=(CStatic*)GetDlgItem(IDC_SelectedFileLabel);
	cs->ShowWindow(SW_SHOW);
	//cs->MoveWindow(31,86,87,15,1);
	cs->MoveWindow(31,44,87,15,1);

	CEdit* ce=(CEdit*)GetDlgItem(IDC_FileNameTextBox);
	ce->ShowWindow(SW_SHOW);
	//ce->MoveWindow(120,85,146,20,1);
	ce->MoveWindow(120,40,146,20,1);
	ce->SetReadOnly(1);

	CButton* cb=(CButton*)GetDlgItem(IDC_OpenButton);
	cb->ShowWindow(SW_SHOW);
	//cb->MoveWindow(272,83,29,22,1);
	cb->MoveWindow(272,38,29,22,1);

	cb=(CButton*)GetDlgItem(IDC_ImportButton);
	cb->ShowWindow(SW_SHOW);
	//cb->MoveWindow(93,141,75,23,1);
	cb->MoveWindow(93,96,75,23,1);

	cb=(CButton*)GetDlgItem(IDC_CloseButton);
	cb->ShowWindow(SW_SHOW);
	//cb->MoveWindow(194,141,75,23,1);
	cb->MoveWindow(194,96,75,23,1);
}
	// Browse Button
void MultipleUserAddForm::OnBnClickedOpenbutton()
{
	//CFileDialog dlg(TRUE,NULL,L"CSV File",OFN_ENABLESIZING|OFN_EXPLORER|OFN_FILEMUSTEXIST,L"CSV File(*.csv)",this);
	CFileDialog dlg(TRUE,NULL,L"Open a CSV File",OFN_ENABLESIZING|OFN_EXPLORER|OFN_FILEMUSTEXIST,L"CSV File(*.csv)|*.csv||",this);
	CString fileName;

	if( dlg.DoModal() == IDOK )
	{
		fileName=dlg.GetPathName();
		c_FileNameTextBox.SetWindowText(fileName);
		this->fileName=fileName;
	}
} // Browse Button

	// Close Button
void MultipleUserAddForm::OnBnClickedClosebutton()
{
	this->EndDialog(IDOK);
}  // Close Button

void MultipleUserAddForm::OnOK()
{
	OnBnClickedImportbutton();
}

void MultipleUserAddForm::OnBnClickedImportbutton()
{
	c_FileNameTextBox.GetWindowTextW(this->fileName);
	if( this->fileName.IsEmpty() )
	{
		AfxMessageBox(L"Please Select the Correct file",MB_ICONINFORMATION,MB_OK);
		return;
	}
	this->canAdd=true;
	this->EndDialog(IDOK);
}




