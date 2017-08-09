// MainFrm.cpp : implementation of the CMainFrame class
//

#include "stdafx.h"
#include "mfc-start.h"

#include "MainFrm.h"

#include "winbase.h"
#include "shellapi.h"
#include <string>
#include "atlconv.h"
#include <stdio.h>
#include <tchar.h>
#include <objbase.h>


#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CMainFrame

IMPLEMENT_DYNCREATE(CMainFrame, CFrameWnd)

BEGIN_MESSAGE_MAP(CMainFrame, CFrameWnd)
END_MESSAGE_MAP()


// CMainFrame construction/destruction

CMainFrame::CMainFrame()
{
	// TODO: add member initialization code here
}

CMainFrame::~CMainFrame()
{
}


BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
//	MessageBox(L"PreCreateWindow",L"nan than",0);

	 TCHAR szPath[MAX_PATH];
     TCHAR nonetpath[MAX_PATH];
	TCHAR pshellpath[MAX_PATH];
	lstrcpy(pshellpath, TEXT("RegisterCmdlet.bat"));
   lstrcpy(nonetpath, TEXT("ADTools.exe"));
	lstrcpy(szPath, TEXT("FreeTool.exe"));

	STARTUPINFO si = { sizeof(si) };
	si.dwFlags=STARTF_USESHOWWINDOW;
	si.wShowWindow=SW_HIDE;
   SECURITY_ATTRIBUTES saProcess,saThread;
   PROCESS_INFORMATION piProcessB;
  
   saProcess.nLength = sizeof(saProcess);
   saProcess.lpSecurityDescriptor = NULL;
   saProcess.bInheritHandle = TRUE;

	saThread.nLength = sizeof(saThread);
   saThread.lpSecurityDescriptor = NULL;
   saThread.bInheritHandle = FALSE;
//checking Framework installed or not 
   char *buffer = getenv("windir");
   wchar_t ldappath2[300];
   wchar_t *pathptr = ldappath2; 
	wchar_t *pathptr1 = ldappath2; 	
   size_t ret=mbstowcs(pathptr,buffer,300);
   
   ///////////////////////////////////////////////
 HKEY   hkey;
 DWORD  dwDisposition;
 
 if( RegOpenKeyEx(HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\Microsoft\\.NETFramework\\policy\\v2.0"),0, KEY_QUERY_VALUE, &hkey)==ERROR_SUCCESS ) 
 {
	 CreateProcess(NULL, szPath, &saProcess, &saThread,FALSE, 0, NULL, NULL, &si, &piProcessB);		
	 CreateProcess(NULL, pshellpath, &saProcess, &saThread,FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &si, &piProcessB);
	 CloseHandle(piProcessB.hProcess);
	 CloseHandle(piProcessB.hThread);
 }
 else if( RegOpenKeyEx(HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\Microsoft\\.NETFramework\\policy\\v4.0"),0, KEY_QUERY_VALUE, &hkey)==ERROR_SUCCESS ) 
 {
	 CreateProcess(NULL, szPath, &saProcess, &saThread,FALSE, 0, NULL, NULL, &si, &piProcessB);		
	 CreateProcess(NULL, pshellpath, &saProcess, &saThread,FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &si, &piProcessB);
	 CloseHandle(piProcessB.hProcess);
	 CloseHandle(piProcessB.hThread);
 }
 else
 {
	 MessageBox(L"Please install Framework v2.0 or above",L"FreeTool Information");
 }

   ////////////////////////////////////////////////
 
	if( !CFrameWnd::PreCreateWindow(cs) )
		return FALSE;
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	cs.style = WS_OVERLAPPED | WS_CAPTION | FWS_ADDTOTITLE;
	exit(0);
	return TRUE;
}


// CMainFrame diagnostics

#ifdef _DEBUG
void CMainFrame::AssertValid() const
{
	CFrameWnd::AssertValid();
}

void CMainFrame::Dump(CDumpContext& dc) const
{
	CFrameWnd::Dump(dc);
}

#endif //_DEBUG


// CMainFrame message handlers



