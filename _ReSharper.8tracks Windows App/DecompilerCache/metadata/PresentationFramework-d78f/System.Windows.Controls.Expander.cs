// Type: System.Windows.Controls.Expander
// Assembly: PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\PresentationFramework.dll

using System.ComponentModel;
using System.Runtime;
using System.Windows;
using System.Windows.Automation.Peers;

namespace System.Windows.Controls
{
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public class Expander : HeaderedContentControl
    {
        public static readonly DependencyProperty ExpandDirectionProperty;
        public static readonly DependencyProperty IsExpandedProperty;
        public static readonly RoutedEvent ExpandedEvent;
        public static readonly RoutedEvent CollapsedEvent;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public Expander();

        [Category("Behavior")]
        [Bindable(true)]
        public ExpandDirection ExpandDirection { get; set; }

        [Bindable(true)]
        [Category("Appearance")]
        public bool IsExpanded { get; set; }

        protected virtual void OnExpanded();
        protected virtual void OnCollapsed();
        protected override AutomationPeer OnCreateAutomationPeer();

        public event RoutedEventHandler Expanded;
        public event RoutedEventHandler Collapsed;
    }
}
