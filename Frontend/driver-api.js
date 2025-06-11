document.getElementById("driverForm").addEventListener("submit", async function(e) {
  e.preventDefault();

  const driverData = {
    name: document.getElementById("name").value,
    vehicleType: document.getElementById("vehicleType").value,
    licenseNumber: document.getElementById("licenseNumber").value,
    phoneNumber: document.getElementById("phoneNumber").value
  };

  const responseBox = document.getElementById("driverResponse");

  try {
    const res = await fetch("http://localhost:5199/api/drivers", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(driverData)
    });

    if (!res.ok) {
      const error = await res.text();
      throw new Error(error);
    }

    const result = await res.json();
    responseBox.innerText = `✅ Registered Successfully! Your Driver ID: ${result.driverId}`;
  } catch (error) {
    console.error("Driver registration failed:", error);
    responseBox.innerText = "❌ Registration failed. Please try again.";
  }
});
