// DCMonitoringToolDlg.cpp : implementation file
//

#include "stdafx.h"
#include "DCMonitoringTool.h"
#include "DCMonitoringToolDlg.h"
#include ".\dcmonitoringtooldlg.h"

#include "DomainControllerSubDlg.h"
#include "afxext.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

double CPUUsage = 0;
int size = 0 , cursel = 0 , noException = 1;
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


// CDCMonitoringToolDlg dialog


CDCMonitoringToolDlg::CDCMonitoringToolDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CDCMonitoringToolDlg::IDD, pParent)
	, V_DomainNameEdit(_T(""))
	, V_UserNameEdit(_T(""))
	, V_PasswordEdit(_T(""))
{
	//m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_hIcon = AfxGetApp()->LoadIcon(IDI_ICON1);
}

void CDCMonitoringToolDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);	
	DDX_Control(pDX, IDC_TAB1, C_ParameterTab);
	DDX_Control(pDX, IDC_BUTTON1, C_SelectDCBtn);
	DDX_Control(pDX, IDC_BUTTON2, C_Home);
	DDX_Text(pDX, IDC_EDIT1, V_DomainNameEdit);
	DDX_Control(pDX, IDC_EDIT1, C_DomainNameEdit);
	DDX_Control(pDX, IDC_EDIT2, C_UserNameEdit);
	DDX_Text(pDX, IDC_EDIT2, V_UserNameEdit);
	DDX_Control(pDX, IDC_EDIT3, C_PasswordEdit);
	DDX_Text(pDX, IDC_EDIT3, V_PasswordEdit);		
	DDX_Control(pDX, IDC_LIST1, C_ParametersListCtrl);
	DDX_Control(pDX, ID_HYPERLINK, C_Link);
}

BEGIN_MESSAGE_MAP(CDCMonitoringToolDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_TIMER()
	//}}AFX_MSG_MAP	
	ON_BN_CLICKED(IDC_BUTTON1, OnBnClickedButton1)
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB1, OnTcnSelchangeTab1)
	ON_BN_CLICKED(IDC_BUTTON2, &CDCMonitoringToolDlg::OnBnClickedButton2)
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
END_MESSAGE_MAP()


// CDCMonitoringToolDlg message handlers

BOOL CDCMonitoringToolDlg::OnInitDialog()
{
	CDialog::OnInitDialog();
	 CFont *m_pFont = new CFont();
     m_pFont->CreatePointFont(165, _T("Arial"));
     GetDlgItem(IDC_STATIC9)->SetFont(m_pFont, TRUE);

	C_SelectDCBtn.SetFaceColor(RGB(0,133,183),true);
	C_SelectDCBtn.SetTextColor(RGB(255,255,255));

	bitmap_home.LoadBitmapA(IDB_BITMAP4);
	HBITMAP hBitmap = (HBITMAP) bitmap_home.GetSafeHandle();
	C_Home.SetBitmap(hBitmap);

	DWORD dwRet = NULL;
	PDOMAIN_CONTROLLER_INFOA pdci;  
	dwRet = DsGetDcName(NULL, 
						NULL, 
						NULL, 
						NULL, 
						DS_TIMESERV_REQUIRED,					
						&pdci);
	if(NO_ERROR == dwRet)			
		this->C_DomainNameEdit.SetWindowText(pdci->DomainName);	
		
	LPSTR ParameterArray[] = {"CPU Utilization","Memory Utilization","Disk Utilization","Page Reads/sec","Page Writes/sec","Handle Count(LSASS)","Handle Count(NTFRS)","File Reads/sec","File Writes/sec","Interrupt Time","Page Faults/sec","Pages Input/sec"," Pages Output/sec"};
	for(unsigned int i = 0; i < sizeof(ParameterArray) / sizeof(LPWSTR) ; i ++)
	{
		TC_ITEM tcItem;
		tcItem.mask = TCIF_TEXT;
		tcItem.pszText = ParameterArray[i];
		this->C_ParameterTab.InsertItem(i,&tcItem);
	}	

	this->InsertColumn("CPU Utilization");

	this->C_ParametersListCtrl.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES);

	this->C_Link.SetURL("http://manageengine.adventnet.com/products/ad-manager/");	
	this->C_Link.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));

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
	
	btnHome.Create ("", WS_VISIBLE | BS_OWNERDRAW| WS_EX_CLIENTEDGE,CRect(20,40,50,70) , this, 1001 );
	btnHome.LoadBitmaps(135,0,0,0);
	CButton *btnOK;
	// Get a handle to each of the existing buttons

	btnOK = reinterpret_cast<CButton *>(GetDlgItem(IDC_BUTTON2));
	// Get the style of the button(s)

	LONG GWLOK = GetWindowLong(btnOK->m_hWnd, GWL_STYLE);
	// Change the button's style to BS_OWNERDRAW

	SetWindowLong(btnOK->m_hWnd, GWL_STYLE, GWLOK | BS_OWNERDRAW);
	// Subclass each button
	btnHome.SubclassDlgItem(IDC_BUTTON2, this); 
	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CDCMonitoringToolDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	RevertToSelf();
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

