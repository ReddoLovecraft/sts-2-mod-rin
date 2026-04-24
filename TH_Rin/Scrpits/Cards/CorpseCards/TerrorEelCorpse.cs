using System.Runtime.Serialization.Json;
using BaseLib.Utils;
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

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class TerrorEelCorpse : CorpseCardModel
{
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[3]
        {
          HoverTipFactory.FromPower<VulnerablePower>(),
          HoverTipFactory.FromPower<VigorPower>(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0),new HpLossVar(0)};
	public TerrorEelCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars.HpLoss.BaseValue = 99m * GetMutilplier();
		DynamicVars["Power"].BaseValue = 7m * GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
	}	
	public override async Task TriggerWhenCombatStart()
        {
		   foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies)
           {
				if(mos.IsAlive)
                await PowerCmd.Apply<VulnerablePower>(mos, this.DynamicVars.HpLoss.IntValue,Owner.Creature, this);
           }
        }
		public override async Task TriggerWhenTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
          if (player != base.Owner)
            {
                return;
            }
			await PowerCmd.Apply<VigorPower>(base.Owner.Creature, this.DynamicVars["Power"].IntValue,base.Owner.Creature, this);
    }
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		
	}
	protected override void OnUpgrade()
	{
		
	}
}

}
