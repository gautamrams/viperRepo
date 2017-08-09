// DateTime.h: interface for the CDateTime class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_DATETIME_H__894ED690_27B3_4970_85BC_6A5735DA197A__INCLUDED_)
#define AFX_DATETIME_H__894ED690_27B3_4970_85BC_6A5735DA197A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class CDateTime  
{
public:
	CDateTime();

    CDateTime(WORD wYear, WORD wMonth, WORD wDayOfWeek, WORD wDay,
               WORD wHour, WORD wMinute =0, WORD wSecond = 0,WORD wMilliseconds =0);
	virtual ~CDateTime();

//operations
public:
void     SetDateTime(WORD wYear, WORD wMonth, WORD wDayOfWeek, WORD wDay,        WORD wHour, WORD wMinute =0, WORD wSecond = 0,WORD wMilliseconds =0);

//attributes
public:

    WORD m_wYear;
    WORD m_wMonth;
    WORD m_wDayOfWeek;
    WORD m_wDay;
    WORD m_wHour;
    WORD m_wMinute; 
    WORD m_wSecond;
    WORD m_wMilliseconds;


};

#endif // !defined(AFX_DATETIME_H__894ED690_27B3_4970_85BC_6A5735DA197A__INCLUDED_)
