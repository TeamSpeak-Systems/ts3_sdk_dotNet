using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TeamSpeak.Sdk
{
    internal struct SdkLock : IDisposable
    {
        private SdkHandle Handle;

        public SdkLock(SdkHandle handle)
        {
            Handle = handle;
            bool success = false;
            Handle.DangerousAddRef(ref success);
            if (success == false)
                throw new ObjectDisposedException(typeof(SdkLock).FullName);
        }
        void IDisposable.Dispose()
        {
            Handle.DangerousRelease();
        }
    }
}

