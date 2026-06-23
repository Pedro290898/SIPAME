jQuery(document).ready(function ($) {
    $("#tbcontratacion").DataTable({
        stateSave: true,
        "ajax": {
            "url": "/Contratacion/obtenertodos"
        },
        "columns": [
            { "data": "contratacionId", "visible": true },
            //{ "data": "cliente.nombre" },

            {
                "data": "cliente",
                "render": function (data, type, row) {
                    // Si el objeto cliente viene nulo o vacío, mostramos un mensaje de alerta
                    if (!data) return '<span class="text-muted">Sin Cliente</span>';

                    // Obtenemos el nombre y manejamos el apellido por si viene nulo
                    var nombre = data.nombre ? data.nombre : "";
                    var apellido = data.apellido ? data.apellido : ""; // ⚠️ Cambia "apellido" por el nombre real de tu campo si es necesario

                    // Unimos ambos textos con un espacio limpio
                    var nombreCompleto = `${nombre} ${apellido}`.trim();

                    return nombreCompleto ? nombreCompleto : "Cliente sin nombre";
                }
            },


            //{ "data": "fechaContratacion" },

            {
                "data": "fechaContratacion",
                "render": function (data) {
                    if (!data) return "Sin fecha";

                    // Separamos la fecha para quitar la 'T00:00:00'
                    var fechaLimpia = data.split('T')[0];
                    var partes = fechaLimpia.split('-'); // partes[0]=Año, partes[1]=Mes, partes[2]=Día

                    // Listado de meses abreviados (empezamos con vacío en el índice 0 para que coincida el número del mes)
                    var meses = ["", "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"];

                    // Convertimos el texto del mes a número entero (ej: "03" pasa a 3)
                    var numeroMes = parseInt(partes[1], 10);
                    var nombreMes = meses[numeroMes];

                    // Retornamos el formato final: Día/Mes/Año
                    return `${partes[2]}/${nombreMes}/${partes[0]}`;
                }
            },


            { "data": "paquete.descripcionPaquete" },
            { "data": "tpoContratacion" },
            { "data": "modocontratacion" },
            { "data": "estatus.descripcionEstatus" },
            {
                "data": "contratacionId",
                "render": function (data) {
                    return `<button type="button" class="btn btn-info" onclick="editarContratacion(${data})">Editar</button>`;
                }
            }
        ]
    });
});

function editarContratacion(id) {
    $.get("/Contratacion/obtenerContratacionId?id=" + id, function (data) {
        if (data != null) {
            // Rellenamos el ID oculto para que el controlador entre al ELSE
            $("#ContratacionId").val(data.contratacionId);

            // Rellenamos los demás campos
            $("#FechaContratacion").val(data.fechaContratacion);
            $("#TpoContratacion").val(data.tpoContratacion);
            $("#Modocontratacion").val(data.modocontratacion);
            $("#ClienteId").val(data.clienteId);
            $("#PaqueteId").val(data.paqueteId);
            $("#EstatusId").val(data.estatusId);


            if (data.fechaContratacion) {
                // Separamos por la 'T' para quitar la hora (de "2026-03-19T00:00:00" a "2026-03-19")
                var fechaParaInput = data.fechaContratacion.split('T')[0];

                // Le pasamos solo los 10 caracteres limpios (AAAA-MM-DD) que el input exige
                $("#FechaContratacion").val(fechaParaInput);
            } else {
                $("#FechaContratacion").val(""); // Por si acaso viene nula
            }


            // Abrimos el modal
            $("#modalcontratacion").modal('show');
        }
    });
}


//funciona para abrir modal
$(document).ready(function () {
    var idElement = document.getElementById("ContratacionId");
    // Validamos que el elemento exista antes de leer su .value
    if (idElement && idElement.value > 0) {
        $("#modalcontratacion").modal('show');
    }
});

function limpiaraddcontratacion() {
    var idContratacion = document.getElementById("ContratacionId");
    var fechaContratacion = document.getElementById("FechaContratacion");
    var tpoContratacion = document.getElementById("TpoContratacion");
    var modocontratacion = document.getElementById("Modocontratacion");
    var clienteId = document.getElementById("ClienteId");
    var paqueteId = document.getElementById("PaqueteId");
    var estatusId = document.getElementById("EstatusId");

    // Agregamos validación para evitar el error de "undefined"
    if (idContratacion) idContratacion.value = 0;
    if (fechaContratacion) fechaContratacion.value = "";
    if (tpoContratacion) tpoContratacion.value = "";
    if (modocontratacion) modocontratacion.value = "";
    if (clienteId) clienteId.value = 0;
    if (paqueteId) paqueteId.value = 0;
    if (estatusId) estatusId.value = 0;
    /*if (zonaCliente) zonaCliente.value = "";*/
    $(".text-danger").text("");
}