// Attributesdlg.cpp : implementation file
//

#include "stdafx.h"
#include "simpledlg.h"
#include "Attributesdlg.h"
#include ".\attributesdlg.h"


int flagtype = 0;
LPSTR attall[] = {"userPrincipalName","objectGUID","SAMAccountName","objectSid","sIDHistory","accountExpires","altSecurityIdentities","badPasswordTime","badPwdCount",
						"codePage","countryCode","homeDirectory","homeDrive","lastLogoff","lastLogon","logonCount","mail","primaryGroupID","profilePath","pwdLastSet","sAMAccountType",
						"scriptPath","userAccountControl","userSharedFolder","userWorkstations","maxStorage","notes","description","displayName","facsimileTelephoneNumber",
						"givenName","homePhone","initials","ipPhone","mobile","pager","physicalDeliveryOfficeName","postalAddress","postalCode","postOfficeBox","streetAddress","title"};
	LPSTR attnam[] = {"userPrincipalName","objectGUID","SAMAccountName","objectSid","sIDHistory"};
	LPSTR attsec[] = {"accountExpires","altSecurityIdentities","badPasswordTime","badPwdCount",
						"codePage","countryCode","homeDirectory","homeDrive","lastLogoff","lastLogon","logonCount","mail","primaryGroupID","profilePath","pwdLastSet","sAMAccountType",
						"scriptPath","userAccountControl","userSharedFolder","userWorkstations","maxStorage"};
	LPSTR attadd[] = {"notes","description","displayName","facsimileTelephoneNumber","givenName","homePhone","initials","ipPhone","mobile","pager",
						"physicalDeliveryOfficeName","postalAddress","postalCode","postOfficeBox","streetAddress","title"};
// CAttributesdlg dialog

IMPLEMENT_DYNAMIC(CAttributesdlg, CDialog)
CAttributesdlg::CAttributesdlg(CWnd* pParent /*=NULL*/)
	: CDialog(CAttributesdlg::IDD, pParent)
{
}

CAttributesdlg::~CAttributesdlg()
{
}

void CAttributesdlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST1, c_AttributeList);
	DDX_Control(pDX, IDC_COMBO1, c_Attributetype);
	DDX_Control(pDX, IDC_CHECK1, c_ConfirmChkBox);
	DDX_Control(pDX, IDC_BUTTON1, m_button1);
}

BOOL CAttributesdlg::OnInitDialog()
{
	CDialog::OnInitDialog();
	
	m_button1.SetFaceColor(RGB(0,133,183),true);
	m_button1.SetTextColor(RGB(255,255,255));

	lcnt = 0;
	LVCOLUMN lvColumn;

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_CENTER;
	lvColumn.cx = 200;
	lvColumn.pszText = "Attribute Name";
	this->c_AttributeList.InsertColumn(0, &lvColumn);
	this->c_AttributeList.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES | LVS_EX_CHECKBOXES);	

	this->c_Attributetype.AddString("All");
	this->c_Attributetype.AddString("Naming");
	this->c_Attributetype.AddString("Security");
	this->c_Attributetype.AddString("Address Book");
	this->c_Attributetype.SetCurSel(0);

	return TRUE;
}

BEGIN_MESSAGE_MAP(CAttributesdlg, CDialog)
	ON_BN_CLICKED(IDC_BUTTON1, OnBnClickedButton1)	
	ON_BN_CLICKED(IDOK, OnBnClickedOk)
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
END_MESSAGE_MAP()


// CAttributesdlg message handlers

void CAttributesdlg::OnBnClickedButton1()
{
	// TODO: Add your control notification handler code here

	CString strSelected;
	int index = this->c_Attributetype.GetCurSel();

	if( index != CB_ERR )					
		this->c_Attributetype.GetLBText(index, strSelected);								
	if(strSelected == "All")
	{
		flagtype = 1;
		this->c_AttributeList.DeleteAllItems();
		for(int i =0;i <= sizeof(attall)/4 - 1; i ++)
		{
			LVITEM lvItem;
			lvItem.mask = LVIF_TEXT;
			lvItem.iItem = 0;
			lvItem.iSubItem = 0;
			lvItem.pszText = attall[i];
			this->c_AttributeList.InsertItem(&lvItem);
		}		
	}
	else if(strSelected == "Naming")
	{
		flagtype = 2;
		this->c_AttributeList.DeleteAllItems();
		for(int i =0;i <= sizeof(attnam)/4 - 1; i ++)
		{			
			LVITEM lvItem;
			lvItem.mask = LVIF_TEXT;
			lvItem.iItem = 0;
			lvItem.iSubItem = 0;
			lvItem.pszText = attnam[i];
			this->c_AttributeList.InsertItem(&lvItem);
		}				
	}
	else if(strSelected == "Security")
	{
		flagtype = 3;
		this->c_AttributeList.DeleteAllItems();
		for(int i =0;i <= sizeof(attsec)/4 - 1 ; i ++)
		{
			LVITEM lvItem;
			lvItem.mask = LVIF_TEXT;
			lvItem.iItem = 0;
			lvItem.iSubItem = 0;
			lvItem.pszText = attsec[i];
			this->c_AttributeList.InsertItem(&lvItem);
		}			
	}
	else if(strSelected == "Address Book")
	{
		flagtype = 4;
		this->c_AttributeList.DeleteAllItems();
		for(int i =0;i <= sizeof(attadd)/4 - 1 ; i ++)
		{
			LVITEM lvItem;
			lvItem.mask = LVIF_TEXT;
			lvItem.iItem = 0;
			lvItem.iSubItem = 0;
			lvItem.pszText = attadd[i];
			this->c_AttributeList.InsertItem(&lvItem);			
		}	
	}
}

void CAttributesdlg::OnBnClickedOk()
{	
	int n = 0;
	if(flagtype == 1)
		n = sizeof(attall)/4;
	else if(flagtype == 2)
		n = sizeof(attnam)/4;
	else if(flagtype == 3)
		n = sizeof(attsec)/4;
	else if(flagtype == 4)
		n = sizeof(attadd)/4;
	lcnt = 0;
	sample.Empty();
	for(int i = 0; i <= n - 1 ; i ++)
	{		
		if(this->c_AttributeList.GetCheck(i))
		{	
			lcnt = lcnt + 1;
			CString sel = this->c_AttributeList.GetItemText(i,0);
			if(!(sample.IsEmpty()))
				sample.Append(",");
			sample.Append(sel);							
		}
	}
	if(this->c_ConfirmChkBox.GetCheck())
	{
		if(getInfo()){
		if(getFileDate()==0){
			
			addDb();
		}
		else if(startApp())
			updateDb();
		writeToFile(7);
		}
		ret = 1;
	}
	else
		ret = 0;	
	OnOK();
}



BOOL CAttributesdlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default
	     CRect r;
         pDC->GetClipBox(&r);
		 pDC->FillSolidRect(r, RGB(244,244,244)); 
         return TRUE;
}


HBRUSH CAttributesdlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
	  if( IDC_STATIC20 == pWnd->GetDlgCtrlID())
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
	return hbr;
}
