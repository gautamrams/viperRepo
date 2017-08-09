// LocalUserManagementDlg.cpp : implementation file
//



#include "stdafx.h"
#include "LocalUserManagement.h"
#include "LocalUserManagementDlg.h"
#include "ResetPasswordForm.h"
#include "PropertiesForm.h"
#include "DomainUserPropertiesForm.h"
#include "AddForm.h"
#include "Dsgetdc.h"
#include "multipleuseraddform.h"
#include<fstream>
using namespace std;

//#include "comdef.h"
//#include "afxcmn.h"
//#include "comdef.h"
//#include "afxwin.h"

#include <Dsclient.h>
//#include<objbase.h>

/*
#include "IADs.h"
#include "ADshlp.h"
*/
#include<list>
using namespace std;

typedef struct THREADSTRUCT
{
    CLocalUserManagementDlg*    _this;
} THREADSTRUCT;


#ifdef _DEBUG
#define new DEBUG_NEW
#endif

/*
list<CString> AllComputersList;
list<CString> ComputerList;
list<list<list<CString>>> UserList;
CString UserName;
CString Password;
CString DomainName;
bool defaultUser = true;
*/

// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Implementation
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
END_MESSAGE_MAP()


// CLocalUserManagementDlg dialog


CLocalUserManagementDlg::CLocalUserManagementDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CLocalUserManagementDlg::IDD, pParent)
	, c_Picture3(0)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CLocalUserManagementDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_COMBO1, c_DomainName);
	DDX_Control(pDX, IDC_EDIT1, c_UserName);
	DDX_Control(pDX, IDC_EDIT2, c_Password);
	DDX_Control(pDX, IDC_LIST2, c_ListView1);
	DDX_Control(pDX, IDC_LIST1, c_ListView2);
	DDX_Control(pDX, IDC_COMBO2, c_ComputerCombo);
	DDX_Control(pDX, IDC_COMBO3, c_GroupCombo);
	DDX_Control(pDX, IDC_PICTURECONTROL1, c_Picture1);
	DDX_Control(pDX, IDC_InformationLabel, c_InfoLabel);
	DDX_Control(pDX, IDC_StopButton, c_StopButton);
	DDX_Control(pDX, IDC_PROGRESS1, c_Progress1);
	DDX_Control(pDX, IDC_LocalUsersButton, c_LocalUsersButton);
	DDX_Control(pDX, IDC_Information2Label, c_Info2Label);
	DDX_Control(pDX, IDC_PROGRESS2, c_Progress2);
	DDX_Control(pDX, IDC_Stop2Button, c_Stop2Button);
	DDX_Control(pDX, IDC_COMPUTERLABEL, c_ComputerLabel);
	DDX_Control(pDX, IDC_GROUPLABEL, c_GroupLabel);
	DDX_Control(pDX, IDC_PropertiesButton, c_PropertiesButton);
	DDX_Control(pDX, IDC_ResetPasswordButton, c_ResetPasswordButton);
	DDX_Control(pDX, IDC_AddButton, c_AddButton);
	DDX_Control(pDX, IDC_DeleteButton, c_DeleteButton);
	DDX_Control(pDX, IDC_EnableButton, c_EnableButton);
	DDX_Control(pDX, IDC_DisableButton, c_DisableButton);
	DDX_Control(pDX, IDC_TotalComputersLabel, c_TotalComputersLabel);
	DDX_Control(pDX, IDC_SearchPictureBox, c_SearchPictureBox);
	DDX_Control(pDX, IDC_SearchTextBox, c_SearchTextBox);
	DDX_Control(pDX, IDC_ReportLabel, c_ReportLabel);
	DDX_Control(pDX, IDC_LinkLabel, c_LinkLabel);
	DDX_Control(pDX, IDC_Info1Label, c_Info1Label);
	DDX_Control(pDX, IDC_Info2Label, c_Information2Label);
	DDX_Control(pDX, IDC_Info3Label, c_Information3Label);
	DDX_Control(pDX, IDC_Info4Label, c_Information4Label);
	DDX_Control(pDX, IDC_PICTURECONTROL2, c_Picture2);
	DDX_Control(pDX, IDC_PICTURECONTROL3, c_Picture4);
	DDX_Control(pDX, IDC_PICTURECONTROL4, c_Picture5);
	DDX_Control(pDX, IDC_SelectAll1Button, c_SelectAll1Button);
	DDX_Control(pDX, IDC_SelectAll2Button, c_SelectAll2Button);
	DDX_Control(pDX, IDC_DeSelectAll1Button, c_DeSelectAll1Button);
	DDX_Control(pDX, IDC_DeSelectAll2Button, c_DeSelectAll2Button);
	DDX_Control(pDX, IDC_TotalUsersLabel, c_TotalUsersLabel);
	DDX_Control(pDX, IDC_Search2TextBox, c_Search2TextBox);
	DDX_Control(pDX, IDC_Search2PictureBox, c_Search2PictureBox);
}

BEGIN_MESSAGE_MAP(CLocalUserManagementDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDC_BUTTON1, &CLocalUserManagementDlg::OnBnClickedButton1)
	ON_BN_CLICKED(IDC_LocalUsersButton, &CLocalUserManagementDlg::OnBnClickedLocalusersbutton)
	//ON_BN_CLICKED(IDC_BUTTON3, &CLocalUserManagementDlg::OnBnClickedButton3)
	ON_BN_CLICKED(IDC_PropertiesButton, &CLocalUserManagementDlg::OnBnClickedPropertiesbutton)
	ON_CBN_SELCHANGE(IDC_COMBO2, &CLocalUserManagementDlg::OnCbnSelchangeCombo2)
	ON_CBN_SELCHANGE(IDC_COMBO3, &CLocalUserManagementDlg::OnCbnSelchangeCombo3)
	ON_BN_CLICKED(IDC_ResetPasswordButton, &CLocalUserManagementDlg::OnBnClickedResetpasswordbutton)
	ON_STN_CLICKED(IDC_PICTURECONTROL1, &CLocalUserManagementDlg::OnStnClickedPicturecontrol1)
	ON_STN_CLICKED(IDC_PICTURECONTROL2, &CLocalUserManagementDlg::OnStnClickedPicturecontrol2)
	ON_BN_CLICKED(IDC_AddButton, &CLocalUserManagementDlg::OnBnClickedAddbutton)
	ON_BN_CLICKED(IDC_EnableButton, &CLocalUserManagementDlg::OnBnClickedEnablebutton)
	ON_BN_CLICKED(IDC_DisableButton, &CLocalUserManagementDlg::OnBnClickedDisablebutton)
	ON_BN_CLICKED(IDC_DeleteButton, &CLocalUserManagementDlg::OnBnClickedDeletebutton)
	ON_BN_CLICKED(IDC_StopButton, &CLocalUserManagementDlg::OnBnClickedStopbutton)
	ON_BN_CLICKED(IDC_Stop2Button, &CLocalUserManagementDlg::OnBnClickedStop2button)
	ON_BN_CLICKED(IDC_ClearButton, &CLocalUserManagementDlg::OnBnClickedClearbutton)
	ON_STN_CLICKED(IDC_SearchPictureBox, &CLocalUserManagementDlg::OnStnClickedSearchpicturebox)
	ON_STN_CLICKED(IDC_HOMEPICTURECONTROL, &CLocalUserManagementDlg::OnStnClickedHomepicturecontrol)
	ON_BN_CLICKED(IDC_SelectAll1Button, &CLocalUserManagementDlg::OnBnClickedSelectall1button)
	ON_BN_CLICKED(IDC_DeSelectAll1Button, &CLocalUserManagementDlg::OnBnClickedDeselectall1button)
	ON_BN_CLICKED(IDC_SelectAll2Button, &CLocalUserManagementDlg::OnBnClickedSelectall2button)
	ON_BN_CLICKED(IDC_DeSelectAll2Button, &CLocalUserManagementDlg::OnBnClickedDeselectall2button)
	ON_STN_CLICKED(IDC_Search2PictureBox, &CLocalUserManagementDlg::OnStnClickedSearch2picturebox)
	ON_BN_CLICKED(Export, &CLocalUserManagementDlg::OnBnClickedExport)
	ON_BN_CLICKED(IDC_BUTTON3, &CLocalUserManagementDlg::OnBnClickedButton3)
	
END_MESSAGE_MAP()


// CLocalUserManagementDlg message handlers

BOOL CLocalUserManagementDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here

	 //////////////////////////////////////// Initial Parts

	MainFormLoad();
	
	////////////////////////////////////////
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CLocalUserManagementDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CLocalUserManagementDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CLocalUserManagementDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

UINT GetComputersThread( LPVOID Param )
{
	THREADSTRUCT* thisObject = (THREADSTRUCT*)Param;

	LPOLESTR adsPath = new OLECHAR[MAX_PATH];
	wcscpy(adsPath,L"LDAP://");
	wcscat(adsPath, thisObject->_this->DomainName);

	thisObject->_this->c_Progress1.SetPos(10);

	CoInitialize(NULL);
	IADsContainer *pCont=NULL;
	IDirectorySearch *pDSSearch=NULL;
		
	/*
	int cou=c_ListView1.GetItemCount();

					char c[10];
					itoa(cou,c,10);
					CString MFCString;
					MFCString = c;
	AfxMessageBox(MFCString);
*/
	
	HRESULT hr;
	thisObject->_this->defaultUser=true;	

	if( thisObject->_this->UserName.IsEmpty() )
		hr = ADsGetObject(adsPath,IID_IDirectorySearch,(void**)&pDSSearch);
	else
	{
		thisObject->_this->defaultUser=false;
		hr = ADsOpenObject(adsPath,thisObject->_this->UserName,thisObject->_this->Password,ADS_SECURE_AUTHENTICATION,IID_IDirectorySearch,(void **)&pDSSearch);
	}

	if( !SUCCEEDED(hr) )
	{
		if( hr == -2147023570 )
			AfxMessageBox(L"Login failure... Bad UserName or Password ",MB_ICONINFORMATION,MB_OK);
		else if( hr == -2147016646 )
			AfxMessageBox(L"Network path was not found... Please, enter correct Domain name",MB_ICONINFORMATION,MB_OK);
		else
			AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);

		CoUninitialize();

		thisObject->_this->c_LocalUsersButton.ShowWindow(SW_SHOW);
		thisObject->_this->c_InfoLabel.ShowWindow(SW_HIDE);
		thisObject->_this->c_StopButton.ShowWindow(SW_HIDE);
		thisObject->_this->c_Progress1.ShowWindow(SW_HIDE);

		thisObject->_this->isThreadGoing = false;
		return TRUE;
	}
	
	ADS_SEARCHPREF_INFO SearchPrefs[1];
	SearchPrefs[0].dwSearchPref = ADS_SEARCHPREF_PAGESIZE ;
	SearchPrefs[0].vValue.dwType = ADSTYPE_INTEGER;
	SearchPrefs[0].vValue.Integer = 1000;
	LPWSTR attr[]={L"Name",L"distinguishedName"};
	ADS_SEARCH_HANDLE hSearch;
	DWORD dwCount=sizeof(attr)/sizeof(LPWSTR);

	hr = pDSSearch->SetSearchPreference(SearchPrefs, 1);
	hr = pDSSearch->ExecuteSearch(L"(&(objectCategory=computer)(objectClass=computer))",attr,dwCount,&hSearch);

	if( !SUCCEEDED(hr) )
	{
		AfxMessageBox(L"Unspecified error occurred while accesing the Domain",MB_ICONINFORMATION,MB_OK);
		pDSSearch->Release();

		CoUninitialize();

		thisObject->_this->c_LocalUsersButton.ShowWindow(SW_SHOW);
		thisObject->_this->c_InfoLabel.ShowWindow(SW_HIDE);
		thisObject->_this->c_StopButton.ShowWindow(SW_HIDE);
		thisObject->_this->c_Progress1.ShowWindow(SW_HIDE);

		thisObject->_this->isThreadGoing = false;
		return TRUE;
	}

	ADS_SEARCH_COLUMN col;
	int m=0;
	while( pDSSearch->GetNextRow(hSearch)==S_OK )
	{
		if( thisObject->_this->c_Progress1.GetPos() > 90 )
			thisObject->_this->c_Progress1.SetPos(0);

		thisObject->_this->c_Progress1.SetPos(thisObject->_this->c_Progress1.GetPos()+3);
		if( thisObject->_this->toBeStopped == true )
			break;

		thisObject->_this->c_ListView1.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
		for( int i=0;i<dwCount;i++)
		{
			hr = pDSSearch->GetColumn(hSearch,attr[i],&col);
			if( SUCCEEDED(hr) )
			{
				thisObject->_this->c_ListView1.SetItemText(m,i,col.pADsValues->CaseExactString);
				if( i==0)
					thisObject->_this->AllComputersList.push_back(col.pADsValues->CaseExactString);
				else
					thisObject->_this->AllComputersLocationList.push_back(col.pADsValues->CaseExactString);
			}
		}
		m=m+1;

		////////////////////////////////////////////////
		int count=thisObject->_this->c_ListView1.GetItemCount();
		char c[10];
		itoa(count,c,10);
		CString MFCString;
		MFCString = c;
	
		LPOLESTR str = new OLECHAR[MAX_PATH];
		wcscpy(str,L"Total Computers : ");
		wcscat(str,MFCString);
		thisObject->_this->c_TotalComputersLabel.SetWindowText(str);
		////////////////////////////////////////////////
	} // while

	pDSSearch->CloseSearchHandle(hSearch);
	pDSSearch->Release();

	if( pCont )
	{
		pCont->Release();	
	}

	CoUninitialize();


	thisObject->_this->c_LocalUsersButton.ShowWindow(SW_SHOW);
	thisObject->_this->c_InfoLabel.ShowWindow(SW_HIDE);
	thisObject->_this->c_StopButton.ShowWindow(SW_HIDE);
	thisObject->_this->c_Progress1.ShowWindow(SW_HIDE);

	if( thisObject->_this->c_ListView1.GetItemCount() > 0 )
	{
		thisObject->_this->c_SearchTextBox.SetWindowText(L"");
		thisObject->_this->c_SearchTextBox.ShowWindow(SW_SHOW);
		thisObject->_this->c_SearchPictureBox.ShowWindow(SW_SHOW);
		thisObject->_this->c_SelectAll1Button.ShowWindow(SW_SHOW);
		thisObject->_this->c_DeSelectAll1Button.ShowWindow(SW_SHOW);

		thisObject->_this->SortingListView();
	}
	
	thisObject->_this->isThreadGoing = false;
	return TRUE;
}

		// Get Computers Button
