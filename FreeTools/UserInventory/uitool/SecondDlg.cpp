// SecondDlg.cpp : implementation file
//

#include "stdafx.h"
#include "uitool.h"
#include "SecondDlg.h"
#include ".\seconddlg.h"
#include"uitoolDlg.h"

LPCTSTR User[] = {"All","Naming","Security","Address Book"};
LPCTSTR Contact[] = {"All","Naming","ContactNumbers","Office Details","Address Book"};
LPCTSTR Computer[] = {"All","Naming","Security","OS Details"};
LPCTSTR Group[] = {"All","Naming","General"};

// CSecondDlg dialog

IMPLEMENT_DYNAMIC(CSecondDlg, CDialog)
CSecondDlg::CSecondDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSecondDlg::IDD, pParent)
{
}


void CSecondDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST1, lbox1);
	DDX_Control(pDX, IDC_LIST2, lbox2);
	DDX_Control(pDX, IDC_LIST3, lbox3);
	DDX_Control(pDX, IDC_LIST4, lbox4);
	DDX_Control(pDX, IDC_COMBO1, cbox1);
	DDX_Control(pDX, IDC_CHECK1, c_ConfirmChkBox);
}


BEGIN_MESSAGE_MAP(CSecondDlg, CDialog)
	ON_BN_CLICKED(IDC_BUTTON1, OnBnClickedButton1)
	ON_BN_CLICKED(IDC_BUTTON2, OnBnClickedButton2)
	ON_BN_CLICKED(IDC_BUTTON3, OnBnClickedButton3)	
	ON_CBN_SELCHANGE(IDC_COMBO1, OnCbnSelchangeCombo1)

	ON_WM_CTLCOLOR()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()


// CSecondDlg message handlers

CSecondDlg::~CSecondDlg()
{
}

BOOL CSecondDlg::OnInitDialog()
{
	
	CDialog::OnInitDialog();
	
	this->resetcontent();
	
	if(u == 1)
	{		
		
		for(int i = 0 ; i < sizeof(User)/sizeof(LPCTSTR) ; i ++)
			this->cbox1.AddString(User[i]);
	
		for(unsigned int i = 0 ;i < sizeof(useratt)/sizeof(LPSTR); i ++)
		{
			this->lbox1.AddString(useratt[i]);
			this->lbox3.AddString(useratt1[i]);	
		}
	
		
	}
	if(cn == 1)
	{		
		for(int i = 0 ; i < sizeof(Contact)/sizeof(LPCTSTR) ; i ++)
			this->cbox1.AddString(Contact[i]);
		for(unsigned int i = 0 ;i < sizeof(conatt)/sizeof(LPSTR); i ++)
		{
			this->lbox1.AddString(conatt[i]);
			this->lbox3.AddString(conatt1[i]);	
		}
		
	}
	if(cm == 1)
	{		
		for(int i = 0 ; i < sizeof(Computer)/sizeof(LPCTSTR) ; i ++)
			this->cbox1.AddString(Computer[i]);
		for(unsigned int i = 0 ;i < sizeof(comatt)/sizeof(LPSTR); i ++)
		{
			this->lbox1.AddString(comatt[i]);
			this->lbox3.AddString(comatt1[i]);	
		}
		
	}
	if(g == 1)
	{	
		for(int i = 0 ; i < sizeof(Group)/sizeof(LPCTSTR) ; i ++)
			this->cbox1.AddString(Group[i]);		
		for(unsigned int i = 0 ;i < sizeof(groupatt)/sizeof(LPSTR); i ++)
		{
			this->lbox1.AddString(groupatt[i]);
			this->lbox3.AddString(groupatt1[i]);	
		}
		
	}		

	this->cbox1.SetCurSel(0);
	
	// To display All attributes by default on clilcking the advanced tab.
	for(unsigned int i = 0 ;i < sizeof(useratt)/sizeof(LPSTR) ; i ++)
	{
		this->lbox1.AddString(useratt[i]);
		this->lbox3.AddString(useratt1[i]);	
	}
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}



