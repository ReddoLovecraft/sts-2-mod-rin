using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using TH_Rin.Scrpits.Monsters;

namespace TH_Rin.Scrpits.Encounters;

public sealed class BlackDragonEventEncounter : CustomEncounterModel
{
	public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<BlackDargon>()];

	public override bool IsValidForAct(ActModel act) => false;

	public override bool IsWeak => false;

	public BlackDragonEventEncounter() : base(RoomType.Monster)
	{
	}

	protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() =>
	[
		(ModelDb.Monster<BlackDargon>().ToMutable(), null)
	];
}
