// lastlogonreport.cpp : implementation file
//

#include "stdafx.h"
#include "LastLogonToolMFC.h"
#include "lastlogonreport.h"
#include <ctime>
#ifdef _DEBUG
#define new DEBUG_NEW
#endif
extern int g_IsWindowsXPorLater();

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

//LPWSTR attributeReportList[] = {L"User",L"admp-dc1.admp.com",L"admp-dc2.admp.com"};
HRESULT hr1;
CStringArray distinguishedArray,sAMAccountArray,domainControllerArray,dcColumnHeader,lastLogonInformation;
LPTSTR innerColumnHeard[]={L"User",L"DomainControllers"};
CString lastLogonValue;
TCHAR szFinal2[255];
CString strText;
int ldapAttributeGlobal,timezoneindex;
//LPWSTR dc1[];

IMPLEMENT_DYNAMIC(lastlogonreport, CDialog)
lastlogonreport::lastlogonreport(CWnd* pParent /*=NULL*/)
	: CDialog(lastlogonreport::IDD, pParent)
{
if (!button1.LoadBitmaps(_T("CLOSEU"), _T("CLOSED"), _T("CLOSEF"), _T("CLOSEX")))
	{
		TRACE0("Failed to load bitmaps for buttons\n");
		AfxThrowResourceException();
	}
}

lastlogonreport::~lastlogonreport()
{
	VariantClear(&m_varYear);
    VariantClear(&m_varMonth);
    VariantClear(&m_varDay);
    VariantClear(&m_varHour);
    VariantClear(&m_varMinute);
    VariantClear(&m_varSecond);

}

void lastlogonreport::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST1, v_lastLogonDate);
	DDX_Control(pDX, IDC_P3Label1, c_p3Label3);
	DDX_Control(pDX, IDC_TimezoneResult, c_timezoneresult);
	DDX_Control(pDX, IDC_P3Link3, c_p3link3);
	DDX_Control(pDX, IDC_LISTOF_TIMEZONESNEW2, c_timezoneenablenew);
	DDX_Control(pDX, IDC_LabelTimezone, c_labeltimezone);
}


BEGIN_MESSAGE_MAP(lastlogonreport, CDialog)
	ON_BN_CLICKED(IDC_CLOSE, &lastlogonreport::OnBnClickedClose)
	ON_BN_CLICKED(IDC_BUTTON1, &lastlogonreport::OnBnClickedButton1)
	ON_CBN_SELCHANGE(IDC_LISTOF_TIMEZONESNEW2, &lastlogonreport::OnCbnSelchangeListofTimezonesnew2)
