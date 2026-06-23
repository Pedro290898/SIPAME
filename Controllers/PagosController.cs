using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SIPAME.Data;
using SIPAME.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIPAME.Controllers
{
    public class PagosController : Controller
    {
        private readonly AppDBContext _context;

        public PagosController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Pagos
        public async Task<IActionResult> obtenertodos()
        {
            var appDBContext = _context.Pago.Include(p => p.Cliente).Include(p => p.Contratacion);
            var todos = (await appDBContext.ToListAsync());
            return Json(new { data = todos });
        }

        public async Task<IActionResult> obtenerPagoporId(int id)
        {
            var pag = await _context.Pago.FindAsync(id);
            // IMPORTANTE: El JSON debe devolver las propiedades tal cual se llaman en C#
            return Json(pag);
        }

        // GET: Pagos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pago
                .Include(p => p.Cliente)
                .Include(p => p.Contratacion)
                .FirstOrDefaultAsync(m => m.PagoId == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pagos/Create
        public async Task<IActionResult>Create(int? id)
        {
            Pago pag = new Pago();

            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", ("Nombre"+ " " + "Apellido"));
            ViewData["ContratacionId"] = new SelectList(_context.contratacion, "ContratacionId", "ContratacionId");

            if (id == null)
            {
                return View(pag);
                //pag = await _context.Pago.FindAsync(id);
                ////return View(pag);
                //if (pag == null)
                //{
                //    return NotFound();
                //}
            }
            else
            {
                pag = await _context.Pago.FindAsync(id);
                ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "NombreCompleto");
                ViewData["ContratacionId"] = new SelectList(_context.contratacion, "ContratacionId", "ContratacionId");
                return View(pag);
            }
            //return View();
        }

        // POST: Pagos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pago pago)
        {

            //SOLUCIÓN AL MODELSTATE: Removemos las propiedades de navegación de la validación
            // para evitar que devuelva 'false' si el formulario solo envía los IDs numéricos.
            ModelState.Remove("Cliente");
            ModelState.Remove("Contratacion");
            {
                if (!ModelState.IsValid) return View(pago);

                // 🔥 VALIDACIÓN MAESTRA: Protege contra duplicados y permite adelantos
                var periodoDuplicado = await _context.Pago.AnyAsync(e =>
                    e.PagoId != pago.PagoId                    // Ignora si es edición
                    && e.ContratacionId == pago.ContratacionId // Valida el contrato específico
                    && e.FechaInicio.Date == pago.FechaInicio.Date // Compara día de inicio de cobertura
                    && e.FechaFin.Date == pago.FechaFin.Date       // Compara día de fin de cobertura
                );

                if (periodoDuplicado)
                {
                    // Preparamos las listas para los dropdowns usando la propiedad calculada
                    // Esto se procesa en el servidor de volada
                    ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "NombreCompleto");

                    ViewData["ContratacionId"] = new SelectList(_context.contratacion, "ContratacionId", "ContratacionId");
                    return View(pago); // Detiene el proceso y muestra la alerta en la vista de periodo duplicado
                }

                // 🔄 LÓGICA UNIFICADA: Crear o Editar
                if (pago.PagoId == 0)
                {
                    // Es un pago nuevo
                    _context.Add(pago);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    // Es una actualización de un pago existente
                    try
                    {
                        _context.Update(pago);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_context.Pago.Any(e => e.PagoId == pago.PagoId)) return NotFound();
                        else throw;
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: Pagos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pago.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Apellido", pago.ClienteId);
            ViewData["ContratacionId"] = new SelectList(_context.contratacion, "ContratacionId", "TpoContratacion", pago.ContratacionId);
            return View(pago);
        }

        // POST: Pagos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PagoId,MontoPago,TpoContratacion,FormaPago,FechaPago,FechaInicio,FechaFin,ClienteId,ContratacionId")] Pago pago)
        {
            if (id != pago.PagoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.PagoId))
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
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Apellido", pago.ClienteId);
            ViewData["ContratacionId"] = new SelectList(_context.contratacion, "ContratacionId", "TpoContratacion", pago.ContratacionId);
            return View(pago);
        }

        // GET: Pagos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pago
                .Include(p => p.Cliente)
                .Include(p => p.Contratacion)
                .FirstOrDefaultAsync(m => m.PagoId == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pago.FindAsync(id);
            if (pago != null)
            {
                _context.Pago.Remove(pago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public bool PagoExists(int id)
        {
            return _context.Pago.Any(e => e.PagoId == id);
        }
    }
}
