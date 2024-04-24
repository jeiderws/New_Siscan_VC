window.addEventListener('DOMContentLoaded', event => {
    // Simple-DataTables
    // https://github.com/fiduswriter/Simple-DataTables/wiki

    //const datatablesSimple = document.getElementById('datatablesSimple');
    //if (datatablesSimple) {
    //    new simpleDatatables.DataTable(datatablesSimple);
  
    //}
    const datatablesSimple = new simpleDatatables.DataTable("#datatablesSimple", {
        searchable: true,
        perPage: 5,
    })

    let cambio = document.querySelector('#pruebatyt');
    let cboEstadoPruebatyt = document.querySelector('#cboEstadoPruebatyt');

    //cambio.style.display = "none";

});

