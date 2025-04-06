using AutoMapper;
using CW2.DAL.Entities;
using CW2.DAL.Repositories;
using CW2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Text;
using System.Xml.Serialization;

namespace CW2.Controllers
{
    public class ArtworkController : Controller
    {


        private readonly IArtworkRepository _artworkRepository;
        private readonly IMapper _mapper;

        //Constructor
        public ArtworkController(IArtworkRepository artworkRepository, IMapper mapper)
        {
            _artworkRepository = artworkRepository;
            _mapper = mapper;
            _artworkRepository = artworkRepository ?? throw new ArgumentNullException(nameof(artworkRepository));

        }

        //-----INDEX-----

        // GET: ArtworkController
        public ActionResult Index()
        {
            var entities = _artworkRepository.GetAll();
            var models = entities.Select(e => _mapper.Map<ArtworkViewModel>(e));

            return View(models);
        }

        //-----DETAILS-----

        // GET: ArtworkController/Details/5
        public ActionResult Details(int id)
        {
            var artwork = _artworkRepository.GetById(id);
            var model = _mapper.Map<ArtworkViewModel>(artwork);
            return View(model);
        }

        //-----CREATE-----

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


        //-----EDIT-----

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

        //-----DELETE-----

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

        //-----FILTER-----

        //GET
        public ActionResult Filter(ArtworkFilterViewModel filter)
        {
            var entities = _artworkRepository.Filter(
                filter.Title,
                filter.Availability,
                filter.ArtistId,
                filter.Page,
                filter.PageSize,
                filter.SortColumn,
                filter.SortDesc
            );

            filter.Artwork = entities.Select(_mapper.Map<ArtworkViewModel>);
            return View(filter);
        }
        //POST


        //-----EXPORT------

        public ActionResult ExportXml(string? title = null,
                             int? artistId = null,
                             int? categoryId = null,
                             int? year = null,
                             string sortColumn = "ArtworkID",
                             bool sortDesc = false)
        {
            var xml = _artworkRepository.ExportToXml(title, year, sortColumn, sortDesc);

            if (string.IsNullOrWhiteSpace(xml))
                return NotFound();
            else
                return File(Encoding.UTF8.GetBytes(xml),
                           "application/xml",
                           $"Artworks_{DateTime.Now:yyyyMMdd}.xml");
        }

        public ActionResult ExportJson(string? title = null,
                                     int? artistId = null,
                                     int? categoryId = null,
                                     int? year = null,
                                     string sortColumn = "ArtworkID",
                                     bool sortDesc = false)
        {
            var json = _artworkRepository.ExportToJson(title, year, sortColumn, sortDesc);

            if (string.IsNullOrWhiteSpace(json))
                return NotFound();
            else
                return File(Encoding.UTF8.GetBytes(json),
                           "application/json",
                           $"Artworks_{DateTime.Now:yyyyMMdd}.json");
        }

        public ActionResult ExportJson(
            string? title = null,
            int? year = null,
            string sortColumn = "ArtworkID",
            bool sortDesc = false)
        {
            var json = _artworkRepository.ExportToJson(title, year, sortColumn, sortDesc);

            if (string.IsNullOrWhiteSpace(json))
                return NotFound();

            return File(Encoding.UTF8.GetBytes(json),
                "application/json",
                $"Artworks_{DateTime.Now:yyyyMMdd}.json");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _artworkRepository.Dispose();

        }
    }
}
