// mfc-startView.h : interface of the CmfcstartView class
//


#pragma once


class CmfcstartView : public CView
{
protected: // create from serialization only
	CmfcstartView();
	DECLARE_DYNCREATE(CmfcstartView)

// Attributes
public:
	CmfcstartDoc* GetDocument() const;

// Operations
public:

// Overrides
public:
	virtual void OnDraw(CDC* pDC);  // overridden to draw this view
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
protected:

// Implementation
public:
	virtual ~CmfcstartView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in mfc-startView.cpp
inline CmfcstartDoc* CmfcstartView::GetDocument() const
   { return reinterpret_cast<CmfcstartDoc*>(m_pDocument); }
#endif

