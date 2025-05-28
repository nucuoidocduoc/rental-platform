import React from 'react';
import { Link } from 'react-router-dom';

const ThankYouPage = ({ bookingDetails, paymentDetails }) => {
  const bookingDate = new Date().toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  });

  if (!bookingDetails || !paymentDetails) {
    return (
      <div className="container mx-auto px-4 py-12 text-center">
        <div className="bg-yellow-100 border border-yellow-400 text-yellow-700 px-4 py-3 rounded max-w-md mx-auto">
          <h2 className="text-2xl font-bold mb-2">Oops!</h2>
          <p>Booking information is missing.</p>
          <Link 
            to="/properties" 
            className="mt-4 inline-block bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition"
          >
            Browse Properties
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-12">
      <div className="max-w-2xl mx-auto bg-white rounded-lg shadow-md overflow-hidden">
        <div className="p-6 bg-green-50 border-b border-green-100 text-center">
          <div className="w-20 h-20 bg-green-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <svg xmlns="http://www.w3.org/2000/svg" className="h-12 w-12 text-green-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
            </svg>
          </div>
          <h1 className="text-3xl font-bold text-green-800 mb-2">Booking Confirmed!</h1>
          <p className="text-green-700">Your booking has been successfully completed.</p>
        </div>
        
        <div className="p-6">
          <h2 className="text-2xl font-bold mb-4">Booking Details</h2>
          
          <div className="mb-6">
            <div className="flex justify-between mb-2">
              <span className="text-gray-700">Booking ID:</span>
              <span className="font-medium">{bookingDetails.bookingId}</span>
            </div>
            <div className="flex justify-between mb-2">
              <span className="text-gray-700">Property:</span>
              <span className="font-medium">{bookingDetails.propertyName}</span>
            </div>
            <div className="flex justify-between mb-2">
              <span className="text-gray-700">Check-in:</span>
              <span>{bookingDetails.checkIn}</span>
            </div>
            <div className="flex justify-between mb-2">
              <span className="text-gray-700">Check-out:</span>
              <span>{bookingDetails.checkOut}</span>
            </div>
            <div className="flex justify-between mb-2">
              <span className="text-gray-700">Guests:</span>
              <span>{bookingDetails.guests}</span>
            </div>
          </div>
          
          <div className="border-t border-gray-200 pt-4 mb-6">
            <h3 className="text-xl font-semibold mb-3">Payment Information</h3>
            <div className="flex justify-between mb-2">
              <span className="text-gray-700">Payment ID:</span>
              <span>{paymentDetails.paymentId}</span>
            </div>
            <div className="flex justify-between mb-2">
              <span className="text-gray-700">Payment Method:</span>
              <span className="capitalize">{paymentDetails.paymentMethod}</span>
            </div>
            <div className="flex justify-between mb-2">
              <span className="text-gray-700">Date:</span>
              <span>{bookingDate}</span>
            </div>
            <div className="flex justify-between font-bold mt-2">
              <span>Total Amount:</span>
              <span>{paymentDetails.currency} {paymentDetails.amount.toFixed(2)}</span>
            </div>
          </div>
          
          <div className="bg-blue-50 p-4 rounded-md mb-6">
            <h3 className="text-lg font-semibold mb-2 text-blue-800">What's Next?</h3>
            <ul className="list-disc list-inside text-blue-700 space-y-1">
              <li>An email with your booking details has been sent to your registered email address.</li>
              <li>The property owner will contact you with check-in instructions.</li>
              <li>You can view all your booking details in your account dashboard.</li>
            </ul>
          </div>
          
          <div className="text-center">
            <Link 
              to="/properties" 
              className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-md transition font-semibold inline-block"
            >
              Browse More Properties
            </Link>
          </div>
        </div>
        
        <div className="p-4 bg-gray-50 border-t text-center text-gray-600">
          Thank you for choosing PropertyBooker! If you have any questions, please contact our support team.
        </div>
      </div>
    </div>
  );
};

export default ThankYouPage;