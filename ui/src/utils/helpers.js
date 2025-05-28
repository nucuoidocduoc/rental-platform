// Helper functions for the property booking application

/**
 * Formats a date object to YYYY-MM-DD format for input fields
 * @param {Date} date - The date to format
 * @returns {string} Formatted date string in YYYY-MM-DD format
 */
export const formatDate = (date) => {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  
  return `${year}-${month}-${day}`;
};

/**
 * Calculates the number of nights between two dates
 * @param {string} checkIn - Check-in date in YYYY-MM-DD format
 * @param {string} checkOut - Check-out date in YYYY-MM-DD format
 * @returns {number} Number of nights between the dates
 */
export const calculateNights = (checkIn, checkOut) => {
  const checkInDate = new Date(checkIn);
  const checkOutDate = new Date(checkOut);
  const timeDifference = checkOutDate.getTime() - checkInDate.getTime();
  const nights = Math.ceil(timeDifference / (1000 * 3600 * 24));
  
  return nights > 0 ? nights : 0;
};

/**
 * Formats a price number to a currency string
 * @param {number} price - The price to format
 * @param {string} currency - Currency code (default: USD)
 * @returns {string} Formatted price string
 */
export const formatPrice = (price, currency = 'USD') => {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency,
  }).format(price);
};

/**
 * Truncates a text to a specific length and adds ellipsis if truncated
 * @param {string} text - Text to truncate
 * @param {number} maxLength - Maximum length before truncation
 * @returns {string} Truncated text
 */
export const truncateText = (text, maxLength) => {
  if (text.length <= maxLength) return text;
  return text.substring(0, maxLength) + '...';
};

/**
 * Generates a star rating component based on a rating number
 * @param {number} rating - The rating value (1-5)
 * @returns {string} HTML string for star rating
 */
export const generateStarRating = (rating) => {
  const fullStars = Math.floor(rating);
  const halfStar = rating % 1 >= 0.5;
  const emptyStars = 5 - fullStars - (halfStar ? 1 : 0);
  
  let stars = '';
  
  // Full stars
  for (let i = 0; i < fullStars; i++) {
    stars += '★';
  }
  
  // Half star if applicable
  if (halfStar) {
    stars += '✫';
  }
  
  // Empty stars
  for (let i = 0; i < emptyStars; i++) {
    stars += '☆';
  }
  
  return stars;
};

/**
 * Validates an email address format
 * @param {string} email - Email address to validate
 * @returns {boolean} True if valid, false otherwise
 */
export const validateEmail = (email) => {
  const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  return re.test(String(email).toLowerCase());
};

/**
 * Parses query parameters from URL
 * @param {string} queryString - URL query string
 * @returns {Object} Object with parsed parameters
 */
export const parseQueryParams = (queryString) => {
  const params = {};
  const searchParams = new URLSearchParams(queryString);
  
  for (const [key, value] of searchParams) {
    params[key] = value;
  }
  
  return params;
};

/**
 * Debounces a function call
 * @param {Function} func - Function to debounce
 * @param {number} wait - Milliseconds to wait
 * @returns {Function} Debounced function
 */
export const debounce = (func, wait) => {
  let timeout;
  
  return function executedFunction(...args) {
    const later = () => {
      clearTimeout(timeout);
      func(...args);
    };
    
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
  };
};