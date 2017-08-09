// DMZAnalyzerDlg.cpp : implementation file
//

#include "stdafx.h"
#include "DMZAnalyzer.h"
#include "DMZAnalyzerDlg.h"
#include "afxbutton.h"
//#include "RPCEndPoint.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif
CStringArray success,fail;
char *ipAddress;

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


// CDMZAnalyzerDlg dialog




CDMZAnalyzerDlg::CDMZAnalyzerDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CDMZAnalyzerDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDI_ICON1);
}

void CDMZAnalyzerDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT1, v_hostName);
	//	DDX_Control(pDX, IDC_EDIT2, v_scanResult);
	DDX_Control(pDX, IDC_Hyper, c_link);
	DDX_Control(pDX, IDC_Status, v_status);
	DDX_Control(pDX, IDC_HOME, home_button);
	DDX_Control(pDX, IDC_BUTTON1, m_button1);
}

BEGIN_MESSAGE_MAP(CDMZAnalyzerDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDC_BUTTON1, &CDMZAnalyzerDlg::OnBnClickedButton1)
	
	ON_BN_CLICKED(IDC_HOME, &CDMZAnalyzerDlg::OnBnClickedHome)
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
END_MESSAGE_MAP()


// CDMZAnalyzerDlg message handlers

BOOL CDMZAnalyzerDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	
	home_bitmap.LoadBitmapW(IDB_BITMAP7);
	HBITMAP hBitmap = (HBITMAP) home_bitmap.GetSafeHandle();
	home_button.SetBitmap(hBitmap);

	CFont *m_pFont = new CFont();
    m_pFont->CreatePointFont(165, _T("Arial"));
    GetDlgItem(IDC_STATIC10)->SetFont(m_pFont, TRUE);

	m_button1.SetFaceColor(RGB(0,133,183),true);
	m_button1.SetTextColor(RGB(255,255,255));

	
	BITMAP bm;
	home_bitmap.GetBitmap(&bm);
	int width = bm.bmWidth;
	int height = bm.bmHeight;
	CRect rec;
	home_button.GetWindowRect(&rec);
	ScreenToClient(&rec);
	rec.right = rec.left + width;
	rec.bottom = rec.top + height;
	home_button.MoveWindow(&rec);
	

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
	
	this->c_link.SetURL(_T("http://manageengine.adventnet.com/products/ad-manager/"));	
	this->c_link.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));
	//VERIFY(button1.SubclassDlgItem(IDC_GETLASTLOGONDETAILS, this));
	VERIFY(v_status.SubclassDlgItem(IDC_Status,this));

	endPointArray[0]=88;//kerberos protocol
	endPointArray[1]=389;//Ldap protocol
	endPointArray[2]=139;//Netbios session or smb protocol
	endPointArray[3]=445;//SMB protocol
	//endPointArray[4]=636;//Ldap over https(LDAPS)
	endPointArray[4]=135;//Remote procedure protocol
	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CDMZAnalyzerDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

void CDMZAnalyzerDlg::OnPaint()
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
HCURSOR CDMZAnalyzerDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


