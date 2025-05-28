import { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import HomePage from './pages/HomePage';
import ListingPage from './pages/ListingPage';
import PropertyDetail from './pages/PropertyDetail';
import PaymentPage from './pages/PaymentPage';
import ThankYouPage from './pages/ThankYouPage';

function App() {
  const [bookingDetails, setBookingDetails] = useState(null);
  const [paymentDetails, setPaymentDetails] = useState(null);

  // Function to handle successful booking and pass data to payment page
  const handleBookingSuccess = (data) => {
    setBookingDetails(data);
  };

  // Function to handle payment information
  const handlePaymentInfo = (data) => {
    setPaymentDetails(data);
  };

  return (
    <Router>
      <div className="flex flex-col min-h-screen">
        <Navbar />
        <main className="flex-grow">
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/properties" element={<ListingPage />} />
            <Route 
              path="/property/:id" 
              element={<PropertyDetail onBookingSuccess={handleBookingSuccess} />} 
            />
            <Route 
              path="/payment" 
              element={
                bookingDetails ? 
                <PaymentPage 
                  bookingDetails={bookingDetails} 
                  onPaymentInfo={handlePaymentInfo} 
                /> : 
                <Navigate to="/properties" />
              } 
            />
            <Route 
              path="/thank-you" 
              element={
                paymentDetails ? 
                <ThankYouPage bookingDetails={bookingDetails} paymentDetails={paymentDetails} /> : 
                <Navigate to="/properties" />
              } 
            />
            <Route path="*" element={<Navigate to="/" />} />
          </Routes>
        </main>
        <Footer />
      </div>
    </Router>
  );
}

export default App;