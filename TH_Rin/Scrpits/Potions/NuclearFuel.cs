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

namespace TH_Rin.Scrpits.Potions;
[Pool(typeof(RinPotionPool))]
public sealed class NuclearFuel : CustomPotionModel
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.Self;

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this),HoverTipFactory.FromPower<DemisePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[1]
    {
        new EnergyVar(6)
    });
    public override string? CustomPackedImagePath => "res://TH_Rin/ArtWorks/Potions/NUCLEAR_FUEL.png";
    public override string? CustomPackedOutlinePath => "res://TH_Rin/ArtWorks/Potions/Outlines/NUCLEAR_FUEL.png"; 
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
       await PlayerCmd.GainEnergy(6,Owner);
       await PowerCmd.Apply<DemisePower>(Owner.Creature,6,Owner.Creature,null);
    }
}
