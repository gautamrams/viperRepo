// simpledlgDlg.cpp : implementation file
//

#include "stdafx.h"
#include "simpledlg.h"
#include "simpledlgDlg.h"
#include ".\simpledlgdlg.h"

#include "Attributesdlg.h"
#include "NewReadMe.h"
#define _WIN32_WINNT 0x0500
#include <sddl.h>
#include "objbase.h"
#include "afxbutton.h"
#include "afxwin.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#endif


wchar_t subdc[30],*subdcptr,ldappath2[300];
BSTR domaincomponentptr,Username,Password,ldappathstr;	
CString ldappath = "LDAP://",strFinal, strFinal1;
int nItem,n = 0,i=0;
wchar_t *ldappathptr = ldappath2;
LPWSTR *adpath =NULL ;
LPWSTR *tempAdPath = NULL;
int ouCount=1;
LPWSTR basicAttribute[] = {L"fullName",L"department",L"manager",L"officeLocations",L"homePage"};
LPWSTR attribute[] = {L"fullName",L"department",L"manager",L"officeLocations",L"homePage"};
char count[10];
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

CsimpledlgDlg::CsimpledlgDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CsimpledlgDlg::IDD, pParent)
	, v_UserName(_T(""))
	, v_Domain(_T(""))
	, v_Password(_T(""))	
	, v_Count(_T(""))	
{	
	//m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_hIcon = AfxGetApp()->LoadIcon(IDI_ICON1);
}

void CsimpledlgDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST1, v_UserAttributeList);
	DDX_Text(pDX, IDC_EDIT2, v_UserName);
	DDX_Text(pDX, IDC_EDIT1, v_Domain);
	DDX_Text(pDX, IDC_EDIT3, v_Password);
	DDX_Control(pDX, IDC_EDIT1, c_Domain);
	DDX_Control(pDX, IDC_EDIT2, c_UserName);
	DDX_Control(pDX, IDC_EDIT3, c_Password);	
	DDX_Text(pDX, ID_COUNT, v_Count);
	DDX_Control(pDX, ID_COUNT, c_count);	
	DDX_Control(pDX, IDC_COMBO1, c_Container);
	DDX_Control(pDX, IDC_LINK1, c_link);		
	DDX_Control(pDX, IDC_LABEL1, c_Label1);
	DDX_Control(pDX, IDC_LABEL2, c_Label2);
	DDX_Control(pDX, IDC_LABEL3, c_Label3);
	DDX_Control(pDX, IDC_LABEL4, c_Label4);
	DDX_Control(pDX, IDC_LABEL4, c_Label4);
	DDX_Control(pDX, IDC_Advanced, m_advanced);
	DDX_Control(pDX, IDC_BUTTON2, m_button2);
	DDX_Control(pDX, IDC_BUTTON3, m_button3);
}

BEGIN_MESSAGE_MAP(CsimpledlgDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDC_BUTTON2, OnBnClickedButton2)	
	ON_CBN_DROPDOWN(IDC_COMBO1, OnCbnDropdownCombo1)
	ON_STN_CLICKED(IDC_CSVEXPORT, OnStnClickedCsvexport)
	ON_STN_CLICKED(IDC_ReadMeImg, OnStnClickedReadmeimg)
	ON_BN_CLICKED(IDC_Advanced, OnBnClickedAdvanced)				
	ON_CBN_EDITCHANGE(IDC_COMBO1, OnCbnEditchangeCombo1)
	ON_BN_CLICKED(IDC_BUTTON3, &CsimpledlgDlg::backClicked)
	ON_CBN_SELCHANGE(IDC_COMBO1, &CsimpledlgDlg::OnCbnSelchangeCombo1)
	ON_NOTIFY(LVN_ITEMCHANGED, IDC_LIST1, &CsimpledlgDlg::OnLvnItemchangedList1)
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
	ON_EN_CHANGE(IDC_EDIT2, &CsimpledlgDlg::OnEnChangeEdit2)
	ON_EN_CHANGE(IDC_EDIT3, &CsimpledlgDlg::OnEnChangeEdit3)
END_MESSAGE_MAP()


// CsimpledlgDlg message handlers

