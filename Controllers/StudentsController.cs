using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : Controller
    {
        private AppDb _context;
        private FilesController fileController;

        public StudentsController()
        {
                _context = new AppDb();
            fileController = new FilesController();
            
        }
        [HttpGet]
        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Student>> GetStudent(int id)
        //{
        //    var student = await _context.Students.FindAsync(id);

        //    if (student == null)
        //    {
        //        return NotFound(); // Return a 404 Not Found response if the student is not found
        //    }

        //    return student; // Return the found student
        //}
        [HttpGet("{email}")]
        public async Task<ActionResult<Student>> GetUserByEmail(string email)
        {
            var user = await _context.Students.FirstOrDefaultAsync(x => x.email == email);

            if (user == null)
            {
                // If user is not found, return a 404 Not Found response
                return NotFound();
            }

            // If user is found, return a 200 OK response with the user data
            return Ok(user);
        }


        private async Task<Student?> getUserByEmail(string email)
        {
            return await _context.Students
                .FirstOrDefaultAsync(x => x.email == email);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] Student student)
        {
            try
            {
                if (await getUserByEmail(student.email) is Student _)
                    return Conflict("user already exist");

                else if (student.profilePictureName != null ) {
                    await _context.Students.AddAsync(student);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetUserByEmail), new { email = student.email }, student);
                }

                await _context.Students.AddAsync(student);
                student.profilePictureName = "uaer.png";
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUserByEmail), new { email = student.email }, student);



                // Return a 201 Created response with the added student

            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 Internal Server Error response
                // You may want to handle exceptions more gracefully in a production scenario
                // and provide more meaningful error messages.
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id,  Student updatedStudent)
        {
            try
            {
                var existingStudent = await _context.Students.FindAsync(id);

                if (existingStudent == null)
                {
                    return NotFound(); // Return a 404 Not Found response if the student with the given id is not found
                }

                // Update the existing student's properties with values from the updatedStudent
                existingStudent.firstName = updatedStudent.firstName;
                existingStudent.surname = updatedStudent.surname;
                existingStudent.profilePictureName = updatedStudent.profilePictureName;
                existingStudent.registrationDate = updatedStudent.registrationDate;
                existingStudent.dateOfBirth = updatedStudent.dateOfBirth;
                existingStudent.Programme = updatedStudent.Programme;
                
                // Update other properties as needed

                _context.Students.Update(existingStudent);
                await _context.SaveChangesAsync();

                // Return a 200 OK response with the updated student
                return Ok(existingStudent);
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 Internal Server Error response
                // You may want to handle exceptions more gracefully in a production scenario
                // and provide more meaningful error messages.
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var studentToDelete = await _context.Students.FindAsync(id);

                if (studentToDelete == null)
                {
                    return NotFound(); // Return a 404 Not Found response if the student with the given id is not found
                }

                _context.Students.Remove(studentToDelete);
                await _context.SaveChangesAsync();

                // Return a 200 OK response with a message or you can return the deleted student if needed
                return Ok("Student deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 Internal Server Error response
                // You may want to handle exceptions more gracefully in a production scenario
                // and provide more meaningful error messages.
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }
        //[HttpPost("{id}/upload-profile-picture")]
        //public async Task<IActionResult> UploadProfilePicture(int id, IFormFile file)
        //{
        //    try
        //    {
        //        var student = await _context.Students.FindAsync(id);

        //        if (student == null)
        //        {
        //            return NotFound(); // Return a 404 Not Found response if the student with the given id is not found
        //        }

        //        // Call the file upload logic from the FilesController
        //        var result = await fileController.Upload(file);

        //        // Assuming the file upload result contains the unique file name
        //       // var uniqueFileName = result.Value as string;

        //        // Update the student's profile picture property with the unique file name
        //        student.profilePictureName = uniqueFileName;

        //        // Update other properties as needed

        //        _context.Students.Update(student);
        //        await _context.SaveChangesAsync();

        //        // Return a 200 OK response with a message or the updated student
        //        return Ok($"Profile picture uploaded successfully. File name: {uniqueFileName}");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception and return a 500 Internal Server Error response
        //        Console.WriteLine(ex.Message);
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}



    }
}
