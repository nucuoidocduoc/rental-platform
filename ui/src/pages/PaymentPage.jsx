import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getPaymentDetails, processPayment } from '../services/api';

const PaymentPage = ({ bookingDetails, onPaymentInfo }) => {
  const navigate = useNavigate();
  const [paymentDetails, setPaymentDetails] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  const [paymentMethod, setPaymentMethod] = useState('credit_card');
  const [cardDetails, setCardDetails] = useState({
    cardNumber: '',
    cardHolder: '',
    expiryDate: '',
    cvv: '',
  });
  
  const [isProcessing, setIsProcessing] = useState(false);
  const [paymentError, setPaymentError] = useState(null);

  useEffect(() => {
    const fetchPaymentDetails = async () => {
      try {
        if (!bookingDetails || !bookingDetails.bookingId) {
          throw new Error('No booking information available');
        }
        
        setLoading(true);
        const data = await getPaymentDetails(bookingDetails.bookingId);
        setPaymentDetails(data);
        setLoading(false);
      } catch (err) {
        setError('Failed to fetch payment details. Please try again.');
        setLoading(false);
      }
    };

    fetchPaymentDetails();
  }, [bookingDetails]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setCardDetails({
      ...cardDetails,
      [name]: value
    });
  };

  const handlePaymentMethodChange = (method) => {
    setPaymentMethod(method);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setPaymentError(null);
    setIsProcessing(true);
    
    // Simple validation
    if (paymentMethod === 'credit_card') {
      if (!cardDetails.cardNumber || !cardDetails.cardHolder || !cardDetails.expiryDate || !cardDetails.cvv) {
        setPaymentError('Please fill in all card details');
        setIsProcessing(false);
        return;
      }
      
      if (cardDetails.cardNumber.replace(/\s/g, '').length !== 16) {
        setPaymentError('Card number should be 16 digits');
        setIsProcessing(false);
        return;
      }
      
      if (cardDetails.cvv.length < 3) {
        setPaymentError('CVV should be at least 3 digits');
        setIsProcessing(false);
        return;
      }
    }

    try {
      const paymentData = {
        bookingId: bookingDetails.bookingId,
        paymentMethod,
        amount: paymentDetails.totalAmount,
        currency: paymentDetails.currency,
        ...(paymentMethod === 'credit_card' && { cardDetails }),
      };
      
      const response = await processPayment(paymentData);
      
      // If payment is successful, pass the payment data and navigate to thank you page
      onPaymentInfo(response);
      navigate('/thank-you');
    } catch (err) {
      setPaymentError('Payment processing failed. Please try again.');
      setIsProcessing(false);
    }
  };

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-12 flex justify-center items-center">
        <div className="text-center">
          <div className="w-16 h-16 border-4 border-blue-600 border-t-transparent rounded-full animate-spin mx-auto mb-4"></div>
          <p className="text-xl">Loading payment details...</p>
        </div>
      </div>
    );
  }

  if (error || !paymentDetails) {
    return (
      <div className="container mx-auto px-4 py-12 text-center">
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded max-w-md mx-auto">
          <p>{error || 'Payment details not found'}</p>
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
      <div className="max-w-3xl mx-auto">
        <h1 className="text-3xl font-bold mb-6">Payment</h1>
        
        <div className="bg-white rounded-lg shadow-md overflow-hidden">
          {/* Order Summary */}
          <div className="bg-gray-50 p-6 border-b">
            <h2 className="text-xl font-semibold mb-4">Order Summary</h2>
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
            <div className="flex justify-between font-bold mt-4 pt-4 border-t border-gray-300">
              <span>Total Amount:</span>
              <span>{paymentDetails.currency} {paymentDetails.totalAmount.toFixed(2)}</span>
            </div>
          </div>
          
          {/* Payment Methods */}
          <form onSubmit={handleSubmit} className="p-6">
            <h2 className="text-xl font-semibold mb-4">Payment Method</h2>
            
            <div className="flex flex-wrap gap-4 mb-6">
              <div 
                className={`border rounded-lg p-4 flex items-center cursor-pointer ${paymentMethod === 'credit_card' ? 'border-blue-500 bg-blue-50' : 'border-gray-300'}`}
                onClick={() => handlePaymentMethodChange('credit_card')}
              >
                <div className="w-12 h-8 bg-blue-600 rounded mr-3 flex items-center justify-center text-white">
                  <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                    <path d="M4 4a2 2 0 00-2 2v1h16V6a2 2 0 00-2-2H4z" />
                    <path fillRule="evenodd" d="M18 9H2v5a2 2 0 002 2h12a2 2 0 002-2V9zM4 13a1 1 0 011-1h1a1 1 0 110 2H5a1 1 0 01-1-1zm5-1a1 1 0 100 2h1a1 1 0 100-2H9z" clipRule="evenodd" />
                  </svg>
                </div>
                <span className="font-medium">Credit Card</span>
              </div>
              
              <div 
                className={`border rounded-lg p-4 flex items-center cursor-pointer ${paymentMethod === 'paypal' ? 'border-blue-500 bg-blue-50' : 'border-gray-300'}`}
                onClick={() => handlePaymentMethodChange('paypal')}
              >
                <div className="w-12 h-8 bg-blue-800 rounded mr-3 flex items-center justify-center text-white">
                  <span className="font-bold">PayPal</span>
                </div>
                <span className="font-medium">PayPal</span>
              </div>
            </div>
            
            {/* Credit Card Form */}
            {paymentMethod === 'credit_card' && (
              <div className="mb-6">
                <div className="mb-4">
                  <label className="block text-gray-700 text-sm font-medium mb-2" htmlFor="cardNumber">
                    Card Number
                  </label>
                  <input
                    type="text"
                    id="cardNumber"
                    name="cardNumber"
                    placeholder="1234 5678 9012 3456"
                    value={cardDetails.cardNumber}
                    onChange={handleInputChange}
                    maxLength="19"
                    className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                    required
                  />
                </div>
                
                <div className="mb-4">
                  <label className="block text-gray-700 text-sm font-medium mb-2" htmlFor="cardHolder">
                    Card Holder Name
                  </label>
                  <input
                    type="text"
                    id="cardHolder"
                    name="cardHolder"
                    placeholder="John Doe"
                    value={cardDetails.cardHolder}
                    onChange={handleInputChange}
                    className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                    required
                  />
                </div>
                
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label className="block text-gray-700 text-sm font-medium mb-2" htmlFor="expiryDate">
                      Expiry Date
                    </label>
                    <input
                      type="text"
                      id="expiryDate"
                      name="expiryDate"
                      placeholder="MM/YY"
                      value={cardDetails.expiryDate}
                      onChange={handleInputChange}
                      className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                      required
                    />
                  </div>
                  
                  <div>
                    <label className="block text-gray-700 text-sm font-medium mb-2" htmlFor="cvv">
                      CVV
                    </label>
                    <input
                      type="text"
                      id="cvv"
                      name="cvv"
                      placeholder="123"
                      value={cardDetails.cvv}
                      onChange={handleInputChange}
                      className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                      required
                    />
                  </div>
                </div>
              </div>
            )}
            
            {/* PayPal Form */}
            {paymentMethod === 'paypal' && (
              <div className="mb-6">
                <p className="text-gray-700 mb-4">You will be redirected to PayPal to complete your payment.</p>
                <div className="bg-gray-100 p-4 rounded border border-gray-300">
                  <p className="text-sm text-gray-600">For this demo, we'll simulate a PayPal payment. In a real implementation, you would be redirected to PayPal's payment interface.</p>
                </div>
              </div>
            )}
            
            {paymentError && (
              <div className="mb-4 text-red-500 text-sm">{paymentError}</div>
            )}
            
            <div className="mt-6">
              <button
                type="submit"
                className={`w-full bg-blue-600 hover:bg-blue-700 text-white py-3 rounded-md transition font-semibold ${isProcessing ? 'opacity-70 cursor-not-allowed' : ''}`}
                disabled={isProcessing}
              >
                {isProcessing ? 'Processing...' : `Pay ${paymentDetails.currency} ${paymentDetails.totalAmount.toFixed(2)}`}
              </button>
            </div>
          </form>
          
          <div className="p-4 bg-gray-50 border-t text-center text-sm text-gray-600">
            Your payment information is securely processed.
          </div>
        </div>
        
        <div className="mt-6 text-center">
          <button 
            onClick={() => navigate(-1)} 
            className="text-blue-600 hover:text-blue-800 font-medium"
          >
            Back to Booking
          </button>
        </div>
      </div>
    </div>
  );
};

export default PaymentPage;