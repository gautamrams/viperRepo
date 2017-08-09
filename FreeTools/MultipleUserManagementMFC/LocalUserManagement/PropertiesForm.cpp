// PropertiesForm.cpp : implementation file
//

#include "stdafx.h"
#include "LocalUserManagement.h"
#include "PropertiesForm.h"
#include "IADs.h"
#include "Adshlp.h"


// PropertiesForm dialog

IMPLEMENT_DYNAMIC(PropertiesForm, CDialog)

PropertiesForm::PropertiesForm(CWnd* pParent /*=NULL*/)
	: CDialog(PropertiesForm::IDD, pParent)
{
}

PropertiesForm::PropertiesForm(CString str1,CString str2,CString str3,CString str4,CString user,CString pass,bool defaultuser)
	: CDialog(PropertiesForm::IDD, NULL)
{
	this->userName=str1;
	this->fullName=str2;
	this->descritpion=str3;
	this->machineName=str4;
	this->defaultuser=defaultuser;
	this->user=user;
	this->pass=pass;
	this->canUpdate=false;
}

PropertiesForm::~PropertiesForm()
{
}

void PropertiesForm::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TAB1, c_TabControl);
	DDX_Control(pDX, IDC_COMBO1, c_GroupCombo);
	DDX_Control(pDX, IDC_Apply1Button, c_Radio1);
	DDX_Control(pDX, IDC_FullNameTextBox, c_FullNameTextBox);
	DDX_Control(pDX, IDC_DescriptionTextBox, c_DescriptionTextBox);
} 


BEGIN_MESSAGE_MAP(PropertiesForm, CDialog)
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB1, &PropertiesForm::OnTcnSelchangeTab1)
	ON_EN_CHANGE(IDC_FullNameTextBox, &PropertiesForm::OnEnChangeFullnametextbox)
	ON_EN_CHANGE(IDC_DescriptionTextBox, &PropertiesForm::OnEnChangeDescriptiontextbox)
	ON_CBN_SELCHANGE(IDC_COMBO1, &PropertiesForm::OnCbnSelchangeCombo1)
	ON_BN_CLICKED(IDC_RADIO1, &PropertiesForm::OnBnClickedRadio1)
	ON_BN_CLICKED(IDC_RADIO2, &PropertiesForm::OnBnClickedRadio2)
	ON_BN_CLICKED(IDC_RADIO3, &PropertiesForm::OnBnClickedRadio3)
	ON_BN_CLICKED(IDC_Close2Button, &PropertiesForm::OnBnClickedClose2button)
	ON_BN_CLICKED(IDC_Close1Button, &PropertiesForm::OnBnClickedClose1button)
	ON_BN_CLICKED(IDC_Apply1Button, &PropertiesForm::OnBnClickedApply1button)
	ON_BN_CLICKED(IDC_Apply2Button, &PropertiesForm::OnBnClickedApply2button)
END_MESSAGE_MAP()

BOOL PropertiesForm::OnInitDialog()
{
	CDialog::OnInitDialog();
	SetWindowPos(NULL,0,0,410,356,SWP_NOZORDER | SWP_NOMOVE );
	LPOLESTR name = new OLECHAR[MAX_PATH];
	wcscpy(name,this->userName);
	wcscat(name,L" Properties");
	SetWindowText(name);

	c_TabControl.MoveWindow(12,12,378,305,1);
	c_TabControl.InsertItem(0,L"Properties");
	c_TabControl.InsertItem(1,L"Group Membership");

	FormLoad();
	GroupMembershipLoad();
	
	return true;
}

