#pragma once
#include "afxwin.h"


// CNewReadMe dialog

class CNewReadMe : public CDialog
{
	DECLARE_DYNAMIC(CNewReadMe)

public:
	CNewReadMe(CWnd* pParent = NULL);   // standard constructor
	virtual ~CNewReadMe();

// Dialog Data
	enum { IDD = IDD_DIALOG2 };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	CEdit c_EditReadMe;
	CEdit c_edit;
};
