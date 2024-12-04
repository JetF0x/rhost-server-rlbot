﻿using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using WmiLight;

namespace RLBotCS.ManagerTools;

internal static class LaunchManager
{
    private const string RocketLeagueExePath = @"C:\Program Files (x86)\Steam\steamapps\common\rocketleague\Binaries\Win64\RocketLeague.exe.unpacked.exe";
    private const string SteamGameId = "252950";
    public const int RlbotSocketsPort = 23234;
    private const int DefaultGamePort = 50000;
    private const int IdealGamePort = 23233;

    private static readonly ILogger Logger = Logging.GetLogger("LaunchManager");

    public static string? GetGameArgs(bool kill)
    {
        Process[] candidates = Process.GetProcessesByName("RocketLeague");

        foreach (var candidate in candidates)
        {
            string args = GetProcessArgs(candidate);
            if (kill)
                candidate.Kill();

            return args;
        }

        return null;
    }

    public static int FindUsableGamePort(int rlbotSocketsPort)
    {
        Process[] candidates = Process.GetProcessesByName("RocketLeague");

        // Search cmd line args for port
        foreach (var candidate in candidates)
        {
            string[] args = GetProcessArgs(candidate).Split(" ");
            foreach (var arg in args)
                if (arg.Contains("RLBot_ControllerURL"))
                {
                    string[] parts = arg.Split(':');
                    var port = parts[^1].TrimEnd('"');
                    return int.Parse(port);
                }
        }

        for (int portToTest = IdealGamePort; portToTest < 65535; portToTest++)
        {
            if (portToTest == rlbotSocketsPort)
                // Skip the port we're using for sockets
                continue;

            // Try booting up a server on the port
            try
            {
                TcpListener listener = new(IPAddress.Any, portToTest);
                listener.Start();
                listener.Stop();
                return portToTest;
            }
            catch (SocketException) { }
        }

        return DefaultGamePort;
    }

    private static string GetProcessArgs(Process process)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return process.StartInfo.Arguments;

        using WmiConnection con = new WmiConnection();
        WmiQuery objects = con.CreateQuery(
            $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}"
        );
        return objects.SingleOrDefault()?["CommandLine"]?.ToString() ?? "";
    }

    private static string[] GetIdealArgs(int gamePort) =>
        [
            "-rlbot",
            $"RLBot_ControllerURL=127.0.0.1:{gamePort}",
            "RLBot_PacketSendRate=240",
            "-nomovie"
        ];

    private static List<string> ParseCommand(string command)
    {
        var parts = new List<string>();
        var regex = new Regex(@"(?<match>[\""].+?[\""]|[^ ]+)");
        var matches = regex.Matches(command);

        foreach (Match match in matches)
        {
            parts.Add(match.Groups["match"].Value.Trim('"'));
        }

        return parts;
    }

    private static Process RunCommandInShell(string command)
    {
        Process process = new();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c {command}";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            process.StartInfo.FileName = "/bin/sh";
            process.StartInfo.Arguments = $"-c \"{command}\"";
        }
        else
            throw new PlatformNotSupportedException(
                "RLBot is not supported on non-Windows/Linux platforms"
            );

        return process;
    }

    private static void LaunchGameViaLegendary()
    {
        Process legendary = RunCommandInShell(
            "legendary launch Sugar -rlbot RLBot_ControllerURL=127.0.0.1:23233 RLBot_PacketSendRate=240 -nomovie"
        );
        legendary.Start();
    }

    public static void LaunchBots(
        Dictionary<string, rlbot.flat.PlayerConfigurationT> processGroups,
        int rlbotSocketsPort
    )
    {
        foreach (var mainPlayer in processGroups.Values)
        {
            if (mainPlayer.RunCommand == "")
                continue;

            Process botProcess = RunCommandInShell(mainPlayer.RunCommand);

            if (mainPlayer.RootDir != "")
                botProcess.StartInfo.WorkingDirectory = mainPlayer.RootDir;

            botProcess.StartInfo.EnvironmentVariables["RLBOT_AGENT_ID"] = mainPlayer.AgentId;
            botProcess.StartInfo.EnvironmentVariables["RLBOT_SERVER_PORT"] =
                rlbotSocketsPort.ToString();

            try
            {
                botProcess.Start();
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to launch bot {mainPlayer.Name}: {e.Message}");
            }
        }
    }

    public static void LaunchScripts(
        List<rlbot.flat.ScriptConfigurationT> scripts,
        int rlbotSocketsPort
    )
    {
        foreach (var script in scripts)
        {
            if (script.RunCommand == "")
                continue;

            Process scriptProcess = RunCommandInShell(script.RunCommand);

            if (script.Location != "")
                scriptProcess.StartInfo.WorkingDirectory = script.Location;

            scriptProcess.StartInfo.EnvironmentVariables["RLBOT_AGENT_ID"] = script.AgentId;
            scriptProcess.StartInfo.EnvironmentVariables["RLBOT_SERVER_PORT"] =
                rlbotSocketsPort.ToString();

            try
            {
                scriptProcess.Start();
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to launch script: {e.Message}");
            }
        }
    }

    public static void LaunchRocketLeague(int gamePort)
    {
        if (!File.Exists(RocketLeagueExePath))
            throw new FileNotFoundException($"Rocket League executable not found at {RocketLeagueExePath}");

        Process rocketLeague = new();
        rocketLeague.StartInfo.FileName = RocketLeagueExePath;
        rocketLeague.StartInfo.Arguments = string.Join(" ", GetIdealArgs(gamePort));
        rocketLeague.StartInfo.UseShellExecute = false;

        Logger.LogInformation(
            $"Starting Rocket League with args: {rocketLeague.StartInfo.Arguments}"
        );

        try
        {
            rocketLeague.Start();
        }
        catch (Exception e)
        {
            Logger.LogError($"Failed to launch Rocket League: {e.Message}");
            throw;
        }
    }

    public static bool IsRocketLeagueRunning() =>
        Process
            .GetProcesses()
            .Any(candidate => candidate.ProcessName.Contains("RocketLeague"));

    public static bool IsRocketLeagueRunningWithArgs()
    {
        Process[] candidates = Process.GetProcesses();

        foreach (var candidate in candidates)
        {
            if (!candidate.ProcessName.Contains("RocketLeague"))
                continue;

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return true;

            var args = GetProcessArgs(candidate);
            if (args.Contains("rlbot"))
                return true;
        }

        return false;
    }

    private static string GetWindowsSteamPath()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException(
                "Getting Windows path on non-Windows platform"
            );

        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
        if (key?.GetValue("SteamExe")?.ToString() is { } value)
            return value;

        throw new FileNotFoundException(
            "Could not find registry entry for SteamExe. Is Steam installed?"
        );
    }
}
