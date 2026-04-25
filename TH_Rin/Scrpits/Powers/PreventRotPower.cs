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
using TH_Rin.Scrpits.Cards;
using TH_Rin.Scrpits.Relics;
using Patchoulib.Scrpits.Main;

namespace TH_Rin.Scrpits.Powers
{
    public sealed class PreventRotPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/PRP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/PRP64.png";
         protected override IEnumerable<IHoverTip> ExtraHoverTips => [Tools.GetStaticKeyword("Rot"),Tools.GetStaticKeyword("Corpse")];
        public PreventRotPower() { }
	}

}