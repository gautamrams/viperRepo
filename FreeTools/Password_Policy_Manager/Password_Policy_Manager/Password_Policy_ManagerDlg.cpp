// Password_Policy_ManagerDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Password_Policy_Manager.h"
#include "Password_Policy_ManagerDlg.h"
#include "dialog_pwdpolicy.h"
#include "hyperlink.h"
#include "hyperlink.cpp"

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


// CPassword_Policy_ManagerDlg dialog




CPassword_Policy_ManagerDlg::CPassword_Policy_ManagerDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CPassword_Policy_ManagerDlg::IDD, pParent)
	, uname(_T(""))
	, pwd(_T(""))
{
	m_hIcon = AfxGetApp()->LoadIcon(IDI_ICON1);
}

void CPassword_Policy_ManagerDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_UNAME, uname);
	DDX_Text(pDX, IDC_PWD, pwd);
	DDX_Control(pDX, IDC_LOGIN, m_button1);
	DDX_Control(pDX, IDCANCEL, m_button2);
	DDX_Control(pDX, IDC_HOME, button_home);
	DDX_Control(pDX, IDC_COMBO1, l_domainNames);
	DDX_Control(pDX, IDC_ADSSP, link);
}

BEGIN_MESSAGE_MAP(CPassword_Policy_ManagerDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_EN_CHANGE(IDC_UNAME, &CPassword_Policy_ManagerDlg::OnEnChangeUname)
	ON_EN_CHANGE(IDC_PWD, &CPassword_Policy_ManagerDlg::OnEnChangePwd)
	ON_WM_CTLCOLOR()
	ON_BN_CLICKED(IDC_HOME, &CPassword_Policy_ManagerDlg::OnBnClickedHome)
	ON_BN_CLICKED(IDC_LOGIN, &CPassword_Policy_ManagerDlg::OnBnClickedLogin)
	
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()


// CPassword_Policy_ManagerDlg message handlers

BOOL CPassword_Policy_ManagerDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	this->link.SetURL(L"www.adselfserviceplus.com");	
	this->link.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));

	bitmap_home.LoadBitmapW(IDB_BITMAP1);
	HBITMAP hBitmap = (HBITMAP) bitmap_home.GetSafeHandle();
	button_home.SetBitmap(hBitmap);

	
	BITMAP bm;
	bitmap_home.GetBitmap(&bm);
	int width = bm.bmWidth;
	int height = bm.bmHeight;
	CRect rec;
	button_home.GetWindowRect(&rec);
	ScreenToClient(&rec);
	rec.right = rec.left + width;
	rec.bottom = rec.top + height;
	button_home.MoveWindow(&rec);

	CDialog::SetDefID(IDC_LOGIN);

	CEdit *foc = (CEdit*)GetDlgItem(IDC_UNAME);
	foc->SetFocus();

	CFont *m_pFont = new CFont();
    m_pFont->CreatePointFont(165, _T("Arial"));
    GetDlgItem(IDC_STATIC10)->SetFont(m_pFont, TRUE);

	m_button1.SetFaceColor(RGB(0,133,183),true);
	m_button1.SetTextColor(RGB(255,255,255));

	m_button2.SetFaceColor(RGB(0,133,183),true);
	m_button2.SetTextColor(RGB(255,255,255));
	/*IADs *startin = NULL;
	IADs *dom = NULL;
	HRESULT hr;
	LPCWSTR domm,domm_new;

	CoInitialize(NULL);

	VARIANT var;

	domm = L"";
	hr = ADsGetObject(L"LDAP://rootDSE", IID_IADs, (void**) &startin);
	if(SUCCEEDED(hr))
	{
		hr = startin->Get(CComBSTR("defaultNamingContext"),&var);
		if(SUCCEEDED(hr))
		{
			domm = var.bstrVal;
			startin->Release();
		}
	}

	LPCWSTR domm_ldap = L"LDAP://";
	wchar_t dom_ldap[50];
	wcscpy(dom_ldap,domm_ldap);
	wcscat(dom_ldap,domm);
	LPCWSTR ldom_ldap = dom_ldap;
	domm_new = L"";

	hr = ADsGetObject(ldom_ldap, IID_IADs, (void**) &dom);
	if(SUCCEEDED(hr))
	{
		hr = dom->Get(CComBSTR("distinguishedName"),&var);
		if(SUCCEEDED(hr))
		{
			domm_new = V_BSTR(&var);
			dom->Release();
		}
	}*/

	domm_new = L"";
	DWORD dwRet = NULL;
	PDOMAIN_CONTROLLER_INFOW pdci;  
	dwRet = DsGetDcName(NULL, 
						NULL, 
						NULL, 
						NULL, 
						DS_TIMESERV_REQUIRED,					
						&pdci);
	//UpdateData(FALSE);
	//CoUninitialize();

	
