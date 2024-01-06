using rlbot.flat;

namespace RLBotCS.Conversion
{
    internal class FlatToCommand
    {
        static public string MakeOpenCommand(MatchSettingsT matchSettings)
        {
            var command = "Open ";

            // Parse game map
            // With RLBot v5, GameMap enum is now ignored
            // You MUST use GameMapUpk instead
            // This is the name of the map file without the extension
            // And can also be used to tell the game to load custom maps
            if (matchSettings.GameMapUpk != "")
            {
                command += matchSettings.GameMapUpk;
            }
            else
            {
                command += "Stadium_P";
                Console.WriteLine("Core got unknown map, defaulting to DFH Stadium");
            }

            // Parse game mode
            command += "?game=";
            switch (matchSettings.GameMode)
            {
                case GameMode.Soccer:
                    command += "TAGame.GameInfo_Soccar_TA";
                    break;
                default:
                    command += "TAGame.GameInfo_Soccar_TA";
                    Console.WriteLine("Core got unknown game mode, defaulting to Soccer");
                    break;
            }

            // Whether to or not to skip the kickoff countdown
            if (!matchSettings.InstantStart)
            {
                command += "?Playtest";
            }

            // Parse mutator settings
            command += "?GameTags=";

            List<string> game_tags = ["PlayerCount8"];

            if (matchSettings.MutatorSettings is MutatorSettingsT mutatorSettings)
            {
                switch (mutatorSettings.MatchLength)
                {
                    case MatchLength.Five_Minutes:
                        break;
                    case MatchLength.Unlimited:
                        game_tags.Add("UnlimitedTime");
                        break;
                    default:
                        Console.WriteLine("Got got unsupported match length option");
                        break;
                }
            }

            command += String.Join(",", game_tags);

            return command;
        }
    }
}