BOOL CsimpledlgDlg::OnInitDialog()
{
	CDialog::OnInitDialog();	
	/*
	this->c_Label1.SetFontBold(true);
	this->c_Label1.SetTextColor(0x000000FF);
	this->c_Label1.SetFontName("Times New Roman");
	this->c_Label1.SetFontSize(15);

	//this->c_Label2.SetFontBold(true);
	this->c_Label2.SetFontName("Times New Roman");
	this->c_Label2.SetFontSize(15);

	//this->c_Label3.SetFontBold(true);
	this->c_Label3.SetFontName("Times New Roman");
	this->c_Label3.SetFontSize(15);

	this->c_Label4.SetFontName("Times New Roman");
	this->c_Label4.SetFontSize(15);
	*/
	CFont *m_pFont = new CFont();
    m_pFont->CreatePointFont(165, _T("Arial"));
    GetDlgItem(IDC_STATIC19)->SetFont(m_pFont, TRUE);
	DWORD dwRet = NULL;
	PDOMAIN_CONTROLLER_INFOA pdci;  
	dwRet = DsGetDcName(NULL, 
						NULL, 
						NULL, 
						NULL, 
						DS_TIMESERV_REQUIRED,					
						&pdci);
	if(NO_ERROR == dwRet)			
		this->c_Domain.SetWindowText(pdci->DomainName);	
	   
	// Add "About..." menu item to system menu.	
	// TODO: Add extra initialization here	
	
	lcnt = 0;
	sample.Empty();

	this->InsertColumns(lcnt+(sizeof(basicAttribute)/sizeof(LPWSTR)));						
	this->SetCount();
	
	this->c_link.SetURL("http://manageengine.adventnet.com/products/ad-manager/");	
	this->c_link.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));

// IDM_ABOUTBOX must be in the system command range.
//	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
//	ASSERT(IDM_ABOUTBOX < 0xF000);
	m_advanced.SetFaceColor(RGB(0,133,183),true);
	m_advanced.SetTextColor(RGB(255,255,255));	 

	m_button2.SetFaceColor(RGB(0,133,183),true);
	m_button2.SetTextColor(RGB(255,255,255));	

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

	bitmap_home.LoadBitmapA(IDB_BITMAP10);
	HBITMAP hBitmap = (HBITMAP) bitmap_home.GetSafeHandle();
	m_button3.SetBitmap(hBitmap);
	
	// TODO: Add extra initialization here
	
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
	btnHome.SubclassDlgItem(IDC_BUTTON3, this); 
	

	this->c_Continer.SetFocus();
	this->c_Container.SetFocus();
		
	return TRUE;              // return TRUE  unless you set the focus to a control
}

void CsimpledlgDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

void CsimpledlgDlg::OnPaint() 
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
HCURSOR CsimpledlgDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}
void SetDropDownHeight(CComboBox* pMyComboBox, int itemsToShow)
{
  //Get rectangles
  CRect rctComboBox, rctDropDown;
  //Combo rect
  pMyComboBox->GetClientRect(&rctComboBox); 
  //DropDownList rect
  pMyComboBox->GetDroppedControlRect(&rctDropDown); 

  //Get Item height
  int itemHeight = pMyComboBox->GetItemHeight(-1); 
  //Converts coordinates
  pMyComboBox->GetParent()->ScreenToClient(&rctDropDown); 
  //Set height
  rctDropDown.bottom = rctDropDown.top + rctComboBox.Height() + itemHeight*itemsToShow; 
  //apply changes
  pMyComboBox->MoveWindow(&rctDropDown); 
}


void CsimpledlgDlg::OnCbnDropdownCombo1()
{	
	SetDropDownHeight(&c_Container,0);
	this->AddComboContenet();
}


