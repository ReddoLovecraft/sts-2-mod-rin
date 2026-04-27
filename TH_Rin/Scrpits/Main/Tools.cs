using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.HoverTips;
using System.Reflection;
using TH_Rin.Scrpits.Cards;
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scripts.Main
{
    public static class Tools
    {
        public static HashSet<(Type,Type)> _monsterToCorpseCard = new HashSet<(Type, Type)>();

        public static bool HasPowerHoverTip<TPower>(IEnumerable<IHoverTip>? hoverTips) where TPower : PowerModel
        {
            if (hoverTips == null)
            {
                return false;
            }

            IHoverTip wanted = HoverTipFactory.FromPower<TPower>();
            foreach (IHoverTip tip in hoverTips)
            {
                if (tip != null && Equals(tip, wanted))
                {
                    return true;
                }
            }

            string wantedKey = GetHoverTipKey(wanted);
            foreach (IHoverTip tip in hoverTips)
            {
                if (tip != null && GetHoverTipKey(tip) == wantedKey)
                {
                    return true;
                }
            }

            return false;
        }

        private static string GetHoverTipKey(IHoverTip tip)
        {
            Type tipType = tip.GetType();

            string? id =
                TryGetStringMember(tip, tipType, "PowerId") ??
                TryGetStringMember(tip, tipType, "PowerID") ??
                TryGetStringMember(tip, tipType, "Id") ??
                TryGetStringMember(tip, tipType, "KeywordId") ??
                TryGetStringMember(tip, tipType, "Key");

            if (!string.IsNullOrEmpty(id))
            {
                return $"{tipType.FullName}|id:{id}";
            }

            Type? modelType =
                TryGetTypeMember(tip, tipType, "PowerType") ??
                TryGetTypeMember(tip, tipType, "PowerModelType") ??
                TryGetTypeMember(tip, tipType, "ModelType");

            if (modelType != null)
            {
                return $"{tipType.FullName}|type:{modelType.FullName}";
            }

            object? innerPower =
                TryGetObjectMember(tip, tipType, "Power") ??
                TryGetObjectMember(tip, tipType, "PowerModel") ??
                TryGetObjectMember(tip, tipType, "Model");

            if (innerPower != null)
            {
                object? innerId = TryGetObjectMember(innerPower, innerPower.GetType(), "Id");
                if (innerId != null)
                {
                    return $"{tipType.FullName}|innerId:{innerId}";
                }
            }

            return $"{tipType.FullName}|str:{tip}";
        }

        private static string? TryGetStringMember(object instance, Type instanceType, string memberName)
        {
            PropertyInfo? prop = instanceType.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null && prop.PropertyType == typeof(string) && prop.GetIndexParameters().Length == 0)
            {
                return (string?)prop.GetValue(instance);
            }

            FieldInfo? field = instanceType.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null && field.FieldType == typeof(string))
            {
                return (string?)field.GetValue(instance);
            }

            return null;
        }

        private static Type? TryGetTypeMember(object instance, Type instanceType, string memberName)
        {
            PropertyInfo? prop = instanceType.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null && prop.PropertyType == typeof(Type) && prop.GetIndexParameters().Length == 0)
            {
                return (Type?)prop.GetValue(instance);
            }

            FieldInfo? field = instanceType.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null && field.FieldType == typeof(Type))
            {
                return (Type?)field.GetValue(instance);
            }

            return null;
        }

        private static object? TryGetObjectMember(object instance, Type instanceType, string memberName)
        {
            PropertyInfo? prop = instanceType.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null && prop.GetIndexParameters().Length == 0)
            {
                return prop.GetValue(instance);
            }

            FieldInfo? field = instanceType.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                return field.GetValue(instance);
            }

            return null;
        }
          public static int GetDebuffTotalCount(Creature target) 
        {
            int result = 0;
            foreach(PowerModel debuff in target.Powers) 
            {
                if(debuff.Type==PowerType.Debuff) 
                {
                    if (debuff.Amount > 0)
                        result += debuff.Amount;
                    else
                        result++;
                }
            }
            return result;

        }
        public static int GetDebuffKind(Creature target)
        {
            int result = 0;
            foreach (PowerModel debuff in target.Powers)
            {
                if (debuff.Type == PowerType.Debuff)
                {
                        result++;
                }
            }
            return result;
        }
        public static T CreateCorpseCard<T>(Player owner, int integrity, int rotCount, bool rotable = true) where T : CorpseCardModel
        {
            T corpseCard = (T)ModelDb.Card<T>().ToMutable();
            corpseCard.Owner = owner;
            bool shouldRot = rotable && !corpseCard.Tags.Contains(CardModfier.NorotTag) && !corpseCard.Keywords.Contains(CardModfier.NorotKeyword);
            corpseCard.Init(integrity, rotCount, shouldRot);
            return corpseCard;
        }
     public static CorpseCardModel GetCorpseCard(MonsterModel monster, Player owner, int integrity, int rotCount, bool rotable = true)
        {
        Type monsterType = monster.GetType();
        var pair = Tools._monsterToCorpseCard.FirstOrDefault(p => p.Item1 == monsterType);
        if (pair == default)
        {
            if(monster is EyeWithTeeth||monster is Parafright)
            {
                string message = "It's just an illusion, there's no corpse.";
                if (MegaCrit.Sts2.Core.Localization.LocManager.Instance.Language == "zhs")
                {
                    message = "这只是个虚影，没有尸体。";
                }
                Patchoulib.Scrpits.Main.Tools.Talk(message, owner.Creature);
                return null;
            }
             if(monster is GasBomb)
            {
                string message = "It's just a cloud of gas, there's no corpse.";
                if (MegaCrit.Sts2.Core.Localization.LocManager.Instance.Language == "zhs")
                {
                    message = "这只是一团气体，没有尸体。";
                }
                Patchoulib.Scrpits.Main.Tools.Talk(message, owner.Creature);
                return null;
            }
               if(monster is GremlinMerc)
            {
                string message = "It's actually a creation made up of two goblins, and they're not dead yet!";
                if (MegaCrit.Sts2.Core.Localization.LocManager.Instance.Language == "zhs")
                {
                    message = "居然是两只地精组合起来的造物，它们还没死！";
                }
                Patchoulib.Scrpits.Main.Tools.Talk(message, owner.Creature);
                return null;
            }
            if(monster is Doormaker)
            {
                string message = "It's just a door.";
                if (MegaCrit.Sts2.Core.Localization.LocManager.Instance.Language == "zhs")
                {
                    message = "只是一扇门而已！";
                }
                Patchoulib.Scrpits.Main.Tools.Talk(message, owner.Creature);
                return null;
            }
            if(monster is BattleFriendV1||monster is BattleFriendV2||monster is BattleFriendV3)
            {
                string message = "It's just a target. There's no need to take it away.";
                if (MegaCrit.Sts2.Core.Localization.LocManager.Instance.Language == "zhs")
                {
                    message = "一个标靶而已，没必要拿走了。";
                }
                Patchoulib.Scrpits.Main.Tools.Talk(message, owner.Creature);
                return null;
            }
           
            //走没找到怪物类型的逻辑
            return CreateCorpseCard<TestCorpse>(owner, integrity, rotCount, rotable);
        }
        Type corpseType = pair.Item2;
        var method = typeof(Tools).GetMethod(nameof(Tools.CreateCorpseCard));
        var genericMethod = method.MakeGenericMethod(corpseType);
        return (CorpseCardModel)genericMethod.Invoke(null, new object[] { owner, integrity, rotCount, rotable });
    }
        public static IBarrowLike? GetBarrowRelic(Player owner)
        {
            Barrow? barrow = owner.GetRelic<Barrow>();
            if (barrow != null)
            {
                return barrow;
            }
            return owner.GetRelic<BasicBarrow>();
        }

        public static void RemoveFromBarrow(IBarrowLike barrow, CorpseCardModel card)
        {
            card.TriggerWhenRemove();
            barrow.CorpseCards.Remove(card);

            NCard nCard = NCard.Create(card);
            if (nCard != null && NRun.Instance?.GlobalUi != null)
            {
                NRun.Instance.GlobalUi.CardPreviewContainer.AddChildSafely(nCard);
                nCard.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
                Tween tween = nCard.CreateTween();
                tween.TweenProperty(nCard, "scale", Vector2.One * 1f, 0.25).From(Vector2.Zero).SetEase(Tween.EaseType.Out)
                    .SetTrans(Tween.TransitionType.Cubic);
                tween.TweenProperty(nCard, "scale:y", 0, 0.30000001192092896).SetDelay(1.5);
                tween.Parallel().TweenProperty(nCard, "scale:x", 1.5f, 0.3).SetDelay(1.5);
                tween.Parallel().TweenProperty(nCard, "modulate", Colors.Black, 0.2).SetDelay(1.5);
                tween.TweenCallback(Callable.From(nCard.QueueFreeSafely));
            }

            barrow.updateDesc();
        }
        
        public static void RemoveFromBarrow(CorpseCardModel card)
        {
            Player? owner = null;
            try
            {
                owner = card.Owner;
            }
            catch
            {
            }

            if (owner == null)
            {
                return;
            }

            IBarrowLike? barrow = GetBarrowRelic(owner);
            if (barrow == null)
            {
                return;
            }
            RemoveFromBarrow(barrow, card);
        }

        public static void AddCorpseToBarrow(Player owner, CorpseCardModel corpseCard)
        {
            if(corpseCard==null)
            {
                return;
            }
            IBarrowLike? barrow = GetBarrowRelic(owner);
            if (barrow == null)
            {
                return;
            }

            CorpseCardModel toAdd = corpseCard;
            if (!toAdd.IsMutable)
            {
                toAdd = (CorpseCardModel)((CardModel)toAdd).ToMutable();
            }

            try
            {
                if (toAdd.Owner == null)
                {
                    toAdd.Owner = owner;
                }
            }
            catch
            {
                toAdd.Owner = owner;
            }
            toAdd.Init(corpseCard.Integrity, corpseCard.RotCount, corpseCard.Rotable);
            PreviewInternal(toAdd, false);
            barrow.CorpseCards.Add(toAdd);
            barrow.updateDesc();
        }
        private static Task FlashRelics(NCard node, IEnumerable<RelicModel>? relicsToFlash)
	{
		if (relicsToFlash == null)
		{
			return Task.CompletedTask;
		}
		foreach (RelicModel item in relicsToFlash)
		{
			item.Flash();
			node.FlashRelicOnCard(item);
		}
		return Task.CompletedTask;
	}
        public static TaskCompletionSource? PreviewInternal(CardModel card, bool isAddingCardsToPile, IEnumerable<RelicModel>? relicsToFlash = null, float time = 1.2f, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
	{
		if (TestMode.IsOn)
		{
			return null;
		}
		if (CombatManager.Instance.IsEnding)
		{
			return null;
		}
		if (!LocalContext.IsMine(card))
		{
			return null;
		}
		PileType pileType = card.Pile?.Type ?? PileType.Deck;
		NCard node = NCard.Create(card);
		Control control;
		switch (style)
		{
		case CardPreviewStyle.HorizontalLayout:
			control = (pileType.IsCombatPile() ? NCombatRoom.Instance?.Ui.CardPreviewContainer : NRun.Instance?.GlobalUi.CardPreviewContainer);
			break;
		case CardPreviewStyle.MessyLayout:
			control = (pileType.IsCombatPile() ? NCombatRoom.Instance?.Ui.MessyCardPreviewContainer : NRun.Instance?.GlobalUi.MessyCardPreviewContainer);
			break;
		case CardPreviewStyle.EventLayout:
			if (pileType.IsCombatPile())
			{
				throw new InvalidOperationException();
			}
			control = NRun.Instance?.GlobalUi.EventCardPreviewContainer;
			break;
		case CardPreviewStyle.GridLayout:
			if (pileType.IsCombatPile())
			{
				throw new InvalidOperationException();
			}
			control = NRun.Instance?.GlobalUi.GridCardPreviewContainer;
			break;
		default:
			throw new ArgumentOutOfRangeException("style", $"Unexpected {"CardPreviewStyle"} {style}!");
		}
		control?.AddChildSafely(node);
		node.UpdateVisuals(pileType, CardPreviewMode.Normal);
		TaskCompletionSource source = new TaskCompletionSource();
		Tween tween = node.CreateTween();
		tween.TweenProperty(node, "scale", Vector2.One, 0.25).From(Vector2.Zero).SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Cubic);
		tween.TweenCallback(Callable.From(delegate
		{
			TaskHelper.RunSafely(FlashRelics(node, relicsToFlash));
		}));
		tween.TweenCallback(Callable.From(delegate
		{
			NCardFlyVfx nCardFlyVfx = null;
			Node node2 = ((pileType != PileType.Deck) ? NCombatRoom.Instance?.CombatVfxContainer : NRun.Instance?.GlobalUi.TopBar.TrailContainer);
			if (node2 != null)
			{
				PileType pileType2 = ((card.Pile != null) ? card.Pile.Type : pileType);
				Vector2 targetPosition = pileType2.GetTargetPosition(node);
				nCardFlyVfx = NCardFlyVfx.Create(node, targetPosition, isAddingCardsToPile, card.Owner.Character.TrailPath);
			}
			if (nCardFlyVfx != null && node2 != null)
			{
				node2.AddChildSafely(nCardFlyVfx);
				nCardFlyVfx.SwooshAwayCompletion.Task.ContinueWith(delegate
				{
					source.SetResult();
				});
			}
			else
			{
				node.QueueFreeSafely();
				source.SetResult();
			}
		})).SetDelay(time);
		return source;
	}

    }
}
