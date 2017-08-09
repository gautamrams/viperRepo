// mfc-start.h : main header file for the mfc-start application
//
#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"       // main symbols


// CmfcstartApp:
// See mfc-start.cpp for the implementation of this class
//

class CmfcstartApp : public CWinApp
{
public:
	CmfcstartApp();


// Overrides
public:
	virtual BOOL InitInstance();

// Implementation
	afx_msg void OnAppAbout();
	DECLARE_MESSAGE_MAP()
};

extern CmfcstartApp theApp;