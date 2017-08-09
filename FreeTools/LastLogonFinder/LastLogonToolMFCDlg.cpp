// LastLogonToolMFCDlg.cpp : implementation file
//

#include "stdafx.h"
#include "LastLogonToolMFC.h"
#include "LastLogonToolMFCDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#define RED        RGB(127,  0,  0)
#define GREEN      RGB(  0,127,  0)
#define BLUE       RGB(  0,  0,127)
#define LIGHTRED   RGB(255,  0,  0)
#define LIGHTGREEN RGB(  0,255,  0)
#define LIGHTBLUE  RGB(  0,  0,255)
#define BLACK      RGB(  0,  0,  0)
#define WHITE      RGB(255,255,255)
#define GRAY       RGB(192,192,192)
#define custom     RGB(51,51,51)


//CStringArray distinguishedNameArray, sAMAccountNameArray;
// attribute[] used to represent the ListView Column Headers
LPWSTR attribute[] = {L"FirstName",L"LastName",L"DistinguishedName",L"sAMAccountName",L"UserPrincipalName"};
int lcnt,n=0,ret,count=0;
CString sample;
char searchcount[10];//Stroes Value of the username was to be searched
TCHAR szFinal[255];
HRESULT hr = S_OK;
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
public:
	afx_msg void OnBnClickedButton1();
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	ON_BN_CLICKED(IDC_BUTTON1, &CAboutDlg::OnBnClickedButton1)
END_MESSAGE_MAP()


// CLastLogonToolMFCDlg dialog




CLastLogonToolMFCDlg::CLastLogonToolMFCDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CLastLogonToolMFCDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDI_ICON1);
	if (!button1.LoadBitmaps(_T("GETLASTLOGONDETAILSU"), _T("GETLASTLOGONDETAILSD"), _T(							"GETLASTLOGONDETAILSF"), _T("GETLASTLOGONDETAILSX")) ||
		!button2.LoadBitmaps(_T("SEARCHD"), _T("SEARCHU"), _T("SEARCHF"), _T("SEARCHX")) ||
		!button3.LoadBitmaps(_T("SELECTALLD"), _T("SELECTALLU"), _T("SELECTALLF"), _T("SELECTALLX"))||
		!button4.LoadBitmaps(_T("DESELECTALLD"), _T("DESELECTALLU"), _T("DESELECTALLF"),_T("DESELECTALLX"))||
		!button5.LoadBitmaps(_T("HOMED"), _T("HOMEU"), _T("HOMEF"), _T("HOMEX")))
	{
		TRACE0("Failed to load bitmaps for buttons\n");
		AfxThrowResourceException();
	}
}

void CLastLogonToolMFCDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST1, v_SearchUser);
	DDX_Control(pDX, IDC_EDIT1, l_userName);
	DDX_Control(pDX, IDC_COMBO1, l_domainNames);
	DDX_Control(pDX, IDC_P1Label1, c_P1Label1);
	DDX_Control(pDX, IDC_P1Label2, c_P1Label2);
	DDX_Control(pDX, IDC_P1Label3, c_P3Label3);

	DDX_Control(pDX, IDC_P1DomainName, c_P1DomainName);
	DDX_Control(pDX, IDC_P1UserName, c_P1UserName);
	DDX_Control(pDX, IDC_P1Link1, c_link);
	DDX_Control(pDX, IDC_P1LabelLink, c_P1LabelLink);
	DDX_Control(pDX, IDC_Result, c_searchResult);
}

