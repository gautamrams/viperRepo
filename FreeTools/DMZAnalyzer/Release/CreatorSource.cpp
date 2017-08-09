// CreatorNative.cpp : Defines the entry point for the console application.
//
#include<StdAfx.h>
#include "CreatorSource.h"
 char custId[128];
 std::string cId="";
 std::string postData,addPost_url,updatePost_url;
 struct feature{
	unsigned long long int ad_query_advanced;
	unsigned long long int ad_query_generate;
	unsigned long long int csv_advanced;
	unsigned long long int csv_generator;
	unsigned long long int dc_monitoring;
	unsigned long long int dmz_port_analyser;
	unsigned long long int empty_pwd_advanced;
	unsigned long long int empty_pwd_generate;
	unsigned long long int get_dc;
	unsigned long long int get_duplicates;
	unsigned long long int last_logon;
	unsigned long long int lum_getLocalUsers;
	unsigned long long int lum_addUsers;
	unsigned long long int lum_deleteUsers;
	unsigned long long int lum_properties;
	unsigned long long int pwd_policy_manager;
	unsigned long long int terminal_session;
	unsigned long long int rm_replicate;
	unsigned long long int rm_replicate2Dcs;
	unsigned long long int rm_lastReplication;
	unsigned long long int spm_file;
	unsigned long long int spm_web;
	unsigned long long int spm_webPage;
	unsigned long long int spm_webSite;
}features;


int getInfo(){
	FILE *fp=fopen("info.dat","r");
	if(fp>0){
		fclose(fp);
		return 1;
	}
	return 0;
}

void createId(){
	FILE *fp;
	SYSTEMTIME st;
	GetLocalTime(&st);
	time_t theTime = time(NULL);
	fp=fopen("id.txt","w");
	fprintf(fp,"%s",cId.c_str());
	fclose(fp);
}

int getDate(){
	time_t theTime = time(NULL);
	struct tm *aTime = localtime(&theTime);
	int day = aTime->tm_mday;
	return day;
}
void createFile(){
	FILE *fp;
	fp=fopen("usage.txt","w");
	fprintf(fp,"0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,");
	fclose(fp);
}

void checkException(){
	FILE *fp=fopen("usage.txt","r");
	if(fp>0){
		fclose(fp);
	}
	else{
		createFile();
	}
	fp=fopen("id.txt","r");
	if(fp>0){
		fclose(fp);
	}
	else{
		cId="X";
		createId();
	}
}

bool readADToolsFile()
{
	std::string line;
	std::ifstream infile("./..//conf//active_directory_tools_details.txt");
	bool flag = false;

	while (std::getline(infile, line))
	{
	    std::istringstream iss(line);
	    if(line.find("AD360=true") != std::string::npos)
	    	{
	    		flag = true;
	    		break;
	    	}
	}
	return flag;
}

extern "C" int __declspec(dllexport) getAD360Data(){
	bool result = readADToolsFile();
	if(result)
		return 1;
	else
		return 0;
}

/*void __declspec(dllexport) writeToFile(int num){
 	checkException();
	FILE* fp = fopen("usage.txt", "r+");
	std::string c="";
	fseek(fp, 0, SEEK_END);
	long size = ftell(fp);
	fseek(fp, 0, SEEK_SET);
	char *pData = new char[size + 1];
	fscanf(fp,"%s",pData);
	fclose(fp);
	char* pch;
	int count=1;
	pch = strtok (pData, ",");
	char *ptr=(char*)malloc(2);
	while (pch != NULL)
	{
		if(count==1)
		{
			itoa(getDate(),ptr,10);
			c.append(ptr);
			printf("%s",c.c_str());
			_getch();
			pch = strtok (NULL, ",");
			count++;
			continue;
		}
		
		if(count==(num+1)){
			itoa((atoi(pch)+1),pch,10);
		if(count!=1)
			c.append(",");
		}
		c.append(pch);
		pch = strtok (NULL, ",");
		count++;
	}
	fp = fopen("usage.txt", "r+");
	fprintf(fp,"%s",c.c_str());
	fclose(fp);
}

*/
void writeToFile(int num){
	checkException();
	FILE *fp=fopen("usage.txt","r");    
	unsigned long long int incNum[32];
	int date;
	fscanf(fp,"%d,",&date);
	for(int i=1;i<32;i++){
		fscanf(fp,"%llu,",&incNum[i]);
		if(i==(num))
			incNum[num]++;
	}
	fclose(fp);
	fp=fopen("usage.txt","w");    
	fprintf(fp,"%d,",getDate());
	for(int i=1;i<31;i++)
		fprintf(fp,"%llu,",incNum[i]);
	fprintf(fp,"%d,",getAD360Data());
	fclose(fp);
	//fscanf(fp,"%d,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu",&date,incNum[]);
	
	//fprintf(fp,"%d,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu,%llu",);
}
std::wstring s2ws(const std::string& s)
{
    int len;
    int slength = (int)s.length() + 1;
    len = MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, 0, 0); 
    wchar_t* buf = new wchar_t[len];
    MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, buf, len);
    std::wstring r(buf);
    delete[] buf;
    return r;
}

 int  getFileDate(){
 	checkException();
	FILE *fp;
	int date;
	fp=fopen("usage.txt","r");
	fscanf(fp,"%d,",&date);	 
	fclose(fp);
	return  date;
}


 int  startApp(){
	checkException();
	if(getDate()==getFileDate())
		return 0;
	return 1;
}

