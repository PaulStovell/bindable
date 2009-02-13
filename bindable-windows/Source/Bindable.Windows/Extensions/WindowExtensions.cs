using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Input;

namespace Bindable.Windows.Extensions
{
    /// <summary>
    /// These exensions provide the ability to customise the default Win32 chrome for enabling Minimize, Maximize, and Help properties.
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>
        /// Identifies the IsMinimizeEnabled property.
        /// </summary>
        public static readonly DependencyProperty IsMinimizeEnabledProperty = DependencyProperty.RegisterAttached("IsMinimizeEnabled", typeof(bool?), typeof(WindowExtensions), new UIPropertyMetadata(null, WindowOption_PropertyChanged));
        /// <summary>
        /// Identifies the IsMaximizeEnabled property.
        /// </summary>
        public static readonly DependencyProperty IsMaximizeEnabledProperty = DependencyProperty.RegisterAttached("IsMaximizeEnabled", typeof(bool?), typeof(WindowExtensions), new UIPropertyMetadata(null, WindowOption_PropertyChanged));
        /// <summary>
        /// Identifies the IsControlBoxEnabled property.
        /// </summary>
        public static readonly DependencyProperty IsControlBoxEnabledProperty = DependencyProperty.RegisterAttached("IsControlBoxEnabled", typeof(bool?), typeof(WindowExtensions), new UIPropertyMetadata(null, WindowOption_PropertyChanged));
        /// <summary>
        /// Identifies the IsHelpEnabled property.
        /// </summary>
        public static readonly DependencyProperty IsHelpEnabledProperty = DependencyProperty.RegisterAttached("IsHelpEnabled", typeof(bool?), typeof(WindowExtensions), new UIPropertyMetadata(null, IsHelpEnabledProperty_PropertyChanged));
        
        private static readonly DependencyProperty HelpButtonExtenderProperty = DependencyProperty.RegisterAttached("HelpButtonExtender", typeof(HelpButtonExtender), typeof(WindowExtensions), new UIPropertyMetadata(null));

        public static bool? GetIsMinimizeEnabled(DependencyObject obj)
        {
            return (bool?)obj.GetValue(IsMinimizeEnabledProperty);
        }

