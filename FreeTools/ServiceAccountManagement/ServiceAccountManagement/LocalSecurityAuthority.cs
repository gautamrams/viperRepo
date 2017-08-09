using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using System.ComponentModel;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using ServiceAccountManagement;
using System.Windows.Forms;
namespace ServiceAccountManagement
{
    using LSA_HANDLE = IntPtr;
    
    class LocalSecurityAuthority
    {
        const int NO_ERROR = 0;
        const int ERROR_INSUFFICIENT_BUFFER = 122;

        enum SID_NAME_USE
        {
            SidTypeUser = 1,
            SidTypeGroup,
            SidTypeDomain,
            SidTypeAlias,
            SidTypeWellKnownGroup,
            SidTypeDeletedAccount,
            SidTypeInvalid,
            SidTypeUnknown,
            SidTypeComputer
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool LookupAccountSid(
          string lpSystemName,
          [MarshalAs(UnmanagedType.LPArray)] byte[] Sid,
          StringBuilder lpName,
          ref uint cchName,
          StringBuilder ReferencedDomainName,
          ref uint cchReferencedDomainName,
          out SID_NAME_USE peUse);
        
        [StructLayout(LayoutKind.Sequential)]
        struct LSA_OBJECT_ATTRIBUTES
        {
            internal int Length;
            internal IntPtr RootDirectory;
            internal IntPtr ObjectName;
            internal int Attributes;
            internal IntPtr SecurityDescriptor;
            internal IntPtr SecurityQualityOfService;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct LSA_UNICODE_STRING
        {
            internal ushort Length;
            internal ushort MaximumLength;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string Buffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LSA_ENUMERATION_INFORMATION
        {
            internal IntPtr PSid;
        }

        sealed class Win32Sec
        {
            [DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true),
            SuppressUnmanagedCodeSecurityAttribute]
            internal static extern uint LsaOpenPolicy(
            LSA_UNICODE_STRING[] SystemName,
            ref LSA_OBJECT_ATTRIBUTES ObjectAttributes,
            int AccessMask,
            out IntPtr PolicyHandle
            );

            [DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true),
            SuppressUnmanagedCodeSecurityAttribute]
            internal static extern uint LsaEnumerateAccountsWithUserRight(
            LSA_HANDLE PolicyHandle,
            LSA_UNICODE_STRING[] UserRights,
            out IntPtr EnumerationBuffer,
            out int CountReturned
            );

            [DllImport("advapi32")]
            internal static extern int LsaNtStatusToWinError(int NTSTATUS);

            [DllImport("advapi32")]
            internal static extern int LsaClose(IntPtr PolicyHandle);

            [DllImport("advapi32")]
            internal static extern int LsaFreeMemory(IntPtr Buffer);

        }
        public class LsaWrapper : IDisposable
        {
            enum Access : int
            {
                POLICY_READ = 0x20006,
                POLICY_ALL_ACCESS = 0x00F0FFF,
                POLICY_EXECUTE = 0X20801,
                POLICY_WRITE = 0X207F8
            }
            const uint STATUS_ACCESS_DENIED = 0xc0000022;
            const uint STATUS_INSUFFICIENT_RESOURCES = 0xc000009a;
            const uint STATUS_NO_MEMORY = 0xc0000017;
            const uint STATUS_NO_MORE_ENTRIES = 0xc000001A;

            IntPtr lsaHandle;

            public LsaWrapper()
                : this(null)
            { }
            // local system if systemName is null
            public LsaWrapper(string systemName)
            {
                LSA_OBJECT_ATTRIBUTES lsaAttr;
                lsaAttr.RootDirectory = IntPtr.Zero;
                lsaAttr.ObjectName = IntPtr.Zero;
                lsaAttr.Attributes = 0;
                lsaAttr.SecurityDescriptor = IntPtr.Zero;
                lsaAttr.SecurityQualityOfService = IntPtr.Zero;
                lsaAttr.Length = Marshal.SizeOf(typeof(LSA_OBJECT_ATTRIBUTES));
                lsaHandle = IntPtr.Zero;
                LSA_UNICODE_STRING[] system = null;
                if (systemName != null)
                {
                    system = new LSA_UNICODE_STRING[1];
                    system[0] = InitLsaString(systemName);
                }

                uint ret = Win32Sec.LsaOpenPolicy(system, ref lsaAttr,
                (int)Access.POLICY_ALL_ACCESS, out lsaHandle);
                if (ret == 0)
                    return;
                if (ret == STATUS_ACCESS_DENIED)
                {
                    throw new UnauthorizedAccessException();
                }
                if ((ret == STATUS_INSUFFICIENT_RESOURCES) || (ret == STATUS_NO_MEMORY))
                {
                    throw new OutOfMemoryException();
                }
                throw new Win32Exception(Win32Sec.LsaNtStatusToWinError((int)ret));
            }
            
            public System.Collections.ArrayList ReadPrivilege(string privilege)
            {
                
                    LSA_UNICODE_STRING[] privileges = new LSA_UNICODE_STRING[1];
                    privileges[0] = InitLsaString(privilege);
                    IntPtr buffer;
                    int count = 0;
                    System.Collections.ArrayList accountslist = new System.Collections.ArrayList();
                    uint ret = Win32Sec.LsaEnumerateAccountsWithUserRight(lsaHandle, privileges, out buffer, out count);
                    if (ret == 0)
                    {
                        
                            LSA_ENUMERATION_INFORMATION[] LsaInfo = new LSA_ENUMERATION_INFORMATION[count];
                            for (int i = 0, elemOffs = (int)buffer; i < count; i++)
                            {
                                try
                               {
                                LsaInfo[i] = (LSA_ENUMERATION_INFORMATION)Marshal.PtrToStructure(
                                       (IntPtr)elemOffs, typeof(LSA_ENUMERATION_INFORMATION));

                                elemOffs += Marshal.SizeOf(typeof(LSA_ENUMERATION_INFORMATION));
                                SecurityIdentifier sec = new SecurityIdentifier(LsaInfo[i].PSid);
                                accountslist.Add(sec);
                                }
                        catch (Exception e) 
                        {
                            // MessageBox.Show(e.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);   
                        }
                        }
                            return accountslist;
                       
                    }
                    if (ret == STATUS_ACCESS_DENIED)
                    {
                        throw new UnauthorizedAccessException();
                    }
                    if ((ret == STATUS_INSUFFICIENT_RESOURCES) || (ret == STATUS_NO_MEMORY))
                    {
                        throw new OutOfMemoryException();
                    }
                    throw new Win32Exception(Win32Sec.LsaNtStatusToWinError((int)ret));
                    
                
            }

            public void Dispose()
            {
                if (lsaHandle != IntPtr.Zero)
                {
                    Win32Sec.LsaClose(lsaHandle);
                    lsaHandle = IntPtr.Zero;
                }
                GC.SuppressFinalize(this);
            }
            ~LsaWrapper()
            {
                Dispose();
            }

            static LSA_UNICODE_STRING InitLsaString(string s)
            {
                // Unicode strings max. 32KB
                if (s.Length > 0x7ffe)
                    throw new ArgumentException("String too long");
                LSA_UNICODE_STRING lus = new LSA_UNICODE_STRING();
                lus.Buffer = s;
                lus.Length = (ushort)(s.Length * sizeof(char));
                lus.MaximumLength = (ushort)(lus.Length + sizeof(char));
                return lus;
            }
        }

       
    }
}

    

