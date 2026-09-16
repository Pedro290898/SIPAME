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
    public class ClientesController : Controller
    {
        private readonly AppDBContext _context;

        public ClientesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> obtenertodosregistros()
        {
            // se agrega include para agregar las demas tablas relacionadas, en este caso estatus y zona
            var appDBContext = _context.Cliente.Include(c => c.Zona);
            var tdos = (await appDBContext.ToListAsync());
            return Json(new { data = tdos });
        }

        // GET: Clientes/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cliente = await _context.Cliente
        //        .Include(c => c.Estatus)
        //        .Include(c => c.Zona)
        //        .FirstOrDefaultAsync(m => m.ClienteId == id);
        //    if (cliente == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cliente);
        //}

        [HttpGet]
        public async Task<IActionResult> obtenerClienteporId(int id)
        {
            var cli = await _context.Cliente.FindAsync(id);
            // IMPORTANTE: El JSON debe devolver las propiedades tal cual se llaman en C#
            return Json(cli);
        }

        // GET: Clientes/Create
        public async Task<IActionResult> Create(int? id)
        {
            Cliente clien = new Cliente();

           // ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus");
            ViewData["ZonaId"] = new SelectList(_context.Zona, "ZonaId", "DescripcionZona");
            if (id == null)
            {
                return View(clien);
            }
            else
            {
                clien = await _context.Cliente.FindAsync(id);
                //ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus");
                ViewData["ZonaId"] = new SelectList(_context.Zona, "ZonaId", "DescripcionZona");
                return View(clien);
            }
            
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var elementexis = await _context.Cliente.AnyAsync(e => e.ClienteId != cliente.ClienteId 
                && e.Nombre.ToLower().Trim() == cliente.Nombre.ToLower().Trim()
                && e.Apellido.ToLower().Trim() == cliente.Apellido.ToLower().Trim()
                && e.TelContacto.ToLower().Trim() == cliente.TelContacto.ToLower().Trim());

                if (elementexis)
                {
                    ModelState.AddModelError("", "Ya existe un cliente con los mismos datos.");
                    //ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus");
                    ViewData["ZonaId"] = new SelectList(_context.Zona, "ZonaId", "DescripcionZona");
                    return View(cliente);
                }
                if (cliente.ClienteId == 0)
                {
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    //var estatusIdManual = Request.Form["EstatusId"];
                    //if (!string.IsNullOrEmpty(estatusIdManual))
                    //{
                    //    cliente.EstatusId = int.Parse(estatusIdManual);
                    //}
                    //cliente.Estatus = null;

                    var zonaIdManual = Request.Form["ZonaId"];
                    if (!string.IsNullOrEmpty(zonaIdManual))
                    {
                        cliente.ZonaId = int.Parse(zonaIdManual);
                    }
                    cliente.Zona = null;

                    _context.Entry(cliente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    ModelState.Clear();
                    return RedirectToAction(nameof(Create), new { id = (int?)null });

                }
            }
           // ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", cliente.EstatusId);
            ViewData["ZonaId"] = new SelectList(_context.Zona, "ZonaId", "DescripcionZona", cliente.ZonaId);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cliente = await _context.Cliente.FindAsync(id);
        //    if (cliente == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", cliente.EstatusId);
        //    ViewData["ZonaId"] = new SelectList(_context.Zona, "ZonaId", "DescripcionZona", cliente.ZonaId);
        //    return View(cliente);
        //}

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ClienteId,Nombre,Apellido,TelContacto,DireccionDomicilio,EstatusId,ZonaId")] Cliente cliente)
        //{
        //    if (id != cliente.ClienteId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(cliente);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ClienteExists(cliente.ClienteId))
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
        //    ViewData["EstatusId"] = new SelectList(_context.Estatus, "EstatusId", "DescripcionEstatus", cliente.EstatusId);
        //    ViewData["ZonaId"] = new SelectList(_context.Zona, "ZonaId", "DescripcionZona", cliente.ZonaId);
        //    return View(cliente);
        //}

        // GET: Clientes/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cliente = await _context.Cliente
        //        .Include(c => c.Estatus)
        //        .Include(c => c.Zona)
        //        .FirstOrDefaultAsync(m => m.ClienteId == id);
        //    if (cliente == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cliente);
        //}

        // POST: Clientes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var cliente = await _context.Cliente.FindAsync(id);
        //    if (cliente != null)
        //    {
        //        _context.Cliente.Remove(cliente);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.ClienteId == id);
        }
    }
}