void CSecondDlg::OnBnClickedButton1()
{
	try
	{
	if(this->lbox1.GetCount() == 0 && this->lbox3.GetCount() == 0)
		MessageBox("There is no items","Attributes", MB_OK | MB_ICONINFORMATION);
	else if(this->lbox1.GetCurSel() == LB_ERR && this->lbox3.GetCurSel() == LB_ERR)
		MessageBox("No Item Selected","Attributes", MB_OK | MB_ICONINFORMATION);
	else
	{
		UpdateData(TRUE);
		int m = lbox3.GetCurSel();
		CString st,s;
		lbox1.GetText(m,st);
		lbox1.DeleteString(m);
		lbox3.GetText(m,s);
		lbox3.DeleteString(m);
		lbox2.AddString(st);
		lbox4.AddString(s);
		UpdateData(FALSE);
	}
	}
	catch(...){}
}

void CSecondDlg::OnBnClickedButton2()
{
	try
	{
	if(this->lbox2.GetCount() == 0 && this->lbox4.GetCount() == 0)
		MessageBox("There is no items","Attributes", MB_OK | MB_ICONINFORMATION);
	else if(this->lbox2.GetCurSel() == LB_ERR && this->lbox4.GetCurSel() == LB_ERR)
		MessageBox("No Item Selected","Attributes", MB_OK | MB_ICONINFORMATION);
	else
	{
		UpdateData(TRUE);
		CString s,st;
		int l = lbox4.GetCurSel();
		lbox2.GetText(l,s);
		lbox1.InsertString(-1,s);
		lbox2.DeleteString(l);
		lbox4.GetText(l,st);
		lbox3.InsertString(-1,st);	
		lbox4.DeleteString(l);	
		UpdateData(FALSE);
	}
	}
	catch(...){}
}

void CSecondDlg::OnBnClickedButton3()
{
	// TODO: Add your control notification handler code here
	UpdateData(TRUE);
	x = this->lbox2.GetCount();
	CString st,s;
	for(unsigned int y = 0;y < x;y ++)
	{
		lbox2.GetText(y,st);
		lbox4.GetText(y,s);
		attlist.Append(st);
		attlist1.Append(s);
			if(y != x - 1)
			{
				attlist.Append(",");
				attlist1.Append(",");
			}
	}
	if(this->c_ConfirmChkBox.GetCheck())
		retVal = 1;
	else
		retVal = 0;
	/*if((!attlist.IsEmpty()) && (!attlist.IsEmpty()))
		retVal = MessageBox("Do U want To Replace The Existing Basic Attributes ? ","Attributes",MB_YESNO);*/
	valid = 1;
	OnOK();
    UpdateData(FALSE);		
}

