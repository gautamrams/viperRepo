// uitoolDlg.cpp : implementation file
//

#include "stdafx.h"
#include "uitool.h"
#include "uitoolDlg.h"
#include ".\uitooldlg.h"
#include "SecondDlg.h"
#include <string>
#include "afxbutton.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#endif

wchar_t (*fnm)[200];
DWORD count;
LPSTR att[] = {"Common Name","Display Name","Description"};
LPWSTR rgpwszAttributes[] = {L"cn",L"displayName",L"description"};
LPSTR basicatt[] = {"Common Name","Display Name","Description"};
LPWSTR basicrgpwszAttributes[] = {L"cn",L"displayName",L"description"};
LPCTSTR objType[] = {"user","contact","group","computer"};
unsigned int row;
int flag = 0;
char uac[20];
char groupType[20];
int uacValues[1000];
int cnt = 0;


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

// CuitoolDlg dialog

CuitoolDlg::CuitoolDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CuitoolDlg::IDD, pParent)
	, filepath(_T(""))		
{
	m_hIcon = AfxGetApp()->LoadIcon(IDI_ICON2);  
}

void CuitoolDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST1, list2);
	DDX_Text(pDX, IDC_EDIT1, filepath);
	DDX_Control(pDX, IDC_COMBO1, cbox);	
	DDX_Control(pDX, IDC_number, m_nm);
	DDX_Control(pDX, IDC_LINK1, c_link1);
	DDX_Control(pDX, IDC_BUTTON2, CSVDeButton);
	DDX_Control(pDX, IDC_BUTTON4, CSVButton);
	DDX_Control(pDX, IDC_BUTTON3, Filebutton);
	DDX_Control(pDX, IDC_EXPORT, Exportbutton);
	DDX_Control(pDX, IDC_ADVANCE, Advancedbutton);
	DDX_Control(pDX, IDC_BUTTON1, Generatebutton);
	DDX_Control(pDX, IDCANCEL, home_button);
	
}

BEGIN_MESSAGE_MAP(CuitoolDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDC_BUTTON1, OnBnClickedButton1)
	ON_BN_CLICKED(IDC_EXPORT, OnBnClickedExport)
	ON_BN_CLICKED(IDC_BUTTON3, OnBnClickedButton3)
	ON_BN_CLICKED(IDC_ADVANCE, OnBnClickedAdvance)		
	ON_BN_CLICKED(IDCANCEL, &CuitoolDlg::backClicked)
	ON_BN_CLICKED(IDC_BUTTON4, &CuitoolDlg::OnBnClickedButton4)
	ON_BN_CLICKED(IDC_BUTTON2, &CuitoolDlg::OnBnClickedButton2)
	
	ON_WM_CTLCOLOR()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

// CuitoolDlg message handlers

BOOL CuitoolDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Add "About..." menu item to system menu.
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	this->c_link1.SetURL("http://manageengine.adventnet.com/products/ad-manager/");	
	this->c_link1.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));

	CFont *m_pFont = new CFont();
    m_pFont->CreatePointFont(165, _T("Arial"));
    GetDlgItem(IDC_STATIC10)->SetFont(m_pFont, TRUE);

	Generatebutton.SetFaceColor(RGB(0,133,183),true);
	Generatebutton.SetTextColor(RGB(255,255,255));

	Advancedbutton.SetFaceColor(RGB(0,133,183),true);
	Advancedbutton.SetTextColor(RGB(255,255,255));

	Exportbutton.SetFaceColor(RGB(0,133,183),true);
	Exportbutton.SetTextColor(RGB(255,255,255));

	CSVButton.SetFaceColor(RGB(0,133,183),true);
	CSVButton.SetTextColor(RGB(255,255,255));

	CSVDeButton.SetFaceColor(RGB(0,133,183),true);
	CSVDeButton.SetTextColor(RGB(255,255,255));

