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
using Patchoulib.Scrpits.Main;
using System.Collections.Generic;
using System.Threading.Tasks;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Potions;
[Pool(typeof(RinPotionPool))]
public sealed class PreventingRotPotion : CustomPotionModel
{
    public override PotionRarity Rarity => PotionRarity.Rare;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.AllEnemies;

    public override bool CanBeGeneratedInCombat => false;

    public override IEnumerable<IHoverTip> ExtraHoverTips => [Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Corpse"),Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")];
    public override string? CustomPackedImagePath => "res://TH_Rin/ArtWorks/Potions/PREVENTING_ROT_POTION.png";
    public override string? CustomPackedOutlinePath => "res://TH_Rin/ArtWorks/Potions/Outlines/PREVENTING_ROT_POTION.png"; 
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies)
        {
            if(mos.IsAlive)
            {
                await PowerCmd.Apply<PreventRotPower>(mos,1,null,null);
            }
        }
    }
}
