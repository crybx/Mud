﻿using Objects.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class BaseObjectId : IBaseObjectId
    {
        public BaseObjectId()
        {
        }

        public BaseObjectId(int zone, int id)
        {
            Zone = zone;
            Id = id;
        }

        public BaseObjectId(IBaseObject baseObject)
        {
            Zone = baseObject.Zone;
            Id = baseObject.Id;
        }

        [ExcludeFromCodeCoverage]
        public int Zone { get; set; }
        [ExcludeFromCodeCoverage]
        public int Id { get; set; }

        public override string ToString()
        {
            return string.Format("{0}-{1}", Zone, Id);
        }
    }
}