char* getCustId(){
	FILE *fp;
	//char* custId=(char*)malloc(30);
	fp=fopen("id.txt","r");
	fscanf(fp,"%s",custId);	 
	fclose(fp);
	return  custId;
}
void getFeatures(){
	FILE *fp;
	int date;
	int a=0;
	char* temp;
	unsigned long long int arr[32];
	char *f[31]={"&ad_query_advanced=", "&ad_query_generate=", "&csv_advanced=", "&csv_generator=", "&dc_monitoring=", "&dmz_port_analyser=", "&empty_pwd_advanced=", "&empty_pwd_generate=", "&get_dc=", "&get_duplicates=", "&last_logon=", "&lum_getLocalUsers=", "&lum_addUsers=", "&lum_deleteUsers=", "&lum_properties=", "&pwd_policy_manager=", "&terminal_session=", "&rm_replicate=", "&rm_replicate2Dcs=", "&rm_lastReplication=", "&spm_file=", "&spm_web=", "&spm_webPage=", "&dns_report=", "&get_managedservice=", "&getserviceaccounts=", "&get_associatedservices=", "&weak_pwd_access_count=", "&weak_pwd_generate_success=", "&weak_pwd_generate_failure=", "&is_ad360=" };
	fp=fopen("usage.txt","r");
	fscanf(fp,"%d,",&date);
	for(a=0;a<31;a++){
		fscanf(fp,"%llu,",&arr[a]);
	}
	fclose(fp);
	addPost_url="/api/json/admpfreetool/ME_Customer_Usage/add/apikey=d0c71aa8f48fb1d7e33b6d54ea08fd60&zc_ownername=metrack";
	updatePost_url="/api/json/admpfreetool/input_ME_Customer_Usage/add/apikey=d0c71aa8f48fb1d7e33b6d54ea08fd60&zc_ownername=metrack";
	for(a=0;a<31;a++){
	temp=(char*)malloc(sizeof(char)*(sizeof(unsigned long long int)+1));
	itoa(arr[a],temp,10);
	postData.append(f[a]);
	postData.append(temp);
	free(temp);
	}
	updatePost_url.append(postData);
}

 void  addDb()
{
	getFeatures();
	DWORD dwSize = 0;
	DWORD dwDownloaded = 0;
  LPSTR pszOutBuffer;
  BOOL  bResults = FALSE;
  HINTERNET  hSession = NULL, 
             hConnect = NULL,
             hRequest = NULL;

  hSession = WinHttpOpen( L"Creator Connection",  
                          WINHTTP_ACCESS_TYPE_DEFAULT_PROXY,
                          WINHTTP_NO_PROXY_NAME, 
                          WINHTTP_NO_PROXY_BYPASS, 0 );

  if( hSession ){
    hConnect = WinHttpConnect( hSession, L"creator.zoho.com",
                               INTERNET_DEFAULT_HTTPS_PORT, 0 );
  }

  if( hConnect ){
	  hRequest = WinHttpOpenRequest( hConnect, L"POST", (s2ws(addPost_url)).c_str(),
                                   NULL, WINHTTP_NO_REFERER, 
                                   WINHTTP_DEFAULT_ACCEPT_TYPES, 
                                   WINHTTP_FLAG_SECURE );
  }


  if( hRequest ){
    bResults = WinHttpSendRequest( hRequest,
                                   WINHTTP_NO_ADDITIONAL_HEADERS, 0,
								   WINHTTP_NO_REQUEST_DATA, 0, 
                                   0, 0 );
  }
  if( !bResults )
  {
    printf( "Error %d has occurred.\n", GetLastError( ) );
	_getch();
  }

     if( bResults )
    bResults = WinHttpReceiveResponse( hRequest, NULL );

   if( bResults )
  {
    do 
    {

      dwSize = 0;
      if( !WinHttpQueryDataAvailable( hRequest, &dwSize ) )
        printf( "Error %u in WinHttpQueryDataAvailable.\n",
                GetLastError( ) );


      pszOutBuffer = new char[dwSize+1];
      if( !pszOutBuffer )
      {
        
        dwSize=0;
      }
      else
      {

        ZeroMemory( pszOutBuffer, dwSize+1 );

        if( !WinHttpReadData( hRequest, (LPVOID)pszOutBuffer, 
                              dwSize, &dwDownloaded ) )
          printf( "Error %u in WinHttpReadData.\n", GetLastError( ) );
        else;
          printf( "%s", pszOutBuffer );
		
		for(int i=0;i<strlen(pszOutBuffer);i++){
			if(pszOutBuffer[i]==':' && pszOutBuffer[i+1]>48 && pszOutBuffer[i+1]<=58)
			{
				i++;
			while(pszOutBuffer[i]!='}'){
					cId.push_back(pszOutBuffer[i]);
				i++;
				}
				break;
			}
		}
		createId();
        delete [] pszOutBuffer;
      }
    } while( dwSize > 0 );
  }

   if( hRequest ) WinHttpCloseHandle( hRequest );
  if( hConnect ) WinHttpCloseHandle( hConnect );
  if( hSession ) WinHttpCloseHandle( hSession );


}

 
void  updateDb()
{
	getFeatures();
	DWORD dwSize = 0;
	DWORD dwDownloaded = 0;
  LPSTR pszOutBuffer;
  BOOL  bResults = FALSE;
  HINTERNET  hSession = NULL, 
             hConnect = NULL,
             hRequest = NULL;

  hSession = WinHttpOpen( L"Creator Connection",  
                          WINHTTP_ACCESS_TYPE_DEFAULT_PROXY,
                          WINHTTP_NO_PROXY_NAME, 
                          WINHTTP_NO_PROXY_BYPASS, 0 );

  if( hSession ){
    hConnect = WinHttpConnect( hSession, L"creator.zoho.com",
                               INTERNET_DEFAULT_HTTPS_PORT, 0 );
  }
   updatePost_url.append("&customerid=");
		updatePost_url.append(getCustId());
		printf(updatePost_url.c_str());

  if( hConnect ){
	  hRequest = WinHttpOpenRequest( hConnect, L"POST", (s2ws(updatePost_url)).c_str(),
                                   NULL, WINHTTP_NO_REFERER, 
                                   WINHTTP_DEFAULT_ACCEPT_TYPES, 
                                   WINHTTP_FLAG_SECURE );
  }


  if( hRequest ){
    bResults = WinHttpSendRequest( hRequest,
                                   WINHTTP_NO_ADDITIONAL_HEADERS, 0,
								   WINHTTP_NO_REQUEST_DATA, 0, 
                                   0, 0 );
  }
  if( !bResults )
  {
    printf( "Error %d has occurred.\n", GetLastError( ) );
	_getch();
  }

     if( bResults )
    bResults = WinHttpReceiveResponse( hRequest, NULL );

   if( bResults )
  {
    do 
    {

      dwSize = 0;
      if( !WinHttpQueryDataAvailable( hRequest, &dwSize ) )
        printf( "Error %u in WinHttpQueryDataAvailable.\n",
                GetLastError( ) );


      pszOutBuffer = new char[dwSize+1];
      if( !pszOutBuffer )
      {
        
        dwSize=0;
      }
      else
      {

        ZeroMemory( pszOutBuffer, dwSize+1 );

        if( !WinHttpReadData( hRequest, (LPVOID)pszOutBuffer, 
                              dwSize, &dwDownloaded ) )
          printf( "Error %u in WinHttpReadData.\n", GetLastError( ) );
        else;
          printf( "%s", pszOutBuffer );
		
		
        delete [] pszOutBuffer;
      }
    } while( dwSize > 0 );
  }

   if( hRequest ) WinHttpCloseHandle( hRequest );
  if( hConnect ) WinHttpCloseHandle( hConnect );
  if( hSession ) WinHttpCloseHandle( hSession );
}