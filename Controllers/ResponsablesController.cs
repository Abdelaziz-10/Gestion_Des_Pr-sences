using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gestion_Des_prèneces.Models;
using Microsoft.AspNetCore.Http;
using Gestion_Des_prèneces.Models.ModelsView;
using Syncfusion.EJ2.Linq;

namespace Gestion_Des_prèneces.Controllers
{
    public class ResponsablesController : Controller
    {
        private readonly Gestion_Des_PrésencesContext _context;

        public ResponsablesController(Gestion_Des_PrésencesContext context)
        {
            _context = context;
        }

        // GET: Responsables
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return View("Login");
            }

            var email = HttpContext.Session.GetString("email");
            var a = from respo in _context.Responsables
                   
                    where respo.Email == email
                    select new Responsable
                    {
                        NumRe = respo.NumRe,

                        NomRe = respo.NomRe,
                        PrenomRe = respo.PrenomRe,
                        Email = respo.Email,
                        Tel = respo.Tel,
                        Adresse = respo.Adresse,


                    };

            return View(await a.ToListAsync());
        }
        public async Task<IActionResult> AllResponsables()
        {
            var alldata = _context.Responsables.ToListAsync();
            return View(await alldata);
        }

        [HttpGet]
        public IActionResult Login()
        {
          
            return View();
        }
        public ActionResult Validate(Responsable admin)
        {
            var _responsable = _context.Responsables.Where(s => s.Email == admin.Email);
            //var SessionName = "";
           // HttpContext.Session.SetString(SessionName, admin.Email);
            //ViewBag.Name = HttpContext.Session.GetString(admin.Email);
            if (_responsable.Any())
            {
                if (_responsable.Where(s => s.MotDePasse == admin.MotDePasse).Any())
                {
                    HttpContext.Session.SetString("email", admin.Email);
                    return Json(new { status = true, message = "Login Successfull!" });
                }
                else
                {
                    return Json(new { status = false, message = "Invalid Password!" });
                }
            }
            else
            {
                return Json(new { status = false, message = "Invalid Email!" });
            }

        }


        // GET: Responsables1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var responsable = await _context.Responsables.FindAsync(id);
            if (responsable == null)
            {
                return NotFound();
            }
            return View(responsable);
        }

        // POST: Responsables1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumRe,NomRe,PrenomRe,Adresse,Tel,Email,MotDePasse")] Responsable responsable)
        {
            if (id != responsable.NumRe)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(responsable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResponsableExists(responsable.NumRe))
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
            return View(responsable);
        }

        // GET: Responsables1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var responsable = await _context.Responsables
                .FirstOrDefaultAsync(m => m.NumRe == id);
            if (responsable == null)
            {
                return NotFound();
            }

            return View(responsable);
        }

        // POST: Responsables1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var responsable = await _context.Responsables.FindAsync(id);
            _context.Responsables.Remove(responsable);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResponsableExists(int id)
        {
            return _context.Responsables.Any(e => e.NumRe == id);
        }


        public ActionResult Bienvenu()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return View("Login");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Loginout()
        {
            HttpContext.Session.Remove("email");
            return View("login");
        }
        public async Task<IActionResult> ShowCollaborateurs()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return View("Login");
            }

            var email = HttpContext.Session.GetString("email");
            var a = from col in _context.Collaborateurs
                    join rc in _context.Respocollaborateurs
                    on col.NumCl equals rc.NumCl
                    join r in _context.Responsables on
                    rc.NumRe equals r.NumRe
                    where r.Email == email
                    select new CollaborateursModelView
                    {
                        NumCl = col.NumCl,

                        NomCl = col.NomCl,
                        PrenomCl = col.PrenomCl,
                        Email = col.Email,
                        Tel = col.Tel,
                        Adresse = col.Adresse,

                    };

            return View(await a.ToListAsync());
        }
        // GET: Absences
        public async Task<IActionResult> Absences()
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
                    join ab in _context.Absences
                    on col.NumCl equals ab.NumCl
                    where r.Email == email
                    select new AbsenceView
                    {
                        IdAbsence = ab.IdAbsence,
                        DateDebutAb = ab.DateDebutAb,
                        DateFinAb = ab.DateFinAb,
                        NomCl=col.NomCl,
                        Prenom=col.PrenomCl,

                    };
            return View(await a.ToListAsync());
            //var gestion_Des_PrésencesContext = _context.Absences.Include(a => a.NumClNavigation);
            //return View(await gestion_Des_PrésencesContext.ToListAsync());
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
                return RedirectToAction("Absences");

            int id = (int)TempData["AbsenceId"];

            var absence = await _context.Absences.FindAsync(id);
            if (absence == null) return NotFound();

            var fullName = await _context.Collaborateurs
                .Where(c => c.NumCl == absence.NumCl)
                .Select(c => c.NomCl + " " + c.PrenomCl)
                .FirstOrDefaultAsync();

            ViewBag.CollaborateurFullName = fullName;
            return View("EditAbsences", absence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAbsenceInternal(Absence absence)
        {
            if (!ModelState.IsValid)
                return View("EditAbsences", absence);

            try
            {
                _context.Update(absence);
                await _context.SaveChangesAsync();
                return RedirectToAction("Absences");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Absences.Any(a => a.IdAbsence == absence.IdAbsence))
                    return NotFound();
                else throw;
            }
        }






        // GET: Absences/Details/5
        public async Task<IActionResult> DetailsAbsences(int? id)
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

        public class DeleteItem
        {
            public int Id { get; set; }
        }


        // GET: Absences/Delete/5
        public async Task<IActionResult> DeleteAbsences(int? id)
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
        public async Task<IActionResult> DeleteConfirmeddd(int id)
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
                    select new CongéView
                    {
                        IdCongé = conge.IdCongé,
                        DateDebutCongé = conge.DateDebutCongé,
                        DateFinCongé = conge.DateFinCongé,
                        Types = conge.Types,
                        NomCl = col.NomCl,
                        Prenom = col.PrenomCl,
                        Accord = conge.Accord,


                    };
            return View(await a.ToListAsync());
            //var gestion_Des_PrésencesContext = _context.Absences.Include(a => a.NumClNavigation);
            //return View(await gestion_Des_PrésencesContext.ToListAsync());
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
                return RedirectToAction("Congés");

            int id = (int)TempData["CongéId"];

            var congé = await _context.Congés.FindAsync(id);
            if (congé == null) return NotFound();

            var fullName = await _context.Collaborateurs
                .Where(c => c.NumCl == congé.NumCl)
                .Select(c => c.NomCl + " " + c.PrenomCl)
                .FirstOrDefaultAsync();

            ViewBag.CollaborateurFullName = fullName;
            return View("EditCongés", congé);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCongéInternal(Congé congé)
        {
            if (!ModelState.IsValid)
                return View("EditCongés", congé);

            try
            {
                _context.Update(congé);
                await _context.SaveChangesAsync();
                return RedirectToAction("Congés");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Congés.Any(a => a.IdCongé == congé.IdCongé))
                    return NotFound();
                else throw;
            }
        }




        // GET: Congé/Edit/5
        public async Task<IActionResult> EditCongés(int? id)
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
            //ViewData["NumCl"] = new SelectList(
            //    _context.Collaborateurs.Select(c => new {
            //        NumCl = c.NumCl,
            //        FullName = c.NomCl + " " + c.PrenomCl
            //    }),
            //    "NumCl", "FullName", congé.NumCl
            //);
            var fullName = _context.Collaborateurs
            .Where(c => c.NumCl == congé.NumCl)
            .Select(c => c.NomCl + " " + c.PrenomCl)
            .FirstOrDefault();

            ViewBag.CollaborateurFullName = fullName;

            //ViewData["NumCl"] = new SelectList(_context.Collaborateurs, "NumCl", "NomCl", congé.NumCl);
            return View(congé);
        }

        // POST: Congé/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCongés(int id, [Bind("IdCongé,DateDebutCongé,DateFinCongé,Types,NumCl,Accord")] Congé congé)
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
                return RedirectToAction("Congés");
            }
            //.Include(c => c.NumClNavigation)
           
            ViewData["NumCl"] = new SelectList(_context.Collaborateurs, "NumCl", "NomCl", congé.NumCl);
            return View(congé);
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
        // GET: Congé/Delete/5
        public async Task<IActionResult> DeleteCongés(int? id)
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
        public async Task<IActionResult> DeleteConfirmedddd(int id)
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
        // GET: Disponibilité
        public async Task<IActionResult> Disponibilités()
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
                    join dis in _context.Disponibilités 
                    on col.NumCl equals dis.NumCl
                    where r.Email == email
                    select new DisponibilitéView
                    {
                        IdDisponibilité = dis.IdDisponibilité,
                        DateMiseEnDisponibilité = dis.DateMiseEnDisponibilité,
                        DateHDebutDisponibilité = dis.DateHDebutDisponibilité,
                        DateHFinDisponibilité = dis.DateHFinDisponibilité,
                        NomCl = col.NomCl,
                        Prenom = col.PrenomCl,
                        // NumClNavigation=p.NumReNavigation
                    };
            return View(await a.ToListAsync());
            //var gestion_Des_PrésencesContext = _context.Absences.Include(a => a.NumClNavigation);
            //return View(await gestion_Des_PrésencesContext.ToListAsync());
        }

        // GET: Disponibilité/Details/5
        public async Task<IActionResult> DetailsDisponibilité(int? id)
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

        // GET: Disponibilité/Edit/5
        public async Task<IActionResult> EditDisponibilité(int? id)
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
        public async Task<IActionResult> EditDisponibilité(int id, [Bind("IdDisponibilité,DateMiseEnDisponibilité,DateHDebutDisponibilité,DateHFinDisponibilité,NumCl")] Disponibilité disponibilité)
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
                return RedirectToAction("Disponibilités");
            }
            ViewData["NumCl"] = new SelectList(_context.Collaborateurs, "NumCl", "NumCl", disponibilité.NumCl);
            return View(disponibilité);
        }

        // GET: Disponibilité/Delete/5
        public async Task<IActionResult> DeleteDisponibilité(int? id)
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
        public async Task<IActionResult> DeleteConfirmedd(int id)
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

        // GET: Responsables/Addcollaborateur
        [HttpGet]
        public IActionResult AddCollaborateur()
        {
            return View();
        }
        // POST: Responsables/Addcollaborateur
        [HttpPost]

        public async Task<IActionResult> AddCollaborateur(Collaborateur model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = HttpContext.Session.GetString("email");
            var responsable = await _context.Responsables.FirstOrDefaultAsync(r => r.Email == email);

            if (responsable == null)
                return RedirectToAction("Login");

            var collaborateur = new Collaborateur
            {
                NomCl = model.NomCl,
                PrenomCl = model.PrenomCl,
                Email = model.Email,
                Tel = model.Tel,
                Adresse = model.Adresse
            };

            _context.Collaborateurs.Add(collaborateur);
            await _context.SaveChangesAsync();

            var link = new Respocollaborateur
            {
                NumCl = collaborateur.NumCl,
                NumRe = responsable.NumRe
            };

            _context.Respocollaborateurs.Add(link);
            await _context.SaveChangesAsync();
           // return Json(new { success = true });
            return RedirectToAction("ShowCollaborateurs");
        }
        // new Action
        //public async Task<IActionResult> ManageCollaborateurs()
        //{
        //    if (HttpContext.Session.GetString("email") == null)
        //        return RedirectToAction("Login");

        //    var email = HttpContext.Session.GetString("email");

        //    var responsable = await _context.Responsables
        //        .FirstOrDefaultAsync(r => r.Email == email);

        //    if (responsable == null)
        //        return RedirectToAction("Login");

        //    var allCollabs = await _context.Collaborateurs
        //        .Select(c => new CollaborateursModelView
        //        {
        //            NumCl = c.NumCl,
        //            NomCl = c.NomCl,
        //            PrenomCl = c.PrenomCl,
        //            Email = c.Email,
        //            Tel = c.Tel,
        //            Adresse = c.Adresse
        //        }).ToListAsync();

        //    var assignedIds = await _context.Respocollaborateurs
        //        .Where(rc => rc.NumRe == responsable.NumRe)
        //        .Select(rc => rc.NumCl)
        //        .ToListAsync();

        //    ViewBag.AssignedIds = assignedIds;
        //    return View(allCollabs);
        //}
        //// This is working
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCollaborateurToResponsable(int collaborateurId)
        {
            var email = HttpContext.Session.GetString("email");
            var responsable = await _context.Responsables.FirstOrDefaultAsync(r => r.Email == email);

            if (responsable == null)
                return RedirectToAction("Login");

            // Prevent duplicates
            var exists = await _context.Respocollaborateurs
                .AnyAsync(rc => rc.NumRe == responsable.NumRe && rc.NumCl == collaborateurId);

            if (!exists)
            {
                var link = new Respocollaborateur
                {
                    NumRe = responsable.NumRe,
                    NumCl = collaborateurId
                };
                _context.Respocollaborateurs.Add(link);
                await _context.SaveChangesAsync();
            }
            return Json(new { success = true });
            //return RedirectToAction("ManageCollaborateurs");
        }


        // The collaborateur is added via AJAX (no page reload).
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCollaborateurToResponsableAjax(int collaborateurId)
        {
            try
            {
                if (collaborateurId <= 0)
                    return Json(new { success = false, message = "Collaborateur invalide." });

                var email = HttpContext.Session.GetString("email");
                if (email == null)
                    return Json(new { success = false, message = "Session expirée. Veuillez vous reconnecter." });

                var responsable = await _context.Responsables.FirstOrDefaultAsync(r => r.Email == email);
                if (responsable == null)
                    return Json(new { success = false, message = "Responsable introuvable." });

                var collabExists = await _context.Collaborateurs.AnyAsync(c => c.NumCl == collaborateurId);
                if (!collabExists)
                    return Json(new { success = false, message = "Collaborateur non trouvé." });

                var exists = await _context.Respocollaborateurs
                    .AnyAsync(rc => rc.NumRe == responsable.NumRe && rc.NumCl == collaborateurId);

                if (exists)
                    return Json(new { success = false, message = "Ce collaborateur est déjà assigné." });

                _context.Respocollaborateurs.Add(new Respocollaborateur
                {
                    NumRe = responsable.NumRe,
                    NumCl = collaborateurId
                });

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erreur serveur : " + ex.Message });
            }
        }



        [HttpPost]
        public async Task<IActionResult> RemoveCollaborateurFromResponsableAjax(int collaborateurId)
        {
            var email = HttpContext.Session.GetString("email");
            var responsable = await _context.Responsables.FirstOrDefaultAsync(r => r.Email == email);
            if (responsable == null) return Json(new { success = false, message = "Session expirée" });

            var link = await _context.Respocollaborateurs
                .FirstOrDefaultAsync(rc => rc.NumRe == responsable.NumRe && rc.NumCl == collaborateurId);

            if (link != null)
            {
                _context.Respocollaborateurs.Remove(link);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Non trouvé." });
        }

        //  Pagination (10 collaborateurs per page)
        public async Task<IActionResult> ManageCollaborateurs(int page = 1, string search = "", string filter = "all")
        {
            int pageSize = 10;

            var email = HttpContext.Session.GetString("email");
            if (email == null) return RedirectToAction("Login");

            var responsable = await _context.Responsables.FirstOrDefaultAsync(r => r.Email == email);
            if (responsable == null) return RedirectToAction("Login");

            var assignedIds = await _context.Respocollaborateurs
                .Where(r => r.NumRe == responsable.NumRe)
                .Select(r => r.NumCl)
                .ToListAsync();

            var query = _context.Collaborateurs.AsQueryable();

            // Search logic
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.NomCl.Contains(search) ||
                    c.PrenomCl.Contains(search) ||
                    c.Email.Contains(search));
            }

            // Filter logic
            if (filter == "Ajouter")
                query = query.Where(c => assignedIds.Contains(c.NumCl));
            else if (filter == "unassigned")
                query = query.Where(c => !assignedIds.Contains(c.NumCl));

            int totalCount = await query.CountAsync();

            var collaborateurs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CollaborateursModelView
                {
                    NumCl = c.NumCl,
                    NomCl = c.NomCl,
                    PrenomCl = c.PrenomCl,
                    Email = c.Email,
                    Tel = c.Tel,
                    Adresse = c.Adresse
                })
                .ToListAsync();

            return View(new PaginatedCollaborateursViewModel
            {
                Collaborateurs = collaborateurs,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                AssignedIds = assignedIds
            });
        }

        public async Task<IActionResult> ManageCollaborateursAjax(string search = "", string filter = "all")
        {
            var email = HttpContext.Session.GetString("email");
            var responsable = await _context.Responsables.FirstOrDefaultAsync(r => r.Email == email);

            var assignedIds = await _context.Respocollaborateurs
                .Where(r => r.NumRe == responsable.NumRe)
                .Select(r => r.NumCl)
                .ToListAsync();

            var query = _context.Collaborateurs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.NomCl.Contains(search) || c.PrenomCl.Contains(search) || c.Email.Contains(search));
            }

            if (filter == "assigned")
                query = query.Where(c => assignedIds.Contains(c.NumCl));
            else if (filter == "unassigned")
                query = query.Where(c => !assignedIds.Contains(c.NumCl));

            var model = await query
                .Select(c => new CollaborateursModelView
                {
                    NumCl = c.NumCl,
                    NomCl = c.NomCl,
                    PrenomCl = c.PrenomCl,
                    Email = c.Email,
                    Tel = c.Tel,
                    Adresse = c.Adresse
                })
                .ToListAsync();

            ViewBag.AssignedIds = assignedIds;
            return PartialView("_CollaborateurRowsPartial", model);
        }



    }
}