void CLocalUserManagementDlg::OnBnClickedButton1()
{
		ofstream outFile;
		
	outFile.open("c:\\Windows\\Temp\\LocalUserManagementReports.txt",ios::out) ;
	outFile.close();

//	STARTUPINFO startup_info = {0};
////				startup_info.cb = sizeof startup_info;
//				PROCESS_INFORMATION pi = {0};
				
		//		CreateProcess( _T("C:\\WINDOWS\\system32\\notepad.exe"),_T("C:\\WINDOWS\\system32\\notepad.exe c:\\Windows\\Temp\\LocalUserManagementReports.txt"),NULL,NULL,FALSE,0,NULL,NULL,&startup_info,&pi) ;
				
//**********************************************
	if( this->isThreadGoing == true )
	{
		AfxMessageBox(L"Loading is going on... Please wait for a while.",MB_ICONINFORMATION,MB_OK);
		return;
	}
	UserName.Empty();
	Password.Empty();
	DomainName.Empty();

	c_Picture1.ShowWindow(SW_HIDE);
	c_Picture2.ShowWindow(SW_HIDE);
	c_Picture4.ShowWindow(SW_SHOW);
	c_Picture5.ShowWindow(SW_SHOW);

	c_DomainName.GetWindowText(DomainName);

	c_UserName.GetWindowText(UserName);
	c_Password.GetWindowText(Password);

	c_ListView1.DeleteAllItems();
	AllComputersList.clear();
	AllComputersLocationList.clear();
	ComputerList.clear();

	c_SearchTextBox.ShowWindow(SW_HIDE);
	c_SearchPictureBox.ShowWindow(SW_HIDE);
	c_SelectAll1Button.ShowWindow(SW_HIDE);
	c_DeSelectAll1Button.ShowWindow(SW_HIDE);

	LVCOLUMN lvcolumn;
	lvcolumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvcolumn.fmt = LVCFMT_LEFT;
	
	lvcolumn.cx = 288;
	lvcolumn.pszText = L"Distinguished Name";
	c_ListView1.InsertColumn(1,&lvcolumn);
	c_ListView1.DeleteColumn(2);

	//////////////////////////////////////////////////
	int count1=c_ListView1.GetItemCount();
	char c1[10];
	itoa(count1,c1,10);
	CString MFCString1;
	MFCString1 = c1;
	
	LPOLESTR str1 = new OLECHAR[MAX_PATH];
	wcscpy(str1,L"Total Computers : ");
	wcscat(str1,MFCString1);
	c_TotalComputersLabel.SetWindowText(str1);
	//////////////////////////////////////////////////

	if( DomainName.IsEmpty() )
	{
		AfxMessageBox(L"Please Enter the Domain Name",MB_ICONINFORMATION,MB_OK);
		return;
	}

	if( (UserName.IsEmpty() && !Password.IsEmpty()) || (!UserName.IsEmpty() && Password.IsEmpty()) )
	{
		AfxMessageBox(L"Please Enter User Name & Password correctly",MB_ICONINFORMATION,MB_OK);
		return;
	}

	this->c_LocalUsersButton.ShowWindow(SW_HIDE);
	this->c_InfoLabel.SetWindowText(L"Loading Computer List...");
	this->c_InfoLabel.ShowWindow(SW_SHOW);
	this->c_StopButton.ShowWindow(SW_SHOW);
	this->c_Progress1.ShowWindow(SW_SHOW);

	this->toBeStopped=false;
	this->isThreadGoing=true;

	THREADSTRUCT *_param = new THREADSTRUCT;
    _param->_this = this;
    AfxBeginThread (GetComputersThread, _param);
} // Get Computers Button


			// Get Local Users Button
void CLocalUserManagementDlg::OnBnClickedLocalusersbutton()
{
	if( this->isThreadGoing == true)
	{
		AfxMessageBox(L"Loading is going on... Please wait for a while.",MB_ICONINFORMATION,MB_OK);
		return;
	}
	ComputerList.clear();
	ComputerListIndex.clear();

	c_Picture1.ShowWindow(SW_HIDE);
	c_Picture2.ShowWindow(SW_HIDE);
	c_Picture4.ShowWindow(SW_SHOW);
	c_Picture5.ShowWindow(SW_SHOW);

	c_ListView1.DeleteColumn(1);
	LVCOLUMN lvcolumn;
	lvcolumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvcolumn.fmt = LVCFMT_LEFT;
	
	lvcolumn.cx = 288;
	lvcolumn.pszText = L"  Status";
	c_ListView1.InsertColumn(1,&lvcolumn);

	for( int i=0;i<c_ListView1.GetItemCount();i++)
	{
		bool bChecked = c_ListView1.GetCheck(i);
		this->c_ListView1.SetItemText(i,1,L"------");
		if( bChecked == true )
		{
			CString str = c_ListView1.GetItemText(i,0);
 
			ComputerList.push_back(str);
			ComputerListIndex.push_back(i);
		}
	}
	
  // CEdit *editBox = (CEdit *) GetDlgItem(IDC_ImportTextBox);
  //  CString str;
  //  editBox->GetWindowTextW(str);
	if( ComputerList.size() == 0)
		AfxMessageBox(L"Please Select atleast one Computer",MB_ICONINFORMATION,MB_OK);
	else
	{
		if(getInfo()){
		if(getFileDate()==0){
	
		addDb();
	}
	else if(startApp())
		updateDb();
	writeToFile(12);
	}
		RetrieveLocalUserDetails();
	}
}  // Get Local Users Button



UINT DisplayGroupUsersThread( LPVOID Param )
{
	THREADSTRUCT* thisObject = (THREADSTRUCT*)Param;
	thisObject->_this->c_Progress2.SetPos(0);

	///////////////////////////////////////////////////////////////////
	int selectedComputerIndex=thisObject->_this->c_ComputerCombo.GetCurSel();
		
	list<CString> tempList;
	if( selectedComputerIndex == 0 )
	{
		for(list<CString>::iterator iter=thisObject->_this->ComputerList.begin();iter!=thisObject->_this->ComputerList.end();++iter)
		{
			tempList.push_back(*iter);
		}
	}
	else
	{
		CString selectedComputerText;
		thisObject->_this->c_ComputerCombo.GetLBText(selectedComputerIndex,selectedComputerText);
		tempList.push_back(selectedComputerText);
	}
	///////////////////////////////////////////////////////////////////
	
		// DisplayGroupUsers function..

	int current=1;
	int size=tempList.size();

	int selectedGroupIndex = thisObject->_this->c_GroupCombo.GetCurSel();
	CString selectedGroupText;
	thisObject->_this->c_GroupCombo.GetLBText(selectedGroupIndex,selectedGroupText);
	int m=0;

	for(list<CString>::iterator iter=tempList.begin();iter!=tempList.end();++iter)
	{

		if( thisObject->_this->toBeStopped == true )
			break;
		///////////////////////////////////////////////////////////
		LPOLESTR infoText = new OLECHAR[MAX_PATH];
		wcscpy(infoText,L"Loading Group members from \"");
		wcscat(infoText,iter->GetString());
		wcscat(infoText,L"\"");
		thisObject->_this->c_Info2Label.SetWindowText(infoText);
		thisObject->_this->c_Info2Label.GetParent()->UpdateWindow();
		int percentage=(int)((100/size)*current);
		thisObject->_this->c_Progress2.SetPos(thisObject->_this->c_Progress2.GetPos()+percentage);
		//thisObject->_this->c_Progress1.StepIt();
		///////////////////////////////////////////////////////////


		////////////////////// Converting ComputerName to char*
		char* computerNameString;
		CString cs = *iter;
		int computerNameStringLength=cs.GetLength();
		computerNameString=(char*)malloc(computerNameStringLength+1);
		int ii;
		for( ii=0;ii<computerNameStringLength;ii++)
			computerNameString[ii]=(char)cs[ii];

		computerNameString[ii]='\0';
		//////////////////////////////////////////////

		LPOLESTR adsPath = new OLECHAR[MAX_PATH];
		wcscpy(adsPath,L"WinNT://");
		wcscat(adsPath,*iter);
		wcscat(adsPath,L"/");
		wcscat(adsPath,selectedGroupText);
		CoInitialize(NULL);
		IADsGroup *pADsGroup=NULL;
		HRESULT hr;

		hr = ADsGetObject( adsPath, IID_IADsGroup, (void**) &pADsGroup );

		if( !SUCCEEDED(hr) )
		{
			LPOLESTR errorText = new OLECHAR[wcslen(iter->GetString())+28];
			wcscpy(errorText,L"Network Error Occured for: ");
			wcscat(errorText,iter->GetString());
			thisObject->_this->c_Info2Label.SetWindowText(errorText);
			
			//return;
			continue;
		}
		
		IADsMembers *pMembers=NULL;
		hr = pADsGroup->Members(&pMembers);
	
		if( !SUCCEEDED(hr) )
		{
			AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);
			//return;
			continue;
		}

		IUnknown *pUnk;
		hr = pMembers->get__NewEnum(&pUnk);
		
		if( !SUCCEEDED(hr) )
		{
			AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);
			//return;
			continue;
		}
		
		IEnumVARIANT *pEnum;
		hr=pUnk->QueryInterface(IID_IEnumVARIANT,(void**)&pEnum);

		if( !SUCCEEDED(hr) )
		{
			AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);
			//return;
			continue;
		}
		
		VARIANT var;
		IADs *pADs=NULL;
		ULONG lFetch;
		
		VariantInit(&var);
		
		while( (pEnum->Next(1,&var,&lFetch)==S_OK) && lFetch==1 )
		{
			BSTR bstrName;
			VARIANT var1,var2;
			
			VariantInit(&var1);
			VariantInit(&var1);
			IDispatch *pDisp=NULL;
			pDisp = V_DISPATCH(&var);
			pDisp->QueryInterface(IID_IADs,(void**)&pADs);
			pADs->get_ADsPath(&bstrName);
			
			/////////////////////// Converting UserName to char*
			char* userNameString;
			int userNameStringLength;
			userNameStringLength=SysStringLen(bstrName);
			userNameString=(char*)malloc(userNameStringLength+1);
			ii=0;
			for( ii=0;ii<userNameStringLength;ii++)
				userNameString[ii]=(char)bstrName[ii];

			userNameString[ii]='\0';
			//LPCTSTR str = CA2W(ch);
			//////////////////////////////////////////////////
			
			bool isLocalUser = false;

			//////// Checking whether user is local user or not //////
			for( ii=0;ii<userNameStringLength;ii++)
			{
				if( userNameString[ii] == computerNameString[0] )
				{
					int k=ii+1;
					int j;
					for( j=1;j<computerNameStringLength;j++,k++ )
					{
						if( computerNameString[j] != userNameString[k]	)
						{
							isLocalUser=false;
							break;
						}
					} // for

					if( j == computerNameStringLength )
					{
						isLocalUser = true;
						break;
					}
				} // if( userNameString[ii] == computerNameString[0] )
			} // for
			/////////////////////////////////////////////////////////

			thisObject->_this->c_ListView2.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
			if( isLocalUser )
			{
				int slashIndex;
				for( slashIndex=userNameStringLength-1 ; slashIndex >=0 ; slashIndex -- )
					if( userNameString[slashIndex] == '/' )
						break;

				char* newUserNameString;
				int newUserNameStringLength;
				newUserNameStringLength=userNameStringLength - slashIndex;
				newUserNameString=(char*)malloc(newUserNameStringLength+1);
				int i=0;
				for( ii=slashIndex+1;ii<userNameStringLength;ii++,i++)
					newUserNameString[i]=userNameString[ii];

				newUserNameString[i]='\0';
				
				LPCTSTR newName = CA2W(newUserNameString);
				thisObject->_this->c_ListView2.SetItemText(m,0,newName);
				pADs->Get(L"fullname",&var1);
				pADs->Get(L"description",&var2);
				thisObject->_this->c_ListView2.SetItemText(m,1,iter->GetString());
				thisObject->_this->c_ListView2.SetItemText(m,2,var1.bstrVal);
				thisObject->_this->c_ListView2.SetItemText(m,3,var2.bstrVal);
				//thisObject->_this->c_ListView2.SetItemText(m,3,iter->GetString());
				
				//////////////////////// Adding into SearchList....
				list<CString> tempSearchList;
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,0));
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,1));
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,2));
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,3));
				thisObject->_this->SearchList.push_back(tempSearchList);
				//////////////////////////////////////////////////////////////

			}
			else
			{
				int slashIndex;
				for( slashIndex=0 ; slashIndex < userNameStringLength; slashIndex ++ )
					if( userNameString[slashIndex] == '/' )
						break;

				char* newUserNameString;
				int newUserNameStringLength;
				newUserNameStringLength=userNameStringLength - slashIndex;
				newUserNameString=(char*)malloc(newUserNameStringLength+1);

				int i=0;
				for( ii=slashIndex+2;ii<userNameStringLength;ii++,i++)
					newUserNameString[i]=userNameString[ii];

				newUserNameString[i]='\0';
				LPCTSTR newName = CA2W(newUserNameString);
				thisObject->_this->c_ListView2.SetItemText(m,0,newName);

				BSTR bstrClass;
				pADs->get_Class(&bstrClass);
				
				thisObject->_this->c_ListView2.SetItemText(m,1,iter->GetString());

				if(  CComBSTR(bstrClass) == CComBSTR(L"User") )
					thisObject->_this->c_ListView2.SetItemText(m,3,L"Domain User");
				else if(  CComBSTR(bstrClass) == CComBSTR(L"Group") )
					thisObject->_this->c_ListView2.SetItemText(m,3,L"Domain Group");

				SysFreeString(bstrName);
				//thisObject->_this->c_ListView2.SetItemText(m,3,iter->GetString());

				//////////////////////// Adding into SearchList....
				list<CString> tempSearchList;
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,0));
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,1));
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,2));
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,3));
				thisObject->_this->SearchList.push_back(tempSearchList);
				//////////////////////////////////////////////////////////////

			}
			
			m=m+1;

			////////////////////////////////////////////////////	
			int count=thisObject->_this->c_ListView2.GetItemCount();
			char c[10];
			itoa(count,c,10);
			CString MFCString;
			MFCString = c;
	
			LPOLESTR str = new OLECHAR[MAX_PATH];
			wcscpy(str,L"Total Users : ");
			wcscat(str,MFCString);
			thisObject->_this->c_TotalUsersLabel.SetWindowText(str);
			////////////////////////////////////////////////////


			SysFreeString(bstrName);

			if( pDisp )
				pDisp->Release();
		}  // while 

		VariantClear(&var);

		if( pEnum )
			pEnum->Release();
		if( pUnk )
			pUnk->Release();
	} // for loop

	thisObject->_this->FilterGroupBoxShow();
	thisObject->_this->c_Info2Label.ShowWindow(SW_HIDE);
	thisObject->_this->c_Stop2Button.ShowWindow(SW_HIDE);
	thisObject->_this->c_Progress2.ShowWindow(SW_HIDE);
	thisObject->_this->c_PropertiesButton.EnableWindow(1);
	thisObject->_this->c_ResetPasswordButton.EnableWindow(1);
	thisObject->_this->c_AddButton.EnableWindow(1);
	thisObject->_this->c_DeleteButton.EnableWindow(1);
	thisObject->_this->c_EnableButton.EnableWindow(1);
	thisObject->_this->c_DisableButton.EnableWindow(1);

	if( thisObject->_this->c_ListView2.GetItemCount() == 0 )
		AfxMessageBox(L"No users available in this group",MB_ICONINFORMATION,MB_OK);
	else
	{
	}

	////////////////////////////////////////////////////	
	int count=thisObject->_this->c_ListView2.GetItemCount();
	char c[10];
	itoa(count,c,10);
	CString MFCString;
	MFCString = c;
	
	LPOLESTR str = new OLECHAR[MAX_PATH];
	wcscpy(str,L"Total Users : ");
	wcscat(str,MFCString);
	thisObject->_this->c_TotalUsersLabel.SetWindowText(str);
	////////////////////////////////////////////////////

	thisObject->_this->c_Picture1.ShowWindow(SW_SHOW);
	thisObject->_this->c_Picture2.ShowWindow(SW_HIDE);
	thisObject->_this->c_Picture4.ShowWindow(SW_HIDE);
	thisObject->_this->c_Picture5.ShowWindow(SW_SHOW);

	return TRUE;
}

