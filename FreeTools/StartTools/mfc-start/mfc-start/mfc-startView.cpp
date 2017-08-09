// mfc-startView.cpp : implementation of the CmfcstartView class
//

#include "stdafx.h"
#include "mfc-start.h"

#include "mfc-startDoc.h"
#include "mfc-startView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CmfcstartView

IMPLEMENT_DYNCREATE(CmfcstartView, CView)

BEGIN_MESSAGE_MAP(CmfcstartView, CView)
END_MESSAGE_MAP()

// CmfcstartView construction/destruction

CmfcstartView::CmfcstartView()
{
	// TODO: add construction code here

}

CmfcstartView::~CmfcstartView()
{
}

BOOL CmfcstartView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CView::PreCreateWindow(cs);
}

// CmfcstartView drawing

void CmfcstartView::OnDraw(CDC* /*pDC*/)
{
	CmfcstartDoc* pDoc = GetDocument();
	ASSERT_VALID(pDoc);
	if (!pDoc)
		return;

	// TODO: add draw code for native data here
}


// CmfcstartView diagnostics

#ifdef _DEBUG
void CmfcstartView::AssertValid() const
{
	CView::AssertValid();
}

void CmfcstartView::Dump(CDumpContext& dc) const
{
	CView::Dump(dc);
}

CmfcstartDoc* CmfcstartView::GetDocument() const // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CmfcstartDoc)));
	return (CmfcstartDoc*)m_pDocument;
}
#endif //_DEBUG


// CmfcstartView message handlers
