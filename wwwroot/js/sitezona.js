jQuery(document).ready(function ($) {
    $("#tbzonas").DataTable({
        stateSave: true,
        "ajax": {
            "url": "/Zonas/ObtenerTodos"
        },
        "columns": [
            { "data": "zonaId","visible": false },
            { "data": "descripcionZona" },
            {
                "data": "zonaId",
                "render": function (data) {
                    return `
                         <div>
                             <a href="/Zonas/Create/${data}" class="btn btn-info" style="cursor: pointer" >Editar</a>
                    </div>`;
                }
            }
        ]
    });
})

$(document).ready(function () {
    var id = document.getElementById("ZonaId");
    if (id.value > 0) {
        $("#modaladdzona").modal('show');
    }
});

function limpiaraddzona() {
    var idZona = document.getElementById("ZonaId");
    var descripcionZona = document.getElementById("DescripcionZona");
    idZona.value = 0;
    descripcionZona.value = "";
    $(".text-danger").text("");
}