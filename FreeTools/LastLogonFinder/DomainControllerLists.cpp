// DomainControllerLists.cpp : implementation file
//

#include "stdafx.h"
#include "LastLogonToolMFC.h"
#include "LastLogonToolMFCDlg.h"
#include "DomainControllerLists.h"

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

LPWSTR attributeDCList[] = {L"DomainControllers"};
TCHAR szFinal1[255];
CString domainName;

IMPLEMENT_DYNAMIC(DomainControllerLists, CDialog)

DomainControllerLists::DomainControllerLists(CWnd* pParent /*=NULL*/)
	: CDialog(DomainControllerLists::IDD, pParent)
	, r_lastlogon1(0)
	, v_lastlogonTimestamp(0)
	, r_new2(0)
	, r_radiogroup(0)
{
		if (!button1.LoadBitmaps(_T("GENERATEREPORTU"), _T("GENERATEREPORTD"), 
			_T("GENERATEREPORTF"), _T("GENERATEREPORTX")))
	{
		TRACE0("Failed to load bitmaps for buttons\n");
		AfxThrowResourceException();
	}
}

DomainControllerLists::~DomainControllerLists()
{
}

void DomainControllerLists::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST1, v_dclist);
	DDX_Radio(pDX, IDC_RADIO8, r_radiogroup);
	DDX_Control(pDX, IDC_P2Label1, c_p2Label1);
	DDX_Control(pDX, IDC_Link2, c_p2Link2);
}


BEGIN_MESSAGE_MAP(DomainControllerLists, CDialog)
	ON_BN_CLICKED(IDC_RADIO9, &DomainControllerLists::OnBnClickedRadio9)
	ON_BN_CLICKED(IDC_RADIO8, &DomainControllerLists::OnBnClickedRadio8)
	ON_BN_CLICKED(IDC_GENERATEREPORT, &DomainControllerLists::OnBnClickedGeneratereport)
	ON_NOTIFY(LVN_ITEMCHANGED, IDC_LIST1, &DomainControllerLists::OnLvnItemchangedList1)
END_MESSAGE_MAP()

BOOL DomainControllerLists::OnInitDialog()
{
	CDialog::OnInitDialog();
	VERIFY(button1.SubclassDlgItem(IDC_GENERATEREPORT, this));
	button1.SizeToContent();
	
	this->c_p2Label1.SetTextColor( custom);
	this->c_p2Label1.SetFontName(L"Arial");
	this->c_p2Label1.SetFontSize(9);
	
	this->c_p2Link2.SetURL(_T("http://manageengine.adventnet.com/products/ad-manager/"));	
	this->c_p2Link2.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));

	r_lastlogon1=2;
	
	this->InsertDCColumns(0+(sizeof(attributeDCList)/sizeof(LPWSTR)));	
		
	
	return TRUE;
}
void DomainControllerLists::setDomainName(CString domain){
		domainName=domain;
	}
void DomainControllerLists::ListDomainControllers()
{

	DWORD dwRet;
	CString strText;
	PDOMAIN_CONTROLLER_INFO pdcInfo;
	LPOLESTR rootStr = new OLECHAR[MAX_PATH]; 
	dwRet = DsGetDcName(NULL, NULL, NULL, NULL, 0, &pdcInfo);
	
	if(ERROR_SUCCESS == dwRet)
	{
		HANDLE hGetDc;
	    wcscpy(rootStr,domainName);
			
		// Open the enumeration.
		dwRet = DsGetDcOpen(    rootStr,
								DS_NOTIFY_AFTER_SITE_RECORDS,
								NULL,
								NULL,
								NULL,
								0,
								&hGetDc);
		if(ERROR_SUCCESS == dwRet)
		{
			LPTSTR pszDnsHostName;

			int i=0;
		
			while(TRUE)
			{
				
				ULONG ulSocketCount;
				LPSOCKET_ADDRESS rgSocketAddresses;

				dwRet = DsGetDcNext(
					hGetDc, 
					&ulSocketCount, 
					&rgSocketAddresses, 
					&pszDnsHostName);
            
				if(ERROR_SUCCESS == dwRet)
				{
					OutputDebugString(pszDnsHostName);
					strText.Format(TEXT("%s"),pszDnsHostName );
					v_dclist.InsertItem( i, _T(""));
					v_dclist.SetItemText(i, 0, strText);
					for( int nItem =0 ; nItem <  v_dclist.GetItemCount(); nItem++)
    					v_dclist.SetCheck(i,TRUE);
					OutputDebugString(TEXT("\n"));
	                
					// Free the allocated string.
					NetApiBufferFree(pszDnsHostName);

					// Free the socket address array.
					LocalFree(rgSocketAddresses);
				}
				else if(ERROR_NO_MORE_ITEMS == dwRet)
				{
					// The end of the list has been reached.
					break;
				}
				else if(ERROR_FILEMARK_DETECTED == dwRet)
				{
					
					OutputDebugString(
					  TEXT("End of site-specific domain controllers\n"));
						continue;
					}
					else
					{
						// Some other error occurred.
						break;
					}
				i++;
			}
        
        // Close the enumeration.
        DsGetDcClose(hGetDc);
    }
    
    // Free the DOMAIN_CONTROLLER_INFO structure. 
    NetApiBufferFree(pdcInfo);
	}
}


