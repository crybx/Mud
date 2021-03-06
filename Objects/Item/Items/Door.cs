﻿using Objects.Item.Items.Interface;
using System.Diagnostics.CodeAnalysis;
using Objects.Command.Interface;
using System;
using Objects.Command;
using Objects.Global.Direction;
using Objects.Interface;

namespace Objects.Item.Items
{
    public class Door : Item, IDoor
    {
        [ExcludeFromCodeCoverage]
        public string OpenMessage { get; set; }

        [ExcludeFromCodeCoverage]
        public bool Opened { get; set; }

        [ExcludeFromCodeCoverage]
        public int KeyNumber { get; set; } = -1;

        [ExcludeFromCodeCoverage]
        public bool Pickable { get; set; }

        [ExcludeFromCodeCoverage]
        public bool Locked { get; set; }

        [ExcludeFromCodeCoverage]
        public int PickDificulty { get; set; }

        [ExcludeFromCodeCoverage]
        public bool Linked { get; set; }

        [ExcludeFromCodeCoverage]
        public IBaseObjectId LinkedRoomId { get; set; }

        [ExcludeFromCodeCoverage]
        public Directions.Direction LinkedRoomDirection { get; set; }

        public IResult Open()
        {
            Opened = true;
            return new Result(true, OpenMessage);
        }
    }
}
