
// 1DnsReporterDlg.cpp : implementation file
//

#include "stdafx.h"
#include "1DnsReporter.h"
#include "1DnsReporterDlg.h"
#include "afxdialogex.h"
#include "Dsgetdc.h"   // DsGetDcName()
#include "Shellapi.h"  // ShellExecuteEx()
#include "winbase.h" // CreateProcess()

    #include <winsock2.h>
    #include <iphlpapi.h>
    #include <stdio.h>
    #include <windows.h>
    #include <fstream>
    #include <string>
    #include <stdlib.h>
	#include <vector>
    #include <dos.h>
    #include "afxbutton.h"
    #pragma comment(lib,"ws2_32.lib")
    #pragma comment(lib, "IPHLPAPI.lib")
	#pragma comment(lib, "netapi32.lib")
	//#pragma comment(lib, "coredll.lib")

    using namespace std;


    #define MALLOC(x) HeapAlloc(GetProcessHeap(), 0, (x))
    #define FREE(x) HeapFree(GetProcessHeap(), 0, (x))
    #define T_A 1

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#ifdef _AFXDLL
#undef _AFXDLL
#endif

vector<wstring> a1,a2,ns1,ns2,srv1,srv2;
DWORD count;

/*
struct DNS_HEADER
    {
        unsigned	short id;		    // identification number
        unsigned	char rd     :1;		// recursion desired
        unsigned	char tc     :1;		// truncated message
        unsigned	char aa     :1;		// authoritative answer
        unsigned	char opcode :4;	    // purpose of message
        unsigned	char qr     :1;		// query/response flag
        unsigned	char rcode  :4;	    // response code
        unsigned	char cd     :1;	    // checking disabled
        unsigned	char ad     :1;	    // authenticated data
        unsigned	char z      :1;		// its z! reserved
        unsigned	char ra     :1;		// recursion available
        unsigned    short q_count;	    // number of question entries
        unsigned	short ans_count;	// number of answer entries
        unsigned	short auth_count;	// number of authority entries
        unsigned	short add_count;	// number of resource entries
    };
	
	

    //Constant sized fields of query structure
    struct QUESTION
    {
        unsigned short qtype;
        unsigned short qclass;
    };

    //Constant sized fields of the resource record structure
    #pragma pack(push, 1)
    struct  R_DATA
    {
        unsigned short type;
        unsigned short _class;
        unsigned int   ttl;
        unsigned short data_len;
    };
    #pragma pack(pop)

    //Pointers to resource record contents
    struct RES_RECORD
    {
        string name;
        struct R_DATA* resource;
        string rdata;
        int pref;
    };

    //Structure of a Query
    typedef struct
    {
        string name;
        struct QUESTION* ques;
    } QUERY;
*/

// CAboutDlg dialog used for App About




class CAboutDlg : public CDialogEx
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

CAboutDlg::CAboutDlg() : CDialogEx(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CMy1DnsReporterDlg dialog




CMy1DnsReporterDlg::CMy1DnsReporterDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CMy1DnsReporterDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CMy1DnsReporterDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TAB1, m_paramaterTab);
	DDX_Control(pDX, IDC_LIST1, m_paramaterList);
	DDX_Control(pDX, IDC_COMBO1, m_domainNames);
	DDX_Control(pDX, IDC_BUTTON4, m_buttonhome);
	DDX_Control(pDX, IDC_BUTTON1, m_button1);
	DDX_Control(pDX, IDC_BUTTON2, m_button2);
}

BEGIN_MESSAGE_MAP(CMy1DnsReporterDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB1, &CMy1DnsReporterDlg::OnTcnSelchangeTab1)
	ON_BN_CLICKED(IDC_BUTTON1, &CMy1DnsReporterDlg::OnClickedButton1)
	ON_CBN_SELCHANGE(IDC_COMBO1, &CMy1DnsReporterDlg::OnSelchangeCombo1)
	ON_NOTIFY(NM_CLICK, IDC_SYSLINK2, &CMy1DnsReporterDlg::OnNMClickSyslink2)
	ON_BN_CLICKED(IDC_BUTTON4, &CMy1DnsReporterDlg::OnClickedButton4)
	ON_WM_CTLCOLOR()
	ON_WM_ERASEBKGND()
	ON_BN_CLICKED(IDC_BUTTON2, &CMy1DnsReporterDlg::OnBnClickedButton2)
