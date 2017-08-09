// Password_Policy_Manager.h : main header file for the PROJECT_NAME application
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CPassword_Policy_ManagerApp:
// See Password_Policy_Manager.cpp for the implementation of this class
//

class CPassword_Policy_ManagerApp : public CWinApp
{
public:
	CPassword_Policy_ManagerApp();

// Overrides
	public:
	virtual BOOL InitInstance();

// Implementation

	DECLARE_MESSAGE_MAP()
};

extern CPassword_Policy_ManagerApp theApp;