END_MESSAGE_MAP()
BOOL lastlogonreport::OnInitDialog()
{
	CDialog::OnInitDialog();
	
	
	this->c_p3Label3.SetTextColor( custom);
	this->c_p3Label3.SetFontName(L"Arial");
	this->c_p3Label3.SetFontSize(9);

	this->c_p3link3.SetURL(_T("http://manageengine.adventnet.com/products/ad-manager/"));	
	this->c_p3link3.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));

	VERIFY(button1.SubclassDlgItem(IDC_CLOSE, this));
	button1.SizeToContent();

	if(dcColumnHeader.GetCount()<=2)
		this->InsertLastLogonDateColumns(0+dcColumnHeader.GetCount(),198);
	if(dcColumnHeader.GetCount()==3)
	{
		this->InsertLastLogonDateColumns(0+dcColumnHeader.GetCount(),177);
		this->OnSize(SIZE_MAXIMIZED,618,426);
	}
	if(dcColumnHeader.GetCount()>=4)
	{
		this->InsertLastLogonDateColumns(0+dcColumnHeader.GetCount(),157);
		this->OnSize(SIZE_MAXIMIZED,618,426);
	}
	PopulateTimeZones();
	this->LastLogonInformation();
	
	
	m_iLocalToUTC = -1;
	m_iTimeFormat = -1;
	m_iTimeFormat = 1;
	VariantInit(&m_varYear);
    VariantInit(&m_varMonth);
    VariantInit(&m_varDay);
    VariantInit(&m_varHour);
    VariantInit(&m_varMinute);
    VariantInit(&m_varSecond);

	m_varYear.vt = m_varMonth.vt = m_varDay.vt = m_varHour.vt = m_varMinute.vt
				 = m_varSecond.vt = VT_I2;
	
	
	UpdateData(TRUE);
	return TRUE;
}
void lastlogonreport::setColumnHeader(CString dc)
{
	dcColumnHeader.Add(dc);
}
void lastlogonreport::OnSize(UINT nType, int cx, int cy) 
{
      CRect rect;
      int nx, ny;

      GetWindowRect(&rect);
      nx = (cx>=220)?cx:220;
      ny = (cy>=220)?cy:220;
      //if ( (nx!=cx) || (ny!=cy) )
            MoveWindow(rect.left,rect.top,nx,ny);

	CRect l_formRect; 
    GetClientRect(&l_formRect); 

	 CListCtrl* pTreeCtrl;
    pTreeCtrl = (CListCtrl *)GetDlgItem(IDC_LIST1); 
	pTreeCtrl->SetWindowPos(&CWnd::wndBottom, 0, 0, 550, 224,   SWP_NOMOVE);
	pTreeCtrl->RedrawWindow();

	CStatic *pGroupbox;
	pGroupbox=(CStatic*)GetDlgItem(IDC_GroupBox);
	pGroupbox->SetWindowPos(&CWnd::wndBottom, 0, 0, 575, 252,   SWP_NOMOVE);
	pGroupbox->RedrawWindow();

	CButton *timezonebutton;
	timezonebutton=(CButton *)GetDlgItem(IDC_BUTTON1);
	timezonebutton->SetWindowPos(&CWnd::wndBottom, 534, 48, 55, 25,   SWP_DRAWFRAME);
	timezonebutton->RedrawWindow();

	CStatic *pwebaddress;
	pwebaddress=(CStatic*)GetDlgItem(IDC_P3Link3);
	pwebaddress->SetWindowPos(&CWnd::wndBottom, 477, 358, 125, 55,   SWP_DRAWFRAME);
	pwebaddress->RedrawWindow();

	CStatic *pLookforMore;
	pLookforMore=(CStatic*)GetDlgItem(IDC_LookForMore);
	pLookforMore->SetWindowPos(&CWnd::wndBottom, 350, 358, 125, 55,   SWP_DRAWFRAME);
	pLookforMore->RedrawWindow();

	button1.SetWindowPos(&CWnd::wndBottom, 300, 375, 55, 25,   SWP_DRAWFRAME);
	button1.RedrawWindow();

	
}

void lastlogonreport::InsertLastLogonDateColumns(int index,int width)
{	
	
	CDialog::OnInitDialog();
	CHeaderCtrl* pHeaderCtrl = v_lastLogonDate.GetHeaderCtrl();
	if (pHeaderCtrl != NULL)
	{
		int nColumnCount = pHeaderCtrl->GetItemCount();
		for(int i = nColumnCount ; i >= 0 ; i --)				
			this->v_lastLogonDate.DeleteColumn(i);
	}	
	for(int i = 0 ; i < index ; i ++)
	{
		LVCOLUMN lvColumn;
		

		lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
		lvColumn.fmt = LVCFMT_CENTER;
		lvColumn.cx = width;
		//_bstr_t bstrIntermediate(innerColumnHeard[i]);			
		_bstr_t bstrIntermediate(dcColumnHeader.ElementAt(i));			
		_stprintf(szFinal2, _T("%s"), (LPCTSTR)bstrIntermediate);								
		lvColumn.pszText = szFinal2;
		this->v_lastLogonDate.InsertColumn(i, &lvColumn);
		
		
		
	}
	this->v_lastLogonDate.SetExtendedStyle( LVS_EX_GRIDLINES  );
	
}
void lastlogonreport::setDistinguishedNames(CString distinguishedName)
{
	distinguishedArray.Add(distinguishedName);
}
	