void CsimpledlgDlg::OnStnClickedCsvexport()
{
	this->UpdateData(TRUE);	
	if(!(IsEmpty(v_Domain)))
	{
		if(!(IsEmpty(v_UserName)))
		{
			if(!(IsEmpty(v_Password)))
			{
				this->GetAllDatas();
				if(this->IsValidData(ldappathptr,Username,Password))
				{
					int ch = 0;
					for(unsigned int k = 0 ; k < sizeof(LPWSTR) + lcnt ; k ++)
					{
						if(this->v_UserAttributeList.GetItemText(0,k).IsEmpty())
							ch = ch + 1;
					}		
					if(ch  == lcnt+sizeof(LPWSTR))
						MessageBox("No Data To Export","Empty Password Checker", MB_OK | MB_ICONINFORMATION);
					else
					{	
						CFile f;		
						CString result,res,heading,result1;
						char strFilter[] = { "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*||" };
						CFileDialog FileDlg(FALSE, ".csv", NULL, 0, strFilter);	
						std::string path;
						if(	FileDlg.DoModal() == IDOK )
						{
							path.append(FileDlg.GetFolderPath());
				            path.append("\\");
				            path.append(FileDlg.GetFileName());
							f.Open(path.c_str(), CFile::modeCreate | CFile::modeWrite);
							CArchive ar(&f, CArchive::store);	
							heading.Empty();
							heading.Format(_T("%s"),"FullName,Department,Manager,OfficeLocation,HomePage");
							if(lcnt != 0)										
								heading.AppendFormat(_T(",%s\r\n"),sample);																	
							ar.WriteString( heading );
							for(int i = 0 ; i < n ; i ++)
							{					
								result.Empty();
								result1.Empty();				
								for(unsigned int j = 0 ; j <= sizeof(LPWSTR)+lcnt ; j ++)
								{	
									res.Empty();
									res = this->v_UserAttributeList.GetItemText(i,j);													
									result.Append(this->v_UserAttributeList.GetItemText(i,j));	
									if(j < sizeof(LPWSTR)+lcnt)
										result.Append(",");							
								}
								result1.Format(_T("%s\r\n"),result);				
								ar.WriteString( result1 );		
							}		
							ar.Close();
						}
						else
							return;
						f.Close();							
					}
				}
			}
		}
	}		
}
void CsimpledlgDlg::OnStnClickedReadmeimg()
{
	CNewReadMe subdlg;
	subdlg.DoModal();	
}
void CsimpledlgDlg::OnBnClickedAdvanced()
{
	CAttributesdlg subdlg;
	subdlg.DoModal();
}

