// Tools.h : main header file for the PROJECT_NAME application
//

#pragma once

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols


// CToolsApp:
// See Tools.cpp for the implementation of this class
//

class CToolsApp : public CWinApp
{
public:
	CToolsApp();

// Overrides
	public:
	virtual BOOL InitInstance();

// Implementation

	DECLARE_MESSAGE_MAP()
};

extern CToolsApp theApp;