END_MESSAGE_MAP()


// CMy1DnsReporterDlg message handlers

BOOL CMy1DnsReporterDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	SetWindowText(L"DNS Reporter");
	bitmap_home.LoadBitmapW(IDB_BITMAP2);
	HBITMAP hBitmap = (HBITMAP) bitmap_home.GetSafeHandle();
	m_buttonhome.SetBitmap(hBitmap);

	
	BITMAP bm;
	bitmap_home.GetBitmap(&bm);
	int width = bm.bmWidth;
	int height = bm.bmHeight;
	CRect rec;
	m_buttonhome.GetWindowRect(&rec);
	ScreenToClient(&rec);
	rec.right = rec.left + width;
	rec.bottom = rec.top + height;
	m_buttonhome.MoveWindow(&rec);
	

	m_button1.SetFaceColor(RGB(0,133,183),true);
	m_button1.SetTextColor(RGB(255,255,255));

    m_button2.SetFaceColor(RGB(0,133,183),true);
	m_button2.SetTextColor(RGB(255,255,255));

	CFont *m_pFont = new CFont();
    m_pFont->CreatePointFont(165, _T("Arial"));
    GetDlgItem(IDC_STATIC13)->SetFont(m_pFont, TRUE);

	DWORD dwRet = NULL;
	PDOMAIN_CONTROLLER_INFOW pdci;  
	dwRet = DsGetDcName(NULL, 
						NULL, 
						NULL, 
						NULL, 
						DS_TIMESERV_REQUIRED,					
						&pdci);
		if(NO_ERROR == dwRet)			
		//this->m_domainNames.SetWindowText(pdci->DomainName);
		this->m_domainNames.AddString(pdci->DomainName);

 char text1[] = "Name";
 char text2[] = "Record Type";
 char text3[] = "Data";
 wchar_t wtext1[20],wtext2[20],wtext3[20];
 //mbstowcs(wtext1, text1, strlen(text1)+1);
 //LPWSTR ptr1 = wtext1;

 
	TCITEM tcItem;

	tcItem.mask = TCIF_TEXT;
	tcItem.pszText = _T("DNS Records");
	this->m_paramaterTab.InsertItem(0, &tcItem);

	tcItem.mask = TCIF_TEXT;
	tcItem.pszText = _T("Authoritative Servers");
	this->m_paramaterTab.InsertItem(1, &tcItem);

	tcItem.mask = TCIF_TEXT;
	tcItem.pszText = _T("Zones");
	this->m_paramaterTab.InsertItem(2, &tcItem);


	LVCOLUMN lvColumn;

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 240;
	mbstowcs(wtext1, text1, strlen(text1)+1);
	lvColumn.pszText=wtext1;
	this->m_paramaterList.InsertColumn(0, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 140;
	mbstowcs(wtext2, text2, strlen(text2)+1);
	lvColumn.pszText=wtext2;
	this->m_paramaterList.InsertColumn(1, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 242;
	mbstowcs(wtext3, text3, strlen(text3)+1);
	lvColumn.pszText=wtext3;
	this->m_paramaterList.InsertColumn(2, &lvColumn);

	this->m_paramaterList.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES);


	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
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

/*	// TODO: Add extra initialization here
	btnHome.Create ("", WS_VISIBLE | BS_OWNERDRAW| WS_EX_CLIENTEDGE,CRect(20,20,50,70) , this, 160 );
	btnHome.LoadBitmaps(160,0,0,0);
	CButton *btnOK;
	// Get a handle to each of the existing buttons

	btnOK = reinterpret_cast<CButton *>(GetDlgItem(IDC_BUTTON3));
	// Get the style of the button(s)

	LONG GWLOK = GetWindowLong(btnOK->m_hWnd, GWL_STYLE);
	// Change the button's style to BS_OWNERDRAW

	SetWindowLong(btnOK->m_hWnd, GWL_STYLE, GWLOK | BS_OWNERDRAW);
	// Subclass each button
	btnHome.SubclassDlgItem(IDC_BUTTON3, this); */

	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CMy1DnsReporterDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CMy1DnsReporterDlg::OnPaint()
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
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CMy1DnsReporterDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CMy1DnsReporterDlg::OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult)
{
	// TODO: Add your control notification handler code here

/*
		int nCount = m_paramaterList.GetItemCount();
		for (int i=0; i < nCount; i++)
		{
			m_paramaterList.DeleteItem(i);
		}
*/
	cursel=m_paramaterTab.GetCurSel();

	if(cursel == 0)	
	{

		int nCount = m_paramaterList.GetItemCount();
		for (int i=0; i < nCount; i++)
		{
			m_paramaterList.DeleteItem(0);
		}

		for(int i=0;i<a1.size();i++)
		{
	
			LVITEM lvItem;
			int nItem;
			lvItem.mask = LVIF_TEXT;
			lvItem.iItem = i+1;
			lvItem.iSubItem = 0;
			//LPWSTR appParams = new wchar_t[a1[i].size()];
			//copy( a1[i].begin(), a1[i].end(), appParams );
			LPWSTR appParams=(LPWSTR)a1[i].c_str();
			lvItem.pszText=appParams;
			//LPWSTR appParams1 = new wchar_t[a2[i].size()];
			//copy( a2[i].begin(), a2[i].end(), appParams1 );
			LPWSTR appParams1=(LPWSTR)a2[i].c_str();
			LPCTSTR lpct = appParams1;
			nItem = m_paramaterList.InsertItem(&lvItem);
			m_paramaterList.SetItemText(nItem, 1, CA2W("A"));
			m_paramaterList.SetItemText(nItem, 2,lpct );
		}
	}
	else
	if(cursel == 1)	
	{
		int nCount = m_paramaterList.GetItemCount();
			for (int i=0; i < nCount; i++)
			{
				m_paramaterList.DeleteItem(0);
			}

			for(int i=0;i<ns1.size();i++)
			{
	
				LVITEM lvItem;
				int nItem;
				lvItem.mask = LVIF_TEXT;
				lvItem.iItem = i+1;
				lvItem.iSubItem = 0;
				//LPWSTR appParams = new wchar_t[ns1[i].size()];
				//copy( ns1[i].begin(), ns1[i].end(), appParams );
				LPWSTR appParams=(LPWSTR)ns1[i].c_str();
				lvItem.pszText=appParams;
				//LPWSTR appParams1 = new wchar_t[ns2[i].size()];
				//copy( ns2[i].begin(), ns2[i].end(), appParams1 );
				LPWSTR appParams1=(LPWSTR)ns2[i].c_str();
				LPCTSTR lpct = appParams1;
				nItem = m_paramaterList.InsertItem(&lvItem);
				m_paramaterList.SetItemText(nItem, 1, CA2W("NS"));
				m_paramaterList.SetItemText(nItem, 2,lpct );

			}

		}
	else
	if(cursel == 2)	
	{

		int nCount = m_paramaterList.GetItemCount();
		for (int i=0; i < nCount; i++)
		{
			m_paramaterList.DeleteItem(0);
		}

			for(int i=0;i<srv1.size();i++)
			{
	
				LVITEM lvItem;
				int nItem;
				lvItem.mask = LVIF_TEXT;
				lvItem.iItem = i+1;
				lvItem.iSubItem = 0;
				//LPWSTR appParams = new wchar_t[srv1[i].size()];
				//copy( srv1[i].begin(), srv1[i].end(), appParams );
				LPWSTR appParams=(LPWSTR)srv1[i].c_str();
				lvItem.pszText=appParams;
				//LPWSTR appParams1 = new wchar_t[srv2[i].size()];
				//copy( srv2[i].begin(), srv2[i].end(), appParams1 );
				LPWSTR appParams1=(LPWSTR)srv2[i].c_str();
				LPCTSTR lpct = appParams1;
				nItem = m_paramaterList.InsertItem(&lvItem);
				m_paramaterList.SetItemText(nItem, 1, CA2W("SRV"));
				m_paramaterList.SetItemText(nItem, 2,lpct );

			}

		}
	
	*pResult = 0;
}


	void CMy1DnsReporterDlg::OnClickedButton1()
	{
		// TODO: Add your control notification handler code here
/*
		int nCount = m_paramaterList.GetItemCount();

		// Delete all of the items from the list view control. 
		for (int i=0; i < nCount; i++)
		{
			m_paramaterList.DeleteItem(0);
		}
*/

	/*
		m_paramaterList.DeleteAllItems();
		ASSERT(m_paramaterList.GetItemCount() == 0);
    */
		a1.clear();
		a2.clear();
		ns1.clear();
		ns2.clear();
		srv1.clear();
		srv2.clear();
		

		WSADATA firstsock;
		CString str;
        if (WSAStartup(MAKEWORD(1,1),&firstsock) != 0)
        {
            str.Format(_T("Failed. Error Code : %d"),WSAGetLastError());
			AfxMessageBox(str,MB_ICONINFORMATION,MB_OK);
            return;
        }

	unsigned char hostname[100];
	char cname[50];
	CString dname,hname,len;
	LPTSTR lpt;
	m_domainNames.GetWindowTextW(dname);
	//dname.Delete(dname.GetLength()-1,1);

	if(dname.IsEmpty())
	{	
		//AfxMessageBox(L"Cannot list hostnames without domain name.",MB_ICONINFORMATION,MB_OK);
		MessageBox(L"Cannot list hostnames without domain name.", L"DNS Reporter", MB_OK|MB_ICONINFORMATION);
		return; 
	}

		lpt=dname.GetBuffer(dname.GetLength());

		//memcpy(hostname,dname,dname.GetLength());
		//strcpy(cname,(char *) lpt);
		//hname.Format(_T("%s"),dname);
		//AfxMessageBox(dname);


		FILE *f,*f1;
        int i=0;
        ofstream fout;

        f=fopen("default-in.txt","w");
        fprintf(f,"set type=any\nls -t ");
        fclose(f);

        fout.open("default-in.txt",ios::app);
		while(i<=dname.GetLength())
		{
			cname[i]=(char) lpt;
			fout<<(char *) lpt++;
			i++;
		}
		fout<<endl;
        fout.close();
		cname[i]='\0';

		//strcpy((char *)hostname, cname);
		//fout<<hostname<<endl;

        //ngethostbyname(hostname);

        printf("\n\n");
       //system("C:/Windows/System32/nslookup<default-in.txt>default-output.txt");

		f1=fopen("cmmd.bat","w");
		fprintf(f1,"CALL C:/Windows/System32/nslookup<default-in.txt>default-output.txt");
        fclose(f1);
		
/*
		STARTUPINFO StartupInfo = {};
		StartupInfo.cb = sizeof StartupInfo;
		PPROCESS_INFORMATION ProcessInfo;
		memset(&ProcessInfo, 0, sizeof(ProcessInfo));
		CreateProcess(L"c:\\windows\\system32\\nslookup<default-in.txt>default-output.txt", 
		NULL, 
		NULL,NULL,FALSE,0,NULL,
		NULL,&StartupInfo,ProcessInfo);
*/
		DWORD dw;
		CString strng;
		BOOL noError;
		STARTUPINFO startupInfo;
		PROCESS_INFORMATION processInformation;
		memset(&processInformation, 0, sizeof(processInformation));
		ZeroMemory(&startupInfo, sizeof(startupInfo));
		startupInfo.cb = sizeof(startupInfo);
		startupInfo.dwFlags = STARTF_USESHOWWINDOW;
		startupInfo.wShowWindow = SW_HIDE;
		//L"<default-in.txt>default-output.txt"
		
		noError = CreateProcess(L"cmmd.bat" ,NULL, NULL,
		NULL, FALSE, CREATE_NO_WINDOW | CREATE_UNICODE_ENVIRONMENT, NULL, NULL, &startupInfo, &processInformation );
		if(!noError)
		{
			dw=GetLastError(); 
			strng.Format(L"%u",dw);
			MessageBox(strng, L"DNS Reporter", MB_OK|MB_ICONINFORMATION);
		}
		
		WaitForSingleObject( processInformation.hProcess, INFINITE );
	//if(system("C:/Windows/System32/nslookup<default-in.txt>default-output.txt")==0)			
		//AfxMessageBox(L"DNS server is not accessible.",MB_ICONERROR,MB_OK);
    
	dname.Append(_T("."));
	ifstream file("default-output.txt");

//	ifstream file("output-type-ANY.txt");
	wstring ws(dname);
	string strr;
	strr.assign(ws.begin(),ws.end());
    string word,s,ss,sss;
	wstring s1,ss1,sss1;
	
    char ch;

    for(int i=0;i<4;i++)
        getline(file,s);
    s.clear();

	if(getline(file,s)!='\0')
	{
    while(file >> s)
    {
        if(s.compare(">")!=0)
        {
            file>>ss;
            if((ss.compare("A")==0) && (s.compare(strr)!=0) && (s.compare("DomainDnsZones")!=0) && (s.compare("ForestDnsZones")!=0))
            {
		  s1.assign(s.begin(), s.end());
				a1.push_back(s1);
				//a1.push_back(s);
                file>>sss;
				sss1.assign(sss.begin(), sss.end());
				a2.push_back(sss1);
                //a2.push_back(sss);
            }
            else if(ss.compare("NS")==0)
            {
		 s1.assign(s.begin(), s.end());
		ns1.push_back(s1);
                file>>sss;
				sss1.assign(sss.begin(), sss.end());
                ns2.push_back(sss1);
            }
          else
            {
                ch = file.get();
                while (ch != '\n')
                {
                    ch = file.get();
                    sss+=ch;
                }
                if(ss.compare("SRV")==0)
                {
			s1.assign(s.begin(), s.end());
			srv1.push_back(s1);
				sss1.assign(sss.begin(), sss.end());
                    srv2.push_back(sss1);
                }

            }
        }
        s.clear();
        ss.clear();
        sss.clear();
    }

	}
	else
		//AfxMessageBox(L"Enter the domain name with 'Allow Zone Transfers' option enabled.",MB_ICONINFORMATION,MB_OK);
		MessageBox(L"Enter the domain name with 'Allow Zone Transfers' option enabled.", L"DNS Reporter", MB_OK|MB_ICONINFORMATION);

	/*	wstring test=L"helloo";
	LPWSTR lp=(LPWSTR)test.c_str();
	//LPWSTR lp=new wchar_t[test.size()];
	//copy(test.begin(),test.end(),lp);
	AfxMessageBox(lp);

	//CString cs;
	LPWSTR lpwstr=new wchar_t[a1[0].size()];
	copy(a1[0].begin(),a1[0].end(),lpwstr);
	//AfxMessageBox(cs);
	AfxMessageBox(lpwstr);

*/	

/*
   for(int i=0;i<a1.size();i++)
            cout<<a1[i]<<"\t\tA\t\t"<<a2[i]<<"\n";

    for(int i=0;i<ns1.size();i++)
            cout<<ns1[i]<<"\t\tNS\t\t"<<ns2[i]<<"\n";

    for(int i=0;i<srv1.size();i++)
            cout<<srv1[i]<<"\tSRV\t"<<srv2[i]<<"\n";
*/
    file.close();
	remove("default-in.txt");
	remove("default-output.txt");
	remove("cmmd.bat");

	cursel=m_paramaterTab.GetCurSel();

	if(cursel == 0)	
	{
		int nCount = m_paramaterList.GetItemCount();
		for (int i=0; i < nCount; i++)
		{
			m_paramaterList.DeleteItem(0);
		}
	for(int i=0;i<a1.size();i++)
	{
		LVITEM lvItem;
		int nItem;
		lvItem.mask = LVIF_TEXT;
		lvItem.iItem = i+1;
		lvItem.iSubItem = 0;
		//LPWSTR appParams = new wchar_t[a1[i].size()];
		//copy( a1[i].begin(), a1[i].end(), appParams );
		LPWSTR appParams=(LPWSTR)a1[i].c_str();
		lvItem.pszText=appParams;
		//LPWSTR appParams1 = new wchar_t[a2[i].size()];
		//copy( a2[i].begin(), a2[i].end(), appParams1 );
			LPWSTR appParams1=(LPWSTR)a2[i].c_str();
		LPCTSTR lpct = appParams1;
		nItem = m_paramaterList.InsertItem(&lvItem);
		m_paramaterList.SetItemText(nItem, 1, CA2W("A"));
		m_paramaterList.SetItemText(nItem, 2,lpct );
	}
	}
	else
	if(cursel == 1)	
	{
		int nCount = m_paramaterList.GetItemCount();
			for (int i=0; i < nCount; i++)
			{
				m_paramaterList.DeleteItem(0);
			}


			for(int i=0;i<ns1.size();i++)
			{
	
				LVITEM lvItem;
				int nItem;
				lvItem.mask = LVIF_TEXT;
				lvItem.iItem = i+1;
				lvItem.iSubItem = 0;
				//LPWSTR appParams = new wchar_t[ns1[i].size()];
				//copy( ns1[i].begin(), ns1[i].end(), appParams );
				LPWSTR appParams=(LPWSTR)ns1[i].c_str();
				lvItem.pszText=appParams;
				//LPWSTR appParams1 = new wchar_t[ns2[i].size()];
				//copy( ns2[i].begin(), ns2[i].end(), appParams1 );
				LPWSTR appParams1=(LPWSTR)ns2[i].c_str();
				LPCTSTR lpct = appParams1;
				nItem = m_paramaterList.InsertItem(&lvItem);
				m_paramaterList.SetItemText(nItem, 1, CA2W("NS"));
				m_paramaterList.SetItemText(nItem, 2,lpct );

			}

		}
	else
	if(cursel == 2)	
	{
		int nCount = m_paramaterList.GetItemCount();
			for (int i=0; i < nCount; i++)
			{
				m_paramaterList.DeleteItem(0);
			}

			for(int i=0;i<srv1.size();i++)
			{
	
				LVITEM lvItem;
				int nItem;
				lvItem.mask = LVIF_TEXT;
				lvItem.iItem = i+1;
				lvItem.iSubItem = 0;
				//LPWSTR appParams = new wchar_t[srv1[i].size()];
				//copy( srv1[i].begin(), srv1[i].end(), appParams );
				LPWSTR appParams=(LPWSTR)srv1[i].c_str();
				lvItem.pszText=appParams;
				//LPWSTR appParams1 = new wchar_t[srv2[i].size()];
				//copy( srv2[i].begin(), srv2[i].end(), appParams1 );
				LPWSTR appParams1=(LPWSTR)srv2[i].c_str();
				LPCTSTR lpct = appParams1;
				nItem = m_paramaterList.InsertItem(&lvItem);
				m_paramaterList.SetItemText(nItem, 1, CA2W("SRV"));
				m_paramaterList.SetItemText(nItem, 2,lpct );

			}

		}
	    
	if(getInfo()){
			if(getFileDate()==0){
				
				addDb();
			}
			else if(startApp())
				updateDb();
			writeToFile(24);
			}
/*	
		CString msg;
		int nCount = m_paramaterList.GetItemCount();
		msg.Format(_T("%d"),nCount);
		AfxMessageBox(msg,MB_ICONINFORMATION,MB_OK);

		for (int i=0; i < nCount; i++)
		{
			m_paramaterList.DeleteItem(0);
		}

*/
	
	}


	void CMy1DnsReporterDlg::OnSelchangeCombo1()
	{
		// TODO: Add your control notification handler code here
	}


	void CMy1DnsReporterDlg::OnNMClickSyslink2(NMHDR *pNMHDR, LRESULT *pResult)
	{
		// TODO: Add your control notification handler code here
		PNMLINK pNMLink=(PNMLINK)pNMHDR;
		::ShellExecute(m_hWnd,_T("open"),pNMLink->item.szUrl,NULL,NULL,SW_SHOWNORMAL);
		*pResult = 0;
	}



	void CMy1DnsReporterDlg::OnClickedButton4()
	{
		// TODO: Add your control notification handler code here
		RevertToSelf();
		OnCancel();
	}



	HBRUSH CMy1DnsReporterDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
	{
		if( IDC_STATIC130 == pWnd->GetDlgCtrlID())
	   {	   
       CPoint ul(0,0);
       CRect rect;
       pWnd->GetWindowRect( &rect );
       CPoint lr( (rect.right-rect.left-2), (rect.bottom-rect.top-2) ); 
       pDC->FillSolidRect( CRect(ul, lr), RGB(255,255,255) );
       pWnd->SetWindowPos( &wndTop, 0, 0, 0, 0, SWP_NOMOVE|SWP_NOSIZE );		   
	   } 

	// TODO:  Change any attributes of the DC here

	// TODO:  Return a different brush if the default is not desired
	switch (nCtlColor)
       {
       case CTLCOLOR_STATIC:
	   if(IDC_STATIC5 == pWnd->GetDlgCtrlID() || IDC_STATIC1 == pWnd->GetDlgCtrlID() || IDC_STATIC6 == pWnd->GetDlgCtrlID() || IDC_STATIC7 == pWnd->GetDlgCtrlID() || IDC_STATIC2 == pWnd->GetDlgCtrlID() || IDC_STATIC3 == pWnd->GetDlgCtrlID())
	   pDC->SetBkColor(RGB(255, 255 , 255));
	   else if(IDC_SYSLINK2 == pWnd->GetDlgCtrlID())
	   {
	     pDC->SetBkColor(RGB(244, 244 , 244));
		 pDC->SetTextColor(RGB(0,0,255));
	   }
	   else
	   pDC->SetBkColor(RGB(244, 244 , 244));
       return (HBRUSH)GetStockObject(NULL_BRUSH);
       default:
	   return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
	   }
	}


	BOOL CMy1DnsReporterDlg::OnEraseBkgnd(CDC* pDC)
	{
		// TODO: Add your message handler code here and/or call default

		 CRect r;
         pDC->GetClipBox(&r);
		 pDC->FillSolidRect(r, RGB(244,244,244)); 
         return TRUE;
	}


	void CMy1DnsReporterDlg::OnBnClickedButton2()
	{
		// TODO: Add your control notification handler code here
		this->UpdateData();
		USES_CONVERSION;
		if(m_paramaterList.GetItemCount() == 0)	
			MessageBox(L"No Data To Export",L"CSV Generator" ,MB_OK | MB_ICONINFORMATION);
		else
		{		
			CFile f;
			CString strFilter = _T("CSV Files (*.csv)|*.csv|All Files (*.*)|*.*||");
			CFileDialog FileDlg(FALSE, _T(".csv"), NULL, 0, strFilter,this);
			std::string path;
			if( FileDlg.DoModal() == IDOK )
			{
				HDITEM hdi;
				CString path = FileDlg.GetPathName();
				f.Open(path, CFile::modeCreate | CFile::modeWrite);
				CArchive ar(&f, CArchive::store);
				LVCOLUMN col;
				col.mask = LVCF_TEXT;
				CString st,r,r1;
				wchar_t *attstr;		
				attstr = (wchar_t *)calloc(8000,sizeof(wchar_t));
				CHeaderCtrl* pHeaderCtrl = m_paramaterList.GetHeaderCtrl();
				if (pHeaderCtrl != NULL)
				{
					TCHAR  lpBuffer[256];
					hdi.mask = HDI_TEXT;
					hdi.pszText = lpBuffer;
					hdi.cchTextMax = 256;

					int nColumnCount = pHeaderCtrl->GetItemCount();
				//	AfxMessageBox(nColumnCount);
					for(int i = 0 ;i<nColumnCount;i++)				
					{
					//	AfxMessageBox(i);

						BOOL status = pHeaderCtrl->GetItem(i,&hdi);
					//	AfxMessageBox(status);
					//	AfxMessageBox(hdi.pszText);
						ar.WriteString(hdi.pszText);
						if(i != count-1)			
							ar.WriteString(L",");
									
					}
				}			
				ar.WriteString(L"\r\n");
				free(attstr);
				for(unsigned int i = 0;i < m_paramaterList.GetItemCount();i ++)
				{
					for(unsigned int j = 0;j < m_paramaterList.GetHeaderCtrl()->GetItemCount();j ++)
					{
						ar.WriteString(this->m_paramaterList.GetItemText(i,j));
						if(j < count-1)
							ar.WriteString(L",");
					}
					ar.WriteString(L"\r\n");			
				}
			ar.Close();
			
			}
			else
				return;
			f.Close();
		}
	}