void CDCMonitoringToolDlg::OnPaint() 
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
HCURSOR CDCMonitoringToolDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CDCMonitoringToolDlg::OnBnClickedButton1()
{
	this->UpdateData(TRUE);
	noException=1;
	selDC.Empty();
	selCnt = 0;
	KillTimer(ID_CLK_TIMER);
	this->C_ParametersListCtrl.DeleteAllItems();

	if(this->CheckEmpty(V_DomainNameEdit))
	{
		if(this->CheckEmpty(V_UserNameEdit))
		{
			if(this->CheckEmpty(V_PasswordEdit))
			{
				GetAllDatas();
				if(IsValidDatas())
				{	
					CDomainControllerSubDlg dlgObj;
					dlgObj.DoModal();
					Impersonate();
					if(timer == 1){
						SetTimer(ID_CLK_TIMER, 1000, NULL);	
						
					}
				}
			}
		}
	}	
}
void CDCMonitoringToolDlg::Impersonate()
{
	HANDLE userToken_;
	CString domain = this->V_DomainNameEdit.AllocSysString();	
	CString User = this->V_UserNameEdit.AllocSysString();
	CString Pass = this->V_PasswordEdit.AllocSysString();
	BOOL bLoggedOn = FALSE;

	bLoggedOn = LogonUser(User, domain,Pass, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_WINNT50, &userToken_ );
	if(bLoggedOn)
	{
		bLoggedOn = ImpersonateLoggedOnUser(userToken_);
		CloseHandle(userToken_);
	}
}

void CDCMonitoringToolDlg::GetAllDatas()
{	
	wchar_t subdc[30],*subdcptr;
	BSTR domaincomponentptr,ldappathstr;	
	CString ldappath = "LDAP://OU=Domain Controllers,";
	ldappathptr = ldappath2;

	this->UpdateData(TRUE);			
	domaincomponentptr = this->V_DomainNameEdit.AllocSysString();	
	Username = this->V_UserNameEdit.AllocSysString();
	Password = this->V_PasswordEdit.AllocSysString();	
	ldappathstr = ldappath.AllocSysString();		
	wcscpy(ldappathptr,ldappathstr);	
	subdcptr = subdc;
	subdcptr = wcstok(domaincomponentptr,L".");
	wcscat(ldappathptr,L"DC=");	
	while(subdcptr != NULL)
	{		
		wcscat(ldappathptr,subdcptr);		
		subdcptr = wcstok(NULL,L".");
		if(subdcptr != NULL)
			wcscat(ldappathptr,L",DC=");
	}		
}

