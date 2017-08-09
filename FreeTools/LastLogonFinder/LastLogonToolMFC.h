// LastLogonToolMFC.h : main header file for the PROJECT_NAME application
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols
#include "RegistryManager.h"
#include "NewTimeZoneInfoManager.h"


// CLastLogonToolMFCApp:
// See LastLogonToolMFC.cpp for the implementation of this class
//

class CLastLogonToolMFCApp : public CWinApp
{
public:
	CLastLogonToolMFCApp();

// Overrides
public:
	virtual BOOL InitInstance();
	CRegistryManager        m_objRegistryManager;
    CTimeZoneInfoManager    m_objTimeZoneInfoManager;

// Implementation

	DECLARE_MESSAGE_MAP()
};

extern CLastLogonToolMFCApp theApp;