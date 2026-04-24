using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class GhostFire : RinCardModel,ITranscendenceCard
{
     protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<WraithPower>(),
		  HoverTipFactory.FromPower<IgnitePower>()
        });

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
	public GhostFire() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		 await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		 await PowerCmd.Apply<WraithPower>(base.Owner.Creature, this.DynamicVars.Cards.IntValue,Owner.Creature,this);
	}
	public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
	{
		if (card == this)
		{
			Creature creature = base.Owner.RunState.Rng.CombatTargets.NextItem(base.Owner.Creature.CombatState.HittableEnemies);
			if (creature != null)
			{
				await Cmd.Wait(0.25f);
				int cnt=Owner.Creature.HasPower<WraithPower>()?Owner.Creature.GetPowerAmount<WraithPower>()+1:1;
				await PowerCmd.Apply<IgnitePower>(creature, cnt, Owner.Creature,this);
			}
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1); 
	}

        public CardModel GetTranscendenceTransformedCard()
        {
           return ModelDb.Card<WraithFire>();
        }
    }

}