BOOL CDCMonitoringToolDlg::IsValidDatas()
{
	CoInitialize(NULL);
	IDirectorySearch *pDSSearch;
	HRESULT hr = ADsOpenObject(ldappathptr,Username,Password,ADS_SECURE_AUTHENTICATION,IID_IDirectorySearch,(void **)&pDSSearch);
	if(SUCCEEDED(hr))
		return(TRUE);
	else
	{
		MessageBox("Invalid User Name or Password","DC Monitoring",MB_OK | MB_ICONINFORMATION);
		return(FALSE);
	}
	CoUninitialize();
}

void CDCMonitoringToolDlg::GetCPUUsage(LPSTR SetDCName)
{	
	CoInitialize(NULL);
	int i = 0;

	try
	{
		System::String * systemstring1 = new String(SetDCName);
		PerformanceCounter* PC = new PerformanceCounter();

		PC->set_MachineName(systemstring1);		
		PC->CategoryName=S"Processor";
		PC->CounterName=S"% Processor Time";		
		PC->InstanceName=S"_Total";

		PC->NextValue();	

		System::Threading::Thread::Sleep(1000);

		CPUUsage = PC->NextValue();
	}

	catch(Exception *e)
	{
		noException = 0;			
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);		
		
	}

	if(noException == 1)
		this->SetItem(SetDCName,CPUUsage);

	CoUninitialize();
}

void CDCMonitoringToolDlg::GetMemoryUtilization(LPSTR SetDCName)
{		
	CoInitialize(NULL);
	double TotalPhysicalMemoryUsage = 0;	
	
	try
	{
		System::String * systemstring1 = new String(SetDCName);		
		System::Diagnostics::Process * myProcess[] = System::Diagnostics::Process::GetProcesses(systemstring1);
		int i = 0;
		
	
		for(i = 0 ; ;i ++)
		{
			try
			{	
				System::String *resultstr=NULL;						
										
				resultstr = resultstr->Copy(myProcess[i]->ProcessName);					
	
				if(resultstr->Compare(resultstr,"Idle") == 0)													
					goto l;			
				else																	
					TotalPhysicalMemoryUsage = TotalPhysicalMemoryUsage +( myProcess[i]->WorkingSet/1024);			
			}
			catch(Exception * e)
			{
				noException = 0;				
				char msg[300];
				sprintf(msg,"%s",e->get_Message());
				MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);	
				
			}
		}
	}
	catch(Exception *e)
	{
		noException = 0;	
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);
		
	}

l:	if(noException == 1)
		this->SetItem(SetDCName,TotalPhysicalMemoryUsage);

	CoUninitialize();
}

void CDCMonitoringToolDlg::GetDiskUtilization(LPSTR SetDCName)
{		
	double TotalPrivateMemorySize = 0;

	CoInitialize(NULL);
	try
	{
		System::String * systemstring1 = new String(SetDCName);		
		System::Diagnostics::Process * myProcess[] = System::Diagnostics::Process::GetProcesses(systemstring1);
		int i = 0;	
	
		for(i = 0 ; ;i ++)
		{
			try
			{	
				System::String *resultstr=NULL;
									
				resultstr = resultstr->Copy(myProcess[i]->ProcessName);					
	
				if(resultstr->Compare(resultstr,"Idle") == 0)													
					goto l;			
				else																	
					TotalPrivateMemorySize = TotalPrivateMemorySize +( myProcess[i]->PrivateMemorySize/1024);			
			}
			catch(Exception *e)
			{
				noException = 0;				
				char msg[300];
				sprintf(msg,"%s",e->get_Message());
				MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);		
				
			}
		}
	}
	catch(Exception *e)
	{
		noException = 0;
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);
		
	}

l:	if(noException == 1)
		this->SetItem(SetDCName,TotalPrivateMemorySize);

	CoUninitialize();
}