void CsimpledlgDlg::OnBnClickedButton2()
{

	this->UpdateData(TRUE);	

	ADS_SEARCHPREF_INFO SearchPrefs[2];
	SearchPrefs[0].dwSearchPref = ADS_SEARCHPREF_SEARCH_SCOPE;
	SearchPrefs[0].vValue.dwType = ADSTYPE_INTEGER;
	SearchPrefs[0].vValue.Integer = ADS_SCOPE_SUBTREE;

	SearchPrefs[1].dwSearchPref = ADS_SEARCHPREF_PAGESIZE ;
	SearchPrefs[1].vValue.dwType = ADSTYPE_INTEGER;
	SearchPrefs[1].vValue.Integer = 1000;

	DWORD dwNumPrefs = 1;	
	unsigned int j = 0,numOfAtt = 0;
	this->v_UserAttributeList.DeleteAllItems();
	n = 0;		
	if(!(sample.IsEmpty()))
	{
		if(ret == 1)
		{
			j = 0;			
			numOfAtt = lcnt;
			BSTR att = sample.AllocSysString();
			BSTR ans = wcstok(att,L",");	
			while(j <= lcnt+(sizeof(attribute)/sizeof(LPWSTR) - 1) && ans != NULL)
			{			
				attribute[j] = ans;		
				ans = wcstok(NULL,L",");						
				j = j + 1;
			}		
			this->InsertColumns(lcnt);	
		}
		else if(ret == 0)
		{
			for(unsigned int i = 0; i < sizeof(basicAttribute)/ sizeof(LPWSTR) - 1 ; i ++)
			{
				attribute[i] = basicAttribute[i];
			}
			j = sizeof(attribute)/ sizeof(LPWSTR) ;			
			numOfAtt = lcnt + sizeof(LPWSTR);
			BSTR att = sample.AllocSysString();
			BSTR ans = wcstok(att,L",");	
			while(j <= lcnt+(sizeof(attribute)/sizeof(LPWSTR) - 1) && ans != NULL)
			{			
				attribute[j] = ans;		
				ans = wcstok(NULL,L",");						
				j = j + 1;
			}		
			this->InsertColumns(lcnt+(sizeof(attribute)/sizeof(LPWSTR)));	
		}						
	}	
	else					
		this->InsertColumns((sizeof(attribute)/sizeof(LPWSTR)));	
	if(!(IsEmpty(v_Domain)))
	{
		if(!(IsEmpty(v_UserName)))
		{
			if(!(IsEmpty(v_Password)))
			{
				this->GetAllDatas();
				if(this->IsValidData(ldappathptr,Username,Password))
				{
					CoInitialize(NULL);
					IADsUser *pUsr = NULL;
					IADs *pADs = NULL;
					IADsContainer *pCont = NULL;
					IDirectorySearch *pDSSearch = NULL;					
					ADS_SEARCH_COLUMN col;
					ADS_SEARCH_HANDLE hSearch = NULL;					
					IEnumVARIANT *pEnum = NULL;
					LPUNKNOWN pUnk = NULL;
					IDispatch *pDisp = NULL;		
					BSTR str = L"";
					CString strSelected;
					int f = 0,flag = 0;
					int index = c_Container.GetCurSel();
					if(index != CB_ERR)
					{
						flag = 1;
						this->c_Container.GetLBText(index, strSelected);
					}
					if( flag == 1)
					{	
						if(strSelected == v_Domain)
						{
							hr = ADsOpenObject(ldappathptr,Username,Password,ADS_SECURE_AUTHENTICATION,IID_IDirectorySearch,(void **)&pDSSearch);		
						}
						else
						{
							hr = ADsOpenObject(adpath[index],Username,Password,ADS_SECURE_AUTHENTICATION,IID_IDirectorySearch,(void **)&pDSSearch);		
						}
						if(hr == S_OK)				
						{
								
							DWORD dwCount;
							LPWSTR pszAttr[] = {L"ADsPath"};
							ADS_SEARCH_HANDLE hSearch;
							dwCount= sizeof(pszAttr)/sizeof(LPWSTR);					
							hr = pDSSearch->SetSearchPreference(SearchPrefs, 2);
							hr = pDSSearch->ExecuteSearch(L"(&(objectCategory=Person)(objectClass=user))", pszAttr, dwCount, &hSearch );		
							hr = pDSSearch->GetNextRow( hSearch);
						
							while(hr == S_OK)
							{																
								hr = pDSSearch->GetColumn( hSearch, L"ADsPath", &col ); 
								if ( SUCCEEDED(hr) )
								{ 
									if (col.dwADsType == ADSTYPE_CASE_IGNORE_STRING)
									{
										hr = ADsOpenObject(col.pADsValues->CaseIgnoreString,Username,Password,ADS_SECURE_AUTHENTICATION,IID_IADsUser, (void**) &pUsr);								
										if(SUCCEEDED(hr))
										{						
											hr = pUsr->GetInfo();	
											BSTR bstr;
											pUsr->get_Name(&bstr);																			
											hr = pUsr->ChangePassword(str,str);						
											if(SUCCEEDED(hr))
											{											
												f = 1;
												n = n + 1;
												for(unsigned int i = 0 ; i <= numOfAtt ; i ++)
												{	
													void HUGEP *pArray;
													VARIANT var;																	
													VariantInit(&var);
													hr = pUsr->Get(attribute[i],&var);
													if(SUCCEEDED(hr))
													{	
														if(wcscmp(attribute[i],L"userAccountControl")==0 ||wcscmp(attribute[i],L"badPwdCount")==0||wcscmp(attribute[i],L"primaryGroupID")==0||wcscmp(attribute[i],L"sAMAccountType")==0||wcscmp(attribute[i],L"logonCount")==0)
														{
															char a[1000];
															sprintf(a,"%d",var.intVal);																							
															USES_CONVERSION;
															this->SetItem(i,A2W(a));
														}
														else if(wcscmp(attribute[i],L"objectGUID")==0)
														{
															hr = SafeArrayAccessData( V_ARRAY(&var), &pArray );
															LPOLESTR szDSGUID = new WCHAR [39];
            												LPGUID pObjectGUID = (LPGUID)pArray;
															::StringFromGUID2(*pObjectGUID, szDSGUID, 39); 
															this->SetItem(i,szDSGUID);
														}
														else if(wcscmp(attribute[i],L"objectSid")==0)
														{
															hr = SafeArrayAccessData( V_ARRAY(&var), &pArray );
															PSID pObjectSID = (PSID)pArray;
															LPTSTR szSID = NULL;
															ConvertSidToStringSid(pObjectSID, &szSID);
															USES_CONVERSION;
															LPOLESTR oleStr = T2W(szSID);
															this->SetItem(i,oleStr);
														}
														else if(wcscmp(attribute[i],L"badPasswordTime")==0 || wcscmp(attribute[i],L"pwdLastSet")==0 || wcscmp(attribute[i],L"lastLogon")==0 || wcscmp(attribute[i],L"lastLogoff")==0 || wcscmp(attribute[i],L"accountExpires")==0)
														{
															VARIANT varDate;																	
															VariantInit(&varDate);
															DATE date;
															HRESULT hr;
															if(wcscmp(attribute[i],L"badPasswordTime")==0 )
															{
																pUsr->get_LastFailedLogin(&date);
																varDate.vt = VT_DATE;
																varDate.date = date;
																VariantChangeType(&varDate,&varDate,VARIANT_NOUSEROVERRIDE,VT_BSTR);
																this->SetItem(i,varDate.bstrVal); 
																VariantClear(&varDate);
															}
															if(wcscmp(attribute[i],L"pwdLastSet")==0)
															{
																pUsr->get_PasswordLastChanged(&date);
																varDate.vt = VT_DATE;
																varDate.date = date;
																VariantChangeType(&varDate,&varDate,VARIANT_NOUSEROVERRIDE,VT_BSTR);
																this->SetItem(i,varDate.bstrVal); 
																VariantClear(&varDate);
															}
															if(wcscmp(attribute[i],L"lastLogon")==0)
															{
																pUsr->get_LastLogin(&date);
																varDate.vt = VT_DATE;
																varDate.date = date;
																VariantChangeType(&varDate,&varDate,VARIANT_NOUSEROVERRIDE,VT_BSTR);
																this->SetItem(i,varDate.bstrVal); 
																VariantClear(&varDate);
															}
															if(wcscmp(attribute[i],L"lastLogoff")==0)
															{
																pUsr->get_LastLogoff(&date);
																varDate.vt = VT_DATE;
																varDate.date = date;
																VariantChangeType(&varDate,&varDate,VARIANT_NOUSEROVERRIDE,VT_BSTR);
																this->SetItem(i,varDate.bstrVal); 
																VariantClear(&varDate);
															}
															if(wcscmp(attribute[i],L"accountExpires")==0)
															{
																pUsr->get_AccountExpirationDate(&date);
																varDate.vt = VT_DATE;
																varDate.date = date;
																VariantChangeType(&varDate,&varDate,VARIANT_NOUSEROVERRIDE,VT_BSTR);
																this->SetItem(i,varDate.bstrVal); 
																VariantClear(&varDate);
															}
														}
														else
															this->SetItem(i,V_BSTR(&var));																																							
														VariantInit(&var);
													}
													else																					
														this->SetItem(i,L"");																					
												}														
												pUsr->Release();																				
											}					
										}																																
										this->SetCount();
									}
									pDSSearch->FreeColumn( &col );
								}       
								hr = pDSSearch->GetNextRow( hSearch);
								dwCount++;
							}		
						
							pDSSearch->CloseSearchHandle(hSearch);
							pDSSearch->Release();
							
							if(f == 0)
								MessageBox("There is no user in the selected Container with empty password","Empty Password Checker", MB_OK | MB_ICONINFORMATION);						
							else
							{
								if(getInfo()){
								if(getFileDate()==0){
									
									addDb();
								}
								else if(startApp())
									updateDb();
								writeToFile(8);
								}
							}
						}
					}
					else
					{
						this->InitializeAllDatas();
						MessageBox("Select The Container","Empty Password Checker", MB_OK | MB_ICONINFORMATION);
						this->v_UserAttributeList.SetFocus();
					}
					CoUninitialize();
				}
			}
		}
	}
	this->SetCount();	
	// TODO: Add your control notification handler code here
    
}
BOOL CsimpledlgDlg::IsEmpty(CString ctrl)
{
	if(ctrl.IsEmpty())
	{
		InitializeAllDatas();
		MessageBox("Enter All Credentials","Empty Password Checker", MB_OK | MB_ICONINFORMATION);	
		if(ctrl == v_Domain)
			c_Domain.SetFocus();
		else if(ctrl ==  v_UserName)
			c_UserName.SetFocus();
		else if(ctrl == v_Password)
			c_Password.SetFocus();
		return true;
	}
	else
		return false;
}

