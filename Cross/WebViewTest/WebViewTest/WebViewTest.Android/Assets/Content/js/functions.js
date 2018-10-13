function loadPage(){
    var dv = document.createElement('div');
    dv.id='dvPrueba';
    dv.innerHTML = 'Esto es un DIV agregado por la función JS agregado a través de un String.';
    document.body.appendChild(dv);
    alert('Se ha agregado el contenido correctament !!');
}