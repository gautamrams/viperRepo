// RegistryManager.h: interface for the CRegistryManager class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_REGISTRYMANAGER_H__1EEE90C4_25D1_4C00_A16D_842C479F6EA6__INCLUDED_)
#define AFX_REGISTRYMANAGER_H__1EEE90C4_25D1_4C00_A16D_842C479F6EA6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class CRegistryManager  
{
public:
	CRegistryManager();
	virtual ~CRegistryManager();

//operations.
public:
    LONG OpenRegistryKey(HKEY hKey,
                    LPCTSTR lpSubKey,
                    DWORD ulOptions = 0,
                    REGSAM samDesired = KEY_READ,
                    PHKEY phkResult=NULL
                    );

    LONG EnumerateSubKeys(HKEY hKey, CStringArray* o_subkeyArray);
    LONG EnumerateKeyValueNames(HKEY hKey, CStringArray* o_keyValueNamesArray);
    LONG QueryReistryValue( HKEY hKey,
                          LPCTSTR lpValueName,
                          LPDWORD lpReserved,
                          LPDWORD lpType,
                          LPBYTE lpData,
                          LPDWORD lpcbData
                        );




};

#endif // !defined(AFX_REGISTRYMANAGER_H__1EEE90C4_25D1_4C00_A16D_842C479F6EA6__INCLUDED_)
