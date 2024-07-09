 using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase{

    //GET: https://loclhost:portnumber
    [HttpGet]
    public IActionResult GetAllStudents(){

        string[] studentNames = new string[] {"A","b","c"};
        return Ok(studentNames);

    }

}