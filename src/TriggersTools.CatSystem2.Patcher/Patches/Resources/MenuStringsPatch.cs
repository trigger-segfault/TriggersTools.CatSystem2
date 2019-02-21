using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClrPlus.Windows.PeBinary.ResourceLib;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public class MenuStringsPatch : MenuResourcePatch {
		
		public IReadOnlyDictionary<string, string> Translation { get; }

		public MenuStringsPatch(string[] lines, string name) : base(name) {
			Translation = StringsScraper.BuildTranslation(lines);
		}
		public MenuStringsPatch(string[] lines, ushort name) : base(name) {
			Translation = StringsScraper.BuildTranslation(lines);
		}

		public override bool Patch(MenuTemplate menu) {
			return PatchMenuItems(menu.MenuItems);
		}
		public override bool Patch(MenuExTemplate menuEx) {
			return PatchMenuItems(menuEx.MenuItems);
		}
		private bool PatchMenuItems(MenuTemplateItemCollection menuItems) {
			foreach (var menuItem in menuItems) {
				if (StringsScraper.IsNormalString(menuItem.MenuString)) {
					string id = StringsScraper.GetResId(menuItem);
					if (Translation.TryGetValue(id, out string translation))
						menuItem.MenuString = translation;
				}
				if (menuItem is MenuTemplateItemPopup popup) {
					if (!PatchMenuItems(popup.SubMenuItems))
						return false;
				}
			}
			return true;
		}
		private bool PatchMenuItems(MenuExTemplateItemCollection menuItemsEx) {
			foreach (var menuItem in menuItemsEx) {
				if (StringsScraper.IsNormalString(menuItem.MenuString)) {
					string id = StringsScraper.GetResId(menuItem);
					if (Translation.TryGetValue(id, out string translation))
						menuItem.MenuString = translation;
				}
				if (menuItem is MenuExTemplateItemPopup popupEx) {
					if (!PatchMenuItems(popupEx.SubMenuItems))
						return false;
				}
			}
			return true;
		}
	}
}