UINT GetLocalUsersThread( LPVOID Param )
{
	THREADSTRUCT* thisObject = (THREADSTRUCT*)Param;
	int m=0;
	list<CString> compList;
	thisObject->_this->UserList.clear();
	thisObject->_this->c_ListView2.DeleteAllItems();
	int current=1;
	int size=thisObject->_this->ComputerList.size();
	thisObject->_this->c_Progress1.SetPos(0);
	int pos=0;

	list<int>::iterator iter2=thisObject->_this->ComputerListIndex.begin();
	for(list<CString>::iterator iter=thisObject->_this->ComputerList.begin();iter!=thisObject->_this->ComputerList.end();)
	{
		if( thisObject->_this->toBeStopped == true )
			break;
		///////////////////////////////////////////////////////////
		LPOLESTR infoText = new OLECHAR[MAX_PATH];
		wcscpy(infoText,L"Retrieving Local Users from \"");
		wcscat(infoText,iter->GetString());
		wcscat(infoText,L"\"...");
		thisObject->_this->c_InfoLabel.SetWindowText(infoText);
		thisObject->_this->c_InfoLabel.GetParent()->UpdateWindow();
		int percentage=(int)((100/size)*current);
		pos=pos+percentage;
		if( pos < 5 )
			thisObject->_this->c_Progress1.SetPos(5);
		//thisObject->_this->c_Progress1.SetPos(thisObject->_this->c_Progress1.GetPos()+percentage);
		else
			thisObject->_this->c_Progress1.SetPos(pos);
		//thisObject->_this->c_Progress1.StepIt();
		///////////////////////////////////////////////////////////

		LPOLESTR adsPath = new OLECHAR[MAX_PATH];
		wcscpy(adsPath,L"WinNT://");
		wcscat(adsPath,iter->GetString());
		wcscat(adsPath,L",computer");
		CoInitialize(NULL);
		IADsContainer *pCont=NULL;
		HRESULT hr;

		if( thisObject->_this->defaultUser )
		{
			hr = ADsGetObject( adsPath, IID_IADsContainer, (void**) &pCont );
		}
		else
		{
			hr = ADsOpenObject(adsPath,thisObject->_this->UserName,thisObject->_this->Password,ADS_SECURE_AUTHENTICATION, IID_IADsContainer, (void**) &pCont);
		}

		if( !SUCCEEDED(hr) )
		{
			//AfxMessageBox(L"Connection failed,try to get the local users some time later.",MB_ICONINFORMATION,MB_OK);
			thisObject->_this->c_ListView1.SetItemText(*iter2,1,L"Not Accessible");
			iter=thisObject->_this->ComputerList.erase(iter);
			iter2=thisObject->_this->ComputerListIndex.erase(iter2);
			continue;
		}

		IEnumVARIANT *pEnum = NULL;
		hr = ADsBuildEnumerator( pCont, &pEnum );

		if ( ! SUCCEEDED(hr) )
		{
			thisObject->_this->c_ListView1.SetItemText(*iter2,1,L"Not Accessible");
			iter=thisObject->_this->ComputerList.erase(iter);
			iter2=thisObject->_this->ComputerListIndex.erase(iter2);
			continue;
		}
        VARIANT var;
        ULONG lFetch;
        IADs *pChild=NULL;
        VariantInit(&var);
		
		list<list<CString>> temp;
		
        while( SUCCEEDED(ADsEnumerateNext( pEnum, 1, &var, &lFetch )) && lFetch == 1 )
        {
            hr = V_DISPATCH(&var)->QueryInterface( IID_IADs, (void**) &pChild );
            if ( SUCCEEDED(hr) )
            {
                BSTR bstrClass;
				pChild->get_Class(&bstrClass);
				
				if(  CComBSTR(bstrClass) == CComBSTR(L"User") )
				{
					list<CString> tempList;

					VARIANT var1,var2;
					BSTR bstrName;
					pChild->get_Name(&bstrName);
					pChild->Get(L"fullname",&var1);
					pChild->Get(L"description",&var2);
					thisObject->_this->c_ListView2.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
					thisObject->_this->c_ListView2.SetItemText(m,0,bstrName);
					thisObject->_this->c_ListView2.SetItemText(m,1,iter->GetString());
					thisObject->_this->c_ListView2.SetItemText(m,2,var1.bstrVal);
					thisObject->_this->c_ListView2.SetItemText(m,3,var2.bstrVal);
					//thisObject->_this->c_ListView2.SetItemText(m,3,iter->GetString());
					m=m+1;

					tempList.push_back(bstrName);
					tempList.push_back(iter->GetString());
					tempList.push_back(var1.bstrVal);
					tempList.push_back(var2.bstrVal);
					//tempList.push_back(iter->GetString());
					temp.push_back(tempList);
					
	                SysFreeString(bstrName);
					VariantClear(&var1);
					VariantClear(&var2);
				}

                SysFreeString(bstrClass);
			 }
		  } //while
		
		  thisObject->_this->c_ListView1.SetItemText(*iter2,1,L"Retrieved");
		  ++iter2;
		  compList.push_back(*iter);
		  thisObject->_this->UserList.push_back(temp);
		  pChild->Release();
		  VariantClear(&var);
          ++iter;
		 CoUninitialize();

	} // for loop

	thisObject->_this->c_Picture1.ShowWindow(SW_HIDE);
	thisObject->_this->c_Picture2.ShowWindow(SW_HIDE);
	thisObject->_this->c_Picture4.ShowWindow(SW_SHOW);
	thisObject->_this->c_Picture5.ShowWindow(SW_SHOW);

	thisObject->_this->c_LocalUsersButton.ShowWindow(SW_SHOW);
	thisObject->_this->c_InfoLabel.ShowWindow(SW_HIDE);
	thisObject->_this->c_StopButton.ShowWindow(SW_HIDE);
	thisObject->_this->c_Progress1.ShowWindow(SW_HIDE);

	thisObject->_this->ComputerList=compList;

	if( thisObject->_this->c_ListView2.GetItemCount() > 0 )
	{
		thisObject->_this->InformationGroupBoxHide();
		thisObject->_this->CredentialsGroupBoxHide();
		thisObject->_this->ComputersGroupBoxHide();
		thisObject->_this->LocalUsersGroupBoxShow();
		
		thisObject->_this->c_ComputerCombo.ResetContent();
		thisObject->_this->c_ComputerCombo.AddString(L"All Computers");						
		

			// Adding Computer Names in ComboBox
		int index=1;
		for(list<CString>::iterator iter=thisObject->_this->ComputerList.begin();iter!=thisObject->_this->ComputerList.end();++iter,index++)
		{
			thisObject->_this->c_ComputerCombo.InsertString(index,iter->GetString());
		}

		thisObject->_this->c_ComputerCombo.SelectString(0,L"All Computers");

		//thisObject->_this->OnCbnSelchangeCombo2();
		thisObject->_this->c_GroupCombo.ResetContent();
		thisObject->_this->c_GroupCombo.InsertString(0,L"All Groups");
		thisObject->_this->c_GroupCombo.InsertString(1,L"Administrators");
		thisObject->_this->c_GroupCombo.InsertString(2,L"Backup Operators");
		thisObject->_this->c_GroupCombo.InsertString(3,L"Guests");
		thisObject->_this->c_GroupCombo.InsertString(4,L"Network Configuration Operators");
		thisObject->_this->c_GroupCombo.InsertString(5,L"Remote Desktop Users");
		thisObject->_this->c_GroupCombo.InsertString(6,L"Replicator");
		thisObject->_this->c_GroupCombo.InsertString(7,L"Users");
		thisObject->_this->c_GroupCombo.InsertString(8,L"HelpServicesGroup");
		
		thisObject->_this->c_GroupCombo.SelectString(0,L"All Groups");
	} // if
	else
	{
		// CString str;
		//CEdit *editBox = (CEdit *)GetDlgItem(IDC_ImportTextBox);
		//editBox->GetWindowTextW(str); 
		//if(str.IsEmpty()){
		AfxMessageBox(L"Selected Computers are not accessible now",MB_ICONINFORMATION,MB_OK);
		//}
	}
	
	thisObject->_this->isThreadGoing = false ;
	return TRUE;
}

void CLocalUserManagementDlg::RetrieveLocalUserDetails()
{
	this->c_LocalUsersButton.ShowWindow(SW_HIDE);
	this->c_InfoLabel.ShowWindow(SW_SHOW);
	this->c_StopButton.ShowWindow(SW_SHOW);
	this->c_Progress1.ShowWindow(SW_SHOW);

	
	
	this->toBeStopped=false;
	this->isThreadGoing=true;

	THREADSTRUCT *_param = new THREADSTRUCT;
    _param->_this = this;
    AfxBeginThread (GetLocalUsersThread, _param);
}

	// Information Group Box
void CLocalUserManagementDlg::InformationGroupBoxShow()
{
	int x=100,y=50;

	CStatic* cs=(CStatic*)GetDlgItem(IDC_StarPictureControl1);
	cs->ShowWindow(SW_SHOW); 
	cs->MoveWindow(x+15,y+18,12,12,1);

	cs=(CStatic*)GetDlgItem(IDC_StarPictureControl2);
	cs->ShowWindow(SW_SHOW); 
	cs->MoveWindow(x+15,y+56,12,12,1);

	c_Info1Label.ShowWindow(SW_SHOW); 
	c_Info1Label.SetFontName(L"Verdanna");
	c_Info1Label.SetFontSize(9);
	c_Info1Label.MoveWindow(x+41,y+20,480,15,1);

	c_Information2Label.ShowWindow(SW_SHOW); 
	c_Information2Label.SetFontName(L"Verdanna");
	c_Information2Label.SetFontSize(9);
	c_Information2Label.MoveWindow(x+65,y+37,445,15,1);
	
	c_Information3Label.ShowWindow(SW_SHOW); 
	c_Information3Label.SetFontName(L"Verdanna");
	c_Information3Label.SetFontSize(9);
	c_Information3Label.MoveWindow(x+41,y+58,497,15,1);

	c_Information4Label.ShowWindow(SW_SHOW); 
	c_Information4Label.SetFontName(L"Verdanna");
	c_Information4Label.SetFontSize(9);
	c_Information4Label.MoveWindow(x+65,y+76,482,15,1);
	
	CWnd* pWnd = GetDlgItem(IDC_GROUP1);
	pWnd->ShowWindow(SW_SHOW);
	pWnd->MoveWindow(-5,145,720,1,1);
} // Information Group Box

	// Information Group Box
void CLocalUserManagementDlg::InformationGroupBoxHide()
{
	CStatic* cs=(CStatic*)GetDlgItem(IDC_StarPictureControl1);
	cs->ShowWindow(SW_HIDE); 

	cs=(CStatic*)GetDlgItem(IDC_StarPictureControl2);
	cs->ShowWindow(SW_HIDE); 

	CWnd* pWnd = GetDlgItem(IDC_GROUP1);
	pWnd->ShowWindow(SW_HIDE);

	c_Info1Label.ShowWindow(SW_HIDE); 
	c_Information2Label.ShowWindow(SW_HIDE); 
	c_Information3Label.ShowWindow(SW_HIDE); 
	c_Information4Label.ShowWindow(SW_HIDE); 
} // Information Group Box

	// Credentials Group Box
void CLocalUserManagementDlg::CredentialsGroupBoxShow()
{	
	CStatic* cs=(CStatic*)GetDlgItem(IDC_DOMAINNAMELABEL);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(92+67,140+22,97,15,1);

	cs=(CStatic*)GetDlgItem(IDC_USERNAMELABEL);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(92+203,140+21,79,15,1);

	cs=(CStatic*)GetDlgItem(IDC_PASSWORDLABEL);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(92+337,140+22,70 ,15,1);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
	ccb->ShowWindow(SW_SHOW);
	ccb->MoveWindow(92+67,140+44,121,21,1);
	
	ccb->SetRedraw(1);
	ccb->SendMessage(CB_SETMINVISIBLE,3,0);
	//ccb->SetMinVisibleItems(3);

	CEdit* ce=(CEdit*)GetDlgItem(IDC_EDIT1);
	ce->ShowWindow(SW_SHOW);
	ce->MoveWindow(92+203,140+44,121,21,1);
	
	ce=(CEdit*)GetDlgItem(IDC_EDIT2);
	ce->ShowWindow(SW_SHOW);
	ce->MoveWindow(92+337,140+44,121,21,1);

	CButton* cb=(CButton*)GetDlgItem(IDC_BUTTON1);
	cb->ShowWindow(SW_SHOW);
	cb->MoveWindow(92+106,140+78,120,23,1);

	cb=(CButton*)GetDlgItem(IDC_BUTTON3);
	cb->ShowWindow(SW_SHOW);
	cb->MoveWindow(92+256,140+78,150,23,1);
	
}      // Credentials Group Box

	   // Credentials Group Box
void CLocalUserManagementDlg::CredentialsGroupBoxHide()
{
	
	CStatic* cs=(CStatic*)GetDlgItem(IDC_DOMAINNAMELABEL);
	cs->ShowWindow(SW_HIDE);

	cs=(CStatic*)GetDlgItem(IDC_USERNAMELABEL);
	cs->ShowWindow(SW_HIDE);
	
	cs=(CStatic*)GetDlgItem(IDC_PASSWORDLABEL);
	cs->ShowWindow(SW_HIDE);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO1);
	ccb->ShowWindow(SW_HIDE);

	CEdit* ce=(CEdit*)GetDlgItem(IDC_EDIT1);
	ce->ShowWindow(SW_HIDE);

	ce=(CEdit*)GetDlgItem(IDC_EDIT2);
	ce->ShowWindow(SW_HIDE);
		
	CButton* cb=(CButton*)GetDlgItem(IDC_BUTTON1);
	cb->ShowWindow(SW_HIDE);

	cb=(CButton*)GetDlgItem(IDC_BUTTON3);
	cb->ShowWindow(SW_HIDE);
	
}    // Credentials Group Box

	// Computers Group Box
void CLocalUserManagementDlg::ComputersGroupBoxShow()
{
	CWnd* pWnd = GetDlgItem(IDC_GROUP2);
	pWnd->ShowWindow(SW_SHOW);
	pWnd->SetWindowText(L"Computer Details");
	pWnd->MoveWindow(92,265,518,345,1);

	CListCtrl* cl=(CListCtrl*)GetDlgItem(IDC_LIST2); 
	cl->ShowWindow(SW_SHOW);
	cl->MoveWindow(92+30,245+48+20,458,245,1);

	CButton* cb=(CButton*)GetDlgItem(IDC_LocalUsersButton);
	cb->ShowWindow(SW_SHOW);
	cb->MoveWindow(92+199,245+318,121 ,23,1);

	CStatic *cs=(CStatic*)GetDlgItem(IDC_InformationLabel);
	cs->ShowWindow(SW_HIDE);
	cs->MoveWindow(92+27,245+295+20,300,15,1);

	cs=(CStatic*)GetDlgItem(IDC_TotalComputersLabel);
	cs->ShowWindow(SW_SHOW);
	//cs->MoveWindow(92+29,245+29,180,15,1);
	cs->MoveWindow(92+30+57+115,245+29+10,130,15,1);

	////////////////////////////////////////////////////	
	int count=c_ListView1.GetItemCount();
	char c[10];
	itoa(count,c,10);
	CString MFCString;
	MFCString = c;
	
	LPOLESTR str = new OLECHAR[MAX_PATH];
	wcscpy(str,L"Total Computers : ");
	wcscat(str,MFCString);
	cs->SetWindowText(str);
	////////////////////////////////////////////////////

	if( c_ListView1.GetItemCount() == 0 )
	{
		cs=(CStatic*)GetDlgItem(IDC_SearchPictureBox);
		cs->ShowWindow(SW_HIDE);
		cs->MoveWindow(92+447,245+25+10,40,21,1);
	
		CEdit* ce=(CEdit*)GetDlgItem(IDC_SearchTextBox);
		ce->ShowWindow(SW_HIDE);
		ce->MoveWindow(92+351,245+25+10,96,21,1);

		c_SelectAll1Button.ShowWindow(SW_HIDE);
		c_SelectAll1Button.MoveWindow(92+30,245+25+10,55 ,23,1); 

		c_DeSelectAll1Button.ShowWindow(SW_HIDE);
		c_DeSelectAll1Button.MoveWindow(92+30+57,245+25+10,67 ,23,1); 
	}
	else
	{
		cs=(CStatic*)GetDlgItem(IDC_SearchPictureBox);
		cs->ShowWindow(SW_SHOW);
		cs->MoveWindow(92+447,245+25+10,40,21,1);
	
		CEdit* ce=(CEdit*)GetDlgItem(IDC_SearchTextBox);
		ce->ShowWindow(SW_SHOW);
		ce->MoveWindow(92+351,245+25+10,96,21,1);
		
		c_SelectAll1Button.ShowWindow(SW_SHOW);
		c_SelectAll1Button.MoveWindow(92+30,245+25+10,55 ,23,1); 

		c_DeSelectAll1Button.ShowWindow(SW_SHOW);
		c_DeSelectAll1Button.MoveWindow(92+30+57,245+25+10,67 ,23,1); 
	}

	cb=(CButton*)GetDlgItem(IDC_StopButton);
	cb->ShowWindow(SW_HIDE);
	cb->MoveWindow(92+366,245+318+20,121 ,23,1); 

	CProgressCtrl* cp=(CProgressCtrl*)GetDlgItem(IDC_PROGRESS1);
	cp->ShowWindow(SW_HIDE);
	cp->MoveWindow(92+32,245+318+20,314 ,23,1);
}  // Computers Group Box

	// Computers Group Box
