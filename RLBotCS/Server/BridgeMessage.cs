﻿using rlbot.flat;
using RLBotCS.Conversion;
using Bridge.Models.Command;
using Bridge.Models.Control;
using Bridge.Models.Message;
using Bridge.State;
using PlayerInput = rlbot.flat.PlayerInput;

namespace RLBotCS.Server;

internal record Input(PlayerInput PlayerInput) : IBridgeMessage
{
    public void HandleMessage(BridgeHandler handler)
    {
        if (PlayerInput.ControllerState == null)
        {
            Console.WriteLine("Received input with null ControllerState from index {0}", PlayerInput.PlayerIndex);
            return;
        }

        CarInput carInput = FlatToModel.ToCarInput(PlayerInput.ControllerState.Value);
        ushort? actorId = handler.PlayerMapping.ActorIdFromPlayerIndex(PlayerInput.PlayerIndex);

        if (actorId.HasValue)
        {
            Bridge.Models.Control.PlayerInput playerInput =
                new() { ActorId = actorId.Value, CarInput = carInput };
            handler.SendPlayerInput(playerInput);
        }
        else
            Console.WriteLine("Core got input from unknown player index {0}", PlayerInput.PlayerIndex);
    }
}

internal record SpawnHuman(PlayerConfigurationT PlayerConfig, uint DesiredIndex) : IBridgeMessage
{
    public void HandleMessage(BridgeHandler handler)
    {
        handler.QueueConsoleCommand("ChangeTeam " + PlayerConfig.Team);
        handler.AddPendingSpawn(
            new SpawnTracker
            {
                CommandId = 0,
                SpawnId = PlayerConfig.SpawnId,
                DesiredPlayerIndex = DesiredIndex,
                IsCustomBot = false,
            }
        );
    }
}

internal record SpawnBot(PlayerConfigurationT PlayerConfig, BotSkill Skill, uint DesiredIndex, bool IsCustomBot)
    : IBridgeMessage
{
    public void HandleMessage(BridgeHandler handler)
    {
        PlayerConfigurationT config = PlayerConfig;
        PlayerMetadata? alreadySpawnedPlayer = handler
            .PlayerMapping.GetKnownPlayers()
            .FirstOrDefault(kp => config.SpawnId == kp.SpawnId);
        if (alreadySpawnedPlayer != null)
            // We've already spawned this player, don't duplicate them.
            return;

        config.Loadout ??= new PlayerLoadoutT();
        config.Loadout.LoadoutPaint ??= new LoadoutPaintT();
        Loadout loadout = FlatToModel.ToLoadout(config.Loadout, config.Team);

        ushort commandId = handler.QueueSpawnCommand(config.Name, (int)config.Team, Skill, loadout);

        handler.AddPendingSpawn(
            new SpawnTracker
            {
                CommandId = commandId,
                SpawnId = config.SpawnId,
                DesiredPlayerIndex = DesiredIndex,
                IsCustomBot = IsCustomBot,
            }
        );
    }
}

internal record ConsoleCommand(string Command) : IBridgeMessage
{
    public void HandleMessage(BridgeHandler handler) => handler.QueueConsoleCommand(Command);
}

internal record SpawnMap(MatchSettingsT MatchSettings) : IBridgeMessage
{
    public void HandleMessage(BridgeHandler handler) => handler.SpawnMap(MatchSettings);
}

internal record AddRenders(int ClientId, int RenderId, List<RenderMessageT> RenderItems) : IBridgeMessage
{
    public void HandleMessage(BridgeHandler handler) => handler.AddRenderGroup(ClientId, RenderId, RenderItems);
}

internal record RemoveRenders(int ClientId, int RenderId) : IBridgeMessage
{
    public void HandleMessage(BridgeHandler handler) => handler.RemoveRenderGroup(ClientId, RenderId);
}

internal record RemoveClientRenders(int ClientId) : IBridgeMessage
{
    public void HandleMessage(BridgeHandler handler) => handler.ClearClientRenders(ClientId);
}

internal record Stop : IBridgeMessage
{
    public void HandleMessage(BridgeHandler handler) { }
}

internal interface IBridgeMessage
{
    public void HandleMessage(BridgeHandler handler);
}