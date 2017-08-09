// NewTimeZoneInfoManager.h: interface for the CTimeZoneInfoManager class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_TIMEZONEINFOMANAGER_H__FCEFF479_92F2_446E_BFA3_15800E129560__INCLUDED_)
#define AFX_TIMEZONEINFOMANAGER_H__FCEFF479_92F2_446E_BFA3_15800E129560__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
#include <afxtempl.h>
#include "DateTime.h"
#define MAX_SIZE    255
/*class CRegTimeZoneInfo
{

public:
    FillFromRegistry(int i_RegistryIndex, BYTE* i_pValue, CString i_strValueName);

public:
    TCHAR m_szDisplay[100];
    TCHAR m_szDlt[100];
    int m_iIndex;
    TCHAR m_szMapID[100];
    TCHAR m_szStd[100];

};*/
struct regTZI
{
    long Bias;
    long StandardBias;
    long DaylightBias;
    SYSTEMTIME StandardDate; 
    SYSTEMTIME DaylightDate;
};


class CRegTimeZoneInfo
{

public:

    TCHAR tcName[MAX_SIZE];
    TCHAR m_szDisplay[MAX_SIZE];
    TCHAR m_szDlt[MAX_SIZE];
    TCHAR m_szStd[MAX_SIZE];
    TCHAR m_szMapID[MAX_SIZE];
    DWORD m_iIndex;
    DWORD ActiveTimeBias;
    regTZI m_regTZI;

public:

void    FillFromRegistry(int i_RegistryIndex, BYTE* i_pValue, CString i_strValueName, DWORD i_ValueSize);
};

class CTimeZoneInfoManager  
{
public:
    CTimeZoneInfoManager();
    virtual ~CTimeZoneInfoManager();

//operations
    int EnumerateTimeZones();
    int GetFullTimeZoneInfoFromRegistry();
    int ConvertFromLocalToUTC(CDateTime* i_LocalTime, int i_TimeZoneIndex, CDateTime* o_UTCTime);
    //int ConvertFromUTCToLocal(CDateTime* i_UTCTime, int i_TimeZoneIndex, CDateTime* o_LocalTime);
    int SpecificLocalTimeToSystemTime(SYSTEMTIME* i_stLocal, SYSTEMTIME* o_stUniversal);
    int GetCurrentTimeZone();
    int SortTimeZoneList();

//attributes
public:
    CStringArray    m_arrTimeZones;
    CArray<CRegTimeZoneInfo*, CRegTimeZoneInfo*> m_arrRegTimeZoneInfo;

};

#endif // !defined(AFX_TIMEZONEINFOMANAGER_H__FCEFF479_92F2_446E_BFA3_15800E129560__INCLUDED_)









