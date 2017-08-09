#include "Stdafx.h"
#include ".\jpeghandler.h"

CJpegHadler::CJpegHadler(void)
{
	//::CoInitialize(NULL);
	//::CoInitializeEx( NULL, COINIT_MULTITHREADED );
	m_Picture = NULL;

}

CJpegHadler::~CJpegHadler(void)
{
	try
	{
		Free();
		//::////Couninitialize();	
	}
	catch(...){}
}
BOOL CJpegHadler::InitJpegHandler(HWND TargetWindow,HDC hdc)
{
	if( (TargetWindow == NULL) || (hdc == NULL)) return FALSE;

	m_TargethWnd = TargetWindow;
	
	m_dc = hdc;
	
	::GetClientRect(this->m_TargethWnd,&m_rt);

	return TRUE;
}

BOOL CJpegHadler::LoadImageFile(void *Buffer,int BufferLen)
{
	try
	{

		LPVOID pvData = NULL;
		
		HGLOBAL hGlobal = GlobalAlloc(GMEM_MOVEABLE, BufferLen);

		if(hGlobal == NULL) 
		{
			return FALSE;
		}

		pvData = GlobalLock(hGlobal);

		if(pvData == NULL) 
			return FALSE;

		CopyMemory(pvData,Buffer,BufferLen);

		LPSTREAM pstm = NULL;
		
		HRESULT hr = CreateStreamOnHGlobal(hGlobal, TRUE, &pstm);

		if(FAILED(hr) || !pstm) return FALSE;
		
		Free();

		hr = ::OleLoadPicture(pstm, BufferLen, FALSE, IID_IPicture, (LPVOID *)&m_Picture);

		pstm->Release();

		if(hGlobal)
		{
			GlobalFree(hGlobal);
			hGlobal = NULL;
		}

		if( (hr == E_OUTOFMEMORY) || (hr == E_FAIL) )
		{
			TRACE0("Out of Memory \n");
			return FALSE;
		}

		if(m_Picture == NULL) return FALSE;

		this->m_Picture->get_Width(&this->m_Width);

		this->m_Picture->get_Height(&this->m_Height);

		

		this->Render(this->m_rt);

		return TRUE;
	}
	catch(...){ return FALSE;}

	return FALSE;
}

BOOL CJpegHadler::LoadImageFile(CString FileName)
{
	if((this->m_TargethWnd == NULL) || (m_dc == NULL))return FALSE;

	try
	{
		HANDLE hFile = CreateFile(FileName, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL);

		if(hFile == INVALID_HANDLE_VALUE) 
			return FALSE;
		
		DWORD dwFileSizeh = 0;
		DWORD dwFileSize = 0;
		dwFileSize = GetFileSize(hFile, &dwFileSizeh);

		if(dwFileSize  <=0 )
		{
			return FALSE;
		}

		LPVOID pvData = NULL;
		
		HGLOBAL hGlobal = GlobalAlloc(GMEM_MOVEABLE, dwFileSize);

		if(hGlobal == NULL) 
		{
			return FALSE;
		}

		pvData = GlobalLock(hGlobal);

		if(pvData == NULL) 
			return FALSE;

		DWORD dwBytesRead = 0;

		BOOL bRead = ReadFile(hFile, pvData, dwFileSize, &dwBytesRead, NULL);
		
		GlobalUnlock(hGlobal);

		CloseHandle(hFile);

		LPSTREAM pstm = NULL;
		
		HRESULT hr = CreateStreamOnHGlobal(hGlobal, TRUE, &pstm);

		if(FAILED(hr) || !pstm) return FALSE;
		
		Free();//if (m_Picture)	{ m_Picture->Release(); m_Picture = NULL;}

		hr = ::OleLoadPicture(pstm, dwFileSize, FALSE, IID_IPicture, (LPVOID *)&m_Picture);

		pstm->Release();

		if(hGlobal)
		{
			GlobalFree(hGlobal);
			hGlobal = NULL;
		}

		if( (hr == E_OUTOFMEMORY) || (hr == E_FAIL) )
		{
			TRACE0("Out of Memory \n");
			//m_Image.Destroy();
			return FALSE;
		}

		if(m_Picture == NULL) return FALSE;

		this->m_Picture->get_Width(&this->m_Width);

		this->m_Picture->get_Height(&this->m_Height);

		//this->m_Height = 600;
		//this->m_Width  = 800;


		this->Render(this->m_rt);

		return TRUE;


		/*
		HRESULT hr = m_Image.Load(FileName);

		if( (hr == E_OUTOFMEMORY) || (hr == E_FAIL) )
		{
			TRACE0("Out of Memory \n");
			m_Image.Destroy();
			return FALSE;
		}

		if( SUCCEEDED(hr))
		{
			m_Width = m_Image.GetWidth();
			m_Height = m_Image.GetHeight();
			m_Image.StretchBlt(m_dc,m_rt);
			this->Render(this->m_rt);
			return TRUE;
		}
		*/
		return FALSE;
	}
	catch(...){return FALSE;}
	return FALSE;
	

}

