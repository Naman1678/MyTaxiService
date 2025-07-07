document.getElementById("bookingForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const bookingData = {
        userId: document.getElementById("userId").value,
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
        responseElement.innerText = `Booking successful! Status: ${result.status}`;
    } catch (error) {
        console.error("Booking failed:", error);
        responseElement.innerText = "Booking failed. Please login again or try later.";
    }
});
