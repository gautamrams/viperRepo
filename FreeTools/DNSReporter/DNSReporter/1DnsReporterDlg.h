
// 1DnsReporterDlg.h : header file
//

#pragma once


// CMy1DnsReporterDlg dialog
class CMy1DnsReporterDlg : public CDialogEx
{
// Construction
public:
	CMy1DnsReporterDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_MY1DNSREPORTER_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	int ngethostbyname(unsigned char*);
	int InsertColumn(int nCol, const LVCOLUMN* pColumn);
	LONG InsertItem(int nItem,TCITEM* pTabCtrlItem); 
	int InsertItem(const LVITEM* pItem);
	BOOL SetItemText(int nItem,int nSubItem,LPCTSTR lpszText);
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()

public:
	CBitmap bitmap_home;

public:
	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedButton1();
	afx_msg void OnCbnSelchangeCombo1();
	CTabCtrl m_paramaterTab;
	CListCtrl m_paramaterList;
	CComboBox m_domainNames;
	int cursel;
	//extern vector<string> a1,a2,ns1,ns2,srv1,srv2;
	afx_msg void OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnClickedButton1();
	afx_msg void OnSelchangeCombo1();
	afx_msg void OnNMClickSyslink2(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnClickedButton4();
	CButton m_buttonhome;
	CMFCButton m_button1;
	CMFCButton m_button2;
	afx_msg void OnBnClickedButton5();
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnBnClickedButton2();
};