BEGIN_MESSAGE_MAP(CLastLogonToolMFCDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	
	ON_BN_CLICKED(IDC_GETLASTLOGONDETAILS, &CLastLogonToolMFCDlg::OnBnClickedGetlastlogondetails)
	ON_BN_CLICKED(IDC_SEARCH, &CLastLogonToolMFCDlg::OnBnClickedSearch)
	ON_BN_CLICKED(IDC_SELECTALL, &CLastLogonToolMFCDlg::OnBnClickedSelectall)
	ON_BN_CLICKED(IDC_DESELECTALL, &CLastLogonToolMFCDlg::OnBnClickedDeselectall)
	ON_BN_CLICKED(IDC_HOME, &CLastLogonToolMFCDlg::OnBnClickedHome)
END_MESSAGE_MAP()


// CLastLogonToolMFCDlg message handlers

BOOL CLastLogonToolMFCDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	this->c_P1Label1.SetFontBold(true);
	this->c_P1Label1.SetTextColor( custom);
	this->c_P1Label1.SetFontName(L"Arial");
	this->c_P1Label1.SetFontSize(9);

	this->c_P1Label2.SetTextColor( custom);
	this->c_P1Label2.SetFontName(L"Arial");
	this->c_P1Label2.SetFontSize(9);

	this->c_P3Label3.SetTextColor(custom);
	this->c_P3Label3.SetFontName(L"Arial");
	this->c_P3Label3.SetFontSize(9);

	this->c_P1DomainName.SetTextColor(custom);
	this->c_P1DomainName.SetFontName(L"Arial");
	this->c_P1DomainName.SetFontSize(10);
	
	this->c_P1UserName.SetTextColor(custom);
	this->c_P1UserName.SetFontName(L"Arial");
	this->c_P1UserName.SetFontSize(10);
	
	this->c_P1LabelLink.SetTextColor(custom);
	this->c_P1LabelLink.SetFontName(L"Arial");
	this->c_P1LabelLink.SetFontSize(8);

	this->c_link.SetURL(_T("http://manageengine.adventnet.com/products/ad-manager/"));	
	this->c_link.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));
	
	VERIFY(button2.SubclassDlgItem(IDC_SEARCH, this));
	button2.SizeToContent();
	VERIFY(button5.SubclassDlgItem(IDC_HOME, this));
	button5.SizeToContent();

	VERIFY(button1.SubclassDlgItem(IDC_GETLASTLOGONDETAILS, this));
	VERIFY(button4.SubclassDlgItem(IDC_DESELECTALL, this));
	VERIFY(button3.SubclassDlgItem(IDC_SELECTALL, this));
	
	v_SearchUser.DeleteAllItems();
	lcnt = 0;
	sample.Empty();
	
	this->InsertColumns(lcnt+(sizeof(attribute)/sizeof(LPWSTR)));	
	
	DWORD dwRet;
	CString strText;
	PDOMAIN_CONTROLLER_INFO pdcInfo;
	LPOLESTR rootStr = new OLECHAR[MAX_PATH]; 
	dwRet = DsGetDcName(NULL, NULL, NULL, NULL, 0, &pdcInfo);
	
	this->l_domainNames.AddString(pdcInfo->DnsForestName);
	//this->l_domainNames.AddString(pdcInfo->DomainName);
	NetApiBufferFree(pdcInfo);
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

	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CLastLogonToolMFCDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

