using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Potions;
[Pool(typeof(RinPotionPool))]
public sealed class CorpseBar : CustomPotionModel
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.Self;

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];
    public override string? CustomPackedImagePath => "res://TH_Rin/ArtWorks/Potions/CORPSE_BAR.png";
    public override string? CustomPackedOutlinePath => "res://TH_Rin/ArtWorks/Potions/Outlines/CORPSE_BAR.png"; 
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
       await PowerCmd.Apply<VigorPower>(Owner.Creature,5,Owner.Creature,null);
       (await PowerCmd.Apply<VigorNextTurnPower>(Owner.Creature,3,Owner.Creature,null)).SetCardVar(5);
    }
}
