// TestHtmlDlgDlg.h : header file
//

#pragma once
#include "winuser.h"

// CTestHtmlDlgDlg dialog
class CTestHtmlDlgDlg : public CDHtmlDialog
{
// Construction
public:
	CTestHtmlDlgDlg(CWnd* pParent = NULL);	// standard constructor

	CJpegFrame		m_TopBand,m_Admp,m_AdAudit,m_AdSelf,m_Also;
	CHyperlink1		m_TopLink,m_Admp_link,m_AdAudit_link,m_AdSelf_link;
	/*void	setURL(CHyperlink1 &ctr, int id,TCHAR *buffer);
	void	setURL(CHyperlink1 &ctr, int id);*/

// Dialog Data
	enum { IDD = IDD_TESTHTMLDLG_DIALOG, IDH = IDR_HTML_TESTHTMLDLG_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support

	HRESULT OnButtonOK(IHTMLElement *pElement);
	HRESULT OnButtonCancel(IHTMLElement *pElement);	
	HRESULT OnEmpty(IHTMLElement *pElement);
	HRESULT OnCSV(IHTMLElement *pElement);
	HRESULT OnADQuery(IHTMLElement *pElement);
	HRESULT OnDC(IHTMLElement *pElement);
	HRESULT OnLastLogonFinder(IHTMLElement *pElement);
	HRESULT OnPasswordPolicyManager(IHTMLElement *pElement);
	HRESULT OnDMZAnalyser(IHTMLElement *pElement);
	HRESULT OnDNSReporter(IHTMLElement *pElement);
	HRESULT OnLocalUserManagement(IHTMLElement *pElement);

	HRESULT OnPowerShell(IHTMLElement *pElement);
	HRESULT OnPowerShell2(IHTMLElement *pElement);
	HRESULT OnHelp(IHTMLElement *pElement);
	HRESULT OnA1(IHTMLElement *pElement);
	HRESULT OnA2(IHTMLElement *pElement);
	HRESULT OnA3(IHTMLElement *pElement);


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
	DECLARE_DHTML_EVENT_MAP()
public:
	afx_msg void OnNMCustomdrawSlider1(NMHDR *pNMHDR, LRESULT *pResult);
	CString GetAppPath();
	CString GetAppPath1();
public:
	afx_msg void OnStnClickedAdmp2();
};