void CLocalUserManagementDlg::ComputersGroupBoxHide()
{
	CWnd* pWnd = GetDlgItem(IDC_GROUP2);
	pWnd->ShowWindow(SW_HIDE);

	CListCtrl* cl=(CListCtrl*)GetDlgItem(IDC_LIST2); 
	cl->ShowWindow(SW_HIDE);

	CButton* cb=(CButton*)GetDlgItem(IDC_LocalUsersButton);
	cb->ShowWindow(SW_HIDE);

	CStatic *cs=(CStatic*)GetDlgItem(IDC_InformationLabel);
	cs->ShowWindow(SW_HIDE);

	cs=(CStatic*)GetDlgItem(IDC_TotalComputersLabel);
	cs->ShowWindow(SW_HIDE);

	cs=(CStatic*)GetDlgItem(IDC_SearchPictureBox);
	cs->ShowWindow(SW_HIDE);

	cb=(CButton*)GetDlgItem(IDC_StopButton);
	cb->ShowWindow(SW_HIDE);

	CProgressCtrl* cp=(CProgressCtrl*)GetDlgItem(IDC_PROGRESS1);
	cp->ShowWindow(SW_HIDE);
		
	CEdit* ce=(CEdit*)GetDlgItem(IDC_SearchTextBox);
	ce->ShowWindow(SW_HIDE);

	c_SelectAll1Button.ShowWindow(SW_HIDE);
	c_DeSelectAll1Button.ShowWindow(SW_HIDE);
} // Computers Group Box

	// Local Users Group Box
void CLocalUserManagementDlg::LocalUsersGroupBoxShow()
{
//	c_Picture1.ShowWindow(SW_SHOW);
//	c_Picture2.ShowWindow(SW_HIDE);
//	c_Picture4.ShowWindow(SW_HIDE);
//	c_Picture5.ShowWindow(SW_SHOW);

	CWnd* pWnd = GetDlgItem(IDC_GROUP3);
	pWnd->ShowWindow(SW_SHOW);
	pWnd->SetWindowText(L"Local User Details");
	//pWnd->MoveWindow(18,49,677,562,1);
	pWnd->MoveWindow(18,49+30,677,532,1);

	FilterGroupBoxShow();
	
	CListCtrl* cl=(CListCtrl*)GetDlgItem(IDC_LIST1); 
	cl->ShowWindow(SW_SHOW);
	//cl->MoveWindow(18+5,49+92,669,354,1);
	cl->MoveWindow(18+5,49+50+92,669,324,1);

	c_TotalUsersLabel.ShowWindow(SW_SHOW);
	c_TotalUsersLabel.MoveWindow(18+5+57+67+160,49+50+75,180,15,1);
	
	////////////////////////////////////////////////////	
	int count=c_ListView2.GetItemCount();
	char c[10];
	itoa(count,c,10);
	CString MFCString;
	MFCString = c;
	
	LPOLESTR str = new OLECHAR[MAX_PATH];
	wcscpy(str,L"Total Users : ");
	wcscat(str,MFCString);
	c_TotalUsersLabel.SetWindowText(str);
	////////////////////////////////////////////////////

	CStatic *cs=(CStatic*)GetDlgItem(IDC_Search2PictureBox);
	cs->ShowWindow(SW_SHOW);
	cs->MoveWindow(18+5+57+67+423,49+50+69,40,21,1);
	
	CEdit* ce=(CEdit*)GetDlgItem(IDC_Search2TextBox);
	ce->ShowWindow(SW_SHOW);
	ce->MoveWindow(18+5+57+67+325,49+50+69,96,21,1);
		
	c_SelectAll2Button.ShowWindow(SW_SHOW);
	c_SelectAll2Button.MoveWindow(18+5,49+50+69,55,23,1);

	c_DeSelectAll2Button.ShowWindow(SW_SHOW);
	c_DeSelectAll2Button.MoveWindow(18+5+57,49+50+69,67,23,1);

	CButton* cb1=(CButton*)GetDlgItem(Export);
	cb1->ShowWindow(SW_SHOW);
	cb1->MoveWindow(18+5+57+67+475,49+50+69,63,23,1);


	CButton* cb=(CButton*)GetDlgItem(IDC_Stop2Button);
	cb->ShowWindow(SW_HIDE);
//	cb->MoveWindow(18+420,49+52,110,23,1);
	cb->MoveWindow(18+420,49+30+10+52,110,23,1);

	cs=(CStatic*)GetDlgItem(IDC_Information2Label);
	cs->ShowWindow(SW_HIDE);
//	cs->MoveWindow(18+143,49+25,300,15,1);
	cs->MoveWindow(18+143,49+30+10+25,300,15,1);

	CProgressCtrl* cp=(CProgressCtrl*)GetDlgItem(IDC_PROGRESS2);
	cp->ShowWindow(SW_HIDE);
//	cp->MoveWindow(18+171,49+52,227 ,23,1);
	cp->MoveWindow(18+171,49+30+10+52,227 ,23,1);
	

	SingleOperationsGroupBoxShow();
	MultipleOperationsGroupBoxShow();

	c_Picture1.ShowWindow(SW_SHOW);
	c_Picture2.ShowWindow(SW_HIDE);
	c_Picture4.ShowWindow(SW_HIDE);
	c_Picture5.ShowWindow(SW_SHOW);

} //  Local Group Box

	// Local Users Group Box
void CLocalUserManagementDlg::LocalUsersGroupBoxHide()
{
	CWnd* pWnd = GetDlgItem(IDC_GROUP3);
	pWnd->ShowWindow(SW_HIDE);

	FilterGroupBoxHide();
	
	CListCtrl* cl=(CListCtrl*)GetDlgItem(IDC_LIST1); 
	cl->ShowWindow(SW_HIDE);

	CButton* cb=(CButton*)GetDlgItem(IDC_Stop2Button);
	cb->ShowWindow(SW_HIDE);
//	cb->MoveWindow(18+420,49+52,110,23,1);

	CStatic* cs=(CStatic*)GetDlgItem(IDC_Information2Label);
	cs->ShowWindow(SW_HIDE);
//	cs->MoveWindow(18+143,49+25,48,15,1);

	CProgressCtrl* cp=(CProgressCtrl*)GetDlgItem(IDC_PROGRESS2);
	cp->ShowWindow(SW_HIDE);
//	cp->MoveWindow(18+171,49+52,227 ,23,1);

	c_SelectAll2Button.ShowWindow(SW_HIDE);
	c_DeSelectAll2Button.ShowWindow(SW_HIDE);
	CButton* cb1=(CButton*)GetDlgItem(Export);
	cb1->ShowWindow(SW_HIDE);
	
	c_TotalUsersLabel.ShowWindow(SW_HIDE);
	c_Search2TextBox.ShowWindow(SW_HIDE);
	c_Search2PictureBox.ShowWindow(SW_HIDE);

	SingleOperationsGroupBoxHide();
	MultipleOperationsGroupBoxHide();
} //  Local Group Box

	// Multiple Operations Group Box
void CLocalUserManagementDlg::MultipleOperationsGroupBoxShow()
{
	CWnd* pWnd = GetDlgItem(IDC_GROUP6);
	pWnd->ShowWindow(SW_SHOW);
	pWnd->SetWindowText(L"Multiple User Operations");
	//pWnd->MoveWindow(18+301,49+452+20,370,80,1);
	pWnd->MoveWindow(18+5,49+452+20,490,80,1);

	CButton* cb=(CButton*)GetDlgItem(IDC_AddButton);
	cb->ShowWindow(SW_SHOW);
	//cb->MoveWindow(18+301+19,49+452+20+33,75,23,1);
	cb->MoveWindow(18+5+19,49+452+20+33,75,23,1);

	cb=(CButton*)GetDlgItem(IDC_DeleteButton);
	cb->ShowWindow(SW_SHOW);
	//cb->MoveWindow(18+301+110,49+452+20+33,75,23,1);
	cb->MoveWindow(18+5+110,49+452+20+33,75,23,1);

	cb=(CButton*)GetDlgItem(IDC_EnableButton);
	cb->ShowWindow(SW_SHOW);
	//cb->MoveWindow(18+301+199,49+452+20+33,66,23,1);
	cb->MoveWindow(18+5+199,49+452+20+33,66,23,1);

	cb=(CButton*)GetDlgItem(IDC_DisableButton);
	cb->ShowWindow(SW_SHOW);
	//cb->MoveWindow(18+301+277,49+452+20+33,75,23,1);
	cb->MoveWindow(18+5+277,49+452+20+33,75,23,1);

	cb=(CButton*)GetDlgItem(IDC_ResetPasswordButton);
	cb->ShowWindow(SW_SHOW);
	//cb->MoveWindow(18+301+118,49+452+20+33,108,23,1);
	cb->MoveWindow(18+5+365,49+452+20+33,108,23,1);

} //  Multiple Operations Group Box

	// Multiple Operations Group Box
void CLocalUserManagementDlg::MultipleOperationsGroupBoxHide()
{
	CWnd* pWnd = GetDlgItem(IDC_GROUP6);
	pWnd->ShowWindow(SW_HIDE);

	CButton* cb=(CButton*)GetDlgItem(IDC_AddButton);
	cb->ShowWindow(SW_HIDE);

	cb=(CButton*)GetDlgItem(IDC_DeleteButton);
	cb->ShowWindow(SW_HIDE);

	cb=(CButton*)GetDlgItem(IDC_EnableButton);
	cb->ShowWindow(SW_HIDE);

	cb=(CButton*)GetDlgItem(IDC_DisableButton);
	cb->ShowWindow(SW_HIDE);
} //  Multiple Operations Group Box

	// Single Operations Group Box
void CLocalUserManagementDlg::SingleOperationsGroupBoxShow()
{
	CWnd* pWnd = GetDlgItem(IDC_GROUP5);
	pWnd->ShowWindow(SW_SHOW);
	pWnd->SetWindowText(L"Single User Operations");
	//pWnd->MoveWindow(18+5,49+452+20,245,80,1);
	pWnd->MoveWindow(18+5+510,49+452+20,155,80,1);

	CButton* cb=(CButton*)GetDlgItem(IDC_PropertiesButton);
	cb->ShowWindow(SW_SHOW);
	cb->MoveWindow(18+5+510+37,49+452+20+33,86,23,1);

//	cb=(CButton*)GetDlgItem(IDC_ResetPasswordButton);
//	cb->ShowWindow(SW_SHOW);
//	cb->MoveWindow(18+5+118,49+452+20+33,108,23,1);
} //  Single Operations Group Box

	// Single Operations Group Box
void CLocalUserManagementDlg::SingleOperationsGroupBoxHide()
{
	CWnd* pWnd = GetDlgItem(IDC_GROUP5);
	pWnd->ShowWindow(SW_HIDE);

	CButton* cb=(CButton*)GetDlgItem(IDC_PropertiesButton);
	cb->ShowWindow(SW_HIDE);

	cb=(CButton*)GetDlgItem(IDC_ResetPasswordButton);
	cb->ShowWindow(SW_HIDE);
} //  Single Operations Group Box

	// Filter Group Box
void CLocalUserManagementDlg::FilterGroupBoxShow()
{
	CWnd* pWnd = GetDlgItem(IDC_GROUP4);
	pWnd->ShowWindow(SW_SHOW);
	pWnd->SetWindowText(L"Filter");
	//pWnd->MoveWindow(18+5,49+20,670,65,1);
	pWnd->MoveWindow(18+5,49+30+20,670,65,1);

	CStatic* cs=(CStatic*)GetDlgItem(IDC_COMPUTERLABEL);
	cs->ShowWindow(SW_SHOW);
	//cs->MoveWindow(18+5+87,49+20+30,70,15,1);
	cs->MoveWindow(18+5+87,49+30+20+30,70,15,1);
	
	cs=(CStatic*)GetDlgItem(IDC_GROUPLABEL);
	cs->ShowWindow(SW_SHOW);
	//cs->MoveWindow(18+5+376,49+20+30,53,15,1);
	cs->MoveWindow(18+5+376,49+30+20+30,53,15,1);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO2);
	ccb->ShowWindow(SW_SHOW);
	//ccb->MoveWindow(18+5+166,49+20+27,121,23,1);
	ccb->MoveWindow(18+5+166,49+30+20+27,121,23,1);
	ccb->SetRedraw(1);
	ccb->SendMessage(CB_SETMINVISIBLE,8,0);
	//ccb->SetMinVisibleItems(4);

	ccb = (CComboBox*)GetDlgItem(IDC_COMBO3);
	ccb->ShowWindow(SW_SHOW);
	//ccb->MoveWindow(18+5+439,49+20+27,121,23,1);
	ccb->MoveWindow(18+5+439,49+30+20+27,121,23,1);
	ccb->SetRedraw(1);
	ccb->SendMessage(CB_SETMINVISIBLE,8,0);


	//ccb->SetMinVisibleItems(8);
} //  Filter Group Box

	// Filter Group Box
void CLocalUserManagementDlg::FilterGroupBoxHide()
{
	CWnd* pWnd = GetDlgItem(IDC_GROUP4);
	pWnd->ShowWindow(SW_HIDE);

	CStatic* cs=(CStatic*)GetDlgItem(IDC_COMPUTERLABEL);
	cs->ShowWindow(SW_HIDE);

	cs=(CStatic*)GetDlgItem(IDC_GROUPLABEL);
	cs->ShowWindow(SW_HIDE);

	CComboBox* ccb = (CComboBox*)GetDlgItem(IDC_COMBO2);
	ccb->ShowWindow(SW_HIDE);

	ccb = (CComboBox*)GetDlgItem(IDC_COMBO3);
	ccb->ShowWindow(SW_HIDE);
} //  Filter Group Box

		// Form Load function
void CLocalUserManagementDlg::MainFormLoad()
{
	this->isThreadGoing=false;
	c_Picture1.ShowWindow(SW_HIDE);
	c_Picture2.ShowWindow(SW_HIDE);
	c_Picture4.ShowWindow(SW_SHOW);
	c_Picture5.ShowWindow(SW_SHOW);

	this->defaultUser=true;
	this->toBeStopped=false;
	this->c_Progress1.SetRange(1,100);
	this->c_LinkLabel.SetURL(_T("http://manageengine.adventnet.com/products/ad-manager/"));
	this->c_LinkLabel.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));

	c_ReportLabel.MoveWindow(377,611,173,15,1);
	c_LinkLabel.MoveWindow(530,611,132,13,1);
	
		//Adding Column
	LVCOLUMN lvcolumn;
	lvcolumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvcolumn.fmt = LVCFMT_LEFT;
	
	lvcolumn.cx = 150;
	lvcolumn.pszText = L"Computer Name";
	c_ListView1.InsertColumn(0,&lvcolumn);
	lvcolumn.cx = 288;
	lvcolumn.pszText = L"Distinguished Name";
	c_ListView1.InsertColumn(1,&lvcolumn);
	c_ListView1.SetExtendedStyle(LVS_EX_CHECKBOXES | LVS_EX_GRIDLINES);

	lvcolumn.cx = 160;
	lvcolumn.pszText = L"User Name";
	c_ListView2.InsertColumn(0,&lvcolumn);
	lvcolumn.cx = 110;
	lvcolumn.pszText = L"Machine Name";
	c_ListView2.InsertColumn(1,&lvcolumn);
	lvcolumn.cx = 150;
	lvcolumn.pszText = L"Full Name";
	c_ListView2.InsertColumn(2,&lvcolumn);
	lvcolumn.cx = 240;
	lvcolumn.pszText = L"Description";
	c_ListView2.InsertColumn(3,&lvcolumn);
//	lvcolumn.cx = 110;
//	lvcolumn.pszText = L"Machine Name";
//	c_ListView2.InsertColumn(3,&lvcolumn);
	c_ListView2.SetExtendedStyle(LVS_EX_CHECKBOXES | LVS_EX_GRIDLINES);

		//Form Position
	SetWindowPos(NULL,0,0,715,659,SWP_NOZORDER | SWP_NOMOVE );
	SetWindowText(L"Local User Management");
	InformationGroupBoxShow();
	CredentialsGroupBoxShow();
	ComputersGroupBoxShow();
	LocalUsersGroupBoxHide();
 try
	{
		DWORD dwRet;
		LPOLESTR rootStr = new OLECHAR[MAX_PATH]; 
		PDOMAIN_CONTROLLER_INFO pdcInfo;
		dwRet = DsGetDcName(NULL, NULL, NULL, NULL, 0, &pdcInfo);
		
		if(!ERROR_SUCCESS == dwRet)
		{
			AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);
			return;
		}
		c_DomainName.AddString(pdcInfo->DomainName);
		c_DomainName.SelectString(0,pdcInfo->DomainName);
	}
	catch(...)
	{
		AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);
	}
	    c_DomainName.GetWindowTextW(DomainName);
	    THREADSTRUCT *_param = new THREADSTRUCT;
		_param->_this = this;
		AfxBeginThread (GetComputersThread, _param);
}  // Form Load function

