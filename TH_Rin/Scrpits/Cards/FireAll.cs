using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class FireAll : RinCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
    {
          HoverTipFactory.FromPower<IgnitePower>(),
          HoverTipFactory.FromCard<Burn>()
    });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(7)];
	public FireAll() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		List<CardModel> list = PileType.Hand.GetPile(base.Owner).Cards.ToList();
		int cardCount = list.Count;
		foreach (CardModel item in list)
		{
			await CardCmd.Exhaust(choiceContext, item);
		}
		foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies)
		{
          if(mos.IsAlive)
		  {
			float scale = 0.8f;
			NGroundFireVfx nGroundFireVfx = NGroundFireVfx.Create(mos);
			if (nGroundFireVfx == null)
			{
					return;
			}
			SfxCmd.Play("event:/sfx/characters/attack_fire");
			nGroundFireVfx.Scale = Vector2.One * scale;
			NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nGroundFireVfx);
			scale += 0.1f;
			await PowerCmd.Apply<IgnitePower>(mos, this.DynamicVars.Cards.IntValue*cardCount,Owner.Creature,this);
		  }
		}
		await CardPileCmd.AddToCombatAndPreview<Burn>(Owner.Creature, PileType.Hand, cardCount, addedByPlayer: true);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(3);
	}
}

}
