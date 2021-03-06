﻿using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Room.Interface;
using Objects.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Zone
{
    public class Zone : BaseObject, IZone, ILoadable
    {
        [ExcludeFromCodeCoverage]
        public bool RepeatZoneProcessing { get; set; }
        [ExcludeFromCodeCoverage]
        public object LockObject { get; } = new object();
        [ExcludeFromCodeCoverage]
        public int ZoneObjectSyncOptions { get; set; } = -1;
        [ExcludeFromCodeCoverage]
        public int InGameDaysTillReset { get; set; } = 150;

        [ExcludeFromCodeCoverage]
        public DateTime ResetTime { get; set; }

        [ExcludeFromCodeCoverage]
        public string Name { get; set; }

        [ExcludeFromCodeCoverage]
        public Dictionary<int, IRoom> Rooms { get; set; } = new Dictionary<int, IRoom>();

        #region Weather
        #region Weather High Points
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationHighBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationHighEnd { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationExtraHighBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationExtraHighEnd { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedHighBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedHighEnd { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedExtraHighBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedExtraHighEnd { get; set; }
        #endregion Weather High Points

        #region Weather Low Points
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationLowBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationLowEnd { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationExtraLowBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZonePrecipitationExtraLowEnd { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedLowBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedLowEnd { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedExtraLowBegin { get; set; }
        [ExcludeFromCodeCoverage]
        public string ZoneWindSpeedExtraLowEnd { get; set; }
        #endregion Weather Low Points
        #endregion Weather

        /// <summary>
        /// The zoneObjectSyncValue is not used at the Zone level but is needed for the interface
        /// </summary>
        /// <param name="zoneObjectSyncValue"></param>
        public override void FinishLoad(int zoneObjectSyncValue = -1)
        {
            ResetTime = GlobalReference.GlobalValues.GameDateTime.InGameDateTime.AddDays(InGameDaysTillReset).Date;

            if (ZoneObjectSyncOptions != -1)
            {
                zoneObjectSyncValue = GlobalReference.GlobalValues.Random.Next(ZoneObjectSyncOptions);
            }

            FinishLoadingObjects(zoneObjectSyncValue);
        }

        private void FinishLoadingObjects(int zoneObjectSyncValue)
        {
            foreach (IRoom room in Rooms.Values)
            {
                room.FinishLoad(zoneObjectSyncValue);
                room.ZoneObjectSyncLoad(zoneObjectSyncValue);

                foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
                {
                    npc.FinishLoad(zoneObjectSyncValue);
                    npc.ZoneObjectSyncLoad(zoneObjectSyncValue);
                }

                foreach (IItem item in room.Items)
                {
                    item.FinishLoad(zoneObjectSyncValue);
                    item.ZoneObjectSyncLoad(zoneObjectSyncValue);

                    IContainer container = item as IContainer;
                    if (container != null)
                    {
                        RecursiveLoadItem(container, ZoneObjectSyncOptions);
                    }
                }
            }
        }

        private void RecursiveLoadItem(IContainer container, int zoneObjectSyncOptions)
        {
            foreach (IItem item in container.Items)
            {
                item.FinishLoad(zoneObjectSyncOptions);
                item.ZoneObjectSyncLoad(zoneObjectSyncOptions);

                IContainer container2 = item as IContainer;
                if (container2 != null)
                {
                    RecursiveLoadItem(container2, ZoneObjectSyncOptions);
                }
            }
        }

        public void RecursivelySetZone()
        {
            foreach (IRoom room in Rooms.Values)
            {
                room.Zone = Id;

                foreach (IItem item in room.Items)
                {
                    SetItemId(item);
                }

                foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
                {
                    npc.Zone = Id;
                    foreach (IItem item in npc.Items)
                    {
                        SetItemId(item);
                    }

                    foreach (IItem item in npc.EquipedEquipment)
                    {
                        SetItemId(item);
                    }

                    foreach (IPersonality personality in npc.Personalities)
                    {
                        IMerchant merchant = personality as IMerchant;
                        if (merchant != null)
                        {
                            foreach (IItem item in merchant.Sellables)
                            {
                                SetItemId(item);
                            }
                        }
                    }
                }
            }
        }

        private void SetItemId(IItem item)
        {
            item.Zone = Id;
            IContainer container = item as IContainer;
            if (container != null)
            {
                foreach (IItem innerItem in container.Items)
                {
                    SetItemId(innerItem);
                }
            }
        }
    }
}
