using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.Resources;
using TriggersTools.Resources.Menu;

namespace TriggersTools.CatSystem2.Patcher.Patches {
	public abstract class MenuResourcePatch : ResourcePatch<MenuResource> {
		
		/*private static readonly FieldInfo MenuExTemplateItem_header;
		private static readonly FieldInfo MenuTemplateItem_header;

		static MenuResourcePatch() {
			// Fix broken shit preparations
			MenuExTemplateItem_header = typeof(MenuExTemplateItem).GetField("_header", BindingFlags.Instance | BindingFlags.NonPublic);
			MenuTemplateItem_header = typeof(MenuTemplateItem).GetField("_header", BindingFlags.Instance | BindingFlags.NonPublic);
		}*/

		public bool AllowNormal { get; protected set; } = true;
		public bool AllowExtended { get; protected set; } = true;

		public MenuResourcePatch() { }
		public MenuResourcePatch(ResourceId name) : base(name) { }

		public override bool IsPatchable(Resource resource) {
			if (!base.IsPatchable(resource))
				return false;
			MenuResource menuRes = (MenuResource) resource;
			switch (menuRes.Template) {
			case MenuTemplate menu: return AllowNormal;
			case MenuExTemplate menuEx: return AllowExtended;
			}
			return false;
		}
		public override bool Patch(MenuResource menuRes) {
			switch (menuRes.Template) {
			case MenuTemplate menu:
				if (Patch(menu)) {
					//FixClrPlusMenuItems(menu.MenuItems);
					return true;
				}
				break;
			case MenuExTemplate menuEx:
				if (Patch(menuEx)) {
					//FixClrPlusMenuItems(menuEx.MenuItems);
					return true;
				}
				break;
			}
			return false;
		}
		public virtual bool Patch(MenuTemplate menu) => true;
		public virtual bool Patch(MenuExTemplate menuEx) => true;

		/*private static void FixClrPlusMenuItems(List<IMenuTemplateItem> menuItems) {
			for (int i = 0; i < menuItems.Count; i++) {
				IMenuTemplateItem menuItem = menuItems[i];
				var header = (MenuItemTemplate) Constants.MenuTemplateItem_header.GetValue(menuItem);
				if (menuItem is MenuTemplateItemPopup)
					header.mtOption |= unchecked((ushort) MenuFlags.MF_POPUP);
				else
					header.mtOption &= unchecked((ushort) ~MenuFlags.MF_POPUP);
				if (i + 1 == menuItems.Count)
					header.mtOption |= unchecked((ushort) MenuFlags.MF_END);
				else
					header.mtOption &= unchecked((ushort) ~MenuFlags.MF_END);
				Constants.MenuTemplateItem_header.SetValue(menuItem, header);
			}
		}
		private static void FixClrPlusMenuItems(List<IMenuExTemplateItem> menuItems) {
			for (int i = 0; i < menuItems.Count; i++) {
				IMenuExTemplateItem menuItem = menuItems[i];
				var header = (MenuExItemTemplate) Constants.MenuExTemplateItem_header.GetValue(menuItem);
				if (menuItem is MenuExTemplateItemPopup)
					header.bResInfo |= unchecked((ushort) MenuResourceType.Sub);
				else
					header.bResInfo &= unchecked((ushort) ~MenuResourceType.Sub);
				if (i + 1 == menuItems.Count)
					header.bResInfo |= unchecked((ushort) MenuResourceType.Last);
				else
					header.bResInfo &= unchecked((ushort) ~MenuResourceType.Last);
				Constants.MenuExTemplateItem_header.SetValue(menuItem, header);
			}
		}*/
	}
}
