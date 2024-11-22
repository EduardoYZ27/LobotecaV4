// Manejador de clic en el botón de cerrar sesión
document.getElementById("logoutButton").addEventListener("click", function () {
    const confirmLogout = confirm("¿Estás seguro de que deseas cerrar sesión?");
    if (confirmLogout) {
        localStorage.removeItem("carreraSeleccionada"); // Eliminar la carrera seleccionada del localStorage
        alert("Has cerrado sesión.");
        window.location.href = "/Login/Login"; // Redirigir a la página de inicio de sesión
    }
});