// PropertiesForm message handlers
void PropertiesForm::FormLoad()
{
	int x=12;
			// Tab-1 Controls
	CStatic* cs=(CStatic*)GetDlgItem(IDC_USERNAMELABEL);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(44+x+40,48+x+30,82,15,1);

	cs=(CStatic*)GetDlgItem(IDC_FullNameLabel);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(50+x+40,83+x+30,76,15,1);

	cs=(CStatic*)GetDlgItem(IDC_DescriptionLabel);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(44+x+40,118+x+30,81,15,1);

	CEdit* ce=(CEdit*)GetDlgItem(IDC_UserNameTextBox);
	ce->ShowWindow(SW_SHOW);
	ce->MoveWindow(139+x+40,45+x+30,100,20,1);
	ce->SetReadOnly(1);
	ce->SetWindowText(this->userName);

	ce=(CEdit*)GetDlgItem(IDC_FullNameTextBox);
	ce->ShowWindow(SW_SHOW);
	ce->MoveWindow(139+x+40,80+x+30,100,20,1);
	ce->SetWindowText(this->fullName);

	ce=(CEdit*)GetDlgItem(IDC_DescriptionTextBox);
	ce->ShowWindow(SW_SHOW);
	ce->MoveWindow(139+x+40,115+x+30,100,20,1);
	ce->SetWindowText(this->descritpion);

	CButton* cb=(CButton*)GetDlgItem(IDC_Apply1Button);
	cb->ShowWindow(SW_SHOW);
	cb->MoveWindow(157+x+20,210+x+20,75,23,1);
	cb->EnableWindow(0);

	cb=(CButton*)GetDlgItem(IDC_Close1Button);
	cb->ShowWindow(SW_SHOW);
	cb->MoveWindow(254+x+20,210+x+20,75,23,1);

			// Tab-2 Controls
	cs=(CStatic*)GetDlgItem(IDC_Label1);
	cs->ShowWindow(SW_HIDE);
	cs->MoveWindow(47+x,26+x+20,258,13,1);

	cs=(CStatic*)GetDlgItem(IDC_Label2);
	cs->ShowWindow(SW_HIDE);
	cs->MoveWindow(229+x,80+x,105,13,1);

	cs=(CStatic*)GetDlgItem(IDC_Label3);
	cs->ShowWindow(SW_HIDE);
	cs->MoveWindow(70+x,102+x,241,13,1);

	cs=(CStatic*)GetDlgItem(IDC_Label4);
	cs->ShowWindow(SW_HIDE);
	cs->MoveWindow(72+x,116+x,231,13,1);

	cs=(CStatic*)GetDlgItem(IDC_Label5);
	cs->ShowWindow(SW_HIDE);
	cs->MoveWindow(243+x,136+x,72,13,1);

	cs=(CStatic*)GetDlgItem(IDC_Label6);
	cs->ShowWindow(SW_HIDE);
	cs->MoveWindow(69+x,155+x,246,13,1);

	cs=(CStatic*)GetDlgItem(IDC_Label7);
	cs->ShowWindow(SW_HIDE);
	cs->MoveWindow(72+x,170+x,261,13,1);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
	ccb->ShowWindow(SW_HIDE);
	ccb->MoveWindow(170+x,195+x,164,21,1);
	
	cb=(CButton*)GetDlgItem(IDC_Apply2Button);
	cb->ShowWindow(SW_HIDE);
	cb->MoveWindow(182+x,240+x,75,23,1);
	cb->EnableWindow(0);

	cb=(CButton*)GetDlgItem(IDC_Close2Button);
	cb->ShowWindow(SW_HIDE);
	cb->MoveWindow(280+x,240+x,75,23,1);

	cb=(CButton*)GetDlgItem(IDC_RADIO1);
	cb->ShowWindow(SW_HIDE);
	cb->MoveWindow(51+x,78+x,93,17,1);

	cb=(CButton*)GetDlgItem(IDC_RADIO2);
	cb->ShowWindow(SW_HIDE);
	cb->MoveWindow(51+x,134+x,98,17,1);

	cb=(CButton*)GetDlgItem(IDC_RADIO3);
	cb->ShowWindow(SW_HIDE);
	cb->MoveWindow(51+x,199+x,56,17,1);

}
	
void PropertiesForm::OnOK()
{
}
	// Tab Changed
void PropertiesForm::OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult)
{
	*pResult = 0;

	int tabIndex=c_TabControl.GetCurSel();

	if( tabIndex == 0 )
	{
		TabOneShow();
		TabTwoHide();
	}
	else if( tabIndex == 1 )
	{
		TabOneHide();
		TabTwoShow();
	}
}

void PropertiesForm::TabOneShow()
{
	CStatic* cs=(CStatic*)GetDlgItem(IDC_USERNAMELABEL);
	cs->ShowWindow(SW_SHOW);

	cs=(CStatic*)GetDlgItem(IDC_FullNameLabel);
	cs->ShowWindow(SW_SHOW);

	cs=(CStatic*)GetDlgItem(IDC_DescriptionLabel);
	cs->ShowWindow(SW_SHOW);

	CEdit* ce=(CEdit*)GetDlgItem(IDC_UserNameTextBox);
	ce->ShowWindow(SW_SHOW);

	ce=(CEdit*)GetDlgItem(IDC_FullNameTextBox);
	ce->ShowWindow(SW_SHOW);

	ce=(CEdit*)GetDlgItem(IDC_DescriptionTextBox);
	ce->ShowWindow(SW_SHOW);

	CButton* cb=(CButton*)GetDlgItem(IDC_Apply1Button);
	cb->ShowWindow(SW_SHOW);

	cb=(CButton*)GetDlgItem(IDC_Close1Button);
	cb->ShowWindow(SW_SHOW);

}

