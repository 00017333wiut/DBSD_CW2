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
    public class ArtworkController : Controller
    {
        private readonly IArtworkRepository _artworkRepository;
        private readonly IMapper _mapper;

        public ArtworkController(IArtworkRepository artworkRepository, IMapper mapper)
        {
            _artworkRepository = artworkRepository;
            _mapper = mapper;
        }

        // GET: ArtworkController
        public ActionResult Index()
        {
            var entities = _artworkRepository.GetAll();
            var models = entities.Select(e => _mapper.Map<ArtworkViewModel>(e));

            return View(models);
        }

        // GET: ArtworkController/Details/5
        public ActionResult Details(int id)
        {
            var artwork = _artworkRepository.GetById(id);
            var model = _mapper.Map<ArtworkViewModel>(artwork);
            return View(model);
        }

        // GET: ArtworkController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArtworkController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArtworkViewModel model)
        {
            try
            {
                var artwork = _mapper.Map<Artwork>(model);
                _artworkRepository.Insert(artwork);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View();
            }
        }
    

        // GET: ArtworkController/Edit/5
        public ActionResult Edit(int id)
        {
            var artwork = _artworkRepository.GetById(id);
            var model = _mapper.Map<ArtworkViewModel>(artwork);
            return View(model);
        }

        // POST: ArtworkController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ArtworkViewModel model)
        {
            try
            {
                var artwork = _mapper.Map<Artwork>(model);
                _artworkRepository.Update(artwork);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArtworkController/Delete/5
        public ActionResult Delete(int id)
        {
            var artwork = _artworkRepository.GetById(id);
            return View(_mapper.Map<ArtworkViewModel>(artwork));
        }

        // POST: ArtworkController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ArtworkViewModel model)
        {
            try
            {
                var artwork = _mapper.Map<Artwork>(model);
                artwork.ArtworkId = id;
                _artworkRepository.Delete(artwork);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View();
            }
        }
    }
}
