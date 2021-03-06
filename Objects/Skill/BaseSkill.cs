﻿using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Skill.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Skill
{
    public abstract class BaseSkill : Ability.Ability, ISkill
    {
        [ExcludeFromCodeCoverage]
        public bool Passive { get; set; } = false;
        [ExcludeFromCodeCoverage]
        public int StaminaCost { get; set; }
        [ExcludeFromCodeCoverage]
        public string SkillName
        {
            get
            {
                return AbilityName;
            }
            set
            {
                AbilityName = value;
            }
        }

        public virtual string TeachMessage
        {
            get
            {
                return "The best way to learn is with lots practice.";
            }
        }

        public virtual IResult ProcessSkill(IMobileObject performer, ICommand command)
        {
            if (performer.Stamina > StaminaCost)
            {
                performer.Stamina -= StaminaCost;
                Effect.ProcessEffect(Parameter);
                IMobileObject targetMob = Parameter.Target as IMobileObject;
                List<IMobileObject> exclusions = new List<IMobileObject>() { performer };
                if (targetMob != null
                    && !exclusions.Contains(targetMob))
                {
                    exclusions.Add(targetMob);
                }

                if (RoomNotification != null)
                {
                    GlobalReference.GlobalValues.Notify.Room(performer, targetMob, performer.Room, RoomNotification, exclusions);
                }

                if (TargetNotification != null)
                {
                    if (targetMob != null)
                    {
                        GlobalReference.GlobalValues.Notify.Mob(targetMob, TargetNotification);
                    }
                }

                return new Result(true, PerformerNotification.GetTranslatedMessage(performer), null);
            }
            else
            {
                return new Result(false, $"You need {StaminaCost} stamina to use the skill {command.Parameters[0].ParameterValue}.");
            }
        }
    }
}