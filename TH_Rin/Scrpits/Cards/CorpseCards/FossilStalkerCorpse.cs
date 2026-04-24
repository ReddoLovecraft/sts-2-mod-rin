using System.Runtime.Serialization.Json;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class FossilStalkerCorpse : CorpseCardModel
{
	  public override bool GainsBlock => true;
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromPower<StrengthPower>()
        });
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardModfier.NorotKeyword];
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0) };
	public FossilStalkerCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars.Cards.BaseValue = 3m* GetMutilplier();
	}
    public override async Task AfterAttack(AttackCommand command)
	{
		if (command.Attacker != base.Owner.Creature || command.TargetSide == base.Owner.Creature.Side || !command.DamageProps.IsPoweredAttack())
		{
			return;
		}
		List<DamageResult> list = command.Results.ToList();
		List<DamageResult> list2 = list.Where((DamageResult r) => r.Receiver.IsPet).ToList();
		foreach (DamageResult petHit in list2)
		{
			list.RemoveAll((DamageResult r) => r.Receiver == petHit.Receiver.PetOwner?.Creature);
		}
		int num = list.Count((DamageResult r) => r.UnblockedDamage > 0);
		if (num > 0)
		{
			await PowerCmd.Apply<StrengthPower>(base.Owner.Creature, this.DynamicVars.Cards.IntValue * num, base.Owner.Creature, this);
		}
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		
	}
	protected override void OnUpgrade()
	{
		
	}
}

}