void lastlogonreport::setsAMAccountNames(CString sAMAccountName)
{
	sAMAccountArray.Add(sAMAccountName);
}
void lastlogonreport::setDomainControllerNames(CString dcNames)
{
	domainControllerArray.Add(dcNames);
}
int lastlogonreport::AttributeSelection(int ldapAttribute)
{
	ldapAttributeGlobal=ldapAttribute;
	return ldapAttributeGlobal;
}
int lastlogonreport:: PopulateTimeZones()
{
        CLastLogonToolMFCApp* pApp = (CLastLogonToolMFCApp*)AfxGetApp();
        CTimeZoneInfoManager* pTimeZoneInfoManager = (CTimeZoneInfoManager*)(&(pApp->								m_objTimeZoneInfoManager));
		
        CComboBox* pListOfTimeZones = (CComboBox*)GetDlgItem(IDC_LISTOF_TIMEZONESNEW2);

        CString DisplayString;
		int NoOfTimezone=pTimeZoneInfoManager->m_arrRegTimeZoneInfo.GetSize();
        for (int cnt = 0; cnt < pTimeZoneInfoManager->m_arrRegTimeZoneInfo.GetSize(); cnt++ )
        {
            CRegTimeZoneInfo* pRegTimeZoneInfo = pTimeZoneInfoManager->m_arrRegTimeZoneInfo[cnt];
			
            DisplayString.Format(TEXT("%s"), pRegTimeZoneInfo->m_szDisplay);
			pListOfTimeZones->AddString (DisplayString);
			
			
            //pListOfTimeZones->AddString(pTimeZoneInfoManager->m_arrTimeZones[cnt]);
        }
		

       m_iCurrentTimeZoneIndex = pTimeZoneInfoManager->GetCurrentTimeZone();
      pListOfTimeZones->SetCurSel(pTimeZoneInfoManager->GetCurrentTimeZone());


        return 0;
}

