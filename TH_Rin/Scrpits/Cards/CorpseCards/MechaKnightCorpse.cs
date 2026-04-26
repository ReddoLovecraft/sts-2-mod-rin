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
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class MechaKnightCorpse : CorpseCardModel
{
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[3]
        {
			HoverTipFactory.FromPower<IgnitePower>(),
			HoverTipFactory.FromPower<ArtifactPower>(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0),new HpLossVar(0)};
	public MechaKnightCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars["Power"].BaseValue = 12m * GetMutilplier();
		DynamicVars.HpLoss.BaseValue = 3m * GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
	}	
	 public override async Task TriggerWhenCombatStart()
        {
           await PowerCmd.Apply<ArtifactPower>(Owner.Creature, this.DynamicVars.HpLoss.IntValue,Owner.Creature,this);
		   foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies)
		   {
			   await PowerCmd.Apply<IgnitePower>(mos, this.DynamicVars["Power"].IntValue,Owner.Creature,this);
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
