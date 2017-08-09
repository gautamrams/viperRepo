// AddForm.cpp : implementation file
//

#include "stdafx.h"
#include "LocalUserManagement.h"
#include "AddForm.h"
#include "SingleAddUserForm.h"
#include "MultipleUserAddForm.h"

#include "IADs.h"
#include "ADshlp.h"

#include<fstream>
#include<string>
using namespace std;


// AddForm dialog

IMPLEMENT_DYNAMIC(AddForm, CDialog)

AddForm::AddForm(CWnd* pParent /*=NULL*/)
	: CDialog(AddForm::IDD, pParent)
{

}
AddForm::AddForm(CString user,CString pass,bool defaultuser,list<CString> compList)
	: CDialog(AddForm::IDD, NULL)
{
	this->user=user;
	this->pass=pass;
	this->defaultuser=defaultuser;
	this->computerList=compList;
	this->canUpdate=false;
}

AddForm::~AddForm()
{
}

void AddForm::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_UserListView, c_UserListView);
	DDX_Control(pDX, IDC_ComputerListView, c_ComputerListView);
	DDX_Control(pDX, IDC_GroupListView, c_GroupListView);
	DDX_Control(pDX, IDC_TitleLabel, c_TitleLabel);
}


BEGIN_MESSAGE_MAP(AddForm, CDialog)
	ON_BN_CLICKED(IDC_AddUserButton, &AddForm::OnBnClickedAdduserbutton)
	ON_BN_CLICKED(IDC_CSVButton, &AddForm::OnBnClickedCsvbutton)
	ON_BN_CLICKED(IDC_CloseButton, &AddForm::OnBnClickedClosebutton)
	ON_BN_CLICKED(IDC_CreateButton, &AddForm::OnBnClickedCreatebutton)
END_MESSAGE_MAP()

BOOL AddForm::OnInitDialog()
{
	CDialog::OnInitDialog();

	FormLoad();
	return true;
}
	
	// Form Load