void PropertiesForm::TabOneHide()
{
	CStatic* cs=(CStatic*)GetDlgItem(IDC_USERNAMELABEL);
	cs->ShowWindow(SW_HIDE);

	cs=(CStatic*)GetDlgItem(IDC_FullNameLabel);
	cs->ShowWindow(SW_HIDE);

	cs=(CStatic*)GetDlgItem(IDC_DescriptionLabel);
	cs->ShowWindow(SW_HIDE);

	CEdit* ce=(CEdit*)GetDlgItem(IDC_UserNameTextBox);
	ce->ShowWindow(SW_HIDE);

	ce=(CEdit*)GetDlgItem(IDC_FullNameTextBox);
	ce->ShowWindow(SW_HIDE);

	ce=(CEdit*)GetDlgItem(IDC_DescriptionTextBox);
	ce->ShowWindow(SW_HIDE);

	CButton* cb=(CButton*)GetDlgItem(IDC_Apply1Button);
	cb->ShowWindow(SW_HIDE);

	cb=(CButton*)GetDlgItem(IDC_Close1Button);
	cb->ShowWindow(SW_HIDE);

}

void PropertiesForm::TabTwoShow()
{
	CStatic* cs=(CStatic*)GetDlgItem(IDC_Label1);
	cs->ShowWindow(SW_SHOW);

	cs=(CStatic*)GetDlgItem(IDC_Label2);
	cs->ShowWindow(SW_SHOW);

	cs=(CStatic*)GetDlgItem(IDC_Label3);
	cs->ShowWindow(SW_SHOW);

	cs=(CStatic*)GetDlgItem(IDC_Label4);
	cs->ShowWindow(SW_SHOW);

	cs=(CStatic*)GetDlgItem(IDC_Label5);
	cs->ShowWindow(SW_SHOW);

	cs=(CStatic*)GetDlgItem(IDC_Label6);
	cs->ShowWindow(SW_SHOW);

	cs=(CStatic*)GetDlgItem(IDC_Label7);
	cs->ShowWindow(SW_SHOW);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
	ccb->ShowWindow(SW_SHOW);
	
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply2Button);
	cb->ShowWindow(SW_SHOW);

	cb=(CButton*)GetDlgItem(IDC_Close2Button);
	cb->ShowWindow(SW_SHOW);

	cb=(CButton*)GetDlgItem(IDC_RADIO1);
	cb->ShowWindow(SW_SHOW);

	cb=(CButton*)GetDlgItem(IDC_RADIO2);
	cb->ShowWindow(SW_SHOW);

	cb=(CButton*)GetDlgItem(IDC_RADIO3);
	cb->ShowWindow(SW_SHOW);
}

void PropertiesForm::TabTwoHide()
{
	CStatic* cs=(CStatic*)GetDlgItem(IDC_Label1);
	cs->ShowWindow(SW_HIDE);

	cs=(CStatic*)GetDlgItem(IDC_Label2);
	cs->ShowWindow(SW_HIDE);

	cs=(CStatic*)GetDlgItem(IDC_Label3);
	cs->ShowWindow(SW_HIDE);

	cs=(CStatic*)GetDlgItem(IDC_Label4);
	cs->ShowWindow(SW_HIDE);

	cs=(CStatic*)GetDlgItem(IDC_Label5);
	cs->ShowWindow(SW_HIDE);

	cs=(CStatic*)GetDlgItem(IDC_Label6);
	cs->ShowWindow(SW_HIDE);

	cs=(CStatic*)GetDlgItem(IDC_Label7);
	cs->ShowWindow(SW_HIDE);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
	ccb->ShowWindow(SW_HIDE);
	
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply2Button);
	cb->ShowWindow(SW_HIDE);

	cb=(CButton*)GetDlgItem(IDC_Close2Button);
	cb->ShowWindow(SW_HIDE);

	cb=(CButton*)GetDlgItem(IDC_RADIO1);
	cb->ShowWindow(SW_HIDE);

	cb=(CButton*)GetDlgItem(IDC_RADIO2);
	cb->ShowWindow(SW_HIDE);

	cb=(CButton*)GetDlgItem(IDC_RADIO3);
	cb->ShowWindow(SW_HIDE);
}
void PropertiesForm::OnEnChangeFullnametextbox()
{
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply1Button);
	cb->EnableWindow(1);
}

