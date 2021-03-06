﻿using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Interface;
using Objects.Crafting.Interface;
using Objects.Language;
using Shared.Sound.Interface;
using Shared.Sound;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Mob
{
    public class PlayerCharacter : MobileObject, IPlayerCharacter
    {
        public PlayerCharacter() : base()
        {

        }

        [ExcludeFromCodeCoverage]
        public string Name { get; set; }

        private int _experience;
        public int Experience
        {
            get
            {
                return _experience;
            }

            set
            {
                AddExperience(value);
            }
        }

        private void AddExperience(int newExperience)
        {
            int currentLevel = Level;
            //when experience is added it adds the existing experience; basically doubling the pc experience each time
            _experience += newExperience - _experience;
            if (GlobalReference.GlobalValues.Experience.GetExpForLevel(currentLevel) <= _experience)
            {
                LevelMobileObject();
                GlobalReference.GlobalValues.Notify.Mob(null, null, this, new TranslationMessage("You gain a level."));

                ISound sound = new Sound();
                sound.Loop = false;
                sound.SoundName = "Misc/Levelup.mp3";

                string serializeSounds = GlobalReference.GlobalValues.Serialization.Serialize(new List<ISound>() { sound });
                EnqueueMessage(GlobalReference.GlobalValues.TagWrapper.WrapInTag(serializeSounds, TagType.Sound));
            }
        }

        [ExcludeFromCodeCoverage]
        public string Password { get; set; }

        #region Stats
        [ExcludeFromCodeCoverage]
        public int StrengthBonus { get; set; }
        [ExcludeFromCodeCoverage]
        public int DexterityBonus { get; set; }
        [ExcludeFromCodeCoverage]
        public int ConstitutionBonus { get; set; }
        [ExcludeFromCodeCoverage]
        public int IntelligenceBonus { get; set; }
        [ExcludeFromCodeCoverage]
        public int WisdomBonus { get; set; }
        [ExcludeFromCodeCoverage]
        public int CharismaBonus { get; set; }

        [ExcludeFromCodeCoverage]
        public int StrengthMultiClassBonus { get; set; }
        [ExcludeFromCodeCoverage]
        public int DexterityMultiClassBonus { get; set; }
        [ExcludeFromCodeCoverage]
        public int ConstitutionMultiClassBonus { get; set; }
        [ExcludeFromCodeCoverage]
        public int IntelligenceMultiClassBonus { get; set; }
        [ExcludeFromCodeCoverage]
        public int WisdomMultiClassBonus { get; set; }
        [ExcludeFromCodeCoverage]
        public int CharismaMultiClassBonus { get; set; }

        public override int StrengthEffective
        {
            get
            {
                return base.StrengthEffective + StrengthBonus + GlobalReference.GlobalValues.MultiClassBonus.CalculateBonus(Level, StrengthMultiClassBonus);
            }
        }

        public override int DexterityEffective
        {
            get
            {
                return base.DexterityEffective + DexterityBonus + GlobalReference.GlobalValues.MultiClassBonus.CalculateBonus(Level, DexterityMultiClassBonus);
            }
        }

        public override int ConstitutionEffective
        {
            get
            {
                return base.ConstitutionEffective + ConstitutionBonus + GlobalReference.GlobalValues.MultiClassBonus.CalculateBonus(Level, ConstitutionMultiClassBonus);
            }
        }

        public override int IntelligenceEffective
        {
            get
            {
                return base.IntelligenceEffective + IntelligenceBonus + GlobalReference.GlobalValues.MultiClassBonus.CalculateBonus(Level, IntelligenceMultiClassBonus);
            }
        }

        public override int WisdomEffective
        {
            get
            {
                return base.WisdomEffective + WisdomBonus + GlobalReference.GlobalValues.MultiClassBonus.CalculateBonus(Level, WisdomMultiClassBonus);
            }
        }

        public override int CharismaEffective
        {
            get
            {
                return base.CharismaEffective + CharismaBonus + GlobalReference.GlobalValues.MultiClassBonus.CalculateBonus(Level, CharismaMultiClassBonus);
            }
        }
        #endregion Stats

        [ExcludeFromCodeCoverage]
        public bool Debug { get; set; }



        [ExcludeFromCodeCoverage]
        public IBaseObjectId RespawnPoint { get; set; } = new RoomId(1, 1);


        private List<ICorpse> _corpses;
        public List<ICorpse> Corpses
        {
            get
            {
                if (_corpses == null)
                {
                    _corpses = new List<ICorpse>();
                }
                return _corpses;
            }
        }

        public List<ICraftsmanObject> CraftsmanObjects { get; } = new List<ICraftsmanObject>();


        public string GotoEnterMessage { get; set; }
        public string GotoLeaveMessage { get; set; }

        public override ICorpse Die()
        {
            ICorpse corpse = base.Die();
            Corpses.Add(corpse.Clone());  //because if someone picks something up out of the corpse it will be reflected here

            Room.RemoveMobileObjectFromRoom(this);
            Room.Items.Insert(0, corpse);
            Room = GlobalReference.GlobalValues.World.Zones[RespawnPoint.Zone].Rooms[RespawnPoint.Id];
            Room.AddMobileObjectToRoom(this);
            Health = 1;
            Mana = 1;
            Stamina = 1;
            EnqueueCommand("Look");
            GlobalReference.GlobalValues.Engine.Event.EnterRoom(this);

            return corpse;
        }

        public void RemoveOldCorpses(DateTime utcDate)
        {
            if (Corpses.Count > 0)
            {
                while (Corpses.Count > 0
                    && Corpses[0].TimeOfDeath < utcDate)
                {
                    Corpses.RemoveAt(0);
                }
            }
        }
    }
}
