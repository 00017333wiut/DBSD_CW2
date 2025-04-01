using AutoMapper;
using CW2.DAL.Entities;
using CW2.DAL.Repositories;
using CW2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Serialization;

namespace CW2.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IMapper _mapper;

        public StaffController(IStaffRepository staffRepository, IMapper mapper)
        {
            _staffRepository = staffRepository;
            _mapper = mapper;
        }

        // GET: StaffController
        public ActionResult Index()
        {
            var entities = _staffRepository.GetAll();
            var models = entities.Select(e => _mapper.Map<StaffViewModel>(e));

            return View(models);
        }

        // GET: StaffController/Details/5
        public ActionResult Details(int id)
        {
            var staff = _staffRepository.GetById(id);
            var model = _mapper.Map<StaffViewModel>(staff);
            return View(model);
        }

        // GET: StaffController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StaffController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StaffViewModel model)
        {
            try
            {
                var staff = _mapper.Map<Staff>(model);
                _staffRepository.Insert(staff);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View();
            }
        }
    

        // GET: StaffController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StaffController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StaffController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StaffController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
