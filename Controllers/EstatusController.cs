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
    public class EstatusController : Controller
    {
        private readonly AppDBContext _context;

        public EstatusController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Estatus
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = (await _context.Estatus.ToListAsync());
            return Json(new { data = todos});
        }


        // GET: Estatus/Create
        public async Task<ActionResult> Create(int? id)
        {
            Estatus est = new Estatus();
            if (id == null)
            {
                return View(est);

            }
            else
            {
                est = await _context.Estatus.FindAsync(id);
                return View(est);
            }
        }

        // POST: Estatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Estatus estatus)
        {

            if (ModelState.IsValid)
            {
                var existelement = _context.Estatus.Any(d => d.DescripcionEstatus.ToLower().Trim() == estatus.DescripcionEstatus.ToLower().Trim());
                if (existelement)
                {
                    ModelState.AddModelError("", "Lo sentimos el registro ya existe");
                    return View(estatus);
                }
                if (estatus.EstatusId == 0)
                {
                    _context.Add(estatus);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    _context.Estatus.Update(estatus);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create), new { id = 0 });
                }
            }
            return View(estatus);
        }

        // GET: Estatus/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var estatus = await _context.Estatus.FindAsync(id);
        //    if (estatus == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(estatus);
        //}

        //// POST: Estatus/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("EstatusId,DescripcionEstatus")] Estatus estatus)
        //{
        //    if (id != estatus.EstatusId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(estatus);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EstatusExists(estatus.EstatusId))
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
        //    return View(estatus);
        //}

        // GET: Estatus/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var estatus = await _context.Estatus
        //        .FirstOrDefaultAsync(m => m.EstatusId == id);
        //    if (estatus == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(estatus);
        //}

        //// POST: Estatus/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var estatus = await _context.Estatus.FindAsync(id);
        //    if (estatus != null)
        //    {
        //        _context.Estatus.Remove(estatus);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool EstatusExists(int id)
        {
            return _context.Estatus.Any(e => e.EstatusId == id);
        }
    }
}
