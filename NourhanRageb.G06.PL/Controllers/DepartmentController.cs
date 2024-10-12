using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NourhanRageb.G06.BLL.interfaces;
using NourhanRageb.G06.DAL.Models;

namespace NourhanRageb.G06.PL.Controllers
{
	[Authorize]
	public class DepartmentController : Controller
    {

        private readonly IDepartmentRepository _departmentRepository; //NULL 

        public DepartmentController(IDepartmentRepository departmentRepository) // ASK CLR To Create Object From DepartmentRepository
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var department = await _departmentRepository.GetAllAsync();
            return View(department);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department model)
        {
            if (ModelState.IsValid)
            {
                var count = await _departmentRepository.AddAsync(model);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var department = await _departmentRepository.GetAsync(id.Value);
            if (department is null) return NotFound();
            return View(ViewName, department);

        }

        public async Task<IActionResult> Edit(int? id)
        {
        
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit([FromRoute] int? id, Department department)
        {
            try
            {
                if (id != department.Id) return BadRequest();

                if (ModelState.IsValid)
                {
                    var count = await _departmentRepository.UpdateAsync(department);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(department);

        }

        public async Task<IActionResult> Delete(int? id)
        {
         
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Department department)
        {
            try
            {
                await _departmentRepository.DeleteAsync(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(department);
            }

        }
    }
}
