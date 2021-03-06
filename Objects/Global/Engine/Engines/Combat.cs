﻿using Objects.Damage.Interface;
using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Mob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Objects.Global.Stats.Stats;
using Objects.Global.Stats;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Objects.Mob.Interface;
using Objects.Command.Interface;
using Objects.Command;
using Objects.Global.Engine.Engines.Interface;
using Objects.Interface;

namespace Objects.Global.Engine.Engines
{
    public class Combat : ICombat
    {
        private uint _combatRound { get; set; } = 0;

        private ConcurrentDictionary<IMobileObject, CombatPair.CombatPair> combatants { get; } = new ConcurrentDictionary<IMobileObject, CombatPair.CombatPair>();

        public void ProcessCombatRound()
        {
            //set the combat round so we can determine if slow weapons need to hit
            _combatRound++;

            List<IMobileObject> combatOrder = combatants.Keys.OrderByDescending(o => o.CalculateAttackOrderRoll()).ToList();

            if (combatOrder.Count > 0)
            {
                foreach (IMobileObject mob in combatOrder)
                {
                    ProcessAttack(mob);
                }
            }
        }

        private void ProcessAttack(IMobileObject mob)
        {
            CombatPair.CombatPair pair = combatants[mob];
            bool invalidComabatPair = RemoveInvalidCombatant(mob, pair);

            if (!invalidComabatPair)
            {
                ProcessWeapons(pair.Attacker, pair.Defender);
                AddCombatPairInternal(pair.Defender, pair.Attacker);
            }
        }

        private bool RemoveInvalidCombatant(IMobileObject mob, CombatPair.CombatPair pair)
        {
            if ((pair.Attacker.Health <= 0 || pair.Defender.Health <= 0)
                || (pair.Attacker.Room != pair.Defender.Room)
                || pair.Attacker.Room.Attributes.Contains(Room.Room.RoomAttribute.Peaceful))
            {
                combatants.TryRemove(mob, out pair);
                return true;
            }

            return false;
        }

        private void ProcessWeapons(IMobileObject attacker, IMobileObject defender)
        {
            foreach (IWeapon weapon in attacker.EquipedWeapon)
            {
                if (_combatRound % weapon.Speed == 0
                    && DetermineIfHit(attacker, defender, weapon.AttackerStat, weapon.DeffenderStat))
                {
                    ProcessWeaponDamage(attacker, defender, weapon);
                }
            }
        }

        private void ProcessWeaponDamage(IMobileObject attacker, IMobileObject defender, IWeapon weapon)
        {
            foreach (IDamage damage in weapon.DamageList)
            {
                DealDamage(attacker, defender, damage);
            }
        }

        public IResult AddCombatPair(IMobileObject attacker, IMobileObject defender)
        {
            CombatPair.CombatPair pair;

            if (combatants.TryGetValue(attacker, out pair))
            {
                return new Result(false, string.Format("You are already attacking {0}.", pair.Defender.KeyWords.FirstOrDefault()));
            }
            else
            {
                pair = new CombatPair.CombatPair();
                pair.Attacker = attacker;
                pair.Defender = defender;
                combatants.TryAdd(attacker, pair);

                //add the defender attacking the attacker
                AddCombatPairInternal(defender, attacker);

                return new Result(true, string.Format("You begin to attack {0}.", defender.KeyWords.FirstOrDefault()));
            }
        }

        /// <summary>
        /// Version that is used for internal that will not try to add the opposite attack order
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        private void AddCombatPairInternal(IMobileObject attacker, IMobileObject defender)
        {
            if (!combatants.Keys.Contains(attacker))
            {
                CombatPair.CombatPair pair = new CombatPair.CombatPair();
                pair.Attacker = attacker;
                pair.Defender = defender;
                combatants.TryAdd(attacker, pair);
            }
        }

        public bool DetermineIfHit(IMobileObject attacker, IMobileObject defender, Stat attackerStat, Stat defenderStat)
        {
            int hitRoll = attacker.CalculateToHitRoll(attackerStat);
            int defenseRoll = defender.CalculateToDodgeRoll(defenderStat);

            if (hitRoll >= defenseRoll)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int DealDamage(IMobileObject attacker, IMobileObject defender, IDamage damage)
        {
            int totalDamage = attacker.CalculateDamage(damage);

            GlobalReference.GlobalValues.Engine.Event.DamageDealtBeforeDefense(attacker, defender, totalDamage);

            int damageReceived = defender.TakeDamage(totalDamage, damage, attacker);

            GlobalReference.GlobalValues.Engine.Event.DamageDealtAfterDefense(attacker, defender, damageReceived);

            return damageReceived;
        }

        public bool AreFighting(IMobileObject mob, IMobileObject mob2)
        {
            CombatPair.CombatPair pair;
            if (combatants.TryGetValue(mob, out pair))
            {
                if (pair.Defender == mob2)
                {
                    return true;
                }
            }

            if (combatants.TryGetValue(mob2, out pair))
            {
                if (pair.Defender == mob)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsInCombat(IMobileObject mob)
        {
            return combatants.ContainsKey(mob);
        }

        public IBaseObject Opponet(MobileObject mobileObject)
        {
            CombatPair.CombatPair pair;
            if (combatants.TryGetValue(mobileObject, out pair))
            {
                return pair.Defender;
            }
            else
            {
                return null;
            }
        }
    }
}
