using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Relics;
using TH_Rin.Scrpits.UI;

namespace TH_Rin.Scrpits.Patches;

[HarmonyPatch(typeof(NSimpleCardSelectScreen), "OnCardClicked")]
public static class BarrowSimpleSelectClickPatch
{
	public static bool Prefix(NSimpleCardSelectScreen __instance, CardModel card)
	{
		if (!BarrowCorpseSelectContext.TryGet(__instance, out IBarrowLike barrow))
		{
			return true;
		}

		TaskHelper.RunSafely(HandleClick(__instance, barrow, card));
		return false;
	}

	private static async Task HandleClick(NSimpleCardSelectScreen screen, IBarrowLike barrow, CardModel card)
	{
		if (card is not CorpseCardModel corpseCard)
		{
			return;
		}

		if (!await ConfirmRemove(barrow))
		{
			return;
		}

		await Tools.RemoveFromBarrowAsync(barrow, corpseCard);

		if (barrow.CorpseCards.Count == 0)
		{
			NOverlayStack.Instance.Remove(screen);
			return;
		}

		RefreshGrid(screen, barrow);
	}

	private static void RefreshGrid(NSimpleCardSelectScreen screen, IBarrowLike barrow)
	{
		NCardGrid grid = screen.GetNode<NCardGrid>("%CardGrid");
		List<SortingOrders> sorting = new List<SortingOrders> { SortingOrders.Ascending };
		grid.SetCards(barrow.CorpseCards.Cast<CardModel>().ToList(), PileType.None, sorting);
	}

	private static async Task<bool> ConfirmRemove(IBarrowLike barrow)
	{
		if (NModalContainer.Instance == null)
		{
			return true;
		}

		if (barrow is not RelicModel relic)
		{
			return true;
		}

		NGenericPopup nGenericPopup = NGenericPopup.Create();
		NModalContainer.Instance.Add(nGenericPopup);
		return await nGenericPopup.WaitForConfirmation(new LocString("relics", $"{relic.Id.Entry}.confirmdelete"), new LocString("relics", $"{relic.Id.Entry}.delete"), new LocString("relics", $"{relic.Id.Entry}.cancel"), new LocString("relics", $"{relic.Id.Entry}.delete"));
	}
}
