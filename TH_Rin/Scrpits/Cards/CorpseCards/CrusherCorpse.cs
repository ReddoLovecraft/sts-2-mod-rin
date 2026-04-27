using System.Runtime.Serialization.Json;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class CrusherCorpse : CorpseCardModel
{
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[4]
        {
		  HoverTipFactory.FromCard<RocketCorpse>(),
          HoverTipFactory.FromPower<VulnerablePower>(),
		  HoverTipFactory.FromPower<WeakPower>(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0),new HpLossVar(0)};
	public CrusherCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars["Power"].BaseValue = 1m * GetMutilplier();
		DynamicVars.HpLoss.BaseValue = 9m * GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
	}
		public override async Task TriggerWhenTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
		if (player != base.Owner)
        {
                return;
        }
		foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies)
		{
			if(mos.IsAlive)
			{
				int muti=1;
				IBarrowLike? barrow = Tools.GetBarrowRelic(Owner);
				if (barrow != null)
				{
					muti = barrow.CorpseCards.FindAll(x => x is RocketCorpse).Count > 0 ? 2 : 1;
				}
				await PowerCmd.Apply<WeakPower>(mos, DynamicVars["Power"].BaseValue*muti,Owner.Creature,this);
				await PowerCmd.Apply<VulnerablePower>(mos, DynamicVars["Power"].BaseValue*muti,Owner.Creature,this);
			}
		}
    }
		 public override async Task TriggerWhenTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
			if(side==CombatSide.Enemy)
			{
				return;
			}
			int muti=1;
			IBarrowLike? barrow = Tools.GetBarrowRelic(Owner);
			if (barrow != null)
			{
				muti = barrow.CorpseCards.FindAll(x => x is RocketCorpse).Count > 0 ? 2 : 1;
			}
			  await CreatureCmd.GainBlock(Owner.Creature,new BlockVar(DynamicVars.HpLoss.IntValue*muti,ValueProp.Unpowered),null);
		}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		
	}
	protected override void OnUpgrade()
	{
		
	}
}

}
