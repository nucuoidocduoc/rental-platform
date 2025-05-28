import React, { useState } from 'react';
import { Link } from 'react-router-dom';

const HomePage = () => {
  const [searchQuery, setSearchQuery] = useState('');
  
  const handleSearch = (e) => {
    e.preventDefault();
    // In a real app, this would trigger a search with parameters
    window.location.href = `/properties?search=${searchQuery}`;
  };

  return (
    <div className="flex flex-col">
      {/* Hero Section */}
      <div 
        className="relative h-[600px] bg-cover bg-center flex items-center"
        style={{ backgroundImage: "url('/assets/images/hero-image.jpg')" }}
      >
        <div className="absolute inset-0 bg-black bg-opacity-40"></div>
        
        <div className="container mx-auto px-4 relative z-10">
          <div className="max-w-2xl text-white">
            <h1 className="text-4xl md:text-5xl font-bold mb-4">
              Find Your Perfect Vacation Rental
            </h1>
            <p className="text-xl mb-8">
              Discover amazing properties for your next getaway. Book with confidence and enjoy your stay.
            </p>
            
            {/* Search Form */}
            <form 
              onSubmit={handleSearch}
              className="bg-white p-4 rounded-lg shadow-lg flex flex-col md:flex-row"
            >
              <input 
                type="text" 
                placeholder="Where do you want to go?" 
                className="flex-grow px-4 py-3 rounded-md border focus:outline-none focus:ring-2 focus:ring-blue-500 mb-3 md:mb-0 md:mr-2"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
              <button 
                type="submit"
                className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-md transition font-semibold"
              >
                Search
              </button>
            </form>
          </div>
        </div>
      </div>
      
      {/* Featured Properties Section */}
      <div className="container mx-auto px-4 py-12">
        <h2 className="text-3xl font-bold mb-6 text-center">Featured Properties</h2>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {/* Featured Property Cards - Just placeholders */}
          {[1, 2, 3].map((item) => (
            <Link 
              to="/properties" 
              key={item} 
              className="bg-white rounded-lg overflow-hidden shadow-lg transition-transform hover:scale-[1.02]"
            >
              <div className="h-48 bg-gray-300"></div>
              <div className="p-5">
                <h3 className="font-bold text-xl mb-2">Beautiful Property {item}</h3>
                <div className="flex items-center mb-2">
                  <span className="text-yellow-500 mr-1">
                    ★★★★★
                  </span>
                  <span className="text-gray-600 text-sm">(32 reviews)</span>
                </div>
                <p className="text-gray-700 mb-3">Experience luxury and comfort in this amazing property...</p>
                <div className="flex justify-between items-center">
                  <span className="text-blue-600 font-bold">$199/night</span>
                  <span className="text-gray-500">View Details →</span>
                </div>
              </div>
            </Link>
          ))}
        </div>
        <div className="text-center mt-8">
          <Link to="/properties" className="inline-block bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-md transition font-semibold">
            View All Properties
          </Link>
        </div>
      </div>
      
      {/* How It Works Section */}
      <div className="bg-gray-50 py-12">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-bold mb-10 text-center">How It Works</h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            <div className="flex flex-col items-center text-center">
              <div className="w-16 h-16 flex items-center justify-center bg-blue-100 text-blue-600 rounded-full mb-4">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                </svg>
              </div>
              <h3 className="text-xl font-bold mb-2">Search</h3>
              <p className="text-gray-600">Find your ideal property by location, amenities, or property type.</p>
            </div>
            
            <div className="flex flex-col items-center text-center">
              <div className="w-16 h-16 flex items-center justify-center bg-blue-100 text-blue-600 rounded-full mb-4">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                </svg>
              </div>
              <h3 className="text-xl font-bold mb-2">Book</h3>
              <p className="text-gray-600">Reserve your stay with our simple booking process.</p>
            </div>
            
            <div className="flex flex-col items-center text-center">
              <div className="w-16 h-16 flex items-center justify-center bg-blue-100 text-blue-600 rounded-full mb-4">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
                </svg>
              </div>
              <h3 className="text-xl font-bold mb-2">Enjoy</h3>
              <p className="text-gray-600">Relax and enjoy your perfect vacation rental.</p>
            </div>
          </div>
        </div>
      </div>
      
      {/* Testimonials Section */}
      <div className="container mx-auto px-4 py-12">
        <h2 className="text-3xl font-bold mb-10 text-center">What Our Guests Say</h2>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
          {/* Testimonial Cards */}
          {[
            {
              name: "Sarah Johnson",
              location: "New York, USA",
              text: "We had an amazing stay! The property was exactly as described, and the host was incredibly helpful throughout our vacation."
            },
            {
              name: "Mark Thompson",
              location: "London, UK",
              text: "PropertyBooker made finding our dream vacation rental so easy. The booking process was smooth, and we got exactly what we wanted."
            },
            {
              name: "Elena Rodriguez",
              location: "Barcelona, Spain",
              text: "Fantastic experience from start to finish. The property exceeded our expectations, and we'll definitely book through PropertyBooker again!"
            }
          ].map((testimonial, index) => (
            <div key={index} className="bg-white p-6 rounded-lg shadow-md">
              <div className="flex items-center mb-4">
                <div className="w-12 h-12 rounded-full bg-gray-300 mr-4"></div>
                <div>
                  <h4 className="font-bold">{testimonial.name}</h4>
                  <p className="text-gray-600 text-sm">{testimonial.location}</p>
                </div>
              </div>
              <div className="text-yellow-500 mb-2">★★★★★</div>
              <p className="text-gray-700">"{testimonial.text}"</p>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default HomePage;