#pragma once
#include "afxwin.h"
#include "label.h"


// MultipleUserAddForm dialog

class MultipleUserAddForm : public CDialog
{
	DECLARE_DYNAMIC(MultipleUserAddForm)

public:
	bool canAdd;
	CString fileName;
	int x;
	MultipleUserAddForm(CWnd* pParent = NULL);
	MultipleUserAddForm(int);
	void FormLoad();
	void FormLoad2();
	virtual ~MultipleUserAddForm();

// Dialog Data
	enum { IDD = IDD_MULTIPLEUSERADD_DIALOG};

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOpenbutton();
	CEdit c_FileNameTextBox;
	afx_msg void OnBnClickedClosebutton();
	afx_msg void OnBnClickedImportbutton();
	void OnOK();

public:
	CLabel c_TitleLabel;
public:
	CLabel c_TitleLabel2;
public:
	afx_msg void OnEnSetfocusFilenametextbox();
public:
	CLabel c_Title3Label;
	
};