        public static void SetIsMinimizeEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMinimizeEnabledProperty, value);
        }

        public static bool? GetIsMaximizeEnabled(DependencyObject obj)
        {
            return (bool?)obj.GetValue(IsMaximizeEnabledProperty);
        }

        public static void SetIsMaximizeEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMaximizeEnabledProperty, value);
        }

        public static bool? GetIsControlBoxEnabled(DependencyObject obj)
        {
            return (bool?)obj.GetValue(IsControlBoxEnabledProperty);
        }

        public static void SetIsControlBoxEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsControlBoxEnabledProperty, value);
        }

        public static bool? GetIsHelpEnabled(DependencyObject obj)
        {
            return (bool?)obj.GetValue(IsHelpEnabledProperty);
        }

        public static void SetIsHelpEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsHelpEnabledProperty, value);
        }

        private static HelpButtonExtender GetHelpButtonExtender(DependencyObject obj)
        {
            return (HelpButtonExtender)obj.GetValue(HelpButtonExtenderProperty);
        }

        private static void SetHelpButtonExtender(DependencyObject obj, HelpButtonExtender value)
        {
            obj.SetValue(HelpButtonExtenderProperty, value);
        }

        private static void WindowOption_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateWindowStyle(sender);
        }

        private static void IsHelpEnabledProperty_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var window = sender as Window;
            if (window != null)
            {
                if (((bool)e.NewValue) == true)
                {
                    var extender = new HelpButtonExtender((Window)sender);
                    SetHelpButtonExtender(window, extender);
                }
                else
                {
                    var extender = GetHelpButtonExtender(window);
                    if (extender != null)
                    {
                        extender.Dispose();
                    }
                }
            }
            WindowOption_PropertyChanged(sender, e);
        }

        private static void UpdateWindowStyle(DependencyObject sender)
        {
            var window = sender as Window;
            if (window == null) return;
            var interopHelper = new WindowInteropHelper(window);

            var currentStyle = NativeMethods.GetWindowLong(new HandleRef(window, interopHelper.Handle), NativeMethods.GWL_STYLE);
            var currentStyleEx = NativeMethods.GetWindowLong(new HandleRef(window, interopHelper.Handle), NativeMethods.GWL_EXSTYLE);

            FlagWhen(GetIsMaximizeEnabled(window), NativeMethods.WS_MAXIMIZEBOX, ref currentStyle);
            FlagWhen(GetIsMinimizeEnabled(window), NativeMethods.WS_MINIMIZEBOX, ref currentStyle);
            FlagWhen(GetIsHelpEnabled(window), NativeMethods.WS_EX_CONTEXTHELP, ref currentStyleEx);
            FlagWhen(!GetIsControlBoxEnabled(window), NativeMethods.WS_EX_DLGMODALFRAME, ref currentStyleEx);

            NativeMethods.SetWindowLong(new HandleRef(window, interopHelper.Handle), NativeMethods.GWL_STYLE, new HandleRef(window, new IntPtr(currentStyle)));
            NativeMethods.SetWindowLong(new HandleRef(window, interopHelper.Handle), NativeMethods.GWL_EXSTYLE, new HandleRef(window, new IntPtr(currentStyleEx)));
        }

        private static void FlagWhen(bool? condition, int flag, ref int style)
        {
            if (condition == true)
            {
                style |= flag;
            }
            else
            {
                style &= ~flag;
            }
        }

        /// <summary>
        /// This class adds a message hook to a Window to dispatch ContextHelpClick events from WM_HELP/WM_SYSCOMMAND 
        /// messages. If the Window is unloaded or help is disabled, the message hook is detached.
        /// </summary>
        private class HelpButtonExtender : ElementExtender<Window>
        {
            private HwndSource _hwndSource;

            /// <summary>
            /// Initializes a new instance of the <see cref="HelpButtonExtender"/> class.
            /// </summary>
            /// <param name="window">The window.</param>
            public HelpButtonExtender(Window window)
                : base(window)
            {
                var interopHelper = new WindowInteropHelper(window);
            }

            /// <summary>
            /// Attaches to the target element. This is called when the element is Loaded, or when the control is first extended
            /// having already been loaded.
            /// </summary>
            protected override void AttachElement()
            {
                var element = GetElement();
                if (element == null) return;

                var interopHelper = new WindowInteropHelper(element);
                if (_hwndSource != null) return;
                _hwndSource = HwndSource.FromHwnd(interopHelper.Handle);
                if (_hwndSource != null)
                {
                    _hwndSource.AddHook(ProcessMessage);
                }
            }

            /// <summary>
            /// Detaches from the target element. This is called when the element is Unloaded, or when the extended behavior is no
            /// longer required (when the ElementExtender is Disposed).
            /// </summary>
            protected override void DetachElement()
            {
                if (_hwndSource != null)
                {
                    _hwndSource.RemoveHook(ProcessMessage);
                }
            }

            private IntPtr ProcessMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
            {
                var element = GetElement();
                if (element != null)
                {

                    if (msg == NativeMethods.WM_HELP ||
                        msg == NativeMethods.WM_SYSCOMMAND && ((int)wParam) == NativeMethods.SC_CONTEXTHELP)
                    {
                        ApplicationCommands.Help.Execute(null, element);
                        handled = true;
                    }
                }

                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// A set of native Win32 messages and P/Invoke calls used by the WindowExtensions.
        /// </summary>
        private static class NativeMethods
        {
            public const int WM_HELP = 0x53;
            public const int WM_LBUTTONDOWN = 0x201;
            public const int WM_SYSCOMMAND = 0x112;
            public const int SC_CONTEXTHELP = 0xF180;
            public const int GWL_STYLE = -16;
            public const int GWL_EXSTYLE = -20;
            public const int WS_BORDER = 0x800000;
            public const int WS_CAPTION = 0xc00000;
            public const int WS_CHILD = 0x40000000;
            public const int WS_CLIPCHILDREN = 0x2000000;
            public const int WS_CLIPSIBLINGS = 0x4000000;
            public const int WS_DISABLED = 0x8000000;
            public const int WS_DLGFRAME = 0x400000;
            public const int WS_EX_APPWINDOW = 0x40000;
            public const int WS_EX_CLIENTEDGE = 0x200;
            public const int WS_EX_COMPOSITED = 0x2000000;
            public const int WS_EX_CONTEXTHELP = 0x400;
            public const int WS_EX_CONTROLPARENT = 0x10000;
            public const int WS_EX_DLGMODALFRAME = 1;
            public const int WS_EX_LAYERED = 0x80000;
            public const int WS_EX_LAYOUTRTL = 0x400000;
            public const int WS_EX_LEFT = 0;
            public const int WS_EX_LEFTSCROLLBAR = 0x4000;
            public const int WS_EX_MDICHILD = 0x40;
            public const int WS_EX_NOACTIVATE = 0x8000000;
            public const int WS_EX_NOINHERITLAYOUT = 0x100000;
            public const int WS_EX_RIGHT = 0x1000;
            public const int WS_EX_RTLREADING = 0x2000;
            public const int WS_EX_STATICEDGE = 0x20000;
            public const int WS_EX_TOOLWINDOW = 0x80;
            public const int WS_EX_TOPMOST = 8;
            public const int WS_EX_TRANSPARENT = 0x20;
            public const int WS_EX_WINDOWEDGE = 0x100;
            public const int WS_HSCROLL = 0x100000;
            public const int WS_MAXIMIZE = 0x1000000;
            public const int WS_MAXIMIZEBOX = 0x10000;
            public const int WS_MINIMIZE = 0x20000000;
            public const int WS_MINIMIZEBOX = 0x20000;
            public const int WS_OVERLAPPED = 0;
            public const int WS_OVERLAPPEDWINDOW = 0xcf0000;
            public const int WS_POPUP = -2147483648;
            public const int WS_SYSMENU = 0x80000;
            public const int WS_TABSTOP = 0x10000;
            public const int WS_THICKFRAME = 0x40000;
            public const int WS_VISIBLE = 0x10000000;
            public const int WS_VSCROLL = 0x200000;
            public const int WSF_VISIBLE = 1;

            public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, HandleRef dwNewLong)
            {
                return IntPtr.Size == 4 ? SetWindowLongPtr32(hWnd, nIndex, dwNewLong) : SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }

            [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
            private static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

            [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
            private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int GetWindowLong(HandleRef hWnd, int nIndex);
        }
    }
}