void PropertiesForm::OnEnChangeDescriptiontextbox()
{
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply1Button);
	cb->EnableWindow(1);
}

void PropertiesForm::OnCbnSelchangeCombo1()
{
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply2Button);
	cb->EnableWindow(1);

	cb=(CButton*)GetDlgItem(IDC_RADIO3);
	cb->SetCheck(1);
}

void PropertiesForm::OnBnClickedRadio1()
{
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply2Button);
	cb->EnableWindow(1);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
	ccb->EnableWindow(0);
}

void PropertiesForm::OnBnClickedRadio2()
{
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply2Button);
	cb->EnableWindow(1);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
	ccb->EnableWindow(0);
}

void PropertiesForm::OnBnClickedRadio3()
{
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply2Button);
	cb->EnableWindow(1);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
	ccb->EnableWindow(1);
}

void PropertiesForm::GroupMembershipLoad()
{
	LoadGroups();
	FindMembership();
}

void PropertiesForm::LoadGroups()
{
	int groupIndex=-1;
	LPOLESTR adsPath = new OLECHAR[MAX_PATH];
	wcscpy(adsPath,L"WinNT://");
	wcscat(adsPath,this->machineName);
	wcscat(adsPath,L",computer");
	
	CoInitialize(NULL);
	IADsContainer *pCont=NULL;
	HRESULT hr;

	hr = ADsGetObject( adsPath, IID_IADsContainer, (void**) &pCont );

	if( !SUCCEEDED(hr) )
	{
		LPOLESTR temp = new OLECHAR[MAX_PATH];
		wcscpy(temp,L"Unspecified error occurred while accessing the computer \"");
		wcscat(temp,machineName);
		wcscat(temp,L"\"");
		AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
		return;
	}
	
	IEnumVARIANT *pEnum = NULL;
	hr = ADsBuildEnumerator( pCont, &pEnum );

	if ( ! SUCCEEDED(hr) )
	{
		LPOLESTR temp = new OLECHAR[MAX_PATH];
		wcscpy(temp,L"Unspecified error occurred while accessing the computer \"");
		wcscat(temp,machineName);
		wcscat(temp,L"\"");
		AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
		return;
	}
	
	VARIANT var;
	ULONG lFetch;
	IADs *pChild=NULL;
	VariantInit(&var);
		
	while( SUCCEEDED(ADsEnumerateNext( pEnum, 1, &var, &lFetch )) && lFetch == 1 )
    {
		hr = V_DISPATCH(&var)->QueryInterface( IID_IADs, (void**) &pChild );
        if ( SUCCEEDED(hr) )
        {
			BSTR bstrClass;
			pChild->get_Class(&bstrClass);
				
			if(  CComBSTR(bstrClass) == CComBSTR(L"Group") )
			{
				BSTR bstrName;
				pChild->get_Name(&bstrName);
				GroupList.push_back(bstrName);
				c_GroupCombo.InsertString(++groupIndex,bstrName);
	            SysFreeString(bstrName);
			}
		}
	} //while
	pChild->Release();
	VariantClear(&var);
	CoUninitialize();
}