void CDCMonitoringToolDlg::GetPageReadsPerSec(LPSTR SetDCName)
{	
	CoInitialize(NULL);
	double TotalPageReadsPerSec = 0;
		
	try
	{
		System::String * systemstring1 = new String(SetDCName);
		PerformanceCounter* PC = new PerformanceCounter();

		PC->set_MachineName(systemstring1);		
		PC->CategoryName=S"Memory";
		PC->CounterName=S"Page Reads/sec";		
		PC->InstanceName=S"";

		PC->NextValue();	

		System::Threading::Thread::Sleep(1000);

		TotalPageReadsPerSec = PC->NextValue();
	}

	catch(Exception *e)
	{
		noException = 0;			
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);		
		
	}

	if(noException == 1)
		this->SetItem(SetDCName,TotalPageReadsPerSec);

	CoUninitialize();
}

void CDCMonitoringToolDlg::GetPageWritesPerSec(LPSTR SetDCName)
{	
	CoInitialize(NULL);
	double TotalPageWritesPerSec = 0;
		
	try
	{
		System::String * systemstring1 = new String(SetDCName);
		PerformanceCounter* PC = new PerformanceCounter();

		PC->set_MachineName(systemstring1);		
		PC->CategoryName=S"Memory";
		PC->CounterName=S"Page Writes/sec";		
		PC->InstanceName=S"";

		PC->NextValue();	

		System::Threading::Thread::Sleep(1000);

		TotalPageWritesPerSec = PC->NextValue();
	}

	catch(Exception *e)
	{
		noException = 0;			
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);	
		
	}

	if(noException == 1)
		this->SetItem(SetDCName,TotalPageWritesPerSec);

	CoUninitialize();
}

void CDCMonitoringToolDlg::GetHandleCountLSASS(LPSTR SetDCName)
{	
	CoInitialize(NULL);
	double TotalHandleCountLSASS = 0;
		
	try
	{
		System::String * systemstring1 = new String(SetDCName);
		PerformanceCounter* PC = new PerformanceCounter();

		PC->set_MachineName(systemstring1);		
		PC->CategoryName=S"Process";
		PC->CounterName=S"Handle Count";		
		PC->InstanceName=S"LSASS";

		PC->NextValue();	

		System::Threading::Thread::Sleep(1000);

		TotalHandleCountLSASS = PC->NextValue();
	}

	catch(Exception *e)
	{
		noException = 0;			
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);		
		
	}

	if(noException == 1)
		this->SetItem(SetDCName,TotalHandleCountLSASS);

	CoUninitialize();
}

void CDCMonitoringToolDlg::GetHandleCountNTFRS(LPSTR SetDCName)
{	
	CoInitialize(NULL);
	double TotalHandleCountNTFRS = 0;
		
	try
	{
		System::String * systemstring1 = new String(SetDCName);
		PerformanceCounter* PC = new PerformanceCounter();

		PC->set_MachineName(systemstring1);		
		PC->CategoryName=S"Process";
		PC->CounterName=S"Handle Count";		
		PC->InstanceName=S"NTFRS";

		PC->NextValue();	

		System::Threading::Thread::Sleep(1000);

		TotalHandleCountNTFRS = PC->NextValue();
	}

	catch(Exception *e)
	{
		noException = 0;			
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);	
		
	}

	if(noException == 1)
		this->SetItem(SetDCName,TotalHandleCountNTFRS);

	CoUninitialize();
}

void CDCMonitoringToolDlg::GetFileReadsPerSec(LPSTR SetDCName)
{	
	CoInitialize(NULL);
	double TotalFileReadsPerSec = 0;
		
	try
	{
		System::String * systemstring1 = new String(SetDCName);
		PerformanceCounter* PC = new PerformanceCounter();

		PC->set_MachineName(systemstring1);		
		PC->CategoryName=S"System";
		PC->CounterName=S"File Read Operations/sec";		
		PC->InstanceName=S"";

		PC->NextValue();	

		System::Threading::Thread::Sleep(1000);

		TotalFileReadsPerSec = PC->NextValue();
	}

	catch(Exception *e)
	{
		noException = 0;			
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);	
		
	}

	if(noException == 1)
		this->SetItem(SetDCName,TotalFileReadsPerSec);

	CoUninitialize();
}

