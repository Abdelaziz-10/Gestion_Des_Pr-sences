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
    public class CongéController : Controller
    {
        private readonly Gestion_Des_PrésencesContext _context;

        public CongéController(Gestion_Des_PrésencesContext context)
        {
            _context = context;
        }

        // GET: Congé
        public async Task<IActionResult> Index()
        {
            //var gestion_Des_PrésencesContext = _context.Congés.Include(c => c.NumClNavigation);
            //return View(await gestion_Des_PrésencesContext.ToListAsync());
            if (HttpContext.Session.GetString("emailC") == null)
            {
                return RedirectToAction("Login", "Collaborateurs");
            }
            var email = HttpContext.Session.GetString("emailC");

            var a = from col in _context.Collaborateurs
                    join conge in _context.Congés on col.NumCl equals conge.NumCl
                    where col.Email == email

                    select new Congé
                    {
                        IdCongé = conge.IdCongé,
                        DateDebutCongé = conge.DateDebutCongé,
                        DateFinCongé = conge.DateFinCongé,
                        Types=conge.Types,
                        Accord=conge.Accord
                    };
            return View(await a.ToListAsync());
        }

        // GET: Congé
        public async Task<IActionResult> Congés()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("Login", "Responsables");
            }
            var email = HttpContext.Session.GetString("email");
            var a = from col in _context.Collaborateurs
                    join rc in _context.Respocollaborateurs
                    on col.NumCl equals rc.NumCl
                    join r in _context.Responsables on
                    rc.NumRe equals r.NumRe
                    join conge in _context.Congés on col.NumCl equals conge.NumCl

                    where r.Email == email
                    select new Congé
                    {
                        IdCongé = conge.IdCongé,
                        DateDebutCongé = conge.DateDebutCongé,
                        DateFinCongé = conge.DateFinCongé,
                        Types = conge.Types,
                        Accord = conge.Accord,

                    };
            return View(await a.ToListAsync());
            //var gestion_Des_PrésencesContext = _context.Absences.Include(a => a.NumClNavigation);
            //return View(await gestion_Des_PrésencesContext.ToListAsync());
        }
        // GET: Congé/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congé = await _context.Congés
                .Include(c => c.NumClNavigation)
                .FirstOrDefaultAsync(m => m.IdCongé == id);
            if (congé == null)
            {
                return NotFound();
            }

            return View(congé);
        }

        // GET: Congé/Create
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
                       NumCl=col.NumCl,
                       NomCl=col.NomCl
                       
                    };
            
            ViewData["NumCl"] = new SelectList(a.ToList(), "NumCl", "NomCl");
            return View();
        }

        // POST: Congé/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCongé,DateDebutCongé,DateFinCongé,Types,NumCl,Accord")] Congé congé)
        {
            if (ModelState.IsValid)
            {
                _context.Add(congé);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NumCl"] = new SelectList(_context.Collaborateurs, "NumCl", "NumCl", congé.NumCl);
            return View(congé);
        }

        // GET: Congé/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congé = await _context.Congés.FindAsync(id);
            if (congé == null)
            {
                return NotFound();
            }
            ViewData["NumCl"] = new SelectList(_context.Collaborateurs, "NumCl", "NumCl", congé.NumCl);
            return View(congé);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RedirectToEditCongé(int id)
        {
            TempData["CongéId"] = id;
            return RedirectToAction("EditCongéInternal");
        }

        [HttpGet]
        public async Task<IActionResult> EditCongéInternal()
        {
            if (!TempData.ContainsKey("CongéId"))
                return RedirectToAction("Index");

            int id = (int)TempData["CongéId"];

            var congé = await _context.Congés.FindAsync(id);
            if (congé == null) return NotFound();

            var fullName = await _context.Collaborateurs
                .Where(c => c.NumCl == congé.NumCl)
                .Select(c => c.NomCl + " " + c.PrenomCl)
                .FirstOrDefaultAsync();

            ViewBag.CollaborateurFullName = fullName;
            return View("Edit", congé);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCongéInternal(Congé congé)
        {
            if (!ModelState.IsValid)
                return View("Edit", congé);

            try
            {
                _context.Update(congé);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Congés.Any(a => a.IdCongé == congé.IdCongé))
                    return NotFound();
                else throw;
            }
        }

        // POST: Congé/DeleteAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCongéAjax([FromBody] DeleteItem dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                return Json(new { success = false, message = "ID invalide." });
            }

            var absence = await _context.Congés.FindAsync(dto.Id);
            if (absence == null)
            {
                return Json(new { success = false, message = "Absence introuvable." });
            }

            _context.Congés.Remove(absence);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // POST: Congé/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCongé,DateDebutCongé,DateFinCongé,Types,NumCl,Accord")] Congé congé)
        {
            if (id != congé.IdCongé)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(congé);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CongéExists(congé.IdCongé))
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
            ViewData["NumCl"] = new SelectList(_context.Collaborateurs, "NumCl", "NumCl", congé.NumCl);
            return View(congé);
        }

        // GET: Congé/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congé = await _context.Congés
                .Include(c => c.NumClNavigation)
                .FirstOrDefaultAsync(m => m.IdCongé == id);
            if (congé == null)
            {
                return NotFound();
            }

            return View(congé);
        }

        // POST: Congé/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var congé = await _context.Congés.FindAsync(id);
            _context.Congés.Remove(congé);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CongéExists(int id)
        {
            return _context.Congés.Any(e => e.IdCongé == id);
        }
    }
}
