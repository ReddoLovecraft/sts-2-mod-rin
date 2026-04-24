using System.Linq;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Relics;
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scrpits.Patches;

[HarmonyPatch(typeof(NRelicInventory), "Add", new[] { typeof(RelicModel), typeof(bool), typeof(int) })]
public static class RelicInventoryRightClickPatch
{
	public static void Postfix(NRelicInventory __instance, RelicModel relic)
	{
		NRelicInventoryHolder? holder = __instance.RelicNodes.FirstOrDefault(n => n.Relic.Model == relic);
		if (holder == null)
		{
			return;
		}

		holder.Connect(MegaCrit.Sts2.Core.Nodes.GodotExtensions.NClickableControl.SignalName.MousePressed, Callable.From<InputEvent>(delegate(InputEvent e)
		{
			if (e is InputEventMouseButton { ButtonIndex: MouseButton.Right, Pressed: true })
			{
				if (relic is Barrow barrow)
				{
					barrow.OpenCorpseScreen();
				}
			}
		}), 4u);
	}
}