void PropertiesForm::FindMembership()
{
	list<CString>::iterator iter;
	for(iter=GroupList.begin();iter!=GroupList.end();++iter)
	{
		bool isMember = false;

		////////////////////// Converting UserName to char*
		char* userNameString;
		int userNameStringLength=userName.GetLength();
		userNameString=(char*)malloc(userNameStringLength+1);
		int ii;
		for( ii=0;ii<userNameStringLength;ii++)
			userNameString[ii]=(char)userName[ii];

		userNameString[ii]='\0';

		//////////////////////////////////////////////

		LPOLESTR adsPath = new OLECHAR[MAX_PATH];
		wcscpy(adsPath,L"WinNT://");
		wcscat(adsPath,this->machineName);
		wcscat(adsPath,L"/");
		wcscat(adsPath,*iter);
		CoInitialize(NULL);
		IADsGroup *pADsGroup=NULL;
		HRESULT hr;

		hr = ADsGetObject( adsPath, IID_IADsGroup, (void**) &pADsGroup );

		if( !SUCCEEDED(hr) )
		{
			LPOLESTR temp = new OLECHAR[MAX_PATH];
			wcscpy(temp,L"Unspecified error occurred while accessing the \"");
			wcscat(temp,*iter);
			wcscat(temp,L"\" group in \"");
			wcscat(temp,this->machineName);
			wcscat(temp,L"\" computer.");
			AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
			return;
		}
		
		IADsMembers *pMembers=NULL;
		hr = pADsGroup->Members(&pMembers);
	
		if( !SUCCEEDED(hr) )
		{
			LPOLESTR temp = new OLECHAR[MAX_PATH];
			wcscpy(temp,L"Unspecified error occurred while accessing the \"");
			wcscat(temp,*iter);
			wcscat(temp,L"\" group in \"");
			wcscat(temp,this->machineName);
			wcscat(temp,L"\" computer.");
			AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
			return;
		}

		IUnknown *pUnk;
		hr = pMembers->get__NewEnum(&pUnk);
		
		if( !SUCCEEDED(hr) )
		{
			LPOLESTR temp = new OLECHAR[MAX_PATH];
			wcscpy(temp,L"Unspecified error occurred while accessing the \"");
			wcscat(temp,*iter);
			wcscat(temp,L"\" group in \"");
			wcscat(temp,this->machineName);
			wcscat(temp,L"\" computer.");
			AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
			return;
		}
		
		IEnumVARIANT *pEnum;
		hr=pUnk->QueryInterface(IID_IEnumVARIANT,(void**)&pEnum);

		if( !SUCCEEDED(hr) )
		{
			LPOLESTR temp = new OLECHAR[MAX_PATH];
			wcscpy(temp,L"Unspecified error occurred while accessing the \"");
			wcscat(temp,*iter);
			wcscat(temp,L"\" group in \"");
			wcscat(temp,this->machineName);
			wcscat(temp,L"\" computer.");
			AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
			return;
		}
		
		VARIANT var;
		IADs *pADs=NULL;
		ULONG lFetch;
		

		VariantInit(&var);
		
		while( (pEnum->Next(1,&var,&lFetch)==S_OK) && lFetch==1 )
		{
			BSTR bstrName;
			IDispatch *pDisp=NULL;
			pDisp = V_DISPATCH(&var);
			pDisp->QueryInterface(IID_IADs,(void**)&pADs);
			pADs->get_Name(&bstrName);
			
			/////////////////////// Converting UserName to char*
			char* GroupuserNameString;
			int GroupuserNameStringLength;
			GroupuserNameStringLength=SysStringLen(bstrName);
			GroupuserNameString=(char*)malloc(GroupuserNameStringLength+1);
			ii=0;
			for( ii=0;ii<GroupuserNameStringLength;ii++)
				GroupuserNameString[ii]=(char)bstrName[ii];

			GroupuserNameString[ii]='\0';
			//LPCTSTR str = CA2W(ch);
			//////////////////////////////////////////////////
			
		

			//////// Checking whether user is local user or not //////
			for( ii=0;ii<userNameStringLength;ii++)
			{
				if( userNameString[ii] == GroupuserNameString[0] )
				{
					int k=ii+1;
					int j;
					for( j=1;j<GroupuserNameStringLength;j++,k++ )
					{
						if( GroupuserNameString[j] != userNameString[k]	)
						{
							isMember=false;
							break;
						}
					} // for

					if( j == GroupuserNameStringLength )
					{
						isMember = true;
						break;
					}
				} // if( userNameString[ii] == GroupuserNameString[0] )
			} // for
			/////////////////////////////////////////////////////////
	

			if( isMember )
			{
				CString cs=*iter;

				if( ! cs.Compare(L"Power Users") )
				{
					CButton *cb=(CButton*)GetDlgItem(IDC_RADIO1);
					cb->SetCheck(1);

					CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
					ccb->EnableWindow(0);
				}
				else if( ! cs.Compare(L"Users") )
				{
					CButton *cb=(CButton*)GetDlgItem(IDC_RADIO2);
					cb->SetCheck(1);

					CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
					ccb->EnableWindow(0);
				}
				else
				{
					CButton *cb=(CButton*)GetDlgItem(IDC_RADIO3);
					cb->SetCheck(1);
				
					CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
					ccb->EnableWindow(1);
					ccb->SelectString(0,*iter);
				}

				break;
			}  // if( isMember )
		//}  
			SysFreeString(bstrName);

			if( pDisp )
				pDisp->Release();
		}  // while 
		
		VariantClear(&var);

		if( pEnum )
			pEnum->Release();
		if( pUnk )
			pUnk->Release();

		if( isMember )
			break;
	} // for loop
}

	// Close Button