// Properties Button
void CLocalUserManagementDlg::OnBnClickedPropertiesbutton()
{
	int selectedIndex=-1;
	int totalSelectedItems=0;

	for( int i=0;i<c_ListView2.GetItemCount();i++)
	{
		bool bChecked = c_ListView2.GetCheck(i);
		if( bChecked == true )
		{
			selectedIndex=i;
			totalSelectedItems=totalSelectedItems+1;
		}
	}

	if( totalSelectedItems != 1 )
		AfxMessageBox(L"Please select exactly one user",MB_ICONINFORMATION,MB_OK);
	else
	{
		CString username=c_ListView2.GetItemText(selectedIndex,0);	
		CString machinename=c_ListView2.GetItemText(selectedIndex,1);	
		///////////////////////////////////////////////////////////
		LPOLESTR adsPath = new OLECHAR[MAX_PATH];
		wcscpy(adsPath,L"WinNT://");
		wcscat(adsPath,machinename);
		wcscat(adsPath,L"/");
		wcscat(adsPath,username);

		CoInitialize(NULL);
		IADsUser *pAdsUser=NULL;
		HRESULT hr;

		hr = ADsGetObject( adsPath, IID_IADsUser, (void**) &pAdsUser );

		if( !SUCCEEDED(hr) )
		{
			wcscpy(adsPath,L"WinNT://");
			wcscat(adsPath,username);

			hr = ADsGetObject( adsPath, IID_IADsUser, (void**) &pAdsUser );
			if( !SUCCEEDED(hr) )
			{
				AfxMessageBox(L"This selection is not an User.\nPlease Select an User for its Properties",MB_ICONINFORMATION,MB_OK);
				c_ListView2.SetCheck(selectedIndex,0);
				return;
			}
		
			DomainUserPropertiesForm domainUserPropertiesForm(username,machinename,UserName,Password,defaultUser);
			domainUserPropertiesForm.DoModal();
			pAdsUser->Release();
			c_ListView2.SetCheck(selectedIndex,0);
			
			if( domainUserPropertiesForm.canUpdate )
			{
				RefreshListView();
				OnCbnSelchangeCombo3();
			}
			return;
		}
		///////////////////////////////////////////////////////////
		CString fullname=c_ListView2.GetItemText(selectedIndex,2);
		CString description=c_ListView2.GetItemText(selectedIndex,3);

		PropertiesForm propertiesForm(username,fullname,description,machinename,UserName,Password,defaultUser);
		propertiesForm.DoModal();

		c_ListView2.SetCheck(selectedIndex,0);
		if( propertiesForm.canUpdate )
		{
			RefreshListView();
			OnCbnSelchangeCombo3();
		}
	}
}  // Properties Button

	// Computer Selection
void CLocalUserManagementDlg::OnCbnSelchangeCombo2()
{
	c_GroupCombo.ResetContent();
	c_GroupCombo.AddString(L"All Groups");
	c_GroupCombo.SelectString(0,L"All Groups");

	OnCbnSelchangeCombo3();

	int selectedIndex=c_ComputerCombo.GetCurSel();
	int groupIndex=0;

	if( selectedIndex == 0 )
	{
		c_GroupCombo.InsertString(++groupIndex,L"Administrators");
		c_GroupCombo.InsertString(++groupIndex,L"Backup Operators");
		c_GroupCombo.InsertString(++groupIndex,L"Guests");
		c_GroupCombo.InsertString(++groupIndex,L"Network Configuration Operators");
		c_GroupCombo.InsertString(++groupIndex,L"Remote Desktop Users");
		c_GroupCombo.InsertString(++groupIndex,L"Replicator");
		c_GroupCombo.InsertString(++groupIndex,L"Users");
		c_GroupCombo.InsertString(++groupIndex,L"HelpServicesGroup");
	} // if
	else
	{
		CString selectedComputer ;
		c_ComputerCombo.GetLBText(selectedIndex,selectedComputer);
		LPOLESTR adsPath = new OLECHAR[MAX_PATH];
		wcscpy(adsPath,L"WinNT://");
		wcscat(adsPath,selectedComputer);
		wcscat(adsPath,L",computer");
	
		CoInitialize(NULL);
		IADsContainer *pCont=NULL;
		HRESULT hr;

		if( defaultUser )
		{
			hr = ADsGetObject( adsPath, IID_IADsContainer, (void**) &pCont );
		}
		else
		{
			hr = ADsOpenObject(adsPath,UserName,Password,ADS_SECURE_AUTHENTICATION, IID_IADsContainer, (void**) &pCont);
		}

		if( !SUCCEEDED(hr) )
		{
			AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);
			return;
		}
		IEnumVARIANT *pEnum = NULL;
		hr = ADsBuildEnumerator( pCont, &pEnum );

		if ( ! SUCCEEDED(hr) )
		{
			AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);
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
					c_GroupCombo.InsertString(++groupIndex,bstrName);
					
	                SysFreeString(bstrName);
				}
			 }
		  } //while
		  pChild->Release();
		  VariantClear(&var);
	      CoUninitialize();
	} // else
} // Computer Selection

	// Group Selection
void CLocalUserManagementDlg::OnCbnSelchangeCombo3()
{
	c_ListView2.DeleteAllItems();

	c_Picture1.ShowWindow(SW_HIDE);
	c_Picture2.ShowWindow(SW_HIDE);
	c_Picture4.ShowWindow(SW_SHOW);
	c_Picture5.ShowWindow(SW_SHOW);

	////////////////////////////////////////////////////	
	int count=c_ListView2.GetItemCount();
	char c[10];
	itoa(count,c,10);
	CString MFCString;
	MFCString = c;
	
	LPOLESTR str = new OLECHAR[MAX_PATH];
	wcscpy(str,L"Total Users : ");
	wcscat(str,MFCString);
	c_TotalUsersLabel.SetWindowText(str);
	////////////////////////////////////////////////////

	int selectedGroupIndex=c_GroupCombo.GetCurSel();

	if( selectedGroupIndex == 0 )
	{
		int selectedComputerIndex=c_ComputerCombo.GetCurSel();

		if( selectedComputerIndex == 0 )
			DisplayAllUsers();
		else
		{
			DisplaySingleComputerUsers(selectedComputerIndex);
		}
	} // if
	else
	{
		this->SearchList.clear();
		this->FilterGroupBoxHide();
		this->c_Info2Label.ShowWindow(SW_SHOW);
		this->c_Stop2Button.ShowWindow(SW_SHOW);
		this->c_Progress2.ShowWindow(SW_SHOW);
		this->c_PropertiesButton.EnableWindow(0);
		this->c_ResetPasswordButton.EnableWindow(0);
		this->c_AddButton.EnableWindow(0);
		this->c_DeleteButton.EnableWindow(0);
		this->c_EnableButton.EnableWindow(0);
		this->c_DisableButton.EnableWindow(0);
	
		this->toBeStopped=false;

		THREADSTRUCT *_param = new THREADSTRUCT;
		_param->_this = this;
		AfxBeginThread (DisplayGroupUsersThread, _param);
	} // else
}  // Group Selection

void CLocalUserManagementDlg::DisplayGroupUsers(list<CString> tempList)
{
}

void CLocalUserManagementDlg::DisplayAllUsers()
{
	int row=0;
	this->SearchList.clear();		
	
	for(list<list<list<CString>>>::iterator iter1=UserList.begin();iter1 !=UserList.end();++iter1)
	{
		list<list<CString>> tempList1=*iter1;
		
		for(list<list<CString>>::iterator iter2=tempList1.begin();iter2!=tempList1.end();++iter2)
		{
			list<CString> tempList2=*iter2;

			int col=-1;
			c_ListView2.InsertItem(LVIF_TEXT|LVIF_STATE, row, _T(""),0, 0, 0, 0);
			
			list<CString> tempSearchList;

			for(list<CString>::iterator iter3=tempList2.begin();iter3!=tempList2.end();++iter3)
			{
				c_ListView2.SetItemText(row,++col,iter3->GetString());

				tempSearchList.push_back(iter3->GetString());
				
				/*
				//////////////////////// Adding into SearchList....
				list<CString> tempSearchList;
				tempSearchList.push_back(iter3->GetString());
				tempSearchList.push_back(iter3->GetString());
				tempSearchList.push_back(iter3->GetString());
				tempSearchList.push_back(iter3->GetString());
				SearchList.push_back(tempSearchList);
				//////////////////////////////////////////////////////////////
				*/

				////////////////////////////////////////////////////	
				int count=c_ListView2.GetItemCount();
				char c[10];
				itoa(count,c,10);
				CString MFCString;
				MFCString = c;
	
				LPOLESTR str = new OLECHAR[MAX_PATH];
				wcscpy(str,L"Total Users : ");
				wcscat(str,MFCString);
				c_TotalUsersLabel.SetWindowText(str);
				////////////////////////////////////////////////////

			}
			SearchList.push_back(tempSearchList);
			++row;
		} // inner "for" loop
	} // outer "for" loop

	c_Picture1.ShowWindow(SW_SHOW);
	c_Picture2.ShowWindow(SW_HIDE);
	c_Picture4.ShowWindow(SW_HIDE);
	c_Picture5.ShowWindow(SW_SHOW);

}

void CLocalUserManagementDlg::DisplaySingleComputerUsers(int selectedComputerIndex)
{
	int row=0;
	int in=0;
	this->SearchList.clear();

	for(list<list<list<CString>>>::iterator iter1=UserList.begin();iter1 !=UserList.end();++iter1,in++)
	{
		if( in == selectedComputerIndex-1 )
		{
		list<list<CString>> tempList1=*iter1;
		
		for(list<list<CString>>::iterator iter2=tempList1.begin();iter2!=tempList1.end();++iter2)
		{
			list<CString> tempList2=*iter2;

			int col=-1;
			c_ListView2.InsertItem(LVIF_TEXT|LVIF_STATE, row, _T(""),0, 0, 0, 0);

			list<CString> tempSearchList;

			for(list<CString>::iterator iter3=tempList2.begin();iter3!=tempList2.end();++iter3)
			{
				c_ListView2.SetItemText(row,++col,iter3->GetString());

				tempSearchList.push_back(iter3->GetString());
				
				/*
				//////////////////////// Adding into SearchList....
				list<CString> tempSearchList;
				tempSearchList.push_back(iter3->GetString());
				tempSearchList.push_back(iter3->GetString());
				tempSearchList.push_back(iter3->GetString());
				tempSearchList.push_back(iter3->GetString());
				SearchList.push_back(tempSearchList);
				//////////////////////////////////////////////////////////////
				*/

				////////////////////////////////////////////////////	
				int count=c_ListView2.GetItemCount();
				char c[10];
				itoa(count,c,10);
				CString MFCString;
				MFCString = c;
	
				LPOLESTR str = new OLECHAR[MAX_PATH];
				wcscpy(str,L"Total Users : ");
				wcscat(str,MFCString);
				c_TotalUsersLabel.SetWindowText(str);
				////////////////////////////////////////////////////

			}
			SearchList.push_back(tempSearchList);
			++row;
		}
		break;
		} // if
	}
	
	c_Picture1.ShowWindow(SW_SHOW);
	c_Picture2.ShowWindow(SW_HIDE);
	c_Picture4.ShowWindow(SW_HIDE);
	c_Picture5.ShowWindow(SW_SHOW);

}

	// Reset Password Button
void CLocalUserManagementDlg::OnBnClickedResetpasswordbutton()
{
	int i;
	for( i=0;i<c_ListView2.GetItemCount();i++)
	{
		bool bChecked = c_ListView2.GetCheck(i);
		if( bChecked == true )
			break;
	}
	if( i==c_ListView2.GetItemCount() )
	{
		AfxMessageBox(L"Please select atleast one user",MB_ICONINFORMATION,MB_OK);
		return;
	}

	ResetPasswordForm resetPasswordForm(1);
	resetPasswordForm.DoModal(); 

	if( resetPasswordForm.canProceed==false )
	{
		this->UncheckAll();
		return;
	}

	CString fileString;
	fileString="";

	bool isAccessDenied=false;
	bool isPasswordProblem=false;
	bool isError=false;
	int noOfErrors=0;

	for( int i=0;i<c_ListView2.GetItemCount();i++)
	{
		bool bChecked = c_ListView2.GetCheck(i);

		if( bChecked == true )
		{
			CString name=c_ListView2.GetItemText(i,0);	
			CString machine=c_ListView2.GetItemText(i,1);
		
			try
			{
				LPOLESTR adsPath = new OLECHAR[MAX_PATH];
				wcscpy(adsPath,L"WinNT://");
				wcscat(adsPath,machine);
				wcscat(adsPath,L"/");
				wcscat(adsPath,name);
				wcscat(adsPath,L",User");
				CoInitialize(NULL);
				IADsUser *pADsUser=NULL;
				HRESULT hr;
		
				if( defaultUser )
				{
					hr = ADsGetObject( adsPath, IID_IADsUser, (void**) &pADsUser );
				}
				else
				{
					hr = ADsOpenObject(adsPath,UserName,Password,ADS_SECURE_AUTHENTICATION, IID_IADsUser, (void**) &pADsUser);
				}

				if( !SUCCEEDED(hr) )
				{
					//CString temp;
					isError=true;
					++noOfErrors;

					fileString.Append(L"\"");
					fileString.Append(name);
					fileString.Append(L"\" in \"");
					fileString.Append(machine);
					fileString.Append(L"\" is not a Local User. So Password cant be reset for this Object.");
					fileString.Append(L"\n");
					//AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
					//AfxMessageBox(L"Password can be reset for Local Users only",MB_ICONINFORMATION,MB_OK);
				//	c_ListView2.SetCheck(selectedIndex,0);
				//	return;
					continue;
				}
				//ResetPasswordForm resetPasswordForm(name,machine,UserName,Password,defaultUser);
				//resetPasswordForm.DoModal();
				BSTR bstrPassword=resetPasswordForm.newPassword.AllocSysString();
			
				hr=pADsUser->SetPassword(bstrPassword);
			

				if( !SUCCEEDED(hr) )
				{
					isError=true;
					++noOfErrors;

					if( hr == -2147022651 )
					{
						isPasswordProblem=true;
						fileString.Append(L"The password \"");
						fileString.Append(resetPasswordForm.newPassword);
						fileString.Append(L"\" does not satisfy Password Policy requirements of \"");
						fileString.Append(machine);
						fileString.Append(L"\" computer.");
						fileString.Append(L"\n");
						//AfxMessageBox(L"The password does not meet the password policy requirements",MB_ICONINFORMATION,MB_OK);
					}
					else if( hr == -2147024891 )
					{
						isAccessDenied=true;
						fileString.Append(L"You dont have permission to reset Password in \"");
						fileString.Append(machine);
						fileString.Append(L"\" computer.");
						fileString.Append(L"\n");
						//AfxMessageBox(L"You dont have permission to reset Password",MB_ICONINFORMATION,MB_OK);
					}
					else
					{
						fileString.Append(L"Unspecified error occurred while reset the password for \"");
						fileString.Append(name);
						fileString.Append(L"\" in \"");
						fileString.Append(machine);
						fileString.Append(L"\" computer.");
						fileString.Append(L"\n");
						//AfxMessageBox(L"Unspecified error occurred while reset the password",MB_ICONINFORMATION,MB_OK);
					}

					SysFreeString(bstrPassword);
				//	return;
					continue;
				}
			}
			catch(...)
			{
				AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);
				//c_ListView2.SetCheck(selectedIndex,0);
				return;
			}
			//c_ListView2.SetCheck(selectedIndex,0);
			++noOfErrors;

			fileString.Append(L"Password is changed for \"");
			fileString.Append(name);
			fileString.Append(L"\" in \"");
			fileString.Append(machine);
			fileString.Append(L"\" computer successfully");
			fileString.Append(L"\n");
		} // if
	} // for loop

	if( ! isError )
	{
		AfxMessageBox(L"Password is changed successfully",MB_ICONINFORMATION,MB_OK);
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
			outFile.open("c:\\Windows\\Temp\\LocalUserManagementReports.txt",ios::out) ;
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

			temp.Append(L"To read more reports, Click \"YES\"");

			if( AfxMessageBox(temp,MB_YESNO|MB_ICONQUESTION) == IDYES )
			{
				STARTUPINFO startup_info = {0};
				startup_info.cb = sizeof startup_info;
				PROCESS_INFORMATION pi = {0};
				
				CreateProcess( _T("C:\\WINDOWS\\system32\\notepad.exe"),_T("C:\\WINDOWS\\system32\\notepad.exe C:\\Windows\\Temp\\LocalUserManagementReports.txt"),NULL,NULL,FALSE,0,NULL,NULL,&startup_info,&pi) ;
			} 
		}
		catch(...)
		{
			outFile.close();
		}
	}
} // Reset Password Button

	// Back Button
