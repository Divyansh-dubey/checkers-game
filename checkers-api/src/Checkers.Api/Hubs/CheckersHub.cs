using Checkers.Api.Handlers;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Checkers.Api.Hubs
{
    public class CheckersHub : Hub
    {
        private readonly IMediator _mediator;
        private const string LOBBY = "Lobby";

        public CheckersHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task JoinLobby(JoinLobbyRequest request)
        {
            var result = await _mediator.Send(request);
            await Groups.AddToGroupAsync(Context.ConnectionId, LOBBY);
            await Clients.Client(Context.ConnectionId).SendAsync("PlayerJoinedLobby", result.Games);
        }

        public async Task LeaveLobby()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, LOBBY);
        }

        public async Task CreateGame(CreateGameRequest request)
        {
            var result = await _mediator.Send(request);
            string groupName = result.Id.ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("GameSuccessfulyCreated", result);
            var availableGamesQuery = new AvailableGamesQuery();
            var avGamesResult = await _mediator.Send(availableGamesQuery);
            await Clients.Group(LOBBY).SendAsync("ListRefreshed", avGamesResult.AvailableGames);
        }

        public async Task JoinGame(JoinGameRequest request)
        {
            var result = await _mediator.Send(request);
            string groupName = result.GameId.ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("PlayerJoinedGame", result);
            var availableGamesQuery = new AvailableGamesQuery();
            var avGamesResult = await _mediator.Send(availableGamesQuery);
            await Clients.Group(LOBBY).SendAsync("ListRefreshed", avGamesResult.AvailableGames);
        }

        public async Task RefreshBoard(RefreshBoardRequest request)
        {
            var result = await _mediator.Send(request);
            string groupName = request.GameId.ToString();
            await Clients.Group(groupName).SendAsync("BoardRefreshed", result.Board);
            await Clients.Group(groupName).SendAsync("TurnRefreshed", result.CurrentTurn);
        }


        public async Task MakeMove(MakeMoveRequest request)
        {
            var result = await _mediator.Send(request);
            string groupName = request.GameId.ToString();
            await Clients.Group(groupName).SendAsync("BoardRefreshed", result.Board);
            await Clients.Group(groupName).SendAsync("TurnRefreshed", result.CurrentTurn);
            if (result.Winner != null)
            {
                await Clients.Group(groupName).SendAsync("GameOver", result.Winner);
            }
        }
    }
}
