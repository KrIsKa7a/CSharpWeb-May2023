namespace TaskBoardApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Extensions;
    using Services.Interfaces;
    using Web.ViewModels.Task;

    [Authorize]
    public class TaskController : Controller
    {
        private readonly IBoardService boardService;
        private readonly ITaskService taskService;

        public TaskController(IBoardService boardService, ITaskService taskService)
        {
            this.boardService = boardService;
            this.taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            TaskFormModel viewModel = new TaskFormModel()
            {
                AllBoards = await this.boardService.AllForSelectAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AllBoards = await this.boardService.AllForSelectAsync();
                return this.View(model);
            }

            bool boardExists = await this.boardService.ExistsByIdAsync(model.BoardId);
            if (!boardExists)
            {
                ModelState.AddModelError(nameof(model.BoardId), "Selected board does not exist!");
                model.AllBoards = await this.boardService.AllForSelectAsync();
                return this.View(model);
            }

            string currentUserId = this.User.GetId();
            await this.taskService.AddAsync(currentUserId, model);

            return this.RedirectToAction("All", "Board");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                TaskDetailsViewModel viewModel = 
                    await this.taskService.GetForDetailsByIdAsync(id);

                return this.View(viewModel);
            }
            catch (Exception)
            {
                return this.RedirectToAction("All", "Board");
            }
        }
    }
}
