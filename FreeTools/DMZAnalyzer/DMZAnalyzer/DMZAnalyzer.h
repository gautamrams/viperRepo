// DMZAnalyzer.h : main header file for the PROJECT_NAME application
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CDMZAnalyzerApp:
// See DMZAnalyzer.cpp for the implementation of this class
//

class CDMZAnalyzerApp : public CWinApp
{
public:
	CDMZAnalyzerApp();

// Overrides
	public:
	virtual BOOL InitInstance();

// Implementation

	DECLARE_MESSAGE_MAP()
};

extern CDMZAnalyzerApp theApp;