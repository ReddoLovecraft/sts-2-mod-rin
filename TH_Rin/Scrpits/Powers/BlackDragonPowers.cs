using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Linq;
using System.Threading.Tasks;
using TH_Rin.Scrpits.Monsters;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace TH_Rin.Scrpits.Powers
{
	public sealed class HardeningShellPower : CustomPowerModel
	{
		private const string _musicNodeName = "TH_RIN_BLACK_DRAGON_MUSIC";
		private const string _stage1MusicPath = "res://TH_Rin/ArtWorks/SFX/stage1.mp3";
		private const string _stage2MusicPath = "res://TH_Rin/ArtWorks/SFX/stage2.mp3";
		private const string _stage3MusicPath = "res://TH_Rin/ArtWorks/SFX/stage3.mp3";

		private class Data
		{
			public decimal DamageReceivedThisTurn;
			public int MusicStage = -1;
		}

		public override PowerType Type => PowerType.Buff;
		public override PowerStackType StackType => PowerStackType.Counter;
		public override bool ShouldScaleInMultiplayer => true;
		public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
		public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/HSP32.png";
		public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/HSP64.png";

		public override int DisplayAmount => (int)Math.Max(0m, (decimal)Amount - GetInternalData<Data>().DamageReceivedThisTurn);

		protected override object InitInternalData()
		{
			return new Data();
		}

		private static int GetMusicStage(Creature owner)
		{
			double hpRatio = (owner.MaxHp <= 0) ? 1.0 : (double)owner.CurrentHp / owner.MaxHp;
			if (hpRatio <= 0.34)
			{
				return 2;
			}
			if (hpRatio <= 0.67)
			{
				return 1;
			}
			return 0;
		}

		private static string GetMusicPathForStage(int stage)
		{
			return stage switch
			{
				0 => _stage1MusicPath,
				1 => _stage2MusicPath,
				2 => _stage3MusicPath,
				_ => _stage3MusicPath
			};
		}

		private void EnsureMusicIsAtLeastStage(int stage)
		{
			Data data = GetInternalData<Data>();
			if (stage <= data.MusicStage)
			{
				return;
			}
			if (TryPlayStageMusic(stage))
			{
				data.MusicStage = stage;
			}
		}

		private bool TryPlayStageMusic(int stage)
		{
			NCombatRoom? combatRoom = NCombatRoom.Instance;
			if (combatRoom == null)
			{
				return false;
			}

			AudioStreamPlayer? player = combatRoom.GetNodeOrNull<AudioStreamPlayer>(new NodePath(_musicNodeName));
			if (player == null)
			{
				player = new AudioStreamPlayer
				{
					Name = _musicNodeName,
					Bus = "Master"
				};
				combatRoom.AddChild(player);
			}

			string path = GetMusicPathForStage(stage);
			AudioStream stream;
			try
			{
				stream = PreloadManager.Cache.GetAsset<AudioStream>(path);
			}
			catch
			{
				return false;
			}
			if (stream is AudioStreamMP3 mp3)
			{
				mp3.Loop = true;
			}
			player.Stream = stream;
			player.Play();
			return true;
		}

		private void StopStageMusic()
		{
			NCombatRoom? combatRoom = NCombatRoom.Instance;
			if (combatRoom == null)
			{
				return;
			}

			AudioStreamPlayer? player = combatRoom.GetNodeOrNull<AudioStreamPlayer>(new NodePath(_musicNodeName));
			if (player == null)
			{
				return;
			}

			if (player.IsPlaying())
			{
				player.Stop();
			}
			player.QueueFree();
		}

		public override Task BeforeCombatStart()
		{
			EnsureMusicIsAtLeastStage(0);
			return Task.CompletedTask;
		}

		public override Task AfterApplied(Creature? applier, CardModel? cardSource)
		{
			EnsureMusicIsAtLeastStage(0);
			return Task.CompletedTask;
		}

		public override Task AfterCreatureAddedToCombat(Creature creature)
		{
			if (creature == Owner)
			{
				EnsureMusicIsAtLeastStage(0);
			}
			return Task.CompletedTask;
		}

		public override Task AfterCombatEnd(CombatRoom room)
		{
			StopStageMusic();
			return Task.CompletedTask;
		}

		public override decimal ModifyHpLostBeforeOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
		{
			if (target != Owner)
			{
				return amount;
			}
			if (amount == 0m)
			{
				return amount;
			}
			decimal remaining = (decimal)Amount - GetInternalData<Data>().DamageReceivedThisTurn;
			if (remaining <= 0m)
			{
				return 0m;
			}
			return Math.Min(amount, remaining);
		}

		public override Task AfterModifyingHpLostBeforeOsty()
		{
			Flash();
			return Task.CompletedTask;
		}

		public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
		{
			if (target != Owner)
			{
				return Task.CompletedTask;
			}
			if (result.WasFullyBlocked)
			{
				return Task.CompletedTask;
			}

			EnsureMusicIsAtLeastStage(GetMusicStage(Owner));

			GetInternalData<Data>().DamageReceivedThisTurn += (decimal)result.UnblockedDamage;
			InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
		{
			if (side != CombatSide.Player)
			{
				return Task.CompletedTask;
			}
			GetInternalData<Data>().DamageReceivedThisTurn = default(decimal);
			InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}
	}

	public sealed class LegendaryBlackDragonPower : CustomPowerModel
	{
		public override PowerType Type => PowerType.Buff;
		public override PowerStackType StackType => PowerStackType.Counter;
		public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
		public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/LBDP32.png";
		public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/LBDP64.png";

		private int GetStage()
		{
			double hpRatio = (Owner.MaxHp <= 0) ? 1.0 : (double)Owner.CurrentHp / Owner.MaxHp;
			if (hpRatio <= 1.0 / 3.0)
			{
				return 2;
			}
			if (hpRatio <= 2.0 / 3.0)
			{
				return 1;
			}
			return 0;
		}

		private decimal GetSingleHitThreshold()
		{
			int stage = GetStage();
			decimal basePct = 0.25m;
			decimal divisor = (decimal)Math.Pow(2.0, stage);
			return basePct / divisor;
		}

		public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
		{
			if (side != Owner.Side)
			{
				return;
			}
			await PowerCmd.Apply<StrengthPower>(Owner, 10m, Owner, null);
		}

		public override decimal ModifyHpLostBeforeOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
		{
			if (target != Owner)
			{
				return amount;
			}
			if (dealer != null)
			{
				return amount;
			}
			if (cardSource != null)
			{
				return amount;
			}
			if (!props.HasFlag(ValueProp.Unblockable) || !props.HasFlag(ValueProp.Unpowered))
			{
				return amount;
			}
			if (!Owner.HasPower<IgnitePower>())
			{
				return amount;
			}
			return amount * 0.25m;
		}

		public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
		{
			if (target != Owner)
			{
				return;
			}
			if (result.WasFullyBlocked)
			{
				return;
			}

			if (dealer == null || dealer == Owner || !dealer.IsPlayer)
			{
				return;
			}

			decimal threshold = (decimal)Owner.MaxHp * GetSingleHitThreshold();
			if ((decimal)result.UnblockedDamage < threshold)
			{
				return;
			}

			Flash();

			if (Owner.HasPower<StrengthPower>())
			{
				await PowerCmd.Remove(Owner.GetPower<StrengthPower>());
			}

			await CreatureCmd.Stun(Owner, nextMoveId: "FLY");

			if (Amount <= 1m)
			{
				await PowerCmd.Remove(this);
			}
			else
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
}