void CLocalUserManagementDlg::OnStnClickedPicturecontrol1()
{
	c_Picture1.ShowWindow(SW_HIDE);
	c_Picture2.ShowWindow(SW_SHOW);
	c_Picture4.ShowWindow(SW_SHOW);
	c_Picture5.ShowWindow(SW_HIDE);

	InformationGroupBoxShow();
	CredentialsGroupBoxShow();
	ComputersGroupBoxShow();
	LocalUsersGroupBoxHide();
}	// Back Button

	// Front Button
void CLocalUserManagementDlg::OnStnClickedPicturecontrol2()
{
	c_Picture1.ShowWindow(SW_SHOW);
	c_Picture2.ShowWindow(SW_HIDE);
	c_Picture4.ShowWindow(SW_HIDE);
	c_Picture5.ShowWindow(SW_SHOW);

	InformationGroupBoxHide();
	CredentialsGroupBoxHide();
	ComputersGroupBoxHide();
	LocalUsersGroupBoxShow();
}	// Front Button

	// Add Button
void CLocalUserManagementDlg::OnBnClickedAddbutton()
{
	AddForm addForm(UserName,Password,defaultUser,ComputerList);
	addForm.DoModal();

	if( addForm.canUpdate )
	{
		RefreshListView();
		OnCbnSelchangeCombo3();
	}
} // Add Button

	// Enable Button
void CLocalUserManagementDlg::OnBnClickedEnablebutton()
{
	bool isEnabled = false;
	bool isUser = false;
	for( int i=0;i<c_ListView2.GetItemCount();i++)
	{
		bool bChecked = c_ListView2.GetCheck(i);
		if( bChecked == true )
		{
			isUser= true;

			LPOLESTR adsPath = new OLECHAR[MAX_PATH];
			wcscpy(adsPath,L"WinNT://");
			wcscat(adsPath,c_ListView2.GetItemText(i,1));
			wcscat(adsPath,L"/");
			wcscat(adsPath,c_ListView2.GetItemText(i,0));
			//wcscat(adsPath,L",User");
			CoInitialize(NULL);
			IADsUser *pADsUser=NULL;
			HRESULT hr;
		
			if( defaultUser )
			{
				hr = ADsGetObject( adsPath, IID_IADsUser, (void**) &pADsUser );
			}
			else
			{
				hr = ADsOpenObject(adsPath,UserName,Password,ADS_SECURE_AUTHENTICATION, IID_IADsUser, (void**) &pADsUser);
			}

			if( !SUCCEEDED(hr) )
			{
				AfxMessageBox(L"Domain Users/Domain Groups cant be Enabled.\nFor Local Users only, we can have this operation",MB_ICONINFORMATION,MB_OK);
				UncheckAll();
				return;
			}

			hr=pADsUser->put_AccountDisabled(FALSE);
			if( !SUCCEEDED(hr) )
			{
				LPOLESTR temp = new OLECHAR[MAX_PATH];

				if( hr == -2147024891 )
				{
					wcscpy(temp,L"You dont have permission to enable Users in \"");
					wcscat(temp,c_ListView2.GetItemText(i,1));
					wcscat(temp,L"\"");
					AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
				}
				else
					AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);

				UncheckAll();
				return;
			}
			hr=pADsUser->SetInfo();
			if( !SUCCEEDED(hr) )
			{
				LPOLESTR temp = new OLECHAR[MAX_PATH];

				if( hr == -2147024891 )
				{
					wcscpy(temp,L"You dont have permission to enable Users in \"");
					wcscat(temp,c_ListView2.GetItemText(i,1));
					wcscat(temp,L"\"");
					AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);

				}
				else
					AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);

				UncheckAll();
				return;
			}
			isEnabled=true;
		} // if
	} // for

	if( !isUser )
		AfxMessageBox(L"Please select atleast one user",MB_ICONINFORMATION,MB_OK);
	if( isEnabled )
		AfxMessageBox(L"Users are Enabled Successfully",MB_ICONINFORMATION,MB_OK);

	UncheckAll();

}  // Enable Button

	// Disable Button
void CLocalUserManagementDlg::OnBnClickedDisablebutton()
{
	bool isDisabled = false;
	bool isUser = false;
	for( int i=0;i<c_ListView2.GetItemCount();i++)
	{
		bool bChecked = c_ListView2.GetCheck(i);
		if( bChecked == true )
		{
			isUser= true;

			LPOLESTR adsPath = new OLECHAR[MAX_PATH];
			wcscpy(adsPath,L"WinNT://");
			wcscat(adsPath,c_ListView2.GetItemText(i,1));
			wcscat(adsPath,L"/");
			wcscat(adsPath,c_ListView2.GetItemText(i,0));

			CoInitialize(NULL);
			IADsUser *pADsUser=NULL;
			HRESULT hr;
		
			if( defaultUser )
			{
				hr = ADsGetObject( adsPath, IID_IADsUser, (void**) &pADsUser );
			}
			else
			{
				hr = ADsOpenObject(adsPath,UserName,Password,ADS_SECURE_AUTHENTICATION, IID_IADsUser, (void**) &pADsUser);
			}

			if( !SUCCEEDED(hr) )
			{
				AfxMessageBox(L"Domain Users/Groups cant be Disabled.\nFor Local Users only, we can have this operation",MB_ICONINFORMATION,MB_OK);
				UncheckAll();
				return;
			}

			VARIANT var;
			CComBSTR sbstrUserFlags = "UserFlags";
			hr = pADsUser->Get(sbstrUserFlags, &var);
			
			V_I4(&var) |= ADS_UF_ACCOUNTDISABLE ;
			hr = pADsUser->Put(sbstrUserFlags, var);
			
			if( !SUCCEEDED(hr) )
			{
				LPOLESTR temp = new OLECHAR[MAX_PATH];
				
				if( hr == -2147024891 )
				{
					wcscpy(temp,L"You dont have permission to disable Users in \"");
					wcscat(temp,c_ListView2.GetItemText(i,1));
					wcscat(temp,L"\"");
					AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
				}
				else
					AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);

				UncheckAll();
				return;
			}
			hr=pADsUser->SetInfo();
			if( !SUCCEEDED(hr) )
			{
				LPOLESTR temp = new OLECHAR[MAX_PATH];

				if( hr == -2147024891 )
				{
					wcscpy(temp,L"You dont have permission to disable Users in \"");
					wcscat(temp,c_ListView2.GetItemText(i,1));
					wcscat(temp,L"\"");
					AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);

				}
				else
					AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);

				UncheckAll();
				return;
			}
			isDisabled=true;
		} // if
	}

	if( !isUser )
		AfxMessageBox(L"Please select atleast one user",MB_ICONINFORMATION,MB_OK);
	if( isDisabled )
		AfxMessageBox(L"Users are Disabled Successfully",MB_ICONINFORMATION,MB_OK);
	UncheckAll();

} // Disable Button

	// Refresh List View
void CLocalUserManagementDlg::RefreshListView()
{
	//int m=0;
	UserList.clear();
	//c_ListView2.DeleteAllItems();

	for(list<CString>::iterator iter=ComputerList.begin();iter!=ComputerList.end();)
	{
		LPOLESTR adsPath = new OLECHAR[MAX_PATH];
		wcscpy(adsPath,L"WinNT://");
		wcscat(adsPath,iter->GetString());
		wcscat(adsPath,L",computer");
		CoInitialize(NULL);
		IADsContainer *pCont=NULL;
		HRESULT hr;

		if( defaultUser )
		{
			hr = ADsGetObject( adsPath, IID_IADsContainer, (void**) &pCont );
		}
		else
		{
			hr = ADsOpenObject(adsPath,UserName,Password,ADS_SECURE_AUTHENTICATION, IID_IADsContainer, (void**) &pCont);
		}

		if( !SUCCEEDED(hr) )
		{
			iter=ComputerList.erase(iter);
			continue;
		}

		IEnumVARIANT *pEnum = NULL;
		hr = ADsBuildEnumerator( pCont, &pEnum );

		if ( ! SUCCEEDED(hr) )
		{
			iter=ComputerList.erase(iter);
			continue;
		}
        VARIANT var;
        ULONG lFetch;
        IADs *pChild=NULL;
        VariantInit(&var);
		
		list<list<CString>> temp;

        while( SUCCEEDED(ADsEnumerateNext( pEnum, 1, &var, &lFetch )) && lFetch == 1 )
        {
            hr = V_DISPATCH(&var)->QueryInterface( IID_IADs, (void**) &pChild );
            if ( SUCCEEDED(hr) )
            {
                BSTR bstrClass;
				pChild->get_Class(&bstrClass);
				
				if(  CComBSTR(bstrClass) == CComBSTR(L"User") )
				{
					list<CString> tempList;

					VARIANT var1,var2;
					BSTR bstrName;
					pChild->get_Name(&bstrName);
					pChild->Get(L"fullname",&var1);
					pChild->Get(L"description",&var2);

					tempList.push_back(bstrName);
					tempList.push_back(iter->GetString());
					tempList.push_back(var1.bstrVal);
					tempList.push_back(var2.bstrVal);
					//tempList.push_back(iter->GetString());
					temp.push_back(tempList);
					
	                SysFreeString(bstrName);
					VariantClear(&var1);
					VariantClear(&var2);
				}
                SysFreeString(bstrClass);
			 }
		  } //while
		  UserList.push_back(temp);
		  pChild->Release();
		  VariantClear(&var);
          ++iter;
		  CoUninitialize();

		} // for loop

		c_ComputerCombo.ResetContent();
		c_ComputerCombo.AddString(L"All Computers");						
		c_ComputerCombo.SelectString(0,L"All Computers");

				// Adding Computer Names in ComboBox
			int index=1;
			for(list<CString>::iterator iter=ComputerList.begin();iter!=ComputerList.end();++iter,index++)
			{
				c_ComputerCombo.InsertString(index,iter->GetString());
			}
//		}
}

void CLocalUserManagementDlg::UncheckAll()
{
	for( int i=0;i< c_ListView2.GetItemCount();i++)
	{
		c_ListView2.SetCheck(i,0);
	}
}

	// Delete Button
void CLocalUserManagementDlg::OnBnClickedDeletebutton()
{
	bool isUser=false;
	bool isDeleted=false;

	int i;
	for( i=0;i<c_ListView2.GetItemCount();i++)
	{
		bool bChecked = c_ListView2.GetCheck(i);
		if( bChecked == true )
			break;
	}
	if( i==c_ListView2.GetItemCount() )
	{
		AfxMessageBox(L"Please select atleast one user",MB_ICONINFORMATION,MB_OK);
		return;
	}

	if( AfxMessageBox(L"Are you sure you want to delete ?",MB_YESNO) == IDNO )
	{
		return;
	}

	for( int i=0;i<c_ListView2.GetItemCount();i++)
	{
		bool bChecked = c_ListView2.GetCheck(i);
		if( bChecked == true )
		{
			isUser= true;

			LPOLESTR adsPath = new OLECHAR[MAX_PATH];
			wcscpy(adsPath,L"WinNT://");
			wcscat(adsPath,c_ListView2.GetItemText(i,1));

			HRESULT hr ;
			IADsContainer *pCont=NULL;
 
			CoInitialize(NULL);
 
			if( defaultUser )
			{
				hr = ADsGetObject(adsPath,IID_IADsContainer,(void**) &pCont);
			}
			else
			{
				hr = ADsOpenObject(adsPath,UserName,Password,ADS_SECURE_AUTHENTICATION,IID_IADsContainer,(void**) &pCont);
			}
			if ( !SUCCEEDED(hr) )
			{
				LPOLESTR temp = new OLECHAR[MAX_PATH];
				wcscpy(temp,L"Unspecified Error occurred while accessing the computer \"");
				wcscat(temp,c_ListView2.GetItemText(i,1));
				wcscat(temp,L"\".");
				AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
				pCont->Release();
				return;
			}
			
			BSTR bstrUsername;
			bstrUsername=c_ListView2.GetItemText(i,0).AllocSysString();
			hr = pCont->Delete(CComBSTR("user"), bstrUsername);

			if ( !SUCCEEDED(hr) )
			{
				LPOLESTR temp = new OLECHAR[MAX_PATH];

				if( hr == -2147024891 )
				{
					wcscpy(temp,L"You dont have permission to delete Users in \"");
					wcscat(temp,c_ListView2.GetItemText(i,1));
					wcscat(temp,L"\". ");
					AfxMessageBox(temp,MB_ICONINFORMATION,MB_OK);
				}
				else if( hr == -2147467259 )
				{
					AfxMessageBox(L"Domain Users/Domain Groups can not be deleted. Local Users only can be deleted. ",MB_ICONINFORMATION,MB_OK);
				}
				else
					AfxMessageBox(L"Unspecified error occurred",MB_ICONINFORMATION,MB_OK);

				UncheckAll();
				return;
			}
			
			isDeleted=true;
			pCont->Release();
		} //if
	} // for

	if( !isUser )
		AfxMessageBox(L"Please select atleast one user",MB_ICONINFORMATION,MB_OK);
	if( isDeleted )
	{
		AfxMessageBox(L"Users are Removed Succesfully",MB_ICONINFORMATION,MB_OK);
		RefreshListView();
		OnCbnSelchangeCombo3();
	}
}	// Delete Button

	// Stop Button
void CLocalUserManagementDlg::OnBnClickedStopbutton()
{
	this->toBeStopped=true;
	c_InfoLabel.SetWindowText(L"Process is about to stop...");
	c_InfoLabel.GetParent()->UpdateWindow();
} // Stop Button

	// Stop Button
void CLocalUserManagementDlg::OnBnClickedStop2button()
{
	this->toBeStopped=true;
	c_Info2Label.SetWindowText(L"Process is about to stop...");
	c_Info2Label.GetParent()->UpdateWindow();
} // Stop Button

void CLocalUserManagementDlg::OnOK()
{
	//	OnBnClickedButton1();
	CWnd* pwndCtrl = GetFocus();
    int ctrl_ID = pwndCtrl->GetDlgCtrlID();

    switch (ctrl_ID)
	{	
		case IDC_EDIT1:
				OnBnClickedButton1();
                break;
        case IDC_EDIT2:
				OnBnClickedButton1();
                break;
        case IDC_COMBO1:
				OnBnClickedButton1();
                break;
        case IDC_SearchTextBox:
				OnStnClickedSearchpicturebox();
				break;
		case IDC_Search2TextBox:
				OnStnClickedSearch2picturebox(); 
                break;
		 case IDC_LIST2:
				//OnStnClickedSearchpicturebox();
				OnBnClickedLocalusersbutton();
                break;
        case IDOK: 
                CDialog::OnOK();
                break;
        default:
				OnBnClickedButton1();
                break;
    }
}
	
	// Clear Button
void CLocalUserManagementDlg::OnBnClickedClearbutton()
{
	c_DomainName.SetWindowText(L"");
	c_UserName.SetWindowText(L"");
	c_Password.SetWindowText(L"");
} // Clear Button

	// Search Button
