// TestHtmlDlgDlg.cpp : implementation file
//

#include "stdafx.h"
#include "TestHtmlDlg.h"
#include "TestHtmlDlgDlg.h"
#include "process.h"
#include "sys/types.h"
#include "windows.h"
#include "tchar.h"
#include <io.h>   // For access().
#include<AtlConv.h>
#include <atlbase.h>

#include <sys/types.h>  // For stat().
#include <sys/stat.h>   // For stat().
#include <iostream>

#include "string.h"



#ifdef _DEBUG
#define new DEBUG_NEW
#endif


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


// CTestHtmlDlgDlg dialog

BEGIN_DHTML_EVENT_MAP(CTestHtmlDlgDlg)
	DHTML_EVENT_ONCLICK(_T("ButtonOK"), OnButtonOK)
	DHTML_EVENT_ONCLICK(_T("ButtonCancel"), OnButtonCancel)
	DHTML_EVENT_ONCLICK(_T("Empty"), OnEmpty)
	DHTML_EVENT_ONCLICK(_T("CSV"), OnCSV)
	DHTML_EVENT_ONCLICK(_T("ADQuery"), OnADQuery)
	DHTML_EVENT_ONCLICK(_T("DC"), OnDC)
	DHTML_EVENT_ONCLICK(_T("LastLogonFinder"), OnLastLogonFinder)
	DHTML_EVENT_ONCLICK(_T("PasswordPolicyManager"), OnPasswordPolicyManager)
	DHTML_EVENT_ONCLICK(_T("DMZAnalyser"), OnDMZAnalyser)
	DHTML_EVENT_ONCLICK(_T("DNSReporter"), OnDNSReporter)
	DHTML_EVENT_ONCLICK(_T("Local"), OnLocalUserManagement)

	DHTML_EVENT_ONCLICK(_T("SharePoint"), OnPowerShell)
//	DHTML_EVENT_ONCLICK(_T("Local"), OnPowerShell)
	DHTML_EVENT_ONCLICK(_T("Terminal"), OnPowerShell)
	DHTML_EVENT_ONCLICK(_T("GetDomain"), OnPowerShell)
	DHTML_EVENT_ONCLICK(_T("GetDuplicates"), OnPowerShell)
	DHTML_EVENT_ONCLICK(_T("ActiveDirectoryReplicationManager"), OnPowerShell)
	DHTML_EVENT_ONCLICK(_T("HELP1"), OnHelp)
	DHTML_EVENT_ONCLICK(_T("A1"), OnA1)
	DHTML_EVENT_ONCLICK(_T("A2"), OnA2)
	DHTML_EVENT_ONCLICK(_T("A3"), OnA3)
	DHTML_EVENT_ONCLICK(_T("A4"), OnPowerShell2)
		
END_DHTML_EVENT_MAP()



CTestHtmlDlgDlg::CTestHtmlDlgDlg(CWnd* pParent /*=NULL*/)
	: CDHtmlDialog(CTestHtmlDlgDlg::IDD, CTestHtmlDlgDlg::IDH, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDI_ICON1);
}

void CTestHtmlDlgDlg::DoDataExchange(CDataExchange* pDX)
{
	CDHtmlDialog::DoDataExchange(pDX);
	DDX_Control(pDX,IDC_LOGO,m_TopBand);
	DDX_Control(pDX,IDC_ADMP2,m_Admp);
	DDX_Control(pDX,IDC_ADAUDIT,m_AdAudit);
	DDX_Control(pDX,IDC_ADSP,m_AdSelf);
	DDX_Control(pDX,IDC_ALSO,m_Also);
}

BEGIN_MESSAGE_MAP(CTestHtmlDlgDlg, CDHtmlDialog)
	ON_WM_SYSCOMMAND()
	//}}AFX_MSG_MAP
	ON_STN_CLICKED(IDC_ADMP2, &CTestHtmlDlgDlg::OnStnClickedAdmp2)
END_MESSAGE_MAP()


// CTestHtmlDlgDlg message handlers

