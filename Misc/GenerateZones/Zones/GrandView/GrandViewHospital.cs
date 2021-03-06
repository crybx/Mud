﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Zone.Interface;
using Objects.Zone;
using System.Reflection;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Magic.Enchantment;
using Objects.Effect;
using Objects.Mob.Interface;
using Objects.Mob;
using Objects;
using static Objects.Global.Direction.Directions;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Item.Items;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Die;
using Objects.Global;
using static Objects.Damage.Damage;
using static Objects.Global.Stats.Stats;
using static Objects.Item.Item;
using Objects.Material.Interface;
using Objects.Material.Materials;
using static Objects.Item.Items.Equipment;
using Objects.Personality.Personalities;
using Objects.Magic.Interface;
using Objects.Magic.Spell.Generic;
using Objects.Global.Language;
using Objects.Language;
using Objects.Language.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace GenerateZones.Zones.GrandView
{
    public class GrandViewHospital : IZoneCode
    {
        Zone zone = new Zone();
        int roomId = 1;
        int itemId = 1;
        int npcId = 1;
        public IZone Generate()
        {
            zone.Id = 7;
            zone.InGameDaysTillReset = 1;
            zone.Name = nameof(GrandViewHospital);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    IRoom room = (IRoom)method.Invoke(this, null);
                    room.Zone = zone.Id;
                    ZoneHelper.AddRoom(zone, room);
                }
            }

            ConnectRooms();

            return zone;
        }

        #region Rooms
        /// <summary>
        /// This is the room you will enter after leaving the starting room and coming back.
        /// </summary>
        /// <returns></returns>
        private IRoom GenerateRoom1()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Peaceful);

            room.ExamineDescription = "The place is dry and well lit.  The beds are comfortable and clean.  While the initial scan of the place seems nice it is still a hospital.  People are here because they are sick.";
            room.LongDescription = "This corner of the hospital is quiet.  One or two beds are occupied by sleeping patients.";
            room.ShortDescription = "A quiet corner of the hospital";

            room.AddMobileObjectToRoom(Nurse());
            room.Items.Add(HospitalGown());
            room.Items.Add(HospitalGown());
            room.Items.Add(HospitalGown());
            return room;
        }


        private INonPlayerCharacter Nurse()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;

            npc.ExamineDescription = "The nurse is dressed in blue dress with a white apron that covers the front of her and ties in the back.  On her head she wears a white cloth that drapes down on either side of her head.";
            npc.LongDescription = "A nurse busily attends to her patients, taking care to make sure each one is as comfortable as possible.";
            npc.ShortDescription = "A nurse attending patients.";
            npc.KeyWords.Add("Nurse");


            npc.Personalities.Add(new Healer() { CastPercent = 10 });
            npc.SpellBook.Add("HEALTH", Health());
            npc.SpellBook.Add("MAGIC", Magic());
            npc.SpellBook.Add("STAMIA", Stamina());

            return npc;
        }

        private ISpell Health()
        {
            ISpell heal = BuildSpell("health");

            heal.TargetNotification = new TranslationMessage("You feel healthier.");
            heal.Effect = new RecoverHealth();

            return heal;
        }

        private static ISpell BuildSpell(string spellName)
        {
            ISpell spell = new SingleTargetSpell();
            spell.SpellName = spellName;
            spell.ManaCost = 0;
            spell.Parameter.Dice = GlobalReference.GlobalValues.DefaultValues.ReduceValues(1, 100);
            string message = "The nurse says {0} and is briefly surrounded in a aura of light.";
            List<ITranslationPair> translate = new List<ITranslationPair>();
            ITranslationPair translationPair = new TranslationPair(Translator.Languages.Magic, spellName);
            translate.Add(translationPair);
            ITranslationMessage translationMessage = new TranslationMessage(message, TagType.Info, translate);
            spell.RoomNotification = translationMessage;
            spell.PerformerNotification = new TranslationMessage("you cast a spell");
            return spell;
        }

        private ISpell Magic()
        {
            ISpell heal = BuildSpell("magic");
            heal.TargetNotification = new TranslationMessage("You feel more magical.");
            heal.Effect = new RecoverMana();

            return heal;
        }

        private ISpell Stamina()
        {
            ISpell heal = BuildSpell("stamina");
            heal.TargetNotification = new TranslationMessage("You feel more energetic.");
            heal.Effect = new RecoverStamina();

            return heal;
        }

        private IArmor HospitalGown()
        {
            IArmor armor = new Armor();
            armor.Level = 1;
            armor.Id = itemId++;
            armor.ItemPosition = AvalableItemPosition.Body;
            armor.Material = new Cloth();
            armor.KeyWords.Add("Hospital");
            armor.KeyWords.Add("Gown");
            armor.ShortDescription = "A loose fitting hospital gown";
            armor.LongDescription = "This is your everyday hospital gown.  White with little blue dots with a tie in the back that exposes a little to much of your rear.";
            armor.ExamineDescription = "Thin and airy this would not provide much defense against anything other than people staring at you.  On second thought if you wore this out in public, people would stare.  This really wouldn't do much good.";
            armor.SentenceDescription = "hospital gown";
            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(armor.Level);
            armor.FinishLoad();
            return armor;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = GenerateRoom();

            room.ExamineDescription = "The hallway way is fairly long and is covered from the floor to the walls in tie.  Foot steps echo up and down giving an empty cold and sterile feeling.";
            room.LongDescription = "You stand at the intersection of the recovery, surgery and entrance of the hospital.";
            room.ShortDescription = "A hallway";
            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = GenerateRoom();

            room.ExamineDescription = "The entrance to GrandView Hospital presents a nice face for what could be an awful experience.  The receptions desk is situated such that it is the first thing you see.  Made of some type of imported wood it stands several feet tall and has a stone top.  Several pieces of paper and pens sit neatly arranged on the desk.";
            room.LongDescription = "The entrance of GrandView Hospital is before you.  By the entrance is the receptions desk and general seating is behind there.  To the west is the surgery and recovery wing and to the south is the morgue.";
            room.ShortDescription = "The entrance of GrandView Hospital";
            return room;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = GenerateRoom();

            room.ExamineDescription = "The operating table sits in the middle of the room.  It has straps for holding the patient in place while the operation is underway.  There is a tray of surgical utensils next to the table and on the far west wall a poster labeling the different body parts.";
            room.LongDescription = "You have entered the main operating room.  It looks like the room is ready for an operation but there is no one here.";
            room.ShortDescription = "Surgery";

            room.Items.Add(Scalpel());
            room.Items.Add(Scalpel());
            room.Items.Add(Scalpel());
            return room;
        }

        private IWeapon Scalpel()
        {
            IWeapon scalpel = new Weapon();
            scalpel.Level = 1;
            scalpel.Id = itemId++;
            scalpel.KeyWords.Add("Scalpel");
            scalpel.ShortDescription = "A surgical scalpel.";
            scalpel.LongDescription = "Made of precision surgical steel it as reflective as it is sharp.";
            scalpel.ExamineDescription = "Useful for making surgical cuts in the hands of a doctor and looking like you escaped from a mental hospital in yours.  Perhaps you should try to find a better, less psycho imagery inducing weapon.";
            scalpel.SentenceDescription = "surgical scalpel";
            scalpel.AttackerStat = Stat.Dexterity;
            scalpel.DeffenderStat = Stat.Dexterity;

            IDamage damage = new Objects.Damage.Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(scalpel.Level);
            damage.Type = DamageType.Slash;
            scalpel.DamageList.Add(damage);
            scalpel.FinishLoad();
            return scalpel;
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = GenerateRoom();

            room.ExamineDescription = "Each of the shelves hold five bodies wrapped in white sheets.  There are a pair of bodies in the corner drained of blood being filed with embalming fluids.";
            room.LongDescription = "The morgue is filled with shelves of dead bodies wrapped in white sheets.";
            room.ShortDescription = "Morgue";

            room.Items.Add(GlowingJar());
            room.Items.Add(GlowingJar());
            room.Items.Add(GlowingJar());
            room.Items.Add(GlowingJar());
            room.Items.Add(GlowingJar());

            return room;
        }


        /// <summary>
        /// This is the room you will enter after dieing on the lifeboat on the starting ship.
        /// </summary>
        /// <returns></returns>
        private IRoom GenerateRoom2()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Peaceful);

            room.ExamineDescription = "The place is dry and well lit.  The beds are comfortable and clean.  While the initial scan of the place seems nice it is still a hospital.  People are here because they are sick.";
            room.LongDescription = "This corner of the hospital is quiet.  One or two beds are occupied by sleeping patients.";
            room.ShortDescription = "A quiet corner of the hospital";

            EnterRoomEnchantment enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new SetRespawnPoint();
            enter.Parameter.ObjectId = new BaseObjectId(7, 3); //hospital
            room.Enchantments.Add(enter);

            INonPlayerCharacter npc = Nurse();
            enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new Tell();
            enter.Parameter.ObjectId = new BaseObjectId(zone.Id, npc.Id);
            enter.Parameter.Message = new TranslationMessage("It's good to have you among the living again.  Next time be more careful because dieing isn't fun and your going to have a nasty scar to remind you of it.", TagType.Communication);
            npc.Enchantments.Add(enter);
            room.AddMobileObjectToRoom(npc);
            room.Items.Add(HospitalGown());
            room.Items.Add(HospitalGown());
            room.Items.Add(HospitalGown());
            return room;
        }

        /// <summary>
        /// This is the room you will respawn in after the ship.
        /// </summary>
        /// <returns></returns>
        private IRoom GenerateRoom3()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Peaceful);

            room.ExamineDescription = "The place is dry and well lit.  The beds are comfortable and clean.  While the initial scan of the place seems nice it is still a hospital.  People are here because they are sick.";
            room.LongDescription = "This corner of the hospital is quiet.  One or two beds are occupied by sleeping patients.";
            room.ShortDescription = "A quiet corner of the hospital";

            EnterRoomEnchantment enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new SetRespawnPoint();
            enter.Parameter.ObjectId = new BaseObjectId(7, 2); //hospital
            room.Enchantments.Add(enter);

            INonPlayerCharacter npc = Nurse();
            enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new Tell();
            enter.Parameter.ObjectId = new RoomId(zone.Id, npc.Id);
            enter.Parameter.Message = new TranslationMessage("It's good to have you conscious again.  The other nurses and I were getting worried when you wouldn't wake up for 2 weeks.  We called in the best healers but they were beginning to loose hope.  Still we had hope because at night would scream and thrash like you were on fire.  It is no wonder since over 85% of your body was burned.  I heard the ship you were on was attacked by the pair of dragons off the coast.  One of the passengers said you were about to make it in to the life boat when you were knocked overboard by a fireball from one of the dragons.  One of the passengers Noah Davies jumped in and saved you from drowning but you were unconscious when they pulled you into the boat.  You floated on the ocean adrift for 3 days before you were picked up by Captain Reynolds.  Once you and the rest of the survivors were picked up you were brought here to GrandView Hospital where we have been treating you.", TagType.Communication);
            npc.Enchantments.Add(enter);
            room.AddMobileObjectToRoom(npc);
            room.Items.Add(HospitalGown());
            room.Items.Add(HospitalGown());
            room.Items.Add(HospitalGown());
            return room;
        }


        /// <summary>
        /// This is the room you will respawn while dieing on the ship.
        /// </summary>
        /// <returns></returns>
        private IRoom GenerateRoom8()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Peaceful);

            room.ExamineDescription = "The place is dry and well lit.  The beds are comfortable and clean.  While the initial scan of the place seems nice it is still a hospital.  People are here because they are sick.";
            room.LongDescription = "This corner of the hospital is quiet.  One or two beds are occupied by sleeping patients.";
            room.ShortDescription = "A quiet corner of the hospital";

            EnterRoomEnchantment enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new SetRespawnPoint();
            enter.Parameter.ObjectId = new BaseObjectId(7, 2); //hospital
            room.Enchantments.Add(enter);

            INonPlayerCharacter npc = Nurse();
            enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new Tell();
            enter.Parameter.ObjectId = new RoomId(zone.Id, npc.Id);
            enter.Parameter.Message = new TranslationMessage("It's good to have you conscious again.  The other nurses and I were getting worried when you wouldn't wake up for 2 weeks.  We called in the best healers but they were beginning to loose hope.  Still we had hope because at night would scream and thrash like you were on fire.  It is no wonder since over 85% of your body was burned.  I heard the ship you were on was attacked by the pair of dragons off the coast.  One of the passengers said you were still on the ship when it exploded. One of the passengers Noah Davies jumped in and saved you from drowning but you were unconscious when they pulled you into the boat.  You floated on the ocean adrift for 3 days before you were picked up by Captain Reynolds.  Once you and the rest of the survivors were picked up you were brought here to GrandView Hospital where we have been treating you.", TagType.Communication);
            npc.Enchantments.Add(enter);
            room.AddMobileObjectToRoom(npc);
            room.Items.Add(HospitalGown());
            room.Items.Add(HospitalGown());
            room.Items.Add(HospitalGown());
            return room;
        }

        private IItem GlowingJar()
        {
            IEquipment item = new Equipment();
            item.Id = itemId++;
            item.Attributes.Add(ItemAttribute.Light);
            item.ItemPosition = Equipment.AvalableItemPosition.Held;
            item.ExamineDescription = "A small jar about 1.5 inches tall.  It glows with a soft {color} light that is capable of lighting a room without being harsh on the eyes.";
            item.KeyWords.Add("Jar");
            item.KeyWords.Add("{color}");
            item.LongDescription = "While not very big it does produces a soft {color} glow that is capable of lighting up a room.";
            item.SentenceDescription = "glowing jar";
            item.ShortDescription = "A glowing jar.";

            item.FlavorOptions.Add("{color}", new List<string>() { "green", "blue", "white", "yellow", "red", "purple" });

            return item;
        }

        private IRoom GenerateRoom()
        {
            IRoom room = new Room();
            room.Id = roomId++;
            room.MovementCost = 1;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);

            return room;
        }

        #endregion Rooms

        private void ConnectRooms()
        {
            zone.RecursivelySetZone();

            //we do these rooms like this because we don't want to connect back
            zone.Rooms[2].East = new Exit() { Zone = 7, Room = 4 };
            zone.Rooms[3].East = new Exit() { Zone = 7, Room = 4 };
            zone.Rooms[8].East = new Exit() { Zone = 7, Room = 4 };

            ZoneHelper.ConnectRoom(zone.Rooms[1], Direction.East, zone.Rooms[4]);
            ZoneHelper.ConnectZone(zone.Rooms[5], Direction.North, 6, 12);
            ZoneHelper.ConnectRoom(zone.Rooms[4], Direction.East, zone.Rooms[5]);
            ZoneHelper.ConnectRoom(zone.Rooms[4], Direction.North, zone.Rooms[6]);
            ZoneHelper.ConnectRoom(zone.Rooms[5], Direction.South, zone.Rooms[7]);
        }
    }
}
