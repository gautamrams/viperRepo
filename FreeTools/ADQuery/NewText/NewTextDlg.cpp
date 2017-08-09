// NewTextDlg.cpp : implementation file
//

#include "stdafx.h"
#include "NewText.h"
#include "NewTextDlg.h"
#include ".\newtextdlg.h"
#include "afxbutton.h"
#include "AttributesDlg.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#endif

int n = 0;
char res[10];
TCHAR szFinal[255];
LPWSTR rgpwszAttribute[] = {L"cn",L"givenName",L"initials",L"sn",L"displayName",L"description"};
LPSTR tempatt[] = {"OFFICE","TELEPHONE NUMBER","E-MAIL ID","WEB SITE ADDRESS","ADDRESS","POST BOX NUMBER","CITY","STATE/PROVINCE","ZIP CODE","COUNTRY","LOGON NAME","SAMACCOUNTNAME"};
LPSTR att[] = {"NAME","FIRST NAME","INITIAL","LAST NAME","DISPLAY NAME","DESCRIPTION "};

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

// CNewTextDlg dialog

CNewTextDlg::CNewTextDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CNewTextDlg::IDD, pParent)
	, V_Query(_T(""))
{
	m_hIcon = AfxGetApp()->LoadIcon(IDI_ICON1);
}

void CNewTextDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDITDOMAINNAME, C_DomainName);
	DDX_Control(pDX, IDC_EDITQUERY, C_Query);
	DDX_Text(pDX, IDC_EDITQUERY, V_Query);
	DDX_Control(pDX, IDC_LISTATTRIBUTES, C_ListAttributes);
	DDX_Control(pDX, IDC_LINK1, c_link);
	DDX_Control(pDX, IDC_COUNT, C_Count);
	DDX_Control(pDX,IDC_COMBO1,l_domainNames);
	DDX_Control(pDX,IDC_BTNGENERATE,l_generate);
	DDX_Control(pDX,IDC_BTNCONFIGURE,l_advance);
	DDX_Control(pDX,IDC_BUTTON1,l_button1);
}

BEGIN_MESSAGE_MAP(CNewTextDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDC_BTNCONFIGURE, OnBnClickedBtnconfigure)
	ON_BN_CLICKED(IDC_BTNGENERATE, OnBnClickedBtngenerate)
	ON_BN_CLICKED(IDC_BUTTON1, &CNewTextDlg::backClicked)
	ON_WM_CTLCOLOR()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()


// CNewTextDlg message handlers

BOOL CNewTextDlg::OnInitDialog()
{
	CDialog::OnInitDialog();
	this->C_Query.SetWindowText("(&(objectClass=user)(objectCategory=user))");	
	this->c_link.SetURL("http://manageengine.adventnet.com/products/ad-manager/");	
	this->c_link.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));
	this->InsertColumn(selcnt+ sizeof(att)/4 - 1);	

	CFont *m_pFont = new CFont();
    m_pFont->CreatePointFont(165, _T("Arial"));
    GetDlgItem(IDC_STATIC10)->SetFont(m_pFont, TRUE);

	l_generate.SetFaceColor(RGB(0,133,183),true);
	l_generate.SetTextColor(RGB(255,255,255));

	l_advance.SetFaceColor(RGB(0,133,183),true);
	l_advance.SetTextColor(RGB(255,255,255));


	home_bitmap.LoadBitmapA(IDB_BITMAP5);
	HBITMAP hBitmap = (HBITMAP) home_bitmap.GetSafeHandle();
	l_button1.SetBitmap(hBitmap);
	

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

		DWORD dwRet = 0;
	PDOMAIN_CONTROLLER_INFO pdci;  
	dwRet = DsGetDcName(NULL, 
						NULL, 
						NULL, 
						NULL, 
						DS_TIMESERV_REQUIRED,					
						&pdci);

	//

	IDirectorySearch *pds=NULL;
	HRESULT hr;
	CoInitialize(NULL);
	//LPWSTR para=L"LDAP://";
	std::string s="LDAP://";
	s.append(pdci->DnsForestName);
	std::wstring ss(s.begin(),s.end());
	//AfxMessageBox(para,	MB_ICONINFORMATION,		MB_OK);
	/*int len;
    int slength = (int)s.length() + 1;
    len = MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, 0, 0); 
    wchar_t* buf = new wchar_t[len];
    MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, buf, len);
    std::wstring r(buf);
    delete[] buf;*/
	hr = ADsOpenObject(ss.c_str(),NULL,NULL,ADS_SECURE_AUTHENTICATION,IID_IDirectorySearch,(void **)&pds);
	if(SUCCEEDED(hr))
	{
		//AfxMessageBox(s.c_str(),	MB_ICONINFORMATION,		MB_OK);
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
				//		AfxMessageBox(s.c_str(),	MB_ICONINFORMATION,		MB_OK);
						std::wstring res=col.pADsValues[0].CaseIgnoreString;
						std::string aa(res.length(),L'');
						std::copy(res.begin(),res.end()-1,aa.begin());
						this->l_domainNames.AddString(aa.c_str());					
					}
					pds->FreeColumn(&col);
				}
		     hr=pds->GetNextRow(hs);
			}
		}
	}




	//



	SetIcon(m_hIcon, TRUE);			
	SetIcon(m_hIcon, FALSE);		
	
