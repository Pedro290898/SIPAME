using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIPAME.Data;
using SIPAME.Models;

namespace SIPAME.Controllers
{
    public class PaquetesController : Controller
    {
        private readonly AppDBContext _context;

        public PaquetesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Paquetes
        public async Task<IActionResult> obtenerTodospaq()
        {
            var appDBContext = _context.Paquete.Include(p => p.Estatus);
            var tdos = (await appDBContext.ToListAsync());
            return Json(new { data = tdos });
            //return View(tdos);
        }

        // GET: Paquetes/Details/por id
        [HttpGet]
        public async Task<IActionResult> obtenerPaquetePorId(int id)
        {
            var paq = await _context.Paquete.FindAsync(id);
            // IMPORTANTE: El JSON debe devolver las propiedades tal cual se llaman en C#
            return Json(paq);
        }

        // GET: Paquetes/Create
        public async Task<ActionResult> Create(int? id)
        {
            Paquete paq = new Paquete();
            ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", paq.EstatusId);
            if (id == null)
            {
                return View(paq);
            }
            else {
                paq = await _context.Paquete.FindAsync(id);
                ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", paq.EstatusId);
                return View(paq);
            }
        }

        // POST: Paquetes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paquete paquete)
        {
            if (ModelState.IsValid)
            {
                var existelement = _context.Paquete.Any(p => p.DescripcionPaquete.ToLower().Trim() == paquete.DescripcionPaquete.ToLower().Trim() && p.EstatusId ==paquete.EstatusId);
                /*&& p.PaqueteId == p.PaqueteId);*/
                if (existelement)
                {
                    ModelState.AddModelError(" ", "Lo sentimos el registro ya existe");
                    ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", paquete.EstatusId);
                    return View(paquete);
                }
                if (paquete.PaqueteId == 0) 
                {
                    _context.Add(paquete);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    // 1. Leemos el ID del estatus directamente del formulario
                    var estatusIdManual = Request.Form["EstatusId"];

                    // 2. Si el formulario trae algo, se lo asignamos a la fuerza
                    if (!string.IsNullOrEmpty(estatusIdManual))
                    {
                        paquete.EstatusId = int.Parse(estatusIdManual);
                    }
                    paquete.Estatus = null;
                    //_context.Paquete.Update(paquete);
                    _context.Entry(paquete).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    ModelState.Clear();
                    //await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create), new {id = (int?) null});
                }
                
            }
            ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", paquete.EstatusId);
            return View(paquete);
        }

        // GET: Paquetes/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var paquete = await _context.Paquete.FindAsync(id);
        //    if (paquete == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", paquete.EstatusId);
        //    return View(paquete);
        //}

        //// POST: Paquetes/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("PaqueteId,DescripcionPaquete,EstatusId")] Paquete paquete)
        //{
        //    if (id != paquete.PaqueteId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(paquete);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PaqueteExists(paquete.PaqueteId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", paquete.EstatusId);
        //    return View(paquete);
        //}

        //// GET: Paquetes/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var paquete = await _context.Paquete
        //        .Include(p => p.Estatus)
        //        .FirstOrDefaultAsync(m => m.PaqueteId == id);
        //    if (paquete == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(paquete);
        //}

        //// POST: Paquetes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var paquete = await _context.Paquete.FindAsync(id);
        //    if (paquete != null)
        //    {
        //        _context.Paquete.Remove(paquete);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool PaqueteExists(int id)
        {
            return _context.Paquete.Any(e => e.PaqueteId == id);
        }
    }
}
