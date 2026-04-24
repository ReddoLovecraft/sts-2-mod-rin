using System.Runtime.Serialization.Json;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
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
public class QueenCorpse : CorpseCardModel
{
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[4]
        {
          HoverTipFactory.FromPower<WeakPower>(),
		  HoverTipFactory.FromPower<FrailPower>(),
		  HoverTipFactory.FromPower<VulnerablePower>(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0)};
	public QueenCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars["Power"].BaseValue = 99m * GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
	}	
	public override async Task TriggerWhenCombatStart()
        {
		   foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies)
		   {
			 if(mos.IsAlive)
			 {
				await PowerCmd.Apply<WeakPower>(mos, this.DynamicVars["Power"].IntValue,Owner.Creature, this);
				await PowerCmd.Apply<FrailPower>(mos, this.DynamicVars["Power"].IntValue,Owner.Creature, this);
				await PowerCmd.Apply<VulnerablePower>(mos, this.DynamicVars["Power"].IntValue,Owner.Creature, this);
			 }
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
