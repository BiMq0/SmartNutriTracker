let ultimaRecomendacion = null;

// Función de validación en frontend
window.validarFormulario = function() {
    const pesoKg = parseFloat(document.querySelector('input[type="number"]').value);
    const alturaM = parseFloat(document.querySelectorAll('input[type="number"]')[1].value);
    const fechaNacimiento = document.querySelector('input[type="date"]').value;
    const sexo = document.querySelectorAll('select')[0].value;
    const nivelActividad = document.querySelectorAll('select')[1].value;
    const objetivo = document.querySelectorAll('select')[2].value;
    
    const errores = [];
    
    // Validar peso
    if (!pesoKg || pesoKg < 30 || pesoKg > 300) {
        errores.push('El peso debe estar entre 30 kg y 300 kg');
    }
    
    // Validar altura
    if (!alturaM || alturaM < 1.2 || alturaM > 2.5) {
        errores.push('La altura debe estar entre 1.2 m y 2.5 m');
    }
    
    // Validar fecha de nacimiento
    if (!fechaNacimiento) {
        errores.push('La fecha de nacimiento es requerida');
    } else {
        const fechaNac = new Date(fechaNacimiento);
        const hoy = new Date();
        let edad = hoy.getFullYear() - fechaNac.getFullYear();
        const mes = hoy.getMonth() - fechaNac.getMonth();
        if (mes < 0 || (mes === 0 && hoy.getDate() < fechaNac.getDate())) {
            edad--;
        }
        if (edad < 13 || edad > 100) {
            errores.push(`La edad debe estar entre 13 y 100 años. Edad calculada: ${edad} años`);
        }
    }
    
    // Validar campos select
    if (!sexo) errores.push('Selecciona un sexo');
    if (!nivelActividad) errores.push('Selecciona un nivel de actividad');
    if (!objetivo) errores.push('Selecciona un objetivo');
    
    return { isValid: errores.length === 0, errores };
};

// Mostrar errores
window.mostrarErrores = function(errores) {
    let errorHtml = '';
    errores.forEach(error => {
        errorHtml += `<div class="error-item">✗ ${error}</div>`;
    });
    const contenedorErrores = document.getElementById('contenedor-errores');
    if (contenedorErrores) {
        contenedorErrores.innerHTML = errorHtml;
        contenedorErrores.style.display = 'block';
    }
};

window.limpiarErrores = function() {
    const contenedorErrores = document.getElementById('contenedor-errores');
    if (contenedorErrores) {
        contenedorErrores.innerHTML = '';
        contenedorErrores.style.display = 'none';
    }
};