void AddForm::FormLoad()
{
	SetWindowPos(NULL,0,0,597,617,SWP_NOZORDER | SWP_NOMOVE );

	CStatic* cs=(CStatic*)GetDlgItem(IDC_TitleLabel);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(161,15,239,20,1);
	c_TitleLabel.SetFontName(L"Microsoft Sans Serif");
	c_TitleLabel.SetFontSize(12);
	c_TitleLabel.SetFontBold(1);
	c_TitleLabel.SetFontUnderline(1);

	CButton* cb=(CButton*)GetDlgItem(IDC_AddUserButton);
	cb->ShowWindow(SW_SHOW);
	cb->MoveWindow(308,71,92,26,1);

	cb=(CButton*)GetDlgItem(IDC_CSVButton);
	cb->ShowWindow(SW_SHOW);
	cb->MoveWindow(414,71,118,26,1);

	cb=(CButton*)GetDlgItem(IDC_CreateButton);
	cb->ShowWindow(SW_SHOW);
	cb->MoveWindow(355,553,92,23,1);

	cb=(CButton*)GetDlgItem(IDC_CloseButton);
	cb->ShowWindow(SW_SHOW);
	cb->MoveWindow(476,553,75,23,1);

	CWnd* pWnd = GetDlgItem(IDC_UserListgroupBox);
	pWnd->ShowWindow(SW_SHOW);
	pWnd->MoveWindow(37,103,501,224,1);

	pWnd = GetDlgItem(IDC_ComputerListgroupBox);
	pWnd->ShowWindow(SW_SHOW);
	pWnd->MoveWindow(37,344,244,191,1);

	pWnd = GetDlgItem(IDC_GroupListgroupBox);
	pWnd->ShowWindow(SW_SHOW);
	pWnd->MoveWindow(311,344,227,193,1);

	CListCtrl* cl=(CListCtrl*)GetDlgItem(IDC_UserListView); 
	cl->ShowWindow(SW_SHOW);
	cl->MoveWindow(37+6,103+20,489,197,1);

	cl=(CListCtrl*)GetDlgItem(IDC_ComputerListView); 
	cl->ShowWindow(SW_SHOW);
	cl->MoveWindow(37+6,344+19,232,158,1);

	cl=(CListCtrl*)GetDlgItem(IDC_GroupListView); 
	cl->ShowWindow(SW_SHOW);
	cl->MoveWindow(311+6,344+19,215,158,1);

			//Adding Column
	LVCOLUMN lvcolumn;
	lvcolumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvcolumn.fmt = LVCFMT_LEFT;
	
	lvcolumn.cx = 150;
	lvcolumn.pszText = L"User Name";
	c_UserListView.InsertColumn(0,&lvcolumn);
	lvcolumn.cx = 100;
	lvcolumn.pszText = L"Full Name";
	c_UserListView.InsertColumn(1,&lvcolumn);
	lvcolumn.cx = 150;
	lvcolumn.pszText = L"Description";
	c_UserListView.InsertColumn(2,&lvcolumn);
	lvcolumn.cx = 90;
	lvcolumn.pszText = L"Password";
	c_UserListView.InsertColumn(3,&lvcolumn);
	c_UserListView.SetExtendedStyle(LVS_EX_CHECKBOXES | LVS_EX_GRIDLINES);

	lvcolumn.cx = 228;
	lvcolumn.pszText = L"Computer Name";
	c_ComputerListView.InsertColumn(0,&lvcolumn);
	c_ComputerListView.SetExtendedStyle(LVS_EX_CHECKBOXES);

	lvcolumn.cx = 210;
	lvcolumn.pszText = L"Group Name";
	c_GroupListView.InsertColumn(0,&lvcolumn);
	c_GroupListView.SetExtendedStyle(LVS_EX_CHECKBOXES);

		// Adding Computers
	int m=0;
	for(list<CString>::iterator iter=computerList.begin();iter!=computerList.end();++iter)
	{
		c_ComputerListView.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
		c_ComputerListView.SetItemText(m,0,*iter);
		c_ComputerListView.SetCheck(m,1);
		m=m+1;
	}

		// Adding Groups
	m=0;
	c_GroupListView.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
	c_GroupListView.SetItemText(m,0,L"Administrators");
	m=m+1;
	c_GroupListView.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
	c_GroupListView.SetItemText(m,0,L"Backup Operators");
	m=m+1;
	c_GroupListView.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
	c_GroupListView.SetItemText(m,0,L"Guests");
	m=m+1;
	c_GroupListView.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
	c_GroupListView.SetItemText(m,0,L"Network Configuration Operators");
	m=m+1;
	c_GroupListView.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
	c_GroupListView.SetItemText(m,0,L"Remote Desktop Users");
	m=m+1;
	c_GroupListView.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
	c_GroupListView.SetItemText(m,0,L"Replicator");
	m=m+1;
	c_GroupListView.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
	c_GroupListView.SetItemText(m,0,L"Users");
	c_GroupListView.SetCheck(m,1);
	m=m+1;
	c_GroupListView.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
	c_GroupListView.SetItemText(m,0,L"HelpServicesGroup");
	m=m+1;
} // Form Load

	// Single User Add Button
void AddForm::OnBnClickedAdduserbutton()
{
	SingleAddUserForm singleUserForm;
	singleUserForm.DoModal();
	
	if( singleUserForm.canAdd )
	{ 
		int m=c_UserListView.GetItemCount();

		c_UserListView.InsertItem(LVIF_TEXT|LVIF_STATE,m, _T(""),0, 0, 0, 0);
		c_UserListView.SetItemText(m,0,singleUserForm.username);
		c_UserListView.SetItemText(m,1,singleUserForm.fullname);
		c_UserListView.SetItemText(m,2,singleUserForm.desc);
		c_UserListView.SetItemText(m,3,singleUserForm.password);
	}
}  // Single User Add Button

bool AddForm::isValidFile(CString fileName)
{
	ifstream myFile;
	myFile.open(fileName);
	char ch;
	int comma=0;
	
	while( !myFile.eof() )
	{
		myFile.get(ch);
		if(ch=='\n' && !myFile.eof())
		{
			if(comma!=3)
			{
				return false;
			}
			else
				comma=0;
		}
		else if( ch==',' && !myFile.eof())
		{
			comma=comma+1;
		}
	}

	if( comma ==3 || comma ==0)
		return true;
	return false;
}

	 // Multiple User Add Button
