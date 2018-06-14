using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.Crm.Isv.CustomizationComparisonUtility
{
    class LockRedraw: IDisposable
    {
        const int WM_SETREDRAW = 0xB;
        IntPtr _hWnd;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public LockRedraw(IntPtr hWnd)
        {
            SendMessage(hWnd, WM_SETREDRAW, (IntPtr)0, (IntPtr)0);
            _hWnd = hWnd;
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            SendMessage(_hWnd, WM_SETREDRAW, (IntPtr)1, (IntPtr)0);
        }

        #endregion
    }
}
