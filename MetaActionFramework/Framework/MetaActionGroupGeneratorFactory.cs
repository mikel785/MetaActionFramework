using System;
using System.Windows.Controls;

namespace MAF.Framework
{
    public class MetaActionGroupGeneratorFactory
    {
        private readonly ItemsControl groupHost;

        public MetaActionGroupGeneratorFactory(ItemsControl groupHost)
        {
            this.groupHost = groupHost;
        }

        public IMetaActionGroupGenerator Create(MetaActionGroup group)
        {
            if (groupHost is ToolBar)
                return new MetaActionGroupGenerator<Button>(groupHost, group);
            if (groupHost is MenuItem)
                return new MetaActionGroupGenerator<MenuItem>(groupHost, group);

            throw new NotSupportedException("Not supproted host type: " + groupHost.GetType().Name);
        }
    }
}
