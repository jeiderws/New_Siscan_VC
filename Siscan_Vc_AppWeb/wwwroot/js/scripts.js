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

let mostrar = document.querySelector('#pruebatyt');
let estadotyt = document.querySelector('#cboEstadoPruebatyt');


estadotyt.addEventListener('change', () => {
    let ValorOption = estadotyt.value;

    var opciones = estadotyt.options[estadotyt.selectedIndex];

    if (opciones.text == "Inscrito") {
        mostrar.style.display = "flex";
    } else {
        mostrar.style.display = "none";
    }
   

})



