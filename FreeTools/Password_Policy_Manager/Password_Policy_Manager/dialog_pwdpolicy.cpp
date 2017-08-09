// dialog_pwdpolicy.cpp : implementation file
//

#include "stdafx.h"
#include "Password_Policy_Manager.h"
#include "dialog_pwdpolicy.h"

// dialog_pwdpolicy dialog

IMPLEMENT_DYNAMIC(dialog_pwdpolicy, CDialog)

dialog_pwdpolicy::dialog_pwdpolicy(CWnd* pParent /*=NULL*/)
	: CDialog(dialog_pwdpolicy::IDD, pParent)
	//, hist(_T(""))
	//, max(_T(""))
	//, min(_T(""))
	//, len(_T(""))
{
     m_generate.SetFaceColor(RGB(0,133,183),true);
	 m_generate.SetTextColor(RGB(255,255,255));
	 m_save.SetFaceColor(RGB(0,133,183),true);
	 m_save.SetTextColor(RGB(255,255,255));
	 m_logout.SetFaceColor(RGB(0,133,183),true);
	 m_logout.SetTextColor(RGB(255,255,255));
	 m_default.SetFaceColor(RGB(0,133,183),true);
	 m_default.SetTextColor(RGB(255,255,255));

}

dialog_pwdpolicy::~dialog_pwdpolicy()
{
}

void dialog_pwdpolicy::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_HIST, hist);
	DDX_Text(pDX, IDC_MAX, max);
	DDX_Text(pDX, IDC_MIN, min);
	DDX_Text(pDX, IDC_LEN, len);
	DDX_Control(pDX, IDC_RAD_COMP_EN, rad1);
	DDX_Control(pDX, IDC_RAD_COMP_DIS, rad2);
	DDX_Control(pDX, IDC_RAD_REV_EN, rad3);
	DDX_Control(pDX, IDC_RAD_REV_DIS, rad4);
	DDX_Control(pDX, IDC_GENERATE, m_generate);
	DDX_Control(pDX, IDC_SAVE, m_save);
	DDX_Control(pDX, IDCANCEL, m_logout);
	DDX_Control(pDX, IDC_DEFAULT, m_default);
}


BEGIN_MESSAGE_MAP(dialog_pwdpolicy, CDialog)
	ON_EN_CHANGE(IDC_HIST, &dialog_pwdpolicy::OnEnChangeHist)
	ON_EN_CHANGE(IDC_MAX, &dialog_pwdpolicy::OnEnChangeMax)
	ON_EN_CHANGE(IDC_MIN, &dialog_pwdpolicy::OnEnChangeMin)
	ON_EN_CHANGE(IDC_LEN, &dialog_pwdpolicy::OnEnChangeLen)
	ON_BN_CLICKED(IDC_GENERATE, &dialog_pwdpolicy::OnBnClickedGenerate)
	ON_BN_CLICKED(IDC_SAVE, &dialog_pwdpolicy::OnBnClickedSave)
	ON_BN_CLICKED(IDC_DEFAULT, &dialog_pwdpolicy::OnBnClickedDefault)
	ON_WM_CTLCOLOR()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

// dialog_pwdpolicy message handlers

void dialog_pwdpolicy::OnEnChangeHist()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialog::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	UpdateData();
}

void dialog_pwdpolicy::OnEnChangeMax()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialog::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	UpdateData();
}

void dialog_pwdpolicy::OnEnChangeMin()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialog::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	UpdateData();
}

void dialog_pwdpolicy::OnEnChangeLen()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialog::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	UpdateData();
}