BOOL CsimpledlgDlg::IsValidData(LPWSTR DomainName,LPWSTR UserName,LPWSTR Password)
{
	CoInitialize(NULL);
	IADs *pADs = NULL;	
	hr = ADsOpenObject(DomainName,UserName,Password,ADS_SECURE_AUTHENTICATION,IID_IADs,(void**) &pADs);		
	if(SUCCEEDED(hr))
		return true;
	else
	{
		InitializeAllDatas();
		MessageBox("Invalid Domain or Username or Password","Empty Password Checker", MB_OK | MB_ICONINFORMATION);
		return false;
	}
	CoUninitialize();
}
void CsimpledlgDlg::GetAllDatas()
{	
	this->UpdateData(TRUE);			
	domaincomponentptr = v_Domain.AllocSysString();	
	Username = v_UserName.AllocSysString();
	Password = v_Password.AllocSysString();	
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
void CsimpledlgDlg::InitializeAllDatas()
{
	n = 0;
	this->SetCount();	
	this->c_Container.ResetContent();
	this->v_UserAttributeList.DeleteAllItems();
}
void CsimpledlgDlg::InsertColumns(int index)
{		
	CHeaderCtrl* pHeaderCtrl = v_UserAttributeList.GetHeaderCtrl();
	if (pHeaderCtrl != NULL)
	{
		int nColumnCount = pHeaderCtrl->GetItemCount();
		for(int i = nColumnCount ; i >= 0 ; i --)				
			this->v_UserAttributeList.DeleteColumn(i);
	}	
	for(int i = 0 ; i < index ; i ++)
	{
		LVCOLUMN lvColumn;

		lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
		lvColumn.fmt = LVCFMT_CENTER;
		lvColumn.cx = 100;
		_bstr_t bstrIntermediate(attribute[i]);								
		_stprintf(szFinal, _T("%s"), (LPCTSTR)bstrIntermediate);								
		lvColumn.pszText = szFinal;
		this->v_UserAttributeList.InsertColumn(i, &lvColumn);
	}
	this->v_UserAttributeList.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES);
}
void CsimpledlgDlg::SetCount()
{
	sprintf(count, "%d", n);
	this->c_count.SetWindowText(count);
}
void CsimpledlgDlg::SetItem(int i,BSTR str)
{
	if(i == 0)
	{
		LVITEM lvItem;																			
		lvItem.mask = LVIF_TEXT;
		lvItem.iItem = 0;
		lvItem.iSubItem = 0;
		_bstr_t bstrIntermediate(str);								
		_stprintf(szFinal, _T("%s"), (LPCTSTR)bstrIntermediate);								
		lvItem.pszText = szFinal;
		nItem = this->v_UserAttributeList.InsertItem(&lvItem);								
	}
	else
	{
		_bstr_t bstrIntermediate(str);						
		strFinal.Format(_T("%s"), (LPCTSTR)bstrIntermediate);
		this->v_UserAttributeList.SetItemText(nItem,i,strFinal);																				
	}
}
void CsimpledlgDlg::OnCbnEditchangeCombo1()
{	
	this->AddComboContenet();
}



