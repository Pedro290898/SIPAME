using System;
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
    public class ContratacionController : Controller
    {
        private readonly AppDBContext _context;

        public ContratacionController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Contratacion
        public async Task<IActionResult> obtenertodos()
        {
            var appDBContext = _context.contratacion.Include(c => c.Cliente).Include(c => c.Estatus).Include(c => c.Paquete);
            var tdos = (await appDBContext.ToListAsync());
            return Json(new { data = tdos});
        }

        // GET: Contratacion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contratacion = await _context.contratacion
                .Include(c => c.Cliente)
                .Include(c => c.Estatus)
                .Include(c => c.Paquete)
                .FirstOrDefaultAsync(m => m.ContratacionId == id);
            if (contratacion == null)
            {
                return NotFound();
            }

            return View(contratacion);
        }

        [HttpGet]
        public async Task<IActionResult> obtenerContratacionId(int id)
        {
            var contratacion = await _context.contratacion.FindAsync(id);
            // IMPORTANTE: El JSON debe devolver las propiedades tal cual se llaman en C#
            return Json(contratacion);
        }

        // GET: Contratacion/Create
        public async Task<IActionResult> Create(int? id)
        {
            Contratacion contra = new Contratacion();

            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Nombre");
            ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus");
            ViewData["PaqueteId"] = new SelectList(_context.Paquete, "PaqueteId", "DescripcionPaquete");
                if (id == null)
                {
                 return View(contra);
                }
                else 
                {
                    contra = await _context.contratacion.FindAsync(id);
                    ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Nombre");
                    ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus");
                    ViewData["PaqueteId"] = new SelectList(_context.Paquete, "PaqueteId", "DescripcionPaquete");
                    return View(contra);
            }
        }

        // POST: Contratacion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contratacion contratacion)
        {
            // 🔥 SOLUCIÓN AL MODELSTATE: Removemos las propiedades de navegación de la validación
            // para evitar que devuelva 'false' si el formulario solo envía los IDs numéricos.
            ModelState.Remove("Cliente");
            ModelState.Remove("Estatus");
            ModelState.Remove("Paquete");
            if (ModelState.IsValid)
            {
                var elementexis = await _context.contratacion.AnyAsync(e => e.ContratacionId != contratacion.ContratacionId
                && e.FechaContratacion == contratacion.FechaContratacion
                && e.TpoContratacion == contratacion.TpoContratacion
                && e.Modocontratacion == contratacion.Modocontratacion
                && e.ClienteId == contratacion.ClienteId
                && e.PaqueteId == contratacion.PaqueteId
                && e.EstatusId == contratacion.EstatusId);
                if (elementexis)
                {
                    ModelState.AddModelError("", "Ya existe un cliente con los mismos datos.");
                    ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Nombre", contratacion.ClienteId);
                    ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", contratacion.EstatusId);
                    ViewData["PaqueteId"] = new SelectList(_context.Paquete, "PaqueteId", "DescripcionPaquete", contratacion.PaqueteId);
                    return View(contratacion);
                }
                if (contratacion.ContratacionId == 0)
                {
                    _context.Add(contratacion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    var estatusIdManual = Request.Form["EstatusId"];
                    if (!string.IsNullOrEmpty(estatusIdManual))
                    {
                        contratacion.EstatusId = int.Parse(estatusIdManual);
                    }
                    contratacion.Estatus = null;

                    var clienteIdManual = Request.Form["ClienteId"];
                    if (!string.IsNullOrEmpty(clienteIdManual))
                    {
                        contratacion.ClienteId = int.Parse(clienteIdManual);
                    }
                    contratacion.Cliente = null;

                    var paqueteIdManual = Request.Form["PaqueteId"];
                    if (!string.IsNullOrEmpty(paqueteIdManual))
                    {
                        contratacion.PaqueteId = int.Parse(paqueteIdManual);
                    }
                    contratacion.Paquete = null;

                    _context.Entry(contratacion).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    ModelState.Clear();
                    return RedirectToAction(nameof(Create), new {id = (int?)null});
                }
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Nombre", contratacion.ClienteId);
            ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", contratacion.EstatusId);
            ViewData["PaqueteId"] = new SelectList(_context.Paquete, "PaqueteId", "DescripcionPaquete", contratacion.PaqueteId);
            return View(contratacion);
        }

        // GET: Contratacion/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var contratacion = await _context.contratacion.FindAsync(id);
        //    if (contratacion == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Apellido", contratacion.ClienteId);
        //    ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", contratacion.EstatusId);
        //    ViewData["PaqueteId"] = new SelectList(_context.Paquete, "PaqueteId", "DescripcionPaquete", contratacion.PaqueteId);
        //    return View(contratacion);
        //}

        // POST: Contratacion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ContratacionId,FechaContratacion,TpoContratacion,ClienteId,PaqueteId,EstatusId")] Contratacion contratacion)
        //{
        //    if (id != contratacion.ContratacionId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(contratacion);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ContratacionExists(contratacion.ContratacionId))
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
        //    ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Apellido", contratacion.ClienteId);
        //    ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", contratacion.EstatusId);
        //    ViewData["PaqueteId"] = new SelectList(_context.Paquete, "PaqueteId", "DescripcionPaquete", contratacion.PaqueteId);
        //    return View(contratacion);
        //}

        // GET: Contratacion/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var contratacion = await _context.contratacion
        //        .Include(c => c.Cliente)
        //        .Include(c => c.Estatus)
        //        .Include(c => c.Paquete)
        //        .FirstOrDefaultAsync(m => m.ContratacionId == id);
        //    if (contratacion == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(contratacion);
        //}

        //// POST: Contratacion/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var contratacion = await _context.contratacion.FindAsync(id);
        //    if (contratacion != null)
        //    {
        //        _context.contratacion.Remove(contratacion);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ContratacionExists(int id)
        {
            return _context.contratacion.Any(e => e.ContratacionId == id);
        }
    }
}
