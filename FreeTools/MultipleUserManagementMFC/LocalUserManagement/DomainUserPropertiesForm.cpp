// DomainUserPropertiesForm.cpp : implementation file
//

#include "stdafx.h"
#include "LocalUserManagement.h"
#include "DomainUserPropertiesForm.h"

#include "IADs.h"
#include "Adshlp.h"



// DomainUserPropertiesForm dialog

IMPLEMENT_DYNAMIC(DomainUserPropertiesForm, CDialog)

DomainUserPropertiesForm::DomainUserPropertiesForm(CWnd* pParent /*=NULL*/)
	: CDialog(DomainUserPropertiesForm::IDD, pParent)
{

}

DomainUserPropertiesForm::DomainUserPropertiesForm(CString str1,CString str2,CString user,CString pass,bool defaultuser)
	: CDialog(DomainUserPropertiesForm::IDD, NULL)
{
	this->userName=str1;
	this->machineName=str2;
	this->defaultuser=defaultuser;
	this->user=user;
	this->pass=pass;
	this->canUpdate=false;
}


DomainUserPropertiesForm::~DomainUserPropertiesForm()
{
}

void DomainUserPropertiesForm::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_COMBO1, c_GroupCombo);
}


BEGIN_MESSAGE_MAP(DomainUserPropertiesForm, CDialog)
	ON_BN_CLICKED(IDC_Apply2Button, &DomainUserPropertiesForm::OnBnClickedApply2button)
	ON_BN_CLICKED(IDC_Close2Button, &DomainUserPropertiesForm::OnBnClickedClose2button)
	ON_BN_CLICKED(IDC_RADIO1, &DomainUserPropertiesForm::OnBnClickedRadio1)
	ON_BN_CLICKED(IDC_RADIO2, &DomainUserPropertiesForm::OnBnClickedRadio2)
	ON_BN_CLICKED(IDC_RADIO3, &DomainUserPropertiesForm::OnBnClickedRadio3)
	ON_CBN_SELCHANGE(IDC_COMBO1, &DomainUserPropertiesForm::OnCbnSelchangeCombo1)
END_MESSAGE_MAP()

BOOL DomainUserPropertiesForm::OnInitDialog()
{
	CDialog::OnInitDialog();
	FormLoad();
	GroupMembershipLoad();
	return true;
}

		// Form Load
void DomainUserPropertiesForm::FormLoad()
{
	SetWindowPos(NULL,0,0,342,323,SWP_NOZORDER | SWP_NOMOVE );
	LPOLESTR name = new OLECHAR[MAX_PATH];
	wcscpy(name,this->userName);
	wcscat(name,L" Properties");
	SetWindowText(name);

	int x=12;

	CStatic *cs=(CStatic*)GetDlgItem(IDC_Label1);
	cs->MoveWindow(32,27,258,13,1);

	cs=(CStatic*)GetDlgItem(IDC_Label2);
	cs->MoveWindow(204,76,105,13,1);

	cs=(CStatic*)GetDlgItem(IDC_Label3);
	cs->MoveWindow(45,98,241,13,1);

	cs=(CStatic*)GetDlgItem(IDC_Label4);
	cs->MoveWindow(47,112,231,13,1);

	cs=(CStatic*)GetDlgItem(IDC_Label5);
	cs->MoveWindow(218,132,72,13,1);

	cs=(CStatic*)GetDlgItem(IDC_Label6);
	cs->MoveWindow(44,151,246,13,1);

	cs=(CStatic*)GetDlgItem(IDC_Label7);
	cs->MoveWindow(47,166,261,13,1);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
	ccb->MoveWindow(145,191,164,21,1);
	
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply2Button);
	cb->MoveWindow(125,247,75,23,1);
	cb->EnableWindow(0);

	cb=(CButton*)GetDlgItem(IDC_Close2Button);
	cb->MoveWindow(233,247,75,23,1);

	cb=(CButton*)GetDlgItem(IDC_RADIO1);
	cb->MoveWindow(26,74,99,18,1);

	cb=(CButton*)GetDlgItem(IDC_RADIO2);
	cb->MoveWindow(26,130,104,18,1);

	cb=(CButton*)GetDlgItem(IDC_RADIO3);
	cb->MoveWindow(26,195,62,18,1);

}  // Form Load

