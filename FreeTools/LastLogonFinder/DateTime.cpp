// DateTime.cpp: implementation of the CDateTime class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
//#include "TimeZoneConverter.h"
#include "DateTime.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CDateTime::CDateTime()
{
    m_wMilliseconds = 0;

}



 CDateTime::CDateTime(WORD wYear, WORD wMonth, WORD wDayOfWeek, WORD wDay,
               WORD wHour, WORD wMinute , WORD wSecond, WORD wMilliseconds)
               :m_wYear(wYear), m_wMonth(wMonth), m_wDayOfWeek(wDayOfWeek),
                m_wDay(wDay), m_wHour(wHour), m_wMinute(wMinute), m_wSecond(wSecond)
{

	m_wMilliseconds = 0;

}




CDateTime::~CDateTime()
{



}



void CDateTime::SetDateTime(WORD wYear, WORD wMonth, WORD wDayOfWeek, WORD wDay,
               WORD wHour, WORD wMinute, WORD wSecond,WORD wMilliseconds)
{


        m_wYear     =   wYear;
        m_wMonth    =   wMonth;
        m_wDayOfWeek=   wDayOfWeek;
        m_wDay      =   wDay;
        m_wHour     =   wHour;
        m_wMinute   =   wMinute;
        m_wSecond   =   wSecond;
        m_wMilliseconds = wMilliseconds;


}