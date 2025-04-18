﻿#pragma checksum "..\..\..\Barista\BaristaWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9A06FE6C209348138B9C07A004000AF437A56EB8C4EB87C3DE5200902C9607EC"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using CoffeeHouse;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CoffeeHouse.Barista {
    
    
    /// <summary>
    /// BaristaWindow
    /// </summary>
    public partial class BaristaWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 94 "..\..\..\Barista\BaristaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PrepareDrinksButton;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\Barista\BaristaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OrdersQueueButton;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\Barista\BaristaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button StatusChangeButton;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\Barista\BaristaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PreparationLogButton;
        
        #line default
        #line hidden
        
        
        #line 119 "..\..\..\Barista\BaristaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock UserRoleText;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\..\Barista\BaristaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock UserFullNameText;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\Barista\BaristaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock UserScheduleText;
        
        #line default
        #line hidden
        
        
        #line 136 "..\..\..\Barista\BaristaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame MainFrame;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CoffeeHouse;component/barista/baristawindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Barista\BaristaWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.PrepareDrinksButton = ((System.Windows.Controls.Button)(target));
            
            #line 96 "..\..\..\Barista\BaristaWindow.xaml"
            this.PrepareDrinksButton.Click += new System.Windows.RoutedEventHandler(this.PrepareDrinksButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.OrdersQueueButton = ((System.Windows.Controls.Button)(target));
            
            #line 101 "..\..\..\Barista\BaristaWindow.xaml"
            this.OrdersQueueButton.Click += new System.Windows.RoutedEventHandler(this.OrdersQueueButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.StatusChangeButton = ((System.Windows.Controls.Button)(target));
            
            #line 106 "..\..\..\Barista\BaristaWindow.xaml"
            this.StatusChangeButton.Click += new System.Windows.RoutedEventHandler(this.StatusChangeButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.PreparationLogButton = ((System.Windows.Controls.Button)(target));
            
            #line 111 "..\..\..\Barista\BaristaWindow.xaml"
            this.PreparationLogButton.Click += new System.Windows.RoutedEventHandler(this.PreparationLogButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.UserRoleText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.UserFullNameText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.UserScheduleText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            
            #line 128 "..\..\..\Barista\BaristaWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LogoutButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.MainFrame = ((System.Windows.Controls.Frame)(target));
            
            #line 136 "..\..\..\Barista\BaristaWindow.xaml"
            this.MainFrame.Navigated += new System.Windows.Navigation.NavigatedEventHandler(this.MainFrame_Navigated);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