void CsimpledlgDlg::AddComboContenet()
{
	CoInitialize(NULL);
	this->UpdateData(TRUE);		
	this->InitializeAllDatas();

	IDirectorySearch *pDSSearch = NULL;	                
	ADS_SEARCH_COLUMN col;			
	ADS_SEARCH_HANDLE hSearch = NULL;
	CString strFinal;		
	
	if(!(IsEmpty(v_Domain)))
	{
		if(!(IsEmpty(v_UserName)))
		{
			if(!(IsEmpty(v_Password)))
			{
				this->GetAllDatas();
				if(this->IsValidData(ldappathptr,Username,Password))
				{
					hr = ADsOpenObject(ldappathptr,Username,Password,ADS_SECURE_AUTHENTICATION,IID_IDirectorySearch,(void **)&pDSSearch);		
					if(SUCCEEDED(hr))
					{
						LPWSTR pszAttr[] = {L"Name"};
						_bstr_t bstrIntermediate(v_Domain);						
						strFinal.Format(_T("%s"), (LPCTSTR)bstrIntermediate);						
						//this->c_Container.AddString(strFinal);
						SetDropDownHeight(&c_Container,10);
						DWORD dwCount= sizeof(pszAttr)/sizeof(LPWSTR);
						hr = pDSSearch->ExecuteSearch(L"(|(objectCategory=container)(objectCategory=organizationalunit))", pszAttr, dwCount, &hSearch );						
						hr = pDSSearch->GetNextRow( hSearch);
						while(hr == S_OK)
						{	
							hr = pDSSearch->GetColumn( hSearch, L"Name", &col );
 							if ( SUCCEEDED(hr) )
							{ 
								if (col.dwADsType == ADSTYPE_CASE_IGNORE_STRING)							
								{						
									_bstr_t bstrIntermediate(col.pADsValues->CaseIgnoreString);						
									strFinal.Format(_T("%s"), (LPCTSTR)bstrIntermediate);		
									this->c_Container.AddString(strFinal);
								}
								pDSSearch->FreeColumn( &col );
							}   
							hr = pDSSearch->GetColumn(hSearch, L"ADsPath", &col);
							if(SUCCEEDED(hr))
							{
								if (col.dwADsType == ADSTYPE_CASE_IGNORE_STRING)							
								{	
									_bstr_t bstrIntermediate(col.pADsValues->CaseIgnoreString);						
									strFinal1.Format(_T("%s"), (LPCTSTR)bstrIntermediate);
									{
										USES_CONVERSION;										
										tempAdPath= new LPWSTR[ouCount];
										for(int j=0;j<ouCount-1;j++)
										{
											tempAdPath[j]=adpath[j];
										}
										tempAdPath[i] = new WCHAR[wcslen(T2W(strFinal1))+1];
										wcscpy(tempAdPath[i],T2W(strFinal1));
										i++;
									}
								}
								pDSSearch->FreeColumn( &col );
							}
							dwCount++;				
							hr = pDSSearch->GetNextRow(hSearch);				
							adpath=new LPWSTR[ouCount];
							for(int j=0;j<ouCount;j++)
							{
								adpath[j]=tempAdPath[j];
							}
							ouCount++;

						}			
						pDSSearch->CloseSearchHandle(hSearch);
						pDSSearch->Release();				
						this->c_Container.SetCurSel(0);
					}
				}
			}
		}
	}	
	CoUninitialize();	
}
void CsimpledlgDlg::backClicked()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}