void CDCMonitoringToolDlg::GetFileWritesPerSec(LPSTR SetDCName)
{	
	CoInitialize(NULL);
	double TotalFileWritesPerSec = 0;
		
	try
	{
		System::String * systemstring1 = new String(SetDCName);
		PerformanceCounter* PC = new PerformanceCounter();

		PC->set_MachineName(systemstring1);		
		PC->CategoryName=S"System";
		PC->CounterName=S"File Write Operations/sec";		
		PC->InstanceName=S"";

		PC->NextValue();	

		System::Threading::Thread::Sleep(1000);

		TotalFileWritesPerSec = PC->NextValue();
	}

	catch(Exception *e)
	{
		noException = 0;			
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);		
		
	}

	if(noException == 1)
		this->SetItem(SetDCName,TotalFileWritesPerSec);

	CoUninitialize();
}

void CDCMonitoringToolDlg::GetInterruptTime(LPSTR SetDCName)
{	
	CoInitialize(NULL);
	double TotalInterruptTime = 0;
		
	try
	{
		System::String * systemstring1 = new String(SetDCName);
		PerformanceCounter* PC = new PerformanceCounter();

		PC->set_MachineName(systemstring1);		
		PC->CategoryName=S"Processor";
		PC->CounterName=S"% Interrupt Time";		
		PC->InstanceName=S"_Total";

		PC->NextValue();	

		System::Threading::Thread::Sleep(1000);

		TotalInterruptTime = PC->NextValue();
	}

	catch(Exception *e)
	{
		noException = 0;			
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);			
		
	}

	if(noException == 1)
		this->SetItem(SetDCName,TotalInterruptTime);

	CoUninitialize();
}

void CDCMonitoringToolDlg::GetPageFaultsPerSec(LPSTR SetDCName)
{	
	CoInitialize(NULL);
	double TotalPageFaultsPerSec = 0;
		
	try
	{
		System::String * systemstring1 = new String(SetDCName);
		PerformanceCounter* PC = new PerformanceCounter();

		PC->set_MachineName(systemstring1);		
		PC->CategoryName=S"Memory";
		PC->CounterName=S"Page Faults/sec";		
		PC->InstanceName=S"";

		PC->NextValue();	

		System::Threading::Thread::Sleep(1000);

		TotalPageFaultsPerSec = PC->NextValue();
	}

	catch(Exception *e)
	{
		noException = 0;			
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);		
		
	}

	if(noException == 1)
		this->SetItem(SetDCName,TotalPageFaultsPerSec);

	CoUninitialize();
}

void CDCMonitoringToolDlg::GetPagesInputPerSec(LPSTR SetDCName)
{	
	CoInitialize(NULL);
	double TotalPagesInputPerSec = 0;
		
	try
	{
		System::String * systemstring1 = new String(SetDCName);
		PerformanceCounter* PC = new PerformanceCounter();

		PC->set_MachineName(systemstring1);		
		PC->CategoryName=S"Memory";
		PC->CounterName=S"Pages Input/sec";		
		PC->InstanceName=S"";

		PC->NextValue();	

		System::Threading::Thread::Sleep(1000);

		TotalPagesInputPerSec = PC->NextValue();
	}

	catch(Exception *e)
	{
		noException = 0;			
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);
		
	}

	if(noException == 1)
		this->SetItem(SetDCName,TotalPagesInputPerSec);

	CoUninitialize();
}

void CDCMonitoringToolDlg::GetPagesOutputPerSec(LPSTR SetDCName)
{	
	CoInitialize(NULL);
	double TotalPagesOutputPerSec = 0;
		
	try
	{
		System::String * systemstring1 = new String(SetDCName);
		PerformanceCounter* PC = new PerformanceCounter();

		PC->set_MachineName(systemstring1);		
		PC->CategoryName=S"Memory";
		PC->CounterName=S"Pages Output/sec";		
		PC->InstanceName=S"";

		PC->NextValue();	

		System::Threading::Thread::Sleep(1000);

		TotalPagesOutputPerSec = PC->NextValue();
	}

	catch(Exception *e)
	{
		noException = 0;			
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);	
		
	}

	if(noException == 1)
		this->SetItem(SetDCName,TotalPagesOutputPerSec);

	CoUninitialize();
}

