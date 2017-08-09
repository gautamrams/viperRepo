// TimeZoneInfoManager.cpp: implementation of the CTimeZoneInfoManager class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "LastLogonToolMFC.h"
#include "NewTimeZoneInfoManager.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//#define _XP_OR_LATER
extern int g_IsWindowsXPorLater();

///Global functions///////////////////////////////////////////////
int TimeZoneComparer(const void *i_TZ1, const void *i_TZ2)
{

    CRegTimeZoneInfo* pTZ1 = *((CRegTimeZoneInfo**)(i_TZ1));
    CRegTimeZoneInfo* pTZ2 = *((CRegTimeZoneInfo**)(i_TZ2));

    int biasDifference = pTZ2->m_regTZI.Bias - pTZ1->m_regTZI.Bias ;

    if (0 != biasDifference)
    {

        return biasDifference; 
    }
    else
    {
        return (_tcscmp(pTZ1->m_szStd, pTZ2->m_szStd));

    }


}



///////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CTimeZoneInfoManager::CTimeZoneInfoManager()
{
    EnumerateTimeZones();
}



CTimeZoneInfoManager::~CTimeZoneInfoManager()
{
	//cleanup
	for (int cnt = 0; cnt < m_arrRegTimeZoneInfo.GetSize(); cnt++)
	{
		CRegTimeZoneInfo* pobjRegTimeZoneInfo = m_arrRegTimeZoneInfo[cnt];
		delete pobjRegTimeZoneInfo;
		pobjRegTimeZoneInfo = NULL;
	}

}



/*
1. Open the registry key : "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones
2. Read all subkeys
3. Fill all the TimeZone Structures.
*/
int CTimeZoneInfoManager::EnumerateTimeZones()
{

    //1.Open the registry key "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones"
    TCHAR szTimeZoneKey[] = _T("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones");

    CLastLogonToolMFCApp* pApp = (CLastLogonToolMFCApp*)AfxGetApp();
    CRegistryManager* pRegistryManager = (CRegistryManager*)(&(pApp->m_objRegistryManager));

    HKEY hTimeZones;
    pRegistryManager->OpenRegistryKey(HKEY_LOCAL_MACHINE, szTimeZoneKey, 0, KEY_READ, &hTimeZones);

    //2.Read all subkeys
    pRegistryManager->EnumerateSubKeys(hTimeZones, &m_arrTimeZones);

    //3. Fill all the TimeZone Structures.
    GetFullTimeZoneInfoFromRegistry();

    SortTimeZoneList();

    return 0;
}




int CTimeZoneInfoManager::GetFullTimeZoneInfoFromRegistry() 
{
    TCHAR szParentTimeZoneKey[] = _T("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones");

    CLastLogonToolMFCApp* pApp = (CLastLogonToolMFCApp*)AfxGetApp();
    CRegistryManager* pRegistryManager = (CRegistryManager*)(&(pApp->m_objRegistryManager));

    //Prepare key
    CString szChildTimeZoneKey;

    for (int cnt = 0; cnt < m_arrTimeZones.GetSize(); cnt++)
    {
        szChildTimeZoneKey.Format( _T("%s\\%s"), szParentTimeZoneKey, m_arrTimeZones[cnt]);

        //open child key
        HKEY hTimeZoneKey;
        pRegistryManager->OpenRegistryKey(HKEY_LOCAL_MACHINE, szChildTimeZoneKey, 0, KEY_READ, &hTimeZoneKey);

        //enumreate all value names
        CStringArray keyValueNamesArray;
        pRegistryManager->EnumerateKeyValueNames(hTimeZoneKey, &keyValueNamesArray);

        DWORD valueType, valueSize;
        BYTE value[512];
        
        CRegTimeZoneInfo* pobjRegTimeZoneInfo = new CRegTimeZoneInfo;
        ZeroMemory(pobjRegTimeZoneInfo, sizeof(CRegTimeZoneInfo));
        ZeroMemory((BYTE*)&pobjRegTimeZoneInfo->m_regTZI , sizeof(regTZI));

        for (int valname_cnt = 0; valname_cnt < keyValueNamesArray.GetSize(); valname_cnt++)
        {
            if (!keyValueNamesArray[valname_cnt].CompareNoCase(_T("TZI")))
            {
                valueSize = sizeof(regTZI);
            }
            else
            {
                valueSize = 512;
            }

             //now get the values and fill the structures
            pRegistryManager->QueryReistryValue(hTimeZoneKey, keyValueNamesArray[valname_cnt], NULL,
                                        &valueType, value, &valueSize);
                

            pobjRegTimeZoneInfo->FillFromRegistry(valname_cnt, value, keyValueNamesArray[valname_cnt], valueSize);

            
        }

        m_arrRegTimeZoneInfo.Add(pobjRegTimeZoneInfo);

    }

    return 0;
}