void PropertiesForm::OnBnClickedClose2button()
{
	this->EndDialog(IDOK);
} // Close Button

	// Close Button
void PropertiesForm::OnBnClickedClose1button()
{
	this->EndDialog(IDOK);
} // Close Button
	
	// Apply Button in Tab-1
void PropertiesForm::OnBnClickedApply1button()
{
	CString full;
	CString desc;
	c_FullNameTextBox.GetWindowText(full);
	c_DescriptionTextBox.GetWindowText(desc);

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
			AfxMessageBox(L"Unspecified error occurred while connecting with the server",MB_ICONINFORMATION,MB_OK);
			return;
		}

		BSTR bstrFullname=full.AllocSysString();
		hr=pADsUser->put_FullName(bstrFullname);
		hr=pADsUser->SetInfo();
		if( !SUCCEEDED(hr) )
		{
			if( hr == -2147024891 )
				AfxMessageBox(L"You dont have permission to modify User's details",MB_ICONINFORMATION,MB_OK);
			else
				AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);

			SysFreeString(bstrFullname);
			return;

		}

		BSTR bstrDescription=desc.AllocSysString();
		hr=pADsUser->put_Description(bstrDescription);
		hr=pADsUser->SetInfo();
		if( !SUCCEEDED(hr) )
		{
			if( hr == -2147024891 )
				AfxMessageBox(L"You dont have permission to modify User's details",MB_ICONINFORMATION,MB_OK);
			else
				AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);

			SysFreeString(bstrDescription);
			return;
		}
		
		this->canUpdate=true;
		AfxMessageBox(L"User Details are modified Succesfully",MB_ICONINFORMATION,MB_OK);

		SysFreeString(bstrFullname);
		SysFreeString(bstrDescription);
	}
	catch(...)
	{
		AfxMessageBox(L"Unspecified error occurred while connecting with the server",MB_ICONINFORMATION,MB_OK);
		return;
	}
} // Apply Button in Tab-1

	// Apply Button in Tab-2
void PropertiesForm::OnBnClickedApply2button()
{
	CButton *cb=(CButton*)GetDlgItem(IDC_RADIO1);
	if( cb->GetCheck() )
	{
		bool b=RemoveMembership();
		if( b )
			AddMembership(L"Power Users");
		return;
	}
	cb=(CButton*)GetDlgItem(IDC_RADIO2);
	if( cb->GetCheck() )
	{
		bool b =RemoveMembership();
		if( b )
			AddMembership(L"Users");
		return;
	}
	cb=(CButton*)GetDlgItem(IDC_RADIO3);
	if( cb->GetCheck() )
	{
		CString selectedGroup;
		c_GroupCombo.GetWindowText(selectedGroup);
		if( selectedGroup.IsEmpty() )
		{
			AfxMessageBox(L"Please Select any one of Group",MB_ICONINFORMATION,MB_OK);
			return;
		}
		bool b=RemoveMembership();
		if( b )
			AddMembership(selectedGroup);
		return;
	}

} // Apply Button in Tab-2