//	Filebutton.SetFaceColor(RGB(0,133,183),true);
//	Filebutton.SetTextColor(RGB(255,255,255));

	home_bitmap.LoadBitmapA(IDB_BITMAP6);
	HBITMAP hBitmap = (HBITMAP) home_bitmap.GetSafeHandle();
	home_button.SetBitmap(hBitmap);

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

	count  =sizeof(objType)/sizeof(LPSTR);	
	for(unsigned int i = 0;i < count;i ++)
		this->cbox.AddString(objType[i]);	
	this->cbox.SetCurSel(0);	
	 
	LVCOLUMN lcol;
	count = sizeof(att)/sizeof(LPSTR);	
	for(unsigned int i = 0;i < count;i ++)
	{
		lcol.mask = LVCF_FMT|LVCF_TEXT|LVCF_WIDTH;
		lcol.fmt = LVCFMT_LEFT;
		lcol.cx = 120;
		lcol.pszText = att[i];
		this->list2.InsertColumn(i,&lcol);
	}
	this->list2.SetExtendedStyle(LVS_EX_GRIDLINES|LVS_EX_FULLROWSELECT);

	//Bitmap button
    
	btnHome.Create ("", WS_VISIBLE | BS_OWNERDRAW| WS_EX_CLIENTEDGE,CRect(20,40,50,70) , this, 1001 );
	btnHome.LoadBitmaps(145,0,0,0);
	CButton *btnOK;
	// Get a handle to each of the existing buttons

	btnOK = reinterpret_cast<CButton *>(GetDlgItem(IDCANCEL));
	// Get the style of the button(s)

	LONG GWLOK = GetWindowLong(btnOK->m_hWnd, GWL_STYLE);
	// Change the button's style to BS_OWNERDRAW

	SetWindowLong(btnOK->m_hWnd, GWL_STYLE, GWLOK | BS_OWNERDRAW);
	// Subclass each button
	btnHome.SubclassDlgItem(IDCANCEL, this); 

	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CuitoolDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

