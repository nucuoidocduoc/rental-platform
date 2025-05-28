import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { fetchProperties } from '../services/api';

const ListingPage = () => {
  const [properties, setProperties] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [filters, setFilters] = useState({
    priceMin: '',
    priceMax: '',
    bedrooms: '',
    propertyType: ''
  });

  useEffect(() => {
    const getProperties = async () => {
      try {
        setLoading(true);
        const data = await fetchProperties();
        setProperties(data);
        setLoading(false);
      } catch (err) {
        setError('Failed to load properties. Please try again later.');
        setLoading(false);
      }
    };

    getProperties();
  }, []);

  const handleFilterChange = (e) => {
    const { name, value } = e.target;
    setFilters({
      ...filters,
      [name]: value
    });
  };

  const applyFilters = (properties) => {
    return properties.filter(property => {
      // Filter by price range
      if (filters.priceMin && property.pricePerNight < Number(filters.priceMin)) {
        return false;
      }
      if (filters.priceMax && property.pricePerNight > Number(filters.priceMax)) {
        return false;
      }
      // Filter by number of bedrooms
      if (filters.bedrooms && property.bedrooms !== Number(filters.bedrooms)) {
        return false;
      }
      // Filter by property type
      if (filters.propertyType && property.type !== filters.propertyType) {
        return false;
      }
      return true;
    });
  };

  const filteredProperties = filters ? applyFilters(properties) : properties;

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-12 flex justify-center items-center">
        <div className="text-center">
          <div className="w-16 h-16 border-4 border-blue-600 border-t-transparent rounded-full animate-spin mx-auto mb-4"></div>
          <p className="text-xl">Loading properties...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container mx-auto px-4 py-12 text-center">
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded max-w-md mx-auto">
          <p>{error}</p>
          <button 
            onClick={() => window.location.reload()} 
            className="mt-2 bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700 transition"
          >
            Try Again
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-6">Available Properties</h1>
      
      {/* Filters Section */}
      <div className="bg-white shadow-md rounded-lg p-4 mb-6">
        <h2 className="text-xl font-semibold mb-4">Filters</h2>
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div>
            <label className="block text-gray-700 text-sm font-medium mb-1">Min Price</label>
            <input
              type="number"
              name="priceMin"
              placeholder="Min Price"
              value={filters.priceMin}
              onChange={handleFilterChange}
              className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-medium mb-1">Max Price</label>
            <input
              type="number"
              name="priceMax"
              placeholder="Max Price"
              value={filters.priceMax}
              onChange={handleFilterChange}
              className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-medium mb-1">Bedrooms</label>
            <select
              name="bedrooms"
              value={filters.bedrooms}
              onChange={handleFilterChange}
              className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <option value="">Any</option>
              <option value="1">1</option>
              <option value="2">2</option>
              <option value="3">3</option>
              <option value="4">4+</option>
            </select>
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-medium mb-1">Property Type</label>
            <select
              name="propertyType"
              value={filters.propertyType}
              onChange={handleFilterChange}
              className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <option value="">Any</option>
              <option value="apartment">Apartment</option>
              <option value="house">House</option>
              <option value="villa">Villa</option>
              <option value="cabin">Cabin</option>
            </select>
          </div>
        </div>
      </div>

      {/* Results Count */}
      <p className="mb-4 text-gray-600">Showing {filteredProperties.length} properties</p>
      
      {/* Properties Grid */}
      {filteredProperties.length === 0 ? (
        <div className="text-center py-8">
          <h3 className="text-xl font-medium text-gray-800">No properties match your filters</h3>
          <p className="text-gray-600 mt-2">Try adjusting your search criteria</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredProperties.map(property => (
            <Link 
              to={`/property/${property.id}`} 
              key={property.id}
              className="bg-white rounded-lg overflow-hidden shadow-md hover:shadow-lg transition"
            >
              <div 
                className="h-48 bg-gray-300 bg-cover bg-center" 
                style={{ backgroundImage: `url(${property.images[0]})` }}
              ></div>
              <div className="p-4">
                <h3 className="font-bold text-lg mb-2 text-gray-800">{property.name}</h3>
                <p className="text-gray-600 mb-2">{property.location}</p>
                <div className="flex items-center mb-2">
                  <span className="text-yellow-500 mr-1">★</span>
                  <span className="font-medium">{property.rating}</span>
                  <span className="text-gray-500 text-sm ml-1">({property.reviewCount} reviews)</span>
                </div>
                <div className="flex justify-between items-center mt-3">
                  <span className="font-bold text-lg text-blue-600">${property.pricePerNight}<span className="text-sm text-gray-600">/night</span></span>
                  <span className="text-gray-600 hover:text-blue-600 transition">View Details →</span>
                </div>
              </div>
            </Link>
          ))}
        </div>
      )}
    </div>
  );
};

export default ListingPage;