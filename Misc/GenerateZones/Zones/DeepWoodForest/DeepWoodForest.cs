﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Zone.Interface;
using Objects.Zone;
using System.Reflection;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Item;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using Objects.Personality.Personalities.Interface;
using static Objects.Global.Direction.Directions;
using Objects.Item.Items.Interface;
using Objects.Material.Materials;
using static Objects.Item.Items.Equipment;
using Shared.Sound.Interface;
using Shared.Sound;
using Objects.Mob;
using Objects.Personality.Personalities;
using Objects.Item.Items;

namespace GenerateZones.Zones.DeepWoodForest
{
    public class DeepWoodForest : IZoneCode
    {
        Zone zone = new Zone();
        int roomId = 1;
        int itemId = 1;
        int npcId = 1;
        public IZone Generate()
        {
            zone.Id = 8;
            zone.InGameDaysTillReset = 1;
            zone.Name = nameof(DeepWoodForest);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    IRoom room = (IRoom)method.Invoke(this, null);
                    room.Zone = zone.Id;
                    ZoneHelper.AddRoom(zone, room);
                }
            }

            ConnectRooms();
            AddSounds();

            return zone;
        }

        private void AddSounds()
        {
            ISound sound = new Sound();
            sound.Loop = true;
            sound.SoundName = string.Format("{0}\\{1}", zone.Name, "Forest.mp3");
            foreach (IRoom room in zone.Rooms.Values)
            {
                room.Sounds.Add(sound);
            }

            sound = new Sound();
            sound.Loop = true;
            sound.SoundName = string.Format("{0}\\{1}", zone.Name, "Stream.mp3");
            zone.Rooms[35].Sounds.Add(sound);
            zone.Rooms[27].Sounds.Add(sound);
            zone.Rooms[28].Sounds.Add(sound);
            zone.Rooms[29].Sounds.Add(sound);
            zone.Rooms[21].Sounds.Add(sound);
            zone.Rooms[13].Sounds.Add(sound);
            zone.Rooms[5].Sounds.Add(sound);
            zone.Rooms[45].Sounds.Add(sound);
            zone.Rooms[53].Sounds.Add(sound);
            zone.Rooms[61].Sounds.Add(sound);
            zone.Rooms[69].Sounds.Add(sound);
            zone.Rooms[77].Sounds.Add(sound);
            zone.Rooms[78].Sounds.Add(sound);
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The forest is thick and almost seems alive.  It slowly over the years has been encroaching on the meadows.";
            room.LongDescription = "You stand at the edge of the forest.  To the west is green meadows and to the east is a thick forest.  The forest runs north and south as far as the eye can see.";
            room.ShortDescription = "Edge of the forest";

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The path you are standing on is warn enough to allow you to find your way can tell there is not much traffic through the area.";
            room.LongDescription = "To the west you can see the opening of the forest down the path.  To the east the forest continues into the distance.";
            room.ShortDescription = "Slightly in the forest";

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tree was once a mighty oak that towered proudly in the forest providing shelter to many animals.  When it was standing it was about 50 feet wide, counting the rings you can tell it was 546 years old when it fell.";
            room.LongDescription = "I large tree has fallen over the path here.  The section that laid over the path has been cut away.  At one time you can image that there was a large collection of fire wood but as people have come this way gotten some with each trip.  There is hardly any wood left.";
            room.ShortDescription = "Forest path";

            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "There are a several pairs of trees here that bend slightly into the path and join together above you.  Their thick canopies forms a bit of an arch that serves both as a natural shelter and a symbolic arch, almost as if to say you are entering a different part of the forest here.";
            room.LongDescription = "The path continues through a section of thick evergreen trees.";
            room.ShortDescription = "Forest path";

            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Looking to the north up stream you can not see anything of interest before it turns to the east.  Looking down stream to the south you the stream flattens out into a wide area of shallow rocks.  There are several animals tracks that you recognize including deer and badger as well as a few larger ones you do not recognize.";
            room.LongDescription = "The forest is washed out here by a small stream that runs north and south.";
            room.ShortDescription = "Forest path";

            return room;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The meadow is waist hight with wild grasses.  Flowers of purple and yellow jot the meadow from the north to the south.";
            room.LongDescription = "Here the forest gives way to a small meadow.  A small pond can be seen to the south.";
            room.ShortDescription = "Edge of the Meadow";

            return room;
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The meadow is waist high with wild grasses.  Flowers of purple and yellow jot the meadow from the north to the south.";
            room.LongDescription = "A small stream flows through the meadow to the south to the pond.";
            room.ShortDescription = "Edge of the Meadow";

            return room;
        }

        private IRoom GenerateRoom8()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Trees grow up on either of the side creating a quiet area the blocks out most of the light from above.  Small mushrooms grow on the cool dark forest floor in small groups creating a soft rich layer of dirt perfect for growing plants.";
            room.LongDescription = "A deer path runs to the east and west leading off into the distance.";
            room.ShortDescription = "Forest path";

            return room;
        }

        private IRoom GenerateRoom9()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Everywhere you look is green.  The only place that you can see brown bark is when you look up and see tree branches that are not covered in vines yet.";
            room.LongDescription = "The forest is filled with lush green undergrowth.  Vines grow up the sides of trees creating a soft green everywhere you look.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }
        private IRoom GenerateRoom10()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The dead tree stands about 60 feet tall and its branches stretch out as if trying to reach sun light to once again grow.";
            room.LongDescription = "A single dead tree stands in the middle of a clearing.  Other trees have grown around it but almost as if to give respect to the dead tree none have grown into its space.";
            room.ShortDescription = "Deep Wood Forest";

            room.AddMobileObjectToRoom(Squirrel());

            return room;
        }

        private IRoom GenerateRoom11()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The red ferns are growing in a circular shape extending 15 feet.  Why they stop after that is a good guess but it does make for a nice breaking in scenery.";
            room.LongDescription = "A section of red ferns grown on the forest floor providing a break in the sea of green with brown buildings.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom12()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Movement is difficult and every where you look looks the same.  The undergrowth is so thick you could hide a baby deer 3 feet in front of you and not know it.";
            room.LongDescription = "The forest has become thick with undergrowth.  Small bushes and new trees fight for what little light comes down to the forest floor.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom13()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The bottom water fall is about 1 foot tall and the upper water fall is about 1.5 feet tall.  The grass growing on either of side of the bank is lush green and soft.  You can see where an animal has bedded down by the bank of the stream but it is gone for the moment.";
            room.LongDescription = "The stream flows over two small water falls and makes soft gurgling sounds.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom14()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Sounds of a small stream can be heard from the west.  Small birds can be heard in the forest to the north and the west and a beautiful meadow extends to the south and the east.";
            room.LongDescription = "The forest extends a bit into the meadow creating a small blind that could be advantageous when hunting or disadvantageous when hunted.";
            room.ShortDescription = "Edge of the Meadow";

            return room;
        }


        private IRoom GenerateRoom15()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The east wall is has completely fallen in and the west wall is leaning haphazardly into the main room.  A strong gust of wind might blow it the rest of the way in.";
            room.LongDescription = "What looks like a hunters hut has been burned down.  The building was burned a while ago and it appears to have been picked over already.";
            room.ShortDescription = "Edge of the Meadow";

            room.AddMobileObjectToRoom(Mouse());

            return room;
        }

        private IRoom GenerateRoom16()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Pine trees descend into the white mist that covers the forest to the east.  One by one each tree since lower and lower until just the tips of trees are showing and eventually even they are gone from sight.";
            room.LongDescription = "From where you stand on the top of a hill you can see to the east the forest stretch out to the east.  Pine trees descend into a white mist below.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom17()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "A soft layer of grass has grown up on the forest floor which provides bedding for several woodland animals.";
            room.LongDescription = "The forest is intermingled with evergreens and deciduous trees, each one trying to be the dominate type of tree here.";
            room.ShortDescription = "Deep Wood Forest";

            room.AddMobileObjectToRoom(Fox());

            return room;
        }

        private IRoom GenerateRoom18()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Standing in the forest you can hear the peaceful silence and then eventually you hear a squirrel jump through from one tree limb to another.  An owl gives a mighty hoot and an unidentified rustling of a limb.";
            room.LongDescription = "Pine trees dominate the landscape here.  Towering 100 feet into the air all you can see above you is a sea of green.  Brown pine needles cover the ground creating a picture that looks like it came out of a painting.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom19()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The deer trail winds through the forest.  It heads north around and over a fallen log and over some overgrowth.  To the south it quickly disappears around a bend.";
            room.LongDescription = "You have stumbled upon a deer trail that runs north and south.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom20()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Trees have grown together forming an almost implementable wall to the north.  The size of the trees are hard to tell as over the years individual trees have grown up and intertangled with each other almost like braided rope.";
            room.LongDescription = "The forest to the north becomes very dense with overgrowth.  To the south it opens up more and a stream can be heard to the east.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom21()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The damn was built in a slow moving part of the stream.  Here the stream had widened out to about 10 feet.  Thanks to the beaver the north side of the stream is now about 3.5 feet deep, enough to take a swim.";
            room.LongDescription = "A beaver has built a damn here making the north side of the stream higher than the south side.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom22()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "A fog rolls in from the west making it hard to see what is in front of you.  Occasionally an own makes a noise but other than that the forest is quiet.";
            room.LongDescription = "Pine trees cover the landscape as far as the eye can see.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom23()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "To the east the signs of a forest fire are undeniable.  The forest has begun to creep back in from the west and the circle of life continues.";
            room.LongDescription = "Burned trees stumps stretch to the east as far as the eye can see.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom24()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Lifeless charred stumps give the forest an eerie lifeless feeling.  The soft sound of ashes breaking can be heard underfoot.  Yet there are signs the forest is recovering.  Small saplings are beginning to sprout up and reach for the sky.";
            room.LongDescription = "Burned trees stumps stretch in all directions.";
            room.ShortDescription = "Deep Wood Forest";

            room.AddMobileObjectToRoom(Owl());

            return room;
        }

        private IRoom GenerateRoom25()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Oak leaves blanket the forest floor as old oak trees tower hundreds of feet in the air.  A small set of fairy mushrooms form a circle about a foot in diameter by a large oak.";
            room.LongDescription = "Giant oaks tower high above your head.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom26()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Several small pine trees are trying to catch sunlight so they can grow up to be mighty trees.";
            room.LongDescription = "A mixture of oak and redwoods grow high above the few pine trees trying to get sunlight amongst all the shade.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom27()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The stream for some reason makes a sharp turn here to the east.  For a moment you ponder why it would decide to turn like that but decide its questions like that are best left the philosophers.";
            room.LongDescription = "A stream makes a sharp turn flowing from the north to the east.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom28()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The rocks range anywhere from 6 feet in diameter to 7 inches.  Each has been worn smooth from the passage of time in the stream.  Moss covers each rock giving each rock a slightly softer look.";
            room.LongDescription = "Large moss covered rocks lie in the stream making walking in the stream bed a tricky task.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom29()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The waterfall is 10 feet tall and is split into 3 different channels.  The largest being the middle.  The two sides do not carry as much water as the main with the left carrying the least.";
            room.LongDescription = "Water from the west stream pours down a 10 foot ravine and then heads to the south.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom30()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The tree has seared marks where it was struck by lightning and has split the tree down the middle.  The power to split such a huge tree down the middle is staggering to the mind.";
            room.LongDescription = "A seared fallen tree lays across your path.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom31()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Pine trees grow up twisted and crooked along a ridge looking down over a burned out forest.  Spots of green among the sea of blank show where new trees are starting to grow on the forest below.";
            room.LongDescription = "Gnarly pine trees grow along a natural ridge to the east.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom32()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "You stand in the middle of a burned forest yet before you stands an untouched oak tree.  Not a single leaf is singed and grass extends 5 feet around the base then stops giving way to burned ashes.";
            room.LongDescription = "Before an unburned tree.";
            room.ShortDescription = "Deep Wood Forest";

            room.AddMobileObjectToRoom(Crow());

            return room;
        }

        private IRoom GenerateRoom33()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "You stand at a vista looking out at the forest to the north.  The lush green forest looks like an emerald city that stretches to the horizon.";
            room.LongDescription = "An vista to the forest.";
            room.ShortDescription = "Deep Wood Forest";

            room.AddMobileObjectToRoom(Squirrel());

            return room;
        }

        private IRoom GenerateRoom34()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Hardly any light can enter from above making even the day look almost like night.  This part of the forest is called the black forest.";
            room.LongDescription = "Dark green pine trees grow so close together that the block out any light from above";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }


        private IRoom GenerateRoom35()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "You stand by a small pond that has a tributary to the north and a distributary to the south.  A few small lily pad grow on the west side of the blue green water.";
            room.LongDescription = "A small pond.";
            room.ShortDescription = "Deep Wood Forest";
            room.AddMobileObjectToRoom(Fish());

            return room;
        }


        private IRoom GenerateRoom36()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "You have come across what is left of Fort Woodbrook.  Now it has been abandoned and the forest has grown up around it.";
            room.LongDescription = "The once mighty Fort Woodbrook stands to the north.";
            room.ShortDescription = "Fort Woodbrook";

            return room;
        }


        private IRoom GenerateRoom37()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The trees here reach up the sky but have become so large that they have bent back down to the ground under their own weight.  Moss grows down from the trees as if trying to reach the ground.";
            room.LongDescription = "Tangled trees reach up to the sky before bending back to the ground.";
            room.ShortDescription = "Deep Wood Forest";
            return room;
        }

        private IRoom GenerateRoom38()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The wagon trail head to the west and east as far as the eye can see.  The cooking fire is long since out and impossible to tell how long.";
            room.LongDescription = "Several sets of wagon trails can be seen running east and west.  Ashes of a cooking fire sit in a dug out fire pit.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom39()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Deep groves from many wagon trips still scar the forest floor.  It is hard to tell if the trail is still used or not though.";
            room.LongDescription = "An old wagon trail runs east and west through the forest here.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom40()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The wagon trail heading to the wast is the main feature here.  To the east it gets lost in as it transitioned from softer soil to a harder stone ground.";
            room.LongDescription = "A wagon trail can be seen heading to the west but fades into the forest to the east.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom41()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "This does remind you of something out a painting.  The painting hanging over reception desk at the hospital.  Other than the fallen log it does appear to be the same place.";
            room.LongDescription = "The forest is light and airy with not much over growth, almost as if something out of a painting.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom42()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Something that smells bad can be smelt in the air.";
            room.LongDescription = "The forest is sparse as if trying to get away from what ever is creating that awful smell.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom43()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Something near by has died and is rotting.";
            room.LongDescription = "The forest is green and lush while the foul stench of death can be smelt near by.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom44()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Something that smells bad is near by.";
            room.LongDescription = "A beautiful stretch of flowers grows on the forest floor.  You would consider staying except for the foul stench you smell.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom45()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The shallow water of the stream run slowly down stream.  Small tadpoles can be seen swimming to and fro in the water.";
            room.LongDescription = "A small stream runs north to the south here.  The water is just deep enough to get your feet wet.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom46()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Tinny flowers make the grass looks like a polka dot quilt of colors red, blue, yellow and white.";
            room.LongDescription = "Fields of grass stretch out before you to the North, East and South while a wall of trees stands to the West.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom47()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The yellow flowers in the meadow make the hills look like waves of golden silk.  Its almost enough to make you want to lay down and sleep and dream of being an emperor.";
            room.LongDescription = "The meadow has some small hills here.  Enough to make the ground look like a rippling banner.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom48()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Tall evergreens stand on either side of the meadow as if acting as color guards protecting the meadow.";
            room.LongDescription = "A small part of the meadow has made its way into this part of the forest dividing the trees between north and south.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom49()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Some type of large animal with claws appears to be marking the trees around here.";
            room.LongDescription = "The trees around here have large gashes on the trunks.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom50()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Some of the bushes have tufts of black hair on them.";
            room.LongDescription = "While all the vegetation looks healthy several of the small bushes are bent and broken.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom51()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The hole is approximately a foot deep and three feet across.  Several roots are sticking out of the edges of the hole.";
            room.LongDescription = "The forest is a full of beautiful green moss growing on the forest floor.  In the center is large hole where something has been digging.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom52()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Each step you take you feet sink into the soft wet ground.  Moss grows up on all the sides of the trees making it impossible to use the old adage \"Moss grows on the north side of the tree.\"";
            room.LongDescription = "The ground begins to get more boggy to the east and dryer to the west.  In spots to the east you can see standing water.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom53()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The stream slowly flows to the south but not before extending far to the west in what looks like a bog.";
            room.LongDescription = "The stream widens out so much it almost stops.  To the west it seems to flow back and up hill as it always seeks new places to go.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom54()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Cat tails grow up on this side of the pond obstructing the view to the rest of the pond.";
            room.LongDescription = "A small pond sits nestled in this corner of the meadow.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom55()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The pond appears to be about 10 feet deep and about 45 feet around.  The west side is covered in cat tails while the east side remains relatively plant free.";
            room.LongDescription = "A small pond sits nestled in this corner of the meadow.";
            room.ShortDescription = "Deep Wood Forest";

            room.AddMobileObjectToRoom(Chipmunk());

            return room;
        }

        private IRoom GenerateRoom56()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Pine trees grow to heights of 65 feet and block out most of the light from above leaving the forest floor cool, dark and covered mostly in pine needles and pine cones.";
            room.LongDescription = "Forest trees grow to incredible heights as they try to out grow each other to get the most sunlight.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom57()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Beyond the scat you almost stepped in... never mind you did, the grass grows to an almost emerald color.  Signs of animals eating the grass can be seen in spots here and there.";
            room.LongDescription = "Some scat lies scattered around the forest floor here.";
            room.ShortDescription = "Deep Wood Forest";

            room.AddMobileObjectToRoom(Owl());

            return room;
        }

        private IRoom GenerateRoom58()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The entrance to the cave is dark and you can only see a few feet in.";
            room.LongDescription = "A large mound stands about 5 feet tall and has a rather large opening on the north side.  Several sets of prints can be seen going in and out of the cave.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom59()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Upon further inspection some of the trees that are not broken down instead have broken limbs.  Perhaps something big was trying to climb in them.";
            room.LongDescription = "Several trees have broken down.  Some look small enough that wind could have done it but others look to be two big.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom60()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The white bark gives a interesting break of the green grass and the green leaves.";
            room.LongDescription = "Aspen trees with their white bark dominate the forest here and provide a bright contrast to the brown bark of other trees.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom61()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The white bark gives a interesting break of the green grass and the green leaves.";
            room.LongDescription = "Aspen trees with their white bark dominate the forest here and provide a bright contrast to the brown bark of other trees.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom62()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Small saplings grow ever deeper into the meadow encroaching inch by inch.";
            room.LongDescription = "The forest has yielded this spot of land to the meadow before you.  Yet it seems to be taking it back as small saplings encroach ever further into the meadow lands.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom63()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Meadow grasses come to you waist and tickle at your skin as you walk.  A small pond can be seen to the north and a trail can be seen to the south.";
            room.LongDescription = "Almost hidden at first a small trail leads into the forest to the south.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom64()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "With the large amounts of rocks and low soil levels it is amazing the trees grow so well here.";
            room.LongDescription = "Red moss speckles large rocks giving the forest a strange look.  Red moss contrast sharply with the gray rocks and the white trees.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom65()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The path has several prints but they are hard to distinguish from one another.";
            room.LongDescription = "A worn path leads to the east through the trees and brush.";
            room.ShortDescription = "Deep Wood Forest";

            room.AddMobileObjectToRoom(Bear());

            return room;
        }

        private IRoom GenerateRoom66()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The path north leads towards a hill.";
            room.LongDescription = "A worn path leads to the north and west east through the trees and brush.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom67()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Several trees here have tufts of black hair.";
            room.LongDescription = "The forest trees are green and healthy.  Each one growing tall and strong.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom68()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "It has dense trees growing in all directions blocking out most of the forest.  A small spring bubbles up in the center and flows to the east.  A large tree stump has been move close the spring as if to allow some one to sit and watch the stream wash their troubles away.";
            room.LongDescription = "This part of the forest is very tranquil with dense trees blocking most of the forest.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom69()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The stream has carved out a bit of a gorge here with walls that rise about ten feet on either side.  Damp moss lines the walls making climbing the walls tricky but not impossible.";
            room.LongDescription = "A small water fall drop ten feet from the west where it splatters noisily onto rocks.";
            room.ShortDescription = "Deep Wood Forest";

            room.AddMobileObjectToRoom(Fish());

            return room;
        }

        private IRoom GenerateRoom70()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "At this point all the trees have begun to blend together making navigation difficult.  At least you can follow your bread crumb trail back... you have been dropping bread crumbs right?";
            room.LongDescription = "Tall trees loom high above you in all directions.";
            room.ShortDescription = "Deep Wood Forest";

            room.AddMobileObjectToRoom(Fox());

            return room;
        }

        private IRoom GenerateRoom71()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Some type of animal is using this area as a path way through the forest.";
            room.LongDescription = "A game trail runs north and south through the forest.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom72()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "There are several animal carcases that have been hung from tree branches here.  They appear to have been dead when they were hung but it is strange to find this in the forest as there does not appear to be anyone to have put them there.";
            room.LongDescription = "Several small animals have been hung from branches.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom73()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "It is impossible to tell where the water goes.  The hole goes down several feet and then makes a sharp turn under a rock.";
            room.LongDescription = "A small spring bubbles up and flows down a rock only to go down a small in the ground.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom74()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Several sets of older trees have had their tops removed.  Their cut off height ascends in an arc from almost at the ground where you stand to a height of over 50 feet.";
            room.LongDescription = "The forest is a mixture of normal young trees and older ones that have had their tops removed.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom75()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Almost missed at first the depressions range from ten to fifteen feet in length and are six to nine inches in depth.";
            room.LongDescription = "The forest has several large depressions here of unusual size and shape.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom76()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "Almost completely covered in vines the signs of a long ago battle sit here whose details are lost with time.";
            room.LongDescription = "The forest is trying to bury a large hammer and sword fused together.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IItem SwordHammer()
        {
            IItem item = new Item();
            item.Attributes.Add(Item.ItemAttribute.NoGet);
            item.Id = itemId++;
            item.KeyWords.Add("sword");
            item.KeyWords.Add("hammer");

            item.ExamineDescription = "The sword appears to be made of an unknown type of metal that cut into the iron hammer but became lodged.  The war hammer runes can still be seen quite clearly even with the large amount of weathering that has occurred.  The sword still is as sharp as ever and looks to be untouched by time.";
            item.LongDescription = "Each weapon is colossal and could wielded only be something equally as big.  The hammer is impressive at over 30 feet long had to be wielded buy something equally as big while the sword is longer yet.  The chaos that these two fighters must have caused makes you glad they are long gone.";
            item.SentenceDescription = "sword fused with a hammer";
            item.ShortDescription = "A sword fused with a hammer lies partly covered in vines.";

            return item;
        }

        private IRoom GenerateRoom77()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The water is cool and clear with small colorful fish swimming through it.";
            room.LongDescription = "A stream runs from the north and to the east and through the forest trees.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom78()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "A large tree has fallen into the stream creating a small \"ramp\" that allows animals to go down and drink water.";
            room.LongDescription = "The water make another turn to the south again before flowing out of sight.";
            room.ShortDescription = "Deep Wood Forest";

            return room;
        }

        private IRoom GenerateRoom79()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "The trees bend inward towards the path almost trying to hide it and definably making it hard to traverse.";
            room.LongDescription = "A small path runs east and west.";
            room.ShortDescription = "Deep Wood Forest";

            room.AddMobileObjectToRoom(Crow());

            return room;
        }

        private IRoom GenerateRoom80()
        {
            IRoom room = ZoneRoom(5);
            room.ExamineDescription = "There are a large amount of tracks leading into and out of the cave.";
            room.LongDescription = "A cooking fire burns next to an entrance of a cave.";
            room.ShortDescription = "Deep Wood Forest";

            INonPlayerCharacter npc = KolboldGuard();
            room.AddMobileObjectToRoom(npc);

            npc = KolboldGuard();
            room.AddMobileObjectToRoom(npc);

            return room;
        }

        private INonPlayerCharacter KolboldGuard()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 7;

            npc.KeyWords.Add("Kobold");
            npc.KeyWords.Add("Guard");
            npc.CorpseLongDescription = "He died while defending his den and did not run like most kobolds would.";
            npc.LongDescription = "The kobold has a few scars where it has been through tough training.";
            npc.SentenceDescription = "kobold guard";
            npc.ShortDescription = "The guard snarls at you trying to make you leave but it does not advance from the entrance.";
            npc.ExamineDescription = "The guard stands about 3 feet tall and is lizard like in his features.";

            IGuard guardPersonality = new Guard(Direction.South);
            npc.Personalities.Add(guardPersonality);

            IShield shield = new Shield();
            shield.Id = itemId++;
            shield.Level = 7;
            shield.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(shield.Level);
            shield.KeyWords.Add("leather");
            shield.KeyWords.Add("shield");
            shield.Material = new Leather();
            shield.ShortDescription = "A well made leather shields.";
            shield.LongDescription = "The shield is made of a wooden ring with leather hides stretched across.";
            shield.SentenceDescription = "a leather shield";
            shield.ExamineDescription = "The shield is surprisingly light for it's size but also feels less durable than originally anticipated.";
            npc.AddEquipment(shield);

            IWeapon weapon = new Weapon();
            weapon.Id = itemId++;
            weapon.Level = 7;
            weapon.KeyWords.Add("spear");
            weapon.LongDescription = "A spear crafted from animal bone and a stick.";
            weapon.SentenceDescription = "a spear";
            weapon.ShortDescription = "A hastily made weapon made of readily available materials.";
            weapon.ExamineDescription = "The point of this spear is fashioned from animal and lashed to a large stick.";
            npc.AddEquipment(weapon);

            IArmor armor = Armor();
            armor.ItemPosition = AvalableItemPosition.Arms;
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("bracer");
            armor.Material = new Leather();
            armor.ShortDescription = "A well made pair leather bracers.";
            armor.LongDescription = "The bracers extend up the wearers arm a good ways giving the user extra protection.";
            armor.SentenceDescription = "a pair of leather bracers";
            armor.ExamineDescription = "The bracers are fairly plain but are well made.";
            npc.AddEquipment(armor);

            armor = Armor();
            armor.ItemPosition = AvalableItemPosition.Body;
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("vest");
            armor.Material = new Leather();
            armor.ShortDescription = "A leather vest with several pockets.";
            armor.LongDescription = "The leather vest looks to be as utilitarian as protectant.";
            armor.SentenceDescription = "a leather vest";
            armor.ExamineDescription = "The vest is a light brown natural leather color. It has four large pockets on the front and the top left one is torn slightly.";
            npc.AddEquipment(armor);
            return npc;
        }

        private IArmor Armor()
        {
            IArmor armor = new Armor();
            armor.Id = itemId++;
            armor.Level = 7;
            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(armor.Level);
            return armor;
        }

        private IRoom ZoneRoom(int movementCost)
        {
            IRoom room = new Room();
            room.Id = roomId++;
            room.MovementCost = movementCost;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);
            return room;
        }
        #endregion Rooms

        #region Animals
        private INonPlayerCharacter Owl()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 7;
            npc.Personalities.Add(new Wanderer());

            npc.KeyWords.Add("Owl");
            npc.LongDescription = "The owl is brown in color with some black feathers for camouflage.";
            npc.SentenceDescription = "an owl";
            npc.ShortDescription = "An owl looks at you as you walk through the forest.";
            npc.ExamineDescription = "Its hard to get a good view of the owl as it won't let you get near it.";

            return npc;
        }

        private INonPlayerCharacter Mouse()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 7;
            npc.Personalities.Add(new Wanderer());

            npc.KeyWords.Add("Mouse");
            npc.LongDescription = "The mouse is {color} in color little pink nose.";
            npc.SentenceDescription = "a mouse";
            npc.ShortDescription = "The mouse skitters away as you get close.";
            npc.ExamineDescription = "You would need to catch it first before you could examine it.";

            npc.FlavorOptions.Add("color", new List<string>() { "brown", "black", "white" });

            return npc;
        }

        private INonPlayerCharacter Squirrel()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 7;
            npc.Personalities.Add(new Wanderer());

            npc.KeyWords.Add("Squirrel");
            npc.LongDescription = "The Squirrel is runs to and fro looking for nuts.";
            npc.SentenceDescription = "a squirrel";
            npc.ShortDescription = "A squirrel looks at you for a moment before choosing to ignore you.";
            npc.ExamineDescription = "It has a long fluffy tail and gray fur on its back.  The underside is a pale white like snow.";

            return npc;
        }

        private INonPlayerCharacter Crow()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 7;
            npc.Personalities.Add(new Wanderer());

            npc.KeyWords.Add("Crow");
            npc.LongDescription = "As you and the crow stare at each other it starts crowing loudly as trying to win a staring contest by making you look away.";
            npc.SentenceDescription = "a crow";
            npc.ShortDescription = "A black as night crow calls out a warning as you approach.";
            npc.ExamineDescription = "It seems to have been born of the night with black feathers, feet and beak. The small black beady eyes are the only thing to reflect any light.";

            return npc;
        }

        private INonPlayerCharacter Fox()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 7;
            npc.Personalities.Add(new Wanderer());

            npc.KeyWords.Add("Fox");
            npc.LongDescription = "A red fox scurries along trying to catch mice.";
            npc.SentenceDescription = "a fox";
            npc.ShortDescription = "A small fox looks at you.";
            npc.ExamineDescription = "The fur on the fox looks soft but you will not be able to get close enough to it while it has a choice.";

            return npc;
        }

        private INonPlayerCharacter Bear()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 10;
            npc.Personalities.Add(new Wanderer());

            npc.KeyWords.Add("Bear");
            npc.LongDescription = "The black bear looks to be thirty two inches long weigh over 200 lbs.";
            npc.SentenceDescription = "a fox";
            npc.ShortDescription = "A small fox looks at you.";
            npc.ExamineDescription = "It looks like a giant black teddy bear but you know this is one teddy you don't want to get a bear hug from.";

            return npc;
        }

        private INonPlayerCharacter Chipmunk()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 7;
            npc.Personalities.Add(new Wanderer());

            npc.KeyWords.Add("Chipmunk");
            npc.LongDescription = "A little chipmunk runs underneath your feet with its fat cheeks stuffed full of food.";
            npc.SentenceDescription = "a chipmunk";
            npc.ShortDescription = "A chipmunk with two full cheeks stands here.";
            npc.ExamineDescription = "For a minute it cocks it head to the side and stares at you as if trying to decide if it could fit you in its cheek.";

            return npc;
        }

        private INonPlayerCharacter Fish()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 7;

            npc.KeyWords.Add("Fish");
            npc.LongDescription = "It is a silver fish with a blue spot behind its gill.";
            npc.SentenceDescription = "a chipmunk";
            npc.ShortDescription = "A small fish swims back and forth in the stream";
            npc.ExamineDescription = "The top of the fish is a bit green in color on top and a bit of orange on its belly.";

            IWanderer wanderer = new Wanderer();
            wanderer.NavigableRooms.Add(new RoomId(9, 35));
            wanderer.NavigableRooms.Add(new RoomId(9, 27));
            wanderer.NavigableRooms.Add(new RoomId(9, 28));
            wanderer.NavigableRooms.Add(new RoomId(9, 29));
            wanderer.NavigableRooms.Add(new RoomId(9, 21));
            wanderer.NavigableRooms.Add(new RoomId(9, 13));
            wanderer.NavigableRooms.Add(new RoomId(9, 5));
            wanderer.NavigableRooms.Add(new RoomId(9, 45));
            wanderer.NavigableRooms.Add(new RoomId(9, 53));
            wanderer.NavigableRooms.Add(new RoomId(9, 61));
            wanderer.NavigableRooms.Add(new RoomId(9, 69));
            wanderer.NavigableRooms.Add(new RoomId(9, 77));
            wanderer.NavigableRooms.Add(new RoomId(9, 78));

            npc.Personalities.Add(wanderer);

            return npc;
        }
        #endregion Animals

        private void ConnectRooms()
        {
            zone.RecursivelySetZone();
            ZoneHelper.ConnectZone(zone.Rooms[1], Direction.West, 9, 80);
            ZoneHelper.ConnectZone(zone.Rooms[36], Direction.North, 10, 1);
            ZoneHelper.ConnectZone(zone.Rooms[80], Direction.South, 12, 1);

            for (int h = 0; h < 5; h++)
            {
                for (int i = 1; i < 9; i++)
                {
                    int baseRoom = i + h * 8;
                    if (i != 8)
                    {
                        ZoneHelper.ConnectRoom(zone.Rooms[baseRoom], Direction.East, zone.Rooms[baseRoom + 1]);
                    }
                    if (h != 4)
                    {
                        ZoneHelper.ConnectRoom(zone.Rooms[baseRoom], Direction.North, zone.Rooms[baseRoom + 8]);
                    }
                }
            }

            for (int h = 0; h < 5; h++)
            {
                for (int i = 1; i < 9; i++)
                {
                    int baseRoom = 72 + i - h * 8;
                    if (i != 8)
                    {
                        ZoneHelper.ConnectRoom(zone.Rooms[baseRoom], Direction.East, zone.Rooms[baseRoom + 1]);
                    }
                    if (h != 4)
                    {
                        ZoneHelper.ConnectRoom(zone.Rooms[baseRoom], Direction.North, zone.Rooms[baseRoom - 8]);
                    }
                    else
                    {
                        ZoneHelper.ConnectRoom(zone.Rooms[baseRoom], Direction.North, zone.Rooms[i]);
                    }
                }
            }
        }
    }
}

