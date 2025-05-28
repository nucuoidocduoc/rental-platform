// Mock API implementation for the property booking application
// In a real application, these functions would interact with actual backend APIs

import { mockProperties } from "../data/mockData";

// Simulated delay to mimic API call latency
const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

// Fetch all properties
export const fetchProperties = async () => {
  try {
    // Simulate API delay
    const response = await fetch("/api/properties", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    return await response.json();
  } catch (error) {
    console.error("Error fetching properties:", error);
    throw error;
  }
};

// Fetch a single property by ID
export const fetchPropertyById = async (id) => {
  try {
    const response = await fetch("/api/properties", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    var properties = await response.json();

    const property = properties.find((prop) => prop.id === id);

    if (!property) {
      throw new Error(`Property with ID ${id} not found`);
    }

    return property;
  } catch (error) {
    console.error(`Error fetching property ${id}:`, error);
    throw error;
  }
};

// Create a booking
export const createBooking = async (bookingData) => {
  try {
    // Simulate API delay
    const response = await fetch("/api/booking", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(bookingData),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    const data = await response.json();

    // Return simulated booking response
    return data;
  } catch (error) {
    console.error("Error creating booking:", error);
    throw error;
  }
};

// Get payment details for a booking
export const getPaymentDetails = async (bookingId) => {
  try {
    // Simulate API delay
    await delay(700);

    // In a real application, this would fetch payment details from a server
    // based on the booking ID

    // Return simulated payment details
    return {
      bookingId,
      totalAmount: Math.floor(Math.random() * 700) + 100, // Random amount between 100-800
      currency: "USD",
      paymentMethods: ["credit_card", "paypal"],
    };
  } catch (error) {
    console.error("Error fetching payment details:", error);
    throw error;
  }
};

// Process payment
export const processPayment = async (paymentData) => {
  try {
    const response = await fetch(
      "/api/payments/callback/" + paymentData.bookingId,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    const data = await response.json();
    return {
      paymentId: data.paymentId,
      bookingId: paymentData.bookingId,
      amount: paymentData.amount,
      currency: paymentData.currency,
      paymentMethod: paymentData.paymentMethod,
      status: "completed",
      timestamp: new Date().toISOString(),
    };
  } catch (error) {
    console.error("Error processing payment:", error);
    throw error;
  }
};