int CTimeZoneInfoManager::SortTimeZoneList()
{

    qsort(m_arrRegTimeZoneInfo.GetData(), m_arrRegTimeZoneInfo.GetSize(), 4, TimeZoneComparer);
   
    return 0;
}


int CTimeZoneInfoManager::GetCurrentTimeZone()
{

        TIME_ZONE_INFORMATION tzi;
    GetTimeZoneInformation(&tzi);
	

    //TCHAR standardName[512];
	/*LPSTR standardName;
	WideCharToMultiByte( CP_ACP, 0, tzi.StandardName, -1, standardName, 256, NULL, NULL );

    TCHAR dayLightName[512];
    WideCharToMultiByte( CP_ACP, 0, tzi.DaylightName, -1,(LPSTR)dayLightName, 256, NULL, NULL );*/

    for ( int index = 0; index < m_arrRegTimeZoneInfo.GetSize(); index++ )
    {
        CRegTimeZoneInfo* pRegTimeZoneInfo = m_arrRegTimeZoneInfo[index];
        if ( pRegTimeZoneInfo->m_regTZI.Bias == tzi.Bias &&
			pRegTimeZoneInfo->m_regTZI.StandardBias == tzi.StandardBias)// &&             (!(_tcscmp(pRegTimeZoneInfo->m_szStd, standardName)))/* &&              (!(_tcscmp(pRegTimeZoneInfo->m_szDlt, dayLightName)))*/
           
        {
            return index;
        }
    }


    return 0;

}




int CTimeZoneInfoManager::ConvertFromLocalToUTC(CDateTime* i_LocalTime, int i_TimeZoneIndex, CDateTime* o_UTCTime)
{
    SYSTEMTIME localTime, universalTime;

    localTime.wDay          = i_LocalTime->m_wDay;
    localTime.wDayOfWeek    = i_LocalTime->m_wDayOfWeek;
    localTime.wHour         = i_LocalTime->m_wHour;
    localTime.wMinute       = i_LocalTime->m_wMinute;
    localTime.wMonth        = i_LocalTime->m_wMonth;
    localTime.wSecond       = i_LocalTime->m_wSecond;
    localTime.wYear         = i_LocalTime->m_wYear;
    localTime.wMilliseconds = i_LocalTime->m_wMilliseconds;

    //get timezone infor from index
    TIME_ZONE_INFORMATION tzi;

    ZeroMemory(&tzi, sizeof(tzi));
    CRegTimeZoneInfo* pRegTimeZoneInfo = m_arrRegTimeZoneInfo[i_TimeZoneIndex];

    tzi.Bias            = pRegTimeZoneInfo->m_regTZI.Bias;
    tzi.DaylightBias    = pRegTimeZoneInfo->m_regTZI.DaylightBias;
    tzi.DaylightDate    = pRegTimeZoneInfo->m_regTZI.DaylightDate;
    tzi.StandardBias    = pRegTimeZoneInfo->m_regTZI.StandardBias;
    tzi.StandardDate    = pRegTimeZoneInfo->m_regTZI.StandardDate;
MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, (LPCSTR)pRegTimeZoneInfo->m_szStd , -1, tzi.StandardName, 32);
  MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, (LPCSTR)pRegTimeZoneInfo->m_szDlt , -1, tzi.DaylightName , 32);
      

    //Change CurrentTimeZone
    //if (!g_IsWindowsXPorLater())
    //{
        SpecificLocalTimeToSystemTime(&localTime, &universalTime);

    //}
    //else
    //{
//#ifdef _XP_OR_LATER
        //available only on XP and Later
        TzSpecificLocalTimeToSystemTime(&tzi,&localTime,&universalTime);
