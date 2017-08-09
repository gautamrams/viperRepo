#pragma once


// CDialogDlg dialog

class CDialogDlg : public CDialog
{
	DECLARE_DYNAMIC(CDialogDlg)

public:
	CDialogDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CDialogDlg();

// Dialog Data
	enum { IDD = IDD_NEWTEXT_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
};
