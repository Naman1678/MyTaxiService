const token = localStorage.getItem("jwtToken");
const driverId = localStorage.getItem("driverId");

fetch("http://localhost:5199/api/authorization/driver-dashboard", {
    method: "GET",
    headers: {
        "Authorization": `Bearer ${token}`
    }
})
.then(res => {
    if (!res.ok) throw new Error("Unauthorized");
    return res.text();
})
.then(message => {
    const welcome = document.getElementById("welcome");
    if (welcome) welcome.innerText = message;
    loadBookings(); 
})
.catch(() => {
    alert("Access denied. Please log in again.");
    window.location.href = "login.html";
});

async function loadBookings() {
    try {
        const res = await fetch("http://localhost:5199/api/bookings/pending", {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        const bookings = await res.json();
        const container = document.getElementById("rideRequests");
        container.innerHTML = "";

        if (bookings.length === 0) {
            container.innerHTML = "<p>No pending ride requests.</p>";
            return;
        }

        bookings.forEach(booking => {
            const card = document.createElement("div");
            card.className = "booking-card";
            card.innerHTML = `
                <p><strong>Booking ID:</strong> ${booking.bookingId}</p>
                <p><strong>Client:</strong> User ${booking.userId}</p>
                <p><strong>From:</strong> ${booking.pickupLocation}</p>
                <p><strong>To:</strong> ${booking.dropoffLocation}</p>
                <button onclick="handleBooking(${booking.bookingId}, 'accept')">✅ Accept</button>
                <button onclick="handleBooking(${booking.bookingId}, 'decline')">❌ Decline</button>
            `;
            container.appendChild(card);
        });
    } catch (err) {
        console.error("Failed to load bookings", err);
    }
}

async function handleBooking(bookingId, action) {
    const driverId = localStorage.getItem("driverId");
    if (!driverId) {
        alert("Driver ID not found. Please login again.");
        return;
    }

    const url = `http://localhost:5199/api/bookings/${bookingId}/${action}?driverId=${driverId}`;

    try {
        const res = await fetch(url, {
            method: "PUT",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!res.ok) throw new Error(await res.text());

                let actionMessage = "";
        if (action === "accept") actionMessage = "accepted";
        else if (action === "decline") actionMessage = "declined";

        alert(`Booking ${actionMessage} successfully!`);


        loadBookings(); 
    } catch (err) {
        alert(`Failed to ${action} booking: ${err.message}`);
    }
}