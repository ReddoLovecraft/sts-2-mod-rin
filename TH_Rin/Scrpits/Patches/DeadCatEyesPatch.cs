using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using TH_Rin.Relics;
using static HarmonyLib.AccessTools;

namespace TH_Rin.Scripts.Patches;

[HarmonyPatch]
public static class CardPileOrderPatches
{
	private static readonly FieldRef<NCardPileScreen, NCardGrid> _pileScreenGridRef =
		FieldRefAccess<NCardPileScreen, NCardGrid>("_grid");


	[HarmonyPatch(typeof(NCardPileScreen), "OnPileContentsChanged")]
	[HarmonyPrefix]
	private static bool NCardPileScreen_OnPileContentsChanged_Prefix(NCardPileScreen __instance)
	{
		if (__instance.Pile.Type != PileType.Draw)
		{
			return true;
		}

		Player? owner = __instance.Pile.Cards.FirstOrDefault()?.Owner;
		if (owner?.GetRelic<DeadCatEyes>() == null)
		{
			return true;
		}

		List<CardModel> list = __instance.Pile.Cards.ToList();
		NCardGrid grid = _pileScreenGridRef(__instance);
		grid.SetCards(list, __instance.Pile.Type, new List<SortingOrders> { SortingOrders.Ascending });
		return false;
	}
}

