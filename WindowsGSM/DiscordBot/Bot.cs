﻿using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace WindowsGSM.DiscordBot
{
	class Bot
	{
		private DiscordSocketClient _client;
		private string _donorType;

		public Bot()
		{
			Configs.CreateConfigs();
		}

		public async Task<bool> Start(string token)
		{
			_client = new DiscordSocketClient();
			_client.Ready += On_Bot_Ready;

			try
			{
				await _client.LoginAsync(TokenType.Bot, token);
				await _client.StartAsync();
			}
			catch
			{
				return false;
			}

			// Listen Commands
			new Commands(_client);

			return true;
		}

		private async Task On_Bot_Ready()
		{
			try
			{
				Stream stream = System.Windows.Application.GetResourceStream(new Uri($"pack://application:,,,/Images/WindowsGSM{(string.IsNullOrWhiteSpace(_donorType) ? "" : $"-{_donorType}")}.png")).Stream;
				await _client.CurrentUser.ModifyAsync(x =>
				{
					x.Username = "WindowsGSM";
					x.Avatar = new Image(stream);
				});
			}
			catch
			{
				// ignore
			}
		}

		public void SetDonorType(string donorType)
		{
			_donorType = donorType;
		}

		public async Task Stop()
		{
			if (_client != null)
			{
				await _client.StopAsync();
			}
		}

		public string GetInviteLink()
		{
			return (_client == null || _client.CurrentUser == null) ? "" : $"https://discordapp.com/api/oauth2/authorize?client_id={_client.CurrentUser.Id}&permissions=67497024&scope=bot";
		}
	}
}