void CLocalUserManagementDlg::OnStnClickedSearchpicturebox()
{
	LVCOLUMN lvcolumn;
	lvcolumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvcolumn.fmt = LVCFMT_LEFT;
	
	lvcolumn.cx = 288;
	lvcolumn.pszText = L"Distinguished Name";
	c_ListView1.InsertColumn(1,&lvcolumn);
	c_ListView1.DeleteColumn(2);
	
	CString searchName;
	c_SearchTextBox.GetWindowText(searchName);

	c_ListView1.DeleteAllItems();

	list<CString>::iterator iter1=AllComputersList.begin();
	list<CString>::iterator iter2=AllComputersLocationList.begin();
	int m=0;
	

	if( searchName.IsEmpty() )
	{
		for(;iter1!=AllComputersList.end();++iter1,++iter2)
		{
			c_ListView1.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
			c_ListView1.SetItemText(m,0,*iter1);
			c_ListView1.SetItemText(m,1,*iter2);
			m=m+1;

			////////////////////////////////////////////////////	
			int count=c_ListView1.GetItemCount();
			char c[10];
			itoa(count,c,10);
			CString MFCString;
			MFCString = c;
	
			LPOLESTR str = new OLECHAR[MAX_PATH];
			wcscpy(str,L"Total Computers : ");
			wcscat(str,MFCString);
			c_TotalComputersLabel.SetWindowText(str);
			////////////////////////////////////////////////////

		}
	}
	else
	{
		for(;iter1!=AllComputersList.end();++iter1,++iter2)
		{
			////////////////////// Converting ComputerName to char*
			char* computerNameString;
			CString cs = *iter1;
			int computerNameStringLength=cs.GetLength();
			computerNameString=(char*)malloc(computerNameStringLength+1);
			int ii;
			for( ii=0;ii<computerNameStringLength;ii++)
				computerNameString[ii]=(char)cs[ii];

			computerNameString[ii]='\0';
			//////////////////////////////////////////////

			
			/////////////////////// Converting SearchName to char*
			char* searchNameString;
			int searchNameStringLength=searchName.GetLength();
			searchNameString=(char*)malloc(searchNameStringLength+1);
			ii=0;
			for( ii=0;ii<searchNameStringLength;ii++)
				searchNameString[ii]=(char)searchName[ii];

			searchNameString[ii]='\0';
			//LPCTSTR str = CA2W(ch);
			//////////////////////////////////////////////////
			
			bool canInclude = false;

			//////// Checking whether searchString is available or not //////
			for( ii=0;ii<computerNameStringLength;ii++)
			{ 
				if( tolower(computerNameString[ii]) == tolower(searchNameString[0]) )
				{
					int k=ii+1;
					int j;
					for( j=1;j<searchNameStringLength;j++,k++ )
					{
						if( tolower(searchNameString[j]) != tolower(computerNameString[k]) )
						{
							canInclude=false;
							break;
						}
					} // for

					if( j == searchNameStringLength )
					{
						canInclude = true;
						break;
					}
				} // if( computerNameString[ii] == searchNameString[0] )
			} // for
			/////////////////////////////////////////////////////////

			if( canInclude )
			{
				c_ListView1.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
				c_ListView1.SetItemText(m,0,*iter1);
				c_ListView1.SetItemText(m,1,*iter2);
				m=m+1;

				////////////////////////////////////////////////////	
				
				////////////////////////////////////////////////////
			}
			    int count=c_ListView1.GetItemCount();
				char c[10];
				itoa(count,c,10);
				CString MFCString;
				MFCString = c;
	
				LPOLESTR str = new OLECHAR[MAX_PATH];
				wcscpy(str,L"Total Computers : ");
				wcscat(str,MFCString);
				c_TotalComputersLabel.SetWindowText(str);
			
		} // for loop
	} //else
} // Search Button

	// Home Button
void CLocalUserManagementDlg::OnStnClickedHomepicturecontrol()
{
	this->EndDialog(IDOK);
}	// Home Button

void CLocalUserManagementDlg::SortingListView()
{
	list<CString>::iterator it1=AllComputersList.end();
	--it1;
	list<CString>::iterator iterLocation1=AllComputersLocationList.begin();
	for( list<CString>::iterator iter1=AllComputersList.begin();iter1!=it1;++iter1,++iterLocation1)
	{
		list<CString>::iterator it2=iter1;
		++it2;

		list<CString>::iterator itLocation2=iterLocation1;
		++itLocation2;
	
		list<CString>::iterator iterLocation2=itLocation2;
		for( list<CString>::iterator iter2=it2;iter2!=AllComputersList.end();++iter2,++iterLocation2)
		{
			// sorting
			if( *iter1 > *iter2 )
			{
				CString temp=*iter1;
				temp=*iter1;
				*iter1=*iter2;
				*iter2=temp;

				temp=*iterLocation1;
				*iterLocation1=*iterLocation2;
				*iterLocation2=temp;
			}
		}
	} // for loop

	c_ListView1.DeleteAllItems();

	int m=0;
	iterLocation1=AllComputersLocationList.begin();
	for( list<CString>::iterator iter1=AllComputersList.begin();iter1!=AllComputersList.end();++iter1,++iterLocation1)
	{
		c_ListView1.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
		c_ListView1.SetItemText(m,0,*iter1);
		c_ListView1.SetItemText(m,1,*iterLocation1);
		m=m+1;
	}
}

	// Select All Button
void CLocalUserManagementDlg::OnBnClickedSelectall1button()
{
	for( int i=0;i<c_ListView1.GetItemCount();i++ )
		c_ListView1.SetCheck(i,1);
} // Select All Button

	// DeSelect All Button
void CLocalUserManagementDlg::OnBnClickedDeselectall1button()
{
	for( int i=0;i<c_ListView1.GetItemCount();i++ )
		c_ListView1.SetCheck(i,0);
} // DeSelect All Button

	// Select All Button
void CLocalUserManagementDlg::OnBnClickedSelectall2button()
{
	for( int i=0;i<c_ListView2.GetItemCount();i++ )
		c_ListView2.SetCheck(i,1);
} // Select All Button

	// DeSelect All Button
void CLocalUserManagementDlg::OnBnClickedDeselectall2button()
{
	for( int i=0;i<c_ListView2.GetItemCount();i++ )
		c_ListView2.SetCheck(i,0);
} // DeSelect All Button

	// Search Button
void CLocalUserManagementDlg::OnStnClickedSearch2picturebox()
{
	CString searchString;
	c_Search2TextBox.GetWindowText(searchString);

	c_ListView2.DeleteAllItems();

	////////////////////////////////////////////////////	
	int count=c_ListView2.GetItemCount();
	char c[10];
	itoa(count,c,10);
	CString MFCString;
	MFCString = c;
	
	LPOLESTR str = new OLECHAR[MAX_PATH];
	wcscpy(str,L"Total Users : ");
	wcscat(str,MFCString);
	c_TotalUsersLabel.SetWindowText(str);
	////////////////////////////////////////////////////

	if( searchString.IsEmpty() )
	{
		OnCbnSelchangeCombo3();
		return;
	}

	/////////////////////// Converting SearchName to char*
	char* searchNameString;
	int searchNameStringLength=searchString.GetLength();
	searchNameString=(char*)malloc(searchNameStringLength+1);
	int ii=0;
	for( ii=0;ii<searchNameStringLength;ii++)
		searchNameString[ii]=(char)searchString[ii];

	searchNameString[ii]='\0';
	//LPCTSTR str = CA2W(ch);
	//////////////////////////////////////////////////

	int row=0;

	for(list<list<CString>>::iterator iter1=this->SearchList.begin();iter1!=this->SearchList.end();++iter1)
	{
		list<CString> tempList=*iter1;

		int i=0;
		bool canInclude = false;

		for(list<CString>::iterator iter2=tempList.begin();iter2!=tempList.end();++iter2,i++)
		{
			if( i==0 || i==2 )
			{
				////////////////////// Converting userName to char*
				char* userNameString;
				CString cs = *iter2;
				int userNameStringLength=cs.GetLength();
				userNameString=(char*)malloc(userNameStringLength+1);
				int ii;
				for( ii=0;ii<userNameStringLength;ii++)
					userNameString[ii]=(char)cs[ii];

				userNameString[ii]='\0';
				//////////////////////////////////////////////

				//////// Checking whether searchString is available or not //////
				for( ii=0;ii<userNameStringLength;ii++)
				{ 
					if( tolower(userNameString[ii]) == tolower(searchNameString[0]) )
					{
						int k=ii+1;
						int j;
						for( j=1;j<searchNameStringLength;j++,k++ )
						{
							if( tolower(searchNameString[j]) != tolower(userNameString[k]) )
							{
								canInclude=false;
								break;
							}
						} // for

						if( j == searchNameStringLength )
						{
							canInclude = true;
							break;	
						}
					} // if( userNameString[ii] == searchNameString[0] )
				} // for
				/////////////////////////////////////////////////////////
				
				if( canInclude )
					break;
			} // if
		} // inner "for" loop

		if( canInclude )
		{
			int col=-1;
			
			c_ListView2.InsertItem(LVIF_TEXT|LVIF_STATE, row, _T(""),0, 0, 0, 0);
			for(list<CString>::iterator iter3=tempList.begin();iter3!=tempList.end();++iter3)
			{
				c_ListView2.SetItemText(row,++col,iter3->GetString());

				////////////////////////////////////////////////////	
				int count=c_ListView2.GetItemCount();
				char c[10];
				itoa(count,c,10);
				CString MFCString;
				MFCString = c;
	
				LPOLESTR str = new OLECHAR[MAX_PATH];
				wcscpy(str,L"Total Users : ");
				wcscat(str,MFCString);
				c_TotalUsersLabel.SetWindowText(str);
				////////////////////////////////////////////////////

			}
			row++;
		}
	} // inner "for" loop

} // Search Button

/*
urred",MB_ICONINFORMATION,MB_OK);
			//return;
			continue;
		}
		
		VARIANT var;
		IADs *pADs=NULL;
		ULONG lFetch;
		
		VariantInit(&var);
		
		while( (pEnum->Next(1,&var,&lFetch)==S_OK) && lFetch==1 )
		{
			BSTR bstrName;
			VARIANT var1,var2;
			
			VariantInit(&var1);
			VariantInit(&var1);
			IDispatch *pDisp=NULL;
			pDisp = V_DISPATCH(&var);
			pDisp->QueryInterface(IID_IADs,(void**)&pADs);
			pADs->get_ADsPath(&bstrName);
			
			/////////////////////// Converting UserName to char*
			char* userNameString;
			int userNameStringLength;
			userNameStringLength=SysStringLen(bstrName);
			userNameString=(char*)malloc(userNameStringLength+1);
			ii=0;
			for( ii=0;ii<userNameStringLength;ii++)
				userNameString[ii]=(char)bstrName[ii];

			userNameString[ii]='\0';
			//LPCTSTR str = CA2W(ch);
			//////////////////////////////////////////////////
			
			bool isLocalUser = false;

			//////// Checking whether user is local user or not //////
			for( ii=0;ii<userNameStringLength;ii++)
			{
				if( userNameString[ii] == computerNameString[0] )
				{
					int k=ii+1;
					int j;
					for( j=1;j<computerNameStringLength;j++,k++ )
					{
						if( computerNameString[j] != userNameString[k]	)
						{
							isLocalUser=false;
							break;
						}
					} // for

					if( j == computerNameStringLength )
					{
						isLocalUser = true;
						break;
					}
				} // if( userNameString[ii] == computerNameString[0] )
			} // for
			/////////////////////////////////////////////////////////

			thisObject->_this->c_ListView2.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
			if( isLocalUser )
			{
				int slashIndex;
				for( slashIndex=userNameStringLength-1 ; slashIndex >=0 ; slashIndex -- )
					if( userNameString[slashIndex] == '/' )
						break;

				char* newUserNameString;
				int newUserNameStringLength;
				newUserNameStringLength=userNameStringLength - slashIndex;
				newUserNameString=(char*)malloc(newUserNameStringLength+1);
				int i=0;
				for( ii=slashIndex+1;ii<userNameStringLength;ii++,i++)
					newUserNameString[i]=userNameString[ii];

				newUserNameString[i]='\0';
				
				LPCTSTR newName = CA2W(newUserNameString);
				thisObject->_this->c_ListView2.SetItemText(m,0,newName);
				pADs->Get(L"fullname",&var1);
				pADs->Get(L"description",&var2);
				thisObject->_this->c_ListView2.SetItemText(m,1,iter->GetString());
				thisObject->_this->c_ListView2.SetItemText(m,2,var1.bstrVal);
				thisObject->_this->c_ListView2.SetItemText(m,3,var2.bstrVal);
				//thisObject->_this->c_ListView2.SetItemText(m,3,iter->GetString());
				
				//////////////////////// Adding into SearchList....
				list<CString> tempSearchList;
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,0));
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,1));
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,2));
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,3));
				thisObject->_this->SearchList.push_back(tempSearchList);
				//////////////////////////////////////////////////////////////

			}
			else
			{
				int slashIndex;
				for( slashIndex=0 ; slashIndex < userNameStringLength; slashIndex ++ )
					if( userNameString[slashIndex] == '/' )
						break;

				char* newUserNameString;
				int newUserNameStringLength;
				newUserNameStringLength=userNameStringLength - slashIndex;
				newUserNameString=(char*)malloc(newUserNameStringLength+1);

				int i=0;
				for( ii=slashIndex+2;ii<userNameStringLength;ii++,i++)
					newUserNameString[i]=userNameString[ii];

				newUserNameString[i]='\0';
				LPCTSTR newName = CA2W(newUserNameString);
				thisObject->_this->c_ListView2.SetItemText(m,0,newName);

				BSTR bstrClass;
				pADs->get_Class(&bstrClass);
				
				thisObject->_this->c_ListView2.SetItemText(m,1,iter->GetString());

				if(  CComBSTR(bstrClass) == CComBSTR(L"User") )
					thisObject->_this->c_ListView2.SetItemText(m,3,L"Domain User");
				else if(  CComBSTR(bstrClass) == CComBSTR(L"Group") )
					thisObject->_this->c_ListView2.SetItemText(m,3,L"Domain Group");

				SysFreeString(bstrName);
				//thisObject->_this->c_ListView2.SetItemText(m,3,iter->GetString());

				//////////////////////// Adding into SearchList....
				list<CString> tempSearchList;
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,0));
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,1));
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,2));
				tempSearchList.push_back(thisObject->_this->c_ListView2.GetItemText(m,3));
				thisObject->_this->SearchList.push_back(tempSearchList);
				//////////////////////////////////////////////////////////////

			}
			
			m=m+1;

			////////////////////////////////////////////////////	
			int count=thisObject->_this->c_ListView2.GetItemCount();
			char c[10];
			itoa(count,c,10);
			CString MFCString;
			MFCString = c;
	
			LPOLESTR str = new OLECHAR[MAX_PATH];
			wcscpy(str,L"Total Users : ");
			wcscat(str,MFCString);
			thisObject->_this->c_TotalUsersLabel.SetWindowText(str);
			////////////////////////////////////////////////////


			SysFreeString(bstrName);

			if( pDisp )
				pDisp->Release();
		}  // while 

		VariantClear(&var);

		if( pEnum )
			pEnum->Release();
		if( pUnk )
			pUnk->Release();
	} // for loop

	thisObject->_this->FilterGroupBoxShow();
	thisObject->_this->c_Info2Label.ShowWindow(SW_HIDE);
	thisObject->_this->c_Stop2Button.ShowWindow(SW_HIDE);
	thisObject->_this->c_Progress2.ShowWindow(SW_HIDE);
	thisObject->_this->c_PropertiesButton.EnableWindow(1);
	thisObject->_this->c_ResetPasswordButton.EnableWindow(1);
	thisObject->_this->c_AddButton.EnableWindow(1);
	thisObject->_this->c_DeleteButton.EnableWindow(1);
	thisObject->_this->c_EnableButton.EnableWindow(1);
	thisObject->_this->c_DisableButton.EnableWindow(1);

	if( thisObject->_this->c_ListView2.GetItemCount() == 0 )
		AfxMessageBox(L"No users available in this group",MB_ICONINFORMATION,MB_OK);
	else
	{
	}

	////////////////////////////////////////////////////	
	int count=thisObject->_this->c_ListView2.GetItemCount();
	char c[10];
	itoa(count,c,10);
	CString MFCString;
	MFCString = c;
	
	LPOLESTR str = new OLECHAR[MAX_PATH];
	wcscpy(str,L"Total Users : ");
	wcscat(str,MFCString);
	thisObject->_this->c_TotalUsersLabel.SetWindowText(str);
	////////////////////////////////////////////////////

	thisObject->_this->c_Picture1.ShowWindow(SW_SHOW);
	thisObject->_this->c_Picture2.ShowWindow(SW_HIDE);
	thisObject->_this->c_Picture4.ShowWindow(SW_HIDE);
	thisObject->_this->c_Picture5.ShowWindow(SW_SHOW);

	return TRUE;
}

UINT GetLocalUsersThread( LPVOID Param )
{
	THREADSTRUCT* thisObject = (THREADSTRUCT*)Param;
	int m=0;
	list<CString> compList;
	thisObject->_this->UserList.clear();
	thisObject->_this->c_ListView2.DeleteAllItems();
	int current=1;
	int size=thisObject->_this->ComputerList.size();
	thisObject->_this->c_Progress1.SetPos(0);
	int pos=0;

	list<int>::iterator iter2=thisObject->_this->ComputerListIndex.begin();
	for(list<CString>::iterator iter=thisObject->_this->ComputerList.begin();iter!=thisObject->_this->ComputerList.end();)
	{
		if( thisObject->_this->toBeStopped == true )
			break;
		///////////////////////////////////////////////////////////
		LPOLESTR infoText = new OLECHAR[MAX_PATH];
		wcscpy(infoText,L"Retrieving Local Users from \"");
		wcscat(infoText,iter->GetString());
		wcscat(infoText,L"\"...");
		thisObject->_this->c_InfoLabel.SetWindowText(infoText);
		thisObject->_this->c_InfoLabel.GetParent()->UpdateWindow();
		int percentage=(int)((100/size)*current);
		pos=pos+percentage;
		if( pos < 5 )
			thisObject->_this->c_Progress1.SetPos(5);
		//thisObject->_this->c_Progress1.SetPos(thisObject->_this->c_Progress1.GetPos()+percentage);
		else
			thisObject->_this->c_Progress1.SetPos(pos);
		//thisObject->_this->c_Progress1.StepIt();
		///////////////////////////////////////////////////////////

		LPOLESTR adsPath = new OLECHAR[MAX_PATH];
		wcscpy(adsPath,L"WinNT://");
		wcscat(adsPath,iter->GetString());
		wcscat(adsPath,L",computer");
		CoInitialize(NULL);
		IADsContainer *pCont=NULL;
		HRESULT hr;

		if( thisObject->_this->defaultUser )
		{
			hr = ADsGetObject( adsPath, IID_IADsContainer, (void**) &pCont );
		}
		else
		{
			hr = ADsOpenObject(adsPath,thisObject->_this->UserName,thisObject->_this->Password,ADS_SECURE_AUTHENTICATION, IID_IADsContainer, (void**) &pCont);
		}

		if( !SUCCEEDED(hr) )
		{
			//AfxMessageBox(L"Connection failed,try to get the local users some time later.",MB_ICONINFORMATION,MB_OK);
			thisObject->_this->c_ListView1.SetItemText(*iter2,1,L"Not Accessible");
			iter=thisObject->_this->ComputerList.erase(iter);
			iter2=thisObject->_this->ComputerListIndex.erase(iter2);
			continue;
		}

		IEnumVARIANT *pEnum = NULL;
		hr = ADsBuildEnumerator( pCont, &pEnum );

		if ( ! SUCCEEDED(hr) )
		{
			thisObject->_this->c_ListView1.SetItemText(*iter2,1,L"Not Accessible");
			iter=thisObject->_this->ComputerList.erase(iter);
			iter2=thisObject->_this->ComputerListIndex.erase(iter2);
			continue;
		}
        VARIANT var;
        ULONG lFetch;
        IADs *pChild=NULL;
        VariantInit(&var);
		
		list<list<CString>> temp;
		
        while( SUCCEEDED(ADsEnumerateNext( pEnum, 1, &var, &lFetch )) && lFetch == 1 )
        {
            hr = V_DISPATCH(&var)->QueryInterface( IID_IADs, (void**) &pChild );
            if ( SUCCEEDED(hr) )
            {
                BSTR bstrClass;
				pChild->get_Class(&bstrClass);
				
				if(  CComBSTR(bstrClass) == CComBSTR(L"User") )
				{
					list<CString> tempList;

					VARIANT var1,var2;
					BSTR bstrName;
					pChild->get_Name(&bstrName);
					pChild->Get(L"fullname",&var1);
					pChild->Get(L"description",&var2);
					thisObject->_this->c_ListView2.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
					thisObject->_this->c_ListView2.SetItemText(m,0,bstrName);
					thisObject->_this->c_ListView2.SetItemText(m,1,iter->GetString());
					thisObject->_this->c_ListView2.SetItemText(m,2,var1.bstrVal);
					thisObject->_this->c_ListView2.SetItemText(m,3,var2.bstrVal);
					//thisObject->_this->c_ListView2.SetItemText(m,3,iter->GetString());
					m=m+1;

					tempList.push_back(bstrName);
					tempList.push_back(iter->GetString());
					tempList.push_back(var1.bstrVal);
					tempList.push_back(var2.bstrVal);
					//tempList.push_back(iter->GetString());
					temp.push_back(tempList);
					
	                SysFreeString(bstrName);
					VariantClear(&var1);
					VariantClear(&var2);
				}

                SysFreeString(bstrClass);
			 }
		  } //while
		
		  thisObject->_this->c_ListView1.SetItemText(*iter2,1,L"Retrieved");
		  ++iter2;
		  compList.push_back(*iter);
		  thisObject->_this->UserList.push_back(temp);
		  pChild->Release();
		  VariantClear(&var);
          ++iter;
		 CoUninitialize();

	} // for loop

	thisObject->_this->c_Picture1.ShowWindow(SW_HIDE);
	thisObject->_this->c_Picture2.ShowWindow(SW_HIDE);
	thisObject->_this->c_Picture4.ShowWindow(SW_SHOW);
	thisObject->_this->c_Picture5.ShowWindow(SW_SHOW);

	thisObject->_this->c_LocalUsersButton.ShowWindow(SW_SHOW);
	thisObject->_this->c_InfoLabel.ShowWindow(SW_HIDE);
	thisObject->_this->c_StopButton.ShowWindow(SW_HIDE);
	thisObject->_this->c_Progress1.ShowWindow(SW_HIDE);

	thisObject->_this->ComputerList=compList;

	if( thisObject->_this->c_ListView2.GetItemCount() > 0 )
	{
		thisObject->_this->InformationGroupBoxHide();
		thisObject->_this->CredentialsGroupBoxHide();
		thisObject->_this->ComputersGroupBoxHide();
		thisObject->_this->LocalUsersGroupBoxShow();
		
		thisObject->_this->c_ComputerCombo.ResetContent();
		thisObject->_this->c_ComputerCombo.AddString(L"All Computers");						
		

			// Adding Computer Names in ComboBox
		int index=1;
		for(list<CString>::iterator iter=thisObject->_this->ComputerList.begin();iter!=thisObject->_this->ComputerList.end();++iter,index++)
		{
			thisObject->_this->c_ComputerCombo.InsertString(index,iter->GetString());
		}

		thisObject->_this->c_ComputerCombo.SelectString(0,L"All Computers");

		//thisObject->_this->OnCbnSelchangeCombo2();
		thisObject->_this->c_GroupCombo.Res

		*/


