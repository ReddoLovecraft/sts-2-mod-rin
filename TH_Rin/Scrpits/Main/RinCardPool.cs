using BaseLib.Abstracts;
using Godot;

namespace TH_Rin.Scripts.Main
{
	public class RinCardPool : CustomCardPoolModel
{
	public override string Title => "TH_Rin";

 	public override Color ShaderColor => new Color("ea00b0ff");
	public override Color DeckEntryCardColor => new Color("ea00b0ff");
  	public override string? BigEnergyIconPath => "res://TH_Rin/ArtWorks/Character/card_orb.png";
	public override string? TextEnergyIconPath => "res://TH_Rin/ArtWorks/Character/cost_orb.png";
	public override bool IsColorless => false;
}
}
