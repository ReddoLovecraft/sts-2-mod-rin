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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Cards;
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scrpits.Powers
{
    public sealed class HellCarCrashPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/HCCP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/HCCP64.png";
        public HellCarCrashPower() { }
        public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
	{
		if (card.Owner.Creature == base.Owner && card.EnergyCost.GetWithModifiers(CostModifiers.Local)>0)
		{
			VfxCmd.PlayOnCreatureCenters(base.CombatState.HittableEnemies, "vfx/vfx_attack_blunt");
			SfxCmd.Play(RinInit.ToModSfxPath("TH_Rin/ArtWorks/SFX/attack.wav"));
			await CreatureCmd.Damage(choiceContext, base.CombatState.HittableEnemies, base.Amount*card.EnergyCost.GetWithModifiers(CostModifiers.Local), ValueProp.Unpowered, base.Owner, null);
		}
	}
    }

}