using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Saves.Runs;
using TH_Rin.Scrpits.Cards;
using TH_Rin.Scrpits.Relics;


namespace TH_Rin.Scripts.Main
{
    public abstract class CorpseCardModel : RinCardModel
    {
        private int _rotcount=10;
        private int _integrity=100;
        private CorpseType _corpseType=CorpseType.Full;
        private bool _rotable=true;
        protected virtual void RefreshCorpseVars()
        {
        }
        [SavedProperty]
        public bool Rotable 
        {
            get{return _rotable;}
            set
            {
            AssertMutable();
            if(!value&&!this.Tags.Contains(CardModfier.NorotTag))
            {
                this.Tags.AddItem(CardModfier.NorotTag);
                if(!this.Keywords.Contains(CardModfier.NorotKeyword))
                CardCmd.ApplyKeyword(this,CardModfier.NorotKeyword);
            }
			_rotable=value;
            RefreshCorpseVars();
            }
        }
        [SavedProperty]
        public int RotCount
        {
            get{return _rotcount;}
            set
            {
            AssertMutable();
			_rotcount=value;
            RefreshCorpseVars();
            }
        }
        [SavedProperty]
        public int Integrity 
        {
            get{return _integrity;}
            set
            {
            AssertMutable();
			_integrity=value;
            RefreshCorpseVars();
            }
        }
           [SavedProperty]
        public CorpseType CorpseType
        {
            get{return _corpseType;}
            set
            {
            AssertMutable();
			_corpseType=value;
            RefreshCorpseVars();
            }
        }
        public virtual void Init(int wzd,int rotCount,bool Rotable=true)
        {
            this.Integrity=wzd;
            if(wzd>=75)
            {
                this.CorpseType=CorpseType.Full;
            }
            else if(wzd>=50)
            {
                this.CorpseType=CorpseType.Half;
            }
            else
            {
                this.CorpseType=CorpseType.Quarter;
            }
            this.RotCount=rotCount;
            this.Rotable=Rotable;
            RefreshCorpseVars();
        }
        public virtual void TriggerWhenRemove()
        {
            
        }
        public virtual async Task TriggerWhenCombatStart()
        {
            await Task.CompletedTask;
        }
        public virtual async Task TriggerWhenTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            await Task.CompletedTask;
        }
        public virtual async Task TriggerWhenTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            await Task.CompletedTask;
        }
        public virtual async Task TriggerEffect()
        {
            await Task.CompletedTask;
        }
        private int _effect1nNum;
        private int _effect2nNum;
        private int _effect3nNum;
        [SavedProperty]
        public int Effect1nNum
        {
            get{return (int)(_effect1nNum*GetMutilplier());}
            set
            {
            AssertMutable();
			_effect1nNum=value;
            }
        }
        [SavedProperty]
        public int Effect2nNum
        {
            get{return (int)(_effect2nNum*GetMutilplier());}
            set
            {
            AssertMutable();
			_effect2nNum=value;
            }
        }
        [SavedProperty]
        public int Effect3nNum
        {
            get{return (int)(_effect3nNum*GetMutilplier());}
            set
            {
            AssertMutable();
			_effect3nNum=value;
            }
        }
        public decimal GetMutilplier()
        {
            switch(CorpseType)
            {
                case CorpseType.Full:
                    return 1;
                case CorpseType.Half:
                    return 0.5m;
                case CorpseType.Quarter:
                    return 0.25m;
                default:
                    return 0;
            }
        }
        public void Decreasement()
        {
            if(!Rotable)
            {
                return;
            }
            this.RotCount--;
            if(RotCount<=0)
            {
	            Tools.RemoveFromBarrow(this);
            }
        }
       
        protected CorpseCardModel(int baseCost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary = true, bool autoAdd = true) : base(baseCost, type, rarity, target, showInCardLibrary, autoAdd)
        {
            _rotable = !Keywords.Contains(CardModfier.NorotKeyword);
        }
    }

}
