using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace KeyTrackerBase
{
    class globalKeyboardHook
    {
        #region Variables & Constants
        //defines the callback type for the hook
        public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);
        public struct keyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        //only include keydown - although in future key up might help stop people bypassing
        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_SYSKEYDOWN = 0x104;
        #endregion

        #region Instance Variables
        //the collection of keys to watch for
        public List<Keys> HookedKeys = new List<Keys>();
        //handle to the hook, used to unhook and call the next hook
        IntPtr hhook = IntPtr.Zero;
        #endregion

        #region Events
        //Record the keys in different ways, using different events available
        //Occurs when one of the hooked keys is pressed
        public event KeyEventHandler KeyDown;
        //key up would also go here if required
        #endregion

        #region Constructor & Destructor
        //init a new instance of globalhook class & installs keyboard hook
        public globalKeyboardHook()
        {
            hook();
        }

        //releases unmanaged resources and performs other cleanup operations before globalhook is reclaimed and uninstalls hook
        ~globalKeyboardHook()
        {
            unhook();
        }
        #endregion

        #region Public Methods
        //installs global hook
        keyboardHookProc hookDelegate;
        public void hook()
        {
            hookDelegate = new keyboardHookProc(hookProc);
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, hookDelegate, hInstance, 0);
        }

        public static IntPtr hInstance = LoadLibrary("User32");

        //uninstalls global hook
        public void unhook()
        {
            UnhookWindowsHookEx(hhook);
        }

        //the callback for the keyboard hook
        public int hookProc(int code, int wParam, ref keyboardHookStruct lParam)
        {
            if (code >= 0)
            {
                Keys key = (Keys)lParam.vkCode;
                if (HookedKeys.Contains(key))
                {
                    KeyEventArgs kea = new KeyEventArgs(key);
                    if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && (KeyDown != null))
                    {
                        KeyDown(this, kea);
                    }
                    if (kea.Handled)
                        return 1;
                }
            }
            return CallNextHookEx(hhook, code, wParam, ref lParam);
        }
        #endregion

        #region DLL Imports
        //sets the windows hook
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

        //unhooks the windows hook
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        //calls next hook
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

        //loads library
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);
        #endregion

    }
}
