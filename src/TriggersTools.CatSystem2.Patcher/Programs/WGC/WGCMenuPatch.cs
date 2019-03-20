using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.Windows.Resources.Menu;

namespace TriggersTools.CatSystem2.Patcher.Programs.WGC {
	internal sealed class WGCMenuPatch : MenuResourcePatch {
		public WGCMenuPatch(ushort name) : base(name) {
			AllowExtended = false;
		}

		public override bool Patch(MenuTemplate menu) {
			var menuItems = menu.MenuItems;

			// Having a menu dropdown as a button just feels wrong.
			// Let's move the settings item into a dropdown menu.
			int index = menuItems.FindIndex(mi => {
				return (mi is MenuTemplateItemCommand command && command.MenuId == 32790);
			});
			if (index == -1)
				return false;
			MenuTemplateItemCommand settings = (MenuTemplateItemCommand) menuItems[index];
			MenuTemplateItemPopup options = new MenuTemplateItemPopup {
				MenuString = "&Options", // Placeholder, is replaced in Menu.txt strings
			};
			options.MenuItems.Add(settings);
			menuItems[index] = options;

			return true;
		}
	}
}