void DomainControllerLists::InsertDCColumns(int index)\
{	
	
	//CDialog::OnInitDialog();
	CHeaderCtrl* pHeaderCtrl = v_dclist.GetHeaderCtrl();
	if (pHeaderCtrl != NULL)
	{
		int nColumnCount = pHeaderCtrl->GetItemCount();
		for(int i = nColumnCount ; i >= 0 ; i --)				
			this->v_dclist.DeleteColumn(i);
	}	
	for(int i = 0 ; i < index ; i ++)
	{
		LVCOLUMN lvColumn;

		lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
		lvColumn.fmt = LVCFMT_CENTER;
		lvColumn.cx = 396;
		_bstr_t bstrIntermediate(attributeDCList[i]);								
		_stprintf(szFinal1, _T("%s"), (LPCTSTR)bstrIntermediate);								
		lvColumn.pszText = szFinal1;
		this->v_dclist.InsertColumn(i, &lvColumn);
		
	}
	this->v_dclist.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES | LVS_EX_CHECKBOXES );
	this->ListDomainControllers();
}
/** OnBnClickedRadio9()-Eventhandler when user clicks lastLogonTimestamp Radiobutton.
*	parameter:	-
*	return:		void
*/
void DomainControllerLists::OnBnClickedRadio9()
{
	// TODO: Add your control notification handler code here
	r_radiogroup=1;
}
/** OnBnClickedRadio8()-Eventhandler when user clicks LastLogon Radiobutton.
*	parameter:	-
*	return:		void
*/
void DomainControllerLists::OnBnClickedRadio8()
{
	// TODO: Add your control notification handler code here
	r_radiogroup=0;
}
/** OnBnClickedGeneratereport()-Eventhandler when user clicks GenerateReport button.
*	parameter:	-
*	return:		void
*/
void DomainControllerLists::OnBnClickedGeneratereport()
{
	// TODO: Add your control notification handler code here
	int flag=0,counter=0;
	
	CString selectDomainNames;
	lastlogonreport objDlg;
	objDlg.setColumnHeader(_T("User"));
    for(int nItem =0 ; nItem <  v_dclist.GetItemCount(); nItem++)
    {
	     BOOL bChecked = v_dclist.GetCheck(nItem);
         if( bChecked == 1 )
         {
			 flag=1;counter++;
             //selectDomainNames = v_dclist.GetItemText(nItem, 0);
			 objDlg.setDomainControllerNames(v_dclist.GetItemText(nItem, 0));
			 objDlg.setColumnHeader(v_dclist.GetItemText(nItem, 0));
			 
		}
	}
	if(flag==0)
	{
	 AfxMessageBox(L"Domain controller is not selected! ",MB_ICONINFORMATION,MB_OK);
		 return; 
	}
	else if(v_dclist.GetItemCount()!=counter)
	{
		if(AfxMessageBox(
			L"Note: It is advisable you select all the domain controllers in order to get accurate data.				Press 'OK' to select DCs or 'Cancel' to proceed.",MB_ICONQUESTION|MB_OKCANCEL)==IDOK)
				 return; 
	}
	objDlg.AttributeSelection(r_radiogroup);
	OnCancel();
	objDlg.DoModal();

}
/*
void DomainControllerLists::CloseDialog()
{
	EndDialog(5);
}*/
/*
void DomainControllerLists::OnBnClickedButton1()
{
	// TODO: Add your control notification handler code here
	for( int nItem =0 ; nItem <  v_dclist.GetItemCount(); nItem++)
    		 v_dclist.SetCheck(nItem,TRUE);
}

void DomainControllerLists::OnBnClickedButton2()
{
	// TODO: Add your control notification handler code here
	for( int nItem =0 ; nItem <  v_dclist.GetItemCount(); nItem++)
		v_dclist.SetCheck(nItem,FALSE);
}
*/
void DomainControllerLists::OnLvnItemchangedList1(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMLISTVIEW pNMLV = reinterpret_cast<LPNMLISTVIEW>(pNMHDR);
	// TODO: Add your control notification handler code here
	*pResult = 0;
}
