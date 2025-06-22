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
using static Gestion_Des_prèneces.Controllers.ResponsablesController;

namespace Gestion_Des_prèneces.Controllers
{
    public class AbsencesController : Controller
    {
        private readonly Gestion_Des_PrésencesContext _context;

        public AbsencesController(Gestion_Des_PrésencesContext context)
        {
            _context = context;
        }  

        // GET: Absences
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("emailC") == null)
            {
                return RedirectToAction("Login", "Collaborateurs");
            }
            var email = HttpContext.Session.GetString("emailC");
            var a = from col in _context.Collaborateurs
                    join ab in _context.Absences on col.NumCl equals ab.NumCl
                    where col.Email == email
                    select new Absence
                    {
                        IdAbsence = ab.IdAbsence,
                        DateDebutAb = ab.DateDebutAb,
                        DateFinAb = ab.DateFinAb,
                        NbHAb = ab.NbHAb,

                    };
            return View(await a.ToListAsync());
            //var gestion_Des_PrésencesContext = _context.Absences.Include(a => a.NumClNavigation);
            //return View(await gestion_Des_PrésencesContext.ToListAsync());
        }

        // GET: Absences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var absence = await _context.Absences
                .Include(a => a.NumClNavigation)
                .FirstOrDefaultAsync(m => m.IdAbsence == id);
            if (absence == null)
            {
                return NotFound();
            }

            return View(absence);
        }

        // GET: Absences/Create
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
        }

        // POST: Absences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAbsence,DateDebutAb,DateFinAb,NbHAb,NumCl")] Absence absence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(absence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NumCl"] = new SelectList(_context.Collaborateurs, "NumCl", "NumCl", absence.NumCl);
            return View(absence);
        }

        // GET: Absences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var absence = await _context.Absences.FindAsync(id);
            if (absence == null)
            {
                return NotFound();
            }
            ViewData["NumCl"] = new SelectList(_context.Collaborateurs, "NumCl", "NumCl", absence.NumCl);
            return View(absence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RedirectToEditAbsence(int id)
        {
            TempData["AbsenceId"] = id;
            return RedirectToAction("EditAbsenceInternal");
        }

        [HttpGet]
        public async Task<IActionResult> EditAbsenceInternal()
        {
            if (!TempData.ContainsKey("AbsenceId"))
                return RedirectToAction("Index");

            int id = (int)TempData["AbsenceId"];

            var absence = await _context.Absences.FindAsync(id);
            if (absence == null) return NotFound();

            var fullName = await _context.Collaborateurs
                .Where(c => c.NumCl == absence.NumCl)
                .Select(c => c.NomCl + " " + c.PrenomCl)
                .FirstOrDefaultAsync();

            ViewBag.CollaborateurFullName = fullName;
            return View("Edit", absence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAbsenceInternal(Absence absence)
        {
            if (!ModelState.IsValid)
                return View("Edit", absence);

            try
            {
                _context.Update(absence);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Absences.Any(a => a.IdAbsence == absence.IdAbsence))
                    return NotFound();
                else throw;
            }
        }

        // DeleteAbsenceAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAbsenceAjax([FromBody] DeleteItem dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                return Json(new { success = false, message = "ID invalide." });
            }

            var absence = await _context.Absences.FindAsync(dto.Id);
            if (absence == null)
            {
                return Json(new { success = false, message = "Absence introuvable." });
            }

            _context.Absences.Remove(absence);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        // POST: Absences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAbsence,DateDebutAb,DateFinAb,NbHAb,NumCl")] Absence absence)
        {
            if (id != absence.IdAbsence)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(absence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbsenceExists(absence.IdAbsence))
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
            ViewData["NumCl"] = new SelectList(_context.Collaborateurs, "NumCl", "NumCl", absence.NumCl);
            return View(absence);
        }

        // GET: Absences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var absence = await _context.Absences
                .Include(a => a.NumClNavigation)
                .FirstOrDefaultAsync(m => m.IdAbsence == id);
            if (absence == null)
            {
                return NotFound();
            }

            return View(absence);
        }

        // POST: Absences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var absence = await _context.Absences.FindAsync(id);
            _context.Absences.Remove(absence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbsenceExists(int id)
        {
            return _context.Absences.Any(e => e.IdAbsence == id);
        }
    }
}
