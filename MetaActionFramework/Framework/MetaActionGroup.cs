using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Markup;

namespace MAF.Framework
{
    [ContentProperty("Actions")]
    [DictionaryKeyProperty("GroupName")]
    public class MetaActionGroup : DependencyObject, ICollection<MetaAction>
    {
        public MetaActionGroup()
        {
            MetaActionManager.Instance.RegisterGroup(this);
        }

        #region ICollection

        public IEnumerator<MetaAction> GetEnumerator()
        {
            return Actions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(MetaAction item)
        {
            Actions.Add(item);
        }

        public void Clear()
        {
            Actions.Clear();
        }

        public bool Contains(MetaAction item)
        {
            return Actions.Contains(item);
        }

        public void CopyTo(MetaAction[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(MetaAction item)
        {
            throw new System.NotImplementedException();
        }

        public int Count { get { return Actions.Count; } }

        public bool IsReadOnly { get { return false; } }

        #endregion

        #region Actions

        readonly ObservableCollection<MetaAction> act = new ObservableCollection<MetaAction>();

        public ObservableCollection<MetaAction> Actions { get { return act; } }

        #endregion

        #region GroupName

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(
            "GroupName", typeof(string), typeof(MetaActionGroup), new PropertyMetadata(default(string), GroupName_OnChanged));

        private static void GroupName_OnChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs args)
        {
            if (string.IsNullOrEmpty((string)args.NewValue))
            {
                throw new ArgumentNullException("GroupName must not be null!");
            }
        }

        public string GroupName
        {
            get
            {
                return (string)GetValue(GroupNameProperty);
                
            } 
            set { SetValue(GroupNameProperty, value); }
        }

        #endregion
    }
}
