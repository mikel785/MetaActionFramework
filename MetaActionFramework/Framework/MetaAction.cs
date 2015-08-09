using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Caliburn.Micro;
using PropertyChanged;

namespace MAF.Framework
{
    [ImplementPropertyChanged]
    public class MetaAction : DependencyObject, ICommand
    {
        #region Private

        private KeyBinding keyBinding;

        #endregion

        #region ctor

        static MetaAction()
        {
            ToolBarIconSize = 32;
        }

        public MetaAction()
        {
            SeparatorPlacement = ActionPlacement.NoWhere;
            IsVisible = true;
            IsEnabled = true;
            MetaActionManager.Instance.RegisterAction(this);
        }

        #endregion

        #region Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(MetaAction), new PropertyMetadata(default(string)));

        public string Title { get { return (string)GetValue(TitleProperty); } set { SetValue(TitleProperty, value); } }

        #endregion

        #region Icon

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(Bitmap), typeof(MetaAction), new PropertyMetadata(default(Bitmap)));

        public Bitmap Icon { get { return (Bitmap)GetValue(IconProperty); } set { SetValue(IconProperty, value); } }

        public static int ToolBarIconSize { get; set; }

        #endregion

        #region Shortcut

        public static readonly DependencyProperty ShortCutProperty = DependencyProperty.Register(
            "ShortCut", typeof(string), typeof(MetaAction), new PropertyMetadata(default(string)));

        public string ShortCut { get { return (string)GetValue(ShortCutProperty); } set { SetValue(ShortCutProperty, value); } }

        public KeyBinding KeyBinding
        {
            get
            {
                if (keyBinding == null && !string.IsNullOrEmpty(ShortCut))
                {
                    var triggerDetail = ShortCut.Trim();

                    var modKeys = ModifierKeys.None;

                    var allKeys = triggerDetail.Split('+').ToList();

                    for (var i = 0; i < allKeys.Count; ++i)
                    {
                        if (allKeys[i].Equals("Ctrl", StringComparison.InvariantCultureIgnoreCase))
                            allKeys[i] = "Control";
                    }

                    var key = (Key)Enum.Parse(typeof(Key), allKeys.Last());

                    foreach (var modifierKey in allKeys.Take(allKeys.Count() - 1))
                    {
                        modKeys |= (ModifierKeys)Enum.Parse(typeof(ModifierKeys), modifierKey);
                    }

                    keyBinding = new KeyBinding(this, key, modKeys);
                    KeyBinding.Command = this;
                }
                return keyBinding;
            }
        }

        #endregion

        #region ToolTip

        public static readonly DependencyProperty ToolTipProperty = DependencyProperty.Register(
            "ToolTip", typeof(string), typeof(MetaAction), new PropertyMetadata(default(string)));

        public string ToolTip { get { return (string)GetValue(ToolTipProperty); } set { SetValue(ToolTipProperty, value); } }

        #endregion

        #region IsEnabled

        public bool IsEnabled { get; set; }

        public static readonly DependencyProperty IsEnabledWatchOnProperty = DependencyProperty.Register(
            "IsEnabledWatchOn", typeof(string), typeof(MetaAction), new PropertyMetadata(default(string)));

        public string IsEnabledWatchOn { get { return (string)GetValue(IsEnabledWatchOnProperty); } set { SetValue(IsEnabledWatchOnProperty, value); } }

        #endregion

        #region IsVisible

        public static readonly DependencyProperty IsVisibleWathOnProperty = DependencyProperty.Register(
            "IsVisibleWathOn", typeof(string), typeof(MetaAction), new PropertyMetadata(default(string)));

        /// <summary>
        /// Property name in DataContext to watch for visibility.
        /// </summary>
        public BindingBase IsVisibleWathOn { get { return (BindingBase)GetValue(IsVisibleWathOnProperty); } set { SetValue(IsVisibleWathOnProperty, value); } }

        public bool IsVisible { get; set; }

        #endregion

        #region Action

        /// <summary>
        /// Caliburn.Message attach.
        /// </summary>
        public static readonly DependencyProperty CaliburnActionProperty = DependencyProperty.Register(
            "CaliburnAction", typeof(string), typeof(MetaAction), new PropertyMetadata(default(string)));

        public string CaliburnAction { get { return (string)GetValue(CaliburnActionProperty); } set { SetValue(CaliburnActionProperty, value); } }

        public static readonly DependencyProperty CaliburnActionTargetProperty = DependencyProperty.Register(
            "CaliburnActionTarget", typeof(object), typeof(MetaAction), new PropertyMetadata(default(object)));

        public object CaliburnActionTarget { get { return (object)GetValue(CaliburnActionTargetProperty); } set { SetValue(CaliburnActionTargetProperty, value); } }

        #endregion

        #region Placement

        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
            "Placement", typeof(ActionPlacement), typeof(MetaAction), new PropertyMetadata(ActionPlacement.Everywhere));

        public ActionPlacement Placement { get { return (ActionPlacement)GetValue(PlacementProperty); } set { SetValue(PlacementProperty, value); } }

        #endregion

        #region Separator

        /// <summary>
        /// Place separator before actions projection.
        /// By default separator is not placed.
        /// </summary>
        public ActionPlacement SeparatorPlacement { get; set; }

        #endregion

        #region Methods

        #endregion

        #region ICommand

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            if (!string.IsNullOrEmpty(CaliburnAction))
            {
                var methodName = CaliburnAction;
                if (methodName.EndsWith(""))
                    methodName = methodName.Remove(methodName.Length - 2);

                // will be overridden by Caliburn.
                var controls = MetaActionManager.Instance.GetControls(this);
                var source = controls.First();

                var trigger = Parser.Parse(source, CaliburnAction).FirstOrDefault();
                var action = trigger.Actions.OfType<ActionMessage>().FirstOrDefault();

                var context = new ActionExecutionContext
                              {
                                  EventArgs = new EventArgs(),
                                  Message = action,
                                  Source = source,
                              };
                ActionMessage.PrepareContext(context);

                if (context.CanExecute == null || context.CanExecute())
                    ActionMessage.InvokeAction(context);
            }
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}
