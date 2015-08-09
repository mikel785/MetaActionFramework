using System.Linq;
using Caliburn.Micro;
using MAF.TestApp.Demos.ToolBarAndMenuSample;

namespace MAF.TestApp.Shell
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public ShellViewModel()
        {
            DisplayName = "MetaActionFramework Test application";
            Items.Add(new DemoViewModel());
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ActivateItem(Items.First());
        }
    }
}