window.pedirRecomendacion = async function() {
    console.log("Iniciando petición...");
    
    // Limpiar errores previos
    window.limpiarErrores();
    
    // Validar en frontend
    const validacion = window.validarFormulario();
    if (!validacion.isValid) {
        window.mostrarErrores(validacion.errores);
        return;
    }
    
    const btnPedir = document.getElementById('btnPedir');
    const btnText = document.getElementById('btnText');
    const contenedorResultado = document.getElementById('contenedor-resultado');
    
    // Deshabilitar el botón y mostrar animación
    btnPedir.disabled = true;
    btnPedir.style.cursor = 'not-allowed';
    btnPedir.style.opacity = '0.6';
    btnText.textContent = "Generando recomendación…";
    
    try {
        const pesoKg = parseFloat(document.querySelector('input[type="number"]').value);
        const alturaM = parseFloat(document.querySelectorAll('input[type="number"]')[1].value);
        const fechaNacimiento = document.querySelector('input[type="date"]').value;
        const sexo = document.querySelectorAll('select')[0].value;
        const nivelActividad = document.querySelectorAll('select')[1].value;
        const objetivo = document.querySelectorAll('select')[2].value;
        
        console.log("Datos del formulario:", { pesoKg, alturaM, fechaNacimiento, sexo, nivelActividad, objetivo });
        
        // Mostrar skeleton loader
        mostrarSkeletonLoader();
        
        const response = await fetch('https://localhost:7187/api/Nutrition/recommendations', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                pesoKg,
                alturaM,
                fechaNacimiento,
                sexo,
                nivelActividad,
                objetivo
            })
        });
        
        console.log("Status:", response.status);
        
        if (response.ok) {
            const data = await response.json();
            console.log("Respuesta recibida:", data);
            
            // Guardar la recomendación completa para exportar
            ultimaRecomendacion = data;
            
            // Mostrar tiempo de respuesta
            const tiempoMs = data.tiempoRespuestaMs || 0;
            document.getElementById('tiempoRespuesta').textContent = `Tiempo de procesamiento: ${tiempoMs} ms`;
            
            // Asegurarse de que textoRecomendacion es string
            let textoRecomendacion = data.textoRecomendacion;
            if (typeof textoRecomendacion === 'object') {
                textoRecomendacion = JSON.stringify(textoRecomendacion, null, 2);
            }
            
            // Mostrar el texto de recomendación
            document.getElementById('textoRecomendacion').textContent = textoRecomendacion;
            
            // Construir la lista de comidas si existen
            const listComidas = document.getElementById('listComidas');
            listComidas.innerHTML = '';
            
            let comidas = data.comidas;
            if (comidas && comidas.length > 0) {
                const titulo = document.createElement('h3');
                titulo.style.color = '#576E40';
                titulo.style.marginTop = '20px';
                titulo.style.marginBottom = '15px';
                titulo.textContent = 'Plan de Comidas';
                listComidas.appendChild(titulo);
                
                const ul = document.createElement('ul');
                ul.className = 'lista-comidas';
                
                comidas.forEach(comida => {
                    const li = document.createElement('li');
                    li.className = 'item-comida';
                    li.innerHTML = `
                        <div class="nombre-comida">${comida.nombre}</div>
                        <div class="stats-comida">
                            <div class="stat-item">${comida.calorias} kcal</div>
                            <div class="stat-item">P: ${comida.proteinas_g} g</div>
                            <div class="stat-item">G: ${comida.grasas_g} g</div>
                            <div class="stat-item">C: ${comida.carbohidratos_g} g</div>
                        </div>
                        <div class="descripcion-comida">${comida.descripcion}</div>
                    `;
                    ul.appendChild(li);
                });
                
                listComidas.appendChild(ul);
            }
            
            // Mostrar el contenedor de resultado
            contenedorResultado.style.display = 'block';
        } else {
            const error = await response.json();
            console.error("Error:", error);
            
            // Mostrar errores del servidor
            if (error.errores && Array.isArray(error.errores)) {
                window.mostrarErrores(error.errores);
            } else {
                alert("Error: " + response.status + "\n" + (error.mensaje || error));
            }
        }
    } catch (error) {
        console.error("Excepción:", error);
        alert("Error de conexión: " + error.message);
    } finally {
        btnPedir.disabled = false;
        btnPedir.style.cursor = 'pointer';
        btnPedir.style.opacity = '1';
        btnText.textContent = "Pedir recomendación";
        ocultarSkeletonLoader();
    }
};

function mostrarSkeletonLoader() {
    const listComidas = document.getElementById('listComidas');
    listComidas.innerHTML = `
        <div class="skeleton-loader">
            <div class="skeleton skeleton-text"></div>
            <div class="skeleton skeleton-text"></div>
            <div class="skeleton skeleton-card"></div>
        </div>
    `;
}

function ocultarSkeletonLoader() {
    const loader = document.querySelector('.skeleton-loader');
    if (loader) {
        loader.style.display = 'none';
    }
}

window.exportarAPdf = async function() {
    if (!ultimaRecomendacion) {
        alert("No hay recomendación para exportar. Por favor, genera una primero.");
        return;
    }
    
    console.log("Exportando a PDF...", ultimaRecomendacion);
    
    try {
        const response = await fetch('https://localhost:7187/api/Nutrition/export-pdf', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(ultimaRecomendacion)
        });
        
        if (response.ok) {
            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `Recomendacion_Nutricional_${new Date().toISOString().split('T')[0]}.pdf`;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);
            alert("PDF descargado exitosamente");
        } else {
            alert("Error al exportar PDF: " + response.status);
        }
    } catch (error) {
        console.error("Error al exportar:", error);
        alert("Error al exportar PDF: " + error.message);
    }
};
