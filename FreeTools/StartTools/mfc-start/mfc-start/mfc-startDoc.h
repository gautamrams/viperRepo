// mfc-startDoc.h : interface of the CmfcstartDoc class
//


#pragma once


class CmfcstartDoc : public CDocument
{
protected: // create from serialization only
	CmfcstartDoc();
	DECLARE_DYNCREATE(CmfcstartDoc)

// Attributes
public:

// Operations
public:

// Overrides
public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);

// Implementation
public:
	virtual ~CmfcstartDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	DECLARE_MESSAGE_MAP()
};