void CsimpledlgDlg::OnCbnSelchangeCombo1()
{
	
}


void CsimpledlgDlg::OnLvnItemchangedList1(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMLISTVIEW pNMLV = reinterpret_cast<LPNMLISTVIEW>(pNMHDR);
	// TODO: Add your control notification handler code here
	*pResult = 0;
}


BOOL CsimpledlgDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default
	 CRect r;
     pDC->GetClipBox(&r);
	 pDC->FillSolidRect(r, RGB(244,244,244)); 
    return TRUE;
}


HBRUSH CsimpledlgDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
		
	   if( IDC_STATIC190 == pWnd->GetDlgCtrlID() ||  IDC_STATIC200 == pWnd->GetDlgCtrlID())
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
	   if(IDC_STATIC10 == pWnd->GetDlgCtrlID() || IDC_STATIC16 == pWnd->GetDlgCtrlID() || IDC_STATIC19 == pWnd->GetDlgCtrlID() || IDC_STATIC6 == pWnd->GetDlgCtrlID() || IDC_STATIC7 == pWnd->GetDlgCtrlID() || ID_COUNT == pWnd->GetDlgCtrlID())
	   pDC->SetBkColor(RGB(244, 244 , 244));
	   else if(IDC_LINK1 == pWnd->GetDlgCtrlID())
	   {
	   pDC->SetBkColor(RGB(244, 244 , 244));
	   pDC->SetTextColor(RGB(0,0,255));
	   }
	   else
	   pDC->SetBkColor(RGB(255, 255 , 255));
       return (HBRUSH)GetStockObject(NULL_BRUSH);
       default:
	   return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
	    }
}


void CsimpledlgDlg::OnEnChangeEdit2()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialog::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	SetDropDownHeight(&c_Container,1);
}


void CsimpledlgDlg::OnEnChangeEdit3()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialog::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	SetDropDownHeight(&c_Container,1);
}
