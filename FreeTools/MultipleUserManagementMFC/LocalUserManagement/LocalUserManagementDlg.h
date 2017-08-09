// LocalUserManagementDlg.h : header file
//

#pragma once
#include "afxwin.h"
#include "afxcmn.h"
#include "hyperlink.h"



#include<list>
#include "label.h"
using namespace std;


// CLocalUserManagementDlg dialog
class CLocalUserManagementDlg : public CDialog
{
// Construction
public:
	
	list<CString> AllComputersList;
	list<CString> AllComputersLocationList;
	list<CString> ComputerList;
	list<int> ComputerListIndex;
	list<list<list<CString>>> UserList;
	list<list<CString>> SearchList;
	CString UserName;
	CString Password;
	CString DomainName;
	bool defaultUser ;
	bool toBeStopped;
	bool isThreadGoing;

	CLocalUserManagementDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_LOCALUSERMANAGEMENT_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CComboBox c_DomainName;
	afx_msg void OnBnClickedButton1();
	CEdit c_UserName;
	CEdit c_Password;
	CListCtrl c_ListView1;
	afx_msg void OnBnClickedLocalusersbutton();
	void OnOK();
	void RetrieveLocalUserDetails();
	void CredentialsGroupBoxShow();
	void CredentialsGroupBoxHide();
	void MainFormLoad();
	void ComputersGroupBoxShow();
	void ComputersGroupBoxHide();
	void LocalUsersGroupBoxShow();
	void LocalUsersGroupBoxHide();
	void FilterGroupBoxShow();
	void FilterGroupBoxHide();
	void InformationGroupBoxShow();
	void InformationGroupBoxHide();
	void SingleOperationsGroupBoxShow();
	void SingleOperationsGroupBoxHide();
	void MultipleOperationsGroupBoxShow();
	void MultipleOperationsGroupBoxHide();
	void DisplayAllUsers();
	void DisplaySingleComputerUsers(int index);
	void DisplayGroupUsers(list<CString> tempList);
	void RefreshListView();
	void UncheckAll();
	void SortingListView();
	CListCtrl c_ListView2;
	CComboBox c_ComputerCombo;
	CComboBox c_GroupCombo;
	afx_msg void OnBnClickedPropertiesbutton();
	afx_msg void OnCbnSelchangeCombo2();
	afx_msg void OnCbnSelchangeCombo3();
	afx_msg void OnBnClickedResetpasswordbutton();
	afx_msg void OnStnClickedPicturecontrol1();
	CStatic c_Picture1;
	afx_msg void OnStnClickedPicturecontrol2();
	afx_msg void OnBnClickedAddbutton();
	afx_msg void OnBnClickedEnablebutton();
	afx_msg void OnBnClickedDisablebutton();
	afx_msg void OnBnClickedDeletebutton();
	afx_msg void OnBnClickedStopbutton();

	CStatic c_InfoLabel;
	CButton c_StopButton;
	CProgressCtrl c_Progress1;
	CButton c_LocalUsersButton;
	CStatic c_Info2Label;
	CProgressCtrl c_Progress2;
	CButton c_Stop2Button;
	afx_msg void OnBnClickedStop2button();
	CStatic c_ComputerLabel;
	CStatic c_GroupLabel;
	CButton c_PropertiesButton;
	CButton c_ResetPasswordButton;
	CButton c_AddButton;
	CButton c_DeleteButton;
	CButton c_EnableButton;
	CButton c_DisableButton;
	CStatic c_TotalComputersLabel;
	afx_msg void OnBnClickedClearbutton();
	CStatic c_SearchPictureBox;
	afx_msg void OnStnClickedSearchpicturebox();
	CEdit c_SearchTextBox;
	CStatic c_ReportLabel;
	CHyperLink c_LinkLabel;
	CLabel c_Info1Label;
	CLabel c_Information2Label;
	CLabel c_Information3Label;
	CLabel c_Information4Label;
	CStatic c_Picture2;
	int c_Picture3;
	CStatic c_Picture4;
	CStatic c_Picture5;
	afx_msg void OnStnClickedHomepicturecontrol();
	CButton c_SelectAll1Button;
	CButton c_SelectAll2Button;
	CButton c_DeSelectAll1Button;
	CButton c_DeSelectAll2Button;
	afx_msg void OnBnClickedSelectall1button();
	afx_msg void OnBnClickedDeselectall1button();
	afx_msg void OnBnClickedSelectall2button();
	afx_msg void OnBnClickedDeselectall2button();
	CStatic c_TotalUsersLabel;
public:
	CEdit c_Search2TextBox;
public:
	CStatic c_Search2PictureBox;
public:
	afx_msg void OnStnClickedSearch2picturebox();
public:
	afx_msg void OnBnClickedExport();
	afx_msg void OnBnClickedImport();
	afx_msg void OnBnClickedDeselectall2button2();
	afx_msg void OnLvnItemchangedList1(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnEnChangeImporttextbox();
	afx_msg void OnBnClickedImport2();
	afx_msg void OnBnClickedBrowse();
	afx_msg void OnEnChangeEdit3();
	afx_msg void OnStnClickedUsernamelabel2();
	afx_msg void OnNMOutofmemoryHotkey1(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnBnClickedButton3();
	afx_msg void OnStnClickedInfo1label();
	afx_msg void OnBnClickedGroup2();
};
