using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Caliburn.Micro;
using MAF.Helpers;
using BooleanToVisibilityConverter = MAF.Converters.BooleanToVisibilityConverter;

namespace MAF.Framework
{
    public class MetaActionManager
    {
        private static MetaActionManager instance;
        private static readonly object LockObject = new object();

        private HashSet<MetaAction> actions;
 
        private List<Tuple<MetaAction, WeakReference<Control>>> controlAction;

        private HashSet<MetaActionGroup> groups; 
 
        protected MetaActionManager()
        {
            actions = new HashSet<MetaAction>();
            controlAction = new List<Tuple<MetaAction, WeakReference<Control>>>();
            groups = new HashSet<MetaActionGroup>();
        }

        public static MetaActionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (LockObject)
                    {
                        if (instance == null)
                            instance = new MetaActionManager();
                    }
                }
                return instance;
            }
        }

        public void Attach(MetaAction metaAction, Control control)
        {
            ValidateType(control);

            var existingItems = controlAction.Where(it => it.Item1 == metaAction);
            var targetItem = existingItems.FirstOrDefault(it =>
            {
                Control c;
                if (it.Item2.TryGetTarget(out c))
                {
                    return c.Equals(control);
                }
                return false;
            });

            // already attached
            if (targetItem != null)
                return;
            Image image = null;
            if (metaAction.Icon != null)
            {
                image = ImageHelper.CreateImage(metaAction);
                image.MaxWidth = MetaAction.ToolBarIconSize;
                image.MaxHeight = MetaAction.ToolBarIconSize;
            }

            if (control is MenuItem)
            {
                var menu = (MenuItem)control;
                menu.Icon = image;
                menu.InputGestureText = metaAction.ShortCut;
                menu.Header = metaAction.Title;
                menu.ToolTip = metaAction.ToolTip;
            }
            if (control is ButtonBase)
            {
                var button = (ButtonBase)control;
                button.Content = image;
                button.ToolTip = metaAction.ToolTip;
                if (!string.IsNullOrEmpty(metaAction.ShortCut))
                {
                    button.ToolTip += ": " + metaAction.ShortCut;
                }
            }

            control.SetValue(ToolTipService.ShowOnDisabledProperty, true);
            // IsEnabled property

            // Visibility
            control.SetBinding(UIElement.VisibilityProperty,
                new Binding
                {
                    Source = metaAction,
                    Path = new PropertyPath("IsVisible"),
                    Mode = BindingMode.OneWay,
                    Converter = BooleanToVisibilityConverter.Instance,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

            // Shortcut
            if (metaAction.KeyBinding != null)
            {
                var window = GetWindow(control);
                if (!window.InputBindings.Contains(metaAction.KeyBinding))
                {
                    window.InputBindings.Add(metaAction.KeyBinding);
                }
            }
          //  Message.SetAttach(control, metaAction.CaliburnAction);
            control.SetValue(Message.AttachProperty, metaAction.CaliburnAction);
            controlAction.Add(new Tuple<MetaAction, WeakReference<Control>>(metaAction, new WeakReference<Control>(control)));
        }

        public void Detach(MetaAction metaAction, Control control)
        {
            ValidateType(control);

            BindingOperations.ClearBinding(control, UIElement.IsEnabledProperty);
            BindingOperations.ClearBinding(control, UIElement.VisibilityProperty);

            Message.SetAttach(control, string.Empty);

            // Shortcut
            if (metaAction.KeyBinding != null)
            {
                var window = GetWindow(control);
                if (window.InputBindings.Contains(metaAction.KeyBinding))
                {
                    window.InputBindings.Remove(metaAction.KeyBinding);
                }
            }
        }

        internal void RegisterAction(MetaAction action)
        {
            actions.Add(action);
        }

        internal void RegisterGroup(MetaActionGroup group)
        {
            groups.Add(group);
        }

        /// <summary>
        /// Unregister action and remove it from all controls.
        /// </summary>
        /// <param name="action"></param>
        internal void UnregisterAction(MetaAction action)
        {
            actions.Remove(action);
        }

        public IEnumerable<Control> GetControls(MetaAction metaAction)
        {
            return controlAction.Where(it => Equals(it.Item1, metaAction)).Select(it =>
            {
                Control c;
                if (it.Item2.TryGetTarget(out c))
                {
                    return c;
                }
                // if failed to get reference target
                return null;
            }).Where(it => it != null).ToArray();
        }

        public MetaAction GetAction(object controlObject)
        {
            var target = controlAction.FirstOrDefault(it =>
            {
                Control c;
                return (it.Item2.TryGetTarget(out c) && ReferenceEquals(c, controlObject));
            });
            if (target != null)
                return target.Item1;
            return null;
        }

        /// <summary>
        /// Get all registered actions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MetaAction> GetActions()
        {
            return controlAction.Select(c => c.Item1).Distinct().ToArray();
        }

        private void ValidateType(Control control)
        {
            if (control is ButtonBase || control is MenuItem)
                return;

            throw new NotSupportedException("Type not supported: " + control.GetType().Name + "!");
        }

        public IEnumerable<MetaActionGroup> GetGroups()
        {
            return groups;
        }

        private static Window GetWindow(FrameworkElement frameworkElement)
        {
            FrameworkElement parent = frameworkElement;
            while (parent != null && !(parent is Window))
            {
                FrameworkElement current = parent;
                parent = parent.Parent as FrameworkElement;
                if (parent == null)
                {
                    parent = current.TemplatedParent as FrameworkElement;
                }
            }
            if (parent == null)
                parent = Application.Current.MainWindow;

            Debug.Assert(parent != null);

            return (Window)parent;
        }
    }
}