bool PropertiesForm::RemoveMembership()
{
	list<CString>::iterator iter;
	for(iter=GroupList.begin();iter!=GroupList.end();++iter)
	{
		bool isMember = false;
		
		LPOLESTR adsPath = new OLECHAR[MAX_PATH];
		wcscpy(adsPath,L"WinNT://");
		wcscat(adsPath,this->machineName);
		wcscat(adsPath,L"/");
		wcscat(adsPath,*iter);
		CoInitialize(NULL);
		IADsGroup *pADsGroup=NULL;
		HRESULT hr;
	
		if( this->defaultuser )
			hr = ADsGetObject( adsPath, IID_IADsGroup, (void**) &pADsGroup );
		else
			hr = ADsOpenObject( adsPath,this->user,this->pass,ADS_SECURE_AUTHENTICATION,IID_IADsGroup,(void**)&pADsGroup );

		if( !SUCCEEDED(hr) )
		{
			LPOLESTR temp = new OLECHAR[MAX_PATH];
			wcscpy(temp,L"Unspecified error occurred while accessing the group \"");
			wcscat(temp,*iter);
			wcscat(temp,L"\" ");
			AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
			return false;
		}
		
	/////////////////////////////////////////////////////////////////
		
		IADsUser* pADsUser=NULL;
		LPOLESTR adsPath1 = new OLECHAR[MAX_PATH];
		wcscpy(adsPath1,L"WinNT://");
		wcscat(adsPath1,this->machineName);
		wcscat(adsPath1,L"/");
		wcscat(adsPath1,this->userName);

		HRESULT hr1;
		if( this->defaultuser )
			hr1 = ADsGetObject( adsPath1, IID_IADsUser, (void**) &pADsUser );
		else
			hr1 = ADsOpenObject( adsPath1,this->user,this->pass,ADS_SECURE_AUTHENTICATION,IID_IADsUser, (void**) &pADsUser );

		if( !SUCCEEDED(hr1) )
		{
			AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);
			return false;
		}

		BSTR bstrUserPath;
		pADsUser->get_ADsPath(&bstrUserPath);

		VARIANT_BOOL vb=false;;
		hr1=pADsGroup->IsMember(bstrUserPath,&vb);
		
		if( vb  )  // if member
		{
			hr1=pADsGroup->Remove(bstrUserPath);

			if( !SUCCEEDED(hr1) )
			{
				if( hr1 == -2147024891 )
					AfxMessageBox(L"You dont have permission to modify User's details",MB_ICONINFORMATION,MB_OK);
				else
					AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);

			SysFreeString(bstrUserPath);
			return false;
			}
		}
	/////////////////////////////////////////////////////////////////

	} // for loop

	return true;
}

void PropertiesForm::AddMembership(CString groupName)
{
		LPOLESTR adsPath = new OLECHAR[MAX_PATH];
		wcscpy(adsPath,L"WinNT://");
		wcscat(adsPath,this->machineName);
		wcscat(adsPath,L"/");
		wcscat(adsPath,groupName);
		CoInitialize(NULL);
		IADsGroup *pADsGroup=NULL;
		HRESULT hr;

		if( this->defaultuser )
			hr = ADsGetObject( adsPath, IID_IADsGroup, (void**) &pADsGroup );
		else
			hr = ADsOpenObject( adsPath,this->user,this->pass,ADS_SECURE_AUTHENTICATION,IID_IADsGroup,(void**)&pADsGroup );

		if( !SUCCEEDED(hr) )
		{
			LPOLESTR temp = new OLECHAR[MAX_PATH];
			wcscpy(temp,L"Unspecified error occurred while accessing the group \"");
			wcscat(temp,groupName);
			wcscat(temp,L"\" ");
			AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
			return ;
		}

		
	/////////////////////////////////////////////////////////////////
		
		IADsUser* pADsUser=NULL;
		LPOLESTR adsPath1 = new OLECHAR[MAX_PATH];
		wcscpy(adsPath1,L"WinNT://");
		wcscat(adsPath1,this->machineName);
		wcscat(adsPath1,L"/");
		wcscat(adsPath1,this->userName);

		HRESULT hr1;
		if( this->defaultuser )
			hr1 = ADsGetObject( adsPath1, IID_IADsUser, (void**) &pADsUser );
		else
			hr1 = ADsOpenObject( adsPath1,this->user,this->pass,ADS_SECURE_AUTHENTICATION,IID_IADsUser, (void**) &pADsUser );

		if( !SUCCEEDED(hr1) )
		{ 
			AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);
			return ;
		}
		
		BSTR bstrUserPath;
		pADsUser->get_ADsPath(&bstrUserPath);

		hr1=pADsGroup->Add(bstrUserPath);

		if( !SUCCEEDED(hr1) )
		{
			if( hr1 == -2147024891 )
				AfxMessageBox(L"You dont have permission to modify User's details",MB_ICONINFORMATION,MB_OK);
			else
				AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);

			return;
		}
	
		this->canUpdate=true;
		LPOLESTR temp1 = new OLECHAR[MAX_PATH];
		wcscpy(temp1,L"User is added in \"");
		wcscat(temp1,groupName);
		wcscat(temp1,L"\" Group");
		AfxMessageBox(temp1,MB_ICONINFORMATION,MB_OK);
	/////////////////////////////////////////////////////////////////
}

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         