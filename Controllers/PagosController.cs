
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIPAME.Data;
using SIPAME.Models;

public class PagosController : Controller
{
    private readonly AppDBContext _context;

    public PagosController(AppDBContext context)
    {
        _context = context;
    }

    // GET: PAGOS
    public async Task<IActionResult> Index()    
    {
        //return View(await _context.Pago.ToListAsync());
        // Le indicamos explícitamente a Entity Framework que cargue al Cliente 
        // y a la Contratación asociada a cada pago antes de mandar la lista a la vista.
        var listaPagos = await _context.Pago
            .Include(p => p.Cliente)
            .Include(p => p.Contratacion)
            .ToListAsync();

        return View(listaPagos);
    }

    // GET: PAGOS/Details/5
    public async Task<IActionResult> Details(int? pagoid)
    {
        if (pagoid == null)
        {
            return NotFound();
        }

        var pago = await _context.Pago
            .FirstOrDefaultAsync(m => m.PagoId == pagoid);
        if (pago == null)
        {
            return NotFound();
        }

        return View(pago);
    }

    // GET: PAGOS/Create
    public IActionResult Create()
    {
        // Unimos Nombre y Apellido para que se vea completo en la vista
        var listaClientes = _context.Cliente
            .Select(c => new {
                ClienteId = c.ClienteId,
                NombreCompleto = c.Nombre + " " + c.Apellido
            }).ToList();

        ViewData["ClienteId"] = new SelectList(listaClientes, "ClienteId", "NombreCompleto");

        // Al cargar por primera vez, el select de contrataciones se inicializa completamente vacío
        ViewData["ContratacionId"] = new SelectList(Enumerable.Empty<SelectListItem>());

        return View();
    }

    // NUEVO MÉTODO: Endpoint AJAX para traer las contrataciones filtradas
    [HttpGet]
    public async Task<JsonResult> GetContratacionesPorCliente(int clienteId)
    {
        var contrataciones = await _context.contratacion
            .Include(c => c.Estatus)
            .Where(c => c.ClienteId == clienteId && c.Estatus !=null && c.Estatus.DescripcionEstatus == "Activo")
            .Select(c => new {
                value = c.ContratacionId,
                text = "Contratación #" + c.ContratacionId // Puedes cambiar el texto si tienes otro campo explicativo
            })
            .ToListAsync();

        return Json(contrataciones);
    }

    // POST: PAGOS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Pago pago)
    {
        if (ModelState.IsValid)
        {
            _context.Add(pago);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Si el modelo falla por validaciones, volvemos a armar las listas para no perder la selección
        var listaClientes = _context.Cliente
            .Select(c => new {
                ClienteId = c.ClienteId,
                NombreCompleto = c.Nombre + " " + c.Apellido
            }).ToList();

        ViewData["ClienteId"] = new SelectList(listaClientes, "ClienteId", "NombreCompleto", pago.ClienteId);

        var contratacionesFiltradas = await _context.contratacion
            .Where(c => c.ClienteId == pago.ClienteId)
            .ToListAsync();

        ViewData["ContratacionId"] = new SelectList(contratacionesFiltradas, "ContratacionId", "ContratacionId", pago.ContratacionId);

        return View(pago);
    }

    [HttpGet]
    public async Task<JsonResult> ObtenerFechasPeriodo(int contratacionId)
    {
        var contratacion = await _context.contratacion
            .FirstOrDefaultAsync(c => c.ContratacionId == contratacionId);

        if (contratacion == null)
            return Json(new { success = false, message = "Contratación no encontrada" });

        var ultimoPago = await _context.Pago
            .Where(p => p.ContratacionId == contratacionId)
            .OrderByDescending(p => p.FechaFin)
            .FirstOrDefaultAsync();

        DateTime fechaInicioCalculada = ultimoPago != null ? ultimoPago.FechaFin.AddDays(1) : contratacion.FechaContratacion;
        DateTime fechaFinCalculada = fechaInicioCalculada.AddMonths(1).AddDays(-1);

        return Json(new
        {
            success = true,
            fechaInicio = fechaInicioCalculada.ToString("yyyy-MM-dd"),
            fechaFin = fechaFinCalculada.ToString("yyyy-MM-dd")
        });
    }



    //// GET: PAGOS/Create
    //public IActionResult Create()
    //{
    //    ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Nombre");
    //    // Al cargar por primera vez, el select de contrataciones estará vacío
    //    ViewData["ContratacionId"] = new SelectList(Enumerable.Empty<SelectListItem>());
    //    return View();
    //    //ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Nombre");
    //    //return View();
    //}

    //// POST: PAGOS/Create
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create(Pago pago)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        _context.Add(pago);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    // Si el modelo no es válido, vuelve a llenar los select con los datos seleccionados para no perderlos
    //    ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Nombre", pago.ClienteId);

    //    var contratacionesFiltradas = await _context.contratacion.Where(c => c.ClienteId == pago.ClienteId).ToListAsync();
    //    ViewData["ContratacionId"] = new SelectList(contratacionesFiltradas, "ContratacionId", "ContratacionId", pago.ContratacionId);



    //    return View(pago);
    //    //ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Nombre");
    //    //ViewData["ContratacionId"] = new SelectList(_context.contratacion, "ContratacionId", "ContratacionId");
    //    //if (ModelState.IsValid)
    //    //{
    //    //    _context.Add(pago);
    //    //    await _context.SaveChangesAsync();
    //    //    return RedirectToAction(nameof(Index));
    //    //}
    //    //return View(pago);
    //}

    // GET: PAGOS/Edit/5
    public async Task<IActionResult> Edit(int? pagoid)
    {
        if (pagoid == null)
        {
            return NotFound();
        }

        var pago = await _context.Pago.FindAsync(pagoid);
        if (pago == null)
        {
            return NotFound();
        }
        return View(pago);
    }

    // POST: PAGOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? pagoid, [Bind("PagoId,MontoPago,FormaPago,FechaPago,FechaInicio,FechaFin,ClienteId,Cliente,ContratacionId,Contratacion")] Pago pago)
    {
        if (pagoid != pago.PagoId)
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
        return View(pago);
    }

    // GET: PAGOS/Delete/5
    public async Task<IActionResult> Delete(int? pagoid)
    {
        if (pagoid == null)
        {
            return NotFound();
        }

        var pago = await _context.Pago
            .FirstOrDefaultAsync(m => m.PagoId == pagoid);
        if (pago == null)
        {
            return NotFound();
        }

        return View(pago);
    }

    // POST: PAGOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? pagoid)
    {
        var pago = await _context.Pago.FindAsync(pagoid);
        if (pago != null)
        {
            _context.Pago.Remove(pago);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<JsonResult> ObtenerContratacionesPorCliente(int clienteId)
    {
        // Buscamos las contrataciones del cliente e incluimos el paquete para que sea más descriptivo
        var contrataciones = await _context.contratacion // Asegúrate de si es 'Contratacion' o 'contratacion' según tu DbContext
            .Include(c => c.Paquete)
            .Where(c => c.ClienteId == clienteId)
            .Select(c => new
            {
                id = c.ContratacionId,
                // Texto que verá el usuario en el select: "Fibra - Renta (Paquete Básico)"
                texto = $"{c.TpoContratacion} - {c.Modocontratacion} " + (c.Paquete != null ? $"({c.Paquete.DescripcionPaquete})" : "")
            })
            .ToListAsync();

        return Json(contrataciones);
    }

    private bool PagoExists(int? pagoid)
    {
        return _context.Pago.Any(e => e.PagoId == pagoid);
    }
}
