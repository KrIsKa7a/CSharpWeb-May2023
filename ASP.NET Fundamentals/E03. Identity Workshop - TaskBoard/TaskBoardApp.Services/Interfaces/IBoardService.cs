namespace TaskBoardApp.Services.Interfaces
{
    using Web.ViewModels.Board;
    using Web.ViewModels.Task;

    public interface IBoardService
    {
        Task<IEnumerable<BoardAllViewModel>> AllAsync();

        Task<IEnumerable<BoardSelectViewModel>> AllForSelectAsync();

        Task<bool> ExistsByIdAsync(int id);
    }
}