void CDCMonitoringToolDlg::OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult)
{	
	cursel = TabCtrl_GetCurSel(C_ParameterTab);	

	if(cursel == 0)	
		this->InsertColumn("CPU Utilization");
	else if(cursel == 1)
		this->InsertColumn("Memory Utilization");			
	else if(cursel == 2)			
		this->InsertColumn("Disk Utilization");	
	else if(cursel == 3)			
		this->InsertColumn("Page Reads/sec");	
	else if(cursel == 4)			
		this->InsertColumn("Page Writes/sec");	
	else if(cursel == 5)			
		this->InsertColumn("Handle Count(LSASS)");	
	else if(cursel == 6)			
		this->InsertColumn("Handle Count(NTFRS)");
	else if(cursel == 7)			
		this->InsertColumn("File Reads/sec");
	else if(cursel == 8)			
		this->InsertColumn("File Writes/sec");
	else if(cursel == 9)			
		this->InsertColumn("Interrupt Time");
	else if(cursel == 10)			
		this->InsertColumn("Page Faults/sec");
	else if(cursel == 11)			
		this->InsertColumn("Pages Input/sec");
	else if(cursel == 12)			
		this->InsertColumn("Pages Output/sec");	

	this->C_ParametersListCtrl.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES);
	*pResult = 0;
}

void CDCMonitoringToolDlg::OnTimer(UINT nIDEvent)
{	
	if(noException==1){
	LPWSTR DCName[] = {L"sample"};
	LPSTR FDCName[] = {"sample"};
	int j = 1;			

	USES_CONVERSION;
	try
	{
		BSTR att = selDC.AllocSysString();
		BSTR ans = wcstok(att,L",");	
		while(j <= selCnt && ans != NULL)
		{			
			DCName[j] = ans;	
			CString samStr = W2A(DCName[j]);
			ans = wcstok(NULL,L",");			
			FDCName[j] = (LPSTR)(LPCTSTR)samStr;
			if(cursel == 0)			
				GetCPUUsage(FDCName[j]);
			else if(cursel == 1)
				GetMemoryUtilization(FDCName[j]);
			else if(cursel == 2)
				GetDiskUtilization(FDCName[j]);
			else if(cursel == 3)
				GetPageReadsPerSec(FDCName[j]);
			else if(cursel == 4)
				GetPageWritesPerSec(FDCName[j]);
			else if(cursel == 5)
				GetHandleCountLSASS(FDCName[j]);
			else if(cursel == 6)
				GetHandleCountNTFRS(FDCName[j]);
			else if(cursel == 7)
				GetFileReadsPerSec(FDCName[j]);
			else if(cursel == 8)
				GetFileWritesPerSec(FDCName[j]);
			else if(cursel == 9)
				GetInterruptTime(FDCName[j]);
			else if(cursel == 10)
				GetPageFaultsPerSec(FDCName[j]);
			else if(cursel == 11)
				GetPagesInputPerSec(FDCName[j]);
			else if(cursel == 12)
				GetPagesOutputPerSec(FDCName[j]);			

			j = j +	1;				
		}
	  //SysFreeString(att);
		SysFreeString(ans);
	}
	catch(Exception * e)
	{
		char msg[300];
		sprintf(msg,"%s",e->get_Message());
		MessageBox(msg,"Exception",MB_OK | MB_ICONINFORMATION);	
		
	
	}	
	}
}