void AddForm::OnBnClickedCsvbutton()
{
	MultipleUserAddForm multipleUserForm;
	multipleUserForm.DoModal();
//char outp[100];

	if( multipleUserForm.canAdd )
	{
		ifstream myFile;
		myFile.open(multipleUserForm.fileName);

		if( ! myFile.is_open() )
		{
			AfxMessageBox(L"Unable to open the file",MB_ICONINFORMATION,MB_OK);
			return;
		}
		
		if( ! isValidFile(multipleUserForm.fileName) )
		{
			AfxMessageBox(L"This CSV file is not in correct format..\nThe Format of file is : \n \tUser Name,Full Name,Description,Password",MB_ICONINFORMATION,MB_OK);
			return;
		}

		int i=0,j=0;
		//char line[400];
		string line;
		int wordCount=0;
		int m=c_UserListView.GetItemCount()-1;

		while(!myFile.eof())
		{
			//myFile >> line;
			getline(myFile,line);
			CString word(line.c_str());
			wordCount=0;
			for( i=0,j=0;i<=word.GetLength();i++)
			{
				if( word[i] == ',' || i == word.GetLength())
				{
					char *word;
					word=(char*)malloc(sizeof(char)*i-j+1);

					int k,l;
					for(k=j,l=0;k<i;k++,l++)
						word[l]=line[k];
					word[l]='\0';

					CString wordCString(word);
				
					j=i+1;

						// Inserting into  List View
					wordCount=wordCount+1;
					if( wordCount%4==1 )
					{
						m=m+1;
						c_UserListView.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
						c_UserListView.SetItemText(m,0,wordCString);
					}
					else if( wordCount%4==2 )
						c_UserListView.SetItemText(m,1,wordCString);
					else if( wordCount%4==3 )
						c_UserListView.SetItemText(m,2,wordCString);
					else if( wordCount%4==0 )
						c_UserListView.SetItemText(m,3,wordCString);
				}
			}
		} //while
	}
}	// Multiple User Add Button

	// Close Button
void AddForm::OnBnClickedClosebutton()
{
	this->EndDialog(IDOK);
}	// Close Button

void AddForm::OnOK()
{
	OnBnClickedCreatebutton();
}
	// Create Button
void AddForm::OnBnClickedCreatebutton()
{
	int i=0;
		
		// Checking User ListView
	for( i=0;i<c_UserListView.GetItemCount();i++)
	{
		bool bChecked = c_UserListView.GetCheck(i);
		if( bChecked == true )
			break;
	}

	if( i == c_UserListView.GetItemCount() )
	{
		AfxMessageBox(L"Please Select atleast one User",MB_ICONINFORMATION,MB_OK);
		return;
	}
		// Checking Computer ListView
	for( i=0;i<c_ComputerListView.GetItemCount();i++)
	{
		bool bChecked = c_ComputerListView.GetCheck(i);
		if( bChecked == true )
			break;
	}

	if( i == c_ComputerListView.GetItemCount() )
	{
		AfxMessageBox(L"Please Select atleast one Computer",MB_ICONINFORMATION,MB_OK);
		return;
	}

			// Checking Group ListView
	for( i=0;i<c_GroupListView.GetItemCount();i++)
	{
		bool bChecked = c_GroupListView.GetCheck(i);
		if( bChecked == true )
			break;
	}

	if( i == c_GroupListView.GetItemCount() )
	{
		AfxMessageBox(L"Please Select atleast one Group",MB_ICONINFORMATION,MB_OK);
		return;
	}
	
	AddUsers();
} // Create Button