//

	IDirectorySearch *pds=NULL;
	HRESULT hr;
	CoInitialize(NULL);
	//LPWSTR para=L"LDAP://";
	std::wstring s=L"LDAP://";
	s.append(pdci->DnsForestName);
	//AfxMessageBox(para,	MB_ICONINFORMATION,		MB_OK);
	/*int len;
    int slength = (int)s.length() + 1;
    len = MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, 0, 0); 
    wchar_t* buf = new wchar_t[len];
    MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, buf, len);
    std::wstring r(buf);
    delete[] buf;*/
	hr = ADsOpenObject(s.c_str(),NULL,NULL,ADS_SECURE_AUTHENTICATION,IID_IDirectorySearch,(void **)&pds);
	if(SUCCEEDED(hr))
	{
	//	AfxMessageBox(s.c_str(),	MB_ICONINFORMATION,		MB_OK);
		ADS_SEARCH_HANDLE hs;
		LPWSTR psa[] = {L"canonicalName"};
		DWORD ac= sizeof(psa)/sizeof(LPWSTR);
		hr = pds->ExecuteSearch(L"((objectCategory=Domain))", psa, ac, &hs );
		if(SUCCEEDED(hr))
		{
		//	AfxMessageBox(s.c_str(),	MB_ICONINFORMATION,		MB_OK);
			hr=pds->GetFirstRow(hs);
			while(hr==S_OK)
			{
			//	AfxMessageBox(s.c_str(),	MB_ICONINFORMATION,		MB_OK);
				ADS_SEARCH_COLUMN col;
				for(DWORD i=0;i<ac;i++)
				{
				//	AfxMessageBox(s.c_str(),	MB_ICONINFORMATION,		MB_OK);
					hr=pds->GetColumn(hs,psa[i],&col);
					if(SUCCEEDED(hr))
					{
					//	AfxMessageBox(s.c_str(),	MB_ICONINFORMATION,		MB_OK);
						//char cp[100];
						//lp=col.pADsValues[0].CaseIgnoreString;
						//wcstombs(cp,lp,sizeof(cp));
						//MessageBox(col.pADsValues[0].CaseIgnoreString);
						CString d=col.pADsValues[0].CaseIgnoreString;
						d.Delete(d.GetLength()-1,1);
						this->l_domainNames.AddString(d);							
					}
					pds->FreeColumn(&col);
				}
		
					hr=pds->GetNextRow(hs);
			}
		}
	}




	//


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

	return FALSE;  // return TRUE  unless you set the focus to a control
}

void CPassword_Policy_ManagerDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

void CPassword_Policy_ManagerDlg::OnPaint()
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
HCURSOR CPassword_Policy_ManagerDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


void CPassword_Policy_ManagerDlg::OnEnChangeUname()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialog::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	UpdateData();
}

void CPassword_Policy_ManagerDlg::OnEnChangePwd()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialog::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	UpdateData();
}


HBRUSH CPassword_Policy_ManagerDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	 if( IDC_STATIC6 == pWnd->GetDlgCtrlID())
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
	   if(IDC_STATIC3 == pWnd->GetDlgCtrlID() || IDC_STATIC4 == pWnd->GetDlgCtrlID() || IDC_STATIC5 == pWnd->GetDlgCtrlID() )
	   pDC->SetBkColor(RGB(255, 255 , 255));
	   else if(IDC_ADSSP== pWnd->GetDlgCtrlID())
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

void CPassword_Policy_ManagerDlg::OnBnClickedHome()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}

void CPassword_Policy_ManagerDlg::OnBnClickedLogin()
{
	// TODO: Add your control notification handler code here
	IDirectoryObject *dom = NULL;
	HRESULT hr;

	CoInitialize(NULL);

	LPCWSTR p,dom_name, user_name, password;
	//p = L"LDAP://";
	CString pp=L"LDAP://";
	CString d;
	l_domainNames.GetWindowTextW(d);
	//d.Delete(d.GetLength()-1,1);
	pp.Append(d);
	dom_name = pp;
	user_name = uname.GetString();
	password = pwd.GetString();

	hr = ADsOpenObject(dom_name,
					   user_name,
					   password,
					   ADS_SECURE_AUTHENTICATION,
					   IID_IDirectoryObject,
					   (void**) &dom);
	if(SUCCEEDED(hr))
	{

		LPWSTR attrnames[] = {L"pwdProperties"};
		DWORD num_attr = sizeof(attrnames)/sizeof(LPWSTR);
		ADS_ATTR_INFO *attrinfo = NULL;
		DWORD attr_num;

		hr = dom->GetObjectAttributes(attrnames, num_attr, &attrinfo, &attr_num);
		if(SUCCEEDED(hr))
		{
			dialog_pwdpolicy obj_pwdpolicy;
			obj_pwdpolicy.user_name_ppm = user_name;
			obj_pwdpolicy.password_ppm = password;
			obj_pwdpolicy.dom_name_ppm = dom_name;
			obj_pwdpolicy.DoModal();
			pwd.Empty();
		}	
		else
		{
			MessageBox(L"Check Login Details1",L"Password Policy Manager");
			pwd.Format(_T(""),0);
		}
	}
	else
	{
		if(d.IsEmpty()){
		MessageBox(L"Please Enter Domain Name",L"Password Policy Manager");
		pwd.Format(_T(""),0);
		}
	    else
	    {
		MessageBox(L"Please Enter User Name & Password correctly",L"Password Policy Manager");
		pwd.Format(_T(""),0);
		}
	}

	CoUninitialize();
	UpdateData(FALSE);
}





BOOL CPassword_Policy_ManagerDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	     CRect r;
         pDC->GetClipBox(&r);
		 pDC->FillSolidRect(r, RGB(244,244,244)); 
         return TRUE;
}
