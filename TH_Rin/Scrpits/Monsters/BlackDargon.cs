using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TH_Rin.Scrpits.Powers;
using TH_Rin.Scripts.Main;
using static BaseLib.Utils.BetaMainCompatibility;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Runs;


namespace TH_Rin.Scrpits.Monsters;
public sealed class BlackDargon : CustomMonsterModel
{
	private const string FlyMoveId = "FLY";
	private const string DownStrikeMoveId = "DOWN_STRIKE";
	private const string FireBreathMoveId = "FIRE_BREATH";
	private const string ThreeBallMoveId = "THREE_BALL";
	private const string FanFirewallMoveId = "FAN_FIREWALL";
	private const string TailSweepMoveId = "TAIL_SWEEP";

	private string? _forcedMoveId;

	private int InitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 700, 660);
	private int InitialPlating => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 24, 20);
	private int InitialHardeningShell => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 200, 250);

	private int DownStrikeDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 40, 35);
	private int DownStrikeStrengthLoss => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 8, 7);
	private int FireBreathIgnite => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 15, 10);

	private int ThreeBallDamage => 20;
	private int FanFirewallDamage => 20;
	private int FanFirewallBaseIgniteMultiplier => 1;
	private int TailSweepBlock => 30;
	private int FlyBlock => 80;

	public override int MinInitialHp => InitialHp;
	public override int MaxInitialHp => InitialHp;
	public override LocString Title => MonsterModel.L10NMonsterLookup(GetType().Name + ".name");
	protected override string VisualsPath => "res://TH_Rin/ArtWorks/Monsters/BLACK_DRAGON.tscn";
	public override IEnumerable<string> AssetPaths => base.AssetPaths.Concat(new[]
	{
		"res://TH_Rin/ArtWorks/SFX/stage1.mp3",
		"res://TH_Rin/ArtWorks/SFX/stage2.mp3",
		"res://TH_Rin/ArtWorks/SFX/stage3.mp3"
	});

	public override NCreatureVisuals? CreateCustomVisuals() =>
		NodeFactory<NCreatureVisuals>.CreateFromScene("res://TH_Rin/ArtWorks/Monsters/BLACK_DRAGON.tscn");

	public override async Task BeforeCombatStart()
	{
		await PowerCmd.Apply<PlatingPower>(Creature, InitialPlating, Creature, null);
		await PowerCmd.Apply<HardeningShellPower>(Creature, InitialHardeningShell, Creature, null);
		await PowerCmd.Apply<LegendaryBlackDragonPower>(Creature, 3m, Creature, null);
	}

	internal void ForceNextMove(string moveId)
	{
		AssertMutable();
		_forcedMoveId = moveId;
	}

	private int GetFireMultiplier()
	{
		double hpRatio = (Creature.MaxHp <= 0) ? 1.0 : (double)Creature.CurrentHp / Creature.MaxHp;
		if (hpRatio <= 1.0 / 3.0)
		{
			return 3;
		}
		if (hpRatio <= 2.0 / 3.0)
		{
			return 2;
		}
		return 1;
	}

	private Creature? GetRandomLivingPlayer(IReadOnlyList<Creature> targets)
	{
		List<Creature> living = targets.Where(t => t != null && t.IsAlive && t.IsPlayer).ToList();
		if (living.Count == 0)
		{
			return null;
		}
		return Creature.CombatState.RunState.Rng.CombatTargets.NextItem(living);
	}

	protected override MonsterMoveStateMachine GenerateMoveStateMachine()
	{
		List<MonsterState> states = new List<MonsterState>();

		MoveState fly = new MoveState(FlyMoveId, FlyMove, new BuffIntent(), new DefendIntent());
		MoveState downStrike = new MoveState(DownStrikeMoveId, DownStrikeMove, new SingleAttackIntent(() => DownStrikeDamage), new DebuffIntent());
		MoveState fireBreath = new MoveState(FireBreathMoveId, FireBreathMove, new DebuffIntent(), new StatusIntent(10));
		MoveState threeBall = new MoveState(ThreeBallMoveId, ThreeBallMove, new MultiAttackIntent(ThreeBallDamage, 3));
		MoveState fanFirewall = new MoveState(FanFirewallMoveId, FanFirewallMove, new SingleAttackIntent(FanFirewallDamage), new DebuffIntent());
		MoveState tailSweep = new MoveState(TailSweepMoveId, TailSweepMove, new DebuffIntent(), new DefendIntent());

		RandomBranchState randomFire = new RandomBranchState("RAND_FIRE");
		randomFire.AddBranch(threeBall, MoveRepeatType.CannotRepeat, 0.5f);
		randomFire.AddBranch(fireBreath, MoveRepeatType.CannotRepeat, 0.3f);
		randomFire.AddBranch(fanFirewall, MoveRepeatType.CannotRepeat, 0.2f);

		RandomBranchState randomMain = new RandomBranchState("RAND_MAIN");
		randomMain.AddBranch(randomFire, MoveRepeatType.CanRepeatForever, 0.7f);
		randomMain.AddBranch(tailSweep, MoveRepeatType.CannotRepeat, 0.3f);

		ConditionalBranchState forced = new ConditionalBranchState("FORCED");
		forced.AddState(fly, () => _forcedMoveId == FlyMoveId && GetLastMoveId() != FlyMoveId);
		forced.AddState(downStrike, () => _forcedMoveId == DownStrikeMoveId && GetLastMoveId() != DownStrikeMoveId);
		forced.AddState(threeBall, () => _forcedMoveId == ThreeBallMoveId && GetLastMoveId() != ThreeBallMoveId);
		forced.AddState(randomMain, () => true);

		fly.FollowUpState = forced;
		downStrike.FollowUpState = forced;
		fireBreath.FollowUpState = forced;
		threeBall.FollowUpState = forced;
		fanFirewall.FollowUpState = forced;
		tailSweep.FollowUpState = forced;

		states.Add(fly);
		states.Add(downStrike);
		states.Add(fireBreath);
		states.Add(threeBall);
		states.Add(fanFirewall);
		states.Add(tailSweep);
		states.Add(randomFire);
		states.Add(randomMain);
		states.Add(forced);

		return new MonsterMoveStateMachine(states, forced);
	}

	private string? GetLastMoveId()
	{
		return MoveStateMachine?.StateLog.LastOrDefault()?.Id;
	}

	private async Task FlyMove(IReadOnlyList<Creature> targets)
	{
		_forcedMoveId = DownStrikeMoveId;
		await PowerCmd.Apply<SoarPower>(Creature, 1m, Creature, null);
		await CreatureCmd.GainBlock(Creature, FlyBlock, ValueProp.Unpowered | ValueProp.Move, null);
	}

	private async Task DownStrikeMove(IReadOnlyList<Creature> targets)
	{
		_forcedMoveId = null;
		if (Creature.HasPower<SoarPower>())
		{
			await PowerCmd.Remove(Creature.GetPower<SoarPower>());
		}

		await DamageCmd.Attack(DownStrikeDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f)
			.WithAttackerFx(null, AttackSfx)
			.Execute(null);

		foreach (Creature target in targets.Where(t => t != null && t.IsAlive && t.IsPlayer))
		{
			await PowerCmd.Apply<StrengthPower>(target, -DownStrikeStrengthLoss, Creature, null);
		}
	}

	private async Task FireBreathMove(IReadOnlyList<Creature> targets)
	{
		_forcedMoveId = null;

		int mult = GetFireMultiplier();
		int burnCount = 5 * mult;
		int ignite = FireBreathIgnite * mult;

		IReadOnlyList<Creature> living = targets.Where(t => t != null && t.IsAlive && t.IsPlayer).ToList();
		if (living.Count == 0)
		{
			return;
		}

		await CardPileCmd.AddToCombatAndPreview<Burn>(living, PileType.Draw, burnCount, addedByPlayer: false);
		await CardPileCmd.AddToCombatAndPreview<Burn>(living, PileType.Discard, burnCount, addedByPlayer: false);

		foreach (Creature target in living)
		{
			await PowerCmd.Apply<IgnitePower>(target, ignite, Creature, null);
		}
	}

	private async Task ThreeBallMove(IReadOnlyList<Creature> targets)
	{
		_forcedMoveId = null;

		int mult = GetFireMultiplier();
		int damagePerHit = ThreeBallDamage * mult;

		await DamageCmd.Attack(damagePerHit).FromMonster(this).WithAttackerAnim("Attack", 0.15f)
			.WithAttackerFx(null, AttackSfx)
			.WithHitCount(3)
			.Execute(null);
	}

	private async Task TailSweepMove(IReadOnlyList<Creature> targets)
	{
		_forcedMoveId = ThreeBallMoveId;

		await CreatureCmd.GainBlock(Creature, TailSweepBlock, ValueProp.Unpowered | ValueProp.Move, null);

		foreach (Creature target in targets.Where(t => t != null && t.IsAlive && t.IsPlayer))
		{
			await PowerCmd.Apply<VulnerablePower>(target, 3m, Creature, null);
			await PowerCmd.Apply<WeakPower>(target, 3m, Creature, null);
			await PowerCmd.Apply<FrailPower>(target, 3m, Creature, null);
		}
	}

	private async Task FanFirewallMove(IReadOnlyList<Creature> targets)
	{
		_forcedMoveId = null;

		int mult = GetFireMultiplier();
		int baseDamage = FanFirewallDamage * mult;

		int burnInDrawTotal = 0;
		foreach (Creature creature in targets.Where(t => t != null && t.IsAlive && t.IsPlayer))
		{
			int count = creature.Player?.PlayerCombatState?.DrawPile.Cards.Count(c => c is Burn) ?? 0;
			burnInDrawTotal += count;
		}
		int damage = (burnInDrawTotal > 0) ? (int)(baseDamage * Math.Pow(2.0, burnInDrawTotal)) : baseDamage;

		var command = await DamageCmd.Attack(damage).FromMonster(this).WithAttackerAnim("Attack", 0.15f)
			.WithAttackerFx(null, AttackSfx)
			.Execute(null);

		foreach (Creature receiver in command.Results.Select(r => r.Receiver).Distinct())
		{
			int unblocked = command.Results.Where(r => r.Receiver == receiver).Sum(r => r.UnblockedDamage);
			if (unblocked > 0)
			{
				await PowerCmd.Apply<IgnitePower>(receiver, unblocked * FanFirewallBaseIgniteMultiplier, Creature, null);
			}
		}
	}
}
