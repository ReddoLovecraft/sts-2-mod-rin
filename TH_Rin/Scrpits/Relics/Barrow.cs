using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Helpers;
using Patchouib.Scrpits.Main;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes;
using TH_Rin.Scrpits.UI;
using TH_Rin.Scrpits.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TH_Rin.Scrpits.Relics
{
[Pool(typeof(RinRelicPool))]
public class Barrow : CustomRelicModel,IRightCilckable
{
	    protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[5]
        {
		  HoverTipFactory.FromPower<WraithPower>(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Corpse"),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Integrity"),
		  HoverTipFactory.FromPower<IgnitePower>(),
		  Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	private List<CorpseCardModel> _corpseCards = new List<CorpseCardModel>();

	protected override void DeepCloneFields()
	{
		base.DeepCloneFields();
		_corpseCards = new List<CorpseCardModel>();
	}
    [SavedProperty]
	public List<SerializableCard> SerializableCorpseCards
	{
		get
		{
			return _corpseCards.Select(c => c.ToSerializable()).ToList();
		}
		set
		{
			AssertMutable();
			_corpseCards.Clear();
			foreach (SerializableCard serializableCard in value)
			{
				if (CardModel.FromSerializable(serializableCard) is CorpseCardModel corpseCard)
				{
					_corpseCards.Add(corpseCard);
				}
			}
			InvokeDisplayAmountChanged();
		}
	}
	  public override async Task AfterAttack(AttackCommand command)
	  {
		foreach(var item in CorpseCards.ToList())
		{
			await item.AfterAttack(command);
		}
	  }
	public List<CorpseCardModel> CorpseCards => _corpseCards;
	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if(player==Owner.Creature.Player)
		{
			foreach(Creature creature in Owner.Creature.CombatState.HittableEnemies.ToList())
			{
				if(creature.Monster is TestSubject&&!creature.HasPower<IntegrityPower>()&&creature.HasPower<NemesisPower>())
				await PowerCmd.Apply<IntegrityPower>(creature,100,null,null);
			}
		}
		foreach(var item in CorpseCards.ToList())
		{
			await item.TriggerWhenTurnStart(choiceContext, player);
		}
	}
	public override async Task AfterCreatureAddedToCombat(Creature creature)
	{
		if (creature.Side != base.Owner.Creature.Side)
		{
			PowerModel? existingPower = creature.GetPower<IntegrityPower>();
			if (existingPower == null)
			{
				Flash();
				await PowerCmd.Apply<IntegrityPower>(creature, 100, null, null);
			}
		}
	}
	public override async Task AfterCardDrawnEarly(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
	{
		foreach(var item in CorpseCards.ToList())
		{
			await item.AfterCardDrawnEarly(choiceContext, card, fromHandDraw);
		}
	}
		public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
	{
		foreach(var item in CorpseCards.ToList())
		{
			await item.AfterCardPlayed(context, cardPlay);
		}
	}
	 public override async Task BeforeCombatStart()
    {
		EnsureCorpseCardOwners(Owner);
		if(Owner.Creature.Player.Character is RinCharacter rc)
        {
                rc.ResetFireGeneratedCount();
        }
		await PowerCmd.Apply<WraithPower>(Owner.Creature,this.DisplayAmount,null,null);
		foreach(Creature creature in Owner.Creature.CombatState.HittableEnemies.ToList())
		{
			if(creature.Monster is not TestSubject)
			await PowerCmd.Apply<IntegrityPower>(creature,100,null,null);
		}
		foreach(var item in CorpseCards.ToList())
		{
			await item.TriggerWhenCombatStart();
		}
	}
		public override async Task AfterModifyingPowerAmountReceived(PowerModel power)
	{
		if(power is WraithPower && power.Owner==this.Owner.Creature&&power.Owner.Player.Character is RinCharacter rc)
		{
			rc.AddFireGeneratedCount(power.Amount);
		}
	}
	public override async Task AfterCombatEnd(CombatRoom room)
	{
		foreach(var item in CorpseCards.ToList())
		{
			await item.AfterCombatEnd(room);
		}
	}
	 public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
		foreach(var item in CorpseCards.ToList())
		{
			await item.TriggerWhenTurnEnd(choiceContext, side);
		}
	}
	 public async Task TriggerAllCorpsesEffect()
    {
		foreach(var item in CorpseCards.ToList())
		{
			await item.TriggerEffect();
		}
	}
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature target, bool wasRemovalPrevented, float deathAnimLength)
		{
			foreach(var item in CorpseCards.ToList())
			{
				await item.AfterDeath(choiceContext, target, wasRemovalPrevented, deathAnimLength);
			}
		}
      public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
	  {
		foreach(var item in CorpseCards.ToList())
		{
			await item.AfterDamageReceived(choiceContext, target, result, props, dealer, cardSource);
		}
	  }

	 public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	 {
		foreach(var item in CorpseCards.ToList())
		{
			await item.BeforeDamageReceived(choiceContext, target, amount, props, dealer, cardSource);
		}
	 }
	 public override async Task AfterRoomEntered(AbstractRoom room)
	{
		for (int i = CorpseCards.Count - 1; i >= 0; i--)
		{
			CorpseCardModel corpseCard = CorpseCards[i];
			if (!corpseCard.Rotable)
			{
				continue;
			}
			corpseCard.RotCount--;
			if (corpseCard.RotCount <= 0)
			{
				Tools.RemoveFromBarrow(this, corpseCard);
			}
		}
		await Task.CompletedTask;
	}
	public void updateDesc()
	{
		this.Flash();
		this.InvokeDisplayAmountChanged();
	}

        public async Task OnRightClick(PlayerChoiceContext context)
        {
			OpenCorpseScreen();
			await Task.CompletedTask;
        }

		public void OpenCorpseScreen()
		{
			Callable.From(delegate
			{
				TaskHelper.RunSafely(OpenCorpseScreenFlow());
			}).CallDeferred();
		}

		private async Task OpenCorpseScreenFlow()
		{
			EnsureCorpseCardOwners(Owner);
			if (CorpseCards.Count == 0)
			{
				ShowInfoPopup();
				return;
			}

			CardSelectorPrefs prefs = new CardSelectorPrefs(new LocString("relics", $"{Id.Entry}.desc"), 0, 1)
			{
				RequireManualConfirmation = true,
				Cancelable = true
			};

			NSimpleCardSelectScreen screen = NSimpleCardSelectScreen.Create(CorpseCards.Cast<CardModel>().ToList(), prefs);
			BarrowCorpseSelectContext.Attach(screen, this);
			NOverlayStack.Instance.Push(screen);
			await Task.CompletedTask;
		}

		private void EnsureCorpseCardOwners(Player player)
		{
			if (player == null)
			{
				return;
			}
			foreach (CorpseCardModel corpseCard in CorpseCards.ToList())
			{
				try
				{
					if (corpseCard.Owner == null)
					{
						corpseCard.Owner = player;
					}
				}
				catch
				{
					corpseCard.Owner = player;
				}
			}
		}

		private void ShowInfoPopup()
		{
			if (NModalContainer.Instance == null)
			{
				return;
			}

			NGenericPopup nGenericPopup = NGenericPopup.Create();
			NModalContainer.Instance.Add(nGenericPopup);
			TaskHelper.RunSafely(nGenericPopup.WaitForConfirmation(new LocString("relics", $"{Id.Entry}.desc"), new LocString("relics", $"{Id.Entry}.title"), null, new LocString("relics", $"{Id.Entry}.confirm")));
		}
    public override bool ShowCounter => true;
    public override int DisplayAmount
	{
		get
		{
			return _corpseCards.Count;
		}
	}
    public override RelicRarity Rarity => RelicRarity.Starter;
	public override string PackedIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
    protected override string PackedIconOutlinePath => $"res://TH_Rin/ArtWorks/Relics/Outlines/{Id.Entry}.png";
    protected override string BigIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
 
}
}
