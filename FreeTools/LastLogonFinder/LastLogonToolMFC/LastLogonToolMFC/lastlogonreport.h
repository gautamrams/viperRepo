#pragma once

#if !defined(AFX_TIMEZONECONVERTERDLG_H__D86A89B4_958E_4D60_BE71_73F226346D10__INCLUDED_)
#define AFX_TIMEZONECONVERTERDLG_H__D86A89B4_958E_4D60_BE71_73F226346D10__INCLUDED_
#endif

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
#include "stdafx.h"
#include "afxdtctl.h"
#include "afxcmn.h"
#include "LastLogonToolMFCDlg.h"
#include "label.h"
#include "afxwin.h"
#include "DateTime.h"
#include "RegistryManager.h"
#include "hyperlink.h"


//#include <Cdate.h>
// lastlogonreport dialog

class lastlogonreport : public CDialog
{
	DECLARE_DYNAMIC(lastlogonreport)

public:
	lastlogonreport(CWnd* pParent = NULL);   // standard constructor
	virtual ~lastlogonreport();
// Dialog Data
	enum { IDD = IDD_DIALOG2 };
	void setDistinguishedNames(CString);
	void setsAMAccountNames(CString);
	void setDomainControllerNames(CString);
	void setColumnHeader(CString);
	int AttributeSelection(int);
protected:
	CBitmapButton button1;
	virtual void DoDataExchange(CDataExchange* pDX); 
	void InsertLastLogonDateColumns(int index,int width);// DDX/DDV support
	void LastLogonInformation ();
	CString FindLastLogonDate(IDirectoryObject*,int ,int,LPWSTR*);
	int		m_iTimeFormat;
	int		m_iLocalToUTC;
	virtual BOOL OnInitDialog();
	DECLARE_MESSAGE_MAP()
public:

	CDateTime m_currLocalDateTime;
    CDateTime m_currUTCDateTime;
    int       m_iCurrentTimeZoneIndex;
	VARIANT m_varYear, m_varMonth, m_varDayOfWeek, m_varDay, m_varHour, m_varMinute, m_varSecond;
	CRegistryManager        m_objRegistryManager;
    CTimeZoneInfoManager    m_objTimeZoneInfoManager;
	int PopulateTimeZones();
	void SetTimezoneResult(CString);
	CListCtrl v_lastLogonDate;
	CLabel c_p3Label3;
	afx_msg void OnBnClickedClose();
	afx_msg void OnSize(UINT ,int , int  );
	afx_msg void OnCbnDropdownCombo1();
	afx_msg void OnCbnSetfocusCombo1();
	afx_msg void OnCbnEditchangeCombo1();
	CHyperLink c_p3link3;
	CStatic c_timezoneresult;
	afx_msg void OnBnClickedButton1();
	//CComboBox c_timezoneenable;
	CComboBox c_timezoneenablenew;
	afx_msg void OnCbnSelchangeListofTimezonesnew2();
	CStatic c_labeltimezone;

	
};
