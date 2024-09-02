/*!
    * Start Bootstrap - SB Admin v7.0.7 (https://startbootstrap.com/template/sb-admin)
    * Copyright 2013-2023 Start Bootstrap
    * Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-sb-admin/blob/master/LICENSE)
    */
    // 
// Scripts
// 

window.addEventListener('DOMContentLoaded', event => {

    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

});


//cargar formulario pruebas tyt 
let formTyt = document.querySelector('#pruebatyt');
let estadotyt = document.querySelector('#cboEstadoPruebatyt');

window.addEventListener('load', () => {
    var opciones = estadotyt.options[estadotyt.selectedIndex];

    if (opciones.text.toLowerCase().trim() == "inscrito") {
        formTyt.style.display = "flex";
    } else {
        formTyt.style.display = "none";
    }
});
estadotyt.addEventListener('change', () => {
    var opciones = estadotyt.options[estadotyt.selectedIndex];

    if (opciones.text.toLowerCase().trim() == "inscrito") {
        formTyt.style.display = "flex";
    } else {
        formTyt.style.display = "none";
    }
});

//cambios en el formulario dependiendo de la modalidad

let cboModalidad = document.querySelector('#cboModalidad');
let inputNitEmpresa = document.querySelector('#inputEmpresa');
let inputNitPoryecto = document.querySelector('#inputNitPoryecto');

window.addEventListener('load', () => {
    var opciones = cboModalidad.options[cboModalidad.selectedIndex];

    if (opciones.text.toLowerCase().trim() == 'proyectoproductivo') {
        inputNitEmpresa.style.display = "none";
        inputNitPoryecto.style.display = "flex";
    } else {
        inputNitPoryecto.style.display = "none";
    }
});
cboModalidad.addEventListener('change', () => {
    var opciones = cboModalidad.options[cboModalidad.selectedIndex];

    if (opciones.text.toLowerCase().trim() == 'proyectoproductivo') {
        inputNitEmpresa.style.display = "none";
        inputNitPoryecto.style.display = "flex";
    } else {
        inputNitPoryecto.style.display = "none";
    }
});



