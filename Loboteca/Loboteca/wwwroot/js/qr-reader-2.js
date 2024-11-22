$(document).ready(function () {
    // Inicializamos una variable para controlar si el QR ya fue escaneado
    let qrEscaneado = false;

    // Manejador de clic en el botón de inicio de sesión con QR
    $("#startQrButton").click(function () {
        // Si ya se escaneó un código QR, no hacer nada
        if (qrEscaneado) {
            return; // Detener la ejecución si ya se ha escaneado
        }

        $("#reader").show(); // Muestra el lector QR

        // Inicializa el escáner
        const html5QrCode = new Html5Qrcode("reader");

        // Iniciar el escáner usando la cámara predeterminada
        html5QrCode.start(
            { facingMode: "user" },
            {
                fps: 10,
                qrbox: { width: 250, height: 250 }
            },
            function (qrCodeMessage) {
                const matricula = qrCodeMessage.trim(); // Asegúrate de que no haya espacios en blanco

                // Validar que la matrícula tenga exactamente 10 dígitos
                if (matricula.length !== 10) {
                    alert("El código QR debe tener exactamente 10 dígitos. Consulta tu matrícula.");
                    return; // Detener el proceso si no es válido
                }

                // Validar que el código QR solo contenga números
                if (!/^\d+$/.test(matricula)) {
                    alert("El código QR debe contener solo números.");
                    return; // Detener el proceso si no es numérico
                }

                // Colocar el valor escaneado en el campo "Usuario"
                $("#startQrButton").val(matricula); // Coloca el código QR escaneado en el campo "Usuario"

                // Mostrar el alert solo una vez
                alert(`Matrícula válida: ${matricula}`); // Confirmación de que la matrícula fue escaneada correctamente

                // Detener el escáner después de un solo escaneo
                html5QrCode.stop(); // Detiene el escáner

                // Establecer que el QR ya fue escaneado para evitar que se repita el proceso
                qrEscaneado = true;

                // Ocultar el lector QR después de escanear
                $("#reader").hide();
            },
            function (errorMessage) {
                console.warn(`Error de escaneo: ${errorMessage}`);
            }
        ).catch(err => {
            console.error("Error al iniciar el escáner: ", err);
            alert("No se pudo iniciar el escáner. Asegúrate de que la cámara esté disponible.");
        });
    });
});
