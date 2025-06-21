using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gestion_Des_prèneces.Models;
using Gestion_Des_prèneces.Models.ModelsView;
using Microsoft.AspNetCore.Http;

namespace Gestion_Des_prèneces.Controllers
{
    public class CollaborateursController : Controller
    {
        private readonly Gestion_Des_PrésencesContext _context;

        public CollaborateursController(Gestion_Des_PrésencesContext context)
        {
            _context = context;
        }
        public static int num;
        // GET: Collaborateurs
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("emailC") == null)
            {
                return View("Login");
            }

            var email = HttpContext.Session.GetString("emailC");
            var a = from col in _context.Collaborateurs
                    join miss in _context.Missions on
                    col.IdMission equals miss.IdMission
                    where col.Email == email
                    select new CollaborateursModelView
                    {
                        NumCl = col.NumCl,

                        NomCl = col.NomCl,
                        PrenomCl = col.PrenomCl,
                        Email = col.Email,
                        IdMission = miss.NomMission,
                        
                        Tel=col.Tel,
                        Adresse=col.Adresse,


                    };
           
            return View(await a.ToListAsync());
            // var gestion_Des_PrésencesContext = _context.Collaborateurs.Include(c => c.IdMissionNavigation).Include(c => c.NumReNavigation);
            //return View(await gestion_Des_PrésencesContext.ToListAsync());
        }

        //[HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public ActionResult Validate(Collaborateur admin)
        {
            var _colaborateur = _context.Collaborateurs.Where(s => s.Email == admin.Email);
            if (_colaborateur.Any())
            {
                if (_colaborateur.Where(s => s.MotDePasse == admin.MotDePasse).Any())
                {
                    HttpContext.Session.SetString("emailC", admin.Email);
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


        public ActionResult Bienvenu()
        {
            if (HttpContext.Session.GetString("emailC") == null)
            {
                return View("Login");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Loginout()
        {
            HttpContext.Session.Remove("emailC");
            return View("login");
        }

        // GET: Collaborateurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collaborateur = await _context.Collaborateurs
                .Include(c => c.IdMissionNavigation)
               
                .FirstOrDefaultAsync(m => m.NumCl == id);
            if (collaborateur == null)
            {
                return NotFound();
            }

            return View(collaborateur);
        }

        // GET: Collaborateurs/Create
        public IActionResult Create()
        {
            ViewData["IdMission"] = new SelectList(_context.Missions, "IdMission", "NomMission");
            return View();
        }

        // POST: Collaborateurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumCl,NomCl,PrenomCl,Adresse,Tel,Email,MotDePasse,IdMission,NumRe")] Collaborateur collaborateur)
        {
            Collaborateur col = new Collaborateur();
            var a = _context.Collaborateurs.Where(e => e.Email == col.Email);

            if (ModelState.IsValid)
            {
                _context.Add(collaborateur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdMission"] = new SelectList(_context.Missions, "IdMission", "IdMission", collaborateur.IdMission);
            return View(collaborateur);
        }
    
        
        // GET: Collaborateurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collaborateur = await _context.Collaborateurs.FindAsync(id);
            if (collaborateur == null)
            {
                return NotFound();
            }
            ViewData["IdMission"] = new SelectList(_context.Missions, "IdMission", "IdMission", collaborateur.IdMission);
            return View(collaborateur);
        }

        // POST: Collaborateurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumCl,NomCl,PrenomCl,Adresse,Tel,Email,MotDePasse,IdMission,NumRe")] Collaborateur collaborateur)
        {
            if (id != collaborateur.NumCl)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(collaborateur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollaborateurExists(collaborateur.NumCl))
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
            ViewData["IdMission"] = new SelectList(_context.Missions, "IdMission", "IdMission", collaborateur.IdMission);
            return View(collaborateur);
        }

        // GET: Collaborateurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collaborateur = await _context.Collaborateurs
                .Include(c => c.IdMissionNavigation)

                .FirstOrDefaultAsync(m => m.NumCl == id);
            if (collaborateur == null)
            {
                return NotFound();
            }

            return View(collaborateur);
        }

        // POST: Collaborateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collaborateur = await _context.Collaborateurs.FindAsync(id);
            _context.Collaborateurs.Remove(collaborateur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollaborateurExists(int id)
        {
            return _context.Collaborateurs.Any(e => e.NumCl == id);
        }
        //show the admis for the collaborateurs
        public async Task<IActionResult> ShowResponsables()
        {
            if (HttpContext.Session.GetString("emailC") == null)
            {
                return View("Login");
            }

            var email = HttpContext.Session.GetString("emailC");
            var a = from col in _context.Collaborateurs
                    join rc in _context.Respocollaborateurs
                    on col.NumCl equals rc.NumCl
                    join r in _context.Responsables on
                    rc.NumRe equals r.NumRe
                    where col.Email == email
                    select new RespoCollaborateurView
                    {
                        NumRe = r.NumRe,

                        NomRe = r.NomRe,
                        PrenomRe = r.PrenomRe,
                        Email = r.Email,
                        Tel = r.Tel,
                        Adresse = r.Adresse,

                    };

            return View(await a.ToListAsync());
        }
    }
}
