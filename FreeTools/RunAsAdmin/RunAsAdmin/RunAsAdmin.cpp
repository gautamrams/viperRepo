

#include <stdio.h>
#include <windows.h>
#include <tchar.h>
#include <shellapi.h>
#include <iostream>
#include <string.h>


//For Get OS info..
#define WINDOWS_2000 0
#define WINDOWS_XP 1
#define WINDOWS_VISTA 2
#define WINDOWS_7 3
#define WINDOWS_LATEST 3
#define UNKNOWN_OS -1

#include <time.h>
#define TIME_24 1
#define TIME_12 0

void Log(char* format, ...);
char* GetTimeString(int mode);
char* IntToStr(unsigned int n);
char* GetDateString();
char* getMessage(const char* message,va_list args);
void getLogPath();
LPSTR LogPath = NULL;	

BOOL isVistaAndAbove();
BOOL Execute(LPSTR window,LPSTR path,LPSTR argument1,LPSTR argument2);
char ChangeToWideStr( LPSTR ansiString, LPWSTR *wideString );

int main(int argc,char* argv[])
{
	getLogPath();

	BOOL bRet = FALSE;
   
    if(_stricmp(argv[1],"Show_Window") == 0)
    {
         /**This conditions are only for BackupDB operation.**/
        if(argc == 3)
	    {
		    bRet = Execute(argv[1], argv[2], "", "");
		    if( !bRet )
		    {			
		    	Log( "Execution Failed\n" );
		    }
	    }
	    else if (argc == 4)
	    {
		    bRet = Execute(argv[1], argv[2], argv[3],"");
		    if( !bRet )
		    {			
			    Log( "Execution Failed\n" );
		    }
	    }
        else if (argc == 5)
	    {
		    bRet = Execute(argv[1], argv[2], argv[3],argv[4]);
		    if( !bRet )
		    {			
			    Log( "Execution Failed\n" );
		    }
	    }
    	else
	    {
	    	Log( "Wrong Argument!\n" );	
	    }
        
    }
    else if(argc == 2)
	{
		bRet = Execute("",argv[1],"","");
		if( !bRet )
		{			
			Log( "Execution Failed\n" );
		}
	}
	else if (argc == 3)
	{
		bRet = Execute("",argv[1], argv[2],"");
		if( !bRet )
		{			
			Log( "Execution Failed\n" );
		}
	}
	else
	{
		Log( "Wrong Argument!\n" );	
	}

	delete[] LogPath;

	return 0;
}

/*******************************************************************************************************/
// ChangeToWideStr(...) - To convert ANSI to widechar
// PARAMETERS :
//		ansiString - (IN) ANSI String..
//		wideString - (OUT) WideString..  
//		
//		
// RETURNS : it returns wide string..
//           
/*******************************************************************************************************/
char ChangeToWideStr( LPSTR ansiString, LPWSTR *wideString )
{
	int reqLen = 0;      

	reqLen = MultiByteToWideChar( CP_ACP, 0, ansiString, -1, 0, 0 );
	*wideString = ( LPWSTR )calloc(1, ((reqLen)*sizeof(WCHAR))+1);
	reqLen = MultiByteToWideChar( CP_ACP, 0, ansiString, -1, *wideString, reqLen );
	if (!reqLen)
    	   return FALSE;

	return TRUE; 
}