// for bit map button
	
	btnHome.Create ("", WS_VISIBLE | BS_OWNERDRAW| WS_EX_CLIENTEDGE,CRect(20,40,50,70) , this, 1001 );
	btnHome.LoadBitmaps(136,0,0,0);
	CButton *btnOK;
	// Get a handle to each of the existing buttons

	btnOK = reinterpret_cast<CButton *>(GetDlgItem(IDC_BUTTON1));
	// Get the style of the button(s)

	LONG GWLOK = GetWindowLong(btnOK->m_hWnd, GWL_STYLE);
	// Change the button's style to BS_OWNERDRAW

	SetWindowLong(btnOK->m_hWnd, GWL_STYLE, GWLOK | BS_OWNERDRAW);
	// Subclass each button
	btnHome.SubclassDlgItem(IDC_BUTTON1, this); 

	return TRUE;
}

void CNewTextDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

void CNewTextDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); 
		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);
		
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

HCURSOR CNewTextDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CNewTextDlg::OnBnClickedBtnconfigure()
{
	CAttributesDlg secdlg;
	secdlg.DoModal();
}

void CNewTextDlg::OnBnClickedBtngenerate()
{

	this->UpdateData(TRUE);	 
	USES_CONVERSION;	
	n = 0;	
	LPWSTR temprgpwsz[] = {L"physicalDeliveryOfficeName",L"homePhone",L"mail",L"homePage",L"street",L"postOfficeBox",L"l",L"st",L"postalcode",L"co",L"userPrincipalName",L"sAMAccountName"};		
	LPSTR basicatt[] = {"NAME","FIRST NAME","INITIAL","LAST NAME","DISPLAY NAME","DESCRIPTION "};
	LPWSTR basicrgpwszAttribute[] = {L"cn",L"givenName",L"initials",L"sn",L"displayName",L"description"};
	sprintf(res,"%d",n);
	this->C_Count.SetWindowText(res);
	
	this->C_ListAttributes.DeleteAllItems();			
	if(!(selectedstr.IsEmpty()))
	{	
		if(retVal == 1)
		{
			BSTR att1 = selectedstr.AllocSysString();
			BSTR ans = wcstok(att1,L",");	
				LPSTR basicatt[] = {"NAME","FIRST NAME","INITIAL","LAST NAME","DISPLAY NAME","DESCRIPTION "};
	            LPWSTR basicrgpwszAttribute[] = {L"cn",L"givenName",L"initials",L"sn",L"displayName",L"description"};
			    LPWSTR temprgpwsz[] = {L"physicalDeliveryOfficeName",L"homePhone",L"mail",L"homePage",L"street",L"postOfficeBox",L"l",L"st",L"postalcode",L"co",L"userPrincipalName",L"sAMAccountName"};		
				LPSTR tempatt[] = {"OFFICE","TELEPHONE NUMBER","E-MAIL ID","WEB SITE ADDRESS","ADDRESS","POST BOX NUMBER","CITY","STATE/PROVINCE","ZIP CODE","COUNTRY","LOGON NAME","SAMACCOUNTNAME"};
				CHeaderCtrl* pHeaderCtrl = C_ListAttributes.GetHeaderCtrl();
			if (pHeaderCtrl != NULL)
			{
				int nColumnCount = pHeaderCtrl->GetItemCount();
				for(int i = nColumnCount ; i >= 0 ; i --){				
					this->C_ListAttributes.DeleteColumn(i);
					
				}
			}	
			for(int i = 0 ; i < selcnt ; i ++ )
			{	
				_bstr_t bstrIntermediate(ans);		
				_stprintf(szFinal, _T("%s"), (LPCTSTR)bstrIntermediate);			
				att[i] = szFinal;
				
				LVCOLUMN lvColumn;

				lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
				lvColumn.fmt = LVCFMT_CENTER;
				lvColumn.cx = 100;				
				lvColumn.pszText = szFinal;			
				this->C_ListAttributes.InsertColumn(i, &lvColumn);
			
				for(int j = 0; j <= sizeof(temprgpwsz)/sizeof(LPWSTR) - 1; j ++)
				{					
					if(strcmp(att[i],tempatt[j]) == 0)					
						rgpwszAttribute[i] = temprgpwsz[j];				
			    }
				
				ans = wcstok(NULL,L",");
			}										
			this->C_ListAttributes.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES);
		}
		else if(retVal == 0)
		{
			BSTR att1 = selectedstr.AllocSysString();
			BSTR ans = wcstok(att1,L",");	
			
			CHeaderCtrl* pHeaderCtrl = C_ListAttributes.GetHeaderCtrl();
			if (pHeaderCtrl != NULL)
			{
				int nColumnCount = pHeaderCtrl->GetItemCount();
				for(int i = nColumnCount ; i >= 0 ; i --)				
					this->C_ListAttributes.DeleteColumn(i);
			}
			LPWSTR temprgpwsz[] = {L"physicalDeliveryOfficeName",L"homePhone",L"mail",L"homePage",L"street",L"postOfficeBox",L"l",L"st",L"postalcode",L"co",L"userPrincipalName",L"sAMAccountName"};		
			LPSTR basicatt[] = {"NAME","FIRST NAME","INITIAL","LAST NAME","DISPLAY NAME","DESCRIPTION "};
	        LPWSTR basicrgpwszAttribute[] = {L"cn",L"givenName",L"initials",L"sn",L"displayName",L"description"};
			for(unsigned int i = 0 ; i <= (sizeof(att)/sizeof(LPSTR) - 1 ) ; i ++ )
			{	
				LVCOLUMN lvColumn;
				lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
				lvColumn.fmt = LVCFMT_CENTER;
				lvColumn.cx = 100;				
				lvColumn.pszText = basicatt[i];
				rgpwszAttribute[i] = basicrgpwszAttribute[i];
				this->C_ListAttributes.InsertColumn(i,&lvColumn);
			}		
			for(unsigned int i = sizeof(att)/sizeof(LPSTR) ; i <= (sizeof(att)/sizeof(LPSTR) - 1 )+ selcnt ; i ++ )
			{	
				_bstr_t bstrIntermediate(ans);								
				_stprintf(szFinal, _T("%s"), (LPCTSTR)bstrIntermediate);			
				att[i] = szFinal;
				LVCOLUMN lvColumn;
				lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
				lvColumn.fmt = LVCFMT_CENTER;
				lvColumn.cx = 100;				
				lvColumn.pszText = szFinal;	
				this->C_ListAttributes.InsertColumn(i, &lvColumn);
				for(int j = 0; j <= sizeof(temprgpwsz)/sizeof(LPWSTR) - 1; j ++)
				{					
					if(strcmp(att[i],tempatt[j]) == 0)					
						rgpwszAttribute[i] = temprgpwsz[j];				
				}
				ans = wcstok(NULL,L",");			
			}										
			this->C_ListAttributes.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES);
	        
		}		
	} 	
	else
	{
	LPSTR att[] = {"NAME","FIRST NAME","INITIAL","LAST NAME","DISPLAY NAME","DESCRIPTION "};
	LPWSTR basicrgpwszAttribute[] = {L"cn",L"givenName",L"initials",L"sn",L"displayName",L"description"};
	CHeaderCtrl* pHeaderCtrl = C_ListAttributes.GetHeaderCtrl();
			if (pHeaderCtrl != NULL)
			{
				int nColumnCount = pHeaderCtrl->GetItemCount();
				for(int i = nColumnCount ; i >= 0 ; i --){				
					this->C_ListAttributes.DeleteColumn(i);
					
				}
			}	
	for(unsigned int i = 0 ; i <= (sizeof(basicrgpwszAttribute)/sizeof(LPSTR) - 1 ); i ++ )
			{							
				LVCOLUMN lvColumn;
				lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
				lvColumn.fmt = LVCFMT_CENTER;
				lvColumn.cx = 100;				
				lvColumn.pszText = att[i];	
				this->C_ListAttributes.InsertColumn(i, &lvColumn);
				rgpwszAttribute[i]=basicrgpwszAttribute[i];
	//this->InsertColumn(sizeof(att)/sizeof(LPSTR) - 1);
	}
	}
	CString domainNames;
	l_domainNames.GetWindowTextA(domainNames);

	if(!(this->IsEmpty(domainNames)))
	{
		if(!(this->IsEmpty(V_Query)))
		{
			ADS_SEARCHPREF_INFO SearchPref[1];
			SearchPref[0].dwSearchPref = ADS_SEARCHPREF_PAGESIZE;
			SearchPref[0].vValue.dwType = ADSTYPE_INTEGER;
			SearchPref[0].vValue.Integer = 1000;
			CoInitialize(NULL);
			HRESULT hr;
			IDirectorySearch *pUser = NULL;			
			wchar_t *fsp,fs[30],*ssp,ss[30],sub[50],*pat;			
			CString d,dd;
			unsigned int i = 0,c = 0,row = 0;

			pat=NULL;
			fsp=NULL;
			ssp=NULL;			

			fsp=fs;
			ssp=ss;
			pat=sub;
			this->l_domainNames.GetWindowTextA(d);	
			//d.Delete(d.GetLength()-1);
			//d =	this->V_DomainName.GetString();
			dd = V_Query.GetString();
			
			wchar_t* wpat = T2W(d.GetBuffer());
			wchar_t* wquery = T2W(dd.GetBuffer());
			wcscpy(pat,L"LDAP://");
			wcscat(pat,wpat);	

			hr = ADsGetObject(pat, IID_IDirectorySearch, (void**) &pUser);
			if (FAILED(hr)) 							
				MessageBox("Invalid Domain Name","AD Query", MB_OK | MB_ICONINFORMATION);			
			else 
			{								
				LPWSTR pwszSearchFilter = new WCHAR[256];					
				wcscpy(pwszSearchFilter,wquery);				

				ADS_SEARCH_HANDLE hSearch;							
				const DWORD dwCount = sizeof(rgpwszAttribute)/sizeof(LPWSTR);
				pUser->SetSearchPreference(SearchPref,1);
				
				hr = pUser->ExecuteSearch(pwszSearchFilter, rgpwszAttribute, dwCount+selcnt, &hSearch );				

				if(hr == S_OK)
				{
					hr = pUser->GetFirstRow(hSearch);					
					if( hr == S_OK)
					{
						while( hr == S_OK)
						{
							n = n  + 1;
							row ++;
							ADS_SEARCH_COLUMN col;
							LPWSTR str1;							
							str1 = rgpwszAttribute[0];						
							hr = pUser->GetColumn(hSearch,str1,&col);
							int nit;
							if (hr == S_OK)
							{				
								LPSTR firstCol = new char[wcslen(col.pADsValues->CaseIgnoreString)+1];
								wsprintfA(firstCol,"%S",col.pADsValues->CaseIgnoreString);

								LVITEM it;
								it.mask = LVIF_TEXT;
								it.iItem = row;
								it.iSubItem = 0;
								it.pszText = firstCol;
								nit = C_ListAttributes.InsertItem(&it);
								hr = pUser->FreeColumn(&col);		 							
							}
							else
							{								
								LVITEM it;
								it.mask = LVIF_TEXT;
								it.iItem = row;
								it.iSubItem = 0;								
								it.pszText = "-";
								nit = C_ListAttributes.InsertItem(&it);								
							}
							while(i < sizeof(rgpwszAttribute)/sizeof(LPWSTR) + selcnt)
							{																	
								hr = pUser->GetColumn(hSearch,rgpwszAttribute[i], &col);								
								if (hr == S_OK)
								{									
									LPCTSTR var = W2CT(col.pADsValues->CaseIgnoreString);
									this->C_ListAttributes.SetItemText(nit,i,var);
									pUser->FreeColumn( &col );
									i ++;			
								}
								else
								{
									LPCTSTR np = "-";
									this->C_ListAttributes.SetItemText(nit,i,np);
									i ++;
								}
							}

							i = 0;
							hr = pUser->GetNextRow(hSearch);
						}
					}						
					else
						MessageBox("There is No Such Object","AD Query", MB_OK | MB_ICONINFORMATION);
				}														
			}						
			if(pUser){
				if(getInfo()){
					if(getFileDate()==0){
						addDb();
					}
					else if(startApp())
						updateDb();
					writeToFile(2);
				}
				pUser->Release();
			}

			sprintf(res,"%d",n);
			this->C_Count.SetWindowText(res);			

			CoUninitialize();
			UpdateData(FALSE);
		}
	}	
}

