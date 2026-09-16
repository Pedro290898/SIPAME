jQuery(document).ready(function ($) {
    $("#tbcliente").DataTable({
        stateSave: true,
        "ajax": {
            "url": "/Clientes/obtenertodosregistros"
        },
        "columns": [
            { "data": "clienteId", "visible": false },
            { "data": "nombre" },
            { "data": "apellido" },
            { "data": "telContacto" },
            { "data": "direccionDomicilio" },
           // { "data": "estatus.descripcionEstatus" },
            { "data": "zona.descripcionZona" },
            {
                "data": "clienteId",
                "render": function (data) {
                    return `<button type="button" class="btn btn-info" onclick="editarCliente(${data})">Editar</button>`;
                }
            }
        ]
    });
});  

//funcion editar cliente

function editarCliente(id) {
    $.get("/Clientes/obtenerClienteporId?id=" + id, function (data) {
        if (data != null) {
            // Rellenamos el ID oculto para que el controlador entre al ELSE
            $("#ClienteId").val(data.clienteId);

            // Rellenamos los demás campos
            $("#NombreCliente").val(data.nombre);
            $("#ApellidoCliente").val(data.apellido);
            $("#TelContactoCliente").val(data.telContacto);
            $("#DireccionDomicilioCliente").val(data.direccionDomicilio);
           // $("#EstatusId").val(data.estatusId);
            $("#ZonaId").val(data.zonaId);

            // Abrimos el modal
            $("#modalcliente").modal('show');
        }
    });
}

//funciona para abrir modal
$(document).ready(function () {
    var idElement = document.getElementById("ClienteId");
    // Validamos que el elemento exista antes de leer su .value
    if (idElement && idElement.value > 0) {
        $("#modalcliente").modal('show');
    }
});

function limpiaraddcliente() {
    var idCliente = document.getElementById("ClienteId");
    var nombreCliente = document.getElementById("NombreCliente");
    var apellidoCliente = document.getElementById("ApellidoCliente");
    var telContactoCliente = document.getElementById("TelContactoCliente");
    var direccionDomicilioCliente = document.getElementById("DireccionDomicilioCliente");

    // Agregamos validación para evitar el error de "undefined"
    if (idCliente) idCliente.value = 0;
    if (nombreCliente) nombreCliente.value = "";
    if (apellidoCliente) apellidoCliente.value = "";
    if (telContactoCliente) telContactoCliente.value = "";
    if (direccionDomicilioCliente) direccionDomicilioCliente.value = "";
   // var estatusCliente = document.getElementById("EstatusId");
    var zonaCliente = document.getElementById("ZonaId");
    //if (estatusCliente) estatusCliente.value = "";
    if (zonaCliente) zonaCliente.value = "";
    $(".text-danger").text("");
}