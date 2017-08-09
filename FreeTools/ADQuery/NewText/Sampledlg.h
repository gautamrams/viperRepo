#pragma once


// CSampledlg dialog

class CSampledlg : public CDialog
{
	DECLARE_DYNAMIC(CSampledlg)

public:
	CSampledlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSampledlg();

// Dialog Data
	enum { IDD = IDD_DIALOG2 };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	CString m_samedit3;
	afx_msg void OnBnClickedCancel();
	afx_msg void OnBnClickedOk();
};