void CuitoolDlg::OnPaint() 
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
HCURSOR CuitoolDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CuitoolDlg::OnBnClickedButton1()
{
	/*
	
		this->UpdateData(TRUE);
	count = 3;
	int f = 0;

	if((this->filepath.IsEmpty() == TRUE))	
		MessageBox("Select The File","CSV Generator", MB_OK | MB_ICONINFORMATION);		
	else
	{		
		unsigned int i,r = 1,l,k = 0,j,w,h,c,usersCount=0;
		CFileStatus status;
		CString sfile;	

		row = 0;
		sfile = this->filepath.GetString();

		if(CFile::GetStatus(sfile,status) == TRUE)
		{
			this->list2.DeleteAllItems();

			// code to get the number of lines in CSV

			FILE *fp;
			fp = fopen(sfile,"r");
			char ch = fgetc(fp);
			while(ch!=EOF)
			{
				if(ch=='\n')
					usersCount++;
				ch = fgetc(fp);
			}
			fclose(fp);
			

			//code to count commas
			wchar_t line[1500];
			FILE *stream;
			stream = fopen(sfile,"r");
			fgetws (line,1500,stream);
			for( i = 0 ; i < wcslen(line) ; i ++)
				if(line[i] == ',')
					r++;

			//code to get header fields
			wchar_t line1[1500];
			fnm = new wchar_t[r][100];
			rewind(stream);
			fgetws(line1,1500,stream);
			j = ftell(stream);
			for(i = 0,l = 0;i < wcslen(line1);i ++)
			{
				if(line1[i] == ',')
				{
					fnm[l][k] = '\0';
					l ++;
					k = 0;
				}
				else
				{
					fnm[l][k] = line1[i];
					k ++;
				}
			}
			fnm[l][k-1] ='\0';
			l ++;

			//code to get field values to construct filter
			wchar_t *v,*fil,com1[75],(*fldval)[100];
			v = (wchar_t *)calloc(1000,sizeof(wchar_t));
			int totalMemory = 80*(usersCount)*(r+1); // total no of users and number of attributes
			fil = (wchar_t *)calloc(totalMemory,sizeof(wchar_t));
			wcscpy(fil,L"(|");
			unsigned int index = this->cbox.GetCurSel();
			CString stng = NULL;
			this->cbox.GetLBText(index,stng);
			USES_CONVERSION;
			LPWSTR m = A2W(stng);
			if((stng == "user")||(stng == "contact"))
				wcscpy(com1,L"(&(objectCategory=person)(objectClass=");
			else
				wcscpy(com1,L"(&(objectCategory=");
			wcscat(com1,m);
			if(l > 1)
				wcscat(com1,L")(&");
			else
				wcscat(com1,L")");
			for(w = 0,h = 0;w < wcslen(com1);w ++)
				if(com1[w] == '&')
					h ++;
			while(!feof(stream))
			{
				fgetws(v,1000,stream);
				fldval = new wchar_t[l][100];
				wcscat(fil,com1);
				for(k = 0 , i = 0,j = 0;k < wcslen(v);k ++)
				{
					if(v[k] == ',')
					{
						fldval[i][j] = '\0';
						wcscat(fil,L"(");
						wcscat(fil,fnm[i]);
						wcscat(fil,L"=");
						wcscat(fil,fldval[i]);
						wcscat(fil,L")");
						i ++;
						j = 0;		
					}
					else
					{
						fldval[i][j] = v[k];
						j ++;
					}
				}

				if(!feof(stream))
					fldval[i][j-1] = '\0';
				else
					fldval[i][j] = '\0';

				wcscat(fil,L"(");
				wcscat(fil,fnm[i]);
				wcscat(fil,L"=");
				wcscat(fil,fldval[i]);
				wcscat(fil,L")");
			
				for(c = 0;c < h;c ++)
					wcscat(fil,L")");
				
				delete []fldval;
			}

			wcscat(fil,L")");	
			free(v);
	
	
	
	*/
	this->UpdateData(TRUE);
	count = 3;
	int f = 0;

	if((this->filepath.IsEmpty() == TRUE))	
		MessageBox("Select The File","CSV Generator", MB_OK | MB_ICONINFORMATION);		
	else
	{		
		unsigned int i,r = 1,l,k = 0,j,w,h,c,usersCount=0;
		CFileStatus status;
		CString sfile;	

		row = 0;
		sfile = this->filepath.GetString();

		if(CFile::GetStatus(sfile,status) == TRUE)
		{
			this->list2.DeleteAllItems();

			// code to get the number of lines in CSV

			FILE *fp;
			fp = fopen(sfile,"r");
			char ch = fgetc(fp);
			while(ch!=EOF)
			{
				if(ch=='\n')
					usersCount++;
				ch = fgetc(fp);
			}
			fclose(fp);
			

			//code to check header
			
			wchar_t header[200];
			FILE *stream;
			stream = fopen(sfile,"r");
			fgetws (header,200,stream);
			header[wcslen(header)-1]='\0';
			if(_wcsicmp(header,L"sAMAccountName") != 0)
			{
				MessageBox("Invalid Format :\nProvide sAMAccountName as header.","CSV Generator", MB_OK | MB_ICONINFORMATION);
				return;
			}
			//code to store Field Values.
			wchar_t line1[200];
		   	fnm = new wchar_t[usersCount][200];
			int i=0;
			rewind(stream);
			fgetws(line1,200,stream);
			while(!feof(stream))
			{
			 fgetws(line1,200,stream);
			 if( i < usersCount-1 )
			 line1[wcslen(line1)-1]='\0';
		     wcsncpy(fnm[i],line1,200);
			 i++;
		    }
			
			
			//code to get field values to construct filter
			wchar_t *v,*fil,com1[75];
			v = (wchar_t *)calloc(1000,sizeof(wchar_t));
			int totalMemory = 80*(usersCount)*(r+1); // total no of users and number of attributes
			fil = (wchar_t *)calloc(totalMemory,sizeof(wchar_t));
			wcscpy(fil,L"(|");
			unsigned int index = this->cbox.GetCurSel();
			CString stng = NULL;
			this->cbox.GetLBText(index,stng);
			USES_CONVERSION;
			LPWSTR m = A2W(stng);
			if((stng == "user")||(stng == "contact"))
				wcscpy(com1,L"(&(objectCategory=person)(objectClass=");
			else
				wcscpy(com1,L"(&(objectCategory=");
			wcscat(com1,m);
			wcscat(com1,L")");
			wcscat(com1,L"(samaccountname=");
			for(int i=0;i<usersCount;i++)
			{
			   wcscat(fil,com1);
			   wcscat(fil,fnm[i]);
			   wcscat(fil,L"))");			  
			}
			wcscat(fil,L")");
		

            
			
			DOMAIN_CONTROLLER_INFO *name;
			CoInitialize(NULL);
			HRESULT hr;
			IDirectorySearch *pUser;
			wchar_t path[60],name1[60];	
			DWORD getdc=DsGetDcName(NULL,NULL,NULL,NULL,NULL,&name);
			//name1 = new wchar_t[strlen(name->DomainName)];
			mbstowcs(name1,name->DomainName,strlen(name->DomainName));
			name1[strlen(name->DomainName)]='\0';
			wcscpy(path,L"LDAP://");
			wcscat(path,name1);
			hr = ADsOpenObject(path,NULL,NULL,ADS_SECURE_AUTHENTICATION,IID_IDirectorySearch, (void**) &pUser);
	        
			//if advanced tab is clicked

			if(valid == 1)
			{	
				if(retVal == 1)
				{
				CHeaderCtrl* pHeaderCtrl = list2.GetHeaderCtrl();
					if (pHeaderCtrl != NULL)
					{
						int nColumnCount = pHeaderCtrl->GetItemCount();
						for(int i = nColumnCount ; i >= 0 ; i --)				
							this->list2.DeleteColumn(i);
					}	
					LPSTR att[] = {"Common Name","Display Name","Description"};
                    
                    LPSTR basicatt[] = {"Common Name","Display Name","Description"};
                    LPWSTR basicrgpwszAttributes[] = {L"cn",L"displayName",L"description"};
					CString s1,s2,s3,s4;
					this->list2.DeleteAllItems();	

					//adding formatted advanced attributes to grid as heading
					char *pC = attlist1.GetBuffer(attlist1.GetLength()) ;
					char *formattr = strtok(pC,",");
					
					unsigned int p = 1;
					LVCOLUMN cl;
					    cl.mask = LVCF_FMT|LVCF_TEXT|LVCF_WIDTH;
						cl.fmt = LVCFMT_LEFT;
						cl.cx = 120;
						cl.pszText = "Common Name";
						this->list2.InsertColumn(p,&cl);
					while(formattr != NULL)
					{	
						cl.mask = LVCF_FMT|LVCF_TEXT|LVCF_WIDTH;
						cl.fmt = LVCFMT_LEFT;
						cl.cx = 120;
						att[p] = formattr;
						cl.pszText = att[p];
						if(strcmp(cl.pszText,"Account Status")==0)
						{
							flag=p;
						}
						this->list2.InsertColumn(p,&cl);
						p ++;
						formattr = strtok(NULL,",");
					}		
					
					LPWSTR wattlist = attlist.AllocSysString();
					LPWSTR ldapattr = wcstok(wattlist,L",");
					unsigned int y = 1;		
					while(ldapattr != NULL)
					{
						rgpwszAttributes[y] = ldapattr;
						ldapattr = wcstok(NULL,L",");			
						y ++;
					}
					count = count + x;	
					attlist.Empty();
					attlist1.Empty();
				}
				else if (retVal == 0)
				{
					
					CHeaderCtrl* pHeaderCtrl = list2.GetHeaderCtrl();
					if (pHeaderCtrl != NULL)
					{
						int nColumnCount = pHeaderCtrl->GetItemCount();
						for(int i = nColumnCount ; i >= 0 ; i --)				
							this->list2.DeleteColumn(i);
					}						
					CString s1,s2,s3,s4;
					this->list2.DeleteAllItems();	
					
					for(unsigned int i = 0 ; i <= sizeof(basicrgpwszAttributes) / sizeof(LPWSTR) ; i ++)					
					{
						rgpwszAttributes[i] = basicrgpwszAttributes[i];
					}
					
					LVCOLUMN lcol;
					for(unsigned int i = 0;i < count;i ++)
					{
						lcol.mask = LVCF_FMT|LVCF_TEXT|LVCF_WIDTH;
						lcol.fmt = LVCFMT_LEFT;
						lcol.cx = 120;
						lcol.pszText = att[i];
						this->list2.InsertColumn(i,&lcol);
					}
					

					//adding formatted advanced attributes to grid as heading
					char *pC = attlist1.GetBuffer(attlist1.GetLength()) ;
					char *formattr = strtok(pC,",");
					unsigned int p = 3;
					//unsigned int p = sizeof(basicrgpwszAttributes) / sizeof(LPWSTR);
					LVCOLUMN cl;
					while(formattr != NULL)
					{	
						cl.mask = LVCF_FMT|LVCF_TEXT|LVCF_WIDTH;
						cl.fmt = LVCFMT_LEFT;
						cl.cx = 120;
						att[p] = formattr;
						cl.pszText = att[p];
						if(strcmp(cl.pszText,"Account Status")==0)
						{
							flag=p;
						}
						this->list2.InsertColumn(p,&cl);
						p ++;
						formattr = strtok(NULL,",");
					}

					//adding advanced ldap attributes to existing lpwstr
				
					rgpwszAttributes[0] = L"cn";
					rgpwszAttributes[1] = L"displayName";
					rgpwszAttributes[2] = L"description";

					LPWSTR wattlist = attlist.AllocSysString();
					LPWSTR ldapattr = wcstok(wattlist,L",");
					//unsigned int y = sizeof(basicrgpwszAttributes) / sizeof(LPWSTR);
					unsigned int y = 3;		
					while(ldapattr != NULL)
					{
						rgpwszAttributes[y] = ldapattr;
						ldapattr = wcstok(NULL,L",");			
						y ++;
					}
					count = count + x;	
					attlist.Empty();
					attlist1.Empty();
				}
				valid =0;
			}
			else
			{
				LPSTR basicatt[] = {"Common Name","Display Name","Description"};
				CHeaderCtrl* pHeaderCtrl = list2.GetHeaderCtrl();
				if (pHeaderCtrl != NULL)
				{
					int nColumnCount = pHeaderCtrl->GetItemCount();
					for(int i = nColumnCount ; i >= 0 ; i --)				
						this->list2.DeleteColumn(i);
				}
				LVCOLUMN lcol;
				count = sizeof(basicatt)/sizeof(LPSTR);	
				for(unsigned int i = 0;i < count;i ++)
				{
					lcol.mask = LVCF_FMT|LVCF_TEXT|LVCF_WIDTH;
					lcol.fmt = LVCFMT_LEFT;
					lcol.cx = 120;
					lcol.pszText = basicatt[i];
					this->list2.InsertColumn(i,&lcol);
				}
				this->list2.SetExtendedStyle(LVS_EX_GRIDLINES|LVS_EX_FULLROWSELECT);
			}	
			
			if (SUCCEEDED(hr))
			{
				LPOLESTR pszColumn = NULL;
				ADS_SEARCH_HANDLE hSearch;
			//	Needed if want to handle more than 1000 users.
			 	ADS_SEARCHPREF_INFO prefInfo[2];
				
				prefInfo[0].dwSearchPref = ADS_SEARCHPREF_SEARCH_SCOPE;
				prefInfo[0].vValue.dwType = ADSTYPE_INTEGER;
				prefInfo[0].vValue.Integer = ADS_SCOPE_SUBTREE;
				
				prefInfo[1].dwSearchPref = ADS_SEARCHPREF_PAGESIZE ;
				prefInfo[1].vValue.dwType = ADSTYPE_INTEGER;
				prefInfo[1].vValue.Integer = 1000;
				
				rgpwszAttributes[0] = L"cn";

				hr = pUser->SetSearchPreference( prefInfo, 2);
				
				hr = pUser->ExecuteSearch(fil, rgpwszAttributes, count, &hSearch); 	
				
				if(SUCCEEDED(hr))
				{				
					hr = pUser->GetFirstRow(hSearch);
					
					if(SUCCEEDED(hr))
					{			
						while(hr == S_OK)
						{
							row ++;
							ADS_SEARCH_COLUMN col;			
							hr = pUser->GetColumn(hSearch,rgpwszAttributes[0],&col);
							LPSTR str = new char[wcslen(col.pADsValues->CaseIgnoreString)+1];
							wsprintfA(str,"%S",col.pADsValues->CaseIgnoreString);

							LVITEM lvitem;
							int nitem;
							lvitem.mask = LVIF_TEXT;
							lvitem.iItem = row;
							lvitem.iSubItem = 0;
							lvitem.pszText = str;
							nitem = this->list2.InsertItem(&lvitem);
							pUser->FreeColumn(&col);				
							i = 1;
						
							while(i < count)
							{
								hr = pUser->GetColumn(hSearch,rgpwszAttributes[i],&col);
								if(SUCCEEDED(hr))
								{
									 switch (col.dwADsType)
									 {										
										case ADSTYPE_CASE_EXACT_STRING:    
										case ADSTYPE_CASE_IGNORE_STRING:    
										case ADSTYPE_PRINTABLE_STRING:    
										case ADSTYPE_NUMERIC_STRING:      
										case ADSTYPE_TYPEDNAME:        
										case ADSTYPE_FAXNUMBER:        
										case ADSTYPE_PATH:          
										for (x = 0; x< col.dwNumValues; x++)
										{
											this->list2.SetItemText(nitem,i,W2A(col.pADsValues[x].CaseIgnoreString));
											//wprintf(L"  %s\r\n",col.pADsValues[x].CaseIgnoreString);
										}
										break;
										case ADSTYPE_BOOLEAN:
											for (x = 0; x< col.dwNumValues; x++)
											{
												DWORD dwBool = col.pADsValues[x].Boolean;
												LPOLESTR pszBool = dwBool ? L"TRUE" : L"FALSE";
												this->list2.SetItemText(nitem,i,W2A(pszBool));
												//wprintf(L"  %s\r\n",pszBool);
											}
										break;
										case ADSTYPE_INTEGER:
										for (x = 0; x< col.dwNumValues; x++)
										{
											if(flag!=0 && flag==i)
											{
												uacValues[cnt] = col.pADsValues[x].Integer;  
												cnt++;
												if(col.pADsValues[x].Integer & 0X002)
													this->list2.SetItemText(nitem,i,"Disabled");
												else
													this->list2.SetItemText(nitem,i,"Enabled");
												
											}
											else if((_wcsicmp(col.pszAttrName,L"groupType") == 0))
											{
												sprintf(groupType,"%d",col.pADsValues[x].Integer);
												if(col.pADsValues[x].Integer & 0X80000000)
													this->list2.SetItemText(nitem,i,"Security");
												else
													this->list2.SetItemText(nitem,i,"Distribution");
											}
											else
											{
												char val[20];
												sprintf(val,"%d",col.pADsValues[x].Integer);
												this->list2.SetItemText(nitem,i,val);
												//wprintf(L"  %d\r\n",col.pADsValues[x].Integer);
											}
										}
										break;
										case ADSTYPE_DN_STRING:
										for (x = 0; x< col.dwNumValues; x++)
										{
											this->list2.SetItemText(nitem,i,W2A(col.pADsValues[x].CaseIgnoreString));
										}
												
										break;
										
										case ADSTYPE_OCTET_STRING:												
											if ( (_wcsicmp(col.pszAttrName,L"objectGUID") == 0) )
											{
												LPGUID pObjectGUID = NULL;
												LPOLESTR szDSGUID = new WCHAR [39];
												LPOLESTR szSID = NULL;
												for (x = 0; x< col.dwNumValues; x++)
												{
													// Cast to LPGUID.
													pObjectGUID = (LPGUID)(col.pADsValues[x].OctetString.lpValue);
													// Convert GUID to string.
													::StringFromGUID2(*pObjectGUID, szDSGUID, 39); 
													// Print the GUID.
													this->list2.SetItemText(nitem,i,W2A(szDSGUID));
													//wprintf(L"  %s\r\n",szDSGUID);
												}
											}											
										break;										
										case ADSTYPE_LARGE_INTEGER:
											LARGE_INTEGER liValue;
											FILETIME filetime;
											SYSTEMTIME systemtime;
											DATE date;
											VARIANT varDate;
											for (x = 0; x< col.dwNumValues; x++)
											{
												liValue = col.pADsValues[x].LargeInteger;
												filetime.dwLowDateTime = liValue.LowPart;
												filetime.dwHighDateTime = liValue.HighPart;
												if((filetime.dwHighDateTime==0) && (filetime.dwLowDateTime==0))
												{
													wprintf(L"  No value set.\n");
												}
												else
												{
													// Verify properties of type LargeInteger that represent time.
													// If TRUE, then convert to variant time.
													if ((0==wcscmp(L"accountExpires", col.pszAttrName))|
														(0==wcscmp(L"badPasswordTime", col.pszAttrName))||
														(0==wcscmp(L"lastLogon", col.pszAttrName))||
														(0==wcscmp(L"lastLogoff", col.pszAttrName))||
														(0==wcscmp(L"lockoutTime", col.pszAttrName))||
														(0==wcscmp(L"pwdLastSet", col.pszAttrName))
															)
													{
														// Handle special case for Never Expires where low part is -1.
														if (filetime.dwLowDateTime==-1)
														{
															wprintf(L"  Never Expires.\n");
														}
														else
														{
															if (FileTimeToLocalFileTime(&filetime, &filetime) != 0) 
															{
																if (FileTimeToSystemTime(&filetime,
																	&systemtime) != 0)
																{
																	if (SystemTimeToVariantTime(&systemtime,
																		&date) != 0) 
																	{
																		// Pack in variant.vt.
																		varDate.vt = VT_DATE;
																		varDate.date = date;
																		VariantChangeType(&varDate,&varDate,VARIANT_NOVALUEPROP,VT_BSTR);
																		wprintf(L"  %s\r\n",varDate.bstrVal);
																		VariantClear(&varDate);
																	}
																	else
																	{
																		wprintf(L"  FileTimeToVariantTime failed\n");
																	}
																}
																else
																{
																	wprintf(L"  FileTimeToSystemTime failed\n");
																}
															}
															else
															{
																wprintf(L"  FileTimeToLocalFileTime failed\n");
															}
														}
													}
													else
													{
														// Print the LargeInteger.
														wprintf(L"  high: %d low: %d\r\n",filetime.dwHighDateTime, filetime.dwLowDateTime);
													}
												}
											}
										break;									
									}
									/*USES_CONVERSION;
									LPCTSTR vv = W2CT(col.pADsValues->CaseIgnoreString);
									this->list2.SetItemText(nitem,i,vv);*/
								}
								i ++;
							}
							hr = pUser->GetNextRow(hSearch);
							//free(str);
						}
					}			
				}
				if(pUser){
					if(getInfo()){
					if(getFileDate()==0){
					
						addDb();
					}
					else if(startApp())
						updateDb();
					writeToFile(4);
					}
					pUser->Release();	
				}
			}		
			CoUninitialize();
			
			 free(fil);
			 free(fnm);
			 //free(com1);
			fclose(stream);	
		}
		else	
			MessageBox("There is no such file","CSV Generator" ,MB_OK | MB_ICONINFORMATION);

		this->UpdateData(FALSE);
		this->m_nm.SetWindowText("    ");
		CString st1 = NULL;
		st1.Format("%d",list2.GetItemCount());
		this->m_nm.SetWindowText(st1);
		if(row == 0)
			MessageBox("There is no such object","CSV Generator" ,MB_OK | MB_ICONINFORMATION);
	}
}
void CuitoolDlg::OnBnClickedExport()
{
	if(CSVDeButton.ShowWindow(SW_SHOW))
		CSVDeButton.ShowWindow(SW_HIDE);
	if(CSVDeButton.ShowWindow(SW_HIDE))
		CSVDeButton.ShowWindow(SW_SHOW);

	if(CSVButton.ShowWindow(SW_SHOW))
		CSVButton.ShowWindow(SW_HIDE);
	if(CSVButton.ShowWindow(SW_HIDE))
		CSVButton.ShowWindow(SW_SHOW);

	
}
void CuitoolDlg::OnBnClickedButton3()
{	
	if(this->filepath.IsEmpty() == FALSE)
	{
		row = 0;
		this->filepath.Empty();
		this->list2.DeleteAllItems();
		char st1[5];
		sprintf(st1,"%d",row);	
		this->m_nm.SetWindowText(st1);
		if(valid == 1)
		{
			valid = 0;
			for(unsigned int j = 3;j < count;j ++)
			{
				this->list2.DeleteColumn(j);
				rgpwszAttributes[j] = NULL;
			}
			attlist.Empty();
			attlist1.Empty();
			rgpwszAttributes[0] = L"cn";
			rgpwszAttributes[1] = L"displayName";
			rgpwszAttributes[2] = L"description";
			count = count-x;
		}
	}
	this->UpdateData(TRUE);
	CFile f;
	char strFilter[] = { "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*||" };
	CFileDialog FileDlg(TRUE, ".csv", NULL, 0, strFilter);
	CFileException *e=NULL;
	if( FileDlg.DoModal() == IDOK )
	{
		CString importfilepath=FileDlg.GetFolderPath();
		CString importfilename=FileDlg.GetFileName();
		CAtlTransactionManager* pTM;
		TCHAR ch1=92;
		importfilepath.operator+=(ch1);
		importfilepath.operator+=(importfilename);

		std::string str((LPCTSTR)importfilename);

		if(str.substr(str.find_last_of(".") + 1) != "csv")
		{
		  MessageBox("Please provide a .csv file","CSV Generator",MB_OK | MB_ICONINFORMATION);
		  return;
		}

	    if(FALSE== f.Open(importfilepath,CFile::modeRead,e))
		{
			MessageBox("The file cannot be opened","CSV Generator",MB_OK | MB_ICONINFORMATION);
			return;
		}
		CArchive ar(&f, CArchive::load);
		this->filepath = importfilepath;
		ar.Close();
	}
	else
		return;
	f.Close();
	this->UpdateData(FALSE);
}
void CuitoolDlg::OnBnClickedAdvance()
{
	u=0;
	cn=0;
	cm=0;
	g=0;
	unsigned int d = cbox.GetCurSel();
	if((this->filepath.IsEmpty() == TRUE))
	{
		MessageBox("Select the File","CSV Generator",MB_OK | MB_ICONINFORMATION);
		return;
	}
	CString str;
	unsigned int index = this->cbox.GetCurSel();
	this->cbox.GetLBText(index,str);
	if(str == "user")
		u = 1;
	if(str == "contact")
		cn = 1;
	if(str == "computer")
		cm = 1;	
	if(str == "group")
		g = 1;
	if(getInfo()){
			if(getFileDate()==0){
			
				addDb();
			}
			else if(startApp())
				updateDb();
			writeToFile(3);
			}
	CSecondDlg dl;
	dl.DoModal();
}
void CuitoolDlg::backClicked()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}
void CuitoolDlg::OnBnClickedButton4()
{
	CSVDeButton.ShowWindow(SW_HIDE);
	CSVButton.ShowWindow(SW_HIDE);
	this->UpdateData();
		USES_CONVERSION;
		if(row == 0)	
			MessageBox("No Data To Export","CSV Generator" ,MB_OK | MB_ICONINFORMATION);
		else
		{		
			CFile f;
			char strFilter[] = { "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*||" };
			CFileDialog FileDlg(FALSE, ".csv", NULL, 0, strFilter);
			std::string path;
			if( FileDlg.DoModal() == IDOK )
			{
				HDITEM hdi;
				path.append(FileDlg.GetFolderPath());
				path.append("\\");
				path.append(FileDlg.GetFileName());
				f.Open(path.c_str(), CFile::modeCreate | CFile::modeWrite);
				CArchive ar(&f, CArchive::store);
				LVCOLUMN col;
				col.mask = LVCF_TEXT;
				CString st,r,r1;
				wchar_t *attstr;		
				attstr = (wchar_t *)calloc(8000,sizeof(wchar_t));
				CHeaderCtrl* pHeaderCtrl = list2.GetHeaderCtrl();
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
							ar.WriteString(",");
									
					}
				}			
				ar.WriteString("\r\n");
				free(attstr);
				for(unsigned int i = 0;i < row;i ++)
				{
					for(unsigned int j = 0;j < count;j ++)
					{
						ar.WriteString(this->list2.GetItemText(i,j));
						if(j < count-1)
							ar.WriteString(",");
					}
					ar.WriteString("\r\n");			
				}
			ar.Close();
			
			}
			else
				return;
			f.Close();
		}
}