void AddForm::AddUsers()
{
	CString fileString;
	fileString="";
	bool isError=false;
	int noOfErrors=0;
	bool isAccessDenied=false;
	bool isPasswordProblem=false;
		
	int computerIndex;
	for(computerIndex=0;computerIndex<c_ComputerListView.GetItemCount();computerIndex++)
	{
		if( c_ComputerListView.GetCheck(computerIndex) )
		{
			int userIndex;
			CString computerName=c_ComputerListView.GetItemText(computerIndex,0);

			for(userIndex=0;userIndex<c_UserListView.GetItemCount();userIndex++)
			{
				if( c_UserListView.GetCheck(userIndex) )
				{
					//CString computerName=c_ComputerListView.GetItemText(computerIndex,0);
					CString userName=c_UserListView.GetItemText(userIndex,0);
					CString fullName=c_UserListView.GetItemText(userIndex,1);
					CString description=c_UserListView.GetItemText(userIndex,2);
					CString password=c_UserListView.GetItemText(userIndex,3);
					
					IADsContainer *pCont = NULL;
					IADsUser *pADsUser = NULL;
					IDispatch *pDisp = NULL;
					HRESULT hr1;
					
					LPOLESTR adsPath1 = new OLECHAR[MAX_PATH];
					wcscpy(adsPath1,L"WinNT://");
					wcscat(adsPath1,computerName);

					CoInitialize(NULL);
					
					if( this->defaultuser )
					{
						hr1 = ADsGetObject( adsPath1,IID_IADsContainer,(void**)&pCont);
					}
					else
					{
						hr1 = ADsOpenObject( adsPath1,this->user,this->pass,ADS_SECURE_AUTHENTICATION, IID_IADsContainer,(void**)&pCont);
					}

					if( !SUCCEEDED(hr1) ) 
					{
						isError=true;
						++noOfErrors;
						fileString.Append(L"Unspecified Error occurred while Getting Connetion to \"");
						fileString.Append(computerName);
						fileString.Append(L"\" Computer");
						fileString.Append(L"\n");

						break;
					}
					
					BSTR bstrUserName;
					bstrUserName=userName.AllocSysString();
					hr1 = pCont->Create(CComBSTR("User"), bstrUserName, &pDisp);

					if( !SUCCEEDED(hr1) ) 
					{
						isError=true;
						++noOfErrors;

						if( hr1 == -2147022651 )
						{
							isPasswordProblem=true;
							fileString.Append(L"Password \'");
							fileString.Append(password);
							fileString.Append(L"\' does not meet the password policy requirements while adding users in \"");
							fileString.Append(computerName);
							fileString.Append(L"\" Computer");
							fileString.Append(L"\n");

						}
						else if( hr1 == -2147024891 )
						{
							isAccessDenied=true;
							fileString.Append(L"You dont have permission to create Users in \"");
							fileString.Append(computerName);
							fileString.Append(L"\"");
							fileString.Append(L"\n");
						}
						else if( hr1 == -2147022672 )
						{
							fileString.Append(L"\"");
							fileString.Append(userName);
							fileString.Append(L"\" already exists in \"");
							fileString.Append(computerName);
							fileString.Append(L"\"");
							fileString.Append(L"\n");
						}
						else
						{
							fileString.Append(L"Unspecified error occurred when creating Users in \"");
							fileString.Append(computerName);
							fileString.Append(L"\"");
							fileString.Append(L"\n");
						}

						//return;
						continue;
					}

					hr1 = pDisp->QueryInterface(IID_IADsUser, (void**) &pADsUser);
					
					BSTR bstrFullName;
					bstrFullName=fullName.AllocSysString();
					hr1=pADsUser->put_FullName(bstrFullName);
					
					BSTR bstrDescription;
					bstrDescription=description.AllocSysString();
					hr1=pADsUser->put_Description(bstrDescription);
					
					BSTR bstrPassword;
					bstrPassword=password.AllocSysString();
					hr1=pADsUser->SetPassword(bstrPassword);

					if( !SUCCEEDED(hr1) ) 
					{
						isError=true;
						++noOfErrors;

						if( hr1 == -2147022651 )
						{
							isPasswordProblem=true;
							fileString.Append(L"Password \'");
							fileString.Append(password);
							fileString.Append(L"\' does not meet the password policy requirements while adding users in \"");
							fileString.Append(computerName);
							fileString.Append(L"\" Computer");
							fileString.Append(L"\n");
						}
						else if( hr1 == -2147024891 )
						{
							isAccessDenied=true;
							fileString.Append(L"You dont have permission to create Users in \"");
							fileString.Append(computerName);
							fileString.Append(L"\"");
							fileString.Append(L"\n");

						}
						else if( hr1 == -2147022672 )
						{

							fileString.Append(L"\"");
							fileString.Append(userName);
							fileString.Append(L"\" already exists in \"");
							fileString.Append(computerName);
							fileString.Append(L"\"");
							fileString.Append(L"\n");

						}
						else
						{

							fileString.Append(L"Unspecified error occurred when creating Users in \"");
							fileString.Append(computerName);
							fileString.Append(L"\"");
							fileString.Append(L"\n");

						}

						//return;
						continue;
					}

					hr1=pADsUser->SetInfo(); // Commit
					if( !SUCCEEDED(hr1) ) 
					{
						isError=true;
						++noOfErrors;

						if( hr1 == -2147022651 )
						{
							isPasswordProblem=true;
							fileString.Append(L"Password \'");
							fileString.Append(password);
							fileString.Append(L"\' does not meet the password policy requirements while adding users in \"");
							fileString.Append(computerName);
							fileString.Append(L"\" Computer");
							fileString.Append(L"\n");

						}
						else if( hr1 == -2147024891 )
						{
							isAccessDenied=true;
							fileString.Append(L"You dont have permission to create Users in \"");
							fileString.Append(computerName);
							fileString.Append(L"\"");
							fileString.Append(L"\n");


						}
						else if( hr1 == -2147022672 )
						{
							fileString.Append(L"\"");
							fileString.Append(userName);
							fileString.Append(L"\" already exists in \"");
							fileString.Append(computerName);
							fileString.Append(L"\"");
							fileString.Append(L"\n");
						}
						else
						{
							fileString.Append(L"Unspecified error occurred when creating Users in \"");
							fileString.Append(computerName);
							fileString.Append(L"\"");
							fileString.Append(L"\n");
						}

						//return;
						continue;
					}
					++noOfErrors;
					fileString.Append(L"\"");
					fileString.Append(userName);
					fileString.Append(L"\" is created in \"");
					fileString.Append(computerName);
					fileString.Append(L"\" computer successfully");
					fileString.Append(L"\n");
					

					this->canUpdate=true;

					int groupIndex;
					for(groupIndex=0;groupIndex<c_GroupListView.GetItemCount();groupIndex++)
					{
						if( c_GroupListView.GetCheck(groupIndex) )
						{
							CString groupName=c_GroupListView.GetItemText(groupIndex,0);
					
							LPOLESTR adsPath = new OLECHAR[MAX_PATH];
							wcscpy(adsPath,L"WinNT://");
							wcscat(adsPath,computerName);
							wcscat(adsPath,L"/");
							wcscat(adsPath,groupName);
							CoInitialize(NULL);
							IADsGroup *pADsGroup=NULL;
							HRESULT hr;

							if( this->defaultuser )
							{
								hr = ADsGetObject( adsPath, IID_IADsGroup, (void**) &pADsGroup );
							}
							else
							{
								hr = ADsOpenObject(adsPath,this->user,this->pass,ADS_SECURE_AUTHENTICATION, IID_IADsGroup, (void**) &pADsGroup);
							}

							if( !SUCCEEDED(hr) )
							{
								isError=true;
								++noOfErrors;

								fileString.Append(L"Unspecified error occurred while accesing \"");
								fileString.Append(groupName);
								fileString.Append(L"\" group in \"");
								fileString.Append(computerName);
								fileString.Append(L"\" computer");
								fileString.Append(L"\n");

								//return;
								continue;
							}
	
							BSTR bstrUserPath;
							pADsUser->get_ADsPath(&bstrUserPath);

							hr=pADsGroup->Add(bstrUserPath);
							if(SUCCEEDED(hr)){
								
							}
							if( !SUCCEEDED(hr) )
							{
								isError=true;
								++noOfErrors;

								if( hr == -2147024891 )
								{
									isAccessDenied=true;
									fileString.Append(L"You dont have permission to create Users in \"");
									fileString.Append(computerName);
									fileString.Append(L"\"");
									fileString.Append(L"\n");

								}
								else
								{
									fileString.Append(L"Unspecified error occurred when creating Users in \"");
									fileString.Append(computerName);
									fileString.Append(L"\"");
									fileString.Append(L"\n");


								}
								SysFreeString(bstrUserPath);
								//return;
								continue;
							}
		
							++noOfErrors;
							fileString.Append(L"\"");
							fileString.Append(userName);
							fileString.Append(L"\" is included at \""); 
							fileString.Append(groupName);
							fileString.Append(L"\" group in \"");
							fileString.Append(computerName);
							fileString.Append(L"\" computer successfully");
							fileString.Append(L"\n");
			

						} // if
					} // 'for' loop for Groups

				} // if
			} // 'for' loop for Users
//			
		} // if 
	} // 'for' loop for Computers

	if( ! isError )
	{
		AfxMessageBox(L"Successfully added user(s)",MB_ICONINFORMATION,MB_OK);
		this->EndDialog(IDOK);
	}
	else if( noOfErrors < 3 )
	{
		AfxMessageBox(fileString,MB_ICONINFORMATION,MB_OK);
	}
	else
	{
		ofstream outFile;

		try
		{
			/*
			CFile cfile;
			cfile.Open(L"LocalUserManagementReports.txt",CFile::modeRead);
			CString filePath=cfile.GetFilePath();
			cfile.Close();
			*/
	
		//	char *buffer = getenv("windir");
		//	CString path(buffer);
		//	path.Append(L"\\Temp\\LocalUserManagementReports.txt");

			outFile.open("c:\\Windows\\Temp\\LocalUserManagementReports.txt",ios::out) ;
//			outFile.open(path,ios::out) ;
			outFile<<CT2CA(fileString);
			outFile.close();
			
			CString temp;
			temp="";
			if( isAccessDenied )
			{
				temp.Append(L"Access Denied...\n");
			}
			else if( isPasswordProblem )
			{
				temp.Append(L"Password does not meet the password policy...\n"); 
			}
			else
			{
				temp.Append(L"Error occurred while adding users...\n"); 
			}
			//CString temp("Error occurred while adding users.\n Do you want read that Error Report ?");
			//temp.Append(L"\n(");
			//temp.Append(filePath);
			//temp.Append(L")");
			temp.Append(L"To read more reports, Click \"YES\"");
			//if( AfxMessageBox(temp,MB_YESNO) == IDYES ) MB_ICONINFORMATION,MB_OK
			if( AfxMessageBox(temp,MB_YESNO|MB_ICONQUESTION) == IDYES )
			{
				STARTUPINFO startup_info = {0};
				startup_info.cb = sizeof startup_info;
				PROCESS_INFORMATION pi = {0};
				
				//CreateProcess( _T("C:\\WINDOWS\\system32\\notepad.exe"),_T("C:\\WINDOWS\\system32\\notepad.exe LocalUserManagementReports.txt"),NULL,NULL,FALSE,0,NULL,NULL,&startup_info,&pi) ;
//				USES_CONVERSION;
//		LPWSTR newchar = A2W(path);
				//wchar_t string1[20];
//wcscpy( string1, CT2CW( (LPCTSTR)path ));
	//		LPCTSTR string1=	T2CW( (LPCTSTR) path );
//				LPWSTR string1 = path.GetBuffer(1000);
//				path.ReleaseBuffer(); p
			//	LPWSTR str1=path.GetBuffer();
//	USES_CONVERSION;
//	path.Replace(L"\\",L"\\\\");
//	MessageBox(path);
//LPWSTR str1 = (LPWSTR)(LPCWSTR)path;
//MessageBox(str1);


				CreateProcess( _T("C:\\WINDOWS\\system32\\notepad.exe"),_T("C:\\WINDOWS\\system32\\notepad.exe C:\\Windows\\Temp\\LocalUserManagementReports.txt"),NULL,NULL,FALSE,0,NULL,NULL,&startup_info,&pi) ;
				//ShellExecute(NULL,L"open",L"notepad.exe",L"LocalUserManagementReports.txt",NULL,SW_SHOWNORMAL);
			} 
		}
		catch(...)
		{
			outFile.close();
		}
	}
	//this->EndDialog(IDOK);
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 