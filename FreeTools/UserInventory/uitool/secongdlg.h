#pragma once


// secongdlg dialog

class secongdlg : public CDialog
{
	DECLARE_DYNAMIC(secongdlg)

public:
	secongdlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~secongdlg();

// Dialog Data
	enum { IDD = IDD_FORMVIEW };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
};
