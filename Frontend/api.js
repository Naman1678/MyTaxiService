document.getElementById("bookingForm").addEventListener("submit", async function (e) {
  e.preventDefault();

  const userId = document.getElementById("userId").value;
  const bookingData = {
    userId: parseInt(userId),
    pickupLocation: document.getElementById("pickupLocation").value,
    dropoffLocation: document.getElementById("dropoffLocation").value
  };

  const responseElement = document.getElementById("response");
  const token = localStorage.getItem("jwtToken");

  try {
    const res = await fetch("http://localhost:5199/api/bookings", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      },
      body: JSON.stringify(bookingData)
    });

    if (!res.ok) {
      const errorText = await res.text();
      throw new Error(`Server Error: ${res.status} - ${errorText}`);
    }

    const result = await res.json();
    responseElement.innerText = `✅ Booking successful! Status: ${result.status}`;
    checkBookingStatus(userId);
  } catch (error) {
    console.error("Booking failed:", error);
    responseElement.innerText = "❌ Booking failed. Please login again or try later.";
  }
});

async function checkBookingStatus(userId) {
  const token = localStorage.getItem("jwtToken");
  const statusContainer = document.getElementById("statusContainer");

  try {
    const res = await fetch(`http://localhost:5199/api/bookings/client-latest?userId=${userId}`, {
      headers: {
        "Authorization": `Bearer ${token}`
      }
    });

    if (!res.ok) {
      statusContainer.innerText = "No active bookings found.";
      return;
    }

    const booking = await res.json();
    statusContainer.innerHTML = `
      <p><strong>Status:</strong> ${booking.status}</p>
      <p><strong>From:</strong> ${booking.pickupLocation}</p>
      <p><strong>To:</strong> ${booking.dropoffLocation}</p>
      ${booking.driverId ? `<p><strong>Driver ID:</strong> ${booking.driverId}</p>` : ""}
    `;
  } catch (err) {
    console.error("Failed to fetch booking status:", err);
    statusContainer.innerText = "Failed to load booking status.";
  }
}

// Auto check status on load if userId present
window.onload = function () {
  const params = new URLSearchParams(window.location.search);
  const userId = params.get("userId");
  if (userId) {
    document.getElementById("userId").value = userId;
    checkBookingStatus(userId);
  }
};