/*******************************************************************************************************/
// Execute(...) -	It executes application at runas account.
// 		
// RETURNS : it returns True if ShellExecuteEx succeeded.
//           
/*******************************************************************************************************/
BOOL Execute(LPSTR window,LPSTR path,LPSTR argument1,LPSTR argument2)
{	
	LPWSTR wPath = NULL;
	if( !ChangeToWideStr(path, &wPath) )
		Log( "Execute: Problem occurred while Converting Path, Error Code : %x\n",GetLastError() );

    LPSTR wArg=NULL;
    wArg=(char*)malloc((strlen(argument1)+strlen(argument2)+strlen(" ")+1));
    strcpy(wArg,argument1);
    strcat(wArg," ");
    strcat(wArg,argument2);

    LPWSTR wArg1 = NULL;
	if( !ChangeToWideStr(wArg, &wArg1) )
		Log( "Execute: Problem occurred while Converting Arguments, Error Code : %x\n",GetLastError() );
   

	SHELLEXECUTEINFO shExecInfo;	
	shExecInfo.cbSize = sizeof(SHELLEXECUTEINFO);
	shExecInfo.fMask = NULL;
	shExecInfo.hwnd = NULL;
	
	if( isVistaAndAbove() )
	{
		shExecInfo.lpVerb =_T("runas");
	}
	else
	{
		shExecInfo.lpVerb = _T("open");      	
	}

	shExecInfo.lpFile = wPath;	
	shExecInfo.lpParameters = wArg1;
	shExecInfo.lpDirectory = NULL;
	if(_stricmp(window,"Show_Window") == 0)
	{	
	    shExecInfo.nShow =SW_MAXIMIZE;
	}
	else
	  shExecInfo.nShow = SW_HIDE;
	
	shExecInfo.hInstApp = NULL;
	
	BOOL  bRet = ShellExecuteEx(&shExecInfo);
	if( !bRet )
	{
		Log( "Execute: ShellExecute is Failed! ErrorCode : %d\n", GetLastError() );	
	}	
	else
	{
		Log( "Execute: ShellExecute is Succeded!\n" );
	}

	if( wPath )
	   free(wPath);

	if( wArg1 )
	   free(wArg1);

    if( wArg)
	   free(wArg);

	return bRet;
}

/*******************************************************************************************************/
// Log(...) -	This is for Log entry.
// PARAMETERS :
//		format - (IN) Log entry string..
//		
//		
// RETURNS : Returns void..
//           
/*******************************************************************************************************/
void Log(char* format, ...) 
{
	// Write error or other information into log file
	FILE* pFile;
	va_list ap;

	pFile = fopen(LogPath,"a");

	if(pFile != NULL) 
	{
		va_start (ap, format);
		fprintf(pFile,strcat(GetDateString()," "));
		fprintf(pFile,""," ");		
		fprintf(pFile,strcat(GetTimeString(TIME_24)," "));
		fprintf(pFile,""," ");
		char * buffer = getMessage(format,ap);
		fprintf (pFile,buffer,ap);
		va_end (ap);
		fclose(pFile);
		free(buffer);
	}
}

/*******************************************************************************************************/
// getMessage(...) -	To Get a string with appended formate.. 
// PARAMETERS :
//		message - (IN) Message string..
//		args - (IN) argument list..
//		
//		
// RETURNS : Returns String..
//           
/*******************************************************************************************************/
char* getMessage(const char* message,va_list args)
{	
	int len;	
	char * buffer= NULL;		
	len = _vscprintf(message, args)	+ 1;	
	buffer = (char*)malloc(len * sizeof(char));		
	if ( buffer == NULL ) { return NULL; }
	vsprintf(buffer, message, args);		
	return buffer;
}

/*******************************************************************************************************/
// IntToStr(...) -	To Convert integer to string..
// PARAMETERS :
//		n  - (IN) unsigned integer..
//		
//		
// RETURNS : Returns string..
//           
/*******************************************************************************************************/
char* IntToStr(unsigned int n)
{
	static char str[3];      /* String the value as a string */
	str[1] = '0'+n%10;       /* Get second digit character   */
	n /= 10;
	str[0] = '0'+n%10;       /* Get first digit character    */
	str[2] = '\0';           /* Append terminator            */

	return str;
}

/*******************************************************************************************************/
// GetDateString(...) -	To Get Date String..
// PARAMETERS :		
//		
// RETURNS : Returns Date Sting..
//           
/*******************************************************************************************************/
char* GetDateString()
{
	static char date_str[9] = {0};   /* Stores the time as a string */
	struct tm *now = NULL;
	time_t time_value = 0;

	time_value = time(NULL);          /* Get time value              */
	now = localtime(&time_value);     /* Get time and date structure */
	strcpy(date_str, IntToStr(now->tm_mday));
	strcat(date_str, "-");
	strcat(date_str, IntToStr((now->tm_mon+1)));
	strcat(date_str, "-");
	strcat(date_str, IntToStr(now->tm_year));
	return date_str;

}	