void CDMZAnalyzerDlg::OnBnClickedButton1()
{
	CString str = _T("");
	v_hostName.GetWindowText(str);
	if(str.IsEmpty())
	{	
		AfxMessageBox(L"Please type in the IPAddress or Fully Qualified Host Name!",MB_ICONINFORMATION,MB_OK);
		return; 
	}
	v_status.SetWindowTextW(L"Querying...");
	fail.RemoveAll();
	success.RemoveAll();
	WellDefinePorts(str);//check whether welldefined ports are opened or not
	v_status.SetWindowTextW(L"                              ");
}
void CDMZAnalyzerDlg::WellDefinePorts(CString ipaddress)
{
	LPOLESTR rootStr = new OLECHAR[MAX_PATH]; //String is used display listening status
	LPOLESTR rootStr1 = new OLECHAR[MAX_PATH]; //String is used display the failure ports
	char portNo[10];
	BOOL flag=true;
	wcscpy(rootStr,L"Port No.");
	wcscpy(rootStr1,L"Following ports, which are essential for efficient functioning of ADManager Plus, are closed:   ");
	BOOL resolving=resolveIPAddress(ipaddress);//If hostname is given,convert to ipaddress
	if(!resolving)
	{
		AfxMessageBox(L"Trobleshoot the connectivity process!",MB_ICONINFORMATION|MB_OK);
		return;
	}
		printf("Success");
		for(int portCounter=0;portCounter<5;portCounter++)
		{
			printf("%d",endPointArray[portCounter]);
			if(ListeningPort(endPointArray[portCounter]))
					int s=endPointArray[portCounter];//endPointArray[portCounter] is success;
			else
			{
				if(endPointArray[portCounter]==135)
					flag=false;
			}
		}//for loop
		if(flag)
			DynamicPorts();//Check whether the ports associated with ActiveDirectory RPC services, are opened or not.
		else
		{
			for(int failPort=0;failPort<fail.GetCount();failPort++)
			{
				wcscat(rootStr1,fail.ElementAt(failPort));
				wcscat(rootStr1,L"    ");
			}
			v_status.SetWindowTextW(L"");
			AfxMessageBox(rootStr1,MB_ICONINFORMATION|MB_OK);
			
		}
		v_status.SetWindowTextW(L"");
}
void CDMZAnalyzerDlg::DynamicPorts()
{
		LPOLESTR rootStr1 = new OLECHAR[MAX_PATH]; //String is used display the failure ports
		wcscpy(rootStr1,L"The following ports are need to be open:  ");
		BOOL rpcPortResult=GetRpcDynamicPorts();//Check whether the ports associated with RPC services, are opened or not
		if(rpcPortResult)
		{		
			v_status.SetWindowTextW(L"");
			AfxMessageBox(L"All the required ports are open",MB_ICONINFORMATION,MB_OK);//succes	
			
			if(getInfo()){
			if(getFileDate()==0){
					addDb();
				}
				else if(startApp())
					updateDb();
				writeToFile(6);
			}
			return;
		}
		else
		{
			for(int failPort=0;failPort<fail.GetCount();failPort++)
			{
				wcscat(rootStr1,fail.ElementAt(failPort));
				wcscat(rootStr1,L"    ");
			}
			v_status.SetWindowTextW(L"");
			AfxMessageBox(rootStr1,MB_ICONINFORMATION|MB_OK);
		}
		
}
BOOL CDMZAnalyzerDlg::GetRpcDynamicPorts()
{
		//const wchar_t *p_netadr =L"192.168.117.36";//L"192.168.117.36";
        const wchar_t *p_endpoint = L"135";
		const wchar_t  *p_protocol=L"ncacn_ip_tcp";
		const wchar_t *p_UUID =L"8a885d04-1ceb-11c9-9fe8-08002b104860";//EndpointMapper UUID
		char *p_options = NULL;
		char * error=NULL;
        int enumep = 0;
		UUID iterateuuid;
		char portNo[10];
		BOOL httpPort=false,tcpPort=false;
		LPOLESTR rootStr = new OLECHAR[MAX_PATH];
		//CString str = _T("");
		//v_hostName.GetWindowText(str);
		char *ip=ipAddress;
		CString str(ipAddress);
		const wchar_t *p_netadr=str;//convert cstring to wchat_t *
		//const wchar_t *p_netadr=ipAddress;
		RPC_WSTR mybinding = NULL,resolvebinding=NULL,stringuuid=NULL,endPoint = NULL,protocol=NULL,ptr2,errorText=NULL;
		RPC_EP_INQ_HANDLE inquiryContext;//
		RPC_IF_ID ifId;
		RPC_BINDING_HANDLE m_hBindEndPoint;
		RPC_STATUS st;
		
		st=RpcNetworkIsProtseqValid((RPC_WSTR)p_protocol);//check the protocol sequence is valid/support for this server
		printf("protosequect valid status is %d\n",st);
		if(st!=RPC_S_OK)
		{
			printf("The given protocol sequence is not supported in that server: Error code %d\n",st);
			//return 0;
		}
	    st = RpcStringBindingCompose(NULL,(RPC_WSTR)p_protocol,(RPC_WSTR)p_netadr,(RPC_WSTR)p_endpoint,NULL,&mybinding);
		printf("Status is %d\n",st);
        if (st == RPC_S_OK)
		{
			printf("Binding: %S   ",mybinding);
			st = RpcBindingFromStringBinding(mybinding, &m_hBindEndPoint);
			printf("Second status is %d\n",st);
			if (st == RPC_S_OK)
			{
				st = RpcMgmtEpEltInqBegin(m_hBindEndPoint,RPC_C_EP_ALL_ELTS,NULL, RPC_C_VERS_ALL,NULL,&inquiryContext);
				if(st==RPC_S_OK)
				{
					while(RpcMgmtEpEltInqNext(inquiryContext, &ifId, &m_hBindEndPoint, &iterateuuid, &ptr2)==RPC_S_OK)
					{
						enumep++;
						st=UuidToString(&ifId.Uuid,&stringuuid);
						if(wcscmp(L"e3514235-4b06-11d1-ab04-00c04fc2dcd2",(wchar_t *)stringuuid)==0)
						{
							st = RpcBindingToStringBinding(m_hBindEndPoint,&resolvebinding);
							st=RpcStringBindingParse(resolvebinding,NULL,&protocol,NULL,&endPoint,NULL);
							if(wcscmp(L"ncacn_ip_tcp",(wchar_t *)protocol)==0)
							{
								printf("Protocol:%S\t Endpoint %S\n",protocol,endPoint);
								printf("uuid: %S\t Annotation:%S\n ",stringuuid,ptr2);
								int p=_tcstoul((const wchar_t*)endPoint,0,10);//convert cstring to integer
								if(ListeningPort(p))
									tcpPort=true;//success   ListeningResult.Add(_T(""));
								
							}
							/*else if(wcscmp(L"ncacn_http",(wchar_t *)protocol)==0)
							{
								printf("Protocol:%S\t Endpoint %S\n",protocol,endPoint);
								printf("uuid: %S\t Annotation:%S\n ",stringuuid,ptr2);
								int p=_tcstoul((const wchar_t*)endPoint,0,10);
								if(ListeningPort(p))
									httpPort=true;//Success     ListeningResult.Add(_T(""));
								
							}*/
						}//MS NT Directory DRS Interface - service compared if block
						else
						{
								//puts("Active directory interface is not supported in that rpc server\n");
							continue;
						}
					}//while
					RpcMgmtEpEltInqDone(&inquiryContext);
				}
				else
				{
					ErrorReport(st);
					return false;
				}
				RpcBindingFree(&m_hBindEndPoint);
			}
			else
			{
				ErrorReport(st);
				return false;
			}
		}
		else
		{
			ErrorReport(st);
			return false;
		}
		RpcStringFree(&mybinding);
		RpcStringFree(&resolvebinding);
		RpcStringFree(&stringuuid);
		RpcStringFree(&protocol);
		RpcStringFree(&ptr2);
		//return (httpPort&tcpPort);
		return (tcpPort);
}
BOOL CDMZAnalyzerDlg::ListeningPort(int port)//check given port no. listening or not
{
	WSADATA wsaData;
	char portNo[10];
	int iResult = WSAStartup(MAKEWORD(2,2), &wsaData);
	if (iResult != NO_ERROR)
		printf("Error at WSAStartup()\n");
	ConnectSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (ConnectSocket == INVALID_SOCKET) 
	{
		printf("Error at socket(): %ld\n", WSAGetLastError());
		WSACleanup();
    }
	BOOL portResult=true;
	CString str = _T("");
	/*v_hostName.GetWindowText(str);
	int nSize = str.GetLength();
	char *ipHost = new char[nSize+1];
	memset(ipHost,0,nSize+1);
	wcstombs(ipHost, str, nSize+1);*/
	
	sockaddr_in clientService; 
	clientService.sin_family = AF_INET;
	//clientService.sin_addr.s_addr = inet_addr(ipHost);
	clientService.sin_addr.s_addr = inet_addr(ipAddress);
	clientService.sin_port = htons(port);
	if ( connect( ConnectSocket, (SOCKADDR*) &clientService, sizeof(clientService) ) ==	SOCKET_ERROR) 
	{
		printf( "Failed to connect.\n" );
		WCHAR NameBuffer[100];
		itoa(port,portNo,10);//convert int to char
		swprintf(NameBuffer,L"%S",portNo);//convert char to wchar
		fail.Add(NameBuffer);
		portResult=false;
		//WSACleanup();
    }
	else
	{
		WCHAR NameBuffer[100];
		itoa(port,portNo,10);//convert int to char
		swprintf(NameBuffer,L"%S",portNo);//convert char to wchar
		success.Add(NameBuffer);
		printf("Connected to server.\n");
	}
	WSACleanup();
	closesocket(ConnectSocket);
	return portResult;
}
BOOL CDMZAnalyzerDlg::resolveIPAddress(CString address)
{	
	struct addrinfo * result;
	struct addrinfo * res;
    int error;
	BOOL resolvingResult=false;
	struct in_addr addr;
	struct hostent *remoteHost;
	WSADATA wsaData;
	int iResult = WSAStartup(MAKEWORD(2,2), &wsaData);
	if (iResult != NO_ERROR)
	{
		printf("Error at WSASta1rtup()\n");
		v_status.SetWindowTextW(L"");
		AfxMessageBox(L"Error at WSAStartup()",MB_ICONINFORMATION,MB_OK);
		return resolvingResult;
	}
	int nSize = address.GetLength();
	char *pAnsiString = new char[nSize+1];
	
	memset(pAnsiString,0,nSize+1);
	wcstombs(pAnsiString, address, nSize+1);
	DWORD dwError;
	error = getaddrinfo(pAnsiString, NULL, NULL, &result);
    if (error != 0)
    {   
        fprintf(stderr, "error in getaddrinfo: %S\n", gai_strerror(error));
        return resolvingResult;
    }   
	for (res = result; res != NULL; res = res->ai_next)
    {   
        error = getnameinfo(res->ai_addr, res->ai_addrlen, hostname, NI_MAXHOST, NULL, 0, 0); 
		if (error != 0)
		{
			fprintf(stderr, "error in getnameinfo: %s\n", gai_strerror(error));
			continue;
		}
		remoteHost=gethostbyname(hostname);
		if (remoteHost == NULL) 
		{
          dwError = WSAGetLastError();
          if (dwError != 0) 
		  {
            if (dwError == WSAHOST_NOT_FOUND) 
			{
                printf("Host not found\n");
				v_status.SetWindowTextW(L"");
				AfxMessageBox(L"Host not found",MB_ICONINFORMATION,MB_OK);
                return false;
            } else if (dwError == WSANO_DATA) 
			{
                //printf("No data record found\n");
				v_status.SetWindowTextW(L"");
				AfxMessageBox(L"Destination host not reachable. Try with IP address or trobleshoot the connectivity",MB_ICONINFORMATION,MB_OK);
                return false;
				//break;
            } else 
			{
                //printf("Function failed with error: %ld\n", dwError);
				v_status.SetWindowTextW(L"");
				AfxMessageBox(L"Function failed with error",MB_ICONINFORMATION,MB_OK);
                return false;
			}
		  }
		}
		else 
		{	
			addr.s_addr = *(u_long *) remoteHost->h_addr_list[0];
			printf("\tFirst IP Address: %s\n", (inet_ntoa(addr)));
			ipAddress=(inet_ntoa(addr));
			resolvingResult=true;
		}
	 	
	 	if (*hostname)
		{
			printf("hostname: %s\n", hostname);
			//resolvingResult=true;
		}
	}//for loop  
    freeaddrinfo(result);
	freeaddrinfo(res);
	return resolvingResult;
}