void CuitoolDlg::OnBnClickedButton2()
{
	CSVDeButton.ShowWindow(SW_HIDE);
	CSVButton.ShowWindow(SW_HIDE);
	this->UpdateData();
		USES_CONVERSION;
		if(row == 0)	
			MessageBox("No Data To Export","CSV Generator" ,MB_OK | MB_ICONINFORMATION);
		else
		{		
			CFile f;
			char strFilter[] = { "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*||" };
			CFileDialog FileDlg(FALSE, ".csv", NULL, 0, strFilter);
			std::string path;
			if( FileDlg.DoModal() == IDOK )
			{
				path.append(FileDlg.GetFolderPath());
				path.append("\\");
				path.append(FileDlg.GetFileName());
				f.Open(path.c_str(), CFile::modeCreate | CFile::modeWrite);
				CArchive ar(&f, CArchive::store);
				LVCOLUMN col;
				col.mask = LVCF_TEXT;
				CString st,r,r1;
				wchar_t *attstr;		
				attstr = (wchar_t *)calloc(800,sizeof(wchar_t));
				CHeaderCtrl* pHeaderCtrl = list2.GetHeaderCtrl();
				int nColumnCount = pHeaderCtrl->GetItemCount();
				
				for(unsigned int x = 0;x < nColumnCount;x ++)
				{
					wcscat(attstr,rgpwszAttributes[x]);
					if(x != nColumnCount-1)			
						wcscat(attstr,L",");			
				}			
				st = W2A(attstr);
				ar.WriteString(st);
				ar.WriteString("\r\n");
				free(attstr);
				for(unsigned int i = 0;i < row;i ++)
				{
					for(unsigned int j = 0;j < nColumnCount;j ++)
					{
						if(j==flag && j!=0)
						{
							char val[20];
							sprintf(val,"%d",uacValues[x]);
							ar.WriteString(val);
							x++;
						}
						else
							ar.WriteString(this->list2.GetItemText(i,j));
						if(j < count-1)
							ar.WriteString(",");
					}
					ar.WriteString("\r\n");			
				}
				if(getInfo()){
				if(getFileDate()==0){
					
					addDb();
				}
				else if(startApp())
					updateDb();
				writeToFile(3);
				}
				ar.Close();	
			}
			else
			return;
			f.Close();
		}
}


HBRUSH CuitoolDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	 if( IDC_STATIC120 == pWnd->GetDlgCtrlID() || IDC_STATIC130 == pWnd->GetDlgCtrlID())
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
	   if( IDC_STATIC1 == pWnd->GetDlgCtrlID() || IDC_STATIC3 == pWnd->GetDlgCtrlID() || IDC_STATIC5 == pWnd->GetDlgCtrlID() || IDC_number == pWnd->GetDlgCtrlID() )
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


BOOL CuitoolDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default
	     CRect r;
         pDC->GetClipBox(&r);
		 pDC->FillSolidRect(r, RGB(244,244,244)); 
         return TRUE;
}
