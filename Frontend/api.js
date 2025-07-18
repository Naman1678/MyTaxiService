/* ----------- helpers ----------- */
const token   = localStorage.getItem("jwtToken");
const headers = {
  "Content-Type": "application/json",
  "Authorization": `Bearer ${token}`
};
const statusBox   = document.getElementById("statusContainer");
const responseBox = document.getElementById("response");

/* ----------- create booking ----------- */
document.getElementById("bookingForm").addEventListener("submit", async e => {
  e.preventDefault();

  const userId = +document.getElementById("userId").value;
  const body   = {
    userId,
    pickupLocation: document.getElementById("pickupLocation").value,
    dropoffLocation: document.getElementById("dropoffLocation").value
  };

  try {
    const res = await fetch("http://localhost:5199/api/bookings", {
      method: "POST",
      headers,
      body: JSON.stringify(body)
    });

    if (!res.ok) throw new Error(await res.text());

    const booking = await res.json();
    responseBox.textContent = `✅ Booking placed – status: ${booking.status}`;
    renderStatus(booking);

  } catch (err) {
    console.error("Booking failed:", err);
    responseBox.textContent = "❌ Booking failed – login again or retry.";
  }
});

/* ----------- SignalR connection ----------- */
const connection = new signalR.HubConnectionBuilder()
  .withUrl(`http://localhost:5199/rideHub?access_token=${token}`)
  .withAutomaticReconnect()
  .configureLogging(signalR.LogLevel.Information)
  .build();

connection.on("RideUpdated", booking => {
  const currentUserId = +document.getElementById("userId").value;
  if (currentUserId && booking.userId === currentUserId) {
    renderStatus(booking);
    responseBox.textContent = `🔔 Your booking was updated: ${booking.status}`;
  }
});

connection.start()
  .then(() => console.log("✅ SignalR connected"))
  .catch(err => console.error("❌ SignalR failed:", err));

/* ----------- show booking status ----------- */
function renderStatus(b) {
  statusBox.innerHTML = `
    <p><strong>Status:</strong> ${b.status}</p>
    <p><strong>From:</strong> ${b.pickupLocation}</p>
    <p><strong>To:</strong> ${b.dropoffLocation}</p>
    ${b.driverId ? `<p><strong>Driver ID:</strong> ${b.driverId}</p>` : ""}
    ${b.carNumber ? `<p><strong>Car Number:</strong> ${b.carNumber}</p>` : ""}
  `;
}


/* ----------- Load status if userId in URL ----------- */
window.addEventListener("DOMContentLoaded", () => {
  const uid = new URLSearchParams(location.search).get("userId");
  if (uid) {
    document.getElementById("userId").value = uid;
  }
});