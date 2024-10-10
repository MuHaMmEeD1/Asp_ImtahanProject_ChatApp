using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.LikeModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class LikeController : Controller
{
    private readonly IMapper _mapper;
    private readonly ILikeService _likeService;

    public LikeController(IMapper mapper, ILikeService likeService)
    {
        _mapper = mapper;
        _likeService = likeService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]LikeCreateModel model)
    {
        try
        {
            Like newLike = _mapper.Map<Like>(model);
            await _likeService.AddAsync(newLike);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding like: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] LikeDeleteModel model)
    {
        try
        {
            Like deletedLike = await _likeService.GetById(model.id);
            if (deletedLike == null)
            {
                return NotFound();
            }

            await _likeService.DeleteAsync(deletedLike);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting like: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}
