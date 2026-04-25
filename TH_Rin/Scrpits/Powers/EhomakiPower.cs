using MegaCrit.Sts2.Core.Models;
using Patchouib.Scripts.Main;
using TH_Rin.Scrpits.Cards;

namespace TH_Rin.Scrpits.Powers
{
public sealed class EhomakiPower :CustomTempStrengthPower
{
	public override AbstractModel OriginModel => ModelDb.Card<SilverVineEhomaki>();
    protected override bool IsPositive => false;
    public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/EP232.png";
    public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/EP264.png";
      
}

}