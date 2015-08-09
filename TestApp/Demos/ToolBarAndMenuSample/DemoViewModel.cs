using System.Windows;
using Caliburn.Micro;
using MAF.Attached;
using MAF.Framework;

namespace MAF.TestApp.Demos.ToolBarAndMenuSample
{
    public class DemoViewModel : Screen
    {
        public DemoViewModel()
        {
            DisplayName = "Unified toolbar and menu actions demo";
        }

        public string DemoNote { get; set; }

        #region Demo actions

        #region File

        /// <summary>
        /// I am lazy to write especial handler for each action =)
        /// </summary>
        public void HandleAction(object sender)
        {
            var meta = MetaActionManager.Instance.GetAction(sender);
            if (meta != null)
            {
                MessageBox.Show(string.Format("Handled meta action exec from \'{0}\'!", sender.GetType().Name),
                    string.Format("Hello from \'{0}\'", meta.Title));
            }
        }

        #endregion

        #endregion
    }
}
