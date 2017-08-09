// DMZAnalyzerDlg.h : header file
//
#include "stdafx.h"
#include "afxbutton.h"
#pragma once
#include "afxwin.h"
#include "afxcmn.h"
#include "comdef.h"
#include "winsock2.h"
#include <windows.h>
#include <wchar.h>
#include "atlstr.h"
#include <stdlib.h>

#include <rpc.h>
#include <stdio.h>
#include <string.h>
#include <Rpcdce.h>
#include "conio.h"
#include "atlstr.h"
#include "comutil.h"
#include "Ws2tcpip.h"
#include <ws2tcpip.h>
#include "hyperlink.h"
#pragma comment(lib, "rpcrt4.lib")
#pragma comment(lib, "iphlpapi.lib")
#pragma comment(lib, "ws2_32.lib")
//#pragma comment(lib, "MSVCRT.LIB")

#ifndef   NI_MAXHOST
#define   NI_MAXHOST 1025
#endif
#define UNICODE_ONLY
// CDMZAnalyzerDlg dialog
class CDMZAnalyzerDlg : public CDialog
{
// Construction
public:
	CDMZAnalyzerDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_DMZANALYZER_DIALOG };

protected:
		virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
		HICON m_hIcon;
		// Generated message map functions
		virtual BOOL OnInitDialog();
		afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
		afx_msg void OnPaint();
		afx_msg HCURSOR OnQueryDragIcon();
		DECLARE_MESSAGE_MAP()
public:
	
	CString hostOrIPName;					//Variable used to receive the ipaddress from the dialog
	BOOL resolveIPAddress(CString address); //convert host name to ipaddress
	BOOL ListeningPort(int port);			//whether the given port is listening on that ipaddress or not
	BOOL GetRpcDynamicPorts();				//Get the Activedir rpc dynamic ports
	void ErrorReport(RPC_STATUS status);	//rpc error codes 
	void DynamicPorts();					//check dynamic ports status
	void WellDefinePorts(CString);			//check well defined ports status
	
	
	int endPointArray[10];
	CStringArray ListeningResult;
	int nLength;
	SOCKET ConnectSocket;
	char hostname[NI_MAXHOST];
	
	CEdit v_scanResult;
	CHyperLink c_link;						//admanagerplus hyperlink text variable
	CStatic v_status;						//'Querying...' text variable
	CEdit v_hostName;						//Edit box variable
	afx_msg void OnBnClickedButton1();		//On click Find button
		
public:
	CButton home_button;
	CMFCButton m_button1;
	CBitmap home_bitmap;
public:
	afx_msg void OnBnClickedHome();
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
};
