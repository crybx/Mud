﻿using Objects.Command.Interface;
using Objects.Guild;
using Objects.Mob.Interface;
using Objects.Personality.Interface;

namespace Objects.Personality.Personalities.Interface
{
    public interface IGuildMaster : IPersonality
    {
        Guild.Guild.Guilds Guild { get; set; }

        IResult Join(IMobileObject guildMaster, IMobileObject peformer);
        IResult Teach(IMobileObject guildMaster, IMobileObject learner, string parameter);
        IResult Teachable(IMobileObject guildMaster, IMobileObject actor);
    }
}