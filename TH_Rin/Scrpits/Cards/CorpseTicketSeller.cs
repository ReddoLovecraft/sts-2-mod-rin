using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Patchoulib.Scrpits.Main;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class CorpseTicketSeller : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
           Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Corpse")
        });
	public CorpseTicketSeller() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PowerCmd.Apply<CorpseTicketSellerPower>(Owner.Creature,1,Owner.Creature,this);
	}
	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}

}
