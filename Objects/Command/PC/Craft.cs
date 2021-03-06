﻿using Objects.Command.Interface;
using Objects.Damage;
using Objects.Item.Items;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;
using static Objects.Item.Items.Equipment;

namespace Objects.Command.PC
{
    public class Craft : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Craft [Position] [Level] [Keyword] [\"SentenceDescription\"] [\"ShortDescription\"] [\"LongDescription\"] [\"ExamineDescription\"] {DamageType}");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Craft" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IPlayerCharacter pc = performer as IPlayerCharacter;
            if (pc == null)
            {
                return new Result(false, "Only player characters can have craftsman craft items.");
            }

            INonPlayerCharacter craftsman = null;
            ICraftsman craftsmanPersonality = null;
            FindCraftsman(performer, ref craftsman, ref craftsmanPersonality);

            if (craftsman == null)
            {
                return new Result(false, "There is no craftsman to make anything for you.");
            }

            if (command.Parameters.Count < 7)
            {
                return new Result(false, "Please provide all the parameters needed for the craftsman to make your item.");
            }

            try
            {
                AvalableItemPosition position = GetPosition(command.Parameters[0].ParameterValue);
                if (position == AvalableItemPosition.Wield && command.Parameters.Count < 8)
                {
                    return new Result(false, "Damage type is required for weapons.");
                }

                int level = int.Parse(command.Parameters[1].ParameterValue);
                string keyword = command.Parameters[2].ParameterValue;
                string sentenceDescription = command.Parameters[3].ParameterValue;
                string shortDescription = command.Parameters[4].ParameterValue;
                string longDescription = command.Parameters[5].ParameterValue;
                string examineDescription = command.Parameters[6].ParameterValue;
                DamageType damageType = DamageType.Acid;
                if (position == AvalableItemPosition.Wield)
                {
                    damageType = GetDamageType(command.Parameters[7].ParameterValue);
                }

                return craftsmanPersonality.Build(craftsman, pc, position, level, keyword, sentenceDescription, shortDescription, longDescription, examineDescription, damageType);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    return new Result(false, ex.Message);
                }
                else
                {
                    return new Result(false, "Please verify all parameters and try again.");
                }
            }
        }

        private DamageType GetDamageType(string damageType)
        {
            string damageTypeLower = damageType.ToLower();

            switch (damageTypeLower)
            {
                case "bludgeon":
                    return DamageType.Bludgeon;
                case "pierce":
                    return DamageType.Pierce;
                case "slash":
                    return DamageType.Slash;
                default:
                    throw new ArgumentException(
@"Available damage types are Bludgeon,
                             Pierce,
                             Slash");
            }
        }

        private static void FindCraftsman(IMobileObject performer, ref INonPlayerCharacter craftsman, ref ICraftsman craftsmanPersonality)
        {
            foreach (INonPlayerCharacter npc in performer.Room.NonPlayerCharacters)
            {
                foreach (IPersonality personality in npc.Personalities)
                {
                    if (personality is ICraftsman)
                    {
                        craftsmanPersonality = personality as ICraftsman;
                        craftsman = npc;
                        break;
                    }
                }

                if (craftsman != null)
                {
                    break;
                }
            }
        }

        private AvalableItemPosition GetPosition(string position)
        {
            string positionLowerCase = position.ToLower();

            switch (positionLowerCase)
            {
                case "arms":
                    return AvalableItemPosition.Arms;
                case "body":
                    return AvalableItemPosition.Body;
                case "feet":
                    return AvalableItemPosition.Feet;
                case "finger":
                    return AvalableItemPosition.Finger;
                case "hand":
                    return AvalableItemPosition.Hand;
                case "head":
                    return AvalableItemPosition.Head;
                case "held":
                    return AvalableItemPosition.Held;
                case "legs":
                    return AvalableItemPosition.Legs;
                case "neck":
                    return AvalableItemPosition.Neck;
                case "waist":
                    return AvalableItemPosition.Waist;
                case "wield":
                    return AvalableItemPosition.Wield;
                default:
                    throw new ArgumentException(
@"Available positions are Wield,
                          Head,
                          Neck,
                          Arms,
                          Hand,
                          Finger,
                          Body,
                          Waist,
                          Legs,
                          Feet,
                          Held ");
            }
        }
    }
}
