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
    public class ZonasController : Controller
    {
        private readonly AppDBContext _context;

        public ZonasController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Zonas
        public async Task<IActionResult> obtenertodos()
        {
            //se mete todas las consultas en una sola variable
            var tdos = await _context.Zona.ToListAsync();
            //retorna un json y los coloca en data para
            //mandarlos a la vista en un data table con ajax
            return Json (new {data =tdos});
        }

        // GET: Zonas/Create
        public async Task<IActionResult> Create(int ? id)
        {
            Zona zon = new Zona();
            if (id == null)
            {
                return View(zon);
            }
            else
            {
                zon = await _context.Zona.FindAsync(id);
                return View(zon);
            }
                
        }

        // POST: Zonas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Zona zona)
        {
            if (ModelState.IsValid)
            {
                var exist = _context.Zona.Any(z => z.DescripcionZona.ToLower().Trim() == zona.DescripcionZona.ToLower().Trim());
                if (exist)
                {
                    ModelState.AddModelError("", "lo sentimos registro ya existe");
                    return View(zona);
                }
                else if(zona.ZonaId == 0)
                {
                _context.Add(zona);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                else
                {
                    _context.Update(zona);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create), new { id = 0 });
                }
            }
            return View(zona);
        }

        // GET: Zonas/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var zona = await _context.Zona.FindAsync(id);
        //    if (zona == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(zona);
        //}

        //// POST: Zonas/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ZonaId,DescripcionZona")] Zona zona)
        //{
        //    if (id != zona.ZonaId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(zona);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ZonaExists(zona.ZonaId))
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
        //    return View(zona);
        //}

        //// GET: Zonas/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var zona = await _context.Zona
        //        .FirstOrDefaultAsync(m => m.ZonaId == id);
        //    if (zona == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(zona);
        //}

        //// POST: Zonas/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var zona = await _context.Zona.FindAsync(id);
        //    if (zona != null)
        //    {
        //        _context.Zona.Remove(zona);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ZonaExists(int id)
        {
            return _context.Zona.Any(e => e.ZonaId == id);
        }
    }
}
