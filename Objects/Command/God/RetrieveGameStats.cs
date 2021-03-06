﻿using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Global;
using Objects.Mob.Interface;

namespace Objects.Command.God
{
    public class RetrieveGameStats : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "RetrieveGameStats");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "RetrieveGameStats" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            string value = null;
            GlobalReference.GlobalValues.World.WorldResults.TryGetValue("GameStats", out value);

            if (value != null)
            {
                return new Result(true, value);
            }
            else
            {
                return new Result(false, "Unable to retrieve game stats.");
            }
        }
    }
}
