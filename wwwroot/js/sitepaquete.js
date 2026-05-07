//import { data } from "jquery";

//funcion de data table
jQuery(document).ready(function ($) {
    $("#tbpaquete").DataTable({
        stateSave: true,
        "ajax": {
            "url": "/Paquetes/obtenerTodosPaq"
        },
        "columns": [
            { "data": "paqueteId", "visible": false },
            { "data": "descripcionPaquete" },
            { "data": "estatus.descripcionEstatus" },
            {
                "data": "paqueteId",
                "render": function (data) {
                    return `<button type="button" class="btn btn-info" onclick="editarPaquete(${data})">Editar</button>`;
                }
            }
        ]
    });
});  

//funcion editar paquete

function editarPaquete(id) {
    $.get("/Paquetes/obtenerPaquetePorId?id=" + id, function (data) {
        if (data != null) {
            // Rellenamos el ID oculto para que el controlador entre al ELSE
            $("#PaqueteId").val(data.paqueteId);

            // Rellenamos los demás campos
            $("#DescripcionPaquete").val(data.descripcionPaquete);
            $("#EstatusId").val(data.estatusId);

            // Abrimos el modal
            $("#modalpaquete").modal('show');
        }
    });
}

//function editarpaquete(id) {
//    $.get("Paquetes/obtenerPaquetePorId=" + id, function (data) {
//        if (data!null)
//    {
//        $("#PaqueteId").val(data.idPaquete)
//        }
//    })
//}

//funciona para abrir modal
    $(document).ready(function () {
        var idElement = document.getElementById("PaqueteId");
        // Validamos que el elemento exista antes de leer su .value
        if (idElement && idElement.value > 0) {
            $("#modalpaquete").modal('show');
        }
    });

    function limpiaraddpaquete() {
        var idPaquete = document.getElementById("PaqueteId");
        var descripcionPaquete = document.getElementById("DescripcionPaquete");

        // Agregamos validación para evitar el error de "undefined"
        if (idPaquete) idPaquete.value = 0;
        if (descripcionPaquete) descripcionPaquete.value = "";

        $(".text-danger").text("");
    }



    //function limpiaradpaquete() {
    //    var idPaquete = document.getElementById("PaqueteId");
    //    var descripcionPaquete = document.getElementById("DescripcionPaquete");
    //    idPaquete.value = 0;
    //    descripcionPaquete.value = "";
    //    $(".text-danger").text("");
    //}
//}