import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { fetchPropertyById, createBooking } from '../services/api';
import { formatDate } from '../utils/helpers';

const PropertyDetail = ({ onBookingSuccess }) => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [property, setProperty] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedImage, setSelectedImage] = useState(0);
  
  const [booking, setBooking] = useState({
    checkIn: formatDate(new Date()),
    checkOut: formatDate(new Date(new Date().setDate(new Date().getDate() + 3))),
    guests: 2,
  });
  
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [bookingError, setBookingError] = useState(null);

  useEffect(() => {
    const getPropertyDetails = async () => {
      try {
        setLoading(true);
        const data = await fetchPropertyById(id);
        setProperty(data);
        setLoading(false);
      } catch (err) {
        setError('Failed to load property details. Please try again later.');
        setLoading(false);
      }
    };

    getPropertyDetails();
  }, [id]);

  const handleBookingChange = (e) => {
    const { name, value } = e.target;
    setBooking({
      ...booking,
      [name]: value
    });
  };

  const calculateTotalPrice = () => {
    if (!property) return 0;
    
    const checkInDate = new Date(booking.checkIn);
    const checkOutDate = new Date(booking.checkOut);
    const nights = Math.ceil((checkOutDate - checkInDate) / (1000 * 60 * 60 * 24));
    
    return property.pricePerNight * nights;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setBookingError(null);
    setIsSubmitting(true);
    
    // Basic validation
    const checkInDate = new Date(booking.checkIn);
    const checkOutDate = new Date(booking.checkOut);
    
    if (checkOutDate <= checkInDate) {
      setBookingError('Check-out date must be after check-in date');
      setIsSubmitting(false);
      return;
    }

    try {
      const bookingData = {
        ...booking,
        propertyId: property.id,
        propertyName: property.name,
        totalPrice: calculateTotalPrice(),
      };
      
      const response = await createBooking(bookingData);
      
      // If booking is successful, pass the data and navigate to payment
      onBookingSuccess(response);
      navigate('/payment');
    } catch (err) {
      setBookingError('Failed to create booking. Please try again.');
      setIsSubmitting(false);
    }
  };

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-12 flex justify-center items-center">
        <div className="text-center">
          <div className="w-16 h-16 border-4 border-blue-600 border-t-transparent rounded-full animate-spin mx-auto mb-4"></div>
          <p className="text-xl">Loading property details...</p>
        </div>
      </div>
    );
  }

  if (error || !property) {
    return (
      <div className="container mx-auto px-4 py-12 text-center">
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded max-w-md mx-auto">
          <p>{error || 'Property not found'}</p>
          <button 
            onClick={() => navigate('/properties')} 
            className="mt-2 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition"
          >
            Back to Properties
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      {/* Back Button */}
      <button 
        onClick={() => navigate('/properties')}
        className="flex items-center text-blue-600 hover:text-blue-800 mb-4"
      >
        <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 mr-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
        </svg>
        Back to properties
      </button>

      {/* Property Title */}
      <h1 className="text-3xl font-bold mb-2">{property.name}</h1>
      
      {/* Location and Rating */}
      <div className="flex flex-wrap items-center mb-6">
        <div className="flex items-center mr-4">
          <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-500 mr-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
          </svg>
          <span className="text-gray-700">{property.location}</span>
        </div>
        <div className="flex items-center">
          <span className="text-yellow-500 mr-1">★</span>
          <span className="font-medium">{property.rating}</span>
          <span className="text-gray-500 text-sm ml-1">({property.reviewCount} reviews)</span>
        </div>
      </div>
      
      {/* Image Gallery */}
      <div className="mb-8">
        <div 
          className="w-full h-96 bg-gray-300 rounded-lg bg-cover bg-center mb-2"
          style={{ backgroundImage: `url(${property.images[selectedImage]})` }}
        ></div>
        
        {/* Thumbnails */}
        <div className="flex overflow-x-auto space-x-2 py-2">
          {property.images.map((image, index) => (
            <div 
              key={index}
              className={`flex-shrink-0 w-24 h-16 bg-cover bg-center cursor-pointer rounded ${selectedImage === index ? 'ring-2 ring-blue-600' : ''}`}
              style={{ backgroundImage: `url(${image})` }}
              onClick={() => setSelectedImage(index)}
            ></div>
          ))}
        </div>
      </div>
      
      {/* Property Details and Booking Form */}
      <div className="flex flex-col lg:flex-row gap-8">
        {/* Property Details Section */}
        <div className="flex-grow">
          <h2 className="text-2xl font-bold mb-4">About this property</h2>
          <p className="text-gray-700 mb-6">{property.description}</p>
          
          {/* Amenities */}
          <h3 className="text-xl font-semibold mb-3">Amenities</h3>
          <div className="grid grid-cols-2 gap-2 mb-6">
            {property.amenities.map((amenity, index) => (
              <div key={index} className="flex items-center">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-green-600 mr-2" viewBox="0 0 20 20" fill="currentColor">
                  <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                </svg>
                <span className="text-gray-700">{amenity}</span>
              </div>
            ))}
          </div>
          
          {/* Property Rules */}
          <h3 className="text-xl font-semibold mb-3">Property Rules</h3>
          <ul className="list-disc list-inside text-gray-700 mb-6">
            <li>Check-in: After 2:00 PM</li>
            <li>Checkout: Before 11:00 AM</li>
            <li>No smoking</li>
            <li>No parties or events</li>
            {property.rules && property.rules.map((rule, index) => (
              <li key={index}>{rule}</li>
            ))}
          </ul>
          
          {/* Host Information */}
          <h3 className="text-xl font-semibold mb-3">About the Host</h3>
          <div className="flex items-center">
            <div className="w-12 h-12 rounded-full bg-gray-300 mr-4"></div>
            <div>
              <p className="font-medium">{property.host?.name || 'Property Host'}</p>
              <p className="text-gray-600 text-sm">Host since 2018</p>
            </div>
          </div>
        </div>
        
        {/* Booking Form */}
        <div className="bg-white rounded-lg shadow-md p-6 lg:w-96 h-fit sticky top-8">
          <h3 className="text-xl font-semibold mb-4 flex justify-between">
            <span>${property.pricePerNight} <span className="text-gray-500 text-sm font-normal">/ night</span></span>
            <div className="flex items-center">
              <span className="text-yellow-500 text-sm mr-1">★</span>
              <span className="text-gray-700 text-sm">{property.rating}</span>
            </div>
          </h3>
          
          <form onSubmit={handleSubmit} className="mt-4">
            <div className="mb-4 grid grid-cols-2 gap-4">
              <div>
                <label className="block text-gray-700 text-sm font-medium mb-1">Check-in</label>
                <input
                  type="date"
                  name="checkIn"
                  value={booking.checkIn}
                  onChange={handleBookingChange}
                  className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>
              <div>
                <label className="block text-gray-700 text-sm font-medium mb-1">Check-out</label>
                <input
                  type="date"
                  name="checkOut"
                  value={booking.checkOut}
                  onChange={handleBookingChange}
                  className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>
            </div>
            
            <div className="mb-6">
              <label className="block text-gray-700 text-sm font-medium mb-1">Guests</label>
              <select
                name="guests"
                value={booking.guests}
                onChange={handleBookingChange}
                className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                required
              >
                {[...Array(property.maxGuests)].map((_, i) => (
                  <option key={i} value={i + 1}>{i + 1} {i === 0 ? 'guest' : 'guests'}</option>
                ))}
              </select>
            </div>
            
            {bookingError && (
              <div className="mb-4 text-red-500 text-sm">{bookingError}</div>
            )}
            
            <div className="border-t border-gray-300 pt-4 mb-4">
              <div className="flex justify-between mb-2">
                <span className="text-gray-700">${property.pricePerNight} x {calculateTotalPrice() / property.pricePerNight} nights</span>
                <span>${calculateTotalPrice()}</span>
              </div>
              <div className="flex justify-between mb-2">
                <span className="text-gray-700">Cleaning fee</span>
                <span>$50</span>
              </div>
              <div className="flex justify-between mb-2">
                <span className="text-gray-700">Service fee</span>
                <span>$30</span>
              </div>
              <div className="flex justify-between font-bold mt-4 pt-4 border-t border-gray-300">
                <span>Total</span>
                <span>${calculateTotalPrice() + 50 + 30}</span>
              </div>
            </div>
            
            <button
              type="submit"
              className={`w-full bg-blue-600 hover:bg-blue-700 text-white py-3 rounded-md transition font-semibold ${isSubmitting ? 'opacity-70 cursor-not-allowed' : ''}`}
              disabled={isSubmitting}
            >
              {isSubmitting ? 'Processing...' : 'Reserve'}
            </button>
          </form>
          
          <p className="text-center text-gray-500 text-sm mt-4">You won't be charged yet</p>
        </div>
      </div>
    </div>
  );
};

export default PropertyDetail;