/*******************************************************************************************************/
// GetTimeString(...) -	To get the Time String..
// PARAMETERS :
//		mode24 - (IN) Time Formate..
//		
//		
// RETURNS : Returns Time string..
//           
/*******************************************************************************************************/
char* GetTimeString(int mode24) 
{
	static char time_str[12] = {0};   /* Stores the time as a string */
	struct tm *now = NULL;
	int hour = 0;
	time_t time_value = 0;

	time_value = time(NULL);          /* Get time value              */
	now = localtime(&time_value);     /* Get time and date structure */
	hour = now->tm_hour;              /* Save the hour value         */
	if(!mode24 && hour>12)            /* Adjust if 12 hour format    */
		hour -= 12;
	strcpy(time_str, IntToStr(hour));
	strcat(time_str, ":");
	strcat(time_str, IntToStr(now->tm_min));
	strcat(time_str, ":");
	strcat(time_str, IntToStr(now->tm_sec));
	if(!mode24)
		strcat(time_str, now->tm_hour>24 ? " am" : " pm");
	return time_str;
}

/*******************************************************************************************************/
// getLogPath(...) -	This is to get currect appalication directory..
// PARAMETERS :		
//		
// RETURNS : It returns the current directory string..
//           
/*******************************************************************************************************/
void getLogPath()
{
	LogPath = new CHAR[MAX_PATH];
	// Getting the current directory where the product with this exe is installed
	
	if(0 == GetCurrentDirectoryA(MAX_PATH, LogPath))
	{	
		strcpy( LogPath,"RunAsAdmin.txt");
		Log( "Unable to find the Current Directory , Errorcode %d \n",GetLastError());
	}
	else 
		strcat(LogPath,"\\..\\logs\\RunAsAdmin.txt");
}

/*******************************************************************************************************/
// GetOsFamily(...) -	This is to get OS family version.
// PARAMETERS :		
//		
// RETURNS : It returns OS versions..
//           
/*******************************************************************************************************/
DWORD GetOsFamily()
{
	OSVERSIONINFOEX osvi;
	BOOL bOsVersionInfoEx;
	ZeroMemory(&osvi, sizeof(OSVERSIONINFOEX));
	osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFOEX);

	if( !(bOsVersionInfoEx = GetVersionEx ((OSVERSIONINFO *) &osvi)) )
	{
		Log( "GetOsFamily : Unable to find the OSVersion , Errorcode %d \n",GetLastError());
		return WINDOWS_XP;
	}
	else
	{
		if ( VER_PLATFORM_WIN32_NT == osvi.dwPlatformId &&  osvi.dwMajorVersion > 4 )
		{  
			if ( osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 0 )
			{
				return WINDOWS_VISTA;  // Vista , 2008 , 2008 x64
			}
			else if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion >= 1 )
			{
				return WINDOWS_7; // windows 7 , 7 x64 , 2008 r2  			
			}
			else if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 0 )
			{
				return WINDOWS_2000; // 2000
			}
			else if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion >= 1 )
			{
				return WINDOWS_XP; // xp , xp x64 , 2003 , 2003 x64  
			}
			else
			{
				return WINDOWS_LATEST; //if major version is 7 , then it should be latest
			}
		}
		else
		{  
			Log( "GetOsFamily :The windows is unable to found . \n"); 
			return UNKNOWN_OS;
		}
	}
	return UNKNOWN_OS;
}

/*******************************************************************************************************/
// isVistaAndAbove(...) -	To check vista and above OS.
// PARAMETERS :
//		
// RETURNS : It returns true if vista and above os.
//           
/*******************************************************************************************************/
BOOL isVistaAndAbove()
{
	DWORD osFamily = GetOsFamily();
	if(osFamily < WINDOWS_VISTA ) 
	{
		Log( "isVistaAndAbove : FALSE\n" );
		return FALSE;
	}
	else
	{
		Log( "isVistaAndAbove : TRUE\n" );
		return TRUE;
	}
}