void CDCMonitoringToolDlg::InsertColumn(LPSTR ParameterName)
{
	CHeaderCtrl* pHeaderCtrl = C_ParametersListCtrl.GetHeaderCtrl();
		
	if (pHeaderCtrl != NULL)
	{
		int nColumnCount = pHeaderCtrl->GetItemCount();
		for(int i = nColumnCount ; i >= 0 ; i --)				
			this->C_ParametersListCtrl.DeleteColumn(i);
	}	

	LVCOLUMN lvColumn;

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_CENTER;
	lvColumn.cx = 230;									
	lvColumn.pszText = "Domain Controllers";
	this->C_ParametersListCtrl.InsertColumn(0, &lvColumn);		

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_CENTER;
	lvColumn.cx = 230;									
	lvColumn.pszText = ParameterName;
	this->C_ParametersListCtrl.InsertColumn(1, &lvColumn);		
}

void CDCMonitoringToolDlg::SetItem(LPSTR SetDCName,double ParameterValue)
{
	char *value,result[20];
	value = result;	
	sprintf(value,"%.0f",ParameterValue);		
	CString value1 = value;
	if(cursel == 0 || cursel == 9)
		value1.Append("%");		
	else if(cursel == 1 || cursel == 2)
		value1.Append("K");		

	if(this->C_ParametersListCtrl.GetItemCount() == selCnt)
		this->C_ParametersListCtrl.DeleteAllItems();

	int nItem;

	LVITEM lvItem;																			
	lvItem.mask = LVIF_TEXT;
	lvItem.iItem = 0;
	lvItem.iSubItem = 0;			
	lvItem.pszText = SetDCName;	
	
	nItem = this->C_ParametersListCtrl.InsertItem(&lvItem);	

	this->C_ParametersListCtrl.SetItemText(nItem,1,value1);	

	this->C_ParametersListCtrl.Invalidate(FALSE);
}

BOOL CDCMonitoringToolDlg::CheckEmpty(CString Credentials)
{
	if(Credentials.IsEmpty())
	{
		MessageBox("Enter All Credentials","DC Monitoring",MB_OK | MB_ICONINFORMATION);

		if(Credentials == this->V_DomainNameEdit)
			this->C_DomainNameEdit.SetFocus();
		else if(Credentials == this->V_UserNameEdit)
			this->C_UserNameEdit.SetFocus();
		else if(Credentials == this->V_PasswordEdit)
			this->C_PasswordEdit.SetFocus();

		return FALSE;
	}
	else
		return TRUE;
}
void CDCMonitoringToolDlg::OnBnClickedButton2()
{
	// TODO: Add your control notification handler code here
	RevertToSelf();
	OnCancel();
}



BOOL CDCMonitoringToolDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default
	     CRect r;
         pDC->GetClipBox(&r);
		 pDC->FillSolidRect(r, RGB(244,244,244)); 
         return TRUE;
}


HBRUSH CDCMonitoringToolDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	   if( IDC_STATIC90 == pWnd->GetDlgCtrlID())
	   {	   
       CPoint ul(0,0);
       CRect rect;
       pWnd->GetWindowRect( &rect );
       CPoint lr( (rect.right-rect.left-2), (rect.bottom-rect.top-2) ); 
       pDC->FillSolidRect( CRect(ul, lr), RGB(255,255,255) );
       pWnd->SetWindowPos( &wndTop, 0, 0, 0, 0, SWP_NOMOVE|SWP_NOSIZE );		   
	   } 
	   switch (nCtlColor)
       {
       case CTLCOLOR_STATIC:
	   if(IDC_STATIC1 == pWnd->GetDlgCtrlID() || IDC_STATIC4 == pWnd->GetDlgCtrlID() || IDC_STATIC5 == pWnd->GetDlgCtrlID())
	   pDC->SetBkColor(RGB(255, 255 , 255));
	   else if(ID_HYPERLINK7 == pWnd->GetDlgCtrlID())
	   {
		pDC->SetTextColor(RGB(0, 0 , 255));
	    pDC->SetBkColor(RGB(244, 244 , 244));
	   }
	   else
	   pDC->SetBkColor(RGB(244, 244 , 244));
       return (HBRUSH)GetStockObject(NULL_BRUSH);
       default:
	   return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
	   }
}
