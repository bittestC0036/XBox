﻿#pragma checksum "..\..\..\..\..\Etc\UserControl\MakeFolderTree\TextEditor.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "CAAC0DBEF294462556A7E5F15B20FC78A437CDE1D6A79F9ACBC037E35DB7E74C"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace XBox {
    
    
    /// <summary>
    /// TextEditor
    /// </summary>
    public partial class TextEditor : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\..\..\Etc\UserControl\MakeFolderTree\TextEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer LineNumberScrollViewer;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\..\Etc\UserControl\MakeFolderTree\TextEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TBL_LineNumber;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\..\Etc\UserControl\MakeFolderTree\TextEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer TBScrollViewer;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\..\Etc\UserControl\MakeFolderTree\TextEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TB_Content;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\..\Etc\UserControl\MakeFolderTree\TextEditor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TB_Property;
        
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
            System.Uri resourceLocater = new System.Uri("/XBox;component/etc/usercontrol/makefoldertree/texteditor.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Etc\UserControl\MakeFolderTree\TextEditor.xaml"
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
            this.LineNumberScrollViewer = ((System.Windows.Controls.ScrollViewer)(target));
            
            #line 23 "..\..\..\..\..\Etc\UserControl\MakeFolderTree\TextEditor.xaml"
            this.LineNumberScrollViewer.ScrollChanged += new System.Windows.Controls.ScrollChangedEventHandler(this.TBScrollViewer_ScrollChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TBL_LineNumber = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.TBScrollViewer = ((System.Windows.Controls.ScrollViewer)(target));
            
            #line 28 "..\..\..\..\..\Etc\UserControl\MakeFolderTree\TextEditor.xaml"
            this.TBScrollViewer.ScrollChanged += new System.Windows.Controls.ScrollChangedEventHandler(this.TBScrollViewer_ScrollChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.TB_Content = ((System.Windows.Controls.TextBox)(target));
            
            #line 31 "..\..\..\..\..\Etc\UserControl\MakeFolderTree\TextEditor.xaml"
            this.TB_Content.AddHandler(System.Windows.Controls.ScrollViewer.ScrollChangedEvent, new System.Windows.Controls.ScrollChangedEventHandler(this.Content_ScrollChanged));
            
            #line default
            #line hidden
            
            #line 32 "..\..\..\..\..\Etc\UserControl\MakeFolderTree\TextEditor.xaml"
            this.TB_Content.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.TB_Content_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 32 "..\..\..\..\..\Etc\UserControl\MakeFolderTree\TextEditor.xaml"
            this.TB_Content.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Content_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.TB_Property = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