BOOL CJpegHadler::LoadImageFile(HWND HandleToTarget,CString FileName)
{
	try
	{
		if(HandleToTarget == NULL) return FALSE;

		if(FileName.GetLength()<=0) return FALSE;

		this->m_TargethWnd = HandleToTarget;

		return this->LoadImageFile(FileName);
	}
	catch(...){return FALSE;}
	return FALSE;
}

BOOL CJpegHadler::Render(CRect rt,bool bTop)
{
	HRESULT hr = NULL;
	try
	{
		if(this->m_Picture != NULL)
		{
			LPCRECT prcMFBounds = NULL;

			//HRESULT hr = this->m_Picture->Render(this->m_dc,rt.left, rt.top, rt.Width(), rt.Height(),0,this->m_Height,this->m_Width,-this->m_Height,prcMFBounds);
			SIZEL pix,hi;
			hi.cx = this->m_Width;
			hi.cy = this->m_Height;
			AtlHiMetricToPixel(&hi,&pix);
			TRACE2("width = %d,Height = %d\n",this->m_Width,this->m_Height);
			if(bTop == true)
				hr = this->m_Picture->Render(this->m_dc,rt.left, rt.top,pix.cx ,pix.cy,0,this->m_Height,this->m_Width,-this->m_Height,prcMFBounds);
			else
				hr = this->m_Picture->Render(this->m_dc,rt.left, rt.top, rt.Width(), rt.Height(),0,	this->m_Height,this->m_Width,-this->m_Height,prcMFBounds);
			

			if(FAILED(hr))	return FALSE;
			
			return TRUE;
		}
		return FALSE;
		

		/*
		if(!m_Image.IsNull())
		{
			m_Image.StretchBlt(m_dc,m_rt);
			return TRUE;
		}
		return FALSE;
		*/
		
	}
	catch(...){return FALSE;}
	return FALSE;

}

BOOL CJpegHadler::Render()
{
	::GetClientRect(this->m_TargethWnd,&m_rt);
	return this->Render(m_rt);
}

BOOL CJpegHadler::GetSize(int &Width,int &Height)
{
	try
	{
		if(!this->m_Picture) return FALSE;

		CSize Size(this->m_Width,this->m_Height);

		int nMapMode = ::GetMapMode(m_dc);
		
		if (nMapMode  < MM_ISOTROPIC &&	nMapMode != MM_TEXT)
		{
			
			((CDC*)this)->SetMapMode(MM_HIMETRIC);
			::LPtoDP(m_dc,(LPPOINT)&Size,2);
			((CDC*)this)->SetMapMode(nMapMode);
		}
		else
		{
			int cxPerInch, cyPerInch;
			cxPerInch = ::GetDeviceCaps(m_dc,LOGPIXELSX);
			cyPerInch = ::GetDeviceCaps(m_dc,LOGPIXELSY);
			Size.cx = MulDiv(Size.cx, cxPerInch, HIMETRIC_INCH);
			Size.cy = MulDiv(Size.cy, cyPerInch, HIMETRIC_INCH);
		}

		
		

		Width = Size.cx;

		Height = Size.cy;

		return TRUE;

		/*
		if(m_Image.IsNull()) return FALSE;

		Width = m_Width;
		Height = m_Height;

		return TRUE;
		*/
	}
	catch(...){
		return FALSE;
	}
	return FALSE;
}

BOOL CJpegHadler::SetSize(int Width,int Height)
{
	try
	{
		//if(m_Image.IsNull()) return FALSE;

		if(!this->m_Picture) return FALSE;

		this->m_rt.top = 0;
		this->m_rt.left = 0;
		this->m_rt.bottom = Height;
		this->m_rt.right = Width;
		this->Render(m_rt);
	}
	catch(...)
	{
		return FALSE;
	}
	return FALSE;
}

BOOL CJpegHadler::UnLoad()
{
	try 
	{
		if(this->m_Picture)
		{
			Free();
			InvalidateRect(m_TargethWnd,m_rt,TRUE);
			return TRUE;
		}

		/*
		if(!m_Image.IsNull())
		{
			Free();
			InvalidateRect(m_TargethWnd,m_rt,TRUE);
			return TRUE;
		}
		*/
		return FALSE;
	}
	catch(...){return FALSE;}
	return FALSE;
}

void CJpegHadler::Free()
{
	try
	{
		if(this->m_Picture != NULL)
		{
			this->m_Picture->Release();
			this->m_Picture = NULL;
		}
		/*
		if(!m_Image.IsNull())
			m_Image.Destroy();
			*/
	}
	catch(...){}
}

BOOL CJpegHadler::IsLoaded()
{
	if(m_Picture != NULL) return TRUE;

	return FALSE;

	/*
	if(!m_Image.IsNull())	
		return TRUE;*/
	
	return FALSE;
}