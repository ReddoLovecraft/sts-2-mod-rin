using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using TH_Rin.Scrpits.Cards;
using TH_Rin.Scrpits.Relics;


namespace TH_Rin.Scripts.Main
{
	public class RinCharacter : PlaceholderCharacterModel
	{
		public override Color NameColor => new Color("ca193eff");
		public override Color EnergyLabelOutlineColor => new Color("fa63baff");
		public override Color DialogueColor => new Color("ef0088ff");
		public override Color MapDrawingColor => new Color("ef0088ff");
		public override Color RemoteTargetingLineColor => new Color("fc578cff");
		public override Color RemoteTargetingLineOutline => new Color("da1c95ff");
		public override CharacterGender Gender => CharacterGender.Feminine;
		public override int StartingHp => 80;
		public override string CustomVisualPath => "res://TH_Rin/ArtWorks/Character/rin.tscn";
		public override string CustomTrailPath => "res://TH_Rin/ArtWorks/VFX/RinCardTrail.tscn";
		public override string CustomIconTexturePath => "res://TH_Rin/ArtWorks/Character/rin_icon.png";
		public override string CustomIconPath => "res://TH_Rin/ArtWorks/Character/rin_icon.tscn";
		public override string CustomEnergyCounterPath => "res://TH_Rin/ArtWorks/Character/rin_energy_counter.tscn";
		// 篝火休息动画。
		public override string CustomRestSiteAnimPath => "res://TH_Rin/ArtWorks/Character/rinrest.tscn";
		// 商店人物动画。
		public override string CustomMerchantAnimPath => "res://TH_Rin/ArtWorks/Character/rin_merchant.tscn";
		public override string CustomArmPointingTexturePath => "res://TH_Rin/ArtWorks/Character/multiplayer_hand_rin_point.png";
		public override string CustomArmRockTexturePath => "res://TH_Rin/ArtWorks/Character/multiplayer_hand_rin_rock.png";
		public override string CustomArmPaperTexturePath => "res://TH_Rin/ArtWorks/Character/multiplayer_hand_rin_paper.png";
		public override string CustomArmScissorsTexturePath => "res://TH_Rin/ArtWorks/Character/multiplayer_hand_rin_scissors.png";
		public override string CustomCharacterSelectBg => "res://TH_Rin/ArtWorks/Character/Rin_bg.tscn";
		public override string CustomCharacterSelectIconPath => "res://TH_Rin/ArtWorks/Character/char_select_rin.png";
		public override string CustomCharacterSelectLockedIconPath => "res://TH_Rin/ArtWorks/Character/char_select_rin_locked.png";
		public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/silent_transition_mat.tres";
		public override string CustomMapMarkerPath => "res://TH_Rin/ArtWorks/Character/map_marker_rin.png";
		// 攻击音效
		public override string CustomAttackSfx => RinInit.ToModSfxPath("TH_Rin/ArtWorks/SFX/attack.wav");
		// 施法音效
		public override string CustomCastSfx => RinInit.ToModSfxPath("TH_Rin/ArtWorks/SFX/cast.wav");
		// 死亡音效
		public override string CustomDeathSfx => RinInit.ToModSfxPath("TH_Rin/ArtWorks/SFX/die.ogg");
		public override string CharacterSelectSfx  => RinInit.ToModSfxPath("TH_Rin/ArtWorks/SFX/characterselect.wav");
		public override string CharacterTransitionSfx => RinInit.ToModSfxPath("TH_Rin/ArtWorks/SFX/transition.wav");
		public override CardPoolModel CardPool => ModelDb.CardPool<RinCardPool>();
		public override RelicPoolModel RelicPool => ModelDb.RelicPool<RinRelicPool>();
		public override PotionPoolModel PotionPool => ModelDb.PotionPool<RinPotionPool>();

		// 初始卡组
		public override IEnumerable<CardModel> StartingDeck => [
			 ModelDb.Card<Strike>(),
			 ModelDb.Card<Strike>(),
			 ModelDb.Card<Strike>(),
			 ModelDb.Card<Strike>(),
			 ModelDb.Card<Defend>(),
			 ModelDb.Card<Defend>(),
			 ModelDb.Card<Defend>(),
			 ModelDb.Card<Defend>(),
			 ModelDb.Card<Defend>(),
			 ModelDb.Card<GhostFire>()
		
	];

		// 初始遗物
		public override IReadOnlyList<RelicModel> StartingRelics => [
			ModelDb.Relic<BasicBarrow>()
	];

		// 攻击建筑师的攻击特效列表
		public override List<string> GetArchitectAttackVfx() => [
		"vfx/vfx_attack_blunt",
		"vfx/vfx_heavy_blunt",
		"vfx/vfx_attack_slash",
		"vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
		];
		public override CreatureAnimator GenerateAnimator(MegaSprite controller)
		{
			AnimState animState = new AnimState("Idle", isLooping: true);
			AnimState animState2 = new AnimState("Cast");
			AnimState animState3 = new AnimState("Attack");
			AnimState animState4 = new AnimState("Hit");
			AnimState state = new AnimState("Die");
			AnimState animState5 = new AnimState("relaxed_loop", isLooping: true);
			animState2.NextState = animState;
			animState3.NextState = animState;
			animState4.NextState = animState;
			animState5.AddBranch("Idle", animState);
			CreatureAnimator creatureAnimator = new CreatureAnimator(animState, controller);
			creatureAnimator.AddAnyState("Idle", animState);
			creatureAnimator.AddAnyState("Dead", state);
			creatureAnimator.AddAnyState("Hit", animState4);
			creatureAnimator.AddAnyState("Attack", animState3);
			creatureAnimator.AddAnyState("Cast", animState2);
			creatureAnimator.AddAnyState("relaxed_loop", animState5);		
			return creatureAnimator;
		}
		private int cnt=0;
		public int FireGeneratedCount()
		{
		return cnt;
		}
		public void ResetFireGeneratedCount()
		{
			cnt=0;
		}
		public void AddFireGeneratedCount(int amount)
		{
			cnt+=amount;
		}
	}
}
