using System.Windows;
using System.Windows.Controls;
using MAF.Framework;

namespace MAF.Attached
{
    public static class Meta
    {
        #region MetaAction

        public static readonly DependencyProperty ActionProperty = DependencyProperty.RegisterAttached(
            "Action", typeof(MetaAction), typeof(Meta), new PropertyMetadata(default(MetaAction), ActionChangedCallback));

        private static void ActionChangedCallback(DependencyObject dpo, DependencyPropertyChangedEventArgs args)
        {
            if (dpo is Control)
            {
                MetaActionManager.Instance.Detach((MetaAction)args.OldValue, (Control)dpo);
                MetaActionManager.Instance.Attach((MetaAction)args.NewValue, (Control)dpo);
            }
        }

        public static void SetAction(DependencyObject element, MetaAction value)
        {
            element.SetValue(ActionProperty, value);
        }

        public static MetaAction GetAction(DependencyObject element)
        {
            return (MetaAction)element.GetValue(ActionProperty);
        }

        #endregion

        #region MetaActionGroup

        public static readonly DependencyProperty GroupProperty = DependencyProperty.RegisterAttached(
            "Group", typeof(MetaActionGroup), typeof(Meta), new PropertyMetadata(null, GroupChangedCallback));

        private static void GroupChangedCallback(DependencyObject dpo, DependencyPropertyChangedEventArgs args)
        {
            var factory = new MetaActionGroupGeneratorFactory((ItemsControl)dpo);
            var generator = factory.Create((MetaActionGroup)args.NewValue);
            generator.GenerateActions();
        }

        public static void SetGroup(DependencyObject element, MetaActionGroup value)
        {
            element.SetValue(GroupProperty, value);
        }

        public static MetaActionGroup GetGroup(DependencyObject element)
        {
            return (MetaActionGroup)element.GetValue(GroupProperty);
        }

        #endregion
    }
}
