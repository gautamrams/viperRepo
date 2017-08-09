#pragma once
#include "afxwin.h"
#include "afxtempl.h"

const CString useratt[]={"userPrincipalName","objectGUID","sAMAccountName","accountExpires","badPwdCount","codePage","countryCode","homeDirectory","homeDrive","mail","profilePath","scriptPath","userAccountControl","userSharedFolder","userWorkStations","maxStorage","c","co","notes","department","fascimileTelephoneNumber","givenName","homePhone","initials","ipPhone","l","manager","mobile","otherIpPhone","pager","physicalDeliveryOfficeName","postalAddress","postalCode","postOfficeBox","sn","st","street","telephoneNumber","title","wWWHomePage","userAccountControl"};
const CString userattNam[] = {"userPrincipalName","objectGUID","sAMAccountName"};
const CString userattSec[] = {"accountExpires","badPwdCount","codePage","countryCode","homeDirectory","homeDrive","mail","profilePath","scriptPath","userAccountControl","userSharedFolder","userWorkStations","maxStorage"};
const CString userattAdd[] = {"c","co","notes","department","fascimileTelephoneNumber","givenName","homePhone","initials","ipPhone","l","manager","mobile","otherIpPhone","pager","physicalDeliveryOfficeName","postalAddress","postalCode","postOfficeBox","sn","st","street","telephoneNumber","title","wWWHomePage"};

const CString useratt1[]={"User Principal Name","Object GUID","SAM Account Name","Account Expires","Bad Password Count","Code Page","Country Code","Home Directory","Home Drive","E-Mail","Profile Path","Script Path","User Account Control","User Shared Folder","User WorkStations","Max Storage","Home Country","Present Country","Notes","Department","Fascimile Telephone Number","First Name","Home Phone","Initials","Ip Phone","Locality","Manager","Mobile","Other Ip Phone","Pager","Physical Delivery OfficeName","Postal Address","Postal Code","PostOffice Box","Last Name","State","Street","Telephone Number","Title","WWWHomePage","Account Status"};
const CString useratt1Nam[]={"User Principal Name","Object GUID","SAM Account Name"};
const CString useratt1Sec[]={"Account Expires","Bad Password Count","Code Page","Country Code","Home Directory","Home Drive","E-mail","Profile Path","Script Path","User Account Control","User Shared Folder","User WorkStations","Max Storage"};
const CString useratt1Add[]={"Home Country","Present Country","Notes","Department","Fascimile Telephone Number","First Name","Home Phone","Initials","Ip Phone","Locality","Manager","Mobile","Other Ip Phone","Pager","Physical Delivery OfficeName","Postal Address","Postal Code","PostOffice Box","Last Name","State","Street","Telephone Number","Title","WWWHomePage"};

const CString conatt[]={"givenName","initials","sn","telephoneNumber","homePhone","pager","mobile","physicalDeliveryOfficeName","manager","title","company","department","info","streetAddress","postOfficeBox","l","st","postalCode","countryCode","managedBy"};
const CString conattNam[]={"givenName","initials","sn"};
const CString conattCN[]={"telephoneNumber","homePhone","pager","mobile"};
const CString conattOD[]={"physicalDeliveryOfficeName","manager","title","company","department","info"};
const CString conattAB[]={"streetAddress","postOfficeBox","l","st","postalCode","countryCode","managedBy"};

const CString conatt1[]={"First Name","Initials","Last Name","Telephone Number","Home Phone","Pager","Mobile","PhysicalDelivery OfficeName","Manager","Title","Company","Department","Notes","Street","PostOfficeBox","City","State","Postal Code","CountryCode","ManagedBy"};
const CString conatt1Nam[]={"First Name","Initials","Last Name"};
const CString conatt1CN[]={"Telephone Number","Home Phone","Pager","Mobile"};
const CString conatt1OD[]={"PhysicalDelivery OfficeName","Manager","Title","Company","Department","Notes"};
const CString conatt1AB[]={"Street","PostOfficeBox","City","State","Postal Code","CountryCode","ManagedBy"};


const CString comatt[]={"userPrincipalName","dNSHostName","accountExpires","AdminCount","badPwdCount","codePage","homeDirectory","homeDrive","profilePath","location","logonWorkstation","managedBy","operatingSystem","operatingSystemVersion","operatorCount","volumeCount","catalogs"};
const CString comattNam[]={"userPrincipalName","dNSHostName"};
const CString comattSec[]={"accountExpires","AdminCount","badPwdCount","codePage","homeDirectory","homeDrive","profilePath","location","logonWorkstation","managedBy"};
const CString comattOS[]={"operatingSystem","operatingSystemVersion","operatorCount","volumeCount","catalogs"};

const CString comatt1[]={"User Principal Name","DNS HostName","Account Expires","Admin Count","Bad Password Count","CodePage","Home Directory","Home Drive","Profile Path","Location","Logon Workstation","ManagedBy","OperatingSystem","OperatingSystem Version","Operator Count","Volume Count","Catalogs"};
const CString comatt1Nam[]={"User Principal Name","DNS HostName"};
const CString comatt1Sec[]={"Account Expires","Admin Count","Bad Password Count","CodePage","Home Directory","Home Drive","Profile Path","Location","Logon Workstation","ManagedBy"};
const CString comatt1OS[]={"OperatingSystem","OperatingSystem Version","Operator Count","Volume Count","Catalogs"};

const CString groupatt[]={"accountNameHistory","adminCount","adminDescription","adminDisplayName","groupType","extensionName","mail","sAMAccountName","groupMembershipSAM","objectGUID","wWWHomePage","managedBy","info","objectCategory","objectClass","objectVersion","operatorCount"};
const CString groupattNam[]={"accountNameHistory","adminCount","adminDescription","adminDisplayName","groupType","extensionName","mail","sAMAccountName","groupMembershipSAM","objectGUID"};
const CString groupattGen[]={"wWWHomePage","managedBy","info","objectCategory","objectClass","objectVersion","operatorCount"};

const CString groupatt1[]={"AccountName History","Admin Count","Admin Description","Admin DisplayName","Group Type","Extension Name","E-mail","SAM AccountName","GroupMembership SAM","Object GUID","WWW HomePage","Managed By","Notes","Object Category","Object Class","Object Version","Operator Count"};
const CString groupatt1Nam[]={"AccountName History","Admin Count","Admin Description","Admin DisplayName","Group Type","Extension Name","E-mail","SAM AccountName","GroupMembership SAM","Object GUID"};
const CString groupatt1Gen[]={"WWW HomePage","Managed By","Notes","Object Category","Object Class","Object Version","Operator Count"};

// CSecondDlg dialog

class CSecondDlg : public CDialog
{
	DECLARE_DYNAMIC(CSecondDlg)
	unsigned int n;
public:
	CSecondDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSecondDlg();

// Dialog Data
	enum { IDD = IDD_SECOND_DLG };
protected:
	virtual BOOL OnInitDialog();
	void resetcontent();
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	CListBox lbox1;
	CListBox lbox2;
	afx_msg void OnBnClickedButton1();
	afx_msg void OnBnClickedButton2();
	afx_msg void OnBnClickedButton3();	
	CListBox lbox3;
	CListBox lbox4;
	CComboBox cbox1;
	afx_msg void OnCbnSelchangeCombo1();
	
	CButton c_ConfirmChkBox;
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
};
