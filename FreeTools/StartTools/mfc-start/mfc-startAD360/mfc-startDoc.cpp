// mfc-startDoc.cpp : implementation of the CmfcstartDoc class
//

#include "stdafx.h"
#include "mfc-start.h"

#include "mfc-startDoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CmfcstartDoc

IMPLEMENT_DYNCREATE(CmfcstartDoc, CDocument)

BEGIN_MESSAGE_MAP(CmfcstartDoc, CDocument)
END_MESSAGE_MAP()


// CmfcstartDoc construction/destruction

CmfcstartDoc::CmfcstartDoc()
{
	// TODO: add one-time construction code here

}

CmfcstartDoc::~CmfcstartDoc()
{
}

BOOL CmfcstartDoc::OnNewDocument()
{
	if (!CDocument::OnNewDocument())
		return FALSE;

	// TODO: add reinitialization code here
	// (SDI documents will reuse this document)

	return TRUE;
}




// CmfcstartDoc serialization

void CmfcstartDoc::Serialize(CArchive& ar)
{
	if (ar.IsStoring())
	{
		// TODO: add storing code here
	}
	else
	{
		// TODO: add loading code here
	}
}


// CmfcstartDoc diagnostics

#ifdef _DEBUG
void CmfcstartDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CmfcstartDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG


// CmfcstartDoc commands
