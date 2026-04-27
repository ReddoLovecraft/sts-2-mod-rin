using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Relics;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Cards;
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scrpits.Powers
{
    public sealed class WraithPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/WP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/WP64.png";
		protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(4)];
        public void SetDamage(int damage)
        {
            DynamicVars.Cards.BaseValue=damage;
        }
		public void AddDamage(int damage)
        {
            DynamicVars.Cards.BaseValue+=damage;
        }
		public async Task TriggerExhasut(int value)
		{
			if(Owner.HasPower<NoDeclineSoulPower>())
			{
				return;
			}
			int FinalValue=Math.Min(value,this.Amount);
			if(Owner.HasPower<UnceasingResentmentPower>())
				await PowerCmd.Apply<WraithPower>(Owner,Owner.GetPowerAmount<UnceasingResentmentPower>(),Owner,null);
			if(Owner.HasPower<CirclePower>())
			    await PlayerCmd.GainEnergy(Owner.GetPowerAmount<CirclePower>(),Owner.Player);
			if(Owner.HasPower<LycorisPower>())
				await CreatureCmd.GainMaxHp(Owner,FinalValue*Owner.GetPowerAmount<LycorisPower>());
			if(Owner.HasPower<PurePower>())
			{
				List<Soul> cards = Soul.Create(Owner.Player,Owner.GetPowerAmount<PurePower>()*FinalValue, base.CombatState).ToList();
				CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Draw, addedByPlayer: true, CardPilePosition.Random));
			}
			await PowerCmd.ModifyAmount(this,-FinalValue,null,null);
		}
        public WraithPower() { }

		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (!CombatManager.Instance.IsInProgress)
		{
			await Task.CompletedTask;
			return;
		}
		if (target == base.Owner)
		{
			await Task.CompletedTask;
			return;
		}
		if(!props.IsPoweredAttack_())
		{
			await Task.CompletedTask;
			return;
		}
        if(dealer==null||dealer!=base.Owner)
        {
			await Task.CompletedTask;
			return;
		}
        Flash();
        // 类似人魂灯特效，大概
	    if	(SaveManager.Instance.PrefsSave.FastMode != FastModeType.Fast)
        await NWraithOrbVfx.Play(base.Owner, target);
		bool flag=Owner.Player.GetRelic<PowerCardCar>()!=null;
		if(!Owner.HasPower<RelyOnPower>())
        	{
				if(!flag)
					await CreatureCmd.Damage(choiceContext,target, DynamicVars.Cards.IntValue, ValueProp.Unblockable | ValueProp.Unpowered,null,null);
				else
					await CreatureCmd.Damage(choiceContext,base.Owner.CombatState.HittableEnemies, DynamicVars.Cards.IntValue, ValueProp.Unblockable | ValueProp.Unpowered,null,null);
				if(target!=null&&target.IsAlive&&Owner.HasPower<BrandPower>())
				{
					await PowerCmd.Apply<IgnitePower>(target,DynamicVars.Cards.IntValue,null,null);
				}
			}
		else
		{
			 if(Owner.HasPower<StrengthPower>())
				{

					if(!flag)
					await CreatureCmd.Damage(choiceContext, target, DynamicVars.Cards.IntValue+Owner.GetPowerAmount<StrengthPower>(), ValueProp.Unblockable | ValueProp.Unpowered,null,null);
					else
					await CreatureCmd.Damage(choiceContext,base.Owner.CombatState.HittableEnemies, DynamicVars.Cards.IntValue+Owner.GetPowerAmount<StrengthPower>(), ValueProp.Unblockable | ValueProp.Unpowered,null,null);
					if(target!=null&&target.IsAlive&&Owner.HasPower<BrandPower>())
					{
					await PowerCmd.Apply<IgnitePower>(target,DynamicVars.Cards.IntValue+Owner.GetPowerAmount<StrengthPower>(),null,null);
					}
				}
			 else
				{
					if(!flag)
					await CreatureCmd.Damage(choiceContext,target, DynamicVars.Cards.IntValue, ValueProp.Unblockable | ValueProp.Unpowered,null,null);
					else
					await CreatureCmd.Damage(choiceContext,base.Owner.CombatState.HittableEnemies, DynamicVars.Cards.IntValue, ValueProp.Unblockable | ValueProp.Unpowered,null,null);
					if(target!=null&&target.IsAlive&&Owner.HasPower<BrandPower>())
					{
					await PowerCmd.Apply<IgnitePower>(target,DynamicVars.Cards.IntValue,null,null);
					}
				}
		}
		IBarrowLike? barrow = Tools.GetBarrowRelic(Owner.Player);
		if (barrow != null && barrow.CorpseCards.FindAll(x => x is SoulNexusCorpse).Count > 0)
		{
			List<Soul> cards = Soul.Create(base.Owner.Player, 1, base.CombatState).ToList();
			CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Draw, addedByPlayer: true, CardPilePosition.Random));
		}
			if(!Owner.HasPower<NoDeclineSoulPower>())
			{
				if(Owner.HasPower<UnceasingResentmentPower>())
					await PowerCmd.Apply<WraithPower>(Owner,1,Owner,null);
				if(Owner.HasPower<CirclePower>())
			    	await PlayerCmd.GainEnergy(Owner.GetPowerAmount<CirclePower>(),Owner.Player);
				if(Owner.HasPower<LycorisPower>())
					await CreatureCmd.GainMaxHp(Owner,Owner.GetPowerAmount<LycorisPower>());
				if(Owner.HasPower<PurePower>())
				{
				List<Soul> cards = Soul.Create(Owner.Player,Owner.GetPowerAmount<PurePower>(), base.CombatState).ToList();
				CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Draw, addedByPlayer: true, CardPilePosition.Random));
				}
				await PowerCmd.Decrement(this);
			}
        await Task.CompletedTask;
	}
	}
    

}