void CDMZAnalyzerDlg:: ErrorReport(RPC_STATUS status)
{
	switch (status)
                    {
                        case EPT_S_NOT_REGISTERED:
                                puts("Server reports no endpoints");
								v_status.SetWindowTextW(L"");
								AfxMessageBox(L"Server reports no endpoints",MB_ICONINFORMATION,MB_OK);
								break;
                        case RPC_S_SERVER_UNAVAILABLE:
                                puts("RPC server unavailable for the interface specified");
								v_status.SetWindowTextW(L"");
								AfxMessageBox(L"RPC server unavailable for the interface specified",MB_ICONINFORMATION,MB_OK);
								break;
                        case RPC_S_ACCESS_DENIED:
							printf("Access for making the remote procedure call was denied");
							v_status.SetWindowTextW(L"");
							AfxMessageBox(L"Access for making the remote procedure call was denied",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_BINDING_INCOMPLETE:
							printf("Not all required elements from the binding handle were supplied");
							v_status.SetWindowTextW(L"");
							AfxMessageBox(L"Not all required elements from the binding handle were supplied",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_CALL_CANCELLED:
							printf("The remote procedure call was canceled, or if a call time out was specified, the call timed out");
							v_status.SetWindowTextW(L"");
							AfxMessageBox(L"The remote procedure call was canceled, or if a call time out was specified, the call timed out",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_CALL_FAILED:
							printf("The remote procedure call failed");
							v_status.SetWindowTextW(L"");
							AfxMessageBox(L"The remote procedure call failed",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_CANT_CREATE_ENDPOINT:
							printf("The endpoint cannot be created");
							v_status.SetWindowTextW(L"");
							AfxMessageBox(L"The endpoint cannot be created",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_COMM_FAILURE:
							printf("Unable to communicate with the server");
							AfxMessageBox(L"Unable to communicate with the server",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_INTERNAL_ERROR:
							printf("An internal error has occurred in a remote procedure call");
							v_status.SetWindowTextW(L"");
							AfxMessageBox(L"An internal error has occurred in a remote procedure call",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_INVALID_BINDING:
							printf("The binding handle is invalid");
							AfxMessageBox(L"The binding handle is invalid",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_INVALID_NET_ADDR:
							printf("The network address is invalid");
							AfxMessageBox(L"The network address is invalid",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_INVALID_RPC_PROTSEQ:
							printf("The RPC protocol sequence is invalid\n");
							v_status.SetWindowTextW(L"");
							AfxMessageBox(L"The RPC protocol sequence is invalid",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_INVALID_STRING_UUID:
							printf("The string UUID is invalid");
							AfxMessageBox(L"The string UUID is invalid",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_NO_ENDPOINT_FOUND:
							printf("No endpoint has been found");
							v_status.SetWindowTextW(L"");
							AfxMessageBox(L"No endpoint has been found",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_NO_PROTSEQS:
							printf("There are no protocol sequences");
							v_status.SetWindowTextW(L"");
							AfxMessageBox(L"There are no protocol sequences",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_NO_PROTSEQS_REGISTERED:
							printf("No protocol sequences have been registered");
							AfxMessageBox(L"No protocol sequences have been registered",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_S_PROTOCOL_ERROR:
							printf("An RPC protocol error has occurred");
							v_status.SetWindowTextW(L"");
							AfxMessageBox(L"An RPC protocol error has occurred",MB_ICONINFORMATION,MB_OK);
							break;
						case RPC_X_NO_MORE_ENTRIES:
							printf("The list of servers available for the [auto_handle] binding has been exhausted");
							v_status.SetWindowTextW(L"");
							AfxMessageBox(L"The list of servers available for the [auto_handle] binding has been exhausted",MB_ICONINFORMATION,MB_OK);
							break;
						default:
							printf("The list of servers available for the [auto_handle] binding has been exhausted");
							v_status.SetWindowTextW(L"");
							AfxMessageBox(L"The list of servers available for the [auto_handle] binding has been exhausted",MB_ICONINFORMATION,MB_OK);
	                        break;
                    }		
}

//int endpt=atoi((const char *)endPoint);//(int)endPoint;//=wcstol(endPoint,endptr,10);
/*void CDMZAnalyzerDlg::OnBnClickedCancel()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}*/
/*void CDMZAnalyzerDlg::cancel()
{
	OnCancel();
}*/

void CDMZAnalyzerDlg::OnBnClickedHome()
{
	// TODO: Add your control notification handler code here
	fail.RemoveAll();
	success.RemoveAll();
	OnCancel();
}


BOOL CDMZAnalyzerDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	     CRect r;
         pDC->GetClipBox(&r);
		 pDC->FillSolidRect(r, RGB(244,244,244)); 
         return TRUE;
}


HBRUSH CDMZAnalyzerDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	 if( IDC_STATIC100 == pWnd->GetDlgCtrlID())
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
	   if(IDC_STATIC1 == pWnd->GetDlgCtrlID() || IDC_STATIC3 == pWnd->GetDlgCtrlID() || IDC_STATIC40 == pWnd->GetDlgCtrlID() || IDC_Status == pWnd->GetDlgCtrlID())
	   pDC->SetBkColor(RGB(255, 255 , 255));
	   else if(IDC_Hyper == pWnd->GetDlgCtrlID())
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
