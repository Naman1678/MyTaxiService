document.getElementById("loginForm").addEventListener("submit", async function (e) {
  e.preventDefault();

  const username = document.getElementById("username").value;
  const password = document.getElementById("password").value;
  const role = document.getElementById("role").value;

  try {
    const response = await fetch("http://localhost:5199/api/authorization/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ username, password, role })
    });

    const result = await response.json();

    if (response.ok) {
      localStorage.setItem("jwtToken", result.token);
      localStorage.setItem("userRole", result.role);

      if (result.role === "Driver") {
        localStorage.setItem("driverId", result.driverId);
        window.location.href = "driverdashboard.html";
      } else if (result.role === "Client") {
        window.location.href = "booking.html";
      }
    } else {
      document.getElementById("loginStatus").innerText = result;
    }
  } catch (err) {
    console.error(err);
    document.getElementById("loginStatus").innerText = "Login failed. Please try again.";
  }
});