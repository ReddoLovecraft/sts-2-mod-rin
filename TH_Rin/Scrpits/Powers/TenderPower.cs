using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TH_Rin.Scrpits.Powers
{
public sealed class TenderPower : CustomPowerModel
{
	private int _cardsPlayedThisTurn;

	   public override PowerType Type => PowerType.Debuff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/TP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/TP64.png";
	public override int DisplayAmount => CardsPlayedThisTurn;

	protected override IEnumerable<IHoverTip> ExtraHoverTips =>(new IHoverTip[2]
	{
		HoverTipFactory.FromPower<StrengthPower>(),
		HoverTipFactory.FromPower<DexterityPower>()
	});

	private int CardsPlayedThisTurn
	{
		get
		{
			return _cardsPlayedThisTurn;
		}
		set
		{
			AssertMutable();
			_cardsPlayedThisTurn = value;
			InvokeDisplayAmountChanged();
		}
	}

	public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
	{
			CardsPlayedThisTurn+=Amount;
			Flash();
			await PowerCmd.Apply<StrengthPower>(base.Owner, -Amount, base.Applier, null, silent: true);
			await PowerCmd.Apply<DexterityPower>(base.Owner, -Amount, base.Applier, null, silent: true);
	}

	public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
	{
		if (side == CombatSide.Enemy)
		{
			await PowerCmd.Apply<StrengthPower>(base.Owner, CardsPlayedThisTurn, base.Applier, null, silent: true);
			await PowerCmd.Apply<DexterityPower>(base.Owner, CardsPlayedThisTurn, base.Applier, null, silent: true);
			CardsPlayedThisTurn = 0;
		}
	}
}
}