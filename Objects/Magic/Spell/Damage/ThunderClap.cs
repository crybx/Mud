﻿using Objects.Global;
using Objects.Language;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public class ThunderClap : BaseDamageSpell
    {
        public ThunderClap() : base(nameof(ThunderClap),
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(10).Die,
                            GlobalReference.GlobalValues.DefaultValues.DiceForSpellLevel(10).Sides,
                            DamageType.Thunder)
        {
            PerformerNotification = new TranslationMessage("{performer} test {target}");
            RoomNotification = new TranslationMessage("{performer} test {target}");
            TargetNotification = new TranslationMessage("{performer} test {target}");
        }
    }
}
