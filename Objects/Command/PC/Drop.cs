﻿using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using Objects.Global;
using Objects.Room.Interface;

namespace Objects.Command.PC
{
    public class Drop : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Drop [Item Name]");


        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Drop" };


        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count <= 0)
            {
                return new Result(false, "What would you like to drop?");
            }

            IParameter parm = command.Parameters[0];
            IItem item = GlobalReference.GlobalValues.FindObjects.FindHeldItemsOnMob(performer, parm.ParameterValue, parm.ParameterNumber);

            if (item != null)
            {
                IRoom room = performer.Room;
                GlobalReference.GlobalValues.Engine.Event.Drop(performer, item);
                performer.Items.Remove(item);
                room.Items.Insert(0, item);


                string message = string.Format("You dropped {0}.", item.SentenceDescription);
                return new Result(true, message);
            }
            else
            {
                string message = string.Format("You were unable to find {0}.", command.Parameters[0].ParameterValue);
                return new Result(false, message);
            }
        }
    }
}
