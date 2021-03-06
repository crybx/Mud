﻿using Objects.Command.Interface;
using Objects.Global;
using Objects.Language;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Command.PC
{
    public class Tell : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Tell [Player Name] [Message]");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Tell" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count > 0)
            {
                if (command.Parameters.Count > 1)
                {
                    IPlayerCharacter player = FindPlayer(command.Parameters[0].ParameterValue);

                    if (player != null)
                    {
                        StringBuilder strBldr = new StringBuilder();
                        for (int i = 1; i < command.Parameters.Count; i++)
                        {
                            strBldr.Append(command.Parameters[i].ParameterValue + " ");
                        }

                        string message = string.Format("{0} tells you -- {1}", performer.KeyWords[0], strBldr.ToString()).Trim();
                        GlobalReference.GlobalValues.Notify.Mob(player, new TranslationMessage(message, TagType.Communication));

                        return new Result(true, "");
                    }
                    else
                    {
                        string message = string.Format("Unable to find {0} to tell them.", command.Parameters[0].ParameterValue);
                        return new Result(false, message);
                    }
                }
                else
                {
                    return new Result(false, "What would you like to tell them?");
                }
            }
            else
            {
                return new Result(false, "Who would you like to tell what?");
            }
        }

        private IPlayerCharacter FindPlayer(string parameterValue)
        {
            return GlobalReference.GlobalValues.World.CurrentPlayers.FirstOrDefault(e => e.KeyWords.Any(f => f.Equals(parameterValue, StringComparison.CurrentCultureIgnoreCase)));
        }
    }
}
