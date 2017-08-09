// ToolsDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Tools.h"
#include "ToolsDlg.h"
#include "HyperLink.h"
#include ".\toolsdlg.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Implementation
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
END_MESSAGE_MAP()


// CToolsDlg dialog



CToolsDlg::CToolsDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CToolsDlg::IDD, pParent)
{
	//m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_hIcon = AfxGetApp()->LoadIcon(IDI_ICON1);
}

void CToolsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LINK1, c_link1);
	DDX_Control(pDX, IDC_LINK2, c_link2);
	DDX_Control(pDX, IDC_LINK3, c_link3);
	DDX_Control(pDX, IDC_ICON1, c_icon1);
	DDX_Control(pDX, IDC_ICON2, c_icon2);	
	DDX_Control(pDX, IDC_ICON3, c_icon3);	
	DDX_Control(pDX, IDC_ICON4, c_icon4);
	DDX_Control(pDX, IDC_ICON5, c_Icon5);	
	DDX_Control(pDX, IDC_SAM, c_Label1);
	DDX_Control(pDX, IDC_LABEL2, c_label2);
}

BEGIN_MESSAGE_MAP(CToolsDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP		
	ON_STN_CLICKED(IDC_SAM, OnStnClickedSam)
	ON_STN_CLICKED(IDC_LABEL2, &CToolsDlg::OnStnClickedLabel2)
END_MESSAGE_MAP()


// CToolsDlg message handlers

BOOL CToolsDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	LPSTR path1 = getDir();
	strcat(path1,"\\ADQuery.exe");		
	this->c_link1.SetToolTipText("AD Query Tool!");
	this->c_link1.SetURL(path1);	
	this->c_link1.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));
	
	LPSTR path2 = getDir();
	strcat(path2,"\\CSVGenerator.exe");		
	this->c_link2.SetToolTipText("CSV Generator Tool!");
	this->c_link2.SetURL(path2);	
	this->c_link2.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));
	
	LPSTR path3 = getDir();
	strcat(path3,"\\EmptyPasswordChecker.exe");		
	this->c_link3.SetToolTipText("Empty Password Users Report!");
	this->c_link3.SetURL(path3);	
	this->c_link3.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));

	this->c_label2.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));
	this->c_label2.SetFontBold(TRUE);	
	
	this->c_Label1.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));
	this->c_Label1.SetFontBold(TRUE);
			
	this->c_icon1.SetToolTipText("AD Query Tool!");
	this->c_icon1.SetURL(path1);	
	this->c_icon1.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));

	this->c_icon2.SetToolTipText("CSV Generator Tool!");
	this->c_icon2.SetURL(path2);	
	this->c_icon2.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));

	this->c_icon3.SetToolTipText("Empty Password Users Report Tool!");
	this->c_icon3.SetURL(path3);	
	this->c_icon3.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));

	this->c_icon4.SetToolTipText("DC Monitoring Tool!");
	this->c_icon4.SetLinkCursor(AfxGetApp()->LoadCursor(IDC_CURSOR1));


	// TODO: Add extra initialization here	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CToolsDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}
// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CToolsDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}
// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CToolsDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}
LPSTR CToolsDlg::getDir()
{
        LPSTR strCurrDir = NULL;
        strCurrDir = new CHAR[MAX_PATH];		
		if(0 == GetCurrentDirectory(MAX_PATH, strCurrDir))
        {
                if(strCurrDir)
                {
                        free(strCurrDir);
                }                
        }
        return strCurrDir;
}

void CToolsDlg::OnStnClickedSam()
{	
	char *buffer = getenv("windir");
	char  str[256];	
	strcpy(str,buffer);
	strcat(str,"\\System32\\WindowsPowerShell\\V1.0\\powershell.exe");
	
	if(_access(str, 0) == -1)
	{
		MessageBox("\"Powershell\" should be installed in your machine to use this tool.It can be downloaded from the link \n http://www.microsoft.com/technet/scriptcenter/topics/msh/download.mspx", "Tools",MB_OK | MB_ICONINFORMATION);
	}	
	else
	{		
		ShellExecute(0,_T("Open"),_T("ManageLocalUsers.bat"),NULL,NULL,0);		
	}

}

void CToolsDlg::OnStnClickedLabel2()
{
	HINSTANCE hInst;
	hInst = ShellExecute(0,_T("Open"),_T("DCMonitoringTool.exe"),NULL,NULL,0);
		
	if(reinterpret_cast<int>(hInst) == SE_ERR_ACCESSDENIED)
	{
		MessageBox("Before using this tool ensure \"Microsoft Visual C++ 2005 Redistributable\" package exists in your system.It can be downloaded from the link \n http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=32bc1bee-a3f9-4c13-9c99-220b62a191ee", "Tools",MB_OK | MB_ICONINFORMATION);
	}
}
