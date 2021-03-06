﻿using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Item.Items.Interface;
using Objects.Room.Interface;
using static Objects.Global.Direction.Directions;

namespace Objects.Command.PC
{
    public class Open : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Open [Item Name]");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Open" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count == 0)
            {
                return new Result(false, "While you ponder what to open you let you mouth hang open.  Hey you did open something!");
            }

            IParameter parameter = command.Parameters[0];
            IBaseObject foundItem = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, parameter.ParameterValue, parameter.ParameterNumber, true, true, false, false, true);

            if (foundItem != null)
            {
                IDoor door = foundItem as IDoor;

                if (door != null)
                {
                    IResult result = ProcessDoor(performer, door);
                    if (result != null)
                    {
                        return result;
                    }

                    if (door.Linked)
                    {
                        IRoom otherRoom = GlobalReference.GlobalValues.World.Zones[door.LinkedRoomId.Zone].Rooms[door.LinkedRoomId.Id];
                        IDoor otherDoor = null;
                        switch (door.LinkedRoomDirection)
                        {
                            case Direction.North:
                                otherDoor = otherRoom.North.Door;
                                break;
                            case Direction.East:
                                otherDoor = otherRoom.East.Door;
                                break;
                            case Direction.South:
                                otherDoor = otherRoom.South.Door;
                                break;
                            case Direction.West:
                                otherDoor = otherRoom.West.Door;
                                break;
                            case Direction.Up:
                                otherDoor = otherRoom.Up.Door;
                                break;
                            case Direction.Down:
                                otherDoor = otherRoom.Down.Door;
                                break;
                        }

                        if (otherDoor != null)
                        {
                            otherDoor.Locked = false;
                            otherDoor.Opened = true;
                        }
                    }
                    return door.Open();
                }
                else
                {

                    IOpenable openable = foundItem as IOpenable;

                    if (openable != null)
                    {
                        return openable.Open();
                    }
                }

                return new Result(false, "You found what you were looking for but could not figure out how to open it.");
            }
            else
            {
                return new Result(false, "You were unable to find that what you were looking for.");
            }
        }

        private static IResult ProcessDoor(IMobileObject mob, IDoor door)
        {
            bool canOpen = true;
            if (door.Locked)
            {
                IEnumerable<IItem> keyPosibility = mob.Items.Where(i => i.Id == door.KeyNumber && i.Zone == door.Zone);
                IItem key = keyPosibility.FirstOrDefault();
                if (key == null)
                {
                    canOpen = false;
                }
            }

            //if the door can not be opened then return that
            if (!canOpen)
            {
                return new Result(false, "The door is locked and will not open.");
            }

            //the door can be opened and will be opened after control is returned
            return null;
        }
    }
}