void CLastLogonToolMFCDlg::OnPaint()
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
/**
*	InsertColumns(int) method used to Set the column Headers in the ListView
*	parameter:	indes - represents the number of columns
*	return	: void

*/
void CLastLogonToolMFCDlg::InsertColumns(int index)
{		
	CHeaderCtrl* pHeaderCtrl = v_SearchUser.GetHeaderCtrl();
	if (pHeaderCtrl != NULL)
	{
		int nColumnCount = pHeaderCtrl->GetItemCount();
		for(int i = nColumnCount ; i >= 0 ; i --)				
			this->v_SearchUser.DeleteColumn(i);
	}	
	
	for(int i = 0 ; i < index ; i ++)
	{
		LVCOLUMN lvColumn;

		lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
		lvColumn.fmt = LVCFMT_CENTER;
		lvColumn.cx = 128;
		_bstr_t bstrIntermediate(attribute[i]);								
		_stprintf(szFinal, _T("%s"), (LPCTSTR)bstrIntermediate);								
		lvColumn.pszText = szFinal;
		this->v_SearchUser.InsertColumn(i, &lvColumn);
	}
	this->v_SearchUser.SetExtendedStyle( LVS_EX_GRIDLINES | LVS_EX_CHECKBOXES );
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CLastLogonToolMFCDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}
/** OnBnClickedGetlastlogondetails() Event Handler- when the user clicks the button GetLastLogonDetails
*	parameter:	-
*	return:		void
*/
void CLastLogonToolMFCDlg::OnBnClickedGetlastlogondetails()
{
	// TODO: Add your control notification handler code here
	// TODO: Add your control notification handler code here
	int flag=0;
	int item = l_domainNames.GetCurSel();
	
	int nItem = 0;
	lastlogonreport lastlogonreportdlg;
    for(nItem =0 ; nItem <  v_SearchUser.GetItemCount(); nItem++)
    {
		
         BOOL bChecked = v_SearchUser.GetCheck(nItem);
         if( bChecked == 1 )
         {
			 flag=1;
             lastlogonreportdlg.setDistinguishedNames(v_SearchUser.GetItemText(nItem, 2));
			 lastlogonreportdlg.setsAMAccountNames(v_SearchUser.GetItemText(nItem, 3));
	     }
	}
	 if( flag==0)
	 {
		 AfxMessageBox(L" Select at least one user!",	MB_ICONINFORMATION,		MB_OK);
		 return;
	 }
	 CString text;//Used to stored domain names from combo box
	l_domainNames.GetWindowTextW(text);
	DomainControllerLists domainControllersLists;
	domainControllersLists.setDomainName(text);
	domainControllersLists.DoModal();
}
/** OnBnClickedSearch() Event Handler- when the user clicks the button Search
*	parameter:	-
*	return:		void
*/
void CLastLogonToolMFCDlg::OnBnClickedSearch()
{
	// TODO: Add your control notification handler code here
	CString text;
	l_domainNames.GetWindowTextW(text);
	if(text.IsEmpty())
	{	
		AfxMessageBox(L"Cannot complete search without domain name.",MB_ICONINFORMATION,MB_OK);
		return; 
	}
	
	
	if(!button1.ShowWindow(SW_SHOW)){
		button1.ShowWindow(SW_SHOW);
		button1.SizeToContent();
	}
	if(!button3.ShowWindow(SW_SHOW)){
	button3.ShowWindow(SW_SHOW);
	button3.SizeToContent();
	}
	if(!button4.ShowWindow(SW_SHOW)){
	button4.ShowWindow(SW_SHOW);
	button4.SizeToContent();}

	v_SearchUser.DeleteAllItems();
	
	 CString str = _T("");
	l_userName.GetWindowText(str);
	
		
	HRESULT hr = S_OK;
	IDirectorySearch     *pDSSearch    =NULL;
	LPWSTR szUsername = NULL; // Username
	LPWSTR szPassword = NULL; // Password
	LPOLESTR rootStr = new OLECHAR[MAX_PATH]; 
	CoInitialize(NULL);
	//LPWSTR userName=  l_userName.GetDlgItemText();
	wcscpy(rootStr,L"LDAP://");
	wcscat(rootStr, text);
	hr = ADsOpenObject(rootStr,szUsername,szPassword,	ADS_SECURE_AUTHENTICATION,								IID_IDirectorySearch,(void **)&pDSSearch);
	if(SUCCEEDED(hr))	
	{
		LPWSTR pszAttr[] =
		{L"givenName",L"cn",L"distinguishedName",L"sAMAccountName",L"userPrincipalName"};	
		ADS_SEARCH_HANDLE hSearch;
		DWORD dwCount = 0;
		DWORD dwAttrNameSize = sizeof(pszAttr)/sizeof(LPWSTR);
		printf("dwAttrNameSize %d\n",dwAttrNameSize);
		CString strText;
		int nColumnCount = this->v_SearchUser.GetHeaderCtrl()->GetItemCount();
		wcscpy(rootStr,L"(&(&(objectClass=user)(objectClass=person))(|(givenName=");
		wcscat(rootStr, str);
		wcscat(rootStr, L"*)(userPrincipalName=");
		wcscat(rootStr, str);
		wcscat(rootStr, L"*)(sn=");
		wcscat(rootStr,str);
		wcscat(rootStr, L"*)(sAMAccountName=");
		wcscat(rootStr, str);
		wcscat(rootStr, L"*)(displayName=");
		wcscat(rootStr, str);
		wcscat(rootStr,L"*)))");
		hr=pDSSearch->ExecuteSearch(rootStr,pszAttr ,dwAttrNameSize,&hSearch );
		//printf("%x\n",hr);
		if(SUCCEEDED(hr))	
		{	//printf("Searching hr success\n");
			LPWSTR pszColumn;
			ADS_SEARCH_COLUMN col;
			count=0;
			while(pDSSearch->GetNextRow( hSearch)==S_OK)
			{	
				this->v_SearchUser.InsertItem(LVIF_TEXT|LVIF_STATE, count, strText, 
							(count%2)==0 ? LVIS_SELECTED : 0, LVIS_SELECTED, 0, 0);
				for(int idx = 0; idx < dwAttrNameSize; idx++ )
				{
					hr = pDSSearch->GetColumn(hSearch,pszAttr[idx], &col );
					if ( SUCCEEDED(hr) )
					{	//typeCheck(col);
						
						switch (col.dwADsType)
						{
						 case ADSTYPE_CASE_IGNORE_STRING:
							strText.Format(TEXT("%s"), col.pADsValues->CaseIgnoreString);
							this->v_SearchUser.SetItemText(count, idx, strText);
							break;
						 case ADSTYPE_DN_STRING:
							strText.Format(TEXT("%s"), col.pADsValues->CaseIgnoreString);
							this->v_SearchUser.SetItemText(count, idx, strText);
							break;
						 case ADSTYPE_PROV_SPECIFIC:
							strText.Format(TEXT("%s"), col.pADsValues->ProviderSpecific.lpValue);
							this->v_SearchUser.SetItemText(count, idx, strText);
							break;
			 			 default:
							strText.Format(TEXT("-"));
							this->v_SearchUser.SetItemText(count, idx, strText);
							 break;
					  }
					pDSSearch->FreeColumn( &col );
					}
				else
				{	//puts("description property NOT available");	
					strText.Format(TEXT("-"));
					this->v_SearchUser.SetItemText(count, idx, strText);
				}
				}
			count++;
			}
			pDSSearch->CloseSearchHandle(hSearch);
			pDSSearch->Release();

		}
	}
	else
	{
		AfxMessageBox(L"Domain can't be contact",MB_ICONINFORMATION,MB_OK);
		return; 
	}
	this->SetCount();
	CoUninitialize();
	
	
}
/** SetCount()- Used to Display the text, "Search Result for: which user entered username"
*	parameter:	-
*	return:		void
*/
void CLastLogonToolMFCDlg::SetCount()
{
	sprintf(searchcount, "%d", count);
	LPOLESTR rootStr = new OLECHAR[MAX_PATH]; 
	 CString str = _T("");
	l_userName.GetWindowText(str);
	wcscpy(rootStr,L"Search Result for : ");
	wcscat(rootStr, str );
	this->c_searchResult.SetWindowTextW(rootStr);
	l_userName.SetWindowText(_T(""));
	//this->c_searchResult.s
}
/** OnBnClickedSelectall()-Eventhandler when user clicks Selectall button.
*	parameter:	-
*	return:		void
*/
void CLastLogonToolMFCDlg::OnBnClickedSelectall()
{
	// TODO: Add your control notification handler code here
	for( int nItem =0 ; nItem <  v_SearchUser.GetItemCount(); nItem++)
    		 v_SearchUser.SetCheck(nItem,TRUE);
}
/** OnBnClickedDeselectall()-Eventhandler when user clicks DeSelectall button.
*	parameter:	-
*	return:		void
*/
void CLastLogonToolMFCDlg::OnBnClickedDeselectall()
{
	// TODO: Add your control notification handler code here
	for( int nItem =0 ; nItem <  v_SearchUser.GetItemCount(); nItem++)
		v_SearchUser.SetCheck(nItem,FALSE);
}
/** OnBnClickedHome()-Eventhandler when user clicks Home button.
*	parameter:	-
*	return:		void
*/
void CLastLogonToolMFCDlg::OnBnClickedHome()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}

void CAboutDlg::OnBnClickedButton1()
{
	// TODO: Add your control notification handler code here
}
