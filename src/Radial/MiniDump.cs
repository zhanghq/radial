using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// MiniDumpType
    /// </summary>
    public enum MiniDumpType
    {
        /// <summary>
        /// Normal
        /// </summary>
        Normal = 0x00000000,
        /// <summary>
        /// WithDataSegs
        /// </summary>
        WithDataSegs = 0x00000001,
        /// <summary>
        /// WithFullMemory
        /// </summary>
        WithFullMemory = 0x00000002,
        /// <summary>
        /// WithHandleData
        /// </summary>
        WithHandleData = 0x00000004,
        /// <summary>
        /// FilterMemory
        /// </summary>
        FilterMemory = 0x00000008,
        /// <summary>
        /// ScanMemory
        /// </summary>
        ScanMemory = 0x00000010,
        /// <summary>
        /// WithUnloadedModules
        /// </summary>
        WithUnloadedModules = 0x00000020,
        /// <summary>
        /// WithIndirectlyReferencedMemory
        /// </summary>
        WithIndirectlyReferencedMemory = 0x00000040,
        /// <summary>
        /// FilterModulePaths
        /// </summary>
        FilterModulePaths = 0x00000080,
        /// <summary>
        /// WithProcessThreadData
        /// </summary>
        WithProcessThreadData = 0x00000100,
        /// <summary>
        /// WithPrivateReadWriteMemory
        /// </summary>
        WithPrivateReadWriteMemory = 0x00000200,
        /// <summary>
        /// WithoutOptionalData
        /// </summary>
        WithoutOptionalData = 0x00000400,
        /// <summary>
        /// WithFullMemoryInfo
        /// </summary>
        WithFullMemoryInfo = 0x00000800,
        /// <summary>
        /// WithThreadInfo
        /// </summary>
        WithThreadInfo = 0x00001000,
        /// <summary>
        /// WithCodeSegs
        /// </summary>
        WithCodeSegs = 0x00002000,
        /// <summary>
        /// WithoutAuxiliaryState
        /// </summary>
        WithoutAuxiliaryState = 0x00004000,
        /// <summary>
        /// WithFullAuxiliaryState
        /// </summary>
        WithFullAuxiliaryState = 0x00008000,
        /// <summary>
        /// WithPrivateWriteCopyMemory
        /// </summary>
        WithPrivateWriteCopyMemory = 0x00010000,
        /// <summary>
        /// IgnoreInaccessibleMemory
        /// </summary>
        IgnoreInaccessibleMemory = 0x00020000,
        /// <summary>
        /// WithTokenInformation
        /// </summary>
        WithTokenInformation = 0x00040000,
        /// <summary>
        /// WithModuleHeaders
        /// </summary>
        WithModuleHeaders = 0x00080000,
        /// <summary>
        /// FilterTriage
        /// </summary>
        FilterTriage = 0x00100000,
        /// <summary>
        /// ValidTypeFlags
        /// </summary>
        ValidTypeFlags = 0x001fffff,
    }

    /// <summary>
    /// MiniDump
    /// </summary>
    public static class MiniDump
    {
        [DllImport("DbgHelp.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern Boolean MiniDumpWriteDump(
                                    IntPtr hProcess,
                                    int processId,
                                    IntPtr fileHandle,
                                    MiniDumpType dumpType,
                                    ref MinidumpExceptionInfo excepInfo,
                                    IntPtr userInfo,
                                    IntPtr extInfo);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern int GetCurrentThreadId();

        /// <summary>
        /// MinidumpExceptionInfo
        /// </summary>
        struct MinidumpExceptionInfo
        {
            /// <summary>
            /// ThreadId
            /// </summary>
            public int ThreadId;
            /// <summary>
            /// ExceptionPointers
            /// </summary>
            public IntPtr ExceptionPointers;
            /// <summary>
            /// ClientPointers
            /// </summary>
            public bool ClientPointers;
        }

        /// <summary>
        /// Write dump file.
        /// </summary>
        /// <param name="dmpType">The mini dump type.</param>
        public static void Write(MiniDumpType dmpType = MiniDumpType.WithFullMemory | MiniDumpType.WithHandleData |
            MiniDumpType.WithModuleHeaders | MiniDumpType.WithUnloadedModules | MiniDumpType.WithProcessThreadData |
            MiniDumpType.WithFullMemoryInfo | MiniDumpType.WithThreadInfo)
        {
            //default path
            string dmpPath = StaticVariables.GetPath(DateTime.Now.ToString("yyyyMMddHHmmss") + ".dmp");

            try
            {
                using (FileStream stream = new FileStream(dmpPath, FileMode.Create))
                {
                    Process process = Process.GetCurrentProcess();

                    MinidumpExceptionInfo mei = new MinidumpExceptionInfo();

                    mei.ThreadId = GetCurrentThreadId();
                    mei.ExceptionPointers = Marshal.GetExceptionPointers();
                    mei.ClientPointers = true;

                    MiniDumpWriteDump(
                                        process.Handle,
                                        process.Id,
                                        stream.SafeFileHandle.DangerousGetHandle(),
                                        dmpType,
                                        ref mei,
                                        IntPtr.Zero,
                                        IntPtr.Zero);

                    stream.Flush();
                    stream.Close();
                }
            }
            catch
            {
                //nothing to do here
            }
        }
    }
}