BOOL CTestHtmlDlgDlg::OnInitDialog()
{
	CDHtmlDialog::OnInitDialog();

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
	CString imagepath=GetAppPath();
	CString temp;
	temp=imagepath;
	//MessageBox(L"ImagePath: "+temp);
	temp.Append(_T("./..//images//also_try.jpg"));	
	m_Also.Init(temp);
	temp="";
	temp=imagepath;
	temp.Append(_T("./..//images//top_band2.gif"));
	m_TopBand.Init(temp);			
	
	temp="";
	temp=imagepath;
	temp.Append(_T("./..//images//admp_add.jpg"));
	m_Admp.Init(temp);
	temp="";
	temp=imagepath;
	temp.Append(_T("./..//images//adap_add.jpg"));
	m_AdAudit.Init(temp);
	temp="";
	temp=imagepath;
	temp.Append(_T("./..//images//adssp_add.jpg"));
	m_AdSelf.Init(temp);
	
	//m_TopLink.setURL(L"http://www.google.com");
	m_TopLink.ConvertStaticToHyperlink(GetSafeHwnd(),IDC_LOGO,L"http://www.techcrafters.com/");

	m_Admp_link.ConvertStaticToHyperlink(GetSafeHwnd(),IDC_ADMP2,L"http://www.manageengine.com/products/ad-manager/index.html");
	m_AdAudit_link.ConvertStaticToHyperlink(GetSafeHwnd(),IDC_ADAUDIT,L"http://www.manageengine.com/products/active-directory-audit/index.html");
	m_AdSelf_link.ConvertStaticToHyperlink(GetSafeHwnd(),IDC_ADSP,L"http://www.manageengine.com/products/self-service-password/index.html");

	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CTestHtmlDlgDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDHtmlDialog::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CTestHtmlDlgDlg::OnPaint()
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
		CDHtmlDialog::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CTestHtmlDlgDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

HRESULT CTestHtmlDlgDlg::OnButtonOK(IHTMLElement* /*pElement*/)
{
	OnOK();
	return S_OK;
}

HRESULT CTestHtmlDlgDlg::OnButtonCancel(IHTMLElement* /*pElement*/)
{
	OnCancel();	
	return S_OK;
}

HRESULT CTestHtmlDlgDlg::OnEmpty(IHTMLElement *pElement)
{
	CoInitializeEx(NULL, COINIT_APARTMENTTHREADED | COINIT_DISABLE_OLE1DDE);
	STARTUPINFO si = { sizeof(si) };
   SECURITY_ATTRIBUTES saProcess,saThread;
   PROCESS_INFORMATION piProcessB;

   TCHAR szPath[MAX_PATH];
   
   saProcess.nLength = sizeof(saProcess);
   saProcess.lpSecurityDescriptor = NULL;
   saProcess.bInheritHandle = TRUE;

	saThread.nLength = sizeof(saThread);
   saThread.lpSecurityDescriptor = NULL;
   saThread.bInheritHandle = FALSE;


	lstrcpy(szPath, TEXT("EmptyPasswordChecker.exe"));
	CTestHtmlDlgDlg::CloseWindow();
   CreateProcess(NULL, szPath, &saProcess, &saThread,
      FALSE, 0, NULL, NULL, &si, &piProcessB);
	WaitForSingleObject(piProcessB.hProcess,INFINITE);
	CloseHandle(piProcessB.hProcess);
	CloseHandle(piProcessB.hThread);
	CTestHtmlDlgDlg::ShowWindow(1);
	return S_OK;
}
HRESULT CTestHtmlDlgDlg::OnPasswordPolicyManager(IHTMLElement *pElement)
{
	CoInitializeEx(NULL, COINIT_APARTMENTTHREADED | COINIT_DISABLE_OLE1DDE);
	STARTUPINFO si = { sizeof(si) };
   SECURITY_ATTRIBUTES saProcess,saThread;
   PROCESS_INFORMATION piProcessB;

   TCHAR szPath[MAX_PATH];
   
   saProcess.nLength = sizeof(saProcess);
   saProcess.lpSecurityDescriptor = NULL;
   saProcess.bInheritHandle = TRUE;

	saThread.nLength = sizeof(saThread);
   saThread.lpSecurityDescriptor = NULL;
   saThread.bInheritHandle = FALSE;


	lstrcpy(szPath, TEXT("Password_Policy_Manager.exe"));
	CTestHtmlDlgDlg::CloseWindow();
   CreateProcess(NULL, szPath, &saProcess, &saThread,
      FALSE, 0, NULL, NULL, &si, &piProcessB);
	WaitForSingleObject(piProcessB.hProcess,INFINITE);
	CloseHandle(piProcessB.hProcess);
	CloseHandle(piProcessB.hThread);
	CTestHtmlDlgDlg::ShowWindow(1);
	return S_OK;
}
HRESULT CTestHtmlDlgDlg::OnDMZAnalyser(IHTMLElement *pElement)
{
	CoInitializeEx(NULL, COINIT_APARTMENTTHREADED | COINIT_DISABLE_OLE1DDE);
	STARTUPINFO si = { sizeof(si) };
   SECURITY_ATTRIBUTES saProcess,saThread;
   PROCESS_INFORMATION piProcessB;

   TCHAR szPath[MAX_PATH];
   
   saProcess.nLength = sizeof(saProcess);
   saProcess.lpSecurityDescriptor = NULL;
   saProcess.bInheritHandle = TRUE;

	saThread.nLength = sizeof(saThread);
   saThread.lpSecurityDescriptor = NULL;
   saThread.bInheritHandle = FALSE;


	lstrcpy(szPath, TEXT("DMZAnalyzer.exe"));
	CTestHtmlDlgDlg::CloseWindow();
   CreateProcess(NULL, szPath, &saProcess, &saThread,
      FALSE, 0, NULL, NULL, &si, &piProcessB);
	WaitForSingleObject(piProcessB.hProcess,INFINITE);
	CloseHandle(piProcessB.hProcess);
	CloseHandle(piProcessB.hThread);
	CTestHtmlDlgDlg::ShowWindow(1);
	return S_OK;
}

HRESULT CTestHtmlDlgDlg::OnDNSReporter(IHTMLElement *pElement)
{
	CoInitializeEx(NULL, COINIT_APARTMENTTHREADED | COINIT_DISABLE_OLE1DDE);
	STARTUPINFO si = { sizeof(si) };
   SECURITY_ATTRIBUTES saProcess,saThread;
   PROCESS_INFORMATION piProcessB;

   TCHAR szPath[MAX_PATH];
   
   saProcess.nLength = sizeof(saProcess);
   saProcess.lpSecurityDescriptor = NULL;
   saProcess.bInheritHandle = TRUE;

	saThread.nLength = sizeof(saThread);
   saThread.lpSecurityDescriptor = NULL;
   saThread.bInheritHandle = FALSE;


	lstrcpy(szPath, TEXT("1DnsReporter.exe"));
	CTestHtmlDlgDlg::CloseWindow();
   CreateProcess(NULL, szPath, &saProcess, &saThread,
      FALSE, 0, NULL, NULL, &si, &piProcessB);
	WaitForSingleObject(piProcessB.hProcess,INFINITE);
	CloseHandle(piProcessB.hProcess);
	CloseHandle(piProcessB.hThread);
	CTestHtmlDlgDlg::ShowWindow(1);
	return S_OK;
}

HRESULT CTestHtmlDlgDlg::OnLocalUserManagement(IHTMLElement *pElement)
{
	CoInitializeEx(NULL, COINIT_APARTMENTTHREADED | COINIT_DISABLE_OLE1DDE);
	STARTUPINFO si = { sizeof(si) };
   SECURITY_ATTRIBUTES saProcess,saThread;
   PROCESS_INFORMATION piProcessB;

   TCHAR szPath[MAX_PATH];
   
   saProcess.nLength = sizeof(saProcess);
   saProcess.lpSecurityDescriptor = NULL;
   saProcess.bInheritHandle = TRUE;

	saThread.nLength = sizeof(saThread);
   saThread.lpSecurityDescriptor = NULL;
   saThread.bInheritHandle = FALSE;


	lstrcpy(szPath, TEXT("LocalUserManagement.exe"));
	CTestHtmlDlgDlg::CloseWindow();
   CreateProcess(NULL, szPath, &saProcess, &saThread,
      FALSE, 0, NULL, NULL, &si, &piProcessB);
	WaitForSingleObject(piProcessB.hProcess,INFINITE);
	CloseHandle(piProcessB.hProcess);
	CloseHandle(piProcessB.hThread);
	CTestHtmlDlgDlg::ShowWindow(1);
	return S_OK;
}

HRESULT CTestHtmlDlgDlg::OnCSV(IHTMLElement *pElement)
{
	
	CoInitializeEx(NULL, COINIT_APARTMENTTHREADED | COINIT_DISABLE_OLE1DDE);
	STARTUPINFO si = { sizeof(si) };
   SECURITY_ATTRIBUTES saProcess,saThread;
   PROCESS_INFORMATION piProcessB;

   TCHAR szPath[MAX_PATH];
   
   saProcess.nLength = sizeof(saProcess);
   saProcess.lpSecurityDescriptor = NULL;
   saProcess.bInheritHandle = TRUE;

	saThread.nLength = sizeof(saThread);
   saThread.lpSecurityDescriptor = NULL;
   saThread.bInheritHandle = FALSE;


	lstrcpy(szPath, TEXT("CSVGenerator.exe"));
	CTestHtmlDlgDlg::CloseWindow();
   CreateProcess(NULL, szPath, &saProcess, &saThread,
      FALSE, 0, NULL, NULL, &si, &piProcessB);
	WaitForSingleObject(piProcessB.hProcess,INFINITE);
	CloseHandle(piProcessB.hProcess);
	CloseHandle(piProcessB.hThread);
	CTestHtmlDlgDlg::ShowWindow(1);
	return S_OK;
}
HRESULT CTestHtmlDlgDlg::OnLastLogonFinder(IHTMLElement *pElement)
{
	
	CoInitializeEx(NULL, COINIT_APARTMENTTHREADED | COINIT_DISABLE_OLE1DDE);
	STARTUPINFO si = { sizeof(si) };
   SECURITY_ATTRIBUTES saProcess,saThread;
   PROCESS_INFORMATION piProcessB;

   TCHAR szPath[MAX_PATH];
   
   saProcess.nLength = sizeof(saProcess);
   saProcess.lpSecurityDescriptor = NULL;
   saProcess.bInheritHandle = TRUE;

	saThread.nLength = sizeof(saThread);
   saThread.lpSecurityDescriptor = NULL;
   saThread.bInheritHandle = FALSE;


	lstrcpy(szPath, TEXT("LastLogonToolMFC.exe"));
	CTestHtmlDlgDlg::CloseWindow();
   CreateProcess(NULL, szPath, &saProcess, &saThread,
      FALSE, 0, NULL, NULL, &si, &piProcessB);
	WaitForSingleObject(piProcessB.hProcess,INFINITE);
	CloseHandle(piProcessB.hProcess);
	CloseHandle(piProcessB.hThread);
	CTestHtmlDlgDlg::ShowWindow(1);
	return S_OK;
}
HRESULT CTestHtmlDlgDlg::OnADQuery(IHTMLElement *pElement)
{

	CoInitializeEx(NULL, COINIT_APARTMENTTHREADED | COINIT_DISABLE_OLE1DDE);
	STARTUPINFO si = { sizeof(si) };
   SECURITY_ATTRIBUTES saProcess,saThread;
   PROCESS_INFORMATION piProcessB;

   TCHAR szPath[MAX_PATH];
   
   saProcess.nLength = sizeof(saProcess);
   saProcess.lpSecurityDescriptor = NULL;
   saProcess.bInheritHandle = TRUE;

	saThread.nLength = sizeof(saThread);
   saThread.lpSecurityDescriptor = NULL;
   saThread.bInheritHandle = FALSE;


	lstrcpy(szPath, TEXT("ADQuery.exe"));
	CTestHtmlDlgDlg::CloseWindow();
   CreateProcess(NULL, szPath, &saProcess, &saThread,
      FALSE, 0, NULL, NULL, &si, &piProcessB);
	WaitForSingleObject(piProcessB.hProcess,INFINITE);
	CloseHandle(piProcessB.hProcess);
	CloseHandle(piProcessB.hThread);
	CTestHtmlDlgDlg::ShowWindow(1);
	return S_OK;
}

BOOL FolderExists(CString strFolderName)
{   
    return GetFileAttributes(strFolderName) != INVALID_FILE_ATTRIBUTES;   
}
HRESULT CTestHtmlDlgDlg::OnPowerShell2(IHTMLElement *pElement)
{
	CString msg=L"\n Please install .Net FrameWork v4.0 or above and PowerShell ";
	MessageBox(msg,L"Free AD Tools Information ",MB_OK | MB_ICONINFORMATION);	
	return S_OK;
}

HRESULT CTestHtmlDlgDlg::OnDC(IHTMLElement *pElement)
{

	char *buffer = getenv("windir");
	size_t len;
	LPSTR str2;
	LPWSTR RealPath;
	
	std::string str1= buffer;
	str1.append("\\Microsoft.NET\\Framework\\v2.0.50727");
	len=str1.length();
	 str2=new char[len+1];
	str1._Copy_s(str2,len,len);
	str2[len]='\0';

	//converting to LPWSTR	
	  USES_CONVERSION;
	  RealPath =A2W(str2) ;
	//MessageBox(RealPath);
	if(GetFileAttributes(RealPath) != INVALID_FILE_ATTRIBUTES && GetFileAttributes(RealPath)== FILE_ATTRIBUTE_DIRECTORY )
	{
		//MessageBox(L"Framework installed");
		CoInitializeEx(NULL, COINIT_APARTMENTTHREADED | COINIT_DISABLE_OLE1DDE);
	STARTUPINFO si = { sizeof(si) };
   SECURITY_ATTRIBUTES saProcess,saThread;
   PROCESS_INFORMATION piProcessB;

   TCHAR szPath[MAX_PATH];
   
   saProcess.nLength = sizeof(saProcess);
   saProcess.lpSecurityDescriptor = NULL;
   saProcess.bInheritHandle = TRUE;

	saThread.nLength = sizeof(saThread);
   saThread.lpSecurityDescriptor = NULL;
   saThread.bInheritHandle = FALSE;


	lstrcpy(szPath, TEXT("DCMonitoringTool.exe"));
	CTestHtmlDlgDlg::CloseWindow();
   CreateProcess(NULL, szPath, &saProcess, &saThread,
      FALSE, 0, NULL, NULL, &si, &piProcessB);
	WaitForSingleObject(piProcessB.hProcess,INFINITE);
	CloseHandle(piProcessB.hProcess);
	CloseHandle(piProcessB.hThread);
	CTestHtmlDlgDlg::ShowWindow(1);
	CoUninitialize();

	return S_OK;
	}
	else
	{
		std::string str1= buffer;
	str1.append("\\Microsoft.NET\\Framework64\\v2.0.50727");
	len=str1.length();
	 str2=new char[len+1];
	str1._Copy_s(str2,len,len);
	str2[len]='\0';

	//converting to LPWSTR	
	  USES_CONVERSION;
	  RealPath =A2W(str2) ;
		if(GetFileAttributes(RealPath) != INVALID_FILE_ATTRIBUTES && GetFileAttributes(RealPath)== FILE_ATTRIBUTE_DIRECTORY )
		{
		//MessageBox(L"Framework installed");
		
		CoInitializeEx(NULL, COINIT_APARTMENTTHREADED | COINIT_DISABLE_OLE1DDE);
	STARTUPINFO si = { sizeof(si) };
   SECURITY_ATTRIBUTES saProcess,saThread;
   PROCESS_INFORMATION piProcessB;

   TCHAR szPath[MAX_PATH];
   
   saProcess.nLength = sizeof(saProcess);
   saProcess.lpSecurityDescriptor = NULL;
   saProcess.bInheritHandle = TRUE;

	saThread.nLength = sizeof(saThread);
   saThread.lpSecurityDescriptor = NULL;
   saThread.bInheritHandle = FALSE;


	lstrcpy(szPath, TEXT("DCMonitoringTool.exe"));
	CTestHtmlDlgDlg::CloseWindow();
   CreateProcess(NULL, szPath, &saProcess, &saThread,
      FALSE, 0, NULL, NULL, &si, &piProcessB);
	WaitForSingleObject(piProcessB.hProcess,INFINITE);
	CloseHandle(piProcessB.hProcess);
	CloseHandle(piProcessB.hThread);
	CTestHtmlDlgDlg::ShowWindow(1);
	CoUninitialize();

		return S_OK;
		}
		MessageBox(L"Please install Framework v2.0 or above");
	}

	/*MessageBox(L"OnDC entered");
	HINSTANCE hInst;
	hInst=ShellExecute(GetSafeHwnd(), L"open", L"DCMonitoringTool.exe", NULL, NULL, SW_SHOWNORMAL);
	if(reinterpret_cast<int>(hInst) == SE_ERR_ACCESSDENIED)
	{
		MessageBox(L"Before using this tool ensure \"Microsoft Visual C++ 2005 Redistributable\" package exists in your system.It can be downloaded from the link \n http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=32bc1bee-a3f9-4c13-9c99-220b62a191ee", L"AD Tools Information ",MB_OK | MB_ICONINFORMATION);
		MessageBox(L"Please install .NET Framework any version ",L"AD Free Tool Error Information",MB_OK | MB_ICONINFORMATION);

	}*/
	return S_OK;
}
HRESULT CTestHtmlDlgDlg::OnPowerShell(IHTMLElement *pElement)
{
	CString msg=L"\n Please install .Net FrameWork v2.0 or above and PowerShell ";
	MessageBox(msg,L"Free AD Tools Information ",MB_OK | MB_ICONINFORMATION);	
	return S_OK;
}
CString CTestHtmlDlgDlg::GetAppPath()
{
	CString strPath;
	TCHAR* pstrExePath = strPath.GetBuffer (MAX_PATH);

	//::GetModuleFileName (0, pstrExePath, MAX_PATH);
	//strPath.ReleaseBuffer ();
	//MessageBox(strPath);
	//int i=strPath.Find(L"bin");		// find the application installation bin folder


	//CString tmpstr=L"";
	//tmpstr=strPath.Left(i);			// extract path before to bin folder.

	//MessageBox(tmpstr);
	GetCurrentDirectory(MAX_PATH,pstrExePath);
	strPath.ReleaseBuffer();
	//MessageBox(L""+strPath);
	return strPath;

}
CString CTestHtmlDlgDlg::GetAppPath1()
{
	CString strPath;
	TCHAR* pstrExePath = strPath.GetBuffer (MAX_PATH);

	::GetModuleFileName (0, pstrExePath, MAX_PATH);
	strPath.ReleaseBuffer ();
	//MessageBox(strPath);
	int i=strPath.Find(L"bin");		// find the application installation bin folder


	CString tmpstr=L"";
	tmpstr=strPath.Left(i);			// extract path before to bin folder.

	//MessageBox(tmpstr);
	return tmpstr;

}
HRESULT CTestHtmlDlgDlg::OnHelp(IHTMLElement *pElement)
{
	CString tmpstr=GetAppPath1();
	tmpstr.Append(L"help\\techcraftersindex.html");

	LPWSTR pStr =  (LPWSTR)(LPCWSTR)tmpstr; 
	//MessageBox(pStr,L"help path:",0);
	ShellExecute(NULL,L"Open",pStr,NULL,NULL,SW_SHOWNORMAL);
	//ShellExecute(NULL, L"open", L"", NULL, NULL, SW_SHOWNORMAL);
	return S_OK;
}
void CTestHtmlDlgDlg::OnStnClickedAdmp2()
{
	// TODO: Add your control notification handler code here
}
HRESULT CTestHtmlDlgDlg::OnA1(IHTMLElement *pElement)
{
ShellExecute(NULL,L"Open",L"http://www.techcrafters.com/",NULL,NULL,SW_SHOWNORMAL);
return S_OK;
}
HRESULT CTestHtmlDlgDlg::OnA2(IHTMLElement *pElement)
{
ShellExecute(NULL,L"Open",L"http://www.techcrafters.com/blogs-2",NULL,NULL,SW_SHOWNORMAL);
return S_OK;
}
HRESULT CTestHtmlDlgDlg::OnA3(IHTMLElement *pElement)
{
ShellExecute(NULL,L"Open",L"http://www.techcrafters.com/ask-questions",NULL,NULL,SW_SHOWNORMAL);
return S_OK;
}