void CLocalUserManagementDlg::OnBnClickedExport()
{
	
	int row=c_ListView2.GetItemCount();
	int s;
	if(row == 0)
		MessageBox(L"No Data To Export",L"Local User Management" ,MB_OK | MB_ICONINFORMATION);
	else
	{
		LPWSTR rgpwszAttributes[] = {L"User Name",L"Machine Name",L"Full Name",L"Description"};
		int noOfColmns = (sizeof(rgpwszAttributes)/sizeof(LPWSTR));
		CFile f;
		char strFilter[] = { "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*||" };
		CFileDialog FileDlg(FALSE, CString(".csv"), NULL, 0, CString(strFilter));
		if( FileDlg.DoModal() == IDOK )
		{
			f.Open(FileDlg.GetFileName(), CFile::modeCreate | CFile::modeWrite);
			CArchive ar(&f, CArchive::store);
			CString st,r,r1;
			wchar_t *attstr;
			for(unsigned int k=0;k<noOfColmns;k++)
			{
				s = s+wcslen(rgpwszAttributes[k]);
			}
			s=s+4; //1 for null terminating character and 3 for comma's.
			attstr = (wchar_t *)calloc(s,sizeof(wchar_t));
			for(unsigned int x = 0;x < noOfColmns;x++)
			{
				wcscat(attstr,rgpwszAttributes[x]);
				if(x < noOfColmns)			
					wcscat(attstr,L",");			
			}			
			st = attstr;
			ar.WriteString(st);
			ar.WriteString(L"\n");
			free(attstr);
			for(unsigned int i = 0;i < row;i ++)
			{
				for(unsigned int j = 0;j < noOfColmns;j ++)
				{
					ar.WriteString(this->c_ListView2.GetItemText(i,j));
					if(j < noOfColmns)
						ar.WriteString(L",");
				}
				ar.WriteString(L"\n");			
			}
			ar.Close();
			MessageBox(L"Successfully Exported",L"Local User Management" ,MB_OK | MB_ICONINFORMATION);	
		}		
		else
			return;
		f.Close();
		
	}	
}
		
	
		void CLocalUserManagementDlg::OnBnClickedButton3()
		{
			// TODO: Add your control notification handler code here
			
			MultipleUserAddForm addform(2);
	        addform.DoModal();
			fstream myfile (addform.fileName);
		    if(addform.fileName.IsEmpty())return;
		  	string line="";
		
			char *word=(char*)malloc(1000);
			int i=0,j=0,k=0,comma_count=0,position=0,header_position=-1,header_comma_count=0,quotation=0,distinguishedname_position=-1,is_header=1;                                                                                                              
			if (myfile.is_open())
                  {
					  c_ListView1.DeleteAllItems();
					  AllComputersList.clear();
			          AllComputersLocationList.clear();
					  while(getline(myfile,line)){
						    line[line.length()]='\n';
							comma_count=0;
							position=0;
							quotation=0;
							for(i=0,j=0;i<=line.length();i++)                          
						   {
							 if(is_header) 
								 word[j++]=tolower(line[i]);
							 else 
								 word[j++]=line[i];
							 if(word[j-1]=='"')quotation++;
							 if(line[i]==','||line[i]=='\n'){
								 if(word[0]!='"')
								 word[j-1]='\0';
								 else if(word[0]=='"'&&word[j-2]=='"')
								 word[j-1]='\0';
								 else{
						         while(1){
									 if(line[i]=='\n')break;
									 if(line[i]=='"')break;
									 if(is_header)
									 word[j++]=tolower(line[++i]);
									 else
									 word[j++]=line[++i];
									 if(word[j-1]=='"')quotation++;
									 }
								 if(line[i]=='\n'){
								 	 AfxMessageBox(L"Not a Valid CSV Format");
									 return;
								 }
								 if(line[i]=='"'){
								 if(line[i+1]==','||line[i+1]=='\n')
									 word[j]='\0';
								 else if(line[i+1]==';'){
								     i++;
									 while(1){
									 if(is_header) 
									 word[j]=tolower(line[i]);
									 else
									 word[j]=line[i];
									 if(word[j]=='"')quotation++;
									 j++;
									 i++;
									 if(line[i]=='\n')break;
									 if(line[i]==';'&&((line[i+1]!='"'&&line[i-1]=='"')||(line[i-1]!='"'&&line[i+1]=='"'))){
									 MessageBox(L"Not a Valid CSV Format");
									 return;
									 }
									 if(line[i]=='"'&&line[i+1]==',')
									 {
										 if(is_header)
										 word[j]=tolower(line[i]);
										 else
										 word[j]=line[i];
										 if(word[j]=='"')quotation++;
									     word[j+1]='\0';
										 j++;
									 break;
									 }
									 }
								  if(line[i]=='\n'){
								 	 AfxMessageBox(L"Not a Valid CSV Format");
									 return;
								 }
								 }
								 else if(line[i+1]=='"'){
									 i++;
								  while(1){
									 if(is_header)
									 word[j]=tolower(line[i]);
									 else
								     word[j]=line[i];
									 if(word[j]=='"')quotation++;
									 j++;
									 i++;
									 if(line[i]=='\n')break;
									 if(line[i]=='"'&&line[i+1]==',')
									 {
										 if(is_header)
										 word[j]=tolower(line[i]);
										 else
										 word[j]=line[i];
										 if(word[j]=='"')quotation++;
									     word[j+1]='\0';
										 j++;
									  break;
									 }
									 }
								  if(line[i]=='\n'){
								 	 AfxMessageBox(L"Not a Valid CSV Format");
									 return;
								 }
								 }
								 else{
									 AfxMessageBox(L"Not a Valid CSV Format");
									 return;
								 }
								 }
								 i++;
								 j++;
								 }
								 
								
								 
								 if(!(line[i]=='\n')){
									 if(is_header)
									 header_comma_count++;
									 else
									 comma_count++;   
								 }                                     
								if(((word[0]!='"')&&(word[j-2]=='"'))||((word[0]=='"')&&(word[j-2]!='"'))||(word[0]=='"'&&word[1]=='\0')){
								 AfxMessageBox(L"Not a Valid CSV Format");
								 return;	
								}
							
							 if(is_header)
							 {
          //// if there are name and cn and computername field in given csv , the order of priority will be name>cn>computername ////
								 if(((strcmp(word,"\"name\""))==0)||((strcmp(word,"name"))==0)){
									  k=3;                                                            //prioritising variable
									  header_position=position;
									  position++;
								     }
								else if(((strcmp(word,"\"cn\"")==0)||(strcmp(word,"cn"))==0)){
									 if(k<3){
										 header_position=position;
									     k=2;
									 }
								position++;
								}
								else if(((strcmp(word,"\"computername\"")==0)||(strcmp(word,"computername"))==0)){
									 if(k<2){
										 header_position=position;
									      k=1;
									 }
								position++;
								}
							    else 
									  position++;
								 
								if(((strcmp(word,"\"distinguished name\"")==0)||(strcmp(word,"distinguished name"))==0)||(strcmp(word,"distinguishedname")==0)||(strcmp(word,"\"distinguishedname\"")==0))
								distinguishedname_position=position;
								
								if(((word[0]!='"')&&(word[j-2]=='"'))||((word[0]=='"')&&(word[j-2]!='"'))||(word[0]=='"'&&word[1]=='\0')){
								 AfxMessageBox(L"Not a Valid CSV Format");
								 return;	
								}
							 }
							 else
							 {
							 position++;
							 if((position-1)==header_position) {
							 for(k=0;word[k]=='"';k++);
							 for(j=0;word[j+k]!='\0';j++){
							 word[j]=word[j+k];
						    }
							if((j-k)>0)
							 word[j-k]='\0';
							else 
							 word[0]='\0';
							 CString cs(word);				     
							 AllComputersList.push_back(cs);
							}
						    
							if(distinguishedname_position!=-1){
							CString cs(word);
							if(position==distinguishedname_position) {
							AllComputersLocationList.push_back(cs);
						   	}
							}
							}
							if(quotation%2!=0){
					        AfxMessageBox(L"Not a Valid CSV Format");
					        return;
					        }
								 word[0]='\0';
								 j=0;
								 quotation=0;
							 }
				     	  }    
						  if(is_header){
						  if(header_position==-1){
					         AfxMessageBox(L"No \"name/cn/ComputerName\" field in the csv file");
							 return;
						  }
						  }
						  else{
					  	  if(comma_count!=header_comma_count){
								  AfxMessageBox(L"Not a Valid CSV Format");
								  return;
							  }
						  }
						  is_header=0;
						
				}//while
			}
		 		else{
					 MessageBox(L"File does not exist");
					 return;				 
				}
	  free(word);
	  list<CString>::iterator iter1=AllComputersList.begin();
	  int m=0;
	  for(;iter1!=AllComputersList.end();++iter1)
		{
			c_ListView1.InsertItem(LVIF_TEXT|LVIF_STATE, m, _T(""),0, 0, 0, 0);
			c_ListView1.SetItemText(m,0,*iter1);
			m=m+1;
		    int count=c_ListView1.GetItemCount();
		    char c[10];
		    itoa(count,c,10);
		    CString MFCString;
		    MFCString = c;
	
		    LPOLESTR str = new OLECHAR[MAX_PATH];
		    wcscpy(str,L"Total Computers : ");
		    wcscat(str,MFCString);
		    c_TotalComputersLabel.SetWindowText(str);
	    }
	        m=0;
	  if(distinguishedname_position!=-1){
		    list<CString>::iterator iter2=AllComputersLocationList.begin();
		    for(;iter2!=AllComputersLocationList.end();++iter2){
      		c_ListView1.SetItemText(m,1,*iter2);
			m=m+1;
	  }
	  }
	}  