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
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class ShrinkerBeetleCorpse : CorpseCardModel
{
		    protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new HpLossVar(0) };
	public ShrinkerBeetleCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars.HpLoss.BaseValue = 25m * GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
	}
	 public override async Task TriggerWhenCombatStart()
        {
		   VfxCmd.PlayOnCreatureCenters(Owner.Creature.CombatState.HittableEnemies, "vfx/vfx_attack_slash");
           foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies)
		   {
			  int lossAmount=mos.MaxHp*this.DynamicVars.HpLoss.IntValue/100;
			  await CreatureCmd.Damage(new BlockingPlayerChoiceContext(),mos,lossAmount,ValueProp.Unblockable|ValueProp.Unpowered,null,null);
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
