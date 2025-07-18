// Fetch token and driverId from localStorage
const token    = localStorage.getItem("jwtToken");
const driverId = localStorage.getItem("driverId");
const hdrAuth  = { "Authorization": `Bearer ${token}` };

// Get driver dashboard welcome message
fetch("http://localhost:5199/api/authorization/driver-dashboard", { headers: hdrAuth })
  .then(response => {
    if (!response.ok) throw new Error("Unauthorized");
    return response.text();
  })
  .then(message => {
    document.getElementById("welcome").textContent = message;
  // Load bookings after welcome message
    loadBookings();  
  })
  .catch(() => {
    alert("Access denied. Please log in again.");
    window.location.href = "login.html";
  });

// Load all pending bookings for driver
async function loadBookings() {
  const container = document.getElementById("rideRequests");
  container.innerHTML = "";

  try {
    const response = await fetch("http://localhost:5199/api/bookings/pending", {
      headers: hdrAuth
    });

    const bookings = await response.json();

    if (!Array.isArray(bookings) || bookings.length === 0) {
      container.textContent = "No pending ride requests.";
      return;
    }

    bookings.forEach(booking => {
      const card = document.createElement("div");
      card.className = "booking-card";

      card.innerHTML = `
        <p><strong>ID:</strong> ${booking.bookingId}</p>
        <p><strong>User:</strong> ${booking.userId}</p>
        <p><strong>From:</strong> ${booking.pickupLocation}</p>
        <p><strong>To:</strong> ${booking.dropoffLocation}</p>
        <button class="acceptBtn" onclick="handleBooking(${booking.bookingId}, 'accept')">✅ Accept</button>
        <button class="declineBtn" onclick="handleBooking(${booking.bookingId}, 'decline')">❌ Decline</button>
      `;

      container.appendChild(card);
    });
  } catch (error) {
    console.error("Error loading bookings:", error);
    container.textContent = "Failed to load ride requests.";
  }
}

// Handle Accept or Decline booking
async function handleBooking(bookingId, action) {
  if (!driverId) {
    alert("Driver ID not found. Please login again.");
    return;
  }

  const url = `http://localhost:5199/api/bookings/${bookingId}/${action}?driverId=${driverId}`;

  try {
    const res = await fetch(url, {
      method: "PUT",
      headers: hdrAuth
    });

    if (!res.ok) {
      const errText = await res.text();
      throw new Error(errText);
    }

    alert(`Booking ${action === "accept" ? "accepted" : "declined"} successfully!`);
    loadBookings(); // Refresh the list
  } catch (err) {
    alert(`Failed to ${action} booking: ${err.message}`);
  }
}
