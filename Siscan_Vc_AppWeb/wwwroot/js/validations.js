document.addEventListener('DOMContentLoaded', function () {
    function validateForm() {
        let isValid = true;
        //inputs
        const tipoDocumento = document.querySelector('#cboTipoDocumento');
        const numeroDocumento = document.querySelector('#inputDocumento');
        const nombre = document.querySelector('#Name');
        const apellido = document.querySelector('#inputLastName');
        const telefono = document.querySelector('#inputTelefono');
        const direccion = document.querySelector('#inputDireccion');
        const correo = document.querySelector('#inputEmail');
        const estadoAp = document.querySelector('#cboEstadoAprendiz');
        const depto = document.querySelector('#cboDepartamentos');
        const ciudad = document.querySelector('#cboCiudad');
        const pruebatyt = document.querySelector('#cboEstadoPruebatyt');
        const programa = document.querySelector('#cboPrograma');
        const ficha = document.querySelector('#cboFicha');
        const acudiente = document.querySelector('#NameAcudient');
        const telAcudiente = document.querySelector('#TelAcudiente');
        const correoAcud = document.querySelector('#EmailAcud');
        const codigotyt = document.querySelector('#inputcodigotyt');
        const convocatoria = document.querySelector('#cboConvocatoria');
        const depto2 = document.querySelector('#cboDepartamentos2');
        const ciudad2 = document.querySelector('#cboCiudad2');

        //span para los mensajes
        document.querySelector('#tipoDocumentoError').textContent = '';
        document.querySelector('#numeroDocumentoError').textContent = '';
        document.querySelector('#NameError').textContent = '';
        document.querySelector('#inputLastNameError').textContent = '';
        document.querySelector('#telefonoError').textContent = '';
        document.querySelector('#inputDireccionError').textContent = '';
        document.querySelector('#inputEmailError').textContent = '';
        document.querySelector('#cboEstadoAprendizError').textContent = '';
        document.querySelector('#cboDepartamentosError').textContent = '';
        document.querySelector('#cboCiudadError').textContent = '';
        document.querySelector('#cboEstadoPruebatytError').textContent = '';
        document.querySelector('#cboProgramaError').textContent = '';
        document.querySelector('#cboFichaError').textContent = '';
        document.querySelector('#NameAcudientError').textContent = '';
        document.querySelector('#TelAcudienteError').textContent = '';
        document.querySelector('#EmailAcudError').textContent = '';
        document.querySelector('#inputcodigotytError').textContent = '';
        document.querySelector('#cboConvocatoriaError').textContent = '';
        document.querySelector('#cboDepartamentos2Error').textContent = '';
        document.querySelector('#cboCiudad2Error').textContent = '';

        //regex
        const documentoRegex = /^\d{6,10}$/;
        const telefonoRegex = /^\d{10}$/; 
        const nombreRegex = /^[A-Za-zÀ-Öà-ö\s\'\"]+$/;
       

        if (tipoDocumento.value === '') {
            document.querySelector('#tipoDocumentoError').textContent = 'El tipo de documento es obligatorio.';
            isValid = false;
        }

        if (numeroDocumento.value.trim() === '') {
            document.querySelector('#numeroDocumentoError').textContent = 'El número de documento es obligatorio.';
            isValid = false;
        } else if (!documentoRegex.test(numeroDocumento.value)) {
            document.querySelector('#numeroDocumentoError').textContent = 'El número de documento no es válido.';
            isValid = false;
        }

        if (nombre.value.trim() === '') {
            document.querySelector('#NameError').textContent = 'El nombre del aprendiz  es obligatorio.';
            isValid = false;
        } else if (!nombreRegex.test(nombre.value)) {
            document.querySelector('#NameError').textContent  = 'El nombre solo puede contener letras, tildes, espacios y comillas simples y dobles.';
            isValid = false;
        }

        if (apellido.value.trim() === '') {
            document.querySelector('#inputLastNameError').textContent  = 'Los apellido del aprendiz  es obligatorio.';
            isValid = false;
        } else if (!nombreRegex.test(apellido.value)) {
            document.querySelector('#inputLastNameError').textContent  = 'El nombre solo puede contener letras, tildes, espacios y comillas simples y dobles.';
            isValid = false;
        }

        if (telefono.value.trim() === '') {
            document.querySelector('#telefonoError').textContent = 'El numero de telefono es obligatorio.';
            isValid = false;
        } else if (!telefonoRegex.test(telefono.value)) {
            document.querySelector('#telefonoError').textContent = 'El teléfono no es válido.';
            isValid = false;
        }

        if (direccion.value.trim() === '') {
            document.querySelector('#inputDireccionError').textContent = 'La direcion es obligatoria.';
            isValid = false;
        }

        if (correo.value.trim() === '') {
            document.querySelector('#inputEmailError').textContent = 'El correo es obligatorio.';
            isValid = false;
        }

        if (estadoAp.value.trim() === '') {
            document.querySelector('#cboEstadoAprendizError').textContent = 'Este campo es obligatorio.';
            isValid = false;
        }

        if (depto.value.trim() === '') {
            document.querySelector('#cboDepartamentosError').textContent = 'Este campo es obligatorio.';
            isValid = false;
        }
        if (ciudad.value.trim() === '') {
            document.querySelector('#cboCiudadError').textContent = 'Este campo es obligatorio.';
            isValid = false;
        }
        if (pruebatyt.value.trim() === '') {
            document.querySelector('#cboEstadoPruebatytError').textContent = 'Este campo es obligatorio.';
            isValid = false;
        }
        if (programa.value.trim() === '') {
            document.querySelector('#cboProgramaError').textContent = 'Este campo es obligatorio.';
            isValid = false;
        }
        if (ficha.value.trim() === '') {
            document.querySelector('#cboFichaError').textContent = 'Este campo es obligatorio.';
            isValid = false;
        }
        if (acudiente.value.trim() === '') {
            document.querySelector('#NameAcudientError').textContent = 'El nombre es obligatorio.';
            isValid = false;
        } else if (!nombreRegex.test(acudiente.value)) {
            document.querySelector('#NameAcudientError').textContent = 'El nombre solo puede contener letras, tildes, espacios y comillas simples y dobles.';
            isValid = false;
        }

        if (telAcudiente.value.trim() === '') {
            document.querySelector('#TelAcudienteError').textContent = 'El telefono es obligatorio.';
            isValid = false;
        } else if (!telefonoRegex.test(telAcudiente.value)) {
            document.querySelector('#TelAcudienteError').textContent = 'El teléfono no es válido.';
            isValid = false;
        }
        if (correoAcud.value.trim() === '') {
            document.querySelector('#EmailAcudError').textContent = 'El correo es obligatorio.';
            isValid = false;
        } 
       
        //if (codigotyt.value.trim() === '') {
        //    document.querySelector('#inputcodigotytError').textContent = 'El correo es obligatorio.';
        //    isValid = false;
        //}  
       
        //if (convocatoria.value.trim() === '') {
        //    document.querySelector('#cboConvocatoriaError').textContent = 'El campo es obligatorio.';
        //    isValid = false;
        //} 
       
        //if (depto2.value.trim() === '') {
        //    document.querySelector('#cboDepartamentos2Error').textContent = 'El campo es obligatorio.';
        //    isValid = false;
        //} 
        //if (ciudad2.value.trim() === '') {
        //    document.querySelector('#cboCiudad2Error').textContent = 'El campo es obligatorio.';
        //    isValid = false;
        //} 
       
      
        return isValid;
    }
    function confirmSave() {
        return confirm("¿Estás seguro de que deseas guardar?");
    }

    const form = document.querySelector('#registroForm');
    form.addEventListener('submit', function (event) {
        if (!validateForm()) {
            event.preventDefault();
        } else {
            confirmSave();
        }
    });
});