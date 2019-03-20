using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.Windows.Resources;
using TriggersTools.Windows.Resources.Menu;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public class MenuStringsPatch : MenuResourcePatch {
		
		public IReadOnlyDictionary<string, string> Translation { get; }
		
		public MenuStringsPatch(IReadOnlyDictionary<string, string> translations, ResourceId name) : base(name) {
			Translation = translations;
		}

		public override bool Patch(MenuTemplate menu) {
			return PatchMenuItems(menu.MenuItems);
		}
		public override bool Patch(MenuExTemplate menuEx) {
			return PatchMenuItems(menuEx.MenuItems);
		}
		private bool PatchMenuItems(IReadOnlyList<IMenuBaseTemplateItem> menuItems) {
			foreach (var menuItem in menuItems) {
				if (StringsScraper.IsNormalString(menuItem.MenuString)) {
					string id = StringsScraper.GetResId(menuItem);
					if (Translation.TryGetValue(id, out string translation))
						menuItem.MenuString = translation;
				}
				if (menuItem is IMenuBaseTemplateItemPopup popup) {
					if (!PatchMenuItems(popup.MenuItems))
						return false;
				}
			}
			return true;
		}
		/*private bool PatchMenuItems(List<IMenuTemplateItem> menuItems) {
			foreach (var menuItem in menuItems) {
				if (StringsScraper.IsNormalString(menuItem.MenuString)) {
					string id = StringsScraper.GetResId(menuItem);
					if (Translation.TryGetValue(id, out string translation))
						menuItem.MenuString = translation;
				}
				if (menuItem is MenuTemplateItemPopup popup) {
					if (!PatchMenuItems(popup.MenuItems))
						return false;
				}
			}
			return true;
		}
		private bool PatchMenuItems(List<IMenuExTemplateItem> menuItemsEx) {
			foreach (var menuItem in menuItemsEx) {
				if (StringsScraper.IsNormalString(menuItem.MenuString)) {
					string id = StringsScraper.GetResId(menuItem);
					if (Translation.TryGetValue(id, out string translation))
						menuItem.MenuString = translation;
				}
				if (menuItem is MenuExTemplateItemPopup popupEx) {
					if (!PatchMenuItems(popupEx.MenuItems))
						return false;
				}
			}
			return true;
		}*/
	}
}