//#endif

    //}


    o_UTCTime->SetDateTime(universalTime.wYear, universalTime.wMonth, universalTime.wDayOfWeek, universalTime.wDay,
                           universalTime.wHour, universalTime.wMinute, universalTime.wSecond);

    
    return 0;

}



/*int CTimeZoneInfoManager::ConvertFromUTCToLocal(CDateTime* i_UTCTime, int i_TimeZoneIndex, CDateTime* o_localTime)
{
    SYSTEMTIME localTime, universalTime;

    universalTime.wDay          = i_UTCTime->m_wDay;
    universalTime.wDayOfWeek    = i_UTCTime->m_wDayOfWeek;
    universalTime.wHour         = i_UTCTime->m_wHour;
    universalTime.wMinute       = i_UTCTime->m_wMinute;
    universalTime.wMonth        = i_UTCTime->m_wMonth;
    universalTime.wSecond       = i_UTCTime->m_wSecond;
    universalTime.wYear         = i_UTCTime->m_wYear;
    universalTime.wMilliseconds = i_UTCTime->m_wMilliseconds;

    //get timezone infor from index
    TIME_ZONE_INFORMATION tzi;
//    GetTimeZoneInformation(&tzi);

     ZeroMemory(&tzi, sizeof(tzi));
    CRegTimeZoneInfo* pRegTimeZoneInfo = m_arrRegTimeZoneInfo[i_TimeZoneIndex];

    tzi.Bias            = pRegTimeZoneInfo->m_regTZI.Bias;
    tzi.DaylightBias    = pRegTimeZoneInfo->m_regTZI.DaylightBias;
    tzi.DaylightDate    = pRegTimeZoneInfo->m_regTZI.DaylightDate;
    tzi.StandardBias    = pRegTimeZoneInfo->m_regTZI.StandardBias;
    tzi.StandardDate    = pRegTimeZoneInfo->m_regTZI.StandardDate;
    //MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, pRegTimeZoneInfo->m_szStd , -1, tzi.StandardName, 32);
    //MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, pRegTimeZoneInfo->m_szDlt , -1, tzi.DaylightName , 32);
      
    
    if (!SystemTimeToTzSpecificLocalTime(&tzi, &universalTime, &localTime))
    {

    }


    o_localTime->SetDateTime(localTime.wYear, localTime.wMonth, localTime.wDayOfWeek, localTime.wDay,
                           localTime.wHour, localTime.wMinute, localTime.wSecond);

    return 0;

}*/




int CTimeZoneInfoManager::SpecificLocalTimeToSystemTime(SYSTEMTIME* i_stLocal, SYSTEMTIME* o_stUniversal)
{

    FILETIME ft, ft_utc;


    if (!(SystemTimeToFileTime(i_stLocal, &ft) &&
          LocalFileTimeToFileTime(&ft, &ft_utc) && 
          FileTimeToSystemTime(&ft_utc,o_stUniversal)))
    {
        
        return 0; 
        
    }

    return 1;

}









/******************************************** CRegTimeZoneInfo *********************************************/


void CRegTimeZoneInfo::FillFromRegistry(int i_RegistryIndex, BYTE* i_pValue, CString i_strValueName, DWORD i_ValueSize)
{

    static int cnt =0;

    if (!i_strValueName.CompareNoCase(_T("Display")))
    {
		memcpy(m_szDisplay, i_pValue, i_ValueSize);
		cnt++;
    }

    if (!i_strValueName.CompareNoCase(_T("Dlt")))
    memcpy(m_szDlt, i_pValue, i_ValueSize);

    if (!i_strValueName.CompareNoCase(_T("Std")))
    memcpy(m_szStd, i_pValue, i_ValueSize);

    if (!i_strValueName.CompareNoCase(_T("MapID")))
    memcpy(m_szMapID, i_pValue, i_ValueSize);

    if (!i_strValueName.CompareNoCase(_T("Index")))
    memcpy((BYTE*)&m_iIndex, i_pValue, i_ValueSize);

    if (!i_strValueName.CompareNoCase(_T("TZI")))
    memcpy((BYTE*)&m_regTZI, i_pValue, i_ValueSize);

}
