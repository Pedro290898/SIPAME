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
    public class ContratacionsController : Controller
    {
        private readonly AppDBContext _context;

        public ContratacionsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Contratacions
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.contratacion.Include(c => c.Cliente).Include(c => c.Estatus).Include(c => c.Paquete);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Contratacions/Details/5
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

        // GET: Contratacions/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Apellido");
            ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus");
            ViewData["PaqueteId"] = new SelectList(_context.Paquete, "PaqueteId", "DescripcionPaquete");
            return View();
        }

        // POST: Contratacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContratacionId,FechaContratacion,TpoContratacion,DescripcionPaquete,ClienteId,PaqueteId,EstatusId")] Contratacion contratacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contratacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Apellido", contratacion.ClienteId);
            ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", contratacion.EstatusId);
            ViewData["PaqueteId"] = new SelectList(_context.Paquete, "PaqueteId", "DescripcionPaquete", contratacion.PaqueteId);
            return View(contratacion);
        }

        // GET: Contratacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contratacion = await _context.contratacion.FindAsync(id);
            if (contratacion == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Apellido", contratacion.ClienteId);
            ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", contratacion.EstatusId);
            ViewData["PaqueteId"] = new SelectList(_context.Paquete, "PaqueteId", "DescripcionPaquete", contratacion.PaqueteId);
            return View(contratacion);
        }

        // POST: Contratacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContratacionId,FechaContratacion,TpoContratacion,DescripcionPaquete,ClienteId,PaqueteId,EstatusId")] Contratacion contratacion)
        {
            if (id != contratacion.ContratacionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contratacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContratacionExists(contratacion.ContratacionId))
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
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Apellido", contratacion.ClienteId);
            ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", contratacion.EstatusId);
            ViewData["PaqueteId"] = new SelectList(_context.Paquete, "PaqueteId", "DescripcionPaquete", contratacion.PaqueteId);
            return View(contratacion);
        }

        // GET: Contratacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Contratacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contratacion = await _context.contratacion.FindAsync(id);
            if (contratacion != null)
            {
                _context.contratacion.Remove(contratacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContratacionExists(int id)
        {
            return _context.contratacion.Any(e => e.ContratacionId == id);
        }
    }
}
