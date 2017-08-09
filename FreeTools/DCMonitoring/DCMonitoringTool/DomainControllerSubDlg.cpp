// DomainControllerSubDlg.cpp : implementation file
//

#include "stdafx.h"
#include "DCMonitoringTool.h"
#include "DomainControllerSubDlg.h"
#include ".\domaincontrollersubdlg.h"


// CDomainControllerSubDlg dialog

IMPLEMENT_DYNAMIC(CDomainControllerSubDlg, CDialog)
CDomainControllerSubDlg::CDomainControllerSubDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CDomainControllerSubDlg::IDD, pParent)
{
}

CDomainControllerSubDlg::~CDomainControllerSubDlg()
{
}

void CDomainControllerSubDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST1, C_DomainControllerList);
	DDX_Control(pDX, IDOK, m_ok);
}


BEGIN_MESSAGE_MAP(CDomainControllerSubDlg, CDialog)
	ON_BN_CLICKED(IDOK, OnBnClickedOk)
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()


// CDomainControllerSubDlg message handlers

BOOL CDomainControllerSubDlg::OnInitDialog()
{
	CDialog::OnInitDialog();
	m_ok.SetFaceColor(RGB(0,133,183),true);
	m_ok.SetTextColor(RGB(255,255,255));


	LVCOLUMN lvColumn;

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_CENTER;
	lvColumn.cx = 298;	
	lvColumn.pszText = "Domain Controllers";
	this->C_DomainControllerList.InsertColumn(0, &lvColumn);

	this->C_DomainControllerList.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES | LVS_EX_CHECKBOXES);	

	CoInitialize(NULL);
	IDirectorySearch *pDSSearch;
	ADS_SEARCH_COLUMN col;
	HRESULT hr;
	int count = 0;		
	hr = ADsOpenObject(ldappathptr,Username,Password,ADS_SECURE_AUTHENTICATION,IID_IDirectorySearch,(void **)&pDSSearch);		
	LPWSTR pszAttr[] = {L"Name"};
	ADS_SEARCH_HANDLE hSearch;
	DWORD dwCount= sizeof(pszAttr)/sizeof(LPWSTR);	
	hr = pDSSearch->ExecuteSearch(L"(&(objectClass=computer))", pszAttr, dwCount, &hSearch );			
	hr = pDSSearch->GetNextRow( hSearch);
	while(hr == S_OK)
	{        
		hr = pDSSearch->GetColumn( hSearch, L"Name", &col ); 
		if ( SUCCEEDED(hr) )
		{ 
			if (col.dwADsType == ADSTYPE_CASE_IGNORE_STRING)
			{			   				
				DWORD dwRet = NULL;
				PDOMAIN_CONTROLLER_INFOA pdci;  
				dwRet = DsGetDcName(CW2A( col.pADsValues->CaseIgnoreString ), 
									NULL, 
									NULL, 
									NULL, 
									//DS_GOOD_TIMESERV_PREFERRED,
									DS_TIMESERV_REQUIRED,					
									&pdci);
				if(NO_ERROR == dwRet)
				{     										
					count = this->C_DomainControllerList.GetItemCount();
					for(int i = 0 ; i <= count ; i ++)
					{
						int comp = strcmp(this->C_DomainControllerList.GetItemText(i,0),strtok(strtok(pdci->DomainControllerName,"."),"\\"));
						if( comp == 0)													
							goto l;						
						else
						{
							LVITEM lvItem;
							lvItem.mask = LVIF_TEXT;
							lvItem.iItem = 0;
							lvItem.iSubItem = 0;							
							lvItem.pszText = strtok(strtok(pdci->DomainControllerName,"."),"\\");							
							this->C_DomainControllerList.InsertItem(&lvItem);							

							NetApiBufferFree(pdci);
							goto l;
						}
					}
				}
				else
					goto l1;
			}
			pDSSearch->FreeColumn( &col );
		}
l:		hr = pDSSearch->GetNextRow( hSearch);
		dwCount++;
	}
l1:	pDSSearch->CloseSearchHandle(hSearch);
	pDSSearch->Release();		
	return TRUE;
}
void CDomainControllerSubDlg::OnBnClickedOk()
{	
	selDC.Empty();
	for(int i = 0 ; i < this->C_DomainControllerList.GetItemCount(); i ++)
	{	
		if(this->C_DomainControllerList.GetCheck(i))
		{
			selCnt = selCnt + 1;
			CString selstr = this->C_DomainControllerList.GetItemText(i,0);				
			if(!(selDC.IsEmpty()))
				selDC.Append(",");
			selDC.Append(selstr);
		}
	}
	if(!(selDC.IsEmpty()))
	{
	if(getInfo()){
		if(getFileDate()==0){
		addDb();
		}
		else if(startApp())
			updateDb();
		writeToFile(5);
	}
		timer = 1;
	}
	OnOK();
}

BOOL CDomainControllerSubDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	     CRect r;
         pDC->GetClipBox(&r);
		 pDC->FillSolidRect(r, RGB(244,244,244)); 
         return TRUE;
}
