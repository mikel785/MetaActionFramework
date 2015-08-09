using System.Windows;
using System.Windows.Controls;

namespace MAF.Framework
{
    public interface IMetaActionGroupGenerator
    {
        void GenerateActions();
    }

    public class MetaActionGroupGenerator<TItem> : IMetaActionGroupGenerator where TItem : Control, new()
    {
        private readonly ItemsControl groupHost;

        private readonly MetaActionGroup group;

        public MetaActionGroupGenerator(ItemsControl groupHost, MetaActionGroup group)
        {
            this.groupHost = groupHost;
            this.group = group;
        }

        public UIElement GroupHost { get { return groupHost; } }

        public string GroupName { get { return group.GroupName; } }

        public void GenerateActions()
        {
            foreach (var metaAction in group)
            {
                if (metaAction.Placement != ActionPlacement.Everywhere)
                {
                    if (groupHost is MenuItem && !metaAction.Placement.HasFlag(ActionPlacement.Menu))
                        continue;
                    if (groupHost is ToolBar && !metaAction.Placement.HasFlag(ActionPlacement.Toolbar))
                        continue;
                }

                if (metaAction.SeparatorPlacement != ActionPlacement.NoWhere)
                {
                    if ((groupHost is MenuItem && metaAction.SeparatorPlacement.HasFlag(ActionPlacement.Menu)) ||
                        (groupHost is ToolBar && metaAction.SeparatorPlacement.HasFlag(ActionPlacement.Toolbar)))
                        groupHost.Items.Add(new Separator());
                }

                var item = new TItem();
                groupHost.Items.Add(item);
                MetaActionManager.Instance.Attach(metaAction, item);
            }
        }
    }
}