void lastlogonreport::LastLogonInformation ()
{
	USES_CONVERSION;
	IDirectoryObject *pDirObject = NULL;
	VARIANT var;
	
	LPOLESTR rootStr = new OLECHAR[MAX_PATH]; 
	LPWSTR lastLogonAttribute[]={L"lastLogon"};
	LPWSTR lastLogonTimestampAttribute[]={L"lastLogonTimestamp"};
	DATE date;
	CoInitialize(NULL);
	
	DWORD dnSize=distinguishedArray.GetCount();
	DWORD dcSize=domainControllerArray.GetCount();
		for(int dnCount=0;dnCount<dnSize;dnCount++)
	{
		v_lastLogonDate.InsertItem( dnCount, _T(""));
		strText.Format(TEXT("%s"),sAMAccountArray.ElementAt(dnCount) );
		//LastLogonLargerInteger[dnCount][0].Add(strText);
		v_lastLogonDate.SetItemText(dnCount, 0, strText);
		for(int dcCount=0;dcCount<dcSize;dcCount++)
		{	
			wcscpy(rootStr,L"LDAP://");
			wcscat(rootStr, domainControllerArray.ElementAt(dcCount));
			wcscat(rootStr,L"/");
			wcscat(rootStr, distinguishedArray.ElementAt(dnCount));
			hr1=ADsOpenObject(rootStr,NULL,NULL,ADS_SECURE_AUTHENTICATION,IID_IDirectoryObject,							(void**)&pDirObject);
			if (SUCCEEDED(hr1)) 
			{			
				if(ldapAttributeGlobal==0)
					lastLogonInformation.Add( FindLastLogonDate(pDirObject,dnCount,dcCount+1,lastLogonAttribute));
				else if(ldapAttributeGlobal==1)
					lastLogonInformation.Add(FindLastLogonDate(pDirObject,dnCount,dcCount+1,lastLogonTimestampAttribute));
			}
			else
			{
			strText.Format(TEXT("%s"),L"Server is not operatioal");
			v_lastLogonDate.SetItemText(dnCount, dcCount+1, strText);
			}
		}
	}
		TIME_ZONE_INFORMATION tzi;
    GetTimeZoneInformation(&tzi);
	this->SetTimezoneResult(tzi.StandardName);
	//int arraySize=lastLogonInformation.GetCount();
	dcColumnHeader.RemoveAll();
	
		
}
void lastlogonreport::SetTimezoneResult(CString zone)
{
	LPOLESTR rootStr = new OLECHAR[MAX_PATH];
	wcscpy(rootStr,L"Search Result for : ");
	wcscat(rootStr, zone );
	this->c_timezoneresult.SetWindowTextW(rootStr);
}
CString  lastlogonreport::FindLastLogonDate(IDirectoryObject *pDirObject,int i,int j,LPWSTR *pAttrNames)
{
	VARIANT var;
	CString strText;
	DATE date;
	FILETIME filetime;
	SYSTEMTIME systemtime;
	
	VARIANT varDate;
	LARGE_INTEGER liValue;
	ADS_ATTR_INFO *pAttrInfo=NULL;
	DWORD dwReturn;
	//LPWSTR pAttrNames[]={L"lastLogon"};
	DWORD dwNumAttr=sizeof(pAttrNames)/sizeof(LPWSTR);
	hr1 = pDirObject->GetObjectAttributes(pAttrNames,dwNumAttr,&pAttrInfo,&dwReturn );
	printf("%x\n",hr1);
	if ( SUCCEEDED(hr1) )//hr==E_ADS_PROPERTY_NOT_FOUND)
	{	
		if(dwNumAttr-1==dwReturn)
		{
			strText.Format(TEXT("%s"),L"-");
			v_lastLogonDate.SetItemText(i, j, strText);
			return strText;;}
		for(DWORD idx = 0; idx < dwReturn; idx++ )
        {
			if (( _wcsicmp(pAttrInfo[idx].pszAttrName,L"lastLogon") == 0) )
			{
				//printf("Adstype is %d\n",pAttrInfo[idx].dwADsType);
				switch (pAttrInfo[idx].dwADsType)
                {
					case ADSTYPE_LARGE_INTEGER:
						liValue = pAttrInfo[idx].pADsValues->LargeInteger;
						//lastLogonValue.Format(TEXT("%s"),liValue);
						filetime.dwLowDateTime = liValue.LowPart;
						filetime.dwHighDateTime = liValue.HighPart;
						if((filetime.dwHighDateTime==0) && (filetime.dwLowDateTime==0))
						{
							strText.Format(TEXT("%s"),L"-");
							v_lastLogonDate.SetItemText(i, j, strText);
							return strText;
							//LastLogonLargerInteger[i][j].Add(strText);
							break;
						}
						if (FileTimeToLocalFileTime(&filetime, &filetime) != 0) 
						{
							if (FileTimeToSystemTime(&filetime,&systemtime) != 0)
							{
								if (SystemTimeToVariantTime(&systemtime,&date) != 0) 
								{
									varDate.vt = VT_DATE;
									varDate.date = date;
									VariantChangeType(&varDate,&varDate,																		VARIANT_NOVALUEPROP,VT_BSTR);
									//wprintf(L"  %s\r\n",varDate.bstrVal);
									strText.Format(TEXT("%s"),varDate.bstrVal );
									v_lastLogonDate.SetItemText(i, j, strText);
									return strText;
									//LastLogonLargerInteger[i][j].Add(strText);
									VariantClear(&varDate);
								}
								else
									wprintf(L"  FileTimeToVariantTime failed\n");
							}
							else
								wprintf(L"  FileTimeToSystemTime failed\n");
						}
						else
                      		wprintf(L"  FileTimeToLocalFileTime failed\n");
									
					break;
				}
			}
			
			
			
		
			else
				printf("Error\n");
		}
		}
	
	
}