void DomainUserPropertiesForm::OnOK()
{
	OnBnClickedApply2button();
}

	// Apply Button
void DomainUserPropertiesForm::OnBnClickedApply2button()
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

} // Apply Button

void DomainUserPropertiesForm::GroupMembershipLoad()
{
	LoadGroups();
	FindMembership();
}

void DomainUserPropertiesForm::LoadGroups()
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

void DomainUserPropertiesForm::FindMembership()
{
	list<CString>::iterator iter;
	for(iter=GroupList.begin();iter!=GroupList.end();++iter)
	{
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
			wcscpy(temp,L"problem In 'FindMembership' function .. in AdsGetObject ");
			wcscat(temp,L".......");
			wcscat(temp,*iter);
			AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
			return ;
		}
		
		//IADsMembers *pMembers=NULL;
		//hr = pADsGroup->Members(&pMembers);
	/////////////////////////////////////////////////////////////////
		
		IADsUser* pADsUser=NULL;
		LPOLESTR adsPath1 = new OLECHAR[MAX_PATH];
		wcscpy(adsPath1,L"WinNT://");
		wcscat(adsPath1,this->userName);

		HRESULT hr1;
		hr1 = ADsGetObject( adsPath1, IID_IADsUser, (void**) &pADsUser );
		
		BSTR bstrUserPath;
		pADsUser->get_ADsPath(&bstrUserPath);

		VARIANT_BOOL vb=false;;
		hr1=pADsGroup->IsMember(bstrUserPath,&vb);

		if( vb )
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
		}
	} // for loop
}

bool DomainUserPropertiesForm::RemoveMembership()
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
//		wcscat(adsPath1,this->machineName);
//		wcscat(adsPath1,L"/");
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

			hr1=pADsGroup->SetInfo();

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

void DomainUserPropertiesForm::AddMembership(CString groupName)
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
//		wcscat(adsPath1,this->machineName);
//		wcscat(adsPath1,L"/");
		wcscat(adsPath1,this->userName);

		HRESULT hr1;
		if( this->defaultuser )
			hr1 = ADsGetObject( adsPath1, IID_IADsUser, (void**) &pADsUser );
		else
			hr1 = ADsOpenObject( adsPath1,this->user,this->pass,ADS_SECURE_AUTHENTICATION,IID_IADsUser, (void**) &pADsUser );

		if( !SUCCEEDED(hr1) )
		{
			AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);
			return;
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
		else{
			
		}
	
		this->canUpdate=true;
		LPOLESTR temp1 = new OLECHAR[MAX_PATH];
		wcscpy(temp1,L"User is added in \"");
		wcscat(temp1,groupName);
		wcscat(temp1,L"\" Group");
		AfxMessageBox(temp1,MB_ICONINFORMATION,MB_OK);
	/////////////////////////////////////////////////////////////////

}

	// Close Button
void DomainUserPropertiesForm::OnBnClickedClose2button()
{
	this->EndDialog(IDOK);
}  // Close Button

void DomainUserPropertiesForm::OnBnClickedRadio1()
{
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply2Button);
	cb->EnableWindow(1);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
	ccb->EnableWindow(0);
}

void DomainUserPropertiesForm::OnBnClickedRadio2()
{
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply2Button);
	cb->EnableWindow(1);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
	ccb->EnableWindow(0);
}

void DomainUserPropertiesForm::OnBnClickedRadio3()
{
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply2Button);
	cb->EnableWindow(1);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
	ccb->EnableWindow(1);
}

void DomainUserPropertiesForm::OnCbnSelchangeCombo1()
{
	CButton* cb=(CButton*)GetDlgItem(IDC_Apply2Button);
	cb->EnableWindow(1);

	cb=(CButton*)GetDlgItem(IDC_RADIO3);
	cb->SetCheck(1);
}
