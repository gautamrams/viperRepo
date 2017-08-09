// AttributesDlg.cpp : implementation file
//

#include "stdafx.h"
#include "NewText.h"
#include "AttributesDlg.h"
#include ".\attributesdlg.h"
LPSTR tempatt1[]={"OFFICE","TELEPHONE NUMBER","E-MAIL ID","WEB SITE ADDRESS","ADDRESS","POST BOX NUMBER","CITY","STATE/PROVINCE","ZIP CODE","COUNTRY","LOGON NAME","SAMACCOUNTNAME"};
// CAttributesDlg dialog
IMPLEMENT_DYNAMIC(CAttributesDlg, CDialog)
CAttributesDlg::CAttributesDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CAttributesDlg::IDD, pParent)
{
}
CAttributesDlg::~CAttributesDlg()
{
}
void CAttributesDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST1, c_leftAttList);
	DDX_Control(pDX, IDC_LIST2, c_rightAttList);
	DDX_Control(pDX, IDC_CHECK1, c_ConfirmChkBox);
}
BEGIN_MESSAGE_MAP(CAttributesDlg, CDialog)
	ON_BN_CLICKED(IDC_BUTTON1, OnBnClickedButton1)
	ON_BN_CLICKED(IDC_BUTTON2, OnBnClickedButton2)
	ON_BN_CLICKED(IDC_BUTTON3, OnBnClickedButton3)
	ON_BN_CLICKED(IDC_BUTTON4, OnBnClickedButton4)
	ON_BN_CLICKED(IDOK, OnBnClickedOk)
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
END_MESSAGE_MAP()

BOOL CAttributesDlg::OnInitDialog()
{
	CDialog::OnInitDialog();
	//To display already selected attributes in right side pane whereas remaining in left side pane.
	if(!selcnt)
	for(int i = 0;i < ((sizeof(tempatt1)/4 - 1)); i ++)
	{		
		this->c_leftAttList.AddString(tempatt1[i]);
	}
	else
	for(int i = 0;i < ((sizeof(tempatt1)/4 - 1) - selcnt); i ++)
	{		
		this->c_leftAttList.AddString(left[i]);
	}
		for(int i = 0;i < selcnt ; i ++)
	{		
		this->c_rightAttList.AddString(right[i]);
	}

	return TRUE;

}
// CAttributesDlg message handlers

void CAttributesDlg::OnBnClickedButton1()
{
	if(this->c_leftAttList.GetCurSel() == LB_ERR)
		MessageBox("No Item is selected","Attributes", MB_OK | MB_ICONINFORMATION);
	else
	{
		CString res;
		this->c_leftAttList.GetText(this->c_leftAttList.GetCurSel(),res);
		this->c_leftAttList.DeleteString(this->c_leftAttList.GetCurSel());
		this->c_rightAttList.AddString(res);
	}
}
void CAttributesDlg::OnBnClickedButton2()
{
	if(this->c_leftAttList.GetCount() != 0)
	{
		this->c_leftAttList.ResetContent();	
		this->c_rightAttList.ResetContent();
		for(int i = 0; i < sizeof(tempatt1)/4 - 1 ; i ++)			
			this->c_rightAttList.AddString(tempatt1[i]);	
	}
	else
		MessageBox("There is no items","Attributes", MB_OK | MB_ICONINFORMATION);
}
void CAttributesDlg::OnBnClickedButton3()
{
	if(this->c_rightAttList.GetCurSel() == LB_ERR)
		MessageBox("No Item is selected","Attributes", MB_OK | MB_ICONINFORMATION);
	else
	{
		CString res;
		this->c_rightAttList.GetText(this->c_rightAttList.GetCurSel(),res);
		this->c_rightAttList.DeleteString(this->c_rightAttList.GetCurSel());
		this->c_leftAttList.AddString(res);
	}
}
void CAttributesDlg::OnBnClickedButton4()
{
	if(this->c_rightAttList.GetCount() != 0)
	{
		this->c_rightAttList.ResetContent();
		this->c_leftAttList.ResetContent();
		for(int i = 0; i < sizeof(tempatt1)/4 - 1 ; i ++)			
			this->c_leftAttList.AddString(tempatt1[i]);	
	}
	else
		MessageBox("There is no items","Attributes", MB_OK | MB_ICONINFORMATION);
}

void CAttributesDlg::OnBnClickedOk()
{	
	selectedstr.Empty();
	CString str;
	selcnt = this->c_rightAttList.GetCount();

//populate left and right arrays for loading values when the attributesdlg is loaded next time
	for(int i = 0 ; i < this->c_rightAttList.GetCount() ; i ++)
	{
		this->c_rightAttList.GetText(i,str);
		if(i < this->c_rightAttList.GetCount() && i != 0)
		selectedstr.Append(",");
		selectedstr.Append(str);
		right[i].Empty();
		right[i].Append(str);
	}
		for(int i = 0 ; i < this->c_leftAttList.GetCount() ; i ++)
	{
		this->c_leftAttList.GetText(i,str);
		left[i].Empty();
		left[i].Append(str);
	}
	if(!selectedstr.IsEmpty()){
		if(getInfo()){
			if(getFileDate()==0){
				addDb();
			}
			else if(startApp())
				updateDb();
			writeToFile(1);
		}
	}
	if(this->c_ConfirmChkBox.GetCheck()){
			
		retVal = 1;
	}
	else
		retVal = 0;	
	OnOK();
}


BOOL CAttributesDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default
	     CRect r;
         pDC->GetClipBox(&r);
		 pDC->FillSolidRect(r, RGB(244,244,244)); 
         return TRUE;
	
}


HBRUSH CAttributesDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
       switch (nCtlColor)
       {
       case CTLCOLOR_STATIC:
	     pDC->SetBkColor(RGB(244, 244 , 244));
		 return (HBRUSH)GetStockObject(NULL_BRUSH);
       default:
	   return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
	   }
}