void CSecondDlg::OnCbnSelchangeCombo1()
{
	// TODO: Add your control notification handler code here
	unsigned int i = this->cbox1.GetCurSel();
	CString st;
	this->cbox1.GetLBText(i,st);
	this->resetcontent();

	//If user object is selected

	if(u == 1)
	{
		if(st == "All")
		{
			this->resetcontent();
			for(unsigned int i = 0 ;i < sizeof(useratt)/sizeof(LPSTR) - 1 ; i ++)
			{
				this->lbox1.AddString(useratt[i]);
				this->lbox3.AddString(useratt1[i]);	
			}
		}
		if(st == "Naming")
		{
			this->resetcontent();
			for(unsigned int i = 0;i < sizeof(userattNam)/sizeof(LPSTR) ;i ++)
			{
				lbox1.AddString(userattNam[i]);
				lbox3.AddString(useratt1Nam[i]);
			}
		}
		if(st == "Security")
		{
			this->resetcontent();
			for(unsigned int i = 0 ;i < sizeof(userattSec)/sizeof(LPSTR) - 1 ;i ++)
			{
				lbox1.AddString(userattSec[i]);
				lbox3.AddString(useratt1Sec[i]);
			}
		}
		if(st == "Address Book")
		{
			this->resetcontent();
			for(unsigned int i = 0 ;i < sizeof(userattAdd)/sizeof(LPSTR) ; i ++)
			{
				lbox1.AddString(userattAdd[i]);
				lbox3.AddString(useratt1Add[i]);
			}	
		}
	}
	//If contact object is selected
	if(cn == 1)
	{
		if(st == "All")
		{
			this->resetcontent();
			for(unsigned int i = 0 ; i < sizeof(conatt)/sizeof(LPSTR) ; i ++)
			{
				lbox1.AddString(conatt[i]);
				lbox3.AddString(conatt1[i]);
			}
		}
		if(st == "Naming")
		{
			this->resetcontent();
			for(unsigned int i = 0 ; i < sizeof(conattNam)/sizeof(LPSTR) ; i ++)
			{
				lbox1.AddString(conattNam[i]);
				lbox3.AddString(conatt1Nam[i]);
			}
		}
		if(st == "Contact Numbers")
		{
			this->resetcontent();
			for(unsigned int i = 0 ; i < sizeof(conattCN)/sizeof(LPSTR) ; i ++)
			{
				lbox1.AddString(conattCN[i]);
				lbox3.AddString(conatt1CN[i]);
			}
		}
		if(st == "Office Details")
		{
			this->resetcontent();
			for(unsigned int i = 0 ; i < sizeof(conattOD)/sizeof(LPSTR) ; i ++)
			{
				lbox1.AddString(conattOD[i]);
				lbox3.AddString(conatt1OD[i]);
			}
		}
		if(st == "Address Book")
		{
			this->resetcontent();
			for(unsigned int i = 0 ; i < sizeof(conattAB)/sizeof(LPSTR) ;i ++)
			{
				lbox1.AddString(conattAB[i]);
				lbox3.AddString(conatt1AB[i]);
			}
		}
	}
	//If computer object is selected
	if(cm == 1)
	{
		if(st == "All")
		{
			this->resetcontent();
			for(unsigned int i = 0 ; i < sizeof(comatt)/sizeof(LPSTR) ; i ++)
			{
				lbox1.AddString(comatt[i]);
				lbox3.AddString(comatt1[i]);
			}
		}
		if(st == "Naming")
		{
			this->resetcontent();
			for(unsigned int i = 0 ; i < sizeof(comattNam)/sizeof(LPSTR) ; i ++)
			{
				lbox1.AddString(comattNam[i]);
				lbox3.AddString(comatt1Nam[i]);
			}
		}
		if(st == "Security")
		{
			this->resetcontent();
			for(unsigned int i = 0 ; i < sizeof(comattSec)/sizeof(LPSTR) ; i ++)
			{				
				lbox1.AddString(comattSec[i]);
				lbox3.AddString(comatt1Sec[i]);
			}
		}
		if(st == "OS Details")
		{
			this->resetcontent();
			for(unsigned int i = 0 ;i < sizeof(comattOS)/sizeof(LPSTR) ; i ++)
			{
				lbox1.AddString(comattOS[i]);
				lbox3.AddString(comatt1OS[i]);
			}
		}
	}
	//If Group object is selected
	if(g == 1)
	{
		if(st == "All")
		{
			this->resetcontent();
			for(unsigned int i = 0 ; i < sizeof(groupatt)/sizeof(LPSTR) ; i ++)
			{
				lbox1.AddString(groupatt[i]);
				lbox3.AddString(groupatt1[i]);
			}
		}
		if(st == "Naming")
		{
			this->resetcontent();
			for(unsigned int i = 0 ; i < sizeof(groupattNam)/sizeof(LPSTR) ; i ++)
			{
				lbox1.AddString(groupattNam[i]);
				lbox3.AddString(groupatt1Nam[i]);
			}
		}
		if(st == "General")
		{
			this->resetcontent();
			for(unsigned int i = 0 ; i < sizeof(groupattGen)/sizeof(LPSTR) ;i ++)
			{
				lbox1.AddString(groupattGen[i]);
				lbox3.AddString(groupatt1Gen[i]);
			}	
		}
	}
}
void CSecondDlg::resetcontent()
{
	this->lbox1.ResetContent();
	this->lbox3.ResetContent();
	this->lbox2.ResetContent();
	this->lbox4.ResetContent();
}


HBRUSH CSecondDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
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


BOOL CSecondDlg::OnEraseBkgnd(CDC* pDC)
{
	     CRect r;
         pDC->GetClipBox(&r);
		 pDC->FillSolidRect(r, RGB(244,244,244)); 
         return TRUE;
}
