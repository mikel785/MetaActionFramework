using System;

namespace MAF.Framework
{
    /// <summary>
    /// Restrict area of action placement.
    /// </summary>
    [Flags]
    public enum ActionPlacement
    {
        NoWhere = 0x00,

        /// <summary>
        /// MenuItem.
        /// </summary>
        Menu = 0x01,

        /// <summary>
        /// ToolBar.
        /// </summary>
        Toolbar = 0x02,

        Everywhere = Menu | Toolbar,
    }
}
