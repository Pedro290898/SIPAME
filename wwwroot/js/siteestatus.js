//funcion de datatable para listar
jQuery(document).ready(function ($) {
    $("#tbestatus").DataTable({
        stateSave: true,
        "ajax": {
            "url": "/Estatus/ObtenerTodos"
        },
        "columns": [
            { "data": "estatusId", "visible": false },
            { "data": "descripcionEstatus" },
            {
                "data": "estatusId",
                "render": function (data) {
                    return `
                         <div>
                             <a href="/Estatus/Create/${data}" class="btn btn-info" style="cursor: pointer" >Editar</a>
                    </div>`;
                }
            }
        ]
    });
})


$(document).ready(function () {
    var id = document.getElementById("EstatusId");
    if (id.value > 0) {
        $("#modaladdestatus").modal('show');
    }
});

function limpiaraddestatus() {
    var idEstatus = document.getElementById("EstatusId");
    var descripcionEstatus = document.getElementById("DescripcionEstatus");
    idEstatus.value = 0;
    descripcionEstatus.value = "";
    $(".text-danger").text("");
}