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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Cards;
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scrpits.Powers
{
    public sealed class ExplosionMagicPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/EMP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/EMP64.png";
        public override bool IsInstanced => true;
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(30m,ValueProp.Unpowered)];
        public void SetDamage(int damage)
        {
            DynamicVars.Damage.BaseValue=damage;
        }
		public void AddDamage(int damage)
        {
            DynamicVars.Damage.BaseValue+=damage;
        }
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IgnitePower>()];
        public ExplosionMagicPower() { }
        public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
	{
		if (side != base.Owner.Side)
		{
			return;
		}
		if (base.Amount > 1)
		{
			await PowerCmd.Decrement(this);
			return;
		}
		Flash();
		await Cmd.CustomScaledWait(0.2f, 0.4f);
		foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
		{
			NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(hittableEnemy));
		}
		await Cmd.CustomScaledWait(0.2f, 0.4f);
        await PowerCmd.Apply<IgnitePower>(base.CombatState.HittableEnemies, base.DynamicVars.Damage.IntValue,null,null);
		await CreatureCmd.Damage(choiceContext, base.CombatState.HittableEnemies, base.DynamicVars.Damage, base.Owner);
		await PowerCmd.Remove(this);
	}
    }
}