void dialog_pwdpolicy::OnBnClickedGenerate()
{
	// TODO: Add your control notification handler code here
	IDirectoryObject *dom = NULL;
	HRESULT hr;

	CoInitialize(NULL);

	hr = ADsOpenObject(dom_name_ppm,
					   user_name_ppm,
					   password_ppm,
					   ADS_SECURE_AUTHENTICATION,
					   IID_IDirectoryObject,
					   (void**) &dom);
	if(SUCCEEDED(hr))
	{
		LPWSTR attrnames[] = {L"pwdHistoryLength", L"maxPwdAge", L"minPwdAge", L"minPwdLength", L"pwdProperties"};
		DWORD num_attr = sizeof(attrnames)/sizeof(LPWSTR);
		ADS_ATTR_INFO *attrinfo = NULL;
		DWORD attr_num;

		DWORD i=0;
		LARGE_INTEGER max_age1, min_age1;
		__int64 imax_age1, imin_age1;
		long imax_high1, imax_low1, imin_high1, imin_low1;
		int comp_rev1;

		hr = dom->GetObjectAttributes(attrnames, num_attr, &attrinfo, &attr_num);
		if(SUCCEEDED(hr))
		{
			for(i=0;i<attr_num;i++)
			{
				if(_wcsicmp(attrinfo[i].pszAttrName, L"pwdHistoryLength") == 0)
				{
					hist.Format(_T("%d"),attrinfo[i].pADsValues->Integer);
					UpdateData(FALSE);
				}
						
				if(_wcsicmp(attrinfo[i].pszAttrName, L"minPwdLength") == 0)
				{					
					len.Format(_T("%d"),attrinfo[i].pADsValues->Integer);
					UpdateData(FALSE);
				}

				if(_wcsicmp(attrinfo[i].pszAttrName, L"maxPwdAge") == 0)
				{							
					max_age1 = attrinfo[i].pADsValues->LargeInteger; 							
				
					imax_high1 = max_age1.HighPart;
					imax_low1 = max_age1.LowPart;
					imax_age1 = (ULONG)imax_high1;
					imax_age1 = imax_age1 << 32;
					imax_age1 = imax_age1 + (ULONG)imax_low1;

					imax_age1 = imax_age1/864000000000;
					
					max.Format(_T("%d"),-imax_age1);
					UpdateData(FALSE);
				}

				if(_wcsicmp(attrinfo[i].pszAttrName, L"minPwdAge") == 0)
				{							
					min_age1 = attrinfo[i].pADsValues->LargeInteger;
					imin_high1 = min_age1.HighPart;
					imin_low1 = min_age1.LowPart;

					imin_age1 = (ULONG)imin_high1;
					imin_age1 = imin_age1 << 32;
					imin_age1 = imin_age1 + (ULONG)imin_low1;

					imin_age1 = imin_age1/864000000000;
				
					min.Format(_T("%d"),-imin_age1);
					UpdateData(FALSE);
				}

				if(_wcsicmp(attrinfo[i].pszAttrName, L"pwdProperties") == 0)
				{
					UpdateData(FALSE);
					comp_rev1 = attrinfo[i].pADsValues->Integer;
					if(comp_rev1 == 0)
					{
						rad1.SetCheck(0);
						rad2.SetCheck(1);
						rad3.SetCheck(0);
						rad4.SetCheck(1);
					}
					else if(comp_rev1 == 1)
					{
						rad1.SetCheck(1);
						rad2.SetCheck(0);
						rad3.SetCheck(0);
						rad4.SetCheck(1);
					}
					else if(comp_rev1 == 16)
					{
						rad1.SetCheck(0);
						rad2.SetCheck(1);
						rad3.SetCheck(1);
						rad4.SetCheck(0);
					}
					else if(comp_rev1 == 17)
					{
						rad1.SetCheck(1);
						rad2.SetCheck(0);
						rad3.SetCheck(1);
						rad4.SetCheck(0);
					}
				}
			}
		}
		dom->Release();
	}
	
	CoUninitialize();
}

