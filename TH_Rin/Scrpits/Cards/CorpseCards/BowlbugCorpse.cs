using System.Runtime.Serialization.Json;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scripts.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class BowlbugCorpse : CorpseCardModel
{
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<RitualPower>(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0),new EnergyVar(0),new HpLossVar(0)};
	public BowlbugCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars["Power"].BaseValue = 16m * GetMutilplier();
		DynamicVars.Energy.BaseValue = 3m * GetMutilplier();
		DynamicVars.HpLoss.BaseValue = 8m * GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
	}
	public override async Task TriggerWhenTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
          if (player != base.Owner)
            {
                return;
            }
			 Rng rng = player.RunState.Rng.CombatCardGeneration;
            int randomNumber = rng.NextInt(1, 5);
			switch (randomNumber)
			{
				case 1:
					Creature creature = base.Owner.RunState.Rng.CombatTargets.NextItem(base.Owner.Creature.CombatState.HittableEnemies);
				if (creature != null)
				{
			  		await CreatureCmd.Damage(choiceContext,creature, DynamicVars["Power"].IntValue,ValueProp.Unpowered,null,null);
				}
					break;
				case 2:
					Creature creature2 = base.Owner.RunState.Rng.CombatTargets.NextItem(base.Owner.Creature.CombatState.HittableEnemies);
					if (creature2 != null)
					{
			  		await CreatureCmd.Damage(choiceContext,creature2, DynamicVars.HpLoss.IntValue,ValueProp.Unpowered,null,null);
					}
					await CreatureCmd.GainBlock(Owner.Creature,new BlockVar(DynamicVars.HpLoss.IntValue,ValueProp.Unpowered),null);
					break;
				case 3:
					await PlayerCmd.GainEnergy(this.DynamicVars.Energy.IntValue,base.Owner);
					break;
				case 4:
				Creature creature3 = base.Owner.RunState.Rng.CombatTargets.NextItem(base.Owner.Creature.CombatState.HittableEnemies);
				if (creature3 != null)
				{
					Vector2? vector = null;
                    NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(creature3);
					if (!vector.HasValue || vector.Value.X > creatureNode.GlobalPosition.X)
					{
						vector = creatureNode.GlobalPosition;
					}
					NCreature creatureNode2 = NCombatRoom.Instance.GetCreatureNode(base.Owner.Creature);
					Node2D specialNode = creatureNode2.GetSpecialNode<Node2D>("Visuals/SpineBoneNode");
				if (specialNode != null)
					{
						specialNode.Position = Vector2.Right * (vector.Value.X - creatureNode2.GlobalPosition.X) * 4f;
					}
					await PowerCmd.Apply<EntangledPower>(creature3, 1,base.Owner.Creature, this);
				}
					break;
				default:
					break;
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