void lastlogonreport::OnBnClickedClose()
{
	// TODO: Add your control notification handler code here
	distinguishedArray.RemoveAll();
	sAMAccountArray.RemoveAll();
	domainControllerArray.RemoveAll();
	lastLogonInformation.RemoveAll();
	OnCancel();
	
}
void lastlogonreport::OnBnClickedButton1()
{
	c_timezoneenablenew.ShowWindow(1);
	LPOLESTR rootStr = new OLECHAR[MAX_PATH];
	wcscpy(rootStr,L"Search Timezone ");
	//wcscat(rootStr, zone );
	this->c_labeltimezone.SetWindowTextW(rootStr);
}


void lastlogonreport::OnCbnSelchangeListofTimezonesnew2()
{
	// TODO: Add your control notification handler code here
	v_lastLogonDate.DeleteAllItems();
	COleDateTime dt;
	DATE date;
	VARIANT varDate;
	CComboBox *m_lstTimeZones = (CComboBox*) GetDlgItem(IDC_LISTOF_TIMEZONESNEW2);

	m_iCurrentTimeZoneIndex = m_lstTimeZones-> GetCurSel();

	
	CDateTime universalTime;
	CLastLogonToolMFCApp* pApp = (CLastLogonToolMFCApp*)AfxGetApp();
    CTimeZoneInfoManager* pTimeZoneInfoManager = (CTimeZoneInfoManager*)(&(pApp->m_objTimeZoneInfoManager));
	USES_CONVERSION;
	for(int dnCount=0,LastLogonDate=0;dnCount<distinguishedArray.GetCount();dnCount++)
	{
		
		v_lastLogonDate.InsertItem( dnCount, _T(""));
		strText.Format(TEXT("%s"),sAMAccountArray.ElementAt(dnCount) );
		v_lastLogonDate.SetItemText(dnCount, 0, strText);
		for(int dcCount=0;dcCount<domainControllerArray.GetCount();dcCount++)
			{
				strText.Format(TEXT("%s"),lastLogonInformation.ElementAt(LastLogonDate) );
				int stringLength=strText.GetLength();
				dt.ParseDateTime(strText);
				m_currLocalDateTime.SetDateTime(dt.GetYear(),dt.GetMonth(),dt.GetDayOfWeek(),dt.GetDay(),dt.				GetHour(),dt.GetMinute(),dt.GetSecond());

				pTimeZoneInfoManager->ConvertFromLocalToUTC(&m_currLocalDateTime, m_iCurrentTimeZoneIndex, &				universalTime);
				SYSTEMTIME tempTime = {universalTime.m_wYear,universalTime.m_wMonth,universalTime.							m_wDayOfWeek,universalTime.m_wDay,universalTime.m_wHour,universalTime.m_wMinute,universalTime				.m_wSecond,universalTime.m_wMilliseconds};
				if (SystemTimeToVariantTime(&tempTime,&date) != 0) 
				{
						varDate.vt = VT_DATE;
						varDate.date = date;
						VariantChangeType(&varDate,&varDate,																		VARIANT_NOVALUEPROP,VT_BSTR);
						strText.Format(TEXT("%s"),varDate.bstrVal );
						v_lastLogonDate.SetItemText(dnCount, dcCount+1, strText);
						VariantClear(&varDate);
				}
				else
					{
					strText.Format(TEXT("%s"),L"-" );
					v_lastLogonDate.SetItemText(dnCount, dcCount+1, strText);
					}
				
				LastLogonDate++;
			}
		}
	
	CRegTimeZoneInfo* pRegTimeZoneInfo = pTimeZoneInfoManager->m_arrRegTimeZoneInfo[m_iCurrentTimeZoneIndex];
	this->SetTimezoneResult(pRegTimeZoneInfo->m_szStd);
		
}
