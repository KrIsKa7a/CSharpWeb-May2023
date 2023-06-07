namespace TaskBoardApp.Services
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Interfaces;
    using Web.ViewModels.Board;
    using Web.ViewModels.Task;

    public class BoardService : IBoardService
    {
        private readonly TaskBoardDbContext dbContext;

        public BoardService(TaskBoardDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<BoardAllViewModel>> AllAsync()
        {
            IEnumerable<BoardAllViewModel> allBoards = await this.dbContext
                .Boards
                .Select(b => new BoardAllViewModel()
                {
                    Name = b.Name,
                    Tasks = b.Tasks
                        .Select(t => new TaskViewModel()
                        {
                            Id = t.Id.ToString(),
                            Title = t.Title,
                            Description = t.Description,
                            Owner = t.Owner.UserName
                        })
                        .ToArray()
                })
                .ToArrayAsync();

            return allBoards;
        }

        public async Task<IEnumerable<BoardSelectViewModel>> AllForSelectAsync()
        {
            IEnumerable<BoardSelectViewModel> allBoards = await this.dbContext
                .Boards
                .Select(b => new BoardSelectViewModel()
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToArrayAsync();

            return allBoards;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            bool result = await this.dbContext
                .Boards
                .AnyAsync(b => b.Id == id);

            return result;
        }
    }
}