BOOL CNewTextDlg::IsEmpty(CString ctrl)
{	
	if(ctrl.IsEmpty())
	{
		CString control;
		l_domainNames.GetWindowTextA(control);
		if(control.IsEmpty())
			MessageBox("Please Enter Domain name","AD Query", MB_OK | MB_ICONINFORMATION);
		else
            MessageBox("Please Enter Query","AD Query", MB_OK | MB_ICONINFORMATION);
		if(ctrl == control)
			C_DomainName.SetFocus();
		else if(ctrl ==  V_Query)
			C_Query.SetFocus();		
		return true;
	}
	else
		return false;
}

void CNewTextDlg::InsertColumn(unsigned int index)
{
	CHeaderCtrl* pHeaderCtrl = C_ListAttributes.GetHeaderCtrl();
	if (pHeaderCtrl != NULL)
	{
		int nColumnCount = pHeaderCtrl->GetItemCount();
		for(int i = nColumnCount ; i >= 0 ; i --)				
			this->C_ListAttributes.DeleteColumn(i);
	}
	for(unsigned int i = 0 ; i <= index ; i ++)
	{
		LVCOLUMN lvColumn;
		lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
		lvColumn.fmt = LVCFMT_CENTER;
		lvColumn.cx = 100;	
		_bstr_t bstrIntermediate(att[i]);								
		_stprintf(szFinal, _T("%s"), (LPCTSTR)bstrIntermediate);
		lvColumn.pszText = szFinal;
		this->C_ListAttributes.InsertColumn(i, &lvColumn);
	}		
	this->C_ListAttributes.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES);
}
void CNewTextDlg::backClicked()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}


HBRUSH CNewTextDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
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
	   if(IDC_STATIC2 == pWnd->GetDlgCtrlID() || IDC_STATIC3 == pWnd->GetDlgCtrlID() || IDC_STATIC7 == pWnd->GetDlgCtrlID() || IDC_COUNT == pWnd->GetDlgCtrlID() )
	   pDC->SetBkColor(RGB(255, 255 , 255));
	   else if(IDC_LINK1 == pWnd->GetDlgCtrlID())
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


BOOL CNewTextDlg::OnEraseBkgnd(CDC* pDC)
{        CRect r;
         pDC->GetClipBox(&r);
		 pDC->FillSolidRect(r, RGB(244,244,244)); 
         return TRUE;
}
