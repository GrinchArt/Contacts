using Contact_Prj.Context;
using Contact_Prj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contact_Prj.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ContactsDbContext _context;

        public ContactsController(ContactsDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Contacts.ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> UploadCsv(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                return Content("File not selected");
            }

            using (var stream = new StreamReader(csvFile.OpenReadStream()))
            {
                bool isFirstLine = true;
                bool hasHeaders = false;

                while (!stream.EndOfStream)
                {
                    var line = await stream.ReadLineAsync();

                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        var values = line.Split(',');

                        // HEre we check the first line to confirm if there are headers
                        if (values.Length == 5 && values[0].Trim().ToLower() == "name")
                        {
                            hasHeaders = true;
                            continue; // If there are headers, we skip the first line
                        }
                        else
                        {
                            // Here we proceed if the are no headers
                            ProcessLine(values);
                            continue;
                        }
                    }

                    var dataValues = line.Split(',');

                    if (dataValues.Length < 5)
                    {
                        continue;
                    }

                    ProcessLine(dataValues);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        private void ProcessLine(string[] values)
        {
            try
            {
                var contact = new Contact
                {
                    Name = values[0],
                    DateOfBirth = DateTime.Parse(values[1]),
                    Married = bool.Parse(values[2]),
                    Phone = values[3],
                    Salary = decimal.Parse(values[4])
                };

                _context.Contacts.Add(contact);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error parsing data: {ex.Message}");
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> Edit(int id, [FromBody] Contact contact)
        //{
        //    if (contact == null || id != contact.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(contact).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}


        //From the beggining I tried to use [FromBody] atribute, but it refused to fetch with JS(I guess the is my bad),
        //so I used the simple way to pass properties separatly

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, string dateOfBirth, bool married, string phone, decimal salary)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone) || salary < 0)
            {
                return BadRequest("Invalid data");
            }

            contact.Name = name;
            contact.DateOfBirth = DateTime.Parse(dateOfBirth);
            contact.Married = married;
            contact.Phone = phone;
            contact.Salary = salary;

            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
