﻿using Objects.Global;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.LevelRange.Interface;
using Objects.Material.Materials;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Mob
{
    public class NonPlayerCharacter : MobileObject, INonPlayerCharacter, ILoadableItems
    {
        public NonPlayerCharacter() : base()
        {

        }

        private List<IPersonality> _personalities;
        public List<IPersonality> Personalities
        {
            get
            {
                if (_personalities == null)
                {
                    _personalities = new List<IPersonality>();
                }

                return _personalities;
            }
        }

        #region Exp/Level
        private int _exp = -1;
        public virtual int EXP
        {
            get
            {
                if (_exp == -1)
                {
                    return GlobalReference.GlobalValues.Experience.GetDefaultNpcExpForLevel(Level);
                }

                return _exp;
            }

            set
            {
                _exp = value;
            }
        }

        [ExcludeFromCodeCoverage]
        public ILevelRange LevelRange { get; set; }
        #endregion Exp/Level

        public override void FinishLoad(int zoneObjectSyncValue = -1)
        {
            base.FinishLoad(zoneObjectSyncValue);
            SetDefaultStats();
            if (Money == 0 && Level != 0)
            {
                Money = GlobalReference.GlobalValues.DefaultValues.MoneyForNpcLevel(Level);
            }

            SetValuesToMax();

            LoadNpcEquipment();

            UpdateFollowerToCurrentReference();

            foreach (IItem item in Items)
            {
                item.FinishLoad(zoneObjectSyncValue);
            }

            foreach (IItem item in EquipedWeapon)
            {
                item.FinishLoad(zoneObjectSyncValue);
            }

            foreach (IItem item in EquipedArmor)
            {
                item.FinishLoad(zoneObjectSyncValue);
            }
        }

        private void UpdateFollowerToCurrentReference()
        {
            if (FollowTarget != null)
            {
                //reload the follower target so we get a reference to the real target instead of a static copy 
                foreach (INonPlayerCharacter npc in Room.NonPlayerCharacters)
                {
                    if (npc.Id == FollowTarget.Id)
                    {
                        FollowTarget = npc;
                        break;
                    }
                }
            }
        }

        private void LoadNpcEquipment()
        {
            //calculate the additional armor pieces based upon level up to 10 minus the actual
            //number of armor pieces equipped
            int additionalPieces = Math.Min(9, Level) - base.EquipedArmor.Count() - NpcEquipedEquipment.Count;
            if (additionalPieces > 0)
            {
                //create 1 piece of armor and add it as many times as needed
                //saves cpu cycles and memory
                IArmor armor = new Armor();
                armor.Level = Level;
                armor.Material = new NpcInnateArmor(this);
                armor.FinishLoad();
                for (int i = 0; i < additionalPieces; i++)
                {
                    NpcEquipedEquipment.Add(armor);
                }
            }
        }

        #region Equipment
        private List<IEquipment> _npcEquipment = null;
        /// <summary>
        /// Any equipment placed here will not be dropped when the NPC dies
        /// </summary>
        public List<IEquipment> NpcEquipedEquipment
        {
            get
            {
                if (_npcEquipment == null)
                {
                    _npcEquipment = new List<IEquipment>();
                }
                return _npcEquipment;
            }
        }

        public override IEnumerable<IArmor> EquipedArmor
        {
            get
            {
                List<IArmor> armors = new List<IArmor>();
                foreach (IItem item in NpcEquipedEquipment)
                {
                    IArmor armor = item as IArmor;
                    if (armor != null)
                    {
                        armors.Add(armor);
                    }
                }
                armors.AddRange(base.EquipedArmor);
                return armors;
            }
        }
        #endregion Equipment

        private void SetValuesToMax()
        {
            Health = MaxHealth;
            Mana = MaxMana;
            Stamina = MaxStamina;
        }

        #region Stat Range
        [ExcludeFromCodeCoverage]
        public int StrengthMin { get; set; }
        [ExcludeFromCodeCoverage]
        public int StrengthMax { get; set; }

        [ExcludeFromCodeCoverage]
        public int DexterityMin { get; set; }
        [ExcludeFromCodeCoverage]
        public int DexterityMax { get; set; }

        [ExcludeFromCodeCoverage]
        public int ConstitutionMin { get; set; }
        [ExcludeFromCodeCoverage]
        public int ConstitutionMax { get; set; }

        [ExcludeFromCodeCoverage]
        public int IntelligenceMin { get; set; }
        [ExcludeFromCodeCoverage]
        public int IntelligenceMax { get; set; }

        [ExcludeFromCodeCoverage]
        public int WisdomMin { get; set; }
        [ExcludeFromCodeCoverage]
        public int WisdomMax { get; set; }

        [ExcludeFromCodeCoverage]
        public int CharismaMin { get; set; }
        [ExcludeFromCodeCoverage]
        public int CharismaMax { get; set; }
        #endregion Stat Range

        private void SetDefaultStats()
        {
            //check if NPC is missing all the stats but not Level
            if (StrengthStat == 0
                && DexterityStat == 0
                && ConstitutionStat == 0
                && IntelligenceStat == 0
                && WisdomStat == 0
                && CharismaStat == 0)
            {
                if (DeterminIfUseStatRange())
                {
                    SetStatRange();
                }
                else
                {
                    SetDefaultLevelStat();
                }

                //set the exp to -1 so it will be calculated later when killed
                EXP = -1;

                //set the max values to -1 so they will be calculated later
                MaxHealth = -1;
                MaxMana = -1;
                MaxStamina = -1;
            }
        }

        private bool DeterminIfUseStatRange()
        {
            if (StrengthMin == 0 && StrengthMax == 0
                && DexterityMin == 0 && DexterityMax == 0
                && ConstitutionMin == 0 && ConstitutionMax == 0
                && IntelligenceMin == 0 && IntelligenceMax == 0
                && WisdomMin == 0 && WisdomMax == 0
                && CharismaMin == 0 && CharismaMax == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SetStatRange()
        {
            StrengthStat = GlobalReference.GlobalValues.Random.Next(StrengthMin, StrengthMax + 1);
            DexterityStat = GlobalReference.GlobalValues.Random.Next(DexterityMin, DexterityMax + 1);
            ConstitutionStat = GlobalReference.GlobalValues.Random.Next(ConstitutionMin, ConstitutionMax + 1);
            IntelligenceStat = GlobalReference.GlobalValues.Random.Next(IntelligenceMin, IntelligenceMax + 1);
            WisdomStat = GlobalReference.GlobalValues.Random.Next(WisdomMin, WisdomMax + 1);
            CharismaStat = GlobalReference.GlobalValues.Random.Next(CharismaMin, CharismaMax + 1);
        }

        private void SetDefaultLevelStat()
        {
            if (Level == 1 && LevelRange != null)
            {
                Level = GlobalReference.GlobalValues.Random.Next(LevelRange.LowerLevel, LevelRange.UpperLevel + 1);
            }

            if (Level >= 1)
            {
                StrengthStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
                DexterityStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
                ConstitutionStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
                IntelligenceStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
                WisdomStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
                CharismaStat = GlobalReference.GlobalValues.Settings.BaseStatValue;

                LevelPoints = GlobalReference.GlobalValues.Settings.AssignableStatPoints;

                while (LevelPoints > 0)
                {
                    LevelRandomStat();
                }

                for (int i = 1; i < Level; i++)
                {
                    LevelMobileObject();
                    //puts the level back to what it was before so this will eventually stop leveling 
                    //when we get to the desired level
                    Level--;
                    while (LevelPoints > 0)
                    {
                        LevelRandomStat();
                    }
                }
            }
        }

        private void LevelRandomStat()
        {
            int amount = 1;
            if (LevelPoints > 10)
            {
                amount = LevelPoints / 10;
            }

            switch (GlobalReference.GlobalValues.Random.Next(6))
            {
                case 0:
                    StrengthStat += amount;
                    break;
                case 1:
                    DexterityStat += amount;
                    break;
                case 2:
                    ConstitutionStat += amount;
                    break;
                case 3:
                    IntelligenceStat += amount;
                    break;
                case 4:
                    WisdomStat += amount;
                    break;
                case 5:
                    CharismaStat += amount;
                    break;
            }

            LevelPoints -= amount;
        }

        public override ICorpse Die()
        {
            ICorpse corpse = base.Die();

            Room.RemoveMobileObjectFromRoom(this);
            Room.Items.Insert(0, corpse);

            return corpse;
        }
    }
}
