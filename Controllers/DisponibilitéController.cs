using Gestion_Des_prèneces.Models;
using Gestion_Des_prèneces.Models.ModelsView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Des_prèneces.Controllers
{
    public class DisponibilitéController : Controller
    {
        private readonly Gestion_Des_PrésencesContext _context;

        public DisponibilitéController(Gestion_Des_PrésencesContext context)
        {
            _context = context;
        }

        // GET: Disponibilité
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("emailC") == null)
            {
                return RedirectToAction("Login", "Collaborateurs");
            }
            var email = HttpContext.Session.GetString("emailC");

            var a = from p in _context.Collaborateurs
                    join q in _context.Disponibilités on p.NumCl equals q.NumCl
                    where p.Email == email
                    select new Disponibilité
                    {
                        IdDisponibilité = q.IdDisponibilité,
                        DateMiseEnDisponibilité = q.DateMiseEnDisponibilité,
                        DateHDebutDisponibilité = q.DateHDebutDisponibilité,
                        DateHFinDisponibilité = q.DateHFinDisponibilité,
                        // NumClNavigation=p.NumReNavigation
                    };
            return View(await a.ToListAsync());
          //  var co = _context.Collaborateurs.Include(e=>e.Email== "AlaouiRania1@gmail.com");
            //var gestion_Des_PrésencesContext = _context.Disponibilités.Include(d => d.NumClNavigation);
            //return View(await gestion_Des_PrésencesContext.Where(c).ToListAsync());
        }

        // GET: Disponibilité/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disponibilité = await _context.Disponibilités
                .Include(d => d.NumClNavigation)
                .FirstOrDefaultAsync(m => m.IdDisponibilité == id);
            if (disponibilité == null)
            {
                return NotFound();
            }

            return View(disponibilité);
        }

        // GET: Disponibilité/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("emailC") == null)
            {
                return RedirectToAction("Login", "Collaborateurs");
            }
            var email = HttpContext.Session.GetString("emailC");

            var a = from col in _context.Collaborateurs

                    where col.Email == email

                    select new CollaborateursModelView
                    {
                        NumCl = col.NumCl,
                        NomCl = col.NomCl

                    };

            ViewData["NumCl"] = new SelectList(a.ToList(), "NumCl", "NomCl");
            return View();
            //var a = from c in _context.Collaborateurs
            //        where c.Email == "qq@gmail.com"
            //        select new Collaborateur
            //        {
            //            NumCl=c.NumCl,
            //            NomCl=c.NomCl
            //        };
            //ViewData["NumCl"] = new SelectList(a, "NumCl", "NomCl");
            //return View();
        }
        
        // POST: Disponibilité/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDisponibilité,DateMiseEnDisponibilité,DateHDebutDisponibilité,DateHFinDisponibilité,NumCl")] Disponibilité disponibilité)
        {
            if (ModelState.IsValid)
            {
                _context.Add(disponibilité);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NumCl"] = new SelectList(_context.Collaborateurs, "NumCl", "NumCl", disponibilité.NumCl);
            return View(disponibilité);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RedirectToEditDisponibilité(int id)
        {
            TempData["DisponibilitéId"] = id;
            return RedirectToAction("EditDisponibilitéInternal");
        }

        [HttpGet]
        public async Task<IActionResult> EditDisponibilitéInternal()
        {
            if (!TempData.ContainsKey("DisponibilitéId"))
                return RedirectToAction("Index");

            int id = (int)TempData["DisponibilitéId"];

            var disponibilité = await _context.Disponibilités.FindAsync(id);
            if (disponibilité == null) return NotFound();

            var fullName = await _context.Collaborateurs
                .Where(c => c.NumCl == disponibilité.NumCl)
                .Select(c => c.NomCl + " " + c.PrenomCl)
                .FirstOrDefaultAsync();

            ViewBag.CollaborateurFullName = fullName;
            return View("Edit", disponibilité);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDisponibilitéInternal(Disponibilité disponibilité)
        {
            if (!ModelState.IsValid)
                return View("Edit", disponibilité);

            try
            {
                _context.Update(disponibilité);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Disponibilités.Any(a => a.IdDisponibilité == disponibilité.IdDisponibilité))
                    return NotFound();
                else throw;
            }
        }


        // GET: Disponibilité/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disponibilité = await _context.Disponibilités.FindAsync(id);
            if (disponibilité == null)
            {
                return NotFound();
            }
            ViewData["NumCl"] = new SelectList(_context.Collaborateurs, "NumCl", "NumCl", disponibilité.NumCl);
            return View(disponibilité);
        }

        // POST: Disponibilité/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDisponibilité,DateMiseEnDisponibilité,DateHDebutDisponibilité,DateHFinDisponibilité,NumCl")] Disponibilité disponibilité)
        {
            if (id != disponibilité.IdDisponibilité)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disponibilité);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisponibilitéExists(disponibilité.IdDisponibilité))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["NumCl"] = new SelectList(_context.Collaborateurs, "NumCl", "NumCl", disponibilité.NumCl);
            return View(disponibilité);
        }

        // GET: Disponibilité/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disponibilité = await _context.Disponibilités
                .Include(d => d.NumClNavigation)
                .FirstOrDefaultAsync(m => m.IdDisponibilité == id);
            if (disponibilité == null)
            {
                return NotFound();
            }

            return View(disponibilité);
        }

        // POST: Disponibilité/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disponibilité = await _context.Disponibilités.FindAsync(id);
            _context.Disponibilités.Remove(disponibilité);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisponibilitéExists(int id)
        {
            return _context.Disponibilités.Any(e => e.IdDisponibilité == id);
        }
    }
}