void dialog_pwdpolicy::OnBnClickedSave()
{
	// TODO: Add your control notification handler code here
	if((!hist.IsEmpty()) && (!max.IsEmpty()) && (!min.IsEmpty()) && (!len.IsEmpty()))
	{
	
	//MessageBox(L"Administrators Rights Required", L"Password Policy Manager");

	IDirectoryObject *dom = NULL;
	HRESULT hr;
	bool is_error = false;
	CoInitialize(NULL);

	hr = ADsOpenObject(dom_name_ppm,
					   user_name_ppm,
					   password_ppm,
					   ADS_SECURE_AUTHENTICATION,
					   IID_IDirectoryObject,
					   (void**) &dom);
	if(SUCCEEDED(hr))
	{
		DWORD num_mod;

		UpdateData();
		ADSVALUE pwdprop_val;
		ADS_ATTR_INFO pwdprop_chattrinfo[] = {L"pwdProperties", ADS_ATTR_UPDATE, ADSTYPE_INTEGER, &pwdprop_val, 1};
		DWORD pwdprop_chnum_attr = sizeof(pwdprop_chattrinfo)/sizeof(ADS_ATTR_INFO);

		pwdprop_val.dwType = ADSTYPE_INTEGER;

		if((rad1.GetCheck() == 1) && (rad2.GetCheck() == 0) && (rad3.GetCheck() == 1) && (rad4.GetCheck() == 0))
			pwdprop_val.Integer = 17;
		else if((rad1.GetCheck() == 1) && (rad2.GetCheck() == 0) && (rad3.GetCheck() == 0) && (rad4.GetCheck() == 1))
			pwdprop_val.Integer = 1;
		else if((rad1.GetCheck() == 0) && (rad2.GetCheck() == 1) && (rad3.GetCheck() == 1) && (rad4.GetCheck() == 0))
			pwdprop_val.Integer = 16;
		else if((rad1.GetCheck() == 0) && (rad2.GetCheck() == 1) && (rad3.GetCheck() == 0) && (rad4.GetCheck() == 1))
			pwdprop_val.Integer = 0;

		dom->SetObjectAttributes(pwdprop_chattrinfo, pwdprop_chnum_attr, &num_mod);

		int pwd_hist_p;
		pwd_hist_p = _wtoi(hist.GetString());
	
		if(pwd_hist_p <= 1024 && pwd_hist_p >= 0)
		{	
			ADSVALUE hist_val;
			ADS_ATTR_INFO hist_chattrinfo[] = {L"pwdHistoryLength", ADS_ATTR_UPDATE, ADSTYPE_INTEGER, &hist_val, 1};
			DWORD hist_chnum_attr = sizeof(hist_chattrinfo)/sizeof(ADS_ATTR_INFO);			

			hist_val.dwType = ADSTYPE_INTEGER;
			hist_val.Integer = pwd_hist_p;

			if(FAILED(dom->SetObjectAttributes(hist_chattrinfo, hist_chnum_attr, &num_mod)))
			{
			MessageBox(L"Administrators Rights Required", L"Password Policy Manager");
			return;
			}
		}
		else
		{
			MessageBox(L"The Password History Length must be between 0 and 1024", L"Password Policy Manager");
			is_error = true;
		}

		int pwd_len_p;
		pwd_len_p = _wtoi(len.GetString());
		if(pwd_len_p <= 256 && pwd_len_p >= 0)
		{
			ADSVALUE len_val;
			ADS_ATTR_INFO len_chattrinfo[] = {L"minPwdLength", ADS_ATTR_UPDATE,	ADSTYPE_INTEGER, &len_val, 1};
			DWORD len_chnum_attr = sizeof(len_chattrinfo)/sizeof(ADS_ATTR_INFO);

			len_val.dwType = ADSTYPE_INTEGER;
			len_val.Integer = pwd_len_p;

			dom->SetObjectAttributes(len_chattrinfo, len_chnum_attr, &num_mod);
		}
		else
		{
			MessageBox(L"The Minimum Password Length must be between 1 and 256 or 0 for no Password required", L"Password Policy Manager");
			is_error = true;
		}

		LPWSTR attrnamesmax[] = {L"minPwdAge"};
		DWORD num_attrmax = sizeof(attrnamesmax)/sizeof(LPWSTR);
		ADS_ATTR_INFO *attrinfomax = NULL;
		DWORD attr_nummax;

		DWORD imax=0;
		LARGE_INTEGER min_age1;
		__int64 imin_age1;
		long imin_high1, imin_low1;
		int pwd_min_age_i;

		hr = dom->GetObjectAttributes(attrnamesmax, num_attrmax, &attrinfomax, &attr_nummax);
		if(SUCCEEDED(hr))
		{
			for(imax=0;imax<attr_nummax;imax++)
			{
				if(_wcsicmp(attrinfomax[imax].pszAttrName, L"minPwdAge") == 0)
				{							
					min_age1 = attrinfomax[imax].pADsValues->LargeInteger;

					imin_high1 = min_age1.HighPart;
					imin_low1 = min_age1.LowPart;

					imin_age1 = (ULONG)imin_high1;
					imin_age1 = imin_age1 << 32;
					imin_age1 = imin_age1 + (ULONG)imin_low1;

					imin_age1 = imin_age1/864000000000;	
					pwd_min_age_i = -imin_age1;
				}
			}
		}
		
		int pwd_max_age_p;
		pwd_max_age_p = _wtoi(max.GetString());
		
		if(pwd_max_age_p <= 999 && pwd_max_age_p >= 0)
		{
			if(pwd_max_age_p != 0 && pwd_max_age_p <= pwd_min_age_i)
			{
				MessageBox(L"The Maximum Password Age must be greater than Minimum Password Age", L"Password Policy Manager");
				is_error = true;
			}
			else
			{
				ADSVALUE max_val;
				ADS_ATTR_INFO max_chattrinfo[] = {L"maxPwdAge", ADS_ATTR_UPDATE, ADSTYPE_LARGE_INTEGER, &max_val, 1};
				DWORD max_chnum_attr = sizeof(max_chattrinfo)/sizeof(ADS_ATTR_INFO);

				int max = pwd_max_age_p;
				long ichmax_high, ichmax_low;
				__int64 ichmax_age;
				LARGE_INTEGER chmax_age;

				ichmax_age = -(max+1) * 864000000000;
				ichmax_high = ichmax_age/4294967296;
				ichmax_low = ichmax_age%4294967296;

				chmax_age.HighPart = (ULONG)ichmax_high;
				chmax_age.LowPart = (ULONG)ichmax_low;

				max_val.dwType = ADSTYPE_LARGE_INTEGER;
				max_val.LargeInteger = chmax_age;

				dom->SetObjectAttributes(max_chattrinfo, max_chnum_attr, &num_mod);
			}
		}
		else
		{
			MessageBox(L"The Maximum Password Age must be between 1 and 999 or 0 for Password to never expire", L"Password Policy Manager");
			is_error = true;
		}

		LPWSTR attrnamesmin[] = {L"maxPwdAge"};
		DWORD num_attrmin = sizeof(attrnamesmin)/sizeof(LPWSTR);
		ADS_ATTR_INFO *attrinfomin = NULL;
		DWORD attr_nummin;

		DWORD imin=0;
		LARGE_INTEGER max_age1;
		__int64 imax_age1;
		long imax_high1, imax_low1;
		int pwd_max_age_i;

		hr = dom->GetObjectAttributes(attrnamesmin, num_attrmin, &attrinfomin, &attr_nummin);
		if(SUCCEEDED(hr))
		{
			for(imin=0;imin<attr_nummin;imin++)
			{
				if(_wcsicmp(attrinfomin[imin].pszAttrName, L"maxPwdAge") == 0)
				{							
					max_age1 = attrinfomin[imin].pADsValues->LargeInteger; 							
					
					imax_high1 = max_age1.HighPart;
					imax_low1 = max_age1.LowPart;

					imax_age1 = (ULONG)imax_high1;
					imax_age1 = imax_age1 << 32;
					imax_age1 = imax_age1 + (ULONG)imax_low1;

					imax_age1 = imax_age1/864000000000;
					pwd_max_age_i = -imax_age1;
				}
			}
		}

		int pwd_min_age_p;
		pwd_min_age_p = _wtoi(min.GetString());
		
		if(pwd_min_age_p <= 998 && pwd_min_age_p >= 0)
		{
			if(pwd_max_age_i != 0 && pwd_min_age_p >= pwd_max_age_i)
			{
				MessageBox(L"The Minimum Password Age must be lesser than Maximum Password Age", L"Password Policy Manager");
				is_error = true;
			}
			else
			{
				ADSVALUE min_val;
				ADS_ATTR_INFO min_chattrinfo[] = {L"minPwdAge", ADS_ATTR_UPDATE, ADSTYPE_LARGE_INTEGER, &min_val, 1};
				DWORD min_chnum_attr = sizeof(min_chattrinfo)/sizeof(ADS_ATTR_INFO);

				int min = pwd_min_age_p;
				long ichmin_high, ichmin_low;
				__int64 ichmin_age;
				LARGE_INTEGER chmin_age;

				ichmin_age = -(min+1) * 864000000000;
				ichmin_high = ichmin_age/4294967296;
				ichmin_low = ichmin_age%4294967296;

				chmin_age.HighPart = (ULONG)ichmin_high;
				chmin_age.LowPart = (ULONG)ichmin_low;

				min_val.dwType = ADSTYPE_LARGE_INTEGER;
				min_val.LargeInteger = chmin_age;

				dom->SetObjectAttributes(min_chattrinfo, min_chnum_attr, &num_mod);
				if(SUCCEEDED(hr)){
					if(getInfo()){
						if(getFileDate()==0){
							
							addDb();
						}
						else if(startApp())
							updateDb();
						writeToFile(16);
					}
				}
			}
		}
		else
		{
			MessageBox(L"The Minimum Password Age must be between 1 and 998 or 0 to allow immediate changes in Password", L"Password Policy Manager");
			is_error = true;
		}
	}
	if(!is_error)
	{
	   MessageBox(L"Data Saved Successfully", L"Password Policy Manager");
	}
	dom->Release();
	CoUninitialize();
	}
	else
	{
		MessageBox(L"No data to save",L"Missing Details");
	}
	UpdateData(FALSE);
}

void dialog_pwdpolicy::OnBnClickedDefault()
{
	// TODO: Add your control notification handler code here
	hist.SetString(L"24");
	max.SetString(L"42");
	min.SetString(L"1");
	len.SetString(L"7");
	UpdateData(FALSE);
	rad1.SetCheck(1);
	rad2.SetCheck(0);
	rad3.SetCheck(0);
	rad4.SetCheck(1);
}


HBRUSH dialog_pwdpolicy::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	   if( IDC_STATIC14 == pWnd->GetDlgCtrlID() || IDC_STATIC15 == pWnd->GetDlgCtrlID())
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
	   if(IDC_STATIC12 == pWnd->GetDlgCtrlID()||IDC_STATIC13 == pWnd->GetDlgCtrlID())
	   pDC->SetBkColor(RGB(240, 240 , 240));
	   else
	   pDC->SetBkColor(RGB(255, 255 , 255));
	   return (HBRUSH)GetStockObject(NULL_BRUSH);
       default:
	   return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
	   }
}


BOOL dialog_pwdpolicy::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default
	     CRect r;
         pDC->GetClipBox(&r);
		 pDC->FillSolidRect(r, RGB(244,244,244)); 
